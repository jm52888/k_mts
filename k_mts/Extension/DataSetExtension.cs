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
	public static class DataSetExtension
	{
		#region Load/Save Excel

		public static DataTable[] LoadExcel(this DataSet ds, string filePath, string fieldNames = "", bool hasHeader = true)
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

				string[] sheetNames = _GetExcelSheetNames(conn);
				if (sheetNames == null || sheetNames.Length == 0) return null;

                List<DataTable> lstTable = new List<DataTable>();

				string fileName = Path.GetFileNameWithoutExtension(filePath);
				foreach (var sheetName in sheetNames)
				{
					string tableName = fileName + "_" + sheetName;
					if (ds.Tables.Contains(tableName)) continue;

					DataTable dt = new DataTable();
					dt.LoadExcel(conn, sheetName, fieldNames);
					dt.TableName = tableName;

					lock (ds.Tables.SyncRoot)
						ds.Tables.Add(dt);

                    lstTable.Add(dt);
				}
                return lstTable.ToArray();
            }
		}
		private static string[] _GetExcelSheetNames(string sConn)
		{
			using (OleDbConnection conn = new OleDbConnection(sConn))
			{
				conn.Open();
				return _GetExcelSheetNames(conn);
			}
		}
		private static string[] _GetExcelSheetNames(OleDbConnection conn)
		{
			DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
			if (dt == null) return null;

			List<string> sheetNames = new List<string>();
			foreach (DataRow row in dt.Rows)
			{
				string sheetName = row["TABLE_NAME"].ToString();
				if (!sheetName.Contains('$')) continue;
				sheetName = sheetName.Replace("$", "");
				sheetName = sheetName.Replace("'", "");
				sheetNames.Add(sheetName);
			}
			return sheetNames.ToArray();
		}

		#endregion
	}
}