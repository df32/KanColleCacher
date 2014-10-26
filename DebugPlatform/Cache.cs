using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace d_f_32.KanColleCacher
{

    enum filetype
    {
        not_file,
        unknown_file,

		game_entry,		//{Core.swf, mainD2.swf}
		entry_large,	//{commonAsset, font, TitleMain}
		port_main,		//{PortMain.swf, sound_se}

        scenes,			//kcs\scenes
        resources,		//kcs\resources
		image,			//kcs\resources\images
        sound,			//kcs\sound

		world_name,		//kcs\resources\image\world
		title_call,		//kcs\sound\titlecall
    }


	class Cache
	{
		Settings set;
		string myCacheFolder;

		public Cache()
		{
			set = Settings.Current;
			VersionChecker.Load();
			myCacheFolder = set.CacheFolder;
		}

		~Cache()
		{
			VersionChecker.Save();
		}


		filetype _RecognizeFileType(Uri uri)
		{
			if (!uri.IsFilePath())
				return filetype.not_file;

			var seg = uri.Segments;

			if (seg[1] != "kcs/")
			{
				return filetype.not_file;
			}
			else
			{

				if (seg[2] == "resources/")
				{
					if (seg[3] == "swf/")
					{
						if (seg[4] == "commonAssets.swf" ||
							seg[4] == "font.swf" ||
							seg[4] == "icons.swf")
						{
							return filetype.entry_large;
						}

						else if (seg[4] == ("sound_se.swf"))
						{
							return filetype.port_main;
						}
					}
					else if (seg[3] == "image/")
					{
						if (seg[4] == "world/")
						{
							return filetype.world_name;
						}

						return filetype.image;
					}
					return filetype.resources;
				}
				else if (seg[2] == "scenes/")
				{
					if (seg[3] == "TitleMain.swf")
					{
						return filetype.entry_large;
					}

					return filetype.scenes;
				}
				else if (seg[2] == "sound/")
				{
					if (seg[3] == "titlecall/")
					{
						return filetype.title_call;
					}

					return filetype.sound;
				}
				else
				{
					if (seg[2] == "Core.swf" ||
						seg[2] == "mainD2.swf")
					{
						return filetype.game_entry;
						//  kcs/mainD2.swf; kcs/Core.swf;
					}
					else if (seg[2] == "PortMain.swf")
					{
						return filetype.port_main;
						//  kcs/PortMain.swf;

					}
				}
				//Log.Note("_RecogniseFileType检查到无法识别的文件", uri.AbsolutePath);

				return filetype.unknown_file;
			}

		}

		int _RecentCheckRecoder(string url, filetype type, string result, int state)
		{
			RecentRecord.Add(url, type, result, state);
			return state;
		}



		public void HaveFileSaved(string url)
		{
			Uri uri;
			try { uri = new Uri(url); }
			catch { return; }

			VersionChecker.Update(uri);
		}

		public bool AllowedToSave(filetype type)
		{
			return (type == filetype.resources && set.CacheResourceFiles > 1) ||
					(type == filetype.entry_large && set.CacheEntryFiles > 1) ||
					(type == filetype.port_main && set.CachePortFiles > 1) ||
					(type == filetype.scenes && set.CacheSceneFiles > 1) ||
					(type == filetype.sound && set.CacheSoundFiles > 1) ||
					(type == filetype.title_call || type == filetype.world_name) && set.CacheResourceFiles > 1;
		}

		/// <summary>
		/// 对于一个新的客户端请求，根据url，决定下一步要对请求怎样处理
		/// </summary>
		/// <param name="url">请求的url</param>
		/// <param name="result">本地文件地址 or 记录的修改日期</param>
		/// <returns>下一步我们该做什么</returns>
		public Direction GotNewRequest(uint id, string url, out string result)
		{
			result = "";
			string filepath = "";

			Uri uri;
			try { uri = new Uri(url); }
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
				return Direction.Discharge_Response;
				//url无效，忽略请求（不进行任何操作）
			}

			if (!uri.IsFilePath())
			{
				return Direction.Discharge_Response;
				//url非文件，忽略请求
			}

			//识别文件类型
			filetype type = _RecognizeFileType(uri);
			if (type == filetype.unknown_file ||
				type == filetype.not_file ||
				type == filetype.game_entry)
			{
				return Direction.Discharge_Response;
				//无效的文件，忽略请求
			}

			//检查Title Call与World Name的特殊地址
			if (set.HackTitleEnabled)
			{
				if (type == filetype.title_call)
				{
					filepath = uri.AbsolutePath.Replace('/', '\\');
					filepath = filepath.Remove(filepath.LastIndexOf('\\')) + ".mp3";
					filepath = myCacheFolder + filepath;
					result = filepath;

					if (File.Exists(filepath))
						return Direction.Return_LocalFile;
				}
				else if (type == filetype.world_name)
				{
					filepath = myCacheFolder + @"\kcs\resources\image\world.png";
					result = filepath;

					if (File.Exists(filepath))
						return Direction.Return_LocalFile;
				}
			}

			//检查一般文件地址
			if ((type == filetype.resources && set.CacheResourceFiles > 0)||
                (type == filetype.entry_large && set.CacheEntryFiles > 0) ||
                (type == filetype.port_main && set.CachePortFiles > 0) ||
                (type == filetype.scenes && set.CacheSceneFiles > 0) ||
                (type == filetype.sound && set.CacheSoundFiles > 0) ||
                ((type == filetype.title_call || 
				  type == filetype.world_name ||
				  type == filetype.image) && set.CacheResourceFiles > 0))
			{
				filepath = myCacheFolder + uri.AbsolutePath.Replace('/', '\\');

				//检查Hack文件地址
				if (set.HackEnabled)
				{
					var fnext = uri.Segments.Last().Split('.');
					string hfilepath = filepath.Replace(uri.Segments.Last(), fnext[0] + ".hack." + fnext.Last());

					if (File.Exists(hfilepath))
					{
						result = hfilepath;
						return Direction.Return_LocalFile;
						//存在hack文件，则返回本地文件
					}

				}

				//检查缓存文件
				if (File.Exists(filepath))
				{
					//存在本地缓存文件 -> 请求服务器验证最后修改时间
					result = GetFileLastModifiedTime(uri);

					if (string.IsNullOrEmpty(result))
					{
						//没有关于这个文件最后修改时间的记录
						//-> 当做这个文件不存在 
						//-> 下载文件（记录保存地址）
						_RecordTask(url, filepath);
						return Direction.Discharge_Response;
					}
					//存在这个文件的修改时间记录
					//-> 请求服务器验证修改时间（记录读取或保存的位置）
					_RecordTask(url, filepath);
					return Direction.Verify_LocalFile;
				}
				else
				{
					//缓存文件不存在
					//-> 下载文件 （记录保存地址）
					_RecordTask(url, filepath);
					return Direction.Discharge_Response;
				}
			}

			//文件类型对应的缓存设置没有开启
			//-> 当做文件不存在
			return Direction.Discharge_Response;
		}


		public string GetFileLastModifiedTime(Uri uri)
		{ }

		void _RecordTask(string url, string filepath)
		{
			TaskRecord.Add(url, filepath);
		}
	}
}