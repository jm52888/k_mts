using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IMouseEventHandler
	{
		bool OnMouseMove(MouseEventArgs e);
		bool OnMouseDown(MouseEventArgs e);
		bool OnMouseUp(MouseEventArgs e);
		bool OnMouseWheel(MouseEventArgs e);
		bool OnMouseClick(MouseEventArgs e);
		bool OnMouseDoubleClick(MouseEventArgs e);
	}
}
