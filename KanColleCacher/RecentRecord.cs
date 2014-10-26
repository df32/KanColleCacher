using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d_f_32.KanColleCacher
{
	static class TaskRecord
	{
		static Dictionary<string, string> record = new Dictionary<string, string>();
		//KEY: url, Value: filepath
		//只有在验证文件修改时间后，向客户端返回本地文件或者将文件保存到本地时才需要使用
		
		static public void Add(string url, string filepath)
		{
			if (record.ContainsKey(url))
				record[url] = filepath;
			else
				record.Add(url, filepath);
		}

		static public string GetAndRemove(string url)
		{
			string ret = Get(url);
			record.Remove(url);
			return ret;
		}
		static public string Get(string url)
		{
			if (record.ContainsKey(url))
				return record[url];
			return "";
		}
	}

	#region 废弃的代码
	//static class RecentRecord
	//{
	//	class RecordItem
	//	{
	//		public string   url     = "";
	//		public filetype type    = filetype.not_file;
	//		public string   result  = "";
	//		public int      state   = 0;
	//		//0 - 无记录；1 - 有效缓存；-1 - 需要更新的缓存

	//		public RecordItem (string url, filetype type, string result, int state)
	//		{
	//			this.url = url;
	//			this.type = type;
	//			this.result = result;
	//			this.state = state;
	//		}
	//	}

	//	static List<RecordItem> list = new List<RecordItem>();

	//	static public void Add(string url, filetype type, string result, int state)
	//	{
	//		if (list.Count > 20)
	//			list.RemoveAt(0);

	//		list.Add(new RecordItem(url, type, result, state));
	//	}

	//	static public int Get(string url, out filetype type, out string result)
	//	{
	//		var item = list.Find(x => x.url == url);
	//		if (item != null)
	//		{
	//			type = item.type;
	//			result = item.result;
	//			return item.state;
	//		}

	//		type = filetype.not_file;
	//		result = "";
	//		return 0;
	//	}

	//}
	#endregion 
}
