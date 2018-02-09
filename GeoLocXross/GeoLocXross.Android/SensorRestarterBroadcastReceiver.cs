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

namespace GeoLocXross.Droid
{
	//[BroadcastReceiver(Label = "RestartServiceWhenStopped",Exported =true,Enabled =true,Name = "GeoLocXross.Droid.SensorRestarterBroadcastReceiver")]
	[BroadcastReceiver]
	public class SensorRestarterBroadcastReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Log.Debug("IN BRODCASTRECIVER", "SERVICE STOPED!");
			context.StartService(new Intent(context, typeof(SensorService)));
		}
	}
}