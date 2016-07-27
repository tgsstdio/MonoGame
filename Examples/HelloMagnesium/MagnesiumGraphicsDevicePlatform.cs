using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OpenTK.Graphics;
using MonoGame.Graphics;
using MonoGame.Platform.DesktopGL;
using Magnesium;
using OpenTK;

namespace HelloMagnesium
{
	public class MagnesiumGraphicsDevicePlatform
	{
		private readonly NativeWindow mWindow;
		private readonly IGraphicsDevicePreferences mDevicePreferences;
		private readonly IGraphicsDeviceLogger mLogger;
		private IPresentationParameters mPresentation;
		private IMgDeviceQuery mDeviceQuery;

		IGLExtensionLookup mExtensions;

		IGLFramebufferHelperSelector mSelector;

		IGLDevicePlatform mGLPlatform;

		public MagnesiumGraphicsDevicePlatform 
		(
			// IMPLEMENTATION BELOW
			NativeWindow window
			,IMgDeviceQuery deviceQuery
			,IGraphicsDevicePreferences devicePreferences
			,IGraphicsDeviceLogger logger
			,IPresentationParameters presentation

			,IGLFramebufferHelperSelector selector

			,IGLExtensionLookup extensions
			,IGLDevicePlatform glPlatform
		)
		{
			mWindow = window;
			mDevicePreferences = devicePreferences;
			mDeviceQuery = deviceQuery;
			mLogger = logger;
			mPresentation = presentation;

			mExtensions = extensions;
			mSelector = selector;
			mGLPlatform = glPlatform;
		}

		internal IGraphicsContext Context { get; private set; }

		void SetupContext ()
		{
			GraphicsMode mode;
			var wnd = mWindow.WindowInfo;

			// Create an OpenGL compatibility context
			var flags = GraphicsContextFlags.Default;
			int major = 1;
			int minor = 0;

			if (Context == null || Context.IsDisposed)
			{
				var color = mDeviceQuery.GetColorFormat(mDeviceQuery.GetFormat(mPresentation.BackBufferFormat));
				var depthBit = mDeviceQuery.GetDepthBit(mPresentation.DepthStencilFormat);
				var stencil = mDeviceQuery.GetStencilBit(mPresentation.DepthStencilFormat);

				var samples = 0;
				if (mDevicePreferences.PreferMultiSampling)
				{
					// Use a default of 4x samples if PreferMultiSampling is enabled
					// without explicitly setting the desired MultiSampleCount.
					if (mPresentation.MultiSampleCount == 0)
					{
						mPresentation.MultiSampleCount = 4;
					}
					samples = mPresentation.MultiSampleCount;
				}
				mode = new GraphicsMode(color, depthBit, stencil, samples);
				try
				{
					Context = new GraphicsContext (mode, wnd, major, minor, flags);
				}
				catch (Exception e)
				{
					mLogger.Log (string.Format ("Failed to create OpenGL context, retrying. Error: {0}", e));
					major = 1;
					minor = 0;
					flags = GraphicsContextFlags.Default;
					Context = new GraphicsContext (mode, wnd, major, minor, flags);
				}
			}
			Context.MakeCurrent (wnd);
			(Context as IGraphicsContextInternal).LoadAll ();
			Context.SwapInterval = mDeviceQuery.GetSwapInterval (mPresentation.PresentationInterval);
			// TODO : background threading 
			// Provide the graphics context for background loading
			// Note: this context should use the same GraphicsMode,
			// major, minor version and flags parameters as the main
			// context. Otherwise, context sharing will very likely fail.
			//			if (Threading.BackgroundContext == null)
			//			{
			//				Threading.BackgroundContext = new GraphicsContext(mode, wnd, major, minor, flags);
			//				Threading.WindowInfo = wnd;
			//				Threading.BackgroundContext.MakeCurrent(null);
			//			}
			Context.MakeCurrent (wnd);

			mExtensions.Initialize();

			mGLPlatform.Initialize ();
		}

		public void Setup ()
		{
			SetupContext ();

			//SetupMagnesium ();
		}

