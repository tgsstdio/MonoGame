using System;
using KtxSharp;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using KtxSharp.JsonNET;
using BirdNest.Core;

namespace KTXPacker
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			using (var game = new GameWindow ())
			{
				Console.WriteLine ("Hello World!");

				var extensionChecker = new GLExtensionChecker ();
				extensionChecker.Initialize ();

				const string hdrExtension = "GL_KHR_texture_compression_astc_hdr";
				Console.WriteLine (hdrExtension + " : " + extensionChecker.HasExtension (hdrExtension));

				const string ldrExtension = "GL_KHR_texture_compression_astc_ldr";
				Console.WriteLine (ldrExtension + " : " + extensionChecker.HasExtension (ldrExtension));

				foreach (var str in extensionChecker.Extensions)
				{
					Console.WriteLine (str);
				}


				IAssetManager am = new AssetManager ();

				var jsNetSerializer = new JsonNETSerializer ();
				var pgLookup = new TexturePageLookup ();
				var locator = new AssetLocator (jsNetSerializer, pgLookup);

				var fs = new DirectoryFileSystem (locator);
				fs.Initialise ("media");
				fs.Register (new BlockIdentifier{Value=10000});

				var unpacker = new IgnoreETCUnpacking ();
				var texLookup = new SparseTextureLookup (am);
				var pgAllocator = new SparseTexturePageAllocator (texLookup);
				var thLookup = new BindlessHandleLookup (am);
				var onePgAllocator = new SinglePageAllocator (thLookup);
				var cmPgAllocator = new SparseCubeMapPageAllocator (texLookup);
				var chAllocator = new SparseTextureChapterAllocator (texLookup, pgAllocator, onePgAllocator, cmPgAllocator);
				var atlas = new SparseTextureAtlas (chAllocator);
				atlas.Initialize ();
				var loader = new KTXTextureManager (am, fs, pgLookup, unpacker, atlas);

				loader.Load (new AssetIdentifier{Value=001});

				game.Load += (sender, e) =>
				{
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
				};

				game.Unload += (sender, e) => 
				{
				};

				game.KeyDown += (object sender, KeyboardKeyEventArgs e) => 
				{
					if (e.Key == Key.Space)
					{
						game.Exit();
					}
				};

				game.UpdateFrame += (sender, e) =>
				{

				};				

				game.RenderFrame += (sender, e) =>
				{
					GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					game.SwapBuffers();
				};

				game.Resize += (sender, e) =>
				{
					GL.Viewport(0, 0, game.Width, game.Height);
				};

				game.Run(60.0);
			}
		}
	}
}
