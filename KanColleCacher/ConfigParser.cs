using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Reflection;
using System.ComponentModel.Composition;

namespace d_f_32.KanColleCacher.Configuration
{
	/// <summary>
	/// 表示一个INI文档节点
	/// </summary>
	public class Section
	{
		readonly Dictionary<string, string> Options = new Dictionary<string,string>();
		readonly Dictionary<string, string> Comments = new Dictionary<string,string>();

		#region 通过索引访问
		/// <summary>
		/// 获取或设置指定指定的选项的选项值。选项名称不区分大小写。
		/// </summary>
		/// <param name="section">选项的名称</param>
		/// <returns>指定的选项。如果找不到指定选项，get 将返回null，set 将创建指定值的选项</returns>
		public string this[string option]
		{
			get
			{
				foreach (var pair in Options)
				{
					if (String.Compare(pair.Key, option, true) == 0)
					{
						return pair.Value;
					}
				}
				return null;
			}
			set
			{
				foreach (var key in Options.Keys)
				{
					if (String.Compare(key, option, true) == 0)
					{
						if (value == null)
						{
							Options.Remove(key);
							//删除注释
						}
						else
						{
							Options[key] = value;
						}
						return;
					}
				}
				if (value != null)
					Options.Add(option, value);
			}
		}

		#endregion

		#region 注释操作

		/// <summary>
		/// 添加指定选项后的注释。空值选项表示节点注释。注释将自动换行。
		/// </summary>
		/// <param name="option">选项名</param>
		/// <param name="comment">注释</param>
		public void AddComment(string option, string comment)
		{
			foreach (var key in Comments.Keys)
			{
				if (String.Compare(key, option, true) == 0)
				{
					Comments[key] += "\r\n" + comment;
					return;
				}
			}
			Comments.Add(option.ToLower(), comment);
		}
		public void SetComment(string option, string comment)
		{
			Comments[option.ToLower()] = comment;
		}

		#endregion

		#region 文本序列化
		/// <summary>
		/// 将节点内容序列化为文本
		/// </summary>
		/// <returns>表示节点内容的文本</returns>
		public override string ToString()
		{
			StringBuilder contents = new StringBuilder();
			
			//节点注释
			if (Comments.ContainsKey(""))
			{
				contents.AppendLine(Comments[""]);
			}

			foreach (var pairs in Options)
			{
				contents.AppendFormat(@"{0}={1}", pairs.Key, pairs.Value);
				contents.AppendLine();

				var key = pairs.Key.ToLower();
				if (Comments.ContainsKey(key))
				{
					contents.AppendLine(Comments[key]);
				}
			}
			return contents.ToString(0, contents.Length -2);
		}

		#endregion

		#region 枚举访问

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return Options.GetEnumerator();
		}

