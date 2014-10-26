using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Grabacr07.KanColleViewer.Models.Data.Xml;

namespace d_f_32.KanColleCacher
{
    class ModifiedRecord
    {
        static string filepath;
        static Dictionary<string, string> record = new Dictionary<string,string>();
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

                        if(reader.Read())
                        {
                            record.Add(reader.Value,time);
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

}
