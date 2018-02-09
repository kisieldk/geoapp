using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeoLocXross
{
	public class BindableObjectBase : INotifyPropertyChanged
	{
		/// <summary>
		/// Zdarzenie zmiany wartości właściwości
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Notywikacja o zmianie wartości właściwości
		/// </summary>
		/// <param name="propertyName">Nazwa propercji</param>
		public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
			{
				try
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

				}
				catch (Exception ex)
				{
					
				}
			}
		}
	}
}
