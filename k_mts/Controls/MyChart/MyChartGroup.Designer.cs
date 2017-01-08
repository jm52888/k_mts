namespace k_mts
{
	partial class MyChartGroup
	{
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.split_v1 = new System.Windows.Forms.SplitContainer();
            this.split_h1 = new System.Windows.Forms.SplitContainer();
            this.Chart1 = new k_mts.MyChart();
            this.Chart0 = new k_mts.MyChart2();
            this.Chart4 = new k_mts.MyChart();
            this.split_v2 = new System.Windows.Forms.SplitContainer();
            this.split_h2 = new System.Windows.Forms.SplitContainer();
            this.Chart2 = new k_mts.MyChart();
            this.Chart5 = new k_mts.MyChart();
            this.split_h3 = new System.Windows.Forms.SplitContainer();
            this.Chart3 = new k_mts.MyChart();
            this.Chart6 = new k_mts.MyChart();
            ((System.ComponentModel.ISupportInitialize)(this.split_v1)).BeginInit();
            this.split_v1.Panel1.SuspendLayout();
            this.split_v1.Panel2.SuspendLayout();
            this.split_v1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_h1)).BeginInit();
            this.split_h1.Panel1.SuspendLayout();
            this.split_h1.Panel2.SuspendLayout();
            this.split_h1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_v2)).BeginInit();
            this.split_v2.Panel1.SuspendLayout();
            this.split_v2.Panel2.SuspendLayout();
            this.split_v2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_h2)).BeginInit();
            this.split_h2.Panel1.SuspendLayout();
            this.split_h2.Panel2.SuspendLayout();
            this.split_h2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_h3)).BeginInit();
            this.split_h3.Panel1.SuspendLayout();
            this.split_h3.Panel2.SuspendLayout();
            this.split_h3.SuspendLayout();
            this.SuspendLayout();
            // 
            // split_v1
            // 
            this.split_v1.BackColor = System.Drawing.Color.Transparent;
            this.split_v1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_v1.Location = new System.Drawing.Point(0, 0);
            this.split_v1.Margin = new System.Windows.Forms.Padding(0);
            this.split_v1.Name = "split_v1";
            this.split_v1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_v1.Panel1
            // 
            this.split_v1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.split_v1.Panel1.Controls.Add(this.split_h1);
            // 
            // split_v1.Panel2
            // 
            this.split_v1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.split_v1.Panel2.Controls.Add(this.split_v2);
            this.split_v1.Size = new System.Drawing.Size(640, 480);
            this.split_v1.SplitterDistance = 158;
            this.split_v1.SplitterWidth = 1;
            this.split_v1.TabIndex = 0;
            this.split_v1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // split_h1
            // 
            this.split_h1.BackColor = System.Drawing.Color.Transparent;
            this.split_h1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_h1.Location = new System.Drawing.Point(0, 0);
            this.split_h1.Margin = new System.Windows.Forms.Padding(0);
            this.split_h1.Name = "split_h1";
            // 
            // split_h1.Panel1
            // 
            this.split_h1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.split_h1.Panel1.Controls.Add(this.Chart1);
            this.split_h1.Panel1.Controls.Add(this.Chart0);
            // 
            // split_h1.Panel2
            // 
            this.split_h1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.split_h1.Panel2.Controls.Add(this.Chart4);
            this.split_h1.Size = new System.Drawing.Size(640, 158);
            this.split_h1.SplitterDistance = 360;
            this.split_h1.SplitterWidth = 2;
            this.split_h1.TabIndex = 0;
            this.split_h1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // Chart1
            // 
            this.Chart1.DataSource = null;
            this.Chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart1.Location = new System.Drawing.Point(0, 0);
            this.Chart1.Margin = new System.Windows.Forms.Padding(0);
            this.Chart1.Name = "Chart1";
            this.Chart1.Size = new System.Drawing.Size(360, 158);
            this.Chart1.TabIndex = 0;
            this.Chart1.Tag = "1";
            // 
            // Chart0
            // 
            this.Chart0.DataSource = null;
            this.Chart0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart0.Location = new System.Drawing.Point(0, 0);
            this.Chart0.Margin = new System.Windows.Forms.Padding(0);
            this.Chart0.Name = "Chart0";
            this.Chart0.Size = new System.Drawing.Size(360, 158);
            this.Chart0.TabIndex = 1;
            this.Chart0.Tag = "0";
            // 
            // Chart4
            // 
            this.Chart4.DataSource = null;
            this.Chart4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart4.Location = new System.Drawing.Point(0, 0);
            this.Chart4.Margin = new System.Windows.Forms.Padding(0);
            this.Chart4.Name = "Chart4";
            this.Chart4.Size = new System.Drawing.Size(278, 158);
            this.Chart4.TabIndex = 0;
            this.Chart4.Tag = "4";
            // 
            // split_v2
            // 
            this.split_v2.BackColor = System.Drawing.Color.Transparent;
            this.split_v2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_v2.Location = new System.Drawing.Point(0, 0);
            this.split_v2.Margin = new System.Windows.Forms.Padding(0);
            this.split_v2.Name = "split_v2";
            this.split_v2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split_v2.Panel1
            // 
            this.split_v2.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.split_v2.Panel1.Controls.Add(this.split_h2);
            // 
            // split_v2.Panel2
            // 
            this.split_v2.Panel2.Controls.Add(this.split_h3);
            this.split_v2.Size = new System.Drawing.Size(640, 321);
            this.split_v2.SplitterDistance = 156;
            this.split_v2.SplitterWidth = 1;
            this.split_v2.TabIndex = 0;
            this.split_v2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // split_h2
            // 
            this.split_h2.BackColor = System.Drawing.Color.Transparent;
            this.split_h2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_h2.Location = new System.Drawing.Point(0, 0);
            this.split_h2.Margin = new System.Windows.Forms.Padding(0);
            this.split_h2.Name = "split_h2";
            // 
            // split_h2.Panel1
            // 
            this.split_h2.Panel1.Controls.Add(this.Chart2);
            // 
            // split_h2.Panel2
            // 
            this.split_h2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.split_h2.Panel2.Controls.Add(this.Chart5);
            this.split_h2.Size = new System.Drawing.Size(640, 156);
            this.split_h2.SplitterDistance = 360;
            this.split_h2.SplitterWidth = 2;
            this.split_h2.TabIndex = 0;
            this.split_h2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // Chart2
            // 
            this.Chart2.DataSource = null;
            this.Chart2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart2.Location = new System.Drawing.Point(0, 0);
            this.Chart2.Margin = new System.Windows.Forms.Padding(0);
            this.Chart2.Name = "Chart2";
            this.Chart2.Size = new System.Drawing.Size(360, 156);
            this.Chart2.TabIndex = 0;
            this.Chart2.Tag = "2";
            // 
            // Chart5
            // 
            this.Chart5.DataSource = null;
            this.Chart5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart5.Location = new System.Drawing.Point(0, 0);
            this.Chart5.Margin = new System.Windows.Forms.Padding(0);
            this.Chart5.Name = "Chart5";
            this.Chart5.Size = new System.Drawing.Size(278, 156);
            this.Chart5.TabIndex = 0;
            this.Chart5.Tag = "5";
            // 
            // split_h3
            // 
            this.split_h3.BackColor = System.Drawing.Color.Transparent;
            this.split_h3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_h3.Location = new System.Drawing.Point(0, 0);
            this.split_h3.Margin = new System.Windows.Forms.Padding(0);
            this.split_h3.Name = "split_h3";
            // 
            // split_h3.Panel1
            // 
            this.split_h3.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.split_h3.Panel1.Controls.Add(this.Chart3);
            // 
            // split_h3.Panel2
            // 
            this.split_h3.Panel2.Controls.Add(this.Chart6);
            this.split_h3.Size = new System.Drawing.Size(640, 164);
            this.split_h3.SplitterDistance = 360;
            this.split_h3.SplitterWidth = 2;
            this.split_h3.TabIndex = 0;
            this.split_h3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.split_SplitterMoved);
            // 
            // Chart3
            // 
            this.Chart3.DataSource = null;
            this.Chart3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart3.Location = new System.Drawing.Point(0, 0);
            this.Chart3.Margin = new System.Windows.Forms.Padding(0);
            this.Chart3.Name = "Chart3";
            this.Chart3.Size = new System.Drawing.Size(360, 164);
            this.Chart3.TabIndex = 0;
            this.Chart3.Tag = "3";
            // 
            // Chart6
            // 
            this.Chart6.DataSource = null;
            this.Chart6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chart6.Location = new System.Drawing.Point(0, 0);
            this.Chart6.Margin = new System.Windows.Forms.Padding(0);
            this.Chart6.Name = "Chart6";
            this.Chart6.Size = new System.Drawing.Size(278, 164);
            this.Chart6.TabIndex = 0;
            this.Chart6.Tag = "6";
            // 
            // MyChartGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(49)))));
            this.Controls.Add(this.split_v1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyChartGroup";
            this.Size = new System.Drawing.Size(640, 480);
            this.split_v1.Panel1.ResumeLayout(false);
            this.split_v1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_v1)).EndInit();
            this.split_v1.ResumeLayout(false);
            this.split_h1.Panel1.ResumeLayout(false);
            this.split_h1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_h1)).EndInit();
            this.split_h1.ResumeLayout(false);
            this.split_v2.Panel1.ResumeLayout(false);
            this.split_v2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_v2)).EndInit();
            this.split_v2.ResumeLayout(false);
            this.split_h2.Panel1.ResumeLayout(false);
            this.split_h2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_h2)).EndInit();
            this.split_h2.ResumeLayout(false);
            this.split_h3.Panel1.ResumeLayout(false);
            this.split_h3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_h3)).EndInit();
            this.split_h3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer split_v1;
		private System.Windows.Forms.SplitContainer split_h1;
		private System.Windows.Forms.SplitContainer split_v2;
		private System.Windows.Forms.SplitContainer split_h2;
		private System.Windows.Forms.SplitContainer split_h3;
        private MyChart Chart1;
        private MyChart Chart4;
        private MyChart Chart2;
        private MyChart Chart5;
        private MyChart Chart3;
        private MyChart Chart6;
        private MyChart2 Chart0;
    }
}
