using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace d_f_32.KanColleCacher
{
	#region 废弃的代码
	//[Obsolete("Will be removed in next version.")]
	//class Debugger
	//{
	//	const string path = "debug.log";
	//	static Stopwatch watch = new Stopwatch();

	//	public static void WriteLine(string str = "")
	//	{
	//		File.AppendAllText(path, elapse_time() + str + "\r\n");
	//	}


	//	public static void WriteLine(params string[] args)
	//	{
	//		File.AppendAllText(
	//			path,
	//			elapse_time() + string.Join("\r\n\t\t\t\t", args) + "\r\n"
	//			);
	//	}

	//	public static void WriteLineF(string format, params object[] args)
	//	{
	//		File.AppendAllText(
	//			path,
	//			elapse_time() + string.Format(format, args) + "\r\n"
	//			);
	//	}

	//	public static string elapse_time()
	//	{
	//		watch.Stop();
	//		TimeSpan ts = watch.Elapsed;
	//		watch.Start();
	//		return String.Format("{0:00}:{1:00}:{2:00}.{3:000}\t",
	//		ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
	//	}
	//}
	#endregion

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
        //调试
		[Obsolete("Release后不要使用这个函数")]
        static public void Note(params string[] args)
        {
            File.AppendAllText(
                path,
                "DEBUG\t" + string.Join("\r\n\t\t", args) + "\r\n"
                );
        }

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

		static int print = 0;
		static public void PrintOnce(string head)
		{
			if (print >= 2)
				return;

			print++;

			File.AppendAllText("head.txt", head );
		}
    }
}
