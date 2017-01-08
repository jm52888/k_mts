using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Ellipse : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        EllipseAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_Ellipse() : base("Ellipse") { }
        public ChartTool_Ellipse(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.BackColor = Color.Transparent;
            this.LineWidth = 1;
            this.LineDashStyle = ChartDashStyle.Solid;
        }

        #endregion

        #region Method

        private void _EndPlacement()
        {
            if(curAnot != null)
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
            }
            curAnot = null;
        }

        private void _MakePositive(Annotation anot)
        {
            if (anot.Width < 0)
            {
                anot.X += anot.Width;
                anot.Width = -anot.Width;
            }

            if (anot.Height > 0)
            {
                anot.Y += anot.Height;
                anot.Height = -anot.Height;
            }
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
            return false;
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                Chart.Focus();

                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                var anot = new EllipseAnnotation();
                anot.AllowMoving = this.AllowMoving;
                anot.AllowResizing = this.AllowResizing;
                anot.AllowSelecting = this.AllowSelecting;
                anot.IsSizeAlwaysRelative = this.IsInfinitive;
                anot.LineColor = this.LineColor;
                anot.BackColor = this.BackColor;
                anot.BackSecondaryColor = this.BackSecondaryColor;
                anot.BackGradientStyle = this.BackGradientStyle;
                anot.BackHatchStyle = this.BackHatchStyle;
                anot.LineDashStyle = this.LineDashStyle;
                anot.LineWidth = this.LineWidth;

                anot.ClipToChartArea = curArea.Name;
                anot.AxisX = Common.GetMainAxisX(curArea);
                anot.AxisY = Common.GetMainAxisY(curArea);
                anot.SetAttr("ToolName", this.Name);

                curAnot = anot;
                Common.Annotations.Add(anot);
                anot.BeginPlacement();
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
            _MakePositive(anot);
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
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            EllipseAnnotation anot2 = bCopy ? new EllipseAnnotation() : (EllipseAnnotation)anot;
            if (bCopy)
            {
                ChartMethod.Assign(anot2, anot);
                anot2.Name = Common.Annotations.NextUniqueName();
                Translate(anot2, transX, transY, false);
                Common.Annotations.Add(anot2);
                return anot2;
            }
            anot2.X += (anot.AxisX.Maximum - anot.AxisX.Minimum) * transX;
            anot2.Y += (anot.AxisY.Maximum - anot.AxisY.Minimum) * transY;
            return anot2;
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            EllipseAnnotation anot2 = bCopy ? new EllipseAnnotation() : (EllipseAnnotation)anot;
            if (bCopy)
            {
                ChartMethod.Assign(anot2, anot);
                anot2.Name = Common.Annotations.NextUniqueName();
                Reflect(anot2, direction, false);
                Common.Annotations.Add(anot2);
                return anot2;
            }

            if (direction == FlowDirection.BottomUp)
            {
                if (anot2.Height > 0)
                    anot2.Y += anot2.Height * 2;
                anot2.Height = -anot2.Height;
            }
            else if (direction == FlowDirection.TopDown)
            {
                if (anot2.Height < 0)
                    anot2.Y += anot2.Height * 2;
                anot2.Height = -anot2.Height;
            }
            else if (direction == FlowDirection.LeftToRight)
            {
                if (anot2.Width > 0)
                    anot2.X += anot2.Width * 2;
                anot2.Width = -anot2.Width;
            }
            else if (direction == FlowDirection.RightToLeft)
            {
                if (anot2.Width < 0)
                    anot2.X += anot2.Width * 2;
                anot2.Width = -anot2.Width;
            }
            _MakePositive(anot2);
            return anot2;
        }

        #endregion
    }
}
