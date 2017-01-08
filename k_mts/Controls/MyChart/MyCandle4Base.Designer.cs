namespace k_mts.Controls
{
	partial class MyCandle4Base
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
            this.split_h1 = new System.Windows.Forms.SplitContainer();
            this.split_v2 = new System.Windows.Forms.SplitContainer();
            this.split_v1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.split_h1)).BeginInit();
            this.split_h1.Panel1.SuspendLayout();
            this.split_h1.Panel2.SuspendLayout();
            this.split_h1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_v2)).BeginInit();
            this.split_v2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_v1)).BeginInit();
            this.split_v1.SuspendLayout();
            this.SuspendLayout();
            // 
            // split_h1
            // 
            this.split_h1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_h1.Location = new System.Drawing.Point(0, 0);
            this.split_h1.Name = "split_h1";
            // 
            // split_h1.Panel1
            // 
            this.split_h1.Panel1.Controls.Add(this.split_v1);
            // 
            // split_h1.Panel2
            // 
            this.split_h1.Panel2.Controls.Add(this.split_v2);
            this.split_h1.Size = new System.Drawing.Size(600, 319);
            this.split_h1.SplitterDistance = 302;
            this.split_h1.TabIndex = 0;
            // 
            // split_v2
            // 
            this.split_v2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_v2.Location = new System.Drawing.Point(0, 0);
            this.split_v2.Name = "split_v2";
            this.split_v2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.split_v2.Size = new System.Drawing.Size(294, 319);
            this.split_v2.SplitterDistance = 159;
            this.split_v2.TabIndex = 0;
            // 
            // split_v1
            // 
            this.split_v1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_v1.Location = new System.Drawing.Point(0, 0);
            this.split_v1.Name = "split_v1";
            this.split_v1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.split_v1.Size = new System.Drawing.Size(302, 319);
            this.split_v1.SplitterDistance = 159;
            this.split_v1.TabIndex = 0;
            // 
            // MyCandle4Base
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.split_h1);
            this.Name = "MyCandle4Base";
            this.Size = new System.Drawing.Size(600, 319);
            this.split_h1.Panel1.ResumeLayout(false);
            this.split_h1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_h1)).EndInit();
            this.split_h1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_v2)).EndInit();
            this.split_v2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_v1)).EndInit();
            this.split_v1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.SplitContainer split_h1;
        private System.Windows.Forms.SplitContainer split_v1;
        private System.Windows.Forms.SplitContainer split_v2;
    }
}
