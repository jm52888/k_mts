using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartAdapter : ChartNamedUtility
	{
		#region Constructor

		public ChartAdapter() { }

		#endregion

		#region Method

		public virtual void DataBind(Series series, string xMember, string yMembers)
		{
			string sUseAdapter = series.GetCustomProperty("UseAdapter");
			if (sUseAdapter != null && sUseAdapter.Trim().ToLower() == "false")
				return;

			var pts = series.Points;
			var _ViewData = _CreateViewData(series, xMember, yMembers);
			if (_ViewData == null) pts.ClearEx();
			else
			{
				char[] sep = new char[] { '.' };
				string xValue = xMember.Split(sep, 2).Last().Trim();
				string yValues = "";

				foreach (string yMember in yMembers.Split(','))
				{
					if (yValues.Length > 0) yValues += ",";
					yValues += yMember.Split(sep, 2).Last().Trim();
				}

				if (_ViewData.Length == 1)
				{
					pts.DataBind(_ViewData[0], xValue, yValues, null);
				}
				else if (_ViewData.Length == 2)
				{
					pts.DataBindXY(_ViewData[0], xValue, _ViewData[1], yValues);
				}
			}
		}
		protected abstract IEnumerable[] _CreateViewData(Series series, string xMember, string yMembers);

		#endregion
	}
}
