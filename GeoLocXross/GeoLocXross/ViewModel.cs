using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace GeoLocXross
{
	public class ViewModel : BindableObjectBase
	{

		public ViewModel(Position position)
		{
			_position = position;
		}
		private Position _position;
		public Position MyPosition
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				RaisePropertyChanged();
			}
		}
		private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
		public ObservableCollection<Pin> PinCollection
		{
			get
			{
				return _pinCollection;
			}
			set
			{
				_pinCollection = value;
				RaisePropertyChanged();
			}
		}
	}
}
