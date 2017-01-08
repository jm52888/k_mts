using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Data;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartAdapter_DataTable : ChartAdapter
	{
		#region Constructor

		public ChartAdapter_DataTable() { Name = "DataTable"; }
		public ChartAdapter_DataTable(string name) { Name = name; }

		#endregion

		#region ChartAdapter

		public override void DataBind(Series series, string xMember, string yMembers)
		{	
			base.DataBind(series, xMember, yMembers);
		}

		protected override IEnumerable[] _CreateViewData(Series series, string xMember, string yMembers)
		{	
			var dt = series.GetAttr("DataSource") as DataTable ?? Common.DataSource as DataTable;
			if (dt == null) return null;

			var cols = dt.Columns;
			if (!cols.Contains(xMember)) return null;
			foreach (string yMember in yMembers.Split(','))
				if (!cols.Contains(yMember.Trim())) return null;

			var rows = dt.Rows;
			int nRow = rows.Count;
			if (nRow == 0) return null;

			var view = Common.ChartAreas.FindByName(series.ChartArea) as ChartView;
			if (view == null) return new IEnumerable[] { rows };

			int r_start =
				Math.Max(-1,
				Math.Min(nRow - 1,
				view.ViewStart));

			int r_end =
				Math.Max(-1,
				Math.Min(Math.Min(nRow - 1, view.ViewEnd),
				view.ViewStart + view.ViewCount - 1));

            if (r_start == -1 || r_end == -1 || r_end - r_start < 1)
                return null;

            List<DataRow> rowList = new List<DataRow>(r_end - r_start + 1);
			for (int r = r_start; r <= r_end; r++)
				rowList.Add(rows[r]);

			return new IEnumerable[] { rowList };
		}

		#endregion
	}
}
