using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;

namespace d_f_32.KanColleCacher
{
	class FolderExistsRule:ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			try
			{
				if (!Directory.Exists(value as string))
					return new ValidationResult(false, "无效的文件夹地址。");

				return new ValidationResult(true, null);
			}
			catch (Exception)
			{
				return new ValidationResult(false, "无效参数");
			}
		}
	}
}
