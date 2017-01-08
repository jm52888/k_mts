namespace k_mts
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.split_main = new System.Windows.Forms.SplitContainer();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCandle = new System.Windows.Forms.Button();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnTool = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel_main = new System.Windows.Forms.MyPanel();
            this.tabMain = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.split_main)).BeginInit();
            this.split_main.Panel1.SuspendLayout();
            this.split_main.Panel2.SuspendLayout();
            this.split_main.SuspendLayout();
            this.panel_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // split_main
            // 
            this.split_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_main.Location = new System.Drawing.Point(0, 0);
            this.split_main.Name = "split_main";
            // 
            // split_main.Panel1
            // 
            this.split_main.Panel1.Controls.Add(this.btnSave);
            this.split_main.Panel1.Controls.Add(this.btnClose);
            this.split_main.Panel1.Controls.Add(this.btnCandle);
            this.split_main.Panel1.Controls.Add(this.btnChart);
            this.split_main.Panel1.Controls.Add(this.btnTool);
            this.split_main.Panel1.Controls.Add(this.button1);
            // 
            // split_main.Panel2
            // 
            this.split_main.Panel2.Controls.Add(this.panel_main);
            this.split_main.Size = new System.Drawing.Size(736, 385);
            this.split_main.SplitterDistance = 135;
            this.split_main.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(0, 317);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(135, 34);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(0, 351);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(135, 34);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCandle
            // 
            this.btnCandle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnCandle.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCandle.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnCandle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCandle.Location = new System.Drawing.Point(0, 98);
            this.btnCandle.Name = "btnCandle";
            this.btnCandle.Size = new System.Drawing.Size(135, 34);
            this.btnCandle.TabIndex = 7;
            this.btnCandle.Text = "CANDLE";
            this.btnCandle.UseVisualStyleBackColor = false;
            this.btnCandle.Click += new System.EventHandler(this.btnCandle_Click);
            // 
            // btnChart
            // 
            this.btnChart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChart.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChart.Location = new System.Drawing.Point(0, 64);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(135, 34);
            this.btnChart.TabIndex = 6;
            this.btnChart.Text = "CHART";
            this.btnChart.UseVisualStyleBackColor = false;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // btnTool
            // 
            this.btnTool.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTool.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTool.Location = new System.Drawing.Point(0, 30);
            this.btnTool.Name = "btnTool";
            this.btnTool.Size = new System.Drawing.Size(135, 34);
            this.btnTool.TabIndex = 5;
            this.btnTool.Text = "TOOL";
            this.btnTool.UseVisualStyleBackColor = false;
            this.btnTool.Click += new System.EventHandler(this.btnTool_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Aqua;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "EDIT";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // panel_main
            // 
            this.panel_main.Controls.Add(this.tabMain);
            this.panel_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_main.Location = new System.Drawing.Point(0, 0);
            this.panel_main.Name = "panel_main";
            this.panel_main.Size = new System.Drawing.Size(597, 385);
            this.panel_main.TabIndex = 0;
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.ItemSize = new System.Drawing.Size(64, 27);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(597, 385);
            this.tabMain.TabIndex = 0;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 385);
            this.Controls.Add(this.split_main);
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.split_main.Panel1.ResumeLayout(false);
            this.split_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_main)).EndInit();
            this.split_main.ResumeLayout(false);
            this.panel_main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer split_main;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCandle;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.Button btnTool;
        private System.Windows.Forms.MyPanel panel_main;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabMain;
    }
}