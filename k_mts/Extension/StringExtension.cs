using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
	public static class StringExtension
	{
		#region Left/Right/Mid

		public static string Left(this string s, int len)
		{
			return s.Substring(0, Math.Min(s.Length, len));
		}

		public static string Right(this string s, int len)
		{
			len = Math.Min(s.Length, len);
			return s.Substring(s.Length - len, len);
		}

		public static string Mid(this string s, int start, int end)
		{
			if (start < s.Length || end < s.Length)
				return s.Substring(start, end);
			else return s;
		}

		#endregion

		#region Filter

		public static string Filter(this string s, Func<char, bool> predicate)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char ch in s.Where(predicate))
				sb.Append(ch);
			return sb.ToString();
		}

		public static string Filter(this string s, Func<char, int, bool> predicate)
		{
			StringBuilder sb = new StringBuilder();
			foreach (char ch in s.Where(predicate))
				sb.Append(ch);
			return sb.ToString();
		}

		#endregion
	}
}
