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
            this.hsViewStart = new System.Windows.Forms.HScrollBar();
            this.txtDataCount = new System.Windows.Forms.TextBox();
            this.nudViewCount = new System.Windows.Forms.MyNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudViewCount)).BeginInit();
            this.SuspendLayout();
            // 
            // hsViewStart
            // 
            this.hsViewStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsViewStart.LargeChange = 1;
            this.hsViewStart.Location = new System.Drawing.Point(48, 0);
            this.hsViewStart.Maximum = 9999999;
            this.hsViewStart.Name = "hsViewStart";
            this.hsViewStart.Size = new System.Drawing.Size(375, 13);
            this.hsViewStart.TabIndex = 9;
            this.hsViewStart.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsViewStart_Scroll);
            this.hsViewStart.ValueChanged += new System.EventHandler(this.hsViewStart_ValueChanged);
            // 
            // txtDataCount
            // 
            this.txtDataCount.AcceptsReturn = true;
            this.txtDataCount.BackColor = System.Drawing.SystemColors.Window;
            this.txtDataCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtDataCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.txtDataCount.Location = new System.Drawing.Point(0, 0);
            this.txtDataCount.MaxLength = 7;
            this.txtDataCount.Name = "txtDataCount";
            this.txtDataCount.ReadOnly = true;
            this.txtDataCount.Size = new System.Drawing.Size(48, 14);
            this.txtDataCount.TabIndex = 8;
            this.txtDataCount.Text = "2000";
            this.txtDataCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudViewCount
            // 
            this.nudViewCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudViewCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.nudViewCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.nudViewCount.Location = new System.Drawing.Point(423, 0);
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
            this.nudViewCount.TabIndex = 7;
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
            this.Controls.Add(this.nudViewCount);
            this.Controls.Add(this.txtDataCount);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MyChartController";
            this.Size = new System.Drawing.Size(480, 13);
            ((System.ComponentModel.ISupportInitialize)(this.nudViewCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.HScrollBar hsViewStart;
		protected System.Windows.Forms.MyNumericUpDown nudViewCount;
		protected System.Windows.Forms.TextBox txtDataCount;
	}
}
