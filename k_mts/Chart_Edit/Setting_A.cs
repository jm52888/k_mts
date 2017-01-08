using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace k_mts.Chart_Edit
{
    public partial class Setting_A : Form
    {
        Setting_B ch1 = new Setting_B();
        Setting_C ch2 = new Setting_C();
        public Setting_A()
        {
            InitializeComponent();
            ch1.Parent = myPanel_A;
            ch2.Parent = myPanel_A;
        }

        private void btn_line_Click(object sender, EventArgs e)
        {
            ch1.Hide();
            ch2.Hide();
            tabControl1.Show();
        }

        private void btn_chart_Click(object sender, EventArgs e)
        {
            tabControl1.Hide();
            ch2.Hide();
            ch1.Show();
            ch1.Dock = DockStyle.Fill;
        }

        private void btn_candle_Click(object sender, EventArgs e)
        {
            tabControl1.Hide();
            ch1.Hide();
            ch2.Show();
            ch2.Dock = DockStyle.Fill;
        }

    

        private void pic_A_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog clo = new ColorDialog();
            PictureBox pic = sender as PictureBox;

            if (clo.ShowDialog() == DialogResult.OK)
            {
                pic.BackColor = clo.Color;
            }
            else { pic.BackColor = Color.Black; }

        }

        private void trackBar_A_Scroll(object sender, EventArgs e)
        {
            if (trackBar_A.Value >= 0)
            {
                label22.Text = trackBar_A.Value.ToString();
                pictureBox13.BackColor = Color.FromArgb(trackBar_A.Value, pictureBox13.BackColor);
            }
            if (trackBar2.Value >= 0)
            {
                label23.Text = trackBar2.Value.ToString();
                pictureBox14.BackColor = Color.FromArgb(trackBar2.Value, pictureBox14.BackColor);
            }
            if (trackBar3.Value >= 0)
            {
                label24.Text = trackBar3.Value.ToString();
                pictureBox15.BackColor = Color.FromArgb(trackBar3.Value, pictureBox15.BackColor);
            }
        }

   
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
      

      
    }
}