using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace System.Windows.Forms
{
	public class MyPanel : Panel
	{
		public MyPanel()
            //: base()
		{
            base.DoubleBuffered = true;
		}
	}
}
