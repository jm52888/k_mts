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
using System.Windows.Forms.DataVisualization.Charting;

namespace k_mts
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            
        }

        bool bToolFirst = true;
        private void btnTool_Click(object sender, EventArgs e)
        {
            if (bToolFirst)
            {
                bToolFirst = false;

                var item = Global.Main.SelectedItem;
                if (string.IsNullOrEmpty(item)) return;

                var g1 = Global.ChartGroups.Values.First();
                var gg = Global.ChartGroups.Values.Where(g => g.Name.Contains(item)).ToArray();

                foreach (var tool in g1[0].Tools)
                {
                    tabMain.TabPages.Add("page_" + tool.Name, tool.Name);
                    var page = tabMain.TabPages["page_" + tool.Name];
                    var grid = new PropertyGrid();
                    grid.Dock = DockStyle.Fill;
                    grid.Parent = page;

                    var lst = new List<object>();
                    foreach (var g in gg)
                    {
                        foreach (var ch in g)
                        {
                            var tool2 = ch.Tools.FindByName(tool.Name);
                            if (tool2 == null) continue;

                            lst.Add(tool2);
                        }
                    }
                    grid.SelectedObjects = lst.ToArray();
                }
            }
            tabMain.SelectTab("page_Default");
        }

        bool bChartFirst = true;
        private void btnChart_Click(object sender, EventArgs e)
        {
            if (bChartFirst)
            {
                tabMain.TabPages.Add("page_chart", "Chart");

                var page = tabMain.TabPages["page_chart"];
                if (page == null) return;

                var grid = new PropertyGrid();
                grid.Dock = DockStyle.Fill;
                grid.Parent = page;

                var item = Global.Main.SelectedItem;
                if (string.IsNullOrEmpty(item)) return;

                var lst = new List<ChartEx>();
                foreach (var g in Global.ChartGroups.Values)
                {
                    if (g.Name.Contains(item))
                    {
                        for (int i = 1; i <= 6; i++)
                            lst.Add(g.GetChart(i));
                    }
                }
                grid.SelectedObjects = lst.ToArray();
            }
            tabMain.SelectTab("page_chart");
        }

        bool bCandleFirst = true;
        private void btnCandle_Click(object sender, EventArgs e)
        {
            if (bCandleFirst)
            {
                bCandleFirst = false;

                tabMain.TabPages.Add("page_candle", "Candle");

                var page = tabMain.TabPages["page_candle"];
                if (page == null) return;

                var grid = new PropertyGrid();
                grid.Dock = DockStyle.Fill;
                grid.Parent = page;

                var item = Global.Main.SelectedItem;
                if (string.IsNullOrEmpty(item)) return;

                var lst = new List<ChartEx>();
                foreach (var g in Global.ChartGroups.Values)
                {
                    if (g.Name.Contains(item))
                        lst.Add(g.GetChart(0));
                }
                grid.SelectedObjects = lst.ToArray();
            }
            tabMain.SelectTab("page_candle");
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (MyChartGroup g in Global.ChartGroups.Values)
                g.SaveChart(Global.ChartPath);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
