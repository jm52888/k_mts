using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Data;

namespace k_mts
{
	public class k_mts_proc2
	{
		#region Calculate Procedure

		// inupt : Euro_5, Euro_5_1 ... Euro_5_6
		// output : Euro_5_1_wave ... Euro_5_6_wave
		public void Calculate_Historical(string tblName)
		{
			var dt0 = Global.DB.Tables[tblName];
			if (dt0 == null) return;

			for (int i = 1; i <= 6; i++)
			{
				var dt = Global.DB.Tables[tblName + "_" + i];                
				if( dt == null) continue;
				if (Global.DB.Tables.Contains(dt.TableName + "_wave")) continue;

				var res = from r0 in dt0.AsEnumerable()
						  join r in dt.AsEnumerable()
						  on r0["date"] equals r["date"]
						  select new { idx = (int)r0["idx"], value = (double)r["value"] };

				var dtWave = new DataTable(dt + "_wave");
                dtWave.Namespace = dt.TableName;
				var cols = dtWave.Columns;
				var rows = dtWave.Rows;

				cols.Add("idx", typeof(int));
				cols.Add("value", typeof(double));
				cols.Add("value_line", typeof(double));

				int idx_1 = -1;
                double val_1 = -1;
				foreach (var r in res)
				{
                    if (val_1 != r.value)
                    {
                        for (int idx = idx_1 + 1; idx < r.idx; idx++)
                            rows.Add(idx, DBNull.Value, DBNull.Value);

                        rows.Add(r.idx, r.value, r.value);
                        idx_1 = r.idx;
                        val_1 = r.value;
                    }
				}
				dtWave = MakeSmoothLine(dtWave, "value", "value_line");
				Global.DB.Tables.Add(dtWave);
			}
		}

		private DataTable MakeSmoothLine(DataTable dt, string colName, string colName2)
		{
			int iCol = dt.Columns.IndexOf(colName);
			int iCol2 = dt.Columns.IndexOf(colName2);
			if (iCol < 0 || iCol2 < 0) return dt;

			var rows = dt.Rows;
			int nRow = rows.Count;
			int start = -1;
			for (int r = 0; r < nRow; r++)
			{	
				if (rows[r][iCol] is DBNull) continue;
				else
				{
					double val = (double)rows[r][iCol];
					rows[r][iCol2] = val;

					if (!(start < 0 || r - start == 1))
					{
						double y_start = (double)rows[start][iCol];
						double dy = (val - y_start) / (r - start);

						for (int i = start + 1; i < r; i++)
							rows[i][iCol2] = y_start + dy * (i - start);
					}
					start = r;
				}
			}
			return dt;
		}

		#endregion
	}
}
