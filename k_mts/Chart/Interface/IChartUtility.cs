using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IChartUtility
	{	
		IChartUtility Parent { get; set; }
		CommonService Common { get; }
		void Update();
	}
}
