using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public interface IDataUpdateEventHandler
    {
        void OnDataUpdating(object sender, ChartView view);
        void OnDataUpdated(object sender, ChartView view);
    }
}
