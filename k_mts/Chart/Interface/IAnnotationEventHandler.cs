using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IAnnotationEventHandler
	{
		void AnnotationTextChanged(Annotation anot);
		void AnnotationSelectionChanged(Annotation anot);
		void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e);
		void AnnotationPositionChanged(Annotation anot);
		void AnnotationPlaced(Annotation anot);
		void AnnotationPaint(Annotation anot, PaintEventArgs e);

        bool IsAlwaysPainting { get; set; }
    }
}
