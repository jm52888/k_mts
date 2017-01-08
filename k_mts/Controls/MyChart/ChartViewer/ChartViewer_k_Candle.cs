using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartViewer_k_Candle : ChartViewer, IDataUpdateEventHandler, IChartCustomizer
    {
        #region Variable and Property

        #endregion

        #region Constructor

        public ChartViewer_k_Candle() : base("k_Candle") { }
        public ChartViewer_k_Candle(string name) : base(name) { }        

        #endregion

        #region UpdateView

        public override void UpdateView(ChartView view)
        {
            int d = 100;
            if (!this.Enable) return;

            Series ms = Common.GetMainSeries(view);
            if (ms == null ||
                ms.ChartType != SeriesChartType.Candlestick) return;

            var axisX = ms.XAxisType == AxisType.Primary ? view.AxisX : view.AxisX2;
            var axisX2 = ms.XAxisType == AxisType.Primary ? view.AxisX2 : view.AxisX;
            var axisY = ms.YAxisType == AxisType.Primary ? view.AxisY : view.AxisY2;
            var axisY2 = ms.YAxisType == AxisType.Primary ? view.AxisY2 : view.AxisY;

            var pts = ms.Points;
            if (pts.Count == 0) return;

            axisX.Minimum = view.ViewStart - view.MarginLeft;
            axisX.Maximum = view.ViewStart + (view.ViewCount + view.MarginRight);

            axisX.ScaleView.Position = ms.Points[0].XValue;
            axisX.Interval = (axisX.Maximum - axisX.Minimum) * view.AxisXIntervalRatio / 100.0;

            string valName1 = "y1"; // high
            string valName2 = "y2"; // low

            var pt_max = pts.FindMaxByValue(valName1);
            if (pt_max == null) return;
            double y_max = pt_max.YValues[0]; // maximum of high prices

            var pt_min = pts.FindMinByValue(valName2);
            if (pt_min == null) return;
            double y_min = pts.FindMinByValue(valName2).YValues[1]; // minimum of low prices

            double dy = y_max - y_min;

            axisY.Maximum = y_max + dy * view.MarginTop / d;
            axisY.Minimum = y_min - dy * view.MarginBottom / d;
            double yInterval = (axisY.ScaleView.ViewMaximum - axisY.ScaleView.ViewMinimum)
                * view.AxisYIntervalRatio / 100.0;// 100.1; // 100.0이면 y축 최상단 라벨이 종종 안나옴.
            axisY.Interval = Math.Max(view.MinmumHeight, yInterval);
            axisY.IntervalOffset = 0.0;// dy * 0.001; // 0.0이면 y축 최하단 라벨이 종종 안나옴.

            axisX2.Minimum = axisX.Minimum;
            axisX2.Maximum = axisX.Maximum;
            axisY2.Minimum = axisY.Minimum;
            axisY2.Maximum = axisY.Maximum;

            foreach (var s in Common.Series)
            {
                if (s.ChartArea == view.Name && s.Name != ms.Name)
                {
                    double yAxisOffsetRatio;
                    if (double.TryParse(s.GetCustomProperty("yAxisOffsetRatio"), out yAxisOffsetRatio))
                    {
                        s.Points.SuspendUpdates();
                        foreach (var pt in s.Points)
                            pt.YValues[0] += dy * yAxisOffsetRatio;
                        s.Points.ResumeUpdates();
                    }
                }
            }
        }

        #endregion

        #region IDataUpdateEventHandler

        public void OnDataUpdating(object sender, ChartView view)
        {
            
        }

        public void OnDataUpdated(object sender, ChartView view)
        {
            if (!this.Enable) return;

            SetCandleToolTip(view);
            ShowWave(view);
        }

        #endregion

        #region IChartCustomizer

        public void OnCustomize(ChartView view)
        {
            if (!this.Enable) return;

            SetAxisLabel(view);
        }

        public void OnCustomizeLegend(ChartView view, LegendItemsCollection legendItems, string legendName)
        {

        }

        #endregion

        #region SetCandleToolTip, SetAxisLabel, ShowWave, ClearWave

        private void SetCandleToolTip(ChartView view)
        {
            var ms = Common.GetMainSeries(view);
            if (ms == null) return;

            DataTable dt = Common.DataSource as DataTable;
            if (dt == null) return;
            
            var c_start = dt.Columns.IndexOf("start");
            if (c_start < 0) return;

            var c_high = dt.Columns.IndexOf("high");
            if (c_high < 0) return;

            var c_low = dt.Columns.IndexOf("low");
            if (c_low < 0) return;

            var c_stop = dt.Columns.IndexOf("stop");
            if (c_stop < 0) return;

            var c_date = dt.Columns.IndexOf("date");
            if (c_date < 0) return;

            string formatX = "yyyy-MM-dd_HH:mm";
            string formatY = Common.GetMainAxisY(view).LabelStyle.Format;
            var rows = dt.Rows;
            foreach (var pt in ms.Points)
            {
                int idx = (int)pt.XValue;
                pt.ToolTip = "시가:" + ((double)rows[idx][c_start]).ToString(formatY) +
                    "\n고가:" + ((double)rows[idx][c_high]).ToString(formatY) +
                    "\n저가:" + ((double)rows[idx][c_low]).ToString(formatY) +
                    "\n종가:" + ((double)rows[idx][c_stop]).ToString(formatY) +
                     "\n일시" + ((DateTime)dt.Rows[(int)pt.XValue][c_date]).ToString(formatX);
            }
        }

        private void SetAxisLabel(ChartView view)
        {
            var ms = Common.GetMainSeries(view);
            if (ms == null) return;
            if (ms.XValueType == ChartValueType.String) return;

            DataTable dt = Common.DataSource as DataTable;
            if (dt == null) return;

            string colName = ms.GetCustomProperty("xAxisLabel");
            if (string.IsNullOrWhiteSpace(colName)) return;

            var iCol = dt.Columns.IndexOf(colName);
            if (iCol < 0) return;

            var axisX = Common.GetMainAxisX(view);
            if (axisX == null) return;

            int idx;
            var rows = dt.Rows;
            int nRow = rows.Count;
            foreach (var label in axisX.CustomLabels)
            {
                idx = (int)Convert.ToDouble(label.Text);

                if (idx < 0 || idx >= nRow)
                    label.Text = "";
                else label.Text = ((DateTime)rows[idx][iCol]).ToString("yyyy-MM-dd_hh:mm");
            }
        }

        public void ShowWave(ChartView view)
        {
            ClearWave();

            DataTable dt0 = Common.DataSource as DataTable;
            if (dt0 == null) return;

            var ms = Common.GetMainSeries(view);
            if (ms == null || ms.Points.Count == 0) return;

            int start = (int)ms.Points[0].XValue;
            int end = (int)ms.Points[ms.Points.Count - 1].XValue;

            for (int i = 1; i <= 6; i++)
            {
                var s = Common.Series.FindByName("wave" + i);
                if (s == null || !s.Enabled) continue;

                var pts = s.Points;
                pts.ClearEx();

                string tblName = string.Format("{0}_{1}_wave", dt0.TableName, i);
                var dt = s.GetAttr<DataTable>("DataSource");
                if (dt == null) continue;

                var cols = dt.Columns;
                int c_idx = cols.IndexOf("idx");
                int c_line = cols.IndexOf("value_line");
                if (c_idx < 0 || c_line < 0) return;

                var rows = dt.Rows;
                int nRow = rows.Count;
                pts.SuspendUpdates();

                int r_left = 0;
                int r_right = nRow - 1;
                int r_mid, temp;
                while (r_left < r_right)
                {
                    r_mid = (r_left + r_right) >> 1;
                    temp = (int)rows[r_mid][c_idx];

                    if (temp > start) r_right = r_mid - 1;
                    else if (temp < start) r_left = r_mid + 1;
                    else
                    {
                        while (--r_mid > 0 && (int)rows[r_mid][c_idx] == start) ;
                        r_left = r_mid + 1;
                        break;
                    }
                }
                int r_start = r_left;

                r_right = nRow - 1;
                while (r_left < r_right)
                {
                    r_mid = (r_left + r_right) >> 1;
                    temp = (int)rows[r_mid][c_idx];

                    if (temp > end) r_right = r_mid - 1;
                    else if (temp < end) r_left = r_mid + 1;
                    else
                    {
                        while (++r_mid < nRow && (int)rows[r_mid][c_idx] == end) ;
                        r_right = r_mid - 1;
                        break;
                    }
                }
                int r_end = r_right;

                for (int r = r_start; r <= r_end; r++)
                    pts.AddXY(rows[r][c_idx], rows[r][c_line]);
   
                pts.ResumeUpdates();
            }
        }

        private void ClearWave()
        {
            for (int i = 1; i <= 6; i++)
            {
                var s = Common.Series.FindByName("wave" + i);
                if (s == null) continue;
                s.Points.ClearEx();
            }
        }

        #endregion
    }
}
