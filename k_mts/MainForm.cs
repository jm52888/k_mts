using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Media;

namespace k_mts
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        #region Variable and Property

        Thread _workThread = null;

        List<MyButton> gItem = new List<MyButton>();
        List<LabelButton> gChart = new List<LabelButton>();
        List<LabelButton> gTool = new List<LabelButton>();
        List<MyButton> gVisible = new List<MyButton>();

        RightUpMessageFilter msgFilter;

        public bool IsRun
        {
            get
            {
                return _workThread != null && _workThread.IsAlive;
            }
            set
            {
                bool bEnable = !value;
                this.Controls.Invokes(ctrl => ctrl.Enabled = bEnable);
            }
        }

        public string SelectedItem { get; protected set; }
        public string SelectedChart { get; protected set; }
        public string SelectedPeriod
        {
            get
            {
                if (SelectedChart == null) return null;
                return SelectedChart.Filter(ch => char.IsNumber(ch));
            }
        }

		private int Depth
		{
			get
			{
				int depth = 0;
				foreach (TabPage page in ChartTab.TabPages)
				{
					var group = page.GetChild<MyChartGroup>();
					if (group == null) continue;

					depth = (group[0] as MyChart2).Depth;
					break;
				}
				return depth;
			}

			set
			{
				foreach (TabPage page in ChartTab.TabPages)
				{
					var group = page.GetChild<MyChartGroup>();
					if (group == null) continue;

					try
					{
						(group[0] as MyChart2).Depth = value;
					}
					catch (ArgumentException ex) { MsgBox.Show(ex.Message); break; }
				}
			}
		}

        #endregion

        #region Class :: RightUpMessageFilter

        private class RightUpMessageFilter : IMessageFilter
        {
            public MainForm Main { get; set; }
            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == WinApi.WM.RBUTTONUP && Main != null)
                {
                    Main.ReleaseTool();
                }
                return false;
            }
        }

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Global.Main = this;
        }

        private void _InitChartTab()
        {
            foreach (var g in Global.ChartGroups.Values)
            {
                string pageName = "page_" + g.Name;
                ChartTab.TabPages.Add(pageName, g.Name);
                g.Parent = ChartTab.TabPages[pageName];
                g.SetLineVisible(true);
                g.SetBarVisible(true);
                g.SetUseMagnet(false);
            }
            ChartTab.Visible = false; // hide at first            
        }

        private void _InitSkins()
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            SetSkin("Visual Studio 2013 Dark");
        }

        private void _InitButtons()
        {
            // Item Button
            gItem.Adds(btnItem1, btnItem2, btnItem3, btnItem4, btnItem5);
            gItem.ForEach(b => b.ButtonGroup = gItem);

            // Chart Button
            gChart.Adds(
                btnCandle,
                btn5, btn5a, btn5b,
                btn15, btn15a, btn15b,
                btn30, btn30a, btn30b,
                //btn45, btn45a, btn45b,
                btn60, btn60a, btn60b,
                //btn90, btn90a, btn90b,
                btn120, btn120a, btn120b,
                btn240, btn240a, btn240b,
                btn333, btn333a, btn333b,
                btn999, btn999a, btn999b);
            gChart.ForEach(b => b.ButtonGroup = gChart);

            // Tool Button
            //gTool.Adds(
            //    btnSelect, btnHand, btnLine, btnLine2, btnInfLine, btnHLine, btnHInfLine, btnVLine, btnVInfLine,
            //    btnEllipse, btnRectangle, btnHEqual2, btnHEqual, btnVEqual, btnPLine,
            //    btnTriangle, btnPolyline);
            //gTool.ForEach(b => b.ButtonGroup = gTool);

            // Tool Button
            gTool.Adds(
                 btnLine, btnLine2, btnInfLine, btnHLine, btnHInfLine, btnVLine, btnVInfLine,
                btnEllipse, btnRectangle, btnHEqual3, btnHEqual, btnHEqual5, btnVEqual, btnPLine,
                btnTriangle, btnPolyline);
            gTool.ForEach(b => b.ButtonGroup = gTool);

            // Line/Bar Button
            gVisible.Adds(btnLineVisible, btnBarVisible);
            gVisible.ForEach(b => b.ButtonGroup = gVisible);
        }

        #endregion

        #region Method

        public void UpdatePage(bool bFirst = false)
        {
            if (bFirst)
            {
                DateTime tEnd = (DateTime)cbDate.SelectedItem;
                foreach (TabPage page in ChartTab.TabPages)
                {
                    var g = page.GetChild<MyChartGroup>();
                    if (g == null) continue;

                    if (!g.Name.Contains(SelectedItem)) continue;

                    g.SetDate(tEnd);
                }
                if (this.InvokeRequired)
                    this.Invoke(new Action(() => UpdatePage(false)));
                else UpdatePage(false);

                return;
            }

            if (SelectedItem == null ||
                SelectedChart == null) return;

            if (SelectedChart == "candle4")
            {
                MyChartGroup g;

                Candle4Base.Panel1.Controls.Clear();
                Candle4Base.Panel2.Controls.Clear();
                Candle4Base.Panel3.Controls.Clear();
                Candle4Base.Panel4.Controls.Clear();

                if (!Global.ChartGroups.TryGetValue(SelectedItem + "_5", out g)) return;
                g[0].Parent = Candle4Base.Panel1;

                if (!Global.ChartGroups.TryGetValue(SelectedItem + "_15", out g)) return;
                g[0].Parent = Candle4Base.Panel2;

                if (!Global.ChartGroups.TryGetValue(SelectedItem + "_30", out g)) return;
                g[0].Parent = Candle4Base.Panel3;

                if (!Global.ChartGroups.TryGetValue(SelectedItem + "_60", out g)) return;
                g[0].Parent = Candle4Base.Panel4;

                ChartTab.SelectTab(page_candle4);
            }
            else // default
            {
                string pageName = "page_" + SelectedItem + "_" + SelectedPeriod;
                var page = ChartTab.TabPages[pageName];
                if (page == null) return;

                var g = page.GetChild<MyChartGroup>();
                if (g == null) return;

                var chartType = SelectedChart.Filter(ch => !char.IsNumber(ch));
                switch (chartType)
                {
                    case "": g.Mode = MyChartGroup.ViewMode.Bar6; break;
                    case "a": g.Mode = MyChartGroup.ViewMode.Candle1; break;
                    case "b": g.Mode = MyChartGroup.ViewMode.Candle1_Bar4; break;
                }
                ChartTab.SelectTab(page);
            }
            ChartTab.Visible = true;
        }

        public void SetSkin(string skinName)
        {
            this.LookAndFeel.SkinName = skinName;

            string nextSkin = "Visual Studio 2013 Dark";
            switch (skinName)
            {
                case "Visual Studio 2013 Dark": nextSkin = "Darkroom"; break;
                    //case "Darkroom": nextSkin = "DevExpress Dark Style"; break;
                    //case "DevExpress Dark Style": nextSkin = "McSkin"; break;
                    //case "McSkin": nextSkin = "Metropolis Dark"; break;
                    //case "Metropolis Dark": nextSkin = "Darkroom"; break;
            }
            btnChangeSkin.Text = nextSkin;
        }

        public void SelectTool(string toolName)
        {
            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.Select(toolName);
            }
        }

        public void ClearTool(string toolName)
        {
            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.Clear(toolName);
            }
        }

        public void ReleaseTool()
        {
            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.Release();
            }

            gTool.ForEach(b =>
            {
                if (b.IsChecked)
                    b.State = ButtonState.Pushed;
            });
        }

        #endregion        

        #region EventHandler :: MainForm

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            msgFilter = new RightUpMessageFilter();
            msgFilter.Main = this;

            Application.AddMessageFilter(msgFilter);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.RemoveMessageFilter(msgFilter);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _InitSkins();
            _InitButtons();
            _InitChartTab();

            cbDate.SelectedIndex = cbDate.Items.Count - 1;

            Global.SetTitle();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRun)
            {
                if (MsgBox.YesNo("현재 스레드가 실행중입니다. 강제종료 하시겠습니까?")
                    == DialogResult.Yes)
                {
                    _workThread.Abort();
                }
                else e.Cancel = true;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.Uninit();
        }

        #endregion

        #region EventHandler :: Item Button

        private void btnItem_Click(object sender, EventArgs e)
        {
            SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.k1);
            _player.Play();

            var btn = sender as MyButton;
            if (btn == null) return;

            SelectedItem = btn.Tag as string;
			// ??? 버튼을 눌렀을 때
			if(SelectedItem == "")
			{
				Depth -= 10;
				return;
			}

			if (SelectedItem == null) return;

            if (!Global.DB.Tables.Contains(SelectedItem + "_5"))
            {
                if (IsRun) return;

                _workThread = new Thread(() =>
                {
                    try
                    {
                        Global.SetTitle(SelectedItem + ".xlsb");
                        DBManager.LoadHistorical(Path.Combine(Global.DataPath, SelectedItem + ".xlsb"));

                        List<Thread> ths = new List<Thread>();
                        foreach (TabPage page in ChartTab.TabPages)
                        {
                            var g = page.GetChild<MyChartGroup>();
                            if (g == null) continue;

                            if (!g.Name.Contains(SelectedItem)) continue;

                            var th = new Thread(() =>
                            {
                                g.Calculate_Historical();
                                Global.SetTitle(g.Name);
                                g.SetData();
                            });
                            ths.Add(th);
                        }
                        ths.ForEach(th => th.Start());
                        ths.ForEach(th => th.Join());
                        Global.SetTitle();
                        UpdatePage(true);
                    }
                    catch (Exception ex) { MsgBox.Show(ex.Message); }
                    finally { IsRun = false; }
                });
                IsRun = true;
                _workThread.Start();
            }
            else UpdatePage();
        }

        #endregion

        #region EventHandler :: Tool Button

        private void btnTool_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            var btn = sender as LabelButton;

            string toolName = btn.Tag as string;
            if (toolName == null) return;

            if (btn.State == ButtonState.Checked)
            {
                SelectTool(toolName);
            }
            else if (btn.State == ButtonState.Pushed)
            {
                ReleaseTool();
            }
            else if (btn.State == ButtonState.Normal)
            {
                ClearTool(toolName);
            }
        }

        private void btnClone_Click(object sender, EventArgs e)
        {
            var btn = sender as LabelButton;

            var page = ChartTab.SelectedTab;
            if (page == null) return;

            var g = page.GetChild<MyChartGroup>();
            if (g == null) return;

            var ch = g.SelectedChart;
            if (ch == null) return;

            var anot = ch.Annotations.FirstOrDefault(a => a.IsSelected);
            if (anot == null) return;

            var editor = ch.Tools.FindByName(anot.GetAttr<string>("ToolName")) as IAnnotationEditor;
            if (editor == null) return;

            string toolName = btn.Tag as string;
            if (toolName == null) return;

            anot.IsSelected = false;
            switch (toolName)
            {
                case "ReflectLeft":
                    anot = editor.Reflect(anot, FlowDirection.RightToLeft, true); break;

                case "ReflectRight":
                    anot = editor.Reflect(anot, FlowDirection.LeftToRight, true); break;

                case "ReflectUp":
                    anot = editor.Reflect(anot, FlowDirection.BottomUp, true); break;

                case "ReflectDown":
                    anot = editor.Reflect(anot, FlowDirection.TopDown, true); break;

                case "TranslateUp":
                    anot = editor.Translate(anot, 0.0, 0.1, true); break;

                case "TranslateDown":
                    anot = editor.Translate(anot, 0.0, -0.1, true); break;

                case "TranslateRight":
                    anot = editor.Translate(anot, 0.1, 0.0, true); break;

                case "TranslateLeft":
                    anot = editor.Translate(anot, -0.1, 0.0, true); break;
            }
            if (anot != null) anot.IsSelected = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();
            foreach (var g in Global.ChartGroups.Values)
                g.SaveChart(Global.ChartPath);
        }

        private void btnGrid_Click(object sender, EventArgs e)
        {
            if (SelectedItem == null ||
                SelectedPeriod == null) return;

            string tblName = SelectedItem + "_" + SelectedPeriod + "_1";
            var dt = Global.DB.Tables[tblName];
            if (dt == null) return;

            Form f = new Form();
            DataGridView dgv = new DataGridView();
            dgv.DataSource = dt;
            dgv.Dock = DockStyle.Fill;
            dgv.Parent = f;
            f.Show();
        }

        private void btnUseMagnet_Click(object sender, EventArgs e)
        {
            var btn = sender as LabelButton;

            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.SetUseMagnet(btn.IsChecked);
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.Clear(null);
            }
        }

        #endregion

        #region EventHandler :: Chart Button

        private void btnChart_Click(object sender, EventArgs e)
        {
            SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.k1);
            _player.Play();

            var btn = sender as LabelButton;
            if (btn == null) return;

            if (string.IsNullOrEmpty(SelectedItem))
            {
                MsgBox.Show("먼저 종목을 선택하세요");
                return;
            }

            SelectedChart = btn.Tag as string;

            UpdatePage();
        }

        private void btnVisible_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                if (btnLineVisible.State == ButtonState.Checked)
                {
                    g.SetLineVisible(true);
                    g.SetBarVisible(false);
                }
                else if (btnBarVisible.State == ButtonState.Checked)
                {
                    g.SetLineVisible(false);
                    g.SetBarVisible(true);
                }
                else
                {
                    g.SetLineVisible(true);
                    g.SetBarVisible(true);
                }
            }
        }

        #endregion

        #region EventHandler :: Date ComboBox, Next/Prev Button

        private void btnPrev_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            int idx = cbDate.SelectedIndex;
            if (idx > 0)
                cbDate.SelectedIndex--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            int idx = cbDate.SelectedIndex;
            int nCnt = cbDate.Items.Count;
            if (nCnt > 0 && idx + 1 < nCnt)
                cbDate.SelectedIndex++;
        }

        private void cbDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            if (cbDate.SelectedIndex < 0) return;

            DateTime tEnd = (DateTime)cbDate.SelectedItem;
            foreach (TabPage page in ChartTab.TabPages)
            {
                var g = page.GetChild<MyChartGroup>();
                if (g == null) continue;

                g.SetDate(tEnd);
            }
        }

        private void FillDateBox(string sItem, int term)
        {
            cbDate.Items.Clear();
            if (string.IsNullOrWhiteSpace(sItem)) return;

            HashSet<string> dates = new HashSet<string>();
            for (int i = 1; i <= 6; i++)
            {
                DataTable dt = Global.DB.Tables[sItem + "." + term + ".chart" + i];
                if (dt == null) continue;

                var rows = dt.Rows;
                int nRow = rows.Count;
                for (int r = 0; r < nRow; r++)
                    dates.Add(rows[r]["date"].ToString().Split('_')[0]);
            }
            cbDate.Items.AddRange(dates.ToArray());
            cbDate.Items.Add("전체보기");
        }

        #endregion

        #region EventHandler :: ETC

        private void btnChangeSkin_Click(object sender, EventArgs e)
        {
            string skinName = btnChangeSkin.Text;
            SetSkin(skinName);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            new SettingForm().ShowDialog();
        }

        #endregion

        #region EventHandler :: Timer

        private void tmMain_Tick(object sender, EventArgs e)
        {
            lblCurTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
        public String candle_s
        {
            get { return this.btn_Style.Text; }
            set { this.btn_Style.Text = value; }
        }

        private void btn_Style_Click(object sender, EventArgs e)
        {
            //SoundPlayer _player = new SoundPlayer(k_mts.Properties.Resources.w1);
            //_player.Play();

            if (btn_Style.Text == "Candle_s")
            {
                btn_Style.Text = "Candle_A";
            }
            else if(btn_Style.Text == "Candle_A")
            {
                btn_Style.Text = "Candle_B";
            }
            else if (btn_Style.Text == "Candle_B")
            {
                btn_Style.Text = "Candle_C";
            }
            else if (btn_Style.Text == "Candle_C")
            {
                btn_Style.Text = "Candle_D";
            }
            else if (btn_Style.Text == "Candle_D")
            {
                btn_Style.Text = "Candle_s";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //public double numcount
        //{
        //    get { return this.numericUpDown1.Value; }
        //    set { this.numericUpDown1.Value = value; }
        //}

    }
}


