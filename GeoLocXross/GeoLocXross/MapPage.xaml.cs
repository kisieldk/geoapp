
using System.Diagnostics;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Utilities;
using System.Reflection;
using BruTile;
namespace GeoLocXross
{

	public partial class MapPage
	{
		public MapPage()
		{
			InitializeComponent();
			try
			{
				var mapControl = new MapsUIView();
				mapControl.NativeMap.Layers.Add(OpenStreetMap.CreateTileLayer());

				var centerOfWarsaw = new Mapsui.Geometries.Point(GeoLoc.Longitude, GeoLoc.Latitude);
				// OSM uses spherical mercator coordinates. So transform the lon lat coordinates to spherical mercator
				var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfWarsaw.X, centerOfWarsaw.Y);
				// Set the center of the viewport to the coordinate. The UI will refresh automatically
				mapControl.NativeMap.NavigateTo(sphericalMercatorCoordinate);
				// Additionally you might want to set the resolution, this could depend on your specific purpose
				mapControl.NativeMap.NavigateTo(mapControl.NativeMap.Resolutions[15]);

				var layer = GenerateIconLayer();
				mapControl.NativeMap.Layers.Add(layer);
				mapControl.NativeMap.InfoLayers.Add(layer);

				mapControl.NativeMap.Info += (sender, args) =>
				{
					var layername = args.Layer?.Name;
					var featureLabel = args.Feature?["Label"]?.ToString();
					var featureType = args.Feature?["Type"]?.ToString();

					if (!string.IsNullOrWhiteSpace(featureLabel))
					{
						ShowPopup(featureLabel, featureType);
					}

					Debug.WriteLine("Info Event was invoked.");
					Debug.WriteLine("Layername: " + layername);
					Debug.WriteLine("Feature Label: " + featureLabel);
					Debug.WriteLine("Feature Type: " + featureType);

					Debug.WriteLine("World Postion: {0:F4} , {1:F4}", args.WorldPosition?.X, args.WorldPosition?.Y);
					Debug.WriteLine("Screen Postion: {0:F4} , {1:F4}", args.ScreenPosition?.X, args.ScreenPosition?.Y);
				};

				ContentGrid.Children.Add(mapControl);
			}
			catch (System.Exception ex)
			{

				
			}
			
		}
		async void ShowPopup(string feature, string type)
		{
			if (await this.DisplayAlert(
				"You have clicked " + feature,
				"Do you want to see details?",
				"Yes",
				"No"))
			{
				Debug.WriteLine("User clicked OK");
			}
		}

		private ILayer GenerateIconLayer()
		{
			//new Point(),

			var layername = "My Local Layer";
			var imageStream = EmbeddedResourceLoader.Load("Ico.lock.png", typeof(MapPage));
			return new Layer(layername)
			{
				DataSource = new MemoryProvider(GetIconFeatures()),
				Style = new SymbolStyle
				{
					BitmapId = BitmapRegistry.Instance.Register(imageStream),
					SymbolScale = 0.75
				}

			};
		}
		private Features GetIconFeatures()
		{
			var features = new Features();
			var feature1 = new Feature
			{
				//Gdansk1
				Geometry = SphericalMercator.FromLonLat(GeoLoc.Longitude, GeoLoc.Latitude),

				["Label"] = "gdansk",
				["Type"] = "Place"
			};
			features.Add(feature1);
			//var feature = new Feature
			//{
			//	//warsaw
			//	Geometry = SphericalMercator.FromLonLat(21.074181, 52.277191),

			//	["Label"] = "Warsaw",
			//	["Type"] = "My Feature Type"
			//};
			//features.Add(feature);
			return features;
		}
	}
}