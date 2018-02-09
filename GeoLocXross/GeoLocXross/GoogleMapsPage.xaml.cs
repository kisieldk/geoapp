using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GeoLocXross
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GoogleMapsPage : ContentPage
	{
		private Task _runningTask;
		private MyOwnTimer _timer;
		private ViewModel vm;
		private double nextLocationIncrement = 0.0031d;
		private double nextLocationIncrementLat = 0.0031d;
		public GoogleMapsPage(Position position)
		{
			InitializeComponent();
			vm = new ViewModel(position);
			vm.PinCollection.Add(new Pin() { Position = position, Type = PinType.Generic, Label = "I'm a Pin" });
			BindingContext = vm;
			MAP.IsShowingUser = true;
			StartGettingNewPosition();
			//Add50Pin();

		}
		protected override void OnDisappearing()
		{
			_timer.Stop();
			base.OnDisappearing();
		}


		private void Add50Pin()
		{
			var a= new Random();
			for (int i = 0; i < 50; i++)
			{
				nextLocationIncrement += ((a.NextDouble() * (0.09d - (-0.003d)) + (-0.009d)));
				nextLocationIncrementLat += ((a.NextDouble() * (0.09d - (-0.003d)) + (-0.009d)));
				var newPos = new Position(vm.MyPosition.Latitude + nextLocationIncrementLat, vm.MyPosition.Longitude + nextLocationIncrement);
				vm.PinCollection.Add(new Pin() { Position = newPos, Type = PinType.Generic, Label = "New" });
			}
			MAP.MoveToRegion(MapSpan.FromCenterAndRadius(vm.MyPosition, Distance.FromKilometers(30)));
		}

		private void StartGettingNewPosition()
		{
			_timer = new MyOwnTimer(TimeSpan.FromSeconds(3), () => {
				nextLocationIncrement += 0.0011d;
				var newPos = new Position(vm.MyPosition.Latitude, vm.MyPosition.Longitude + nextLocationIncrement);
				vm.PinCollection.Clear();
				vm.PinCollection.Add(new Pin() { Position = newPos, Type = PinType.Generic, Label = "New" });
				MAP.MoveToRegion(MapSpan.FromCenterAndRadius(newPos, Distance.FromKilometers(1)));
			});
			_timer.Start();
		}
	}
}