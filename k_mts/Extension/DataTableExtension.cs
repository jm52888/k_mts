using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Globalization;

namespace System.Data
{
	public static class DataTableExtension
	{
		#region Load/Save Text

		public static bool LoadText(this DataTable dt, string filePath, char sep = '\t', bool hasHeaders = true)
		{
			dt.Clear();
			var cols = dt.Columns;
			var rows = dt.Rows;

			using (StreamReader sr = new StreamReader(filePath))
			{
				string line = sr.ReadLine();
				string[] words = line.Split(sep);
				int nCol = words.Length;

				if (string.IsNullOrWhiteSpace(line)) return false;

				if (hasHeaders)
				{
					foreach (string colName in words)
						cols.Add(colName);
					line = sr.ReadLine();
				}
				else
				{
					for (int c = 0; c < nCol; c++)
						cols.Add("col" + (c + 1).ToString());
				}

				while (!string.IsNullOrWhiteSpace(line))
				{
					words = line.Split(sep);
					var r = dt.NewRow();
					for (int c = 0; c < nCol; c++)
						r[c] = words[c];
					dt.Rows.Add(r);
					line = sr.ReadLine();
				}
			}
			return true;
		}

		public static void SaveText(this DataTable dt, string filePath, char sep = '\t', bool hasHeaders = true)
		{
			dt.Clear();
		}

		#endregion

		#region Load/Save Excel

		public static void LoadExcel(this DataTable dt, string filePath, string sheetName, string fieldNames = "", bool hasHeader = true)
		{
			string sHDR = hasHeader ? "Yes" : "No";
			string sConn;
			string sExt = filePath.Substring(filePath.LastIndexOf('.')).ToLower();

			if (sExt == ".xlsx" || sExt == ".xlsb")
				sConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + sHDR + ";IMEX=0\"";
			else sConn = "Provder=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + sHDR + ";IMEX=0\"";

			using (OleDbConnection conn = new OleDbConnection(sConn))
			{
				conn.Open();

				DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
				DataRow schemaRow = schemaTable.Rows[0];

				string sheet = schemaRow["TABLE_NAME"].ToString();
				if (!sheet.EndsWith("_"))
					dt.LoadExcel(conn, sheetName, fieldNames);
			}
		}
		public static void LoadExcel(this DataTable dt, OleDbConnection conn, string sheetName, string fieldNames = "")
		{
			dt.Clear(); // todo - If columns is not empty, read specific fields form file.

			string sQuery = string.Format(
				"SELECT {0} FROM [{1}]"
				, string.IsNullOrWhiteSpace(fieldNames) ? "*" : fieldNames
				, sheetName + "$");

			using (OleDbDataAdapter da = new OleDbDataAdapter(sQuery, conn))
			{
				dt.Locale = CultureInfo.CurrentCulture;
				da.Fill(dt);
			}
		}

		#endregion

		#region Data Manipulation

		public static void AddToColumn(this DataTable dt, string colName, double val)
		{
			int iCol = dt.Columns.IndexOf(colName);
			if (iCol < 0) return;

			var rows = dt.Rows;
			int nRow = rows.Count;
			for (int r = 0; r < nRow; r++)
				rows[r][iCol] = Convert.ToDouble(rows[r][iCol]) + val;
		}

		public static void MultiplyToColumn(this DataTable dt, string colName, double val)
		{
			int iCol = dt.Columns.IndexOf(colName);
			if (iCol < 0) return;

			var rows = dt.Rows;
			int nRow = rows.Count;
			for (int r = 0; r < nRow; r++)
				rows[r][iCol] = Convert.ToDouble(rows[r][iCol]) * val;
		}

		#endregion
	}
}
