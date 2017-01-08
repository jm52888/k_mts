using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ToolChangedEventArgs : EventArgs
	{
		#region Property

		private ChartTool _oldTool;
		public ChartTool OldTool { get { return _oldTool; } }

		private ChartTool _newTool;
		public ChartTool NewTool { get { return _newTool; } }

		#endregion

		#region Constructor

		protected ToolChangedEventArgs() { }
		public ToolChangedEventArgs(ChartTool oldTool, ChartTool newTool)
		{
			_oldTool = oldTool;
			_newTool = newTool;
		}

		#endregion
	}
}
