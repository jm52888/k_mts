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
	public partial class MyChart2 : UserControl, IMyChart
    {
		#region Variable and Property

		public int Depth
		{
			get
			{
				return (chart.Viewers.First() as ChartViewer_k_Candle).Depth;
			}
			set
			{
				if (value > 0)
				{
					(chart.Viewers.First() as ChartViewer_k_Candle).Depth = value;
					Viewers.Update();
				}
				else if (Depth == 1)
				{
					throw new ArgumentException("더 이상 작아질 수 없습니다.");
				}
				else
				{
					(chart.Viewers.First() as ChartViewer_k_Candle).Depth = 1;
					Viewers.Update();
				}
			}
		}

		#endregion

		#region Constructor

		public MyChart2()
		{	
            _InitChart();
		}

        private void _InitChart()
        {
            InitializeComponent();
            chart.Viewers.Add(new ChartViewer_k_Candle());
            chart.Viewers.SetDefault("k_Candle");
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
            //foreach (var s in chart.Series)
            //    if (s.ChartType == SeriesChartType.Line ||
            //        s.ChartType == SeriesChartType.FastLine)
            //        s.Enabled = bVisible;
            //chart.DataBind();
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

        private void ChartMenu_Opening(object sender, CancelEventArgs e)
        {
            if (chart.Tools.SelectedTool == null ||
                chart.Tools.SelectedTool == chart.Tools.DefaultTool &&
                !chart.Tools.SelectedTool.LabelVisible)
                return;

            e.Cancel = true;
        }

        private void 테스트ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dt0 = chart.DataSource as DataTable;
			if(dt0 == null) return;

			string tblName = dt0.TableName;
            if (!Global.DB.Tables.Contains(tblName + "_1_wave"))
            {
                k_mts_proc2 proc = new k_mts_proc2();
                proc.Calculate_Historical(tblName);
            }
			
            for (int i = 1; i <= 6; i++)
            {
                tblName = dt0.TableName + "_" + i + "_wave";
                var dt = Global.DB.Tables[tblName];
                if (dt == null) continue;

                var s = chart.Series.FindByName("wave" + i);
                if (s == null) continue;

                s.SetAttr("DataSource", dt);
            }
            chart.DataBind();
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

        private void 차트설정ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form f = new Form();
			PropertyGrid grid = new PropertyGrid();
			grid.SelectedObject = chart;
			grid.Dock = DockStyle.Fill;
			grid.Parent = f;
            f.Text = chart.ToString();
			f.Show();
		}

		private void 그리드ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dt0 = chart.DataSource as DataTable;
			var dt = Global.DB.Tables[dt0.TableName + "_1_wave"];
			if (dt == null) return;

			Form f = new Form();
			DataGridView dgv = new DataGridView();
			dgv.DataSource = dt;
			dgv.Dock = DockStyle.Fill;
			dgv.Parent = f;
			f.Show();
		}

        private void 파동ToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            foreach (ToolStripMenuItem item in menuItem.DropDownItems)
            {
                var tag = item.Tag as string;
                if (tag == null) item.Enabled = false;

                var s = chart.Series.FindByName(tag);
                if (s == null) item.Enabled = false;

                item.Checked = s.Enabled;
            }
        }

        private void waveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            var tag = item.Tag as string;
            if (tag == null) return;

            var s = chart.Series.FindByName(tag);
            if (s == null) return;

            item.Checked = !item.Checked;
            s.Enabled = item.Checked;
            chart.DataBind();
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

        public static explicit operator ChartEx(MyChart2 ch)
        {
            return ch.chart;
        }


        #endregion

        private void chart_Customize(object sender, EventArgs e)
        {
            MainForm parent = this.ParentForm as MainForm;
            var s = chart.Series[5];
            var n = chart.ChartAreas[0];

            string sUpColor = s.GetCustomProperty("PriceUpColor");
            string sDownColor = s.GetCustomProperty("PriceDownColor");

            var colorUp = Color.FromName(sUpColor);
            var colorDown = Color.FromName(sDownColor);
            chart.ChartAreas[0].AxisX.IsInterlaced = false;

            var pts = s.Points;
            int nPoint = pts.Count;
            for (int i = 0; i < nPoint; i++)
            {
                if (parent.candle_s == "Candle_A")
                //if (Sendv == "Candle_style_B")
                {
                    //chart.ChartAreas[0].BackColor = Color.FromArgb(37, 37, 38);//64, 64, 64
                    //chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);
                    //chart.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);

                    //chart.ChartAreas[0].AxisX.IsInterlaced = false;
                    //chart.Series[5]["PointWidth"] = "0.6";
                    //pts[i].BorderWidth = 1;
                    //pts[i].CustomProperties = "PointWidth = 0.5"; 
                    if (i == 0) continue; // 현재봉이 화면상 처음봉이면 skip

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon  
                        pts[i].Color = Color.FromArgb(255, 54, 255); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(255, 0, 221); //Color.FromArgb(255, 162, 255);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(224, 224, 224); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.DarkGray; //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        pts[i].BorderWidth = 1;

                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(224, 224, 224); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.DarkGray; //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.Cyan; //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(0, 176, 240); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }



                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"64, 188, 54"
                        pts[i].Color = pts[i].Color = Color.Cyan; //Color.FromArgb(64, 188, 54); //colorUp;
                        pts[i].BorderColor = Color.DeepSkyBlue; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 3 / 2;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(224, 224, 224); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.DarkGray;// Color.FromArgb(0, 176, 240); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(224, 224, 224); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.DarkGray; //Color.FromArgb(0, 176, 240);
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(255, 54, 255); ;//Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.DeepSkyBlue;
                        //pts[i].BorderWidth = 1;
                    }


                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.White;
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.White;
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.Cyan;
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }
                    //ex. 전봉과 현재봉 비교
                    //if (i == 0) continue; // 현재봉이 화면상 처음봉이면 skip

                    //else if (pts[i].YValues[0] > pts[i - 1].YValues[0] && pts[i].YValues[1] < pts[i - 1].YValues[1])
                    //{
                    //    pts[i].Color = Color.White;
                    //    pts[i].BorderColor = Color.White;
                    //    pts[i].BorderWidth = 1;
                    //}
                    //else if (pts[i].YValues[0] <= pts[i - 1].YValues[0] && pts[i].YValues[1] >= pts[i - 1].YValues[1])
                    //{
                    //    pts[i].Color = Color.Lime;
                    //    pts[i].BorderColor = Color.Lime;
                    //    pts[i].BorderWidth = 1;
                    //}
                    //else if (pts[i].YValues[0] > pts[i - 1].YValues[0])
                    //{
                    //    pts[i].Color = Color.Red;
                    //    pts[i].BorderColor = Color.Red;
                    //    pts[i].BorderWidth = 1;
                    //}
                    //else
                    //{
                    //    pts[i].Color = Color.Blue;
                    //    pts[i].BorderColor = Color.Blue;
                    //    pts[i].BorderWidth = 1;
                    //}
                }
                else if (parent.candle_s == "Candle_B")
                //if (Sendv == "Candle_style_B")
                {
                    //chart.ChartAreas[0].BackColor = Color.FromArgb(37, 37, 38);//64, 64, 64
                    //chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);
                    //chart.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);

                    //chart.ChartAreas[0].AxisX.IsInterlaced = false;
                    //chart.Series[5]["PointWidth"] = "0.6";
                    pts[i].BorderWidth = 1;
                    //pts[i].CustomProperties = "PointWidth = 0.5"; 
                    if (i == 0) continue; // 현재봉이 화면상 처음봉이면 skip

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon  
                        pts[i].Color = Color.FromArgb(162, 0, 120); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(162, 0, 120); //Color.FromArgb(255, 162, 255);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(162, 0, 120); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(162, 0, 120); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;

                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(162, 0, 120); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(162, 0, 120);//Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "129, 0, 95"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(162, 0, 120); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(162, 0, 120); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }



                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"64, 188, 54"
                        pts[i].Color = Color.FromArgb(0, 117, 179); //Color.FromArgb(64, 188, 54); //colorUp;
                        pts[i].BorderColor = Color.FromArgb(0, 117, 179); //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 3 / 2;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(0, 117, 179); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(0, 117, 179);// Color.FromArgb(0, 176, 240); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(0, 117, 179); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(0, 117, 179); //Color.FromArgb(0, 176, 240);
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "0, 84, 127";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(0, 117, 179);//Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(0, 117, 179);
                        //pts[i].BorderWidth = 1;
                    }


                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        //pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }
                }



                else if (parent.candle_s == "Candle_C")
                //if (Sendv == "Candle_style_B")
                {
                    //chart.ChartAreas[0].BackColor = Color.WhiteSmoke;// FromArgb(231, 231, 231);// FromArgb(37, 37, 38);//64, 64, 64
                    //chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;// FromArgb(64, 64, 64);
                    //chart.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.Gainsboro;// FromArgb(64, 64, 64);

                    //chart.ChartAreas[0].AxisX.IsInterlaced = false;
                    //chart.Series[5]["PointWidth"] = "0.6";
                    pts[i].BorderWidth = 1;
                    //pts[i].CustomProperties = "PointWidth = 0.5"; 
                    if (i == 0) continue; // 현재봉이 화면상 처음봉이면 skip

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "222, 60, 4"; //"Fuchsia";//192, 0, 192  Maroon  
                        pts[i].Color = Color.FromArgb(200, 55, 4); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(200, 55, 4); //Color.FromArgb(255, 162, 255);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "222, 60, 4"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(200, 55, 4); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(200, 55, 4); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;

                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "222, 60, 4"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(200, 55, 4); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(200, 55, 4);//Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "222, 60, 4"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(200, 55, 4); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(200, 55, 4); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }



                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"64, 188, 54"
                        pts[i].Color = Color.FromArgb(3, 185, 38); //Color.FromArgb(64, 188, 54); //colorUp;
                        pts[i].BorderColor = Color.FromArgb(129, 192, 255); //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 3 / 2;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(3, 185, 38);// Color.FromArgb(0, 176, 240); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(3, 185, 38); //Color.FromArgb(0, 176, 240);
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38);//Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(3, 185, 38);
                        //pts[i].BorderWidth = 1;
                    }


                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38);
                        //pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "4, 222, 5";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(3, 185, 38);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }
                }
                else if (parent.candle_s == "Candle_D")
                //if (Sendv == "Candle_style_B")
                {
                    //chart.ChartAreas[0].BackColor = Color.FromArgb(37, 37, 38);//64, 64, 64
                    //chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);
                    //chart.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.FromArgb(64, 64, 64);

                    //chart.ChartAreas[0].AxisX.IsInterlaced = false;
                    //chart.Series[5]["PointWidth"] = "0.6";
                    pts[i].BorderWidth = 1;
                    //pts[i].CustomProperties = "PointWidth = 0.5"; 
                    if (i == 0) continue; // 현재봉이 화면상 처음봉이면 skip

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "191, 191, 191"; //"Fuchsia";//192, 0, 192  Maroon  
                        pts[i].Color = Color.FromArgb(191, 191, 191); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(191, 191, 191); //Color.FromArgb(255, 162, 255);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "191, 191, 191"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(191, 191, 191); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(191, 191, 191); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;

                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "191, 191, 191"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(191, 191, 191); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(191, 191, 191);//Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] > pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceUpColor"] = "191, 191, 191"; //"Fuchsia";//192, 0, 192  Maroon
                        pts[i].Color = Color.FromArgb(191, 191, 191); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(191, 191, 191); //Color.FromArgb(0, 176, 240);
                        // pts[i].BorderColor = Color.Fuchsia; //Color.FromArgb(192, 0, 192);
                        //pts[i].BorderWidth = 1;
                    }



                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "83, 141, 213";//"64, 188, 54"
                        pts[i].Color = Color.FromArgb(83, 141, 213); //Color.FromArgb(64, 188, 54); //colorUp;
                        pts[i].BorderColor = Color.FromArgb(83, 141, 213); //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 3 / 2;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "83, 141, 213";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(141, 180, 226); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(141, 180, 226);// Color.FromArgb(0, 176, 240); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 1;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "83, 141, 213";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(141, 180, 226); //Color.FromArgb(64, 188, 54); //Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(141, 180, 226); //Color.FromArgb(0, 176, 240);
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] < pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "83, 141, 213";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(141, 180, 226);//Color.Fuchsia; //colorUp;//Color.FromArgb(255, 0, 255);
                        pts[i].BorderColor = Color.FromArgb(141, 180, 226);
                        //pts[i].BorderWidth = 1;
                    }


                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        //pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        //pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] > pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] >= pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }

                    else if ((pts[i].YValues[3] == pts[i].YValues[2]) && (pts[i].YValues[0] <= pts[i - 1].YValues[0]) && (pts[i].YValues[1] < pts[i - 1].YValues[1]))
                    {
                        pts[i]["PriceDownColor"] = "64, 188, 54";//"71, 200, 62"
                        pts[i].Color = Color.FromArgb(64, 188, 54);
                        pts[i].BorderColor = Color.Yellow; //Color.FromArgb(0, 182, 246); //Color.FromArgb(0, 176, 240);
                        pts[i].BorderWidth = 2;
                        //pts[i].Color = colorDown;
                    }
                }
            }
        }

        private void chart_DoubleClick(object sender, EventArgs e)
        {
            controller.Focus();
        }

        private void chart_Click(object sender, EventArgs e)
        {
            controller.Focus();
        }
    }
}
