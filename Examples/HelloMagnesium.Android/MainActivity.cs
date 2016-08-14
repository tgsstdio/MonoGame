using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Magnesium;
using DryIoc;
using System;
using System.Diagnostics;

namespace HelloMagnesium.Android
{
	[Activity (Label = "HelloMagnesium.Android", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		private DryIoc.Container mContainer;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			//RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);

			// IOC here
			mContainer = new DryIoc.Container();
			mContainer.Register<IMgDriver, MgDriver>(Reuse.Singleton);
			SetupOpenGL();


			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.myButton);

			try
			{

				using (var driver = mContainer.Resolve<IMgDriver>())
				{
					driver.Initialize(
						new MgApplicationInfo
						{
							ApplicationName = "HelloMagnesium",
							ApplicationVersion = 1,
							EngineName = "MonoGame",
							EngineVersion = 1,
							ApiVersion = 1,
						},
						  new string[] { },
						new string[] { "VK_KHR_surface", "VK_KHR_android_surface" }
					 );

					using (var device = driver.CreateLogicalDevice())
					using (var partition = device.Queues[0].CreatePartition())
					{
						mContainer.RegisterInstance<IMgThreadPartition>(partition);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("MonoGame : " + ex);
			}

			button.Click += delegate
			{
				button.Text = string.Format("{0} clicks YO!", count++);
			};
		}


		protected override void OnDestroy ()
		{
			mContainer.Dispose ();
			mContainer = null;
			base.OnDestroy ();
		}

		void SetupOpenGL()
		{
			mContainer.Register<IMgEntrypoint, Magnesium.OpenGL.GLEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLDeviceEntrypoint, Magnesium.OpenGL.DefaultGLDeviceEntrypoint>(Reuse.Singleton);

			mContainer.Register<Magnesium.OpenGL.IGLQueue, Magnesium.OpenGL.GLQueue>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLQueueRenderer, Magnesium.OpenGL.GLQueueRenderer>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLErrorHandler, MockGLErrorHandler>(Reuse.Singleton);

			mContainer.Register<Magnesium.OpenGL.IGLCmdBlendEntrypoint, MockGLCmdBlendEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdStencilEntrypoint, MockGLCmdStencilEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdVBOEntrypoint, MockGLCmdVBOEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdRasterizationEntrypoint, MockGLCmdRasterizationEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdDepthEntrypoint, MockGLCmdDepthEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdShaderProgramCache, MockGLCmdShaderProgramCache>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdScissorsEntrypoint, MockGLCmdScissorsEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdDrawEntrypoint, MockGLCmdDrawEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdClearEntrypoint, MockGLCmdClearEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLCmdImageEntrypoint, MockGLCmdImageEntrypoint>(Reuse.Singleton);

			mContainer.Register<Magnesium.OpenGL.IGLSamplerEntrypoint, MockGLSamplerEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLImageEntrypoint, MockGLImageEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLImageViewEntrypoint, MockGLImageViewEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLImageDescriptorEntrypoint, MockGLImageDescriptorEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLShaderModuleEntrypoint, MockGLShaderModuleEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLDescriptorPoolEntrypoint, MockGLDescriptorPoolEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLBufferEntrypoint, MockGLBufferEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLDeviceMemoryEntrypoint, MockGLDeviceMemoryEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLSemaphoreEntrypoint, MockGLSemaphoreEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLGraphicsPipelineEntrypoint, MockGLGraphicsPipelineEntrypoint>(Reuse.Singleton);
			mContainer.Register<Magnesium.OpenGL.IGLImageFormatEntrypoint, MockGLImageFormatEntrypoint>(Reuse.Singleton);
		}
	}
}


