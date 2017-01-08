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
using System.Collections;

namespace k_mts
{
	public partial class MyChartGroup : UserControl, IEnumerable<IMyChart>
	{
		#region Variable and Property

		private ViewMode _Mode = ViewMode.Bar6;
		[DefaultValue(ViewMode.Bar6)]
		public ViewMode Mode 
		{
			get { return _Mode; }
			set 
			{	
				_Mode = value;                
				_UpdateView();
			}
		}

		[Browsable(false)]
		public ChartEx SelectedChart { get; set; }

		#endregion

		#region View Mode

		public enum ViewMode
		{
			Candle1,
			Bar6,
			Candle1_Bar4
		}

		private void _UpdateView()
		{
			switch (_Mode)
			{
				case ViewMode.Candle1:
                    split_v1.Parent = null;
                    Chart0.Parent = this;
                    break;

				case ViewMode.Bar6:
                    Chart0.Parent = null;
                    split_h1.Parent = split_v1.Panel1;                    
                    Chart1.Parent = split_h1.Panel1;
                    Chart2.Parent = split_h2.Panel1;
                    Chart3.Parent = split_h3.Panel1;
                    Chart4.Parent = split_h1.Panel2;
                    Chart5.Parent = split_h2.Panel2;
                    Chart6.Parent = split_h3.Panel2;
                    split_v1.Parent = this;
                    split_v1.SplitterDistance = split_v1.Height / 3;
                    split_v2.SplitterDistance = split_v1.Height / 3;
                    break;

				case ViewMode.Candle1_Bar4:
                    split_h1.Parent = null;
					Chart0.Parent = split_v1.Panel1;
					Chart1.Parent = split_h2.Panel1;
					Chart2.Parent = split_h3.Panel1;
					Chart3.Parent = split_h2.Panel2;
					Chart4.Parent = split_h3.Panel2;
					Chart5.Parent = null;
					Chart6.Parent = null;
                    split_v1.Parent = this;
                    split_v1.SplitterDistance = split_v1.Height / 2;
                    split_v2.SplitterDistance = split_v1.Height / 4;                    
                    break;
			}
		}

		#endregion

		#region Constructor

		public MyChartGroup()
		{	
            _Initialize();
		}

        private void _Initialize()
        {
            InitializeComponent();
            _InitChart();
        }

        private void _InitChart()
		{
            for (int i = 0; i <= 6; i++)
            {
                GetChart(i).MouseDown += MyChartGroup_MouseDown;

                if (i <= 3)
                    this[i].SetCount(130);
                else this[i].SetCount(100);
            }
        }

        #endregion        

        #region Method

        public void Calculate_Historical()
        {
            string sTerm = this.Name.Filter(ch => char.IsNumber(ch));
            int term = int.Parse(sTerm);

            k_mts_proc proc = new k_mts_proc(term);
            proc.Calculate_Historical(this.Name);

            k_mts_proc2 proc2 = new k_mts_proc2();
            proc2.Calculate_Historical(this.Name);
        }

        public void SetData()
        {
            Chart0.DataSource = Global.DB.Tables[this.Name];
            Chart1.DataSource = Global.DB.Tables[this.Name + "_1"];
            Chart2.DataSource = Global.DB.Tables[this.Name + "_2"];
            Chart3.DataSource = Global.DB.Tables[this.Name + "_3"];
            Chart4.DataSource = Global.DB.Tables[this.Name + "_4"];
            Chart5.DataSource = Global.DB.Tables[this.Name + "_5"];
            Chart6.DataSource = Global.DB.Tables[this.Name + "_6"];
            
            for (int i = 1; i <= 6; i++)
            {
                string tblName = this.Name + "_" + i + "_wave";
                var dt = Global.DB.Tables[tblName];
                if (dt == null) continue;

                var s = Chart0.Series.FindByName("wave" + i);
                if (s == null) continue;

                s.SetAttr("DataSource", dt);                
            }
        }

        public void DataBind()
        {
            foreach (var ch in this) ch.DataBind();
        }

        public void Select(string toolName)
		{
            foreach (var ch in this) ch.Tools.Select(toolName);
        }

		public void Release()
		{
            foreach (var ch in this) ch.Tools.Release();
        }

        public void Clear(string toolName)
        {   
            foreach (var ch in this)
            {
                if (string.IsNullOrWhiteSpace(toolName))
                {
                    foreach (var tool in ch.Tools)
                    {
                        var editor = tool as IAnnotationEditor;
                        if (editor != null) editor.Clear();
                    }
                }
                else
                {
                    var editor = ch.Tools.FindByName(toolName) as IAnnotationEditor;
                    if (editor != null) editor.Clear();
                }
            }
        }

		public void LoadChart(string dir)
		{
            foreach (var ch in this)
            {
                string path = Path.Combine(dir, this.Name + "_" + ch.Tag + ".chart");
                if (File.Exists(path))
                    ch.LoadChart(path);
                else ch.SaveChart(path);
            }
		}

		public void SaveChart(string dir)
		{
            foreach (var ch in this)
            {
                string path = Path.Combine(dir, this.Name + "_" + ch.Tag + ".chart");
                ch.SaveChart(path);
            }
        }
        
		public void SetDate(DateTime tEnd)
		{
            foreach (var ch in this) ch.SetDate(tEnd);
        }

        public void SetLineVisible(bool bVisible)
        {
            foreach (var ch in this)
                ch.SetLineVisible(bVisible);
        }
        
        public void SetBarVisible(bool bVisible)
        {
            foreach (var ch in this)
                ch.SetBarVisible(bVisible);
        }

        public void SetUseMagnet(bool bUseMagnet)
        {
            foreach (var ch in this)
                ch.GetInternal().UseMagnet = bUseMagnet;
        }

        #endregion

        #region Indexer

        public IMyChart this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: return Chart0;
                    case 1: return Chart1;
                    case 2: return Chart2;
                    case 3: return Chart3;
                    case 4: return Chart4;
                    case 5: return Chart5;
                    case 6: return Chart6;
                }
                return null;
            }
        }

        public ChartEx GetChart(int idx)
        {
            switch (idx)
            {
                case 0: return Chart0.GetInternal();
                case 1: return Chart1.GetInternal();
                case 2: return Chart2.GetInternal();
                case 3: return Chart3.GetInternal();
                case 4: return Chart4.GetInternal();
                case 5: return Chart5.GetInternal();
                case 6: return Chart6.GetInternal();
            }
            return null;
        }

        #endregion

        #region IEnumerable :: IMyChart

        public IEnumerator<IMyChart> GetEnumerator()
        {
            for (int i = 0; i <= 6; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region EventHandler :: MyChartGroup

        void MyChartGroup_MouseDown(object sender, MouseEventArgs e)
        {
            SelectedChart = sender as ChartEx;
        }

        #endregion

        #region EventHandler :: spliters

        private void split_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Chart1.Focus();
        }

        #endregion
    }
}
