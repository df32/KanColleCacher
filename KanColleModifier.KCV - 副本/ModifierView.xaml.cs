using Fiddler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gizeta.KanColleModifier.KCV
{
    /// <summary>
    /// ModifierView.xaml 的交互逻辑
    /// </summary>
    public partial class ModifierView : UserControl
    {
        private static bool modifierOn = false;
        private static bool hasInitialize = false;
        private static Dictionary<string, string> data = new Dictionary<string, string>();

        public ModifierView()
        {
            InitializeComponent();
            readModifierData();
            showList();
            setModifier();
            toggle(modifierOn);
        }

        public static void Initialize()
        {
            if(File.Exists("modifier.enable"))
            {
                modifierOn = true;
                readModifierData();
                setModifier();
            }
        }

        private static void setModifier()
        {
            if (!hasInitialize)
            {
                hasInitialize = true;
                FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
                FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            }
        }

        private static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (modifierOn && oSession.fullUrl.IndexOf("/kcs/resources/swf/ships/") >= 0)
            {
                var tmp1 = oSession.fullUrl.Split('/');
                var tmp2 = tmp1.Last().Split('.');
                if (tmp2.Length >= 1)
                {
                    if (data.ContainsKey(tmp2[0]))
                    {
                        oSession.utilDecodeResponse();
                        oSession.ResponseBody = File.ReadAllBytes(data[tmp2[0]]);
                        oSession.oResponse.headers.HTTPResponseCode = 200;
                        oSession.oResponse.headers.HTTPResponseStatus = "200 OK";
                    }
                }
            }
        }

        private static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (modifierOn && oSession.fullUrl.IndexOf("/kcs/resources/swf/ships/") >= 0)
            {
                var tmp1 = oSession.fullUrl.Split('/');
                var tmp2 = tmp1.Last().Split('.');
                if (tmp2.Length >= 1)
                {
                    if (data.ContainsKey(tmp2[0]))
                    {
                        oSession.bBufferResponse = true;
                    }
                }
            }
        }

        private static void readModifierData()
        {
            if(File.Exists("魔改.txt"))
            {
                data.Clear();
                var file = File.Open("魔改.txt", FileMode.Open);
                using (var stream = new StreamReader(file))
                {
                    while (!stream.EndOfStream)
                    {
                        var str = stream.ReadLine();
                        var st = str.LastIndexOf('\\') + 1;
                        var ed = str.LastIndexOf(".hack.swf");
                        if (st > 0 && ed > 0)
                        {
                            if (File.Exists(str))
                            {
                                data.Add(str.Substring(st, ed - st), str);
                            }
                        }
                    }
                }
                file.Close();
            }
            else
            {
                modifierOn = false;
            }
        }

        private void showList()
        {
            Modifier_List.Text = "魔改文件：";
            foreach (var d in data)
            {
                Modifier_List.Text += "\r\n" + d.Value;
            }
        }

        private void Modifier_Switch_Click(object sender, RoutedEventArgs e)
        {
            toggle(Modifier_Switch.Content.ToString() == "开启魔改");
        }

        private void Modifier_UpdateList_Click(object sender, RoutedEventArgs e)
        {
            readModifierData();
            showList();
        }

        private void toggle(bool on)
        {
            if(on)
            {
                modifierOn = true;
                Modifier_Switch.Content = "关闭魔改";
                if(!File.Exists("modifier.enable"))
                {
                    File.Create("modifier.enable").Close();
                }
            }
            else
            {
                modifierOn = false;
                Modifier_Switch.Content = "开启魔改";
                if (File.Exists("modifier.enable"))
                {
                    File.Delete("modifier.enable");
                }
            }
        }
    }
}