		#endregion 枚举访问
	}


	#region  DynamicSection
	//public class Section : DynamicObject
	//{
	//	readonly Dictionary<string, string> Options = new Dictionary<string, string>();
	//	readonly Dictionary<string, string> Comments = new Dictionary<string, string>();

	//	#region [公开]文本解析

	//	////将一行文本解析并添加到当前节点中
	//	//string ParseLine(string line, string lastOption)
	//	//{
	//	//	line = line.Trim();
	//	//	int pos = line.IndexOf('=');

	//	//	//当前行是 Option=Value
	//	//	if (pos > 0)
	//	//	{
	//	//		string option = line.Substring(0, pos).TrimEnd();
	//	//		string value = line.Substring(pos + 1).TrimStart();
	//	//		_TrySetValue(option, value);
	//	//		return option;
	//	//	}
	//	//	//当前行是 无效内容
	//	//	else
	//	//	{
	//	//		AddComment(lastOption, line);
	//	//		return "";
	//	//	}
	//	//}

	//	////将多行文本解析并添加到当前节点中
	//	//public void Parse(string[] lines)
	//	//{
	//	//	string curOption = "";

	//	//	foreach (var line in lines)
	//	//	{
	//	//		curOption = ParseLine(line, curOption);
	//	//	}
	//	//}

	//	////将一段文本解析并添加到当前节点中
	//	//public void Parse(string content)
	//	//{
	//	//	string curOption = "";

	//	//	using (var reader = new StringReader(content))
	//	//	{
	//	//		string line = reader.ReadLine();
	//	//		while (line != null)
	//	//		{
	//	//			curOption = ParseLine(line, curOption);
	//	//			line = reader.ReadLine();
	//	//		}
	//	//	}
	//	//}

	//	#endregion

	//	#region [私有][基础]对选项值的操作【类型转换】【不支持List<string>】
	//	//获取选项值。不区分大小写。同时进行转换。同时使用ConvertValue()转换
	//	//选项存在返回True；不存在返回False
	//	//不存在时结果为null
	//	bool _TryGetValue(string option, out object result, Type type)
	//	{
	//		if (_TryGetValue(option, out result))
	//		{
	//			result = (object)ConvertValue((string)result, type);
	//			return true;
	//		}
	//		return false;
	//	}
	//	bool _TryGetValue(string option, out object result)
	//	{
	//		foreach (var pair in Options)
	//		{
	//			if (String.Compare(pair.Key, option, true) == 0)
	//			{
	//				result = pair.Value;
	//				return true;
	//			}
	//		}
	//		result = null;
	//		return false;
	//	}


	//	//设置选项值。不区分大小写。同时使用ConvertBackValue()转换
	//	//若值为null则删除选项。
	//	//始终返回True
	//	bool _TrySetValue(string option, object value)
	//	{
	//		foreach (var key in Options.Keys)
	//		{
	//			if (String.Compare(key, option, true) == 0)
	//			{
	//				if (value == null)
	//				{
	//					Options.Remove(key);
	//					//删除注释
	//				}
	//				else
	//				{
	//					Options[key] = ConvertBackValue(value);
	//				}
	//				return true;
	//			}
	//		}
	//		if (value != null)
	//			Options.Add(option, ConvertBackValue(value));

	//		return true;
	//	}

	//	#endregion

	//	#region [动态]对成员的动态访问【类型转换】【List<string>】

	//	/// <summary>
	//	/// 获取成员。不区分大小写。只允许值类型和List(string)
	//	/// </summary>
	//	/// <param name="binder"></param>
	//	/// <param name="result">若成员不存在，则返回null。（List除外）</param>
	//	/// <returns>始终返回真</returns>
	//	public override bool TryGetMember(GetMemberBinder binder, out object result)
	//	{
	//		return TryGetMember(binder.Name, binder.ReturnType, out result);
	//	}
	//	public bool TryGetMember(string name, Type type, out object result)
	//	{
	//		if (type == typeof(List<string>))
	//		{
	//			result = getListValue<string>(name);
	//			return true;
	//		}
	//		_TryGetValue(name, out result, type);
	//		return true;
	//	}

	//	/// <summary>
	//	/// 设置成员。若成员不存在，则创建成员。不区分大小写。只允许值类型和List(string)
	//	/// </summary>
	//	/// <param name="value">若值为null，则删除成员。（List除外）</param>
	//	/// <returns>始终返回真</returns>
	//	public override bool TrySetMember(SetMemberBinder binder, object value)
	//	{
	//		if (value.GetType() == typeof(List<string>))
	//		{
	//			setListValue<string>(binder.Name, (List<string>)value);
	//			return true;
	//		}
	//		_TrySetValue(binder.Name, value);
	//		return true;
	//	}

	//	#endregion

	//	#region [索引]通过索引访问【Get不转换】【Set转换】【不支持List<string>】
	//	/// <summary>
	//	/// 获取或设置指定指定的选项的选项值。选项名称不区分大小写。
	//	/// </summary>
	//	/// <param name="section">选项的名称</param>
	//	/// <returns>指定的选项。如果找不到指定选项，get 将返回null，set 将创建指定值的选项</returns>
	//	public string this[string option]
	//	{
	//		get
	//		{
	//			object result;
	//			if (_TryGetValue(option, out result))
	//				return result.ToString();
	//			return null;

	//		}
	//		set
	//		{
	//			_TrySetValue(option, value);
	//		}
	//	}

	//	#endregion

	//	#region 对象转换与序列化

	//	//Section转换为对象（dynamic Section 且 显式转换）对象只能是string或class
	//	public override bool TryConvert(ConvertBinder binder, out object result)
	//	{
	//		if (binder.Type == typeof(string))
	//		{
	//			result = ToString();
	//			return true;
	//		}
	//		else if (binder.Explicit && binder.Type.IsClass)
	//		{
	//			//只序列化可写的公共属性
	//			result = DeserializeObject(binder.Type);
	//			return true;
	//		}
	//		return base.TryConvert(binder, out result);
	//	}
	//	//Section -> Object
	//	public T DeserializeObject<T>()
	//	{
	//		return (T)DeserializeObject(typeof(T));
	//	}
	//	//Object -> Section
	//	public void SerializeObject<T>(T obj)
	//	{
	//		SerializeObject(obj, typeof(T));
	//	}


	//	//对象序列化到Section，【公有属性】【值类型】【List<string>】
	//	void SerializeObject(object obj, Type type)
	//	{
	//		foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
	//			.Where(p => p.CanRead))
	//		{
	//			if (pi.PropertyType == typeof(List<string>))
	//			{
	//				setListValue<string>(pi.Name, (List<string>)pi.GetValue(obj));
	//			}
	//			else //if (pi.PropertyType.IsValueType)
	//			{
	//				_TrySetValue(pi.Name, pi.GetValue(obj));
	//			}

	//			//检查注释
	//			foreach (Attribute attr in Attribute.GetCustomAttributes(pi))
	//			{
	//				if (attr.GetType() == typeof(ExportMetadataAttribute))
	//				{
	//					var meta = (ExportMetadataAttribute)attr;
	//					if (meta.Name.ToLower() == "description"
	//						|| meta.Name.ToLower() == "comment")
	//					{
	//						SetComment(pi.Name, "; " + meta.Value.ToString());
	//					}
	//				}
	//			}
	//		}
	//	}

	//	//从Section反序列化为对象【公有属性】【值类型】【List<string>】
	//	object DeserializeObject(Type targetType)
	//	{
	//		var ret = Activator.CreateInstance(targetType);
	//		foreach (var pi in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
	//			.Where(p => p.CanWrite))
	//		{
	//			if (pi.PropertyType == typeof(List<string>))
	//			{
	//				pi.SetValue(ret, (getListValue<string>(pi.Name)));
	//			}
	//			else //if (pi.PropertyType.IsValueType)
	//			{
	//				object optValue;
	//				if (_TryGetValue(pi.Name, out optValue, pi.PropertyType))
	//				{
	//					pi.SetValue(ret, optValue);
	//				} //若没有该选项，则不赋值
	//			}
	//		}
	//		return ret;
	//	}

	//	#endregion

	//	#region List<string>的处理

	//	//返回option, option2, option3的列表选项值
	//	List<T> getListValue<T>(string option)
	//	{
	//		var ret = new List<T>();
	//		object val;
	//		string key = option;
	//		int idx = 1;
	//		while (_TryGetValue(key, out val))
	//		{
	//			ret.Add(ConvertValue((string)val, typeof(T)));
	//			idx++;
	//			key = option + idx;
	//		}
	//		return ret;
	//	}

	//	//从列表设置选项
	//	void setListValue<T>(string option, List<T> list)
	//	{
	//		int idx = 1;
	//		string key = option;
	//		foreach (var value in list)
	//		{
	//			//_TrySetValue(key, ConvertBackValue(value));
	//			//_TrySetValue已经使用了ConvertBackValue
	//			_TrySetValue(key, value);
	//			idx++;
	//			key = option + idx;
	//		}
	//	}

	//	#endregion

	//	#region [基础][静态]值类型转换
	//	//选项值类型转换。 （string -> ?）
	//	static dynamic ConvertValue(string value, Type type)
	//	{
	//		if (type == typeof(string))
	//			return value;

	//		else if (type == typeof(int))
	//			return ConvertInt(value);

	//		else if (type == typeof(bool))
	//			return ConvertBool(value);

	//		return Convert.ChangeType(value, type);
	//	}

	//	//转换为选项值类型（? -> string）
	//	static string ConvertBackValue(object value)
	//	{
	//		if (value.GetType() == typeof(bool))
	//			return (bool)value ? "1" : "0";
	//		return value.ToString();
	//	}

	//	//将值转换为Int32
	//	static int ConvertInt(string value)
	//	{
	//		int ret;
	//		if (Int32.TryParse(value, out ret))
	//			return ret;
	//		return default(int);
	//	}

	//	//将值转换为Boolean
	//	static bool ConvertBool(string value)
	//	{
	//		if (String.Compare(value, "True", true) == 0) return true;
	//		if (String.Compare(value, "False", true) == 0) return true;
	//		int ret;
	//		if (Int32.TryParse(value, out ret))
	//			return ret > 0;
	//		return default(bool);
	//	}

	//	#endregion


	//	/// <summary>
	//	/// 添加指定选项后的注释。空值选项表示节点注释。注释将自动换行。
	//	/// </summary>
	//	/// <param name="option">选项名</param>
	//	/// <param name="comment">注释</param>
	//	public void AddComment(string option, string comment)
	//	{
	//		foreach (var key in Comments.Keys)
	//		{
	//			if (String.Compare(key, option, true) == 0)
	//			{
	//				Comments[key] += "\r\n" + comment;
	//				return;
	//			}
	//		}
	//		Comments.Add(option.ToLower(), comment);
	//	}
	//	public void SetComment(string option, string comment)
	//	{
	//		Comments[option.ToLower()] = comment;
	//	}



	//	/// <summary>
	//	/// 将节点内容序列化为文本
	//	/// </summary>
	//	/// <returns>表示节点内容的文本</returns>
	//	public override string ToString()
	//	{
	//		StringBuilder contents = new StringBuilder();

	//		//节点注释
	//		if (Comments.ContainsKey(""))
	//		{
	//			contents.AppendLine(Comments[""]);
	//		}

	//		foreach (var pairs in Options)
	//		{
	//			contents.AppendFormat(@"{0}={1}", pairs.Key, pairs.Value);
	//			contents.AppendLine();

	//			var key = pairs.Key.ToLower();
	//			if (Comments.ContainsKey(key))
	//			{
	//				contents.AppendLine(Comments[key]);
	//			}
	//		}
	//		return contents.ToString(0, contents.Length - 2);
	//	}

	//	#region 枚举访问

	//	public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
	//	{
	//		return Options.GetEnumerator();
	//	}

	//	//System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	//	//{
	//	//	return Options.GetEnumerator();
	//	//}
	//	#endregion 枚举访问
	//}

	#endregion



	/// <summary>
	/// 表示一个INI配置文件
	/// </summary>
	public class ConfigParser
	{
		/// <summary>
		/// 此INI文件的文档注释。
		/// 文档注释位于所有节点的上面。
		/// </summary>
		public string Comment { set; get; }

		Dictionary<string, Section> m_Sections;

		/// <summary>
		/// 获取或设置指定名称的节点。节点名不区分大小写。
		/// </summary>
		/// <param name="section">节点的名称</param>
		/// <returns>指定的节点。如果找不到指定的节点，get 将返回null，set 将创建指定的节点</returns>
		public Section this[string section]
		{
			set
			{
				m_Sections[section] = value;
			}
			get
			{
				foreach (var key in m_Sections.Keys)
				{
					if (String.Compare(key, section, true) == 0)
						return m_Sections[key];
				}
				return null;
			}
		}

		/// <summary>
		/// 获取或设置指定节点的指定选项的值。节点名与选项名不区分大小写。
		/// </summary>
		/// <param name="section">节点的名称</param>
		/// <param name="option">选项的名称</param>
		/// <returns>指定选项值。如果找不到指定的节点或选项，get 将返回null，set 将创建节点或选项</returns>
		public string this[string section, string option]
		{
			set 
			{
				var sec = this[section] = this[section] ?? new Section();
				sec[option] = value;
			}
			get 
			{
				var sec = this[section];
				if (sec != null)
					return sec[option];
				return null;
			}
		}
		
		/// <summary>
		/// Variables 节点。
		/// 声明了在INI文档中可以通过“#变量名#”的形式引用的名为“变量”的特使选项。
		/// 此成员仅用于替换变量的引用，不会被直接打印。
		/// </summary>
		public Section Variables { private set; get; }

		//BuildInVariables

		
		/// <summary>
		/// 初始化<see cref="ConfigParser"/>类的空白新实例。
		/// <para>也可以使用<see cref="ConfigParser.ReadIniFile"/>的静态方法来创建新实例。</para>
		/// </summary>
		public ConfigParser()
		{
			m_Sections = new Dictionary<string,Section>();
			Variables = new Section();
		}

		/// <summary>
		/// 由一个INI文件创建<see cref="ConfigParser"/>类的新实例。
		/// 若文件不存在或无法访问将引发异常。
		/// </summary>
		/// <param name="iniFile">要加载的INI文件地址</param>
		/// <param name="encoding">应用到文件内容的字符编码</param>
		/// <exception cref="FileNotFoundException">INI文件不存在</exception>
		public static ConfigParser ReadIniFile(string iniFile, Encoding encoding = null)
		{
			//检查文件是否存在
			if (!File.Exists(iniFile))
			{
				throw new FileNotFoundException("指定的INI文件不存在。", iniFile);
			}

			ConfigParser parser = new ConfigParser();

			//parser.Parse(File.ReadAllText(iniFile, encoding ?? Encoding.Default));
			parser.Parse(File.ReadAllLines(iniFile, encoding ?? Encoding.Default));

			//设置文档变量【读取文档前设置的变量会丢失
			parser._ResetVariablesProperty();

			return parser;
		}

		#region 解析文本并添加到节点中

		//[基础]将一行文本解析并添加到指定节点中
		string ParseLine(string line, ref Section section, string lastOption)
		{
			line = line.Trim();
			int pos = line.IndexOf('=');

			//当前行是 Option=Value
			if (pos > 0)
			{
				string option = line.Substring(0, pos).TrimEnd();
				string value = line.Substring(pos + 1).TrimStart();
				section[option] = value;
				return option;
			}
			//当前行是 无效内容
			else
			{
				section.AddComment(lastOption, line);
				return lastOption;
			}
		}
		
		/// <summary>
		/// 将一段只包含一个节点内容的文本解析并添加到指定节点。（针对 单节点文档）
		/// </summary>
		/// <param name="content">包含节点内容的文本，不含节点标签</param>
		/// <param name="section">节点名</param>
		public void Parse(string content, string section)
		{
			var curSection = this[section] = this[section] ?? new Section();
			string[] lines = content.Split('\n');
			Parse(lines, curSection, "");
		}

		/// <summary>
		/// 将一段包含多个节点内容的文本解析并添加到相应节点。（针对 多节点文档）
		/// </summary>
		/// <param name="content">包含多个节点内容的文本</param>
		public void Parse(string content)
		{
			string[] lines = content.Split('\n');
			Parse(lines, null, "");
		}

		public void Parse(string[] lines)
		{
			Parse(lines, null, "");
		}

		//[基础]解析多行
		void Parse(string[] lines, Section curSection, string curOption)
		{
			foreach (var li in lines)
			{
				var line = li.Trim();
				//当前行是 [节点] 标记
				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					var section = line.Substring(1, line.Count() - 2);

					curSection = this[section] = this[section] ?? new Section();
					curOption = "";
				}
				else
				{
					//当前行位于 任何节点以外
					if (curSection == null)
					{
						if (String.IsNullOrEmpty(Comment))
							Comment = line;
						else
							Comment += "\r\n" + line;
					}
					//当前行位于 某个节点下
					else
					{
						curOption = ParseLine(line, ref curSection, curOption);
					}
				}
			}
		}

		#endregion

		#region 输出与保存

		/// <summary>
		/// 将INI文档保存并输出。
		/// 若文件无法访问将引发异常。
		/// </summary>
		/// <param name="iniFile">INI文件地址</param>
		/// <param name="encoding">应用到文件的字符编码</param>
		/// <exception cref="DirectoryNoFoundException">必须是已经存在的文件夹</exception>
		public void SaveIniFile(string iniFile, Encoding encoding = null)
		{
			File.WriteAllText(iniFile, Print(), Encoding.Default);
		}

		/// <summary>
		/// 尝试将INI文档保存并输出。返回是否成功。
		/// </summary>
		/// <param name="iniFile">INI文件地址</param>
		/// <param name="encoding">应用到文件的字符编码</param>
		/// <returns>是否成功</returns>
		public bool TrySaveIniFile(string iniFile, Encoding encoding = null)
		{
			try { SaveIniFile(iniFile, encoding); }
			catch { return false; }
			return true;
		}

		/// <summary>
		/// 将INI文档内容序列化为文本。
		/// </summary>
		/// <returns>表示文档内容的文本</returns>
		public string Print()
		{
			StringBuilder contents = new StringBuilder();

			//节点注释
			if (!String.IsNullOrEmpty(Comment))
			{
				contents.AppendLine(Comment);
			}

			foreach (var pairs in m_Sections)
			{
				contents.AppendFormat(
@"[{0}]
{1}
", pairs.Key, pairs.Value.ToString());
			}
			
			return contents.ToString();
		}

		#endregion

		#region 文档变量的读取与替换

		/// <summary>
		/// 将<see cref="ConfigParser.Variables"/>重新设置为<see cref="ConfigParser"/>["Variables"]。
		/// <see cref="ConfigParser.Variables"/>是非公开写入的，以避免被设置为了null。
		/// <para>此方法应当仅被用于<see cref="ConfigParser.ReadIniFile"/>方法中。</para>
		/// </summary>
		public void _ResetVariablesProperty()
		{
			var _vars = this["Variables"];
			if (_vars != null)
				this.Variables = _vars;
		}

		/// <summary>
		/// 获取变量。推荐直接使用 <see cref="ConfigParser.Variables"/>[name] ?? defValue
		/// </summary>
		/// <param name="name">变量名</param>
		/// <param name="defValue">默认值</param>
		/// <returns></returns>
		public string GetVariable(string name,string defValue)
		{
			//if (Variables == null) return defValue;
			//变量应当可以是空白的，在使用#var#引用时替换为""！
			//这点与Rainmeter不同
			return Variables[name] ?? defValue;
		}

		/// <summary>
		/// 替换一段字符串中的所有变量，并返回结果
		/// </summary>
		/// <param name="str">包含变量的字符串</param>
		/// <returns>替换后的字符串</returns>
		public string ReplaceVariables(string str)
		{
			//if (Variables == null) return str;

			if (String.IsNullOrEmpty(str)) return "";
			var spls = str.Split('#');
			var lst = spls.Count() - 1;
			bool isvar = false;			//当前字符串是否为变量
			for (int i = 0; i < lst; i++)
			{
				if (isvar)
				{
					var _key = spls[i].Trim();
					var _val = "";
					isvar = !String.IsNullOrEmpty(_key);
					if (isvar)
					{
						_val = Variables[_key];
						isvar = _val != null;	//变量可以是空的
					}
					if (isvar)
						spls[i] = _val;
					else
						spls[i] = "#" + spls[i];
				}
				isvar = !isvar;
			}
			if (isvar) spls[lst] = "#" + spls[lst];
			return String.Concat(spls);
		}

		#endregion

		#region 读取指定选项的值

		/// <summary>
		/// 读取值类型为<see cref="String"/>的选项。当节点或选项不存在时，返回指定的默认值。
		/// </summary>
		/// <param name="section">节点名</param>
		/// <param name="option">选项名</param>
		/// <param name="defValue">默认值</param>
		/// <param name="replaceVar">是否进行变量的替换</param>
		/// <returns>选项值</returns>
		public string ReadString(string section, string option, string defValue = "", bool replaceVar = false)
		{
			var sec = this[section];
			if (sec != null)
			{
				defValue = sec[option] ?? defValue;
			}
			if (replaceVar) return ReplaceVariables(defValue);

			return defValue;
		}

		/// <summary>
		/// 读取值类型为<see cref="Int32"/>的选项。当节点或选项不存在时，返回指定的默认值。
		/// </summary>
		/// <param name="section">节点名</param>
		/// <param name="option">选项名</param>
		/// <param name="defValue">默认值</param>
		/// <returns>选项值</returns>
		public int ReadInt(string section, string option, int defValue = 0)
		{
			var ret = ReadString(section, option, "");

			int value;
			if (ret != "" && Int32.TryParse(ret, out value))
				return value;

			return defValue;
		}

		/// <summary>
		/// 读取值类型为<see cref="bool"/>的选项。当节点或选项不存在时，返回指定的默认值。
		/// </summary>
		/// <param name="section">节点名</param>
		/// <param name="option">选项名</param>
		/// <param name="defValue">默认值</param>
		/// <returns>选项值</returns>
		public bool ReadBool(string section,string option, bool defValue = false)
		{
			return ReadInt(section, option, defValue ? 1 : 0) > 0 ;
		}

		/// <summary>
		/// 读取像<param name="option"/>, <param name="option"/>2, <param name="option"/>3这样的连续的选项
		/// </summary>
		/// <param name="section">节点名</param>
		/// <param name="option">选项名</param>
		/// <param name="count">最大读取数量（-1代表无限循环）</param>
		/// <returns></returns>
		public List<string> ReadList(string section, string option)
		{
			var sec = this[section];
			if (sec == null) return new List<string>();

			return getListValue<string>(option, sec);
		}
		
		#endregion

		#region 设置指定选项值以及注释
		/// <summary>
		/// 设置指定节点下指定选项的值。操作将自动创建不存在的节点和选项。
		// <para>推荐直接使用<see cref="Section[string]"/>来设置选项，因为后者会自动进行值类型的转换。</para>
		/// </summary>
		/// <param name="section">节点名</param>
		/// <param name="option">选项名</param>
		/// <param name="value">选项值</param>
		public void SetValue(string section, string option, object value)
		{
			var sec = this[section];
			if (sec == null)
				this[section] = sec = new Section();

			sec[option] = ConvertBackValue(value);
		}
		void SetValue(Section section,string option, object value)
		{
			section[option] = ConvertBackValue(value);
		}
		public void SetList<T>(string section, string option, IEnumerable<T> list)
		{
			var sec = this[section];
			if (sec == null)
				this[section] = sec = new Section();

			setListValue<T>(option, list, sec);
		}


		/// <summary>
		/// 添加指定节点下指定选项后的注释。不应当对注释有积极的操作。
		/// </summary>
		/// <param name="section">节点名。将忽略不存在的节点。</param>
		/// <param name="option">选项名。空值表示节点的注释。</param>
		/// <param name="comment">注释。将自动换行。</param>
		public void AddComment(string section, string option, string comment)
		{
			var sec = this[section];
			if ( sec != null)
				sec.AddComment(option, comment);
		}

		#endregion

		#region List<string>的处理

		//返回option, option2, option3的列表选项值
		List<T> getListValue<T>(string option, Section section)
		{
			var ret = new List<T>();
			string key = option;
			string val = section[option];
			int idx = 1;
			while (val != null)
			{
				ret.Add(ConvertValue(val, typeof(T)));
				idx++;
				key = option + idx;
			}
			return ret;
		}

		//从列表设置选项
		void setListValue<T>(string option, IEnumerable<T> list, Section section)
		{
			int idx = 1;
			string key = option;
			foreach (var value in list)
			{
				section[key] = ConvertBackValue(value);
				idx++;
				key = option + idx;
			}
		}

		#endregion

		#region [静态]值类型转换
		//选项值类型转换。 （string -> ?）
		static dynamic ConvertValue(string value, Type type)
		{
			if (type == typeof(string))
				return value;

			else if (type == typeof(int))
				return ConvertInt(value);

			else if (type == typeof(bool))
				return ConvertBool(value);

			return Convert.ChangeType(value, type);
		}

		//转换为选项值类型（? -> string）
		static string ConvertBackValue(object value)
		{
			if (value.GetType() == typeof(bool))
				return (bool)value ? "1" : "0";
			return value.ToString();
		}

		//将值转换为Int32
		static int ConvertInt(string value)
		{
			int ret;
			if (Int32.TryParse(value, out ret))
				return ret;
			return default(int);
		}

		//将值转换为Boolean
		static bool ConvertBool(string value)
		{
			if (String.Compare(value, "True", true) == 0) return true;
			if (String.Compare(value, "False", true) == 0) return true;
			int ret;
			if (Int32.TryParse(value, out ret))
				return ret > 0;
			return default(bool);
		}

		#endregion

		#region 对象转换与序列化


		/// <summary>
		/// Section -> Object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="section"></param>
		/// <returns></returns>
		public T DeserializeObject<T>(string section)where T : new()
		{
			Section sec = this[section];
			T ret = new T();
			
			if (sec != null)
				DeserializeObject(ret, typeof(T), sec);

			return ret;
		}
		/// <summary>
		/// Object -> Section
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="section"></param>
		public void SerializeObject<T>(T obj, string section)
		{
			Section sec = this[section] = this[section] ?? new Section();
			SerializeObject(obj, typeof(T), ref sec);
		}


		//对象序列化到Section，【公有属性】【值类型】【List<string>】
		void SerializeObject(object obj, Type type, ref Section section)
		{
			foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.CanRead))
			{
				if (pi.PropertyType == typeof(List<string>))
				{
					setListValue<string>(pi.Name, (List<string>)pi.GetValue(obj), section);
				}
				else //if (pi.PropertyType.IsValueType)
				{
					section[pi.Name] = ConvertBackValue(pi.GetValue(obj));
				}

				//检查注释
				foreach (Attribute attr in Attribute.GetCustomAttributes(pi))
				{
					if (attr.GetType() == typeof(ExportMetadataAttribute))
					{
						var meta = (ExportMetadataAttribute)attr;
						if (meta.Name.ToLower() == "description"
							|| meta.Name.ToLower() == "comment")
						{
							string buf = meta.Value.ToString();
							if (!buf.StartsWith(";")) buf = "; " + buf;
							section.SetComment(pi.Name, buf);
						}
					}
				}
			}
		}

		//从Section反序列化为对象【公有属性】【值类型】【List<string>】
		void DeserializeObject(object obj, Type targetType, Section section)
		{
			foreach (var pi in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.CanWrite))
			{
				if (pi.PropertyType == typeof(List<string>))
				{
					pi.SetValue(obj, (getListValue<string>(pi.Name,section)));
				}
				else //if (pi.PropertyType.IsValueType)
				{
					string optValue = section[pi.Name];
					if (optValue != null)
					{
						pi.SetValue(obj, ConvertValue(optValue,pi.PropertyType));
					} //若没有该选项，则不赋值
				}
			}
		}
		#endregion

		#region 枚举访问

		public IEnumerator<KeyValuePair<string, Section>> GetEnumerator()
		{
			return m_Sections.GetEnumerator();
		}

		#endregion 枚举访问
	}
}

