using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IUtilityNameController
	{
		bool IsColectionEditing { get; set; }
		IList Snapshot { get; }

		bool IsUniqueName(string name);
		void DoSnapshot(bool save, EventHandler<UtilityNameChangedEventArgs> changingCallback, EventHandler<UtilityNameChangedEventArgs> changedCallback);
		void OnUtilityNameChanged(UtilityNameChangedEventArgs e);
		void OnUtilityNameChanging(UtilityNameChangedEventArgs e);
	}
}
