using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
	public class LabelButton : Label
	{
		#region Variable and Property

		private ButtonStyle _Style = ButtonStyle.Push;
		[DefaultValue(ButtonStyle.Push)]
		public ButtonStyle Style
		{
			get { return _Style; }
			set
			{
				_Style = value;
				_UpdateColor();
			}
		}

		private ButtonState _State = ButtonState.Normal;
		[DefaultValue(ButtonState.Normal)]
		public ButtonState State
		{
			get { return _State; }
			set
			{
				if (_State == value) return;
				else _State = value;

				_UpdateColor();
			}
		}

		private List<LabelButton> _ButtonGroup = null;
		[Browsable(false)]
		public List<LabelButton> ButtonGroup
		{
			get { return _ButtonGroup; }
			set { _ButtonGroup = value; }
		}

		private Color _NormalBackColor = Color.Silver;
		[DefaultValue(typeof(Color), "Silver")]//"Transparent"
        public Color NormalBackColor 
		{
			get { return _NormalBackColor; }
			set { _NormalBackColor = value; }
		}
		private Color _NormalForeColor = Color.FromArgb(51, 51, 51); //Color.Black;
		[DefaultValue(typeof(Color), "51, 51, 51")]
		public Color NormalForeColor 
		{
			get { return _NormalForeColor; }
			set { _NormalForeColor = value; }
		}

		private Color _PushedBackColor = Color.Transparent;
		[DefaultValue(typeof(Color), "Transparent")]
		public Color PushedBackColor
		{
			get { return _PushedBackColor; }
			set { _PushedBackColor = value; }
		}
		private Color _PushedForeColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color PushedForeColor
		{
			get { return _PushedForeColor; }
			set { _PushedForeColor = value; }
		}

		private Color _CheckedBackColor = Color.Transparent;
		[DefaultValue(typeof(Color), "Transparent")]
		public Color CheckedBackColor
		{
			get { return _CheckedBackColor; }
			set { _CheckedBackColor = value; }
		}
		private Color _CheckedForeColor = Color.FromArgb(51, 51, 51); //Color.Black;
		[DefaultValue(typeof(Color), "51, 51, 51")]
		public Color CheckedForeColor
		{
			get { return _CheckedForeColor; }
			set { _CheckedForeColor = value; }
		}

		private Color _HoverBackColor = Color.Transparent;
		[DefaultValue(typeof(Color), "Transparent")]
		public Color HoverBackColor
		{
			get { return _HoverBackColor; }
			set { _HoverBackColor = value; }
		}
		private Color _HoverForeColor = Color.Black;
		[DefaultValue(typeof(Color), "Black")]
		public Color HoverForeColor
		{
			get { return _HoverForeColor; }
			set { _HoverForeColor = value; }
		}

		[Browsable(false)]
		public bool IsChecked { get { return _State == ButtonState.Checked; } }
		[Browsable(false)]
		public bool IsPushed { get { return _State == ButtonState.Pushed; } }
		[Browsable(false)]
		public bool IsNormal { get { return _State == ButtonState.Normal; } }

		private bool _OnlyOneChecked = false;
		[DefaultValue(false)]
		public bool OnlyOneChecked 
		{
			get { return _OnlyOneChecked; }
			set { _OnlyOneChecked = value; }
		}

		private bool _MustOneChecked = false;
		[DefaultValue(false)]
		public bool MustOneChecked
		{
			get { return _MustOneChecked; }
			set { _MustOneChecked = value; }
		}

		#endregion

		#region Enumeration

		public enum ButtonStyle
		{
			Push,
			Check,
			ThreeState
		}

		#endregion

		#region Constructor

		public LabelButton() 
		{

		}

		#endregion

		#region Method

		public void PerformClick()
		{
			this.OnMouseClick(new MouseEventArgs(Forms.MouseButtons.Left, 1, -1, -1, 0));
		}

		void _UpdateColor()
		{
			switch (_State)
			{
				case ButtonState.Normal:
					BackColor = NormalBackColor;
					ForeColor = NormalForeColor;
					break;

				case ButtonState.Pushed:
					BackColor = PushedBackColor;
					ForeColor = PushedForeColor;
					break;

				case ButtonState.Checked:
					BackColor = CheckedBackColor;
					ForeColor = CheckedForeColor;
					break;
			}
		}

		#endregion

		#region Override

		protected override void OnMouseEnter(EventArgs e)
		{
			BackColor = HoverBackColor;
			ForeColor = HoverForeColor;
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{	
			_UpdateColor();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == Forms.MouseButtons.Right) return;

			if (_Style == ButtonStyle.Push)
			{
				base.BackColor = PushedBackColor;
				base.ForeColor = PushedForeColor;
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == Forms.MouseButtons.Right) return;

			if (_Style == ButtonStyle.Push)
			{
				base.BackColor = HoverBackColor;
				base.ForeColor = HoverForeColor;				
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (e.Button == Forms.MouseButtons.Right) return;

			if (_Style == ButtonStyle.Check)
			{
				if (_State == ButtonState.Normal)
				{
					if (_OnlyOneChecked && _ButtonGroup != null)
					{
						_ButtonGroup.ForEach(b =>
						{
							if (b.IsChecked && b != this) b.State = ButtonState.Normal;
						});
					}
					State = ButtonState.Checked;
				}
				else if (_State == ButtonState.Checked)
				{
					if (_MustOneChecked && _ButtonGroup != null &&
						_ButtonGroup.FirstOrDefault(b => b.IsChecked && b != this) == null) return;

					State = ButtonState.Normal;
				}
			}
			else if (_Style == ButtonStyle.ThreeState)
			{
				if (_State == ButtonState.Normal || _State == ButtonState.Pushed)
				{
					if (_OnlyOneChecked && _ButtonGroup != null)
					{
						_ButtonGroup.ForEach(b =>
						{
							if (b.IsChecked && b != this) b.State = ButtonState.Pushed;
						});
					}
					State = ButtonState.Checked;
				}
				else if (_State == ButtonState.Checked)
				{
					State = ButtonState.Normal;
				}
			}
			_DispatchClick(e);
		}

		protected void _DispatchClick(MouseEventArgs e)
		{
			base.OnClick(e);
			base.OnMouseClick(e);
		}

		protected override void OnClick(EventArgs e)
		{
			// Suspend original click event
		}

		#endregion
	}
}
