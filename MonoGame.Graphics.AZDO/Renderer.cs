using System;
using OpenTK.Graphics.OpenGL;

namespace MonoGame.Graphics.AZDO
{
	public class Renderer
	{
		private IBlendCapabilities mBlend;
		private IDepthStencilCapabilities mDepthStencil;
		public Renderer (IBlendCapabilities blend, IDepthStencilCapabilities depth)
		{
			mBlend = blend;
			mDepthStencil = depth;
			SetDefault ();
		}

		public class Marker
		{

		}

		public class Resource
		{

		}

		Mesh FindMesh ()
		{
			throw new NotImplementedException ();
		}

		public class Effect
		{
			public EffectPass[] Passes { get; set; }
		}

		public class MeshEffect
		{
			public int PassNumber { get; set;}
			public Effect Technique { get; set;}
			public Mesh Part { get; set; }
			public int Options { get; set; }
		}

		public void RegisterModelNode(MeshEffect[] nodes)
		{
			foreach (var entry in nodes)
			{
				var pass = entry.Technique.Passes [entry.PassNumber];
				RegisterMesh (entry.Part, pass, entry.Options);
			}
		}

		public class MeshMarkerCollection
		{
			public void Add(object item)
			{

			}

			public bool Exists(Mesh m, out Marker found)
			{
				throw new NotImplementedException ();
			}
		}

		public class Buffer
		{

		}

		public class Criteria
		{

		}

		public MeshMarkerCollection markers;
		public IConstantBufferCollection buffers;

		public void RegisterMesh(Mesh mesh, EffectPass pass, int options)
		{
			var vertexFormat = mesh.VertexFormat;

			Marker found;
			if (markers.Exists (mesh, out found))
			{

			}
			else
			{

			}

			var programId = pass.Match (vertexFormat, options);

			var id = mesh.GetBlockId();

			var candidates = buffers.Filter (mesh, pass, options);
			if (candidates.Length == 0)
			{
				buffers.Add (null);
			}

		}


		public DrawItem mPreviousItem;
		private DrawItemBitFlags mPreviousColorMask;

		private void ApplyBlendValues (DrawItem from, DrawItem proposedState)
		{
			var pastBlend = from.BlendValues;
			var nextBlend = proposedState.BlendValues;

			var blendEnabled = !(nextBlend.ColorSourceBlend == Blend.One && 
				nextBlend.ColorDestinationBlend == Blend.Zero &&
				nextBlend.AlphaSourceBlend == Blend.One &&
				nextBlend.AlphaDestinationBlend == Blend.Zero);

			if (blendEnabled != mBlend.IsEnabled)
			{
				mBlend.EnableBlending (blendEnabled);
			}

			if (nextBlend.ColorSourceBlend != pastBlend.ColorSourceBlend ||
				nextBlend.ColorDestinationBlend != pastBlend.ColorDestinationBlend ||
				nextBlend.AlphaSourceBlend != pastBlend.AlphaSourceBlend ||
				nextBlend.AlphaDestinationBlend != pastBlend.AlphaDestinationBlend)
			{
				mBlend.ApplyBlendSeparateFunction (
					nextBlend.ColorSourceBlend,
					nextBlend.ColorDestinationBlend,
					nextBlend.AlphaSourceBlend,
					nextBlend.AlphaDestinationBlend);
			}

			var writeMask = DrawItemBitFlags.RedColorWriteChannel
				| DrawItemBitFlags.GreenColorWriteChannel
				| DrawItemBitFlags.BlueColorWriteChannel
				| DrawItemBitFlags.AlphaColorWriteChannel; 
			
			var colorWriteMask = writeMask & proposedState.Flags;

			if ( (mPreviousColorMask & colorWriteMask) == 0)
			{
				mBlend.SetColorMask (colorWriteMask);
			}
		}

		private void ApplyDepthStencilValues (DrawItem previous, DrawItem next)
		{
			var enabled = (next.Flags & DrawItemBitFlags.DepthBufferEnabled) != 0;

			if (mDepthStencil.IsDepthBufferEnabled != enabled)
			{
				if (!mDepthStencil.IsDepthBufferEnabled)
				{
					mDepthStencil.DisableDepthBuffer ();
				}
				else
				{
					// enable Depth Buffer
					mDepthStencil.EnableDepthBuffer ();
				}
			}

			var pastDepth = previous.DepthStencilValues;
			var nextDepth = next.DepthStencilValues;

			if (pastDepth.DepthBufferFunction != nextDepth.DepthBufferFunction)
			{
				mDepthStencil.SetDepthBufferFunc (nextDepth.DepthBufferFunction);
			}

			var oldDepthWrite = (previous.Flags & DrawItemBitFlags.DepthBufferWriteEnabled);
			var newDepthWrite = (next.Flags & DrawItemBitFlags.DepthBufferWriteEnabled);

			if ((oldDepthWrite & newDepthWrite) == 0)
			{
				mDepthStencil.SetDepthMask(newDepthWrite != 0);
			}

			var newStencilEnabled = (next.Flags & DrawItemBitFlags.StencilEnabled);

			if (mDepthStencil.IsStencilBufferEnabled != ( newStencilEnabled != 0))
			{
				if (!mDepthStencil.IsStencilBufferEnabled)
				{
					mDepthStencil.DisableStencilBuffer ();
				}
				else
				{
					mDepthStencil.EnableStencilBuffer ();
				}
			}

			if (pastDepth.StencilWriteMask != nextDepth.StencilWriteMask)
			{
				mDepthStencil.SetStencilWriteMask (nextDepth.StencilWriteMask);
			}
		}

		public void SetDefault()
		{
			mBlend.Initialise ();
			mDepthStencil.Initialise ();
		}

		public void Render(DrawItem[] items)
		{
			var currentState = mPreviousItem;
			foreach (var proposedState in items)
			{
				if (!(proposedState.BlendValues.Equals(currentState.BlendValues)))
				{
					ApplyBlendValues (currentState, proposedState);
				}

				if (!(proposedState.DepthStencilValues.Equals(currentState.DepthStencilValues)))
				{
					ApplyDepthStencilValues (currentState, proposedState);
				}

				currentState = proposedState;
			}
		}
	}
}

