using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace System.Windows.Forms
{
	public class InputBox : Form
	{
		private const int CS_DROPSHADOW = 0x00020000;
		protected Label lblMessage;
		protected TextBox txtInput;
		protected string _txtInput;
		protected bool _txtPaintInvalidated = false;

		private InputBox()
		{
			Panel pl = new Panel();
			pl.Dock = DockStyle.Fill;

			FlowLayoutPanel flp = new FlowLayoutPanel();
			flp.Dock = DockStyle.Fill;

			lblMessage = new Label();
			lblMessage.Font = new Font("Segoe UI", 10);
			lblMessage.ForeColor = Color.White;
			lblMessage.AutoSize = true;

			Panel txtPl = new Panel();
			txtPl.BorderStyle = BorderStyle.None;
			txtPl.Width = 360;
			txtPl.Height = 28;
			txtPl.Padding = new Padding(5);
			txtPl.BackColor = Color.White;
			txtPl.Margin = new Padding(0, 15, 0, 0);
			txtPl.Paint += txtPl_Paint;

			txtInput = new TextBox();
			txtInput.Dock = DockStyle.Fill;
			txtInput.BorderStyle = BorderStyle.None;
			txtInput.Font = new Font("Segoe UI", 9);
			txtInput.KeyDown += txtInput_KeyDown;
			txtInput.BackColor = Color.FromArgb(240, 240, 240);
			txtInput.Multiline = true;
			txtPl.Controls.Add(txtInput);

			FlowLayoutPanel flpButtons = new FlowLayoutPanel();
			flpButtons.Dock = DockStyle.Bottom;
			flpButtons.FlowDirection = FlowDirection.RightToLeft;
			flpButtons.Height = 35;

			Button btnCancel = new Button();
			btnCancel.Text = "Cancel";
			btnCancel.ForeColor = Color.FromArgb(170, 170, 170);
			btnCancel.Font = new Font("Segoe UI", 8);
			btnCancel.Padding = new Padding(3);
			btnCancel.FlatStyle = FlatStyle.Flat;
			btnCancel.Height = 30;
			btnCancel.Click += btnCancel_Click;

			Button btnOK = new Button();
			btnOK.Text = "OK";
			btnOK.ForeColor = Color.FromArgb(170, 170, 170);
			btnOK.Font = new System.Drawing.Font("Segoe UI", 8);
			btnOK.Padding = new Padding(3);
			btnOK.FlatStyle = FlatStyle.Flat;
			btnOK.Height = 30;
			btnOK.Click += btnOK_Click;

			flpButtons.Controls.Add(btnCancel);
			flpButtons.Controls.Add(btnOK);

			flp.Controls.Add(lblMessage);
			flp.SetFlowBreak(lblMessage, true);
			flp.Controls.Add(txtPl);
			flp.SetFlowBreak(txtPl, true);
			flp.Controls.Add(flpButtons);
			pl.Controls.Add(flp);

			this.Controls.Add(pl);
			this.Controls.Add(flpButtons);
			this.FormBorderStyle = FormBorderStyle.None;
			this.BackColor = Color.FromArgb(45, 45, 48);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Padding = new Padding(20);
			this.Width = 400;
			this.Height = 200;

			this.DialogResult = DialogResult.Cancel;
		}

		void txtInput_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox txt = (TextBox)sender;

			if (e.KeyCode == Keys.Enter)
			{
				_txtInput = txt.Text;
				this.DialogResult = DialogResult.OK;
				this.Dispose();
			}
			else if (e.KeyCode == Keys.Escape)
			{
				this.DialogResult = DialogResult.Cancel;
				this.Dispose();
			}
			else
			{
				if (txt.Text.Length > 60)
				{
					txt.Parent.Height = 80;

					if (!_txtPaintInvalidated)
					{
						txt.Parent.Invalidate();
						_txtPaintInvalidated = true;
					}
				}

				if (txt.Text.Length < 60)
				{
					txt.Parent.Height = 28;

					if (_txtPaintInvalidated)
					{
						txt.Parent.Invalidate();
						_txtPaintInvalidated = false;
					}
				}
			}
		}

		void txtPl_Paint(object sender, PaintEventArgs e)
		{
			Panel pl = (Panel)sender;
			base.OnPaint(e);

			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(new Point(0, 0), new Size(pl.Width - 1, pl.Height - 1));
			Pen pen = new Pen(Color.FromArgb(0, 151, 251));
			pen.Width = 3;
			g.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), rect);
			g.DrawRectangle(pen, rect);
		}

		void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Dispose();
		}

		void btnOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			_txtInput = txtInput.Text;
			this.Dispose();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= CS_DROPSHADOW;
				return cp;
			}
		}

		public static string Show(string message)
		{
			InputBox dialog = new InputBox();
			dialog.lblMessage.Text = message;
			dialog.ShowDialog();

			return dialog._txtInput;
		}

		public static DialogResult Show(string message, ref string text)
		{
			InputBox dialog = new InputBox();
			dialog.lblMessage.Text = message;

			if (!string.IsNullOrEmpty(text))
				dialog._txtInput = text;

			if (dialog.ShowDialog() == DialogResult.OK)
				text = dialog._txtInput;

			return dialog.DialogResult;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics g = e.Graphics;
			Rectangle rect = new Rectangle(new Point(0, 0), new Size(this.Width - 1, this.Height - 1));
			Pen pen = new Pen(Color.FromArgb(0, 151, 251));

			g.DrawRectangle(pen, rect);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// OtsInputBox
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "OtsInputBox";
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);

		}
	}
}