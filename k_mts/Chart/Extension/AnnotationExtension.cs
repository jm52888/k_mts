using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class AnnotationExtension
	{
		public static bool IsValid(this Annotation anot)
		{
			if (double.IsNaN(anot.X) ||
				double.IsNaN(anot.Y) ||
				double.IsNaN(anot.Width) ||
				double.IsNaN(anot.Height) ||
				(anot.Width == 0 && anot.Height == 0)) return false;

			return true;
		}
	}
}
