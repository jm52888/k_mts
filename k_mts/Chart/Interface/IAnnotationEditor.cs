using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public interface IAnnotationEditor
	{
        void Clear();
		bool Remove(Annotation anot);
        Annotation Translate(Annotation anot, double transX, double transY, bool bCopy = false);
        Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false);
    }
}
