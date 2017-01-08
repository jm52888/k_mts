using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace k_mts
{
	public static class DBManager
	{
        public static void LoadHistorical(string filePath)
        {
            try
            {
                foreach (DataTable dt in Global.DB.LoadExcel(filePath))
                {
                    int c_idx = dt.Columns.Add("idx", typeof(int)).Ordinal;
                    int c_date = dt.Columns.Add("date", typeof(DateTime)).Ordinal;
                    int c_targetDate = dt.Columns.IndexOf("targetDate");
                    int c_targetTime = dt.Columns.IndexOf("targetTime");

                    var rows = dt.Rows;
                    int nRow = rows.Count;
                    for (int r = 0; r < nRow; r++)
                    {
                        rows[r][c_idx] = r;

                        DateTime targetDate = (DateTime)rows[r][c_targetDate];
                        DateTime targetTime = (DateTime)rows[r][c_targetTime];
                        rows[r][c_date] = targetDate.Date.Add(targetTime.TimeOfDay);
                    }
                }
            }
            catch (Exception ex) { MsgBox.Show(ex.Message); }
        }
	}
}