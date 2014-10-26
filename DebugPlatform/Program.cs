using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Text.RegularExpressions;

namespace DebugPlatform
{
	class Program
	{
			const string sample = @"svdata={""api_result"":1,""api_result_msg"":""\u6210\u529f"",""api_data"":{""api_count"":14,""api_page_count"":3,""api_disp_page"":3,
""api_list"":[
{""api_no"":404,""api_category"":4,""api_type"":3,""api_state"":1,""api_title"":""\u5927\u898f\u6a21\u9060\u5f81\u4f5c\u6226\u3001\u767a\u4ee4\uff01"",""api_detail"":""\u4eca\u9031\u4e2d\u306b\u300c\u9060\u5f81\u300d30\u56de\u6210\u529f\u3055\u305b\u3088\u3046\uff01"",""api_get_material"":[300,500,500,300],""api_bonus_flag"":1,""api_progress_flag"":0,""api_invalid_flag"":0},
{""api_no"":503,""api_category"":5,""api_type"":2,""api_state"":1,""api_title"":""\u8266\u968a\u5927\u6574\u5099\uff01"",""api_detail"":""\u5404\u8266\u968a\u304b\u3089\u6574\u5099\u304c\u5fc5\u8981\u306a\u8266\u30925\u96bb\u4ee5\u4e0a\u30c9\u30c3\u30af\u5165\u308a\u3055\u305b\u3001\u5927\u898f\u6a21\u306a\u6574\u5099\u3092\u3057\u3088\u3046\uff01"",""api_get_material"":[30,30,30,30],""api_bonus_flag"":1,""api_progress_flag"":0,""api_invalid_flag"":0},
{""api_no"":606,""api_category"":6,""api_type"":2,""api_state"":1,""api_title"":""\u65b0\u9020\u8266\u300c\u5efa\u9020\u300d\u6307\u4ee4"",""api_detail"":""\u300c\u5de5\u5ee0\u300d\u3067\u8266\u5a18\u3092\u672c\u65e5\u4e2d\u306b\u65b0\u305f\u306b\u300c\u5efa\u9020\u300d\u3057\u3088\u3046\uff01"",""api_get_material"":[50,50,50,50],""api_bonus_flag"":1,""api_progress_flag"":0,""api_invalid_flag"":0},
{""api_no"":702,""api_category"":7,""api_type"":2,""api_state"":1,""api_title"":""\u8266\u306e\u300c\u8fd1\u4ee3\u5316\u6539\u4fee\u300d\u3092\u5b9f\u65bd\u305b\u3088\uff01"",""api_detail"":""\u8fd1\u4ee3\u5316\u6539\u4fee\u3092\u5b9f\u65bd\u3057\u3066\u3001\uff12\u56de\u4ee5\u4e0a\u3053\u308c\u3092\u6210\u529f\u3055\u305b\u3088\uff01"",""api_get_material"":[20,20,50,0],""api_bonus_flag"":1,""api_progress_flag"":0,""api_invalid_flag"":0}
,-1],""api_exec_count"":0,""api_exec_type"":2456180}}";

		static void Main(string[] args)
		{

		}
		class api_quest
		{
			public int api_no;
			public string api_title;
			public string api_title_text;
			public string api_detail;
			public string api_detail_text;
		}

		static void getdata()
		{
			string svdata = sample;
			Regex reg = new Regex(@"\{""api_no"":(\d{1,4}).*?""api_title"":""([\\\w]+).*?""api_detail"":""([\\\w]+).*?}");

			List<string> segments = new List<string>();
			List<api_quest> list = new List<api_quest>();
			api_quest item;
			int pos = svdata.IndexOf("api_list");
			int org = 0;
			
			while (pos > 0)
			{
				item = new api_quest();
				pos = svdata.IndexOf("api_no", pos);
				if (pos < 0) break;
				pos = svdata.IndexOf(':', pos);
				segments.Add(svdata.Substring(org, pos - org));
				org = pos + 1;


			}
			int pos_start = svdata.IndexOf("api_no");

		}
	}
	

	[ValueConversion(typeof(int), typeof(bool))]
	class SwitchCaseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (int)value == (int)parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ((bool)value)
				return (int)parameter;

			return DependencyProperty.UnsetValue;
		}
	}
}
