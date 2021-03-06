﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace k_mts
{
    public partial class MyChartController : UserControl
    {
        #region Variable and Property

        ChartEx _chart = null;
        ChartView _view = null;

        private int _LargeIncrement = 10;
        [DefaultValue(10)]
        public int LargeIncrement { get; set; }

        public int ViewStart { get { return _view.ViewStart; } }
        public int ViewCount { get { return _view.ViewCount; } }
        public int ViewEnd { get { return _view.ViewEnd; } }
        public int DataCount
        {
            get
            {
                var dt = _chart.DataSource as DataTable;
                if (dt == null) return 0;
                return dt.Rows.Count;
            }
        }

        #endregion

        #region Constructor

        public MyChartController()
        {
            InitializeComponent();
            AutoScroll = true;
        }

        #endregion

        #region Method

        public void Attach(ChartEx chart)
        {
            if (chart == null) return;
            if (chart.ChartAreas.Count > 0)
                Attach(chart, chart.ChartAreas[0] as ChartView);
        }

        public void Attach(ChartEx chart, ChartView view)
        {
            if (chart == null || view == null) return;

            Detach(_chart);

            _chart = chart;
            _chart.Customize -= chart_Customize; // prevent duplication
            _chart.Customize += chart_Customize;
            _view = view;

            _UpdateCtrl();
        }

        private void _UpdateCtrl()
        {
            if (_view == null) return;

            var dt = _chart.DataSource as DataTable;
            if (dt == null) return;

            txtDataCount.Text = dt.Rows.Count.ToString();// _view.ViewEnd.ToString();
            hsViewStart.Maximum = dt.Rows.Count - _view.ViewCount;
        }

        public void Detach(ChartEx chart)
        {
            if (chart == null) return;

            chart.Customize -= chart_Customize;

            hsViewStart.DataBindings.Clear();
            nudViewCount.DataBindings.Clear();
        }

        public void SetCount(int cnt)
        {
            if (_view.ViewCount == cnt) return;

            _view.ViewCount = cnt;
            _chart.DataBind();
        }

        public void SetPos(int pos)
        {
            if (_view.ViewStart == pos) return;

            _view.ViewStart = pos;
            _chart.DataBind();
        }


        #endregion

        #region EventHandler :: chart (Customize)

        void chart_Customize(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            if (_view == null) return;

            int nData = this.DataCount;
            if (nData == 0) return;

            bool isScrollEnd = hsViewStart.Value == hsViewStart.Maximum;
            bool isViewEnd = _view.ViewStart >= hsViewStart.Maximum;

            hsViewStart.Maximum =
                Math.Max(0,
                Math.Min(nData - 1, _view.ViewEnd) - _view.ViewCount + 1);

            hsViewStart.Value = isScrollEnd && isViewEnd && AutoScroll ? hsViewStart.Maximum :
                Math.Max(hsViewStart.Minimum,
                Math.Min(hsViewStart.Maximum, _view.ViewStart));

            nudViewCount.Value = _view.ViewCount;
            txtDataCount.Text = Math.Min(_view.ViewEnd + 1, nData).ToString();
        }

        #endregion

        #region EventHandler :: hsViewStart

        private void hsViewStart_Scroll(object sender, ScrollEventArgs e)
        {
            if (_view == null) return;

            if (e.Type == ScrollEventType.LargeIncrement)
                e.NewValue = Math.Min(hsViewStart.Maximum, e.OldValue + _LargeIncrement);
            else if (e.Type == ScrollEventType.LargeDecrement)
                e.NewValue = Math.Max(hsViewStart.Minimum, e.OldValue - _LargeIncrement);

            if (_view.ViewStart == hsViewStart.Value) return;

            _view.ViewStart = hsViewStart.Value;
            _chart.DataBind();
        }

        private void hsViewStart_ValueChanged(object sender, EventArgs e)
        {
            if (_view.ViewStart == hsViewStart.Value) return;

            _view.ViewStart = hsViewStart.Value;
            _chart.DataBind();
        }

        private void hsViewStart_MouseEnter(object sender, EventArgs e)
        {
            btnEnd_MouseLeave(null, null);
        }

        #endregion

        #region EventHandler :: nudViewCount

        private void nudViewCount_ValueChanged(object sender, EventArgs e)
        {
            if (_view == null) return;

            if (_view.ViewCount == (int)nudViewCount.Value) return;

            int cnt_1 = _view.ViewCount;
            int cnt = (int)nudViewCount.Value;

            _view.ViewCount = cnt;

            bool isScrollEnd = hsViewStart.Value == hsViewStart.Maximum;
            if (isScrollEnd)
            {
                int nData = this.DataCount;
                if (nData < 0) return;

                hsViewStart.Maximum =
                    Math.Max(0,
                    Math.Min(nData - 1, _view.ViewEnd) - _view.ViewCount + 1);

                _view.ViewStart = hsViewStart.Maximum;
                hsViewStart.Value = _view.ViewStart;

                //if (cnt > cnt_1) return;
            }
            _chart.DataBind();
        }

        private void nudViewCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        #endregion

        #region EventHandler :: txtDataCount

        private void txtDataCount_MouseEnter(object sender, EventArgs e)
        {
            btnStart_MouseLeave(null, null);
        }

        #endregion

        #region EventHandler :: btnEnd

        private void btnEnd_Click(object sender, EventArgs e)
        {
            //hsViewStart.Value = hsViewStart.Maximum;
        }
        
        private void btnEnd_MouseEnter(object sender, EventArgs e)
        {
            //btnEnd.ForeColor = Color.Black;
            //btnEnd.BorderStyle = BorderStyle.Fixed3D;
        }

        private void btnEnd_MouseLeave(object sender, EventArgs e)
        {
            //btnEnd.ForeColor = Color.Gray;
            //btnEnd.BorderStyle = BorderStyle.None;
        }

        private void btnEnd_MouseMove(object sender, MouseEventArgs e)
        {
            //btnEnd_MouseEnter(null, null);
        }

        #endregion

        #region EventHandler :: btnStart

        private void btnStart_Click(object sender, EventArgs e)
        {
            //hsViewStart.Value = hsViewStart.Minimum;
        }

        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            //btnStart.ForeColor = Color.Black;
            //btnStart.BorderStyle = BorderStyle.Fixed3D;
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            //btnStart.ForeColor = Color.Gray;
            //btnStart.BorderStyle = BorderStyle.None;
        }

        private void btnStart_MouseMove(object sender, MouseEventArgs e)
        {
            //btnStart_MouseEnter(null, null);
        }

        #endregion

        private void btnEnd_MouseDown(object sender, MouseEventArgs e)
        {
            btnEnd.ForeColor = Color.Red;
            hsViewStart.Value = hsViewStart.Maximum;
        }

        private void btnEnd_MouseUp(object sender, MouseEventArgs e)
        {
            btnEnd.ForeColor = Color.Black;
        }

        private void btnStart_MouseDown(object sender, MouseEventArgs e)
        {
            btnStart.ForeColor = Color.Red;
            hsViewStart.Value = hsViewStart.Minimum;
        }

        private void btnStart_MouseUp(object sender, MouseEventArgs e)
        {
            btnStart.ForeColor = Color.Black;
        }
    }
}
