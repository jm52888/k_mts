using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Reflection;

namespace System
{
	public static class Array<T>
	where T : struct
	{
		public static T[] LoadArray(DataTable dt)
		{
			return LoadArray(dt, null);
		}
		public static T[] LoadArray(DataTable dt, string[] fields)
		{
			Type t = typeof(T);

			FieldInfo[] info = t.GetFields();

			if (info.Length == 0)
				return new T[0];

			if (fields == null)
				fields = info.Select(field => field.Name).ToArray();

			int nField = fields.Length;
			if (nField == 0)
				return new T[0];

			int[] colIdx = new int[nField];
			for (int i = 0; i < colIdx.Length; i++)
				colIdx[i] = dt.Columns.IndexOf(fields[i]);

			if (colIdx.Count(val => val != -1) == 0)
				return new T[0];

			var cols = dt.Columns;
			var rows = dt.Rows;
			int nRow = rows.Count;
			T[] arr = new T[nRow];
			for (int i = 0; i < nField; i++)
			{
				if (colIdx[i] != -1)
				{
					FieldInfo field = info[i];
					Type fieldType = field.FieldType;
					for (int r = 0; r < nRow; r++)
					{
						TypedReference tref = __makeref(arr[r]);
						field.SetValueDirect(tref,
							Convert.ChangeType(rows[r][colIdx[i]], fieldType));
					}
				}
			}
			return arr;
		}
	}

	public static class ArrayExtension
	{
		public static DataTable ToTable<T>(this Array src, string idxName = null)
		{
			DataTable dt = new DataTable();

			T[] arr = src as T[];
			if (arr == null) throw new InvalidCastException();

			if (arr.Length == 0) return dt;

			Type t = typeof(T);
			FieldInfo[] fields = t.GetFields();

			if (fields.Length == 0) return dt;

			var cols = dt.Columns;
			var rows = dt.Rows;
			int nCol = fields.Length;
			int nRow = src.Length;
			bool bUseIdx = !string.IsNullOrWhiteSpace(idxName);

			for (int c = 0; c < nCol; c++)
				cols.Add(fields[c].Name, fields[c].FieldType);
			if (bUseIdx) cols.Add(idxName, typeof(int));
				

			for (int r = 0; r < nRow; r++)
			{
				DataRow row = rows.Add();
				TypedReference tref = __makeref(arr[r]);
				for (int c = 0; c < nCol; c++)
					row[c] = fields[c].GetValueDirect(tref);
				if (bUseIdx) row[nCol] = r;
			}
			return dt;
		}

		public static DataTable ToTable<T>(this Array src, object nullValue, string idxName = null)
		{
			DataTable dt = new DataTable();

			T[] arr = src as T[];
			if (arr == null)
				throw new InvalidCastException();

			if (arr.Length == 0)
				return dt;

			Type t = typeof(T);
			FieldInfo[] fields = t.GetFields();

			if (fields.Length == 0)
				return dt;

			var cols = dt.Columns;
			var rows = dt.Rows;
			int nCol = fields.Length;
			int nRow = src.Length;
			bool bUseIdx = !string.IsNullOrWhiteSpace(idxName);

			for (int c = 0; c < nCol; c++)
				cols.Add(fields[c].Name, fields[c].FieldType);
			if (bUseIdx) cols.Add(idxName, typeof(int));

			for (int r = 0; r < nRow; r++)
			{
				DataRow row = rows.Add();
				TypedReference tref = __makeref(arr[r]);
				for (int c = 0; c < nCol; c++)
				{
					row[c] = fields[c].GetValueDirect(tref);
					if (row[c].Equals(nullValue))
						row[c] = DBNull.Value;
					if (bUseIdx) row[nCol] = r;
				}
			}
			return dt;
		}
	}
}
