using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Template : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        #endregion

        #region Constructor        

        #endregion

        #region Method

        #endregion

        #region ChartTool

        public override bool Select()
        {
            throw new NotImplementedException();
        }

        public override void Release()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMouseEventHandler

        public bool OnMouseClick(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAnnotationEventHandler

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void AnnotationPlaced(Annotation anot)
        {
            throw new NotImplementedException();
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            throw new NotImplementedException();
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void AnnotationSelectionChanged(Annotation anot)
        {
            throw new NotImplementedException();
        }

        public void AnnotationTextChanged(Annotation anot)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAnnotationEditor

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Annotation anot)
        {
            throw new NotImplementedException();
        }

        public Annotation Translate(Annotation anot, double transX, double transY, bool bCopy = false)
        {
            throw new NotImplementedException();
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
