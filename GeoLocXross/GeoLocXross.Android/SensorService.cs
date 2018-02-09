using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Java.Util;
using Android.Support.V7.App;
using System.Net.Http;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Java.Lang;

namespace GeoLocXross.Droid
{
	//[Service(Enabled =true,Name = "GeoLocXross.Droid.SensorService")]
	[Service]
	public class SensorService : Service
	{
		public int counter = 0;
		public SensorService(Context applicationContext)
		{

			Log.Debug("HERE", "here I am!");
		}


		public override IBinder OnBind(Intent intent)
		{
			return null;
		}
		public SensorService()
		{
		}
		PowerManager.WakeLock wl;
		public override void OnCreate()
		{
			PowerManager pm = (PowerManager)this.GetSystemService(
										  Context.PowerService);
			wl = pm.NewWakeLock(WakeLockFlags.Partial, "SensorService");
			wl.Acquire();

			Intent intent = new Intent(this, typeof(MainActivity));
			PendingIntent pendingIntent = PendingIntent.GetActivity(Application.Context, 0, intent, 0);

			Notification builder = new NotificationCompat.Builder(this)
		   .SetContentTitle("Test")
		   .SetContentText("Test")
			.SetDefaults(1)
		   .SetSmallIcon(Resource.Drawable.icon)
		   .SetContentIntent(pendingIntent).Build();
			StartForeground(1337, builder);
		}
		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{

			StartTimer(TimeSpan.FromSeconds(20), () =>
			{
			
				System.Threading.Tasks.Task.Run(async () =>
				{

					try
					{
						string lat = string.Empty;
						string lon = string.Empty;
						var hasPermission = await Utils.CheckPermissions(Permission.Location);

						var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
						var status = results[Permission.Location];
						var locator = CrossGeolocator.Current;
						locator.DesiredAccuracy = 4500;
						if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
						{
							var positionFromLoaction = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
							lat = string.IsNullOrEmpty(positionFromLoaction?.Latitude.ToString()?.Replace(',', '.')) ? "1" : positionFromLoaction.Latitude.ToString().Replace(',', '.');
							lon = string.IsNullOrEmpty(positionFromLoaction?.Longitude.ToString()?.Replace(',', '.')) ? "1" : positionFromLoaction.Longitude.ToString().Replace(',', '.');
						}

						Log.Debug("FROM TIMER", string.Format("GPS lat {0} ; long {1}", lat, lon));
						//using (var client = new HttpClient())
						//{
						//	client.DefaultRequestHeaders.Add("Token", "MDU1MmY0MzUtOWI1My00NjZiLWFiNzUtNjQ2YzhkM2ViNGZhOnN6eW1lbGthOjg5NDgwMjI3MTYwNDMxODUxNjE=");
						//	// send a GET request  
						//	var uri = string.Format("https://104tstwss4.dfcpoland.com/WSS4MobileInsideApi.svc/GetNotReadNotificationNumber?employeeId={0}&maxId={1}&latitude={2}&longitude={3}",
						//		1845,
						//		false,
						//		lat,
						//		lon);
						//	var result = await client.GetStringAsync(uri);

						//	//handling the answer  


						//}

					}
					catch (System.Exception ex)
					{

					}

				});



				return true;
			});
			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			Log.Debug("Exit", "Destrouy");					
			Intent broadcastIntent = new Intent(this, typeof(SensorRestarterBroadcastReceiver));
			//SendBroadcast(broadcastIntent);
			//handler.UnregisterFromRuntime();
			wl.Release();
		
		}


		public void StartTimer(TimeSpan interval, Func<bool> callback)
		{

			var handler = new Handler(Looper.MainLooper);
			handler.PostDelayed(() =>
			{
				if (callback())
					StartTimer(interval, callback);
				handler.Dispose();
				handler = null;
			}, (long)interval.TotalMilliseconds);
			
		} 
		//public void startTimer()
		//{
		//	//set a new Timer
		//	timer = new Timer();

		//	//initialize the TimerTask's job
		//	initializeTimerTask();

		//	//schedule the timer, to wake up every 1 second
		//	timer.Schedule(()=> { }, 1000, 1000); //
		//}

		//public void initializeTimerTask()
		//{
		//	timerTask.Run() = ()=> { };
			
		//}

		private Timer timer;
		private TimerTask timerTask;
		long oldTime = 0;
	}
}