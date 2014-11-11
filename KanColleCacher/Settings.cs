using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Models.Data.Xml;
using Grabacr07.KanColleViewer;
using Livet;
using Debug = System.Diagnostics.Debug;

namespace d_f_32.KanColleCacher
{
    [Serializable]
    public class Settings :NotificationObject
    {
        private static string filePath;

        public static Settings Current { get; private set; }

        /// <summary>
        /// 加载插件设置
        /// </summary>
        public static void Load()
        {
			var switch_on = 0;

SwitchCase:
			switch (switch_on)
			{
				case 0:
					goto SetPath1;

				//第一次读取失败
				case 1:
					goto SetPath2;

				//第二次读取失败
				case 2:
					goto Failed;
			}
goto Failed;


SetPath1:
			filePath = Directory.GetCurrentDirectory() + @"\Plugins\KanColleCacher.xml";

switch_on = 1;
goto ReadFile;


SetPath2:
			filePath = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						"grabacr.net",
						"KanColleViewer",
						"KanColleCacher.xml"
						);
switch_on = 2;
goto ReadFile;


ReadFile:
			if (File.Exists(filePath))
			{
				try
				{
					Current = filePath.ReadXml<Settings>();
					goto Succeed;
				}
				catch (Exception ex)
				{
					Log.Exception(ex.InnerException, ex, "读取设置文件时出现异常");
				}
			}
goto SwitchCase;
			

Failed:
			Current = new Settings();
return;


Succeed:
			if (!Directory.Exists(Current.CacheFolder))
			{
				try
				{
					Directory.CreateDirectory(Current.CacheFolder);
				}
				catch (Exception ex)
				{
					Current.CacheFolder = Directory.GetCurrentDirectory() + @"\MyCache";
					Log.Exception(ex.InnerException, ex, "设置文件中CacheFolder不存在，试图创建时发生异常");
				}
			}
return;
        }
        
        /// <summary>
        /// 保存设置
        /// </summary>
        public static void Save()
        {
            try
            {
                Current.WriteXml(filePath);
            }
            catch (Exception ex)
            {
				Log.Exception(ex.InnerException, ex, "保存设置文件时出现异常");
            }
        }

        
        public Settings ()
        {       
                _CacheFolder = Directory.GetCurrentDirectory() + @"\MyCache";
                _CacheEnabled = true;
                _HackEnabled = true;
                _HackTitleEnabled = true;

                _CacheEntryFiles = 2;
                _CachePortFiles = 2;
                _CacheSceneFiles = 2;
                _CacheResourceFiles = 2;
                _CacheSoundFiles = 2;

				_CheckFiles = 1;
				_PrintGraphList = true;
         }
       


        private string _CacheFolder;
		/// <summary>
		/// 缓存文件夹
		/// </summary>
        public string CacheFolder
        {
            get { return this._CacheFolder; }
            set
            {
                if (this._CacheFolder != value)
                {
                    this._CacheFolder = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _CacheEnabled;
		/// <summary>
		/// 启用缓存功能
		/// </summary>
        public bool CacheEnabled
        {
            get { return this._CacheEnabled; }
            set
            {
                if (this._CacheEnabled != value)
                {
                    this._CacheEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackEnabled;
		/// <summary>
		/// 启用Hack规则
		/// </summary>
        public bool HackEnabled
        {
            get { return this._HackEnabled; }
            set
            {
                if (this._HackEnabled != value)
                {
                    this._HackEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _HackTitleEnabled;
		/// <summary>
		/// 启用针对TitleCall与WorldName的特殊规则
		/// </summary>
        public bool HackTitleEnabled
        {
            get { return this._HackTitleEnabled; }
            set
            {
                if (this._HackTitleEnabled != value)
                {
                    this._HackTitleEnabled = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheEntryFiles;
        public int CacheEntryFiles
        {
            get { return this._CacheEntryFiles; }
            set
            {
                if (this._CacheEntryFiles != value)
                {
                    this._CacheEntryFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CachePortFiles;
        public int CachePortFiles
        {
            get { return this._CachePortFiles; }
            set
            {
                if (this._CachePortFiles != value)
                {
                    this._CachePortFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheSceneFiles;
        public int CacheSceneFiles
        {
            get { return this._CacheSceneFiles; }
            set
            {
                if (this._CacheSceneFiles != value)
                {
                    this._CacheSceneFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheResourceFiles;
        public int CacheResourceFiles
        {
            get { return this._CacheResourceFiles; }
            set
            {
                if (this._CacheResourceFiles != value)
                {
                    this._CacheResourceFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private int _CacheSoundFiles;
        public int CacheSoundFiles
        {
            get { return this._CacheSoundFiles; }
            set
            {
                if (this._CacheSoundFiles != value)
                {
                    this._CacheSoundFiles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

		private int _CheckFiles;
		/// <summary>
		/// 向服务器发送文件验证请求
		/// 0 - 不验证；1 - 不验证资源SWF文件；2 - 验证所有SWF文件
		/// 文件验证会延长文件加载时间
		/// </summary>
		public int CheckFiles
		{
			get { return this._CheckFiles; }
			set
			{
				if (this._CheckFiles != value)
				{
					this._CheckFiles = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private bool _PrintGraphList;
		/// <summary>
		/// 输出舰娘立绘的文件名列表
		/// 只有当缓存文件夹中的GraphList.txt不存在时才输出
		/// 只在游戏加载时有效
		/// </summary>
		public bool PrintGraphList
		{
			get { return this._PrintGraphList; }
			set
			{
				if (this._PrintGraphList != value)
				{
					this._PrintGraphList = value;
					this.RaisePropertyChanged();
				}
			}
		}
    }
}
