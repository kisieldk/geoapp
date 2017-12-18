using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoLocXross
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		int count;
		bool tracking;
		Position savedPosition;
		public ObservableCollection<Position> Positions { get; } = new ObservableCollection<Position>();
		public MainPage()
		{
			InitializeComponent();
		}

		private async void bt_Clicked(object sender, EventArgs e)
		{
			try
			{
				var hasPermission = await Utils.CheckPermissions(Permission.Location);
				if (!hasPermission)
					return;
				var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
				var status = results[Permission.Location];
				aa.Text = "gps - searching...";
				var locator = CrossGeolocator.Current;
				var gowno = CrossGeolocator.IsSupported;
				locator.DesiredAccuracy = 1000000;

				var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(60000), null, true);
				var a = position.Latitude;
				if (position == null)
				{
					aa.Text = "null gps :(";
					return;
				}
				savedPosition = position;
				GeoLoc.Latitude = position.Latitude;
				GeoLoc.Longitude = position.Longitude;
				aa.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
					position.Timestamp, position.Latitude, position.Longitude,
					position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
				btMap.IsEnabled = true;
			}
			catch (Exception ex)
			{
				await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
			}
		}

		private async void btMap_Clicked(object sender, EventArgs e)
		{
			try
			{
				await Navigation.PushAsync(new MapPage());
			}
			catch (Exception ex)
			{

			}
			
		}

		private void btNavi_Clicked(object sender, EventArgs e)
		{
			try
			{
				Uri rrl = new Uri(string.Format("google.navigation:?q=Aquapark,Sopot"));
				Device.OpenUri(rrl);
			}
			catch (Exception ex)
			{

			
			}
	
		}
	}
}
