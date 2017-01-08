using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class UtilityNameChangedEventArgs : EventArgs
	{
		#region Variable and Property

		private ChartNamedUtility _OldElement;
		public ChartNamedUtility OldElement
		{
			get { return _OldElement; }
		}

		private string _OldName;
		public string OldName
		{
			get { return _OldName; }
		}

		private string _NewName;
		public string NewName
		{
			get { return _NewName; }
		}

		#endregion

		#region Constructor

		protected UtilityNameChangedEventArgs() { }
		public UtilityNameChangedEventArgs(ChartNamedUtility oldElement, ChartNamedUtility newElement)
		{
			_OldElement = oldElement;
			_OldName = oldElement != null ? oldElement.Name : string.Empty;
			_NewName = newElement != null ? newElement.Name : string.Empty;
		}
		public UtilityNameChangedEventArgs(ChartNamedUtility oldElement, string oldName, string newName)
		{
			_OldElement = oldElement;
			_OldName = oldName;
			_NewName = newName;
		}

		#endregion
	}
}
