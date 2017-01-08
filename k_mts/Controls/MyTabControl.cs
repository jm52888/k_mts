using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace k_mts.Controls
{
	public class MyTabControl : TabControl
	{
		#region Property

		public bool UseDefaultHeader { get; set; }

        #endregion

        #region Constructor

        public MyTabControl()
        {
            base.Multiline = true;            
        }
                    

		#endregion

		#region Override

		protected override Padding DefaultMargin
		{
			get { return new Padding(0); }
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x1328 &&
				!DesignMode &&
				!UseDefaultHeader)
			{
				m.Result = (IntPtr)1;
			}
			else
			{
				base.WndProc(ref m);
			}
		}

        #endregion        
    }
}
