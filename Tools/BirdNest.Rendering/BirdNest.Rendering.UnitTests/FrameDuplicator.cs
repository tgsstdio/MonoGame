using System.Collections.Generic;
using BirdNest.MonoGame.Graphics;
using MonoGame.Content;

namespace BirdNest.Rendering.UnitTests
{
	public class FrameDuplicator<TLightInfo, TCameraInfo> : IFrameDuplicator
		where TLightInfo : IViewer
		where TCameraInfo : IViewer
	{
		#region IFrameSet implementation

		public Frame[] GetFrames ()
		{
			var result = new List<Frame> ();
			result.AddRange (mDepthMapFrames.Frames.Values);
			result.AddRange (mVisibleSet.Frames.Values);
			return result.ToArray ();
		}

		static Frame CreateNewFrame (IRenderTarget target, IShaderProgram program, IViewer viewer)
		{
			var result = new Frame ();
			result.Viewer = viewer;
			result.Program = program;
			result.Target = target;
			return result;
		}

		static LayerSetKey GenerateKey (IRenderTarget target, IShaderProgram program, IViewer viewer)
		{
			var key = new LayerSetKey ();
			key.TargetId = target.Id;
			key.ShaderId = program.Id;
			key.ViewerId = viewer.Id;
			return key;
		}

		public void Add (LayerType layer, IRenderTarget target, IShaderProgram program, OptimizedPass values)
		{
			if (layer == LayerType.DepthMap)
			{
				foreach (var light in mLights)
				{
					var key = GenerateKey (target, program, light);

					Frame result;
					if (!mDepthMapFrames.Frames.TryGetValue (key, out result))
					{
						result = CreateNewFrame (target, program, light);
						mDepthMapFrames.Frames.Add (key, result);
					}
					result.Passes.Add (values);
				}
			} else if (layer == LayerType.Visible)
			{
				foreach (var camera in mCameras)
				{
					var key = GenerateKey (target, program, camera);

					Frame result;
					if (!mVisibleSet.Frames.TryGetValue (key, out result))
					{
						result = CreateNewFrame (target, program, camera);
						mVisibleSet.Frames.Add (key, result);
					}
					result.Passes.Add (values);
				}
			}
		}

		#endregion

		private class LayerSetKey
		{
			public InstanceIdentifier TargetId;
			public AssetIdentifier ShaderId;
			public InstanceIdentifier ViewerId;
		}

		private class LayerSet
		{
			public Dictionary<LayerSetKey, Frame> Frames { get; private set; }

			public LayerSet ()
			{
				Frames = new Dictionary<LayerSetKey, Frame>();
			}
		}

		private readonly LayerSet mDepthMapFrames;
		private readonly LayerSet mVisibleSet;

		private readonly TLightInfo[] mLights;
		private readonly TCameraInfo[] mCameras;

		public FrameDuplicator (TLightInfo[] lights, TCameraInfo[] cameras)
		{
			mDepthMapFrames = new LayerSet ();
			mVisibleSet = new LayerSet ();
			mLights = lights;
			mCameras = cameras;
		}
	}
}

