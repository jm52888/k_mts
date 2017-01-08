using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms
{
	public class MyNumericUpDown : NumericUpDown
	{
		private int _WheelDelta = 3;
		[DefaultValue(3)]
		public int WheelDelta
		{
			get { return _WheelDelta; }
			set { _WheelDelta = value; }
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.Value =
				Math.Max(base.Minimum,
				Math.Min(base.Maximum, 
				base.Value + (e.Delta > 0 ? _WheelDelta : -_WheelDelta)
				));

			return;
		}
	}
}