		void SetupMagnesium ()
		{
			var depthStencil = mDeviceQuery.GetDepthStencilFormat (mPresentation.DepthStencilFormat);
			IMgCommandBuffer setupCmdBuffer = null;
			//SetupDepthStencil (device, setupCmdBuffer, depthStencil);
		}

//		static IMgDevice CreateDevice (IMgPhysicalDevice gpu, uint graphicsQueueIndex)
//		{	
//			const string VK_KHR_SWAPCHAIN_EXTENSION_NAME = "VK_KHR_swapchain";
//			var deviceCreateInfo = new MgDeviceCreateInfo {
//				EnabledExtensionNames = new[] {
//					VK_KHR_SWAPCHAIN_EXTENSION_NAME
//				},
//				QueueCreateInfos = new[] {
//					new MgDeviceQueueCreateInfo {
//						QueueFamilyIndex = graphicsQueueIndex,
//						QueueCount = 1,
//						QueuePriorities = new[] {
//							0f
//						},
//					},
//				},
//			};
//			IMgDevice device;
//			var errorCode = gpu.CreateDevice (deviceCreateInfo, null, out device);
//			Debug.Assert (errorCode == Result.SUCCESS);
//			return device;
//		}

//		class DepthStencilData
//		{
//			public IMgImage image;
//			public IMgImageView view;
//			public IMgDeviceMemory mem;
//		}
//
//		private DepthStencilData mDepthStencil = new DepthStencilData();
//		static void SetupDepthStencil (IMgDevice device, IMgCommandBuffer setupCmdBuffer, MgFormat depthStencil)
//		{
//			if (mDepthStencil != null)
//			{
//				if (mDepthStencil.view != null)
//					mDepthStencil.view.DestroyImageView (device, null);
//				if (mDepthStencil.image != null)
//					mDepthStencil.image.DestroyImage (device, null);
//				if (mDepthStencil.mem != null)
//					mDepthStencil.mem.FreeMemory (device, null);
//			}
//
//			var image = new MgImageCreateInfo {
//				ImageType = MgImageType.TYPE_2D,
//				Format = depthStencil,
//				Extent = new MgExtent3D {
//					Width= (uint) mPresentation.BackBufferWidth,
//					Height = (uint) mPresentation.BackBufferHeight,
//					Depth = 1
//				},
//				MipLevels = 1,
//				ArrayLayers = 1,
//				Samples = MgSampleCountFlagBits.COUNT_1_BIT,
//				Tiling = MgImageTiling.OPTIMAL,
//				Usage = MgImageUsageFlagBits.DEPTH_STENCIL_ATTACHMENT_BIT | MgImageUsageFlagBits.TRANSFER_SRC_BIT,
//				Flags = 0,
//			};
//			var mem_alloc = new MgMemoryAllocateInfo {
//				AllocationSize = 0,
//				MemoryTypeIndex = 0,
//			};
//			var depthStencilView = new MgImageViewCreateInfo {
//				ViewType = MgImageViewType.TYPE_2D,
//				Format = depthStencil,
//				Flags = 0,
//				SubresourceRange = new MgImageSubresourceRange {
//					AspectMask = MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT,
//					BaseMipLevel = 0,
//					LevelCount = 1,
//					BaseArrayLayer = 0,
//					LayerCount = 1,
//				},
//			};
//			MgMemoryRequirements memReqs;
//			Magnesium.Result err;
//
//			err = device.CreateImage (image, null, out mDepthStencil.image);
//			Debug.Assert (err == Result.SUCCESS);
//			device.GetImageMemoryRequirements (mDepthStencil.image, out memReqs);
//
//			mem_alloc.AllocationSize = memReqs.Size;
//
//			uint memTypeIndex;
//			bool memoryTypeFound = mPartition.GetMemoryType (memReqs.MemoryTypeBits, MgMemoryPropertyFlagBits.DEVICE_LOCAL_BIT, out memTypeIndex);
//			Debug.Assert (memoryTypeFound);
//			mem_alloc.MemoryTypeIndex = memTypeIndex;
//
//			err = device.AllocateMemory (mem_alloc, null, out mDepthStencil.mem);
//			Debug.Assert (err == Result.SUCCESS);
//			err = mDepthStencil.image.BindImageMemory (device, mDepthStencil.mem, 0);
//			Debug.Assert (err == Result.SUCCESS);
//
//			mImageTools.SetImageLayout(setupCmdBuffer, 
//				mDepthStencil.image, 
//				MgImageAspectFlagBits.DEPTH_BIT | MgImageAspectFlagBits.STENCIL_BIT, MgImageLayout.UNDEFINED, MgImageLayout.DEPTH_STENCIL_ATTACHMENT_OPTIMAL);
//			
//			depthStencilView.Image = mDepthStencil.image;
//
//			err = device.CreateImageView (depthStencilView, null, out mDepthStencil.view);
//			Debug.Assert (err == Result.SUCCESS);
//		}

//		private MgPhysicalDeviceMemoryProperties mDeviceMemoryProperties;
//		private bool getMemoryType(uint typeBits, MgMemoryPropertyFlagBits memoryPropertyFlags, out uint typeIndex)
//		{
//			// Search memtypes to find first index with those properties
//			for (UInt32 i = 0; i < mDeviceMemoryProperties.MemoryTypes.Length; i++)
//			{
//				if ((typeBits & 1) == 1)
//				{
//					// Type is available, does it match user properties?
//					if ((mDeviceMemoryProperties.MemoryTypes[i].PropertyFlags & memoryPropertyFlags) == memoryPropertyFlags)
//					{
//						typeIndex = i;
//						return true;
//					}
//				}
//				typeBits >>= 1;
//			}
//			// No memory types matched, return failure
//			typeIndex = 0;
//			return false;
//		}

		public void Initialize ()
		{
			// Ensure the vertex attributes are reset

			// Free all the cached shader programs. 

			// Force reseting states
			mSelector.Initialize();
		}

		#region IGraphicsDevicePlatform implementation

		public GraphicsProfile GetHighestSupportedGraphicsProfile (IGraphicsDevice graphicsDevice)
		{
			return GraphicsProfile.HiDef;
		}

		public void SetViewport (ref Viewport value)
		{
			throw new NotImplementedException ();
		}

		public void DrawIndexedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount)
		{
			throw new NotImplementedException ();
		}

		public IRenderTarget ApplyRenderTargets ()
		{
			throw new NotImplementedException ();
		}

		public void ApplyDefaultRenderTarget ()
		{
			throw new NotImplementedException ();
		}

		public void ResolveRenderTargets ()
		{
			throw new NotImplementedException ();
		}

		public void ApplyState (bool applyShaders)
		{
			throw new NotImplementedException ();
		}

		public void Present ()
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount, IVertexDeclaration vertexDeclaration) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void DrawPrimitives (PrimitiveType primitiveType, int vertexStart, int vertexCount)
		{
			throw new NotImplementedException ();
		}

		public void DrawUserPrimitives<T> (PrimitiveType primitiveType, T[] vertexData, int vertexOffset, IVertexDeclaration vertexDeclaration, int vertexCount) where T : struct
		{
			throw new NotImplementedException ();
		}

		public void Clear (ClearOptions options, OpenTK.Vector4 vector4, float maxDepth, int i)
		{
			throw new NotImplementedException ();
		}

		public void BeginApplyState ()
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose ()
		{

		}

		#endregion
	}
}

