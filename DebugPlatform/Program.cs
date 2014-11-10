using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
//using System.Text.RegularExpressions;

using System.Xml;
using System.Xml.Linq;
using System.IO;


//using d_f_32.KanColleCacher;
namespace DebugPlatform
{
	class Program
	{

		static void Main(string[] args)
		{
			Console.WriteLine("程序开始....\n");

			//Settings.Load();
			//var set = Settings.Current;
			//set.CacheFolder = @"B:\GitHub\KanColleCacher\DebugPlatform\";

			Console.WriteLine("文件加载....");
			ModifiedRecord.Load();
			//Console.ReadLine();

			Console.WriteLine("文件保存....");
			ModifiedRecord.Save();


			Console.ReadLine();
		}



	}







	class ModifiedRecord
	{
		static string filepath;
		static XDocument fileXML;
		static IEnumerable<XElement> recordList;

		const string _RootName = "Last-Modified";
		const string _ItemElm = "Record";
		const string _ElmPath = "Path";
		const string _ElmTime = "Time";
		const string _ElmVersion = "Version";

		#region 加载与保存

		/// <summary>
		/// 从Last-Modified.xml加载记录
		/// </summary>
		static public void Load()
		{
			//filepath = Settings.Current.CacheFolder + "\\Last-Modified.xml";
			filepath = @"B:\GitHub\KanColleCacher\DebugPlatform\Last-Modified - 副本.xml";
			try
			{
				if (File.Exists(filepath))
					fileXML = XDocument.Load(filepath);
			}
			catch (Exception ex)
			{
				//Log.Exception(ex.InnerException, ex, "加载Last-Modified.xml时发生异常");
				throw ex;
			}

			if (fileXML == null)
			{
				fileXML = new XDocument();
				fileXML.Add(new XElement(_RootName));
			}

			recordList = fileXML.Descendants(_ItemElm);
			//recordList 和 fileXML是同步的
		}

		/// <summary>
		/// 保存到Last-Modified.xml
		/// </summary>
		static public void Save()
		{
			filepath = @"B:\GitHub\KanColleCacher\DebugPlatform\Last-Modified.xml";



			try
			{
				var elms = fileXML.Descendants(_ItemElm)
							.OrderBy(elm =>
								{ return elm.Element(_ElmPath).Value; }
							).ToArray();

				fileXML.Root.Elements().Remove();
				fileXML.Root.Add(elms);
			}
			catch
			{
				//Log.Exception(ex.InnerException, ex, "保存Last-Modified.xml时发生异常");
			}
			
			fileXML.Save(filepath);
		}

	}
		#endregion



}
