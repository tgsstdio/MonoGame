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
			base.OnCreate (savedInstanceState);

			// IOC here
			mContainer = new DryIoc.Container ();



			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

			try
			{

				using (var driver = mContainer.Resolve<IMgDriver>())
				{
					driver.Initialize(new MgApplicationInfo
					{
						ApplicationName = "HelloMagnesium",
						ApplicationVersion = 1,
						EngineName = "MonoGame",
						EngineVersion = 1,
						ApiVersion = 1,
					});

					using (var device = driver.CreateLogicalDevice())
					using (var partition = device.Queues[0].CreatePartition())
					{
						mContainer.RegisterInstance<IMgThreadPartition>(partition);
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("MonoGame : " + ex.Message);
			}

			button.Click += delegate {
				button.Text = string.Format ("{0} clicks YO!", count++);
			};
		}

		protected override void OnDestroy ()
		{
			mContainer.Dispose ();
			mContainer = null;
			base.OnDestroy ();
		}
	}
}


