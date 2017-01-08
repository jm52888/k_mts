using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
	public static class CollectionExtension
	{
		#region 연속추가(Adds)

		public static void Adds<T>(this ICollection<T> lst, params T[] vals)
		{
			foreach (T val in vals)
				lst.Add(val);
		}

		public static void Adds<T>(this ICollection<T> lst, Func<T, T> func, params T[] vals)
		{
			foreach (T val in vals)
				lst.Add(func(val));
		}

		#endregion

		#region Console입력(scanf)

		public static void scanf<T>(this ICollection<T> lst)
		{
			lst.Add((T)Convert.ChangeType(Console.ReadLine(), typeof(T)));
		}

		public static void scanf<T>(this ICollection<T> lst, int num)
		{
			Type t = typeof(T);
			for (int i = 0; i < num; i++)
				lst.Add((T)Convert.ChangeType(Console.ReadLine(), t));
		}

		public static void scanf<T>(this ICollection<T> lst, IFormatProvider provider)
		{
			lst.Add((T)Convert.ChangeType(Console.ReadLine(), typeof(T), provider));
		}

		public static void scanf<T>(this ICollection<T> lst, int num, IFormatProvider provider)
		{
			Type t = typeof(T);
			for (int i = 0; i < num; i++)
				lst.Add((T)Convert.ChangeType(Console.ReadLine(), t, provider));
		}

		#endregion
	}
}