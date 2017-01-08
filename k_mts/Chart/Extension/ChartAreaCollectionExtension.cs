using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class ChartAreaCollectionExtension
	{
		public static ChartView AddEx(this ChartAreaCollection areas, string name)
		{
			var area = new ChartView();
			area.Name = name;
			areas.Add(area);
			return area;
		}
	}
}
