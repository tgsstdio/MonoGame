using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using BirdNest.MonoGame.Blocks;
using BirdNest.MonoGame.Graphics;

namespace BirdNest.MonoGame
{
	public class SparseTextureAtlas : ITextureAtlas
	{
		public SparseTextureAtlas (ITextureChapterAllocator allocator)
		{
			mChapterAllocator = allocator;
		}

		#region ITextureAtlas implementation

		private readonly ITextureChapterAllocator mChapterAllocator;
		// TODO : find more specific enum
		/// <summary>
		/// 3 tuples
		/// 	1 Image type
		/// 	2 Dimensions
		/// 	3 Image Parmeters
		/// value type
		/// 	to handle overflow
		/// </summary>
		private Dictionary<Tuple<Tuple<int, int, int, int, int, int>,Tuple<int, int, int>, Tuple<int, int, int, int>>, ConcurrentStack<ITextureChapter>> mChapters;
		public void Initialize ()
		{
			mChapters = new Dictionary<Tuple<Tuple<int, int, int, int, int, int>, Tuple<int, int, int>, Tuple<int, int, int, int>>, ConcurrentStack<ITextureChapter>> ();
		}

		public void Clear ()
		{
			foreach (var chapter in mChapters.Values)
			{
				chapter.Clear ();
			}
		}

		static Tuple<int, int, int, int, int, int> ExtractImageTypeKey (AtlasTextureType info)
		{
			return new Tuple<int, int, int, int, int, int> (info.GlType, info.GlTypeSize, info.GlFormat, info.GlInternalFormat, info.GlBaseInternalFormat, info.NoOfMipmapLevels);
		}

		static Tuple<int, int, int> ExtractDimensionsKey (ImageDimensions info)
		{
			return new Tuple<int, int, int> (info.Width, info.Height, info.Depth);
		}

		static Tuple<int, int, int, int> ExtractParametersKey (TextureCatalog info)
		{
			return new Tuple<int, int, int, int> (info.MagFilter, info.MinFilter, info.TextureWrapS, info.TextureWrapT);
		}

		static Tuple<Tuple<int, int, int, int, int, int>,Tuple<int, int, int>, Tuple<int, int, int, int>> ExtractKey(TextureCatalog info, AtlasTextureType typeKey, ImageDimensions dims)
		{
			return new Tuple<Tuple<int, int, int, int, int, int>,Tuple<int, int, int>, Tuple<int, int, int, int>>(
				ExtractImageTypeKey(typeKey),
				ExtractDimensionsKey (dims),
				ExtractParametersKey(info));
		}

		public ITextureChapter Add (TextureCatalog catalog, AtlasTextureType imageType, AtlasTextureTarget glTarget, ImageDimensions dims)
		{
			var key = ExtractKey(catalog, imageType, dims);

			ConcurrentStack<ITextureChapter> dest = null;
			if (mChapters.TryGetValue (key, out dest))
			{
				ITextureChapter top = null;
				if (dest.TryPeek (out top))
				{
					if (top.IsFull ())
					{						
						var next = mChapterAllocator.Generate (catalog, imageType, dims, glTarget);
						dest.Push (next);
						return next;
					}
					else
					{
						return top;
					}
				}
				else
				{
					// after cleared -- MAYBE
					throw new InvalidOperationException();		
				}
			}
			else 	
			{
				// create stack
				dest = new ConcurrentStack<ITextureChapter> ();
				mChapters.Add (key, dest);

				var next = mChapterAllocator.Generate(catalog, imageType, dims, glTarget);
				dest.Push (next);
				return next;
			}
		}

		#endregion
	}
}

