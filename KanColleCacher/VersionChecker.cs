using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Grabacr07.KanColleViewer.Models.Data.Xml;

namespace d_f_32.KanColleCacher
{
	#region 保留的代码
	/*
	class ModifiedRecord
	{
		static string filepath;
		static Dictionary<string, string> record = new Dictionary<string, string>();
		/// <summary>
		/// 从Last-Modified.xml加载记录
		/// </summary>
		static public void Load()
		{
			filepath = Settings.Current.CacheFolder + "\\Last-Modified.xml";
			try
			{
				if (File.Exists(filepath))
					_ReadFile();
			}
			catch (Exception ex)
			{
				Log.Exception(ex.InnerException, ex, "读取Last-Modified.xml时发生异常");
			}
		}
		/// <summary>
		/// 保存到Last-Modified.xml
		/// </summary>
		static public void Save()
		{
			record = record.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);

			filepath = Settings.Current.CacheFolder + "\\Last-Modified.xml";

			if (File.Exists(filepath))
				File.Delete(filepath);
			try
			{
				_WriteFile();
			}
			catch (Exception ex)
			{
				Log.Exception(ex.InnerException, ex, "保存Last-Modified.xml时发生异常");
				throw ex;
			}
		}

		/// <summary>
		/// 返回文件最后更改日期
		/// </summary>
		/// <param name="uri">文件对应的url</param>
		/// <param name="time">最后修改日期</param>
		/// <returns>1 - 返回日期；0 - 无日期记录；-1 - 文件类型不需要检测</returns>
		static public int GetFileLastTime(Uri uri, out string time)
		{
			time = "";

			if (!uri.AbsolutePath.EndsWith(".swf"))
				//只有swf才需要检查修改时间
				return -1;

			if (record.TryGetValue(uri.AbsolutePath, out time))
				return 1;

			return 0;
		}

		static public void Add(Uri uri, string time)
		{
			if (!uri.AbsolutePath.EndsWith(".swf"))
				return;

			if (record.ContainsKey(uri.AbsolutePath))
				record[uri.AbsolutePath] = time;

			else
				record.Add(uri.AbsolutePath, time);

		}


		static void _ReadFile()
		{
			using (XmlReader reader = XmlReader.Create(filepath))
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element &&
						reader.Name == "FilePath")
					{
						var time = reader["LastModified"];

						if (reader.Read())
						{
							record.Add(reader.Value, time);
						}
					}
				}
			}
		}

		static void _WriteFile()
		{
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("\t");
			settings.NewLineOnAttributes = true;

			using (XmlWriter writer = XmlWriter.Create(filepath, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("CacheFilesLastModifiedTime");

				foreach (var file in record.Keys)
				{
					writer.WriteStartElement("FilePath");
					writer.WriteAttributeString("LastModified", record[file]);
					writer.WriteString(file);
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteComment(DateTime.Now.ToString() + "\tby 风飏");
				writer.WriteEndDocument();
			}
		}
	}
	*/
	#endregion


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
			filepath = Settings.Current.CacheFolder + "\\Last-Modified.xml";
			try
			{
				if (!File.Exists(filepath))
					return;

				fileXML = XDocument.Load(filepath);
			}
			catch (Exception ex)
			{
				Log.Exception(ex.InnerException, ex, "加载Last-Modified.xml时发生异常");
			}

			if (fileXML == null)
			{
				fileXML = new XDocument();
				fileXML.Add(new XElement(_RootName));
			}

