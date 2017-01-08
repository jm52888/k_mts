using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IKeyboardEventHandler
	{
		bool OnKeyDown(KeyEventArgs e);
		bool OnKeyUp(KeyEventArgs e);
		bool OnKeyPress(KeyPressEventArgs e);
	}
}
