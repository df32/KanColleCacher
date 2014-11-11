using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;




/*
 *	输出调试信息的规则
 * ------------------------------
 * 【头引用】	
 *	using Debug = System.Diagnostics.Debug;
 * 
 * 【输出调试】	
 * （只输出到调试监听）
 *	Debug.WriteLine("CACHR> (信息内容)");
 * 
 * 【输出警告】
 *  （输出到调试监听 和 错误文件）
 *  Log.Warning(sender, "警告信息", "警告信息2"...)
 * 
 * 【输出异常】	
 * （输出到调试监听 和 错误文件）
 *	Log.Execption(sender, ex, message)
 */


namespace d_f_32.KanColleCacher
{

	class Log
    {
        const string path = "error2.log";
		const string wrMsg = @"
===================================KAN==COLLE==CACHER======
WARNING  date = {0}, sender = {1}, message = 
		 {2}
";
        const string exMsg = @"
===================================KAN==COLLE==CACHER======
ERROR	date = {0}, sender = {1}, 
		message = {2}
		exception = {3}
";
		const string wrFmt = @"
CACHR>	{0}
";
		const string exFmt = @"
CACHR>	{0}
-------------------------------------------------
{1}
-------------------------------------------------
";
        
        //希望被人看到的警告信息
		static public void Warning(object sender, params string[] args)
		{
			var message = string.Join("\r\n\t\t ", args) + "\r\n";
			Debug.WriteLine(wrFmt, message);

			File.AppendAllText(path,
				string.Format(wrMsg,
					DateTimeOffset.Now,
					sender,
					message
				)
			);

			Debug.Flush();
		}

		//希望被人看到的错误信息
        static public void Exception(object sender, Exception exception, string describe)
        {
			Debug.WriteLine(exFmt, describe, exception);

            try
            {
                var message = string.Format(
						exMsg, 
						DateTimeOffset.Now, 
						sender, 
						describe, 
						exception
					);

                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
				Debug.WriteLine("CACHR>	Log.Exception()异常");
                Debug.WriteLine("		"+ex.Message);
            }

			Debug.Flush();
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

#endif

    }
}
