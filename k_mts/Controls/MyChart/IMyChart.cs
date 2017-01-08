using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace k_mts
{
    public interface IMyChart
    {
        ChartAreaCollection Areas { get; }
        SeriesCollection Series { get; }
        AnnotationCollection Annotations { get; }
        LegendCollection Legends { get; }
        TitleCollection Titles { get; }

        object DataSource { get; set; }
        ChartToolCollection Tools { get; }
        ChartViewerCollection Viewers { get; }

        object Tag { get; set; }
        Control Parent { get; set; }
        
        void SaveChart(string filePath);
        void LoadChart(string filePath);
        void DataBind();

        ChartEx GetInternal();
        void SetDate(DateTime tEnd);
        void SetPos(int pos);
        void SetCount(int cnt);

        void SetLineVisible(bool bVisible);
        void SetBarVisible(bool bVisible);
    }
}
