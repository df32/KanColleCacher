using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using Livet.Messaging;
//using Livet.Messaging.IO;
//using Grabacr07.KanColleViewer.ViewModels;

namespace d_f_32.KanColleCacher
{
    /// <summary>
    /// ModifierView.xaml 的交互逻辑
    /// </summary>
    public partial class CacherToolView : UserControl
    {
        public CacherToolView()
        {
            try 
            { 
				InitializeComponent(); 
			}
            catch (Exception  ex)
            {
				Log.Exception(ex.Source, ex, "ToolView初始化时发生异常");
            }
			
        }
    }
}