			recordList = fileXML.Root.Elements();
		}

		/// <summary>
		/// 保存到Last-Modified.xml
		/// </summary>
		static public void Save()
		{
			try
			{

				/*	试图排序
				//var elms0 = fileXML.Root.Elements();
				//Log.Note("fileXML.Root.Elements()：	" + elms0.Count().ToString());
				//Log.Note("fileXML.Root.Elements()[0]：	" + elms0.First().Name);
				//Log.Note(elms0.First().Name + (elms0.First().Name.ToString() == _ItemElm ? "==" : "!=") + _ItemElm);

				//var elms1 = fileXML.Root.Elements(_ItemElm);
				//Log.Note("fileXML.Root.Elements(_ItemElm)：	" + elms1.Count().ToString());

				//var elms3 = fileXML.Root.Elements("Record");
				//Log.Note("fileXML.Root.Elements(Record)：	" + elms1.Count().ToString());


				var elms2 = fileXML.Root.Elements().Where(elm => elm.Name.ToString() == _ItemElm);
				Log.Note("fileXML.Root.Elements().Where(_ItemElm)：	" + elms2.Count().ToString());

				
				//try 
				//{
					var elms4 = elms2.OrderBy(elm => elm.Element(_ElmPath).Value);
					Log.Note("fileXML.Root.Elements().Where(_ItemElm).OrderBy()：	" + elms4.Count().ToString());
					var elms = elms2.ToArray();

					fileXML.Root.Elements().Remove();
					fileXML.Root.Add(elms);
				//}
				//catch (Exception ex)
				//{
				//	Log.Note("try1 error");
				//	Log.Exception(ex.InnerException, ex, "保存Last-Modified.xml时发生异常");
				//}

				*/

				fileXML.Save(filepath);
			}
			catch (Exception ex)
			{
				Log.Exception(ex.InnerException, ex, "保存Last-Modified.xml时发生异常");
			}
		}
#endregion



		/// <summary>
		/// 添加记录
		/// </summary>
		/// <param name="uri">文件对应的uri</param>
		/// <param name="time">修改时间</param>
		/// <param name="version">文件版本</param>
		static public void Add(Uri uri, string time)
		{
			string path = uri.AbsolutePath;
			if (!path.EndsWith(".swf"))
				return;

			string version = _GetVersionFromUri(uri);

			XElement elm;
			if (GetRecord(uri, out elm) > 0)
			{
				elm.SetElementValue(_ElmTime, time);
				elm.SetElementValue(_ElmVersion, version);
			}
			else
			{
				fileXML.Root.Add(new XElement(_ItemElm,
						new XElement(_ElmPath, uri.AbsolutePath),
						new XElement(_ElmTime, time),
						new XElement(_ElmVersion, version)
						)
					);
				recordList = fileXML.Root.Elements();
			}
		}


		/// <summary>
		/// 获取文件记录
		/// </summary>
		/// <param name="uri">文件对应的uri</param>
		/// <param name="time">修改时间</param>
		/// <param name="version">文件版本</param>
		/// <returns></returns>
		static public int GetRecord(Uri uri, out XElement element)
		{
			element = null;
			
			foreach (XElement elm in recordList)
			{
				if (elm.Name == _ItemElm &&
					elm.Element(_ElmPath) != null &&
					elm.Element(_ElmPath).Value == uri.AbsolutePath)
				{
					element = elm;

					return 1;
				}
			}

			return 0;
		}

		/// <summary>
		/// 返回文件最后更改日期
		/// </summary>
		/// <param name="uri">文件对应的url</param>
		/// <param name="time">最后修改日期</param>
		/// <returns>1 - 返回日期；0 - 无日期记录；-1 - 文件类型不需要检测</returns>
		static public int GetFileLastTime(Uri uri, out string time)
		{
			Log.Note("【GetFileLastTime】", uri.AbsolutePath);
			time = "";
			string version = "";
			if (!uri.AbsolutePath.EndsWith(".swf"))
				//只有swf才需要检查修改时间
				return -1;

			XElement elm;

			Log.Note("【GetFileLastTime】 311");

			if (GetRecord(uri, out elm) <= 0)
				return 0;

			Log.Note("【GetFileLastTime】 316");

			time = elm.Element(_ElmTime) != null ?
				elm.Element(_ElmTime).Value :
				"";
			version = elm.Element(_ElmVersion) != null ?
				elm.Element(_ElmVersion).Value :
				"";

			string queryVer = _GetVersionFromUri(uri);
			Log.Note("【比对版本】version ? queryVer", version + (version == queryVer? "==": "!=") + queryVer);

			if (!string.IsNullOrEmpty(queryVer) && version != queryVer)
			{
				//版本记录不符合 -> 返回无记录

				return 0;
			}

			return 1;
		}


		static string _GetVersionFromUri(Uri uri)
		{
			var query = uri.Query.ToLower();
			var pos = query.IndexOf("version=");
			if (pos < 0)
				return "";

			return query.Substring(pos + 8);
		}
	}
}
