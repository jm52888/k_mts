using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_ParallelLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        LineAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_ParallelLine() : base("ParallelLine") { }
        public ChartTool_ParallelLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.Alignment = ContentAlignment.TopRight;
            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsInfinitive = false;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.BackColor = Color.Red;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelAlignment = LabelAlignmentStyles.Right;
            this.LabelFormat = "0.000";
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
                Remove(curAnot);
            }
            curAnot = null;
        }

        private void _DoMagnet(Annotation anot)
        {
            var area = Common.ChartAreas.FindByName(anot.ClipToChartArea);
            if (area == null) return;

            var ms = Common.GetMainSeries(area);
            if (ms == null) return;

            var pts = ms.Points;
            if (pts.Count == 0) return;

            PointF pt1 = new PointF((float)anot.X, (float)anot.Y);
            PointF pt2 = new PointF((float)(anot.X + anot.Width), (float)(anot.Y + anot.Height));

            var dp1 = pts.FindNearestPoint(pt1);
            var dp2 = pts.FindNearestPoint(pt2);

            if (ms.ChartType == SeriesChartType.Candlestick ||
                ms.ChartType == SeriesChartType.Stock)
            {
                double y1 = Math.Abs(dp1.YValues[0] - anot.Y) <= Math.Abs(dp1.YValues[1] - anot.Y) ? dp1.YValues[0] : dp1.YValues[1];
                double y2 = Math.Abs(dp2.YValues[0] - anot.Y - anot.Height) <= Math.Abs(dp2.YValues[1] - anot.Y - anot.Height) ? dp2.YValues[0] : dp2.YValues[1];

                anot.X = dp1.XValue;
                anot.Y = y1;
                anot.Width = dp2.XValue - dp1.XValue;
                anot.Height = y2 - y1;
            }
            else
            {
                anot.X = dp1.XValue;
                anot.Y = dp1.YValues[0];
                anot.Width = dp2.XValue - dp1.XValue;
                anot.Height = dp2.YValues[0] - dp1.YValues[0];
            }
        }

        private LineAnnotation _CreateAnnotation()
        {
            var anot = new LineAnnotation();
            anot.AllowMoving = this.AllowMoving;
            anot.AllowResizing = this.AllowResizing;
            anot.AllowSelecting = this.AllowSelecting;
            anot.IsInfinitive = this.IsInfinitive;
            anot.IsSizeAlwaysRelative = this.IsSizeAlwaysRelative;
            anot.LineColor = this.LineColor;
            anot.LineDashStyle = this.LineDashStyle;
            anot.LineWidth = this.LineWidth;

            anot.ClipToChartArea = curArea.Name;
            anot.AxisX = Common.GetMainAxisX(curArea);
            anot.AxisY = Common.GetMainAxisY(curArea);
            anot.SetAttr("ToolName", this.Name);

            return anot;
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

            if (e.Button == MouseButtons.Left)
            {
                if (curAnot == null)
                {
                    curArea = Common.GetCurrentArea(e.X, e.Y);
                    if (curArea == null) return true;

                    curAnot = _CreateAnnotation();
                    curAnot.IsInfinitive = true;
                    Common.Annotations.Add(curAnot);
                    curAnot.BeginPlacement();
                }
                else
                {
                    var anot1 = curAnot;
                    anot1.IsInfinitive = false;

                    double w = anot1.Width, h = anot1.Height;
                    if (w > 0) // h > 0 or h < 0
                    {
                        w *= 1000;
                        h *= 1000;
                    }
                    else if (h > 0) // w > 0
                    {
                        w *= -1000;
                        h *= -1000;
                    }
                    else // w < 0 and h < 0
                    {
                        w *= -1000;
                        h *= -1000;
                    }
                    anot1.Width = w;
                    anot1.Height = h;

                    var anot2 = _CreateAnnotation();
                    anot2.X = anot1.AxisX.PixelPositionToValue(e.X);
                    anot2.Y = anot1.AxisY.PixelPositionToValue(e.Y);
                    anot2.Width = w;
                    anot2.Height = h;

                    var anot3 = _CreateAnnotation();
                    anot3.X = (anot1.X + anot2.X) / 2;
                    anot3.Y = (anot1.Y + anot2.Y) / 2;
                    anot3.Width = w;
                    anot3.Height = h;

                    Common.Annotations.Add(anot2);
                    Common.Annotations.Add(anot3);

                    List<Annotation> lst = new List<Annotation>();
                    lst.Add(anot1);
                    lst.Add(anot2);
                    lst.Add(anot3);
                    lst.ForEach(a => a.SetAttr("AnotList", lst));
                    lst.ForEach(a => a.AllowResizing = false);

                    curAnot = null;
                }
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
                Remove(anot);
                curAnot = null;
            }
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            if (curAnot != null && this.UseMagnet)
                _DoMagnet(curAnot);
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {
            var anot = e.Annotation;
            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst == null) return;

            double dx = e.NewLocationX - anot.X;
            double dy = e.NewLocationY - anot.Y;

            lst.ForEach(a =>
            {
                if (a == anot) return;
                a.X += dx;
                a.Y += dy;
            });
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
                .Where(a => this.Name.Equals(a.GetAttr("ToolName")))
                .ToArray();

            if (anots.Length == 0) return;

            Common.Annotations.SuspendUpdates();
            foreach (var anot in anots) Remove(anot);
            Common.Annotations.ResumeUpdates();
        }

        public bool Remove(Annotation anot)
        {
            if (anot.GetAttr<string>("ToolName") != this.Name ||
                !Common.Annotations.Remove(anot)) return false;

            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst != null) lst.ForEach(a =>
            {
                a.SetAttr("AnotList", null);
                Remove(a);
            });
            return true;
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
