using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android.Util;
using System.Net;

namespace GeoLocXross.Droid
{
	[Activity(Label = "GeoLocXross", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		Intent mServiceIntent;
		private SensorService mSensorService;
		Context ctx;
		public Context getCtx()
		{
			return ctx;
		}
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			//Accept non-signed SSL Certificates
			ServicePointManager.ServerCertificateValidationCallback += delegate
			{
				return true;
			};
			base.OnCreate(bundle);
			ctx = this;
			global::Xamarin.Forms.Forms.Init(this, bundle);
			LoadApplication(new App());
			mSensorService = new SensorService(getCtx());
			mServiceIntent = new Intent(getCtx(), typeof(SensorService));

			bool running = IsServiceRunning(getCtx(), typeof(SensorService));
			if (!running)
			{			
				StartService(mServiceIntent);
			}
			if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
			{
				Intent intenta = new Intent();
				String packageName = ctx.PackageName;
				PowerManager pm = (PowerManager)ctx.GetSystemService(Context.PowerService);

				if (pm.IsIgnoringBatteryOptimizations(packageName))
				{
					intenta.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
					intenta.SetData(Android.Net.Uri.Parse("package:" + packageName));
					ctx.StartActivity(intenta);
				}
			}
			Xamarin.FormsMaps.Init(this, bundle);
		}
		protected override void OnDestroy()
		{
			StopService(mServiceIntent);		
			Log.Debug("MAINAPP?", "UBIL SERVICE");
			base.OnDestroy();
		}

		public bool IsServiceRunning(Context context, Type serviceClass)
		{
			ActivityManager manager = (ActivityManager)context.GetSystemService(Context.ActivityService);
			foreach (var service in manager.GetRunningServices(int.MaxValue))
			{
				if (service.Process == context.PackageName && service.Service.ClassName.EndsWith(serviceClass.Name))
				{
					return true;
				}
			}
			return false;
		}	
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}

