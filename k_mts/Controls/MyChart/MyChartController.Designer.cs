namespace k_mts
{
	partial class MyChartController
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
            this.btnStart = new System.Windows.Forms.Label();
            this.txtDataCount = new System.Windows.Forms.TextBox();
            this.btnEnd = new System.Windows.Forms.Label();
            this.hsViewStart = new System.Windows.Forms.HScrollBar();
            this.nudViewCount = new System.Windows.Forms.MyNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudViewCount)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnStart.Font = new System.Drawing.Font("Wingdings 3", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(0, 0);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.btnStart.Size = new System.Drawing.Size(33, 11);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "t";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            this.btnStart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnStart_MouseDown);
            this.btnStart.MouseEnter += new System.EventHandler(this.btnStart_MouseEnter);
            this.btnStart.MouseLeave += new System.EventHandler(this.btnStart_MouseLeave);
            this.btnStart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnStart_MouseMove);
            this.btnStart.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnStart_MouseUp);
            // 
            // txtDataCount
            // 
            this.txtDataCount.AcceptsReturn = true;
            this.txtDataCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtDataCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtDataCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.txtDataCount.Location = new System.Drawing.Point(33, 0);
            this.txtDataCount.Margin = new System.Windows.Forms.Padding(0);
            this.txtDataCount.MaxLength = 7;
            this.txtDataCount.Name = "txtDataCount";
            this.txtDataCount.ReadOnly = true;
            this.txtDataCount.Size = new System.Drawing.Size(48, 14);
            this.txtDataCount.TabIndex = 2;
            this.txtDataCount.Text = "2000";
            this.txtDataCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDataCount.MouseEnter += new System.EventHandler(this.txtDataCount_MouseEnter);
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.SystemColors.Control;
            this.btnEnd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEnd.Font = new System.Drawing.Font("Wingdings 3", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnEnd.ForeColor = System.Drawing.Color.Black;
            this.btnEnd.Location = new System.Drawing.Point(447, 0);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(0);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.btnEnd.Size = new System.Drawing.Size(33, 11);
            this.btnEnd.TabIndex = 4;
            this.btnEnd.Text = "u";
            this.btnEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            this.btnEnd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEnd_MouseDown);
            this.btnEnd.MouseEnter += new System.EventHandler(this.btnEnd_MouseEnter);
            this.btnEnd.MouseLeave += new System.EventHandler(this.btnEnd_MouseLeave);
            this.btnEnd.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnEnd_MouseMove);
            this.btnEnd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnEnd_MouseUp);
            // 
            // hsViewStart
            // 
            this.hsViewStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsViewStart.LargeChange = 1;
            this.hsViewStart.Location = new System.Drawing.Point(81, 0);
            this.hsViewStart.Maximum = 9999999;
            this.hsViewStart.Name = "hsViewStart";
            this.hsViewStart.Size = new System.Drawing.Size(309, 11);
            this.hsViewStart.TabIndex = 5;
            this.hsViewStart.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsViewStart_Scroll);
            this.hsViewStart.ValueChanged += new System.EventHandler(this.hsViewStart_ValueChanged);
            this.hsViewStart.MouseEnter += new System.EventHandler(this.hsViewStart_MouseEnter);
            // 
            // nudViewCount
            // 
            this.nudViewCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudViewCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.nudViewCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.nudViewCount.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudViewCount.Location = new System.Drawing.Point(390, 0);
            this.nudViewCount.Margin = new System.Windows.Forms.Padding(0);
            this.nudViewCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudViewCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudViewCount.Name = "nudViewCount";
            this.nudViewCount.Size = new System.Drawing.Size(57, 17);
            this.nudViewCount.TabIndex = 0;
            this.nudViewCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudViewCount.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            this.nudViewCount.ValueChanged += new System.EventHandler(this.nudViewCount_ValueChanged);
            this.nudViewCount.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudViewCount_KeyDown);
            // 
            // MyChartController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hsViewStart);
            this.Controls.Add(this.txtDataCount);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.nudViewCount);
            this.Controls.Add(this.btnEnd);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyChartController";
            this.Size = new System.Drawing.Size(480, 11);
            this.MouseEnter += new System.EventHandler(this.hsViewStart_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.nudViewCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion
        protected System.Windows.Forms.MyNumericUpDown nudViewCount;
        protected System.Windows.Forms.TextBox txtDataCount;
        private System.Windows.Forms.Label btnEnd;
        private System.Windows.Forms.Label btnStart;
        protected System.Windows.Forms.HScrollBar hsViewStart;
    }
}
