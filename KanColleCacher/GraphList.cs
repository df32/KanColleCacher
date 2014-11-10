using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Models.Raw;
using Fiddler;
using System.IO;

namespace d_f_32.KanColleCacher
{
	class GraphList
	{
		static List<ship_graph_item> graphList = new List<ship_graph_item>();

		static void PrintToFile()
		{
			string filepath = Settings.Current.CacheFolder + "\\GraphList.txt";
			StringBuilder content = new StringBuilder();

			content.AppendFormat(
				"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
				"SortNo", "ShipId", "ShipName",
				"FileName", "FileVersion", "GraphSortNo",
				"TypeName", "TypeSortNo", "TypeId"
				);
			content.AppendFormat(
				"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
				"序号", "ID", "名称",
				"文件名", "文件版本", "文件序号",
				"类型", "类型序号", "类型ID"
				);

			graphList.Sort((x,y) => 
			{
				return x.ship_sortno <= y.ship_sortno ? -1 : 1;
			});

			graphList.ForEach(x =>
				{
					content.AppendFormat(
						"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
						x.ship_sortno, x.ship_id, x.ship_name,
						x.ship_filename, x.ship_version, x.ship_graph_sortno,
						x.ship_type_name, x.ship_type_sortno, x.ship_type_id
					);
				});

			try
			{
				File.WriteAllText(filepath, content.ToString());
			}
			catch (Exception ex)
			{
				Log.Exception(ex.Source, ex, "GraphList.PrintToFile() 写入文件时异常");
			}
		}

		static void ParseSession(Session oSession)
		{
			SvData<kcsapi_start2> svd;
			if (!SvData.TryParse(oSession, out svd)) 
				return;

			var mst_shipgraph = svd.Data.api_mst_shipgraph
									.ToDictionary(x => x.api_id);
			var mst_ship = svd.Data.api_mst_ship
									.ToDictionary(x => x.api_id);
			var mst_stype = svd.Data.api_mst_stype
									.ToDictionary(x => x.api_id);

			graphList.Clear();

			foreach (var _pair in mst_shipgraph)
			{
				var item = new ship_graph_item();
				var _loc1 = _pair.Value;

				item.ship_id = _loc1.api_id;
				item.ship_filename = _loc1.api_filename;
				item.ship_version = _loc1.api_version;
				item.ship_graph_sortno = _loc1.api_sortno;

				if (mst_ship.ContainsKey(item.ship_id))
				{
					var _loc2 = mst_ship[item.ship_id];

					item.ship_sortno = _loc2.api_sortno;
					item.ship_name = _loc2.api_name;
					item.ship_type_id = _loc2.api_stype;

					if (mst_stype.ContainsKey(item.ship_type_id))
					{
						var _loc3 = mst_stype[item.ship_type_id];
						item.ship_type_name = _loc3.api_name;
						item.ship_type_sortno = _loc3.api_sortno;
					}

					mst_ship.Remove(item.ship_id);
				}
				else
				{
#if DEBUG
					Log.Note(String.Format(@"> shipgraph->ship匹配失败
> {0} = {1} {2} {3}
", _loc1.ToString(), _loc1.api_id, _loc1.api_sortno, _loc1.api_filename));
#endif
				}
			}


		}

		static public void PrintGraphListRule(Session oSession)
		{
			if (oSession.PathAndQuery != "/kcsapi/api_start2")
				return;
#if DEBUG
			Log.Note("PrintGraphListRule> Start");
#endif
			ParseSession(oSession);
			PrintToFile();
		}
	}
	
	
	class ship_graph_item
	{
		public int ship_id { get; set; }
		public int ship_sortno { get; set; }
		public string ship_name { get; set; }

		public int ship_type_id { get; set; }
		public int ship_type_sortno { get; set; }
		public string ship_type_name { get; set; }

		public int ship_graph_sortno { get; set; }
		public string ship_filename { get; set; }
		public string ship_version { get; set; }
	}
}
