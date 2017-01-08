using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Polygon : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        PolygonAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_Polygon() : base("Polygon") { }
        public ChartTool_Polygon(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.Cursor = Cursors.Arrow;

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.AllowPathEditing = true;
            this.IsInfinitive = false;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;
        }

        #endregion

        #region Method

        private PolygonAnnotation _CreateAnnotation()
        {
            var anot = new PolygonAnnotation();
            anot.AllowMoving = this.AllowMoving;
            anot.AllowResizing = this.AllowResizing;
            anot.AllowSelecting = this.AllowSelecting;
            anot.AllowPathEditing = this.AllowPathEditing;
            anot.IsSizeAlwaysRelative = this.IsSizeAlwaysRelative;
            anot.LineColor = this.LineColor;
            anot.LineDashStyle = this.LineDashStyle;
            anot.LineWidth = this.LineWidth;
            anot.ClipToChartArea = curArea.Name;
            //anot.AxisX = Common.GetMainAxisX(curArea);
            //anot.AxisY = Common.GetMainAxisY(curArea);
            anot.SetAttr("ToolName", this.Name);
            return anot;
        }

        private void _EndPlacement()
        {
            if (curAnot != null)
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
            }
            curAnot = null;
        }

        #endregion

        #region ChartTool

        public override bool Select()
        {
            _EndPlacement();
            return true;
        }

        public override void Release()
        {
            _EndPlacement();
        }

        #endregion

        #region IMouseEventHandler

        public bool OnMouseClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            Chart.Focus();

            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                curAnot = _CreateAnnotation();
                Common.Annotations.Add(curAnot);                
                curAnot.BeginPlacement();
            }
            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curAnot != null && !curAnot.IsValid())
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
                curAnot = null;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Common.Tools.Release();
                return true;
            }
            return false;
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            return true;
        }

        #endregion

        #region IAnnotationEventHandler

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {

        }

        public void AnnotationPlaced(Annotation anot)
        {
            if (!anot.IsValid())
            {
                Common.Annotations.Remove(anot);
                return;
            }
            //anot.AxisX = Common.GetMainAxisX(curArea);
            //anot.AxisY = Common.GetMainAxisY(curArea);
            curAnot = null;
        }

        public void AnnotationPositionChanged(Annotation anot)
        {

        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {

        }

        public void AnnotationSelectionChanged(Annotation anot)
        {

        }

        public void AnnotationTextChanged(Annotation anot)
        {

        }

        #endregion

        #region IAnnotationEditor

        public void Clear()
        {
            var anots = Common.Annotations
                .Where(a => a.GetAttr<string>("ToolName") == this.Name)
                .ToArray();

            if (anots.Length == 0) return;

            Common.Annotations.SuspendUpdates();
            foreach (var anot in anots) Remove(anot);
            Common.Annotations.ResumeUpdates();
        }

        public bool Remove(Annotation anot)
        {
            if (anot.GetAttr<string>("ToolName") != this.Name) return false;
            return Common.Annotations.Remove(anot);
        }

        public Annotation Translate(Annotation anot, double transX, double transY, bool bCopy = false)
        {
            return null;
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            return null;
        }

        #endregion

    }
}
