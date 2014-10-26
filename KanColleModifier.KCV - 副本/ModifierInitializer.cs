using Grabacr07.KanColleViewer.Composition;
using System.ComponentModel.Composition;

namespace Gizeta.KanColleModifier.KCV
{
    [Export(typeof(INotifier))]
    [ExportMetadata("Title", "ModifierInitializer")]
    [ExportMetadata("Description", "非Notifier，仅供KanColleModifier.KCV提前加载初始化使用。")]
    [ExportMetadata("Version", "1.0")]
    [ExportMetadata("Author", "@Gizeta")]
    public class ModifierInitializer : INotifier
    {
        public void Initialize()
        {
            ModifierView.Initialize();
        }

        public void Show(NotifyType type, string header, string body, System.Action activated, System.Action<System.Exception> failed = null)
        {
        }

        public object GetSettingsView()
        {
            return null;
        }

        public void Dispose()
        {
        }
    }
}
