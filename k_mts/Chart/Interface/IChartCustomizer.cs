using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IChartCustomizer
	{
		void OnCustomize(ChartView view);
		void OnCustomizeLegend(ChartView view, LegendItemsCollection legendItems, string legendName);
    }
}
