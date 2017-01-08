using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_VerticalLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        VerticalLineAnnotation curAnot;        

        #endregion

        #region Constructor

        public ChartTool_VerticalLine() : base("VerticalLine") { }
        public ChartTool_VerticalLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelFormat = "0.00";
            this.LabelOffsetX = 7.5f;
            this.LabelOffsetY = 7.5f;
        }

        #endregion

        #region Method

        private void _EndPlacement()
        {
            if (curAnot != null)
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
            }
            curAnot = null;
        }

        private void _DoMagnet(Annotation anot)
        {
            
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
            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                Chart.Focus();

                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                var anot = new VerticalLineAnnotation();
                anot.AllowMoving = this.AllowMoving;
                anot.AllowResizing = this.AllowResizing;
                anot.AllowSelecting = this.AllowSelecting;
                anot.IsSizeAlwaysRelative = this.IsInfinitive;
                anot.LineColor = this.LineColor;
                anot.LineDashStyle = this.LineDashStyle;
                anot.LineWidth = this.LineWidth;

                anot.ClipToChartArea = curArea.Name;
                anot.AxisX = Common.GetMainAxisX(curArea);
                anot.AxisY = Common.GetMainAxisY(curArea);
                anot.SetAttr("ToolName", this.Name);

                curAnot = anot;
                Chart.Annotations.Add(anot);
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
            if (e.Button == MouseButtons.Right)
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
            var x = (float)anot.AxisX.ValueToPixelPosition(anot.X);
            var y = (float)anot.AxisY.ValueToPixelPosition(anot.Y);
            var x2 = (float)anot.AxisX.ValueToPixelPosition(anot.X + anot.Width);
            var y2 = (float)anot.AxisY.ValueToPixelPosition(anot.Y + anot.Height);            

            using (Brush b = new SolidBrush(this.ForeColor))            
            {
                float dx = x > x2 ? -7.5f : -7.5f;
                float dy = y > y2 ? 7.5f : -17.5f;
                //float dx2 = x2 > x ? -7.5f : -7.5f;
                //float dy2 = y2 > y ? 7.5f : -17.5f;

                e.Graphics.DrawString(
                anot.Y.ToString(this.LabelFormat),
                this.Font, b,
                x + dx, y + dy);

                //e.Graphics.DrawString(
                //    (anot.Y + anot.Height).ToString(this.LabelFormat),
                //    this.Font, b2,
                //    x2 + dx2, y2 + dy2);

            }
        }

        public void AnnotationPlaced(Annotation anot)
        {
            if (!anot.IsValid())
            {
                Chart.Annotations.Remove(anot);
                return;
            }
            curAnot = null;
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            if (this.UseMagnet)
                _DoMagnet(anot);
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
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            LineAnnotation line = new LineAnnotation();
            ChartMethod.Assign(line, anot);

            line.X += (anot.AxisX.Maximum - anot.AxisX.Minimum) * transX;
            line.Y += (anot.AxisY.Maximum - anot.AxisY.Minimum) * transY;
            line.SetAttr("ToolName", this.Name);
            line.Name = Common.Annotations.NextUniqueName();

            Common.Annotations.Add(line);
            return line;
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            LineAnnotation line = new LineAnnotation();
            ChartMethod.Assign(line, anot);
            line.Name = Common.Annotations.NextUniqueName();

            if (direction == FlowDirection.BottomUp)
            {
                if (anot.Height > 0)
                    anot.Y += anot.Height * 2;
                anot.Height = -anot.Height;
            }
            else if (direction == FlowDirection.TopDown)
            {
                if (anot.Height < 0)
                    anot.Y += anot.Height * 2;
                anot.Height = -anot.Height;
            }
            else if (direction == FlowDirection.LeftToRight)
            {
                if (anot.Width > 0)
                    anot.X += anot.Width * 2;
                anot.Width = -anot.Width;
            }
            else if (direction == FlowDirection.RightToLeft)
            {
                if (anot.Width < 0)
                    anot.X += anot.Width * 2;
                anot.Width = -anot.Width;
            }
            Common.Annotations.Add(line);
            return anot;
        }
        
        #endregion
    }
}
