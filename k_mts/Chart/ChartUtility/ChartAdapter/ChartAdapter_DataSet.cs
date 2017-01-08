using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Data;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartAdapter_DataSet : ChartAdapter
	{
		#region Constructor

		public ChartAdapter_DataSet() { Name = "DataSet"; }
		public ChartAdapter_DataSet(string name) { Name = name; }

		#endregion

		#region ChartAdapter

		protected override IEnumerable[] _CreateViewData(Series series, string xMember, string yMembers)
		{	
			var ds = series.GetAttr("DataSource") as DataSet ?? Common.DataSource as DataSet;
			if (ds == null) return null;

			List<IEnumerable> lst = new List<IEnumerable>();
			List<string> tblNames = new List<string>();

			char[] seps = new char[] { '.' };
			string members = xMember + ',' + yMembers;
			foreach (string member in members.Split(','))
			{
				string[] path = member.Split(seps, 2);
				if (path.Length < 2) return null;

				if (!ds.Tables.Contains(path[0])) return null;

				var dt = ds.Tables[path[0]];
				var cols = dt.Columns;
				if (!cols.Contains(path[1])) return null;

				if (tblNames.Contains(path[0])) continue;
				else tblNames.Add(path[0]);

				var rows = dt.Rows;
				int nRow = rows.Count;
				if (nRow == 0) return null;

				var view = Common.ChartAreas.FindByName(series.ChartArea) as ChartView;
				if (view == null)
				{
					lst.Add(rows);					
					continue;
				}				

				int r_start =
					Math.Max(0,
					Math.Min(nRow - 1,
					view.ViewStart));

				int r_end =
					Math.Max(r_start,
					Math.Min(Math.Min(nRow - 1, view.ViewEnd),
					view.ViewStart + view.ViewCount - 1));

				List<DataRow> rowList = new List<DataRow>(r_end - r_start + 1);
				for (int r = r_start; r <= r_end; r++)
					rowList.Add(rows[r]);

				lst.Add(rowList);
			}
			return lst.ToArray();
		}

		#endregion
	}
}
