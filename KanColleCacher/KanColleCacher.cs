using Grabacr07.KanColleViewer.Composition;
using System.ComponentModel.Composition;
using Debug = System.Diagnostics.Debug;
using File = System.IO.File;

namespace d_f_32.KanColleCacher
{
	[Export(typeof(IToolPlugin))]
	[ExportMetadata("Title", "KanColleCacher")]
	[ExportMetadata("Description", "通过创建本地缓存以加快游戏加载速度（并支持魔改）")]
	[ExportMetadata("Version", "1.3.0.0")]
	[ExportMetadata("Author", "d.f.32")]
	public class KanColleCacher : IToolPlugin
    {
		const string name = "缓存工具";
		static bool isInitialized = false;
		static CacherToolView view;

		static public void Initialize()
		{
			if (isInitialized) return;
			isInitialized = true;

#if DEBUG
			Debug.WriteLine(@"
KanColleCacher
=================================================
CACHR>	初始化开始：{0}
", System.DateTime.Now);
#endif
			
			Settings.Load();
			view = new CacherToolView();

			Debug.WriteLine(@"CACHR>	GraphList加入规则");

			//只有当列表文件不存在时才打印列表
			if (Settings.Current.SaveApiStart2 &&
					!File.Exists(Settings.Current.CacheFolder + "\\api_start2.dat"))
			{
				GraphList.AppendRule();
			}
			Debug.WriteLine(@"CACHR>	GraphList加入规则完成");

			Debug.WriteLine(@"CACHR>	Fiddler初始化开始");
			FiddlerRules.Initialize();
			Debug.WriteLine(@"CACHR>	Fiddler初始化完成");

			Debug.WriteLine(@"CACHR>	初始化完成");
		}

		~KanColleCacher()
		{
			Settings.Save();
			Debug.Flush();
		}

		public string ToolName
		{
			get { return name; }
		}

		public object GetToolView()
		{
			return view;
		}

		public object GetSettingsView()
		{
			return null;
		}
	}


	[Export(typeof(INotifier))]
	[ExportMetadata("Title", "KanColleCacher")]
	[ExportMetadata("Description", "通过创建本地缓存以加快游戏加载速度（并支持魔改）")]
	[ExportMetadata("Version", "1.3.0.0")]
	[ExportMetadata("Author", "d.f.32")]
	public class KanColleCacher_Initializer : INotifier
	{
		public void Initialize()
		{
			KanColleCacher.Initialize();
		}

		public void Dispose()
		{
		}

		public object GetSettingsView()
		{
			return null;
		}

		public void Show(NotifyType type, string header, string body, System.Action activated, System.Action<System.Exception> failed = null)
		{
		}
	}
}
