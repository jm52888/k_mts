using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_HorizontalEqualLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        Annotation curAnot;

        private LineStyleCollection _Lines;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LineStyleCollection Lines { get { return _Lines; } }

        [DefaultValue(false)]
        public bool UseCustomLine { get; set; }

        private int _LineCount = 7;
        [DefaultValue(7)]
        public int LineCount
        {
            get
            {
                return UseCustomLine ? _Lines.Count : _LineCount;
            }
            set
            {
                if (UseCustomLine) return;
                _LineCount = Math.Max(2, value);
            }
        }

        #endregion

        #region Class :: LineStyleCollection

        public class LineStyleCollection : Collection<LineAnnotation>
        {
            #region Variable and Property

            private ChartTool_HorizontalEqualLine Parent;
            public string NamePrefix { get; private set; }

            #endregion

            #region Constructor

            private LineStyleCollection() { }
            public LineStyleCollection(ChartTool_HorizontalEqualLine parent)
            {
                NamePrefix = typeof(LineAnnotation).Name;
                Parent = parent;
            }

            #endregion

            #region Override :: InsertItem

            protected override void InsertItem(int index, LineAnnotation item)
            {
                if (string.IsNullOrEmpty(item.Name))
                    item.Name = this.NextUniqueName();
                else if (!this.IsUniqueName(item.Name))
                    throw ChartExceptionFactory.NameAlreadyExists(item.Name, typeof(LineStyleCollection).Name);

                base.InsertItem(index, item);
            }

            #endregion

            #region Method :: IsUniqueName, FindByName, NextUniqueName

            private bool IsUniqueName(string name)
            {
                return this.FindByName(name) == null;
            }

            private object FindByName(string name)
            {
                foreach (LineAnnotation style in this)
                {
                    if (style.Name == name)
                        return style;
                }
                return null;
            }

            private string NextUniqueName()
            {
                string name = string.Empty;
                string namePrefix = this.NamePrefix;
                for (int index = 1; index < int.MaxValue; ++index)
                {
                    name = namePrefix + index.ToString();
                    if (this.IsUniqueName(name)) break;
                }
                return name;
            }

            #endregion
        }

        #endregion

        #region Constructor

        public ChartTool_HorizontalEqualLine() : base("HorizontalEqualLine") { }
        public ChartTool_HorizontalEqualLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.UseCustomLine = false;
            this._Lines = new LineStyleCollection(this);

            this.AllowMoving = true;
            this.AllowResizing = false;
            this.AllowSelecting = true;
            this.IsInfinitive = false;
            this.Alignment = ContentAlignment.MiddleRight;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelFormat = "0.00";
            this.LabelOffsetX = 5.0f;
            this.LabelOffsetY = -10.0f;

            this.IsAlwaysPainting = true;
        }

        #endregion

        #region Method

        private void _EndPlacement()
        {
            if (curAnot == null) return;

            curAnot.EndPlacement();
            Remove(curAnot);
            curAnot = null;
        }

        private HorizontalLineAnnotation _CreateAnnotation(bool bAlwaysVisible = false)
        {
			var anot = new HorizontalLineAnnotation
			{
				AllowMoving = this.AllowMoving,
				AllowResizing = this.AllowResizing,
				AllowSelecting = this.AllowSelecting,
				IsInfinitive = this.IsInfinitive,
				IsSizeAlwaysRelative = this.IsSizeAlwaysRelative,
				LineColor = this.LineColor,
				LineDashStyle = this.LineDashStyle,
				LineWidth = this.LineWidth,

				ClipToChartArea = curArea.Name,
				AxisX = Common.GetMainAxisX(curArea),
				AxisY = Common.GetMainAxisY(curArea)
			};

			anot.SetAttr("ToolName", this.Name);
			
			if (this.Alignment == ContentAlignment.MiddleLeft ||
                this.Alignment == ContentAlignment.BottomLeft ||
                this.Alignment == ContentAlignment.TopLeft)
                anot.Width = -CommonService.MAX_WIDTH;
            else anot.Width = CommonService.MAX_WIDTH;

            return anot;
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

            var dp1 = pts.FindNearestPoint(pt1);

            if (ms.ChartType == SeriesChartType.Candlestick ||
                ms.ChartType == SeriesChartType.Stock)
            {
                //double y1 = Math.Abs(dp1.YValues[0] - anot.Y) <= Math.Abs(dp1.YValues[1] - anot.Y) ? dp1.YValues[0] : dp1.YValues[1];
                double y1 = Math.Abs(dp1.YValues[0]) <= Math.Abs(dp1.YValues[1]) ? dp1.YValues[0] : dp1.YValues[1];
                anot.X = dp1.XValue;
                anot.Y = y1;
            }
            else
            {
                anot.X = dp1.XValue;
                anot.Y = dp1.YValues[0];
            }
        }

        #endregion

        #region ChartTool

        public override bool Select()
        {
            if (LineCount < 2) return false;
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
                int nLine = UseCustomLine ? Lines.Count : _LineCount;
                if (nLine < 2) return true;

                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                List<Annotation> lstAnot = new List<Annotation>();
                for (int i = 0; i < nLine; i++)
                {
                    bool bAlwaysVisible = i == 0 || i == LineCount - 1;
                    var anot = _CreateAnnotation(bAlwaysVisible);

                    anot.X = anot.AxisX.PixelPositionToValue(e.X);
                    anot.Y = anot.AxisY.PixelPositionToValue(e.Y);

                    //if (bAlwaysVisible) _DoMagnet(anot); 수평등분 자석기능 해제

                    if (UseCustomLine)
                    {
                        anot.LineWidth = Lines[i].LineWidth;
                        anot.LineColor = Lines[i].LineColor;
                        anot.LineDashStyle = Lines[i].LineDashStyle;
                    }
                    Common.Annotations.Add(anot);
                    lstAnot.Add(anot);
                }
                curAnot = lstAnot[0];
                curAnot.IsSelected = true;
                curAnot.SetAttr("AnotList", lstAnot);
                curAnot.BeginPlacement();
            }
            return false;
        }
		
		public bool OnMouseMove(MouseEventArgs e)
        {
			if (curAnot == null) return true;
			
            try { curAnot.Y = curAnot.AxisY.PixelPositionToValue(e.Y); }
            catch { return true; }

            var lst = curAnot.GetAttr<List<Annotation>>("AnotList");
			if (lst == null) return true;

			var firstAnot = curAnot;
			var lastAnot = lst.Last();

			double height = lastAnot.Y - firstAnot.Y;
			double space = height / (lst.Count - 1);
			
            for (int i = 1; i < lst.Count - 1; i++)
			{ 				
				lst[i].Y = firstAnot.Y + (this.UseCustomLine ? height * Lines[i].Y : space * i);
			}

            return true;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
			/*if (e.Button == MouseButtons.Left && curAnot != null)
			{

			}
			else */
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

        public void AnnotationSelectionChanged(Annotation anot)
        {
			
		}

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {
            e.NewLocationX = e.Annotation.X;
        }

        public void AnnotationPositionChanged(Annotation anot)
        {

        }

        public void AnnotationPlaced(Annotation anot)
        {
            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst == null) return;

            lst.ForEach(a => a.SetAttr("AnotList", lst));
            curAnot = null;
        }

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {
			var lst = anot.GetAttr<List<Annotation>>("AnotList");
			if (lst == null) lst = curAnot.GetAttr<List<Annotation>>("AnotList");

			// Get Value
			var first = lst.First();
			var last = lst.Last();

			double ratio = double.NaN;
			double space = first.Y - last.Y;
			if (space != 0) ratio = (anot.Y - last.Y) / space;

			string value = ratio.ToString(this.LabelFormat);

			// Write Value
			if (anot.X > anot.AxisX.Minimum)
            {
				double x = anot.AxisX.ValueToPixelPosition(Math.Min(anot.AxisX.Maximum, anot.X)) + this.LabelOffsetX;
				double y = anot.AxisY.ValueToPixelPosition(anot.Y) + this.LabelOffsetY;

                using (Brush brush = new SolidBrush(this.ForeColor))
                {
					e.Graphics.DrawString(
						$"({anot.Y.ToString(anot.AxisY.LabelStyle.Format)}) {value}",
                        this.Font, brush, (float)x, (float)y);
                }
            }
        }

        public void AnnotationTextChanged(Annotation anot)
        {

        }

        #endregion

        #region IAnnotationEditor

        public void Clear()
        {
            var anots = Common.Annotations
                .Where(a => Name.Equals(a.GetAttr("ToolName")))
                .ToArray();

            if (anots.Length == 0) return;

            Common.Annotations.SuspendUpdates();
            foreach (var anot in anots) Remove(anot);
            Common.Annotations.ResumeUpdates();
        }

        public bool Remove(Annotation anot)
        {
            if (!Name.Equals(anot.GetAttr("ToolName")) ||
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
