using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace d_f_32.KanColleCacher
{

    [ValueConversion(typeof(int), typeof(bool?))]
    class SwitchCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if (parameter is string)
				return (int)value == int.Parse((string)parameter);
			
				return (int)value == (int)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if ((bool?)value == true)
				if (parameter is string)
					return int.Parse((string)parameter);
				else
					return (int)parameter;

			return DependencyProperty.UnsetValue;
        }
    }

}
