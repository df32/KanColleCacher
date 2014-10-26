using Grabacr07.KanColleViewer.Composition;
using System.ComponentModel.Composition;

namespace d_f_32.KanColleCacher
{
	[Export(typeof(IToolPlugin))]
	[ExportMetadata("Title", "KanColleCacher")]
	[ExportMetadata("Description", "将游戏资源缓存到指定文件夹，以加快游戏加载速度。")]
	[ExportMetadata("Version", "1.0.0.1")]
	[ExportMetadata("Author", "d.f.32")]
	public class KanColleCacher : IToolPlugin
    {
		static CacherToolView view;
		const string name = "缓存工具";
		
        static public void Initialize()
        {
			//Log.Note("-> Initialize()", System.DateTime.Now.ToString());
			Settings.Load();
			view = new CacherToolView();
			FiddlerRules.Initialize();
        }

		~KanColleCacher()
		{
			Settings.Save();
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
	[ExportMetadata("Description", "将游戏资源缓存到指定文件夹，以加快游戏加载速度。")]
	[ExportMetadata("Version", "1.0.0.1")]
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
