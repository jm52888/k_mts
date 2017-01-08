using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartViewer_k_Bar : ChartViewer, IDataUpdateEventHandler, IChartCustomizer
    {
        #region Internal Variable and Property

        #endregion

        #region Constructor

        public ChartViewer_k_Bar() : base("k_Bar") { }
        public ChartViewer_k_Bar(string name) : base(name) { }

        #endregion

        #region UpdateView

        public override void UpdateView(ChartView view)
        {
            if (!this.Enable) return;

            Series ms = Common.GetMainSeries(view);
            if (ms == null ||
                ms.ChartType != SeriesChartType.Column) return;

            var axisX = ms.XAxisType == AxisType.Primary ? view.AxisX : view.AxisX2;
            var axisX2 = ms.XAxisType == AxisType.Primary ? view.AxisX2 : view.AxisX;
            var axisY = ms.YAxisType == AxisType.Primary ? view.AxisY : view.AxisY2;
            var axisY2 = ms.YAxisType == AxisType.Primary ? view.AxisY2 : view.AxisY;

            var pts = ms.Points;
            int nPoint = pts.Count;
            if (nPoint == 0) return;

            // 현재가 추가            
            //var dt = Common.DataSource as DataTable;
            //if (dt != null)
            //{
            //    int x_end = Math.Min(view.ViewEnd, dt.Rows.Count - 1);
            //    if ((int)pts[nPoint - 1].XValue >= x_end && pts[nPoint - 1].Tag == null)
            //    {
            //        var dt0 = Global.DB.Tables[dt.Namespace];
            //        if (dt0 != null && dt0.Rows.Count > 0)
            //        {
            //            var row0 = dt0.Rows[dt0.Rows.Count - 1];
            //            pts.AddXY(pts[nPoint - 1].XValue + 1, row0["stop"]);
            //            var pt = pts[nPoint];
            //            pt.Tag = "현재가";
            //            pt.ToolTip = "값:" + ((double)row0["stop"]).ToString(axisY.LabelStyle.Format) +
            //                "\n일시:" + ((DateTime)row0["tm"]).ToString("yyyy-MM-dd_HH:mm");
            //        }
            //    }
            //}

            axisX.Minimum = view.ViewStart - view.MarginLeft;
            axisX.Maximum = view.ViewStart + (view.ViewCount + view.MarginRight);

            axisX.ScaleView.Position = ms.Points[0].XValue;
            axisX.Interval = (axisX.Maximum - axisX.Minimum) * view.AxisXIntervalRatio / 100.0;

            var pt_max = pts.FindMaxByValue();
            if (pt_max == null) return;
            double y_max = pt_max.YValues[0];

            var pt_min = pts.FindMinByValue();
            if (pt_min == null) return;
            double y_min = pt_min.YValues[0];

            double dy = Math.Max(view.MinmumHeight, y_max - y_min);

            axisY.Maximum = y_max + dy * view.MarginTop / 100.0;
            axisY.Minimum = y_min - dy * view.MarginBottom / 100.0;
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

            SetBarToolTip(view);
            SetBarColor(view);
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

        #region SetBarToolTip / SetBarColor / SetAxisLabel

        private void SetBarToolTip(ChartView view)
        {
            var ms = Common.GetMainSeries(view);
            if (ms == null) return;

            DataTable dt = Common.DataSource as DataTable;
            if (dt == null) return;
            var c_value = dt.Columns.IndexOf("value");
            if (c_value < 0) return;

            var c_date = dt.Columns.IndexOf("date");
            if (c_date < 0) return;

            string formatX = "yyyy-MM-dd_HH:mm";
            string formatY = Common.GetMainAxisY(view).LabelStyle.Format;
            var rows = dt.Rows;
            foreach (var pt in ms.Points)
            {
                if (pt.Tag != null) continue;

                int idx = (int)pt.XValue;
                pt.ToolTip = "값:" + ((double)rows[idx][c_value]).ToString(formatY) +
                    "\n일시:" + ((DateTime)dt.Rows[(int)pt.XValue][c_date]).ToString(formatX);
            }
        }

        private void SetBarColor(ChartView view)
        {
            var ms = Common.GetMainSeries(view);
            if (ms == null) return;

            var dt = Common.DataSource as DataTable;
            if (dt == null) return;

            var colName = ms.GetCustomProperty("xAxisLabel");
            if (string.IsNullOrWhiteSpace(colName)) return;

            var iCol_color = dt.Columns.IndexOf("color");
            if (iCol_color < 0) return;

            int iCol = dt.Columns.IndexOf(colName);
            if (iCol < 0) return;

            // 막대 그래프 색깔 변경.
            var rows = dt.Rows;
            var pts = ms.Points;

            int nPoint = pts.Count;
            for (int i = 0; i < nPoint; i++)
            {
                if (pts[i].Tag != null) // 현재가 입니다.
                {
                    pts[i].Color = Color.FromArgb(0, 255, 0); //Color.Lime;//Color.FromArgb(255, 255, 255); // 현재가는 흰색
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                    break;
                }

                int idx = (int)pts[i].XValue;
                var color = rows[idx][iCol_color];

                 if (color.Equals("H3"))
                {
                    pts[i].Color = Color.FromArgb(211, 129, 127);
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                }
                else if (color.Equals("R3"))
                {
                    pts[i].Color = Color.FromArgb(85, 142, 213);
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                }
                else if (color.Equals("SM"))
                {
                    pts[i].Color = Color.FromArgb(203, 203, 203);
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                }
                else if (color.Equals("HM"))
                {
                    pts[i].Color = Color.FromArgb(174, 193, 132);
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                }
                else
                {
                    pts[i].Color = Color.FromArgb(255, 205, 18);
                    pts[i].BorderColor = Color.FromArgb(64, 64, 64);
                    pts[i].BorderWidth = 1;
                }
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

        #endregion
    }
}
