using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace d_f_32.KanColleCacher
{
	#region 保留的代码
	//[ValueConversion(typeof(int), typeof(bool?))]
	//class SwitchCaseConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//	{
	//		if (parameter is string)
	//			return (int)value == int.Parse((string)parameter);
			
	//			return (int)value == (int)parameter;
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//	{
	//		if ((bool?)value == true)
	//			if (parameter is string)
	//				return int.Parse((string)parameter);
	//			else
	//				return (int)parameter;

	//		return DependencyProperty.UnsetValue;
	//	}
	//}

	//[ValueConversion(typeof(int), typeof(bool?))]
	//class CacheFilesValueConverter: IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//	{
	//		var _val = (int)value;
	//		if (_val > 0) return true;
	//		return false;
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
	//	{
	//		var _val = (bool?) value;
	//		if (_val == true) return 2;
	//		return 0;
	//	}
	//}
	#endregion

	[ValueConversion(typeof(int), typeof(bool?))]
	class ThreeStateValueConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var _val = (int)value;
			if (_val > 1) return true;
			else if (_val > 0) return null;
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var _val = (bool?)value;
			if (_val == true) return 2;
			else if (_val == null) return 1;
			return 0;
		}
	}
}
