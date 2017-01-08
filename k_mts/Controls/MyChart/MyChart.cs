using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace k_mts
{
	public partial class MyChart : UserControl, IMyChart
    {    
        #region Variable and Property



        #endregion

        #region Constructor

        public MyChart()
		{	
            _InitChart();
		}

        private void _InitChart()
        {
            InitializeComponent();
            chart.Viewers.Add(new ChartViewer_k_Bar());
            chart.Viewers.SetDefault("k_Bar");
            chart.Tools.SetDefault("Cursor");
            controller.Attach(chart);
        }

        #endregion

        #region IMyChart

        public ChartAreaCollection Areas { get { return chart.ChartAreas; } }
        public SeriesCollection Series { get { return chart.Series; } }
        public AnnotationCollection Annotations { get { return chart.Annotations; } }
        public LegendCollection Legends { get { return chart.Legends; } }
        public TitleCollection Titles { get { return chart.Titles; } }        
        
        public object DataSource
        {
            get { return chart.DataSource; }
            set { chart.DataSource = value; }
        }
        public ChartToolCollection Tools { get { return chart.Tools; } }
        public ChartViewerCollection Viewers { get { return chart.Viewers; } }        
        
        public void SaveChart(string filePath)
        {
            chart.SaveFile(filePath);
        }
        public void LoadChart(string filePath)
        {
            controller.Detach(chart);
            chart.LoadFile(filePath);
            controller.Attach(chart);
        }
        public void DataBind()
        {
            chart.DataBind();
        }

        public ChartEx GetInternal()
        {
            return chart;
        }
        public void SetDate(DateTime tEnd)
        {
            tEnd = tEnd.AddDays(1);

            var dt = chart.DataSource as DataTable;
            if (dt == null) return;

            var view = chart.ChartAreas[0] as ChartView;
            if (view == null) return;

            var rows = dt.Rows;
            int nRow = rows.Count;
            int viewEnd = nRow - 1;
            for (int r = 0; r < nRow; r++)
            {
                if ((DateTime)rows[r]["date"] >= tEnd)
                {
                    viewEnd = r - 1;
                    break;
                }
            }
            view.ViewEnd = viewEnd;
            view.ViewStart = viewEnd - view.ViewCount + 1;

            this.DataBind();
        }
        public void SetPos(int pos)
        {
            this.controller.SetPos(pos);
        }
        public void SetCount(int cnt)
        {
            this.controller.SetCount(cnt);
        }
        public void SetLineVisible(bool bVisible)
        {
            foreach (var s in chart.Series)
                if (s.ChartType == SeriesChartType.Line ||
                    s.ChartType == SeriesChartType.FastLine)                    
                    s.Enabled = bVisible;            
        }
        public void SetBarVisible(bool bVisible)
        {
            foreach (var s in chart.Series)
                if (s.ChartType == SeriesChartType.Column ||
                    s.ChartType == SeriesChartType.Candlestick)
                    s.Enabled = bVisible;            
        }

        #endregion

        #region EventHandler :: chart

        //private void chart_Enter(object sender, EventArgs e)
        //{
        //    //controller.Focus();
        //}

        #endregion

        #region ChartMenu

        private void 차트설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            PropertyGrid grid = new PropertyGrid();
            grid.SelectedObject = chart;
            grid.Dock = DockStyle.Fill;
            grid.Parent = f;
            f.Text = chart.ToString();
            f.Width = 320;
            f.Height = 480;
            f.Show();
        }

        private void 도구설정ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            menuItem.DropDownItems.Clear();
            foreach (var tool in chart.Tools)
            {
                var item = menuItem.DropDownItems.Add(tool.Name);
                item.Click += Item_Click;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            var tool = chart.Tools[item.Text];
            if (tool == null) return;

            Form f = new Form();
            PropertyGrid grid = new PropertyGrid();
            grid.SelectedObject = tool;
            grid.Dock = DockStyle.Fill;
            grid.Parent = f;
            f.Text = tool.ToString();
            f.Width = 320;
            f.Height = 480;
            f.Show();
        }

        private void ChartMenu_Opening(object sender, CancelEventArgs e)
        {
            if (chart.Tools.SelectedTool == null ||
                chart.Tools.SelectedTool == chart.Tools.DefaultTool &&
                !chart.Tools.SelectedTool.LabelVisible)
                return;

            e.Cancel = true;
        }

        private void translateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var anots = chart.Annotations.Where(a => a.IsSelected).ToArray();
            if (anots.Length == 0) return;

            var anot = anots[0];
            var editor = chart.Tools.GetCompatiabe(anot) as IAnnotationEditor;
            if (editor == null) return;

            anot.IsSelected = false;
            switch (((ToolStripMenuItem)sender).Tag.ToString())
            {
                case "Up": anot = editor.Translate(anot, 0.0, 0.1, true); break;
                case "Down": anot = editor.Translate(anot, 0.0, -0.1, true); break;
                case "Left": anot = editor.Translate(anot, -0.1, 0.0, true); break;
                case "Right": anot = editor.Translate(anot, 0.1, 0.0, true); break;
            }
            if (anot != null) anot.IsSelected = true;
        }

        private void reflectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var anots = chart.Annotations.Where(a => a.IsSelected).ToArray();
            if (anots.Length == 0) return;

            var anot = anots[0];
            var editor = chart.Tools.GetCompatiabe(anot) as IAnnotationEditor;
            if (editor == null) return;

            anot.IsSelected = false;
            switch (((ToolStripMenuItem)sender).Tag.ToString())
            {
                case "Up": anot = editor.Reflect(anot, FlowDirection.BottomUp, true); break;
                case "Down": anot = editor.Reflect(anot, FlowDirection.TopDown, true); break;
                case "Left": anot = editor.Reflect(anot, FlowDirection.RightToLeft, true); break;
                case "Right": anot = editor.Reflect(anot, FlowDirection.LeftToRight, true); break;
            }
            if (anot != null) anot.IsSelected = true;
        }

        #endregion

        #region Type Casting

        public static explicit operator ChartEx(MyChart ch)
		{
			return ch.chart;
		}

        #endregion

        private void chart_DoubleClick(object sender, EventArgs e)
        {
            controller.Focus();
        }
    }
}