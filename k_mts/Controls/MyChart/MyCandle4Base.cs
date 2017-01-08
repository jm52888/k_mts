using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace k_mts.Controls
{
	public partial class MyCandle4Base : UserControl
	{
		public MyCandle4Base()
		{
			InitializeComponent();
            base.DoubleBuffered = true;
		}

		public Panel Panel1 { get { return split_v1.Panel2; } }
		public Panel Panel2 { get { return split_v1.Panel1; } }
		public Panel Panel3 { get { return split_v2.Panel1; } }
		public Panel Panel4 { get { return split_v2.Panel2; } }
	}
}
