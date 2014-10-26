using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace d_f_32.KanColleCacher
{

	class Log
    {
        const string path = "error.log";
        const string exMsg = @"
===================================KAN==COLLE==CACHER======
ERROR	date = {0}, sender = {1}, 
		message = {2}
		exception = {3}
";
        const string wrMsg = @"
===================================KAN==COLLE==CACHER======
WARNING  date = {0}, sender = {1}, message = 
		 {2}
";
        
        //预期的未知的错误
		static public void Warning(object sender, params string[] args)
		{
			File.AppendAllText(
				path,
				string.Format(
					wrMsg,
					sender,
					DateTimeOffset.Now,
					string.Join("\r\n\t\t ", args) + "\r\n"
				)
			);
		}	

        //非预期的错误
        static public void Exception(object sender, Exception exception, string describe)
        {
            try
            {
                var message = string.Format(exMsg, DateTimeOffset.Now, sender, describe, exception);

                Debug.WriteLine(message);
                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

#if DEBUG
		static int print = 0;
		[Obsolete("仅用于Debug模式")]
		static public void PrintOnce(string head)
		{
			if (print >= 2)
				return;

			print++;

			File.AppendAllText("head.txt", head + "\r\n\r\n" );
		}

		//调试
		[Obsolete("仅用于Debug模式")]
		static public void Note(params string[] args)
		{
			File.AppendAllText(
				path,
				"DEBUG\t" + string.Join("\r\n\t\t", args) + "\r\n"
				);
		}
#endif

    }
}
