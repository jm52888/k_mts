using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	public class DateComboBox : ComboBox
	{
        private Brush BorderBrush = new SolidBrush(Color.FromArgb(33, 33, 35));
        private Brush ArrowBrush = new SolidBrush(Color.Blue);
        private Brush DropButtonBrush = new SolidBrush(Color.Red);

        private Color _borderColor = Color.Black;
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        //private static int WM_PAINT = 0x000F;

        private Color _ButtonColor = SystemColors.Control;//Control;
        #region Variable and Property

        private DateTime _StartDate = DateTime.Now.Date.AddYears(-10);
		[Category("날짜")]
		public DateTime StartDate 
		{
			get { return _StartDate; }
			set { _StartDate = value; }
		}

		private DateTime _EndDate = DateTime.Now.Date;
		[Category("날짜")]
		public DateTime EndDate 
		{
			get { return _EndDate; }
			set { _EndDate = value; }
		}

		private bool _AutoIncrease = false;
		[Category("날짜"), DefaultValue(false)]
		public bool AutoIncrease 
		{
			get { return _AutoIncrease; }
			set { _AutoIncrease = value; }
		}

		#endregion

		#region Constructor

		public DateComboBox()
		{
			if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) 
				return;
			
			_Initialize();
		}

		private void _Initialize()
		{
			List<DateTime> lst = new List<DateTime>();
			DateTime t = StartDate;
			while (t <= EndDate)
			{
				lst.Add(t);
				t = t.AddDays(1);
			}
			base.DataSource = lst;
		}

		#endregion

		#region override

		protected override void OnDropDown(EventArgs e)
		{
			var lst = base.DataSource as List<DateTime>;
			if (_AutoIncrease && lst != null)
			{
				var dtNow = DateTime.Now.Date;
				if (!lst.Contains(dtNow))
					lst.Add(dtNow);
			}
			base.OnDropDown(e);
		}

        #endregion
        public Color ButtonColor
        {
            get { return _ButtonColor; }
            set
            {
                _ButtonColor = value;
                DropButtonBrush = new SolidBrush(this.ButtonColor);
                this.Invalidate();
            }
        }
        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        protected override void OnLostFocus(System.EventArgs e)
        {
            base.OnLostFocus(e);
            this.Invalidate();
        }

        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            this.Invalidate();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case 0xf:
                    Graphics g = this.CreateGraphics();
                    Pen p = new Pen(Color.Red);
                    g.FillRectangle(BorderBrush, this.ClientRectangle);

                    //Draw the background of the dropdown button
                    Rectangle rect = new Rectangle(this.Width - 0, 0, 0, this.Height);//17
                    g.FillRectangle(DropButtonBrush, rect);

                    //Create the path for the arrow
                    System.Drawing.Drawing2D.GraphicsPath pth = new System.Drawing.Drawing2D.GraphicsPath();
                    PointF TopLeft = new PointF(this.Width - 13, (this.Height - 5) / 2);//-13
                    PointF TopRight = new PointF(this.Width - 6, (this.Height - 5) / 2);//-6
                    PointF Bottom = new PointF(this.Width - 9, (this.Height + 2) / 2);//-9
                    pth.AddLine(TopLeft, TopRight);
                    pth.AddLine(TopRight, Bottom);

                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //Determine the arrow's color.
                    if (this.DroppedDown)
                    {
                        ArrowBrush = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        ArrowBrush = new SolidBrush(Color.White);
                    }

                    //Draw the arrow
                    g.FillPath(ArrowBrush, pth);

                    break;
                default:
                    break;
            }
        }
    }
}
