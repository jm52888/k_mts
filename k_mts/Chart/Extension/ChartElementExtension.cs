using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class ChartElementExtension
	{
		#region Get/Set Attribute

		public static void SetAttr(this ChartElement el, string name, object obj)
		{
			var dic = el.Tag as Dictionary<string, object>;
			if (dic == null)
			{
				dic = new Dictionary<string, object>();
				dic.Add("tag", el.Tag);
				el.Tag = dic;
			}
			dic[name] = obj;
		}

		public static object GetAttr(this ChartElement el, string name)
		{
			object obj = null;

			var dic = el.Tag as Dictionary<string, object>;
			if (dic != null)
				dic.TryGetValue(name, out obj);

			return obj;
		}

		public static T GetAttr<T>(this ChartElement el, string name)
		{
			object obj = null;

			var dic = el.Tag as Dictionary<string, object>;
			if (dic != null)
				dic.TryGetValue(name, out obj);

			return (T)obj;
		}        

		#endregion
	}
}