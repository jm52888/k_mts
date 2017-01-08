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
    public class ChartTool_VerticalEqualLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
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

            private ChartTool_VerticalEqualLine Parent;
            public string NamePrefix { get; private set; }

            #endregion

            #region Constructor

            private LineStyleCollection() { }
            public LineStyleCollection(ChartTool_VerticalEqualLine parent)
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

        public ChartTool_VerticalEqualLine() : base("VerticalEqualLine") { }
        public ChartTool_VerticalEqualLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.UseCustomLine = false;
            this._Lines = new LineStyleCollection(this);

            this.AllowMoving = true;
            this.AllowResizing = false;
            this.AllowSelecting = false;
            this.IsInfinitive = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelAlignment = LabelAlignmentStyles.Top;
            this.LabelFormat = "0.00";
            this.LabelOffsetX = 12.0f;
            this.LabelOffsetY = 1.0f;

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

        private VerticalLineAnnotation _CreateAnnotation(bool bAlwaysVisible = false)
        {
            var anot = new VerticalLineAnnotation();
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
            anot.SetAttr("CheckBox", _CreateCheckBox(anot, bAlwaysVisible));
            return anot;
        }

        private CheckBox _CreateCheckBox(Annotation anot, bool bAlwaysVisible)
        {
            var chk = new CheckBox();
            chk.BackColor = Drawing.Color.Transparent;
            chk.Checked = true;
            chk.Tag = anot;
            chk.AutoSize = true;
            if (bAlwaysVisible) chk.Enabled = false;
            else chk.CheckedChanged += (sender, e) => anot.Visible = chk.Checked;
            return chk;
        }

        private void _DoMagnet(Annotation anot)
        {

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
                    var anot = _CreateAnnotation(i == 0 || i == LineCount - 1);
                    anot.X = anot.AxisX.PixelPositionToValue(e.X);
                    anot.Y = anot.AxisY.PixelPositionToValue(e.Y);
                    if (UseCustomLine)
                    {
                        anot.LineWidth = Lines[i].LineWidth;
                        anot.LineColor = Lines[i].LineColor;
                        anot.LineDashStyle = Lines[i].LineDashStyle;
                    }
                    anot.SetAttr("AnotList", lstAnot);
                    Common.Annotations.Add(anot);
                    lstAnot.Add(anot);
                }
                curAnot = lstAnot[0];
                curAnot.BeginPlacement();
            }
            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            if (curAnot == null) return true;

            try { curAnot.X = curAnot.AxisX.PixelPositionToValue(e.X); }
            catch { return true; }

            var lst = curAnot.GetAttr<List<Annotation>>("AnotList");
            if (lst == null) return true;

            var curAnot2 = lst.Last();
            double dx = (curAnot2.X - curAnot.X) / (lst.Count - 1);
            double w = curAnot2.X - curAnot.X;

            for (int i = 1; i < lst.Count - 1; i++)
                lst[i].X = curAnot.X +
                    (this.UseCustomLine ? w * Lines[i].X : dx * i);

            return true;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curAnot != null)
            {
                var lst = curAnot.GetAttr<List<Annotation>>("AnotList");
                lst.ForEach(a =>
                {
                    var chk = a.GetAttr<CheckBox>("CheckBox");
                    chk.Parent = Common.Chart;
                });
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

        public void AnnotationSelectionChanged(Annotation anot)
        {
            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst == null) return;

            bool bVisible = !lst.TrueForAll(a => !a.IsSelected);
            lst.ForEach(a =>
            {
                var chk = a.GetAttr<CheckBox>("CheckBox");
                if (chk != null) chk.Visible = bVisible;
            });
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {

        }

        public void AnnotationPositionChanged(Annotation anot)
        {

        }

        public void AnnotationPlaced(Annotation anot)
        {
            curAnot = null;
        }

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {
            var axisX = anot.AxisX;
            var axisY = anot.AxisY;

            var chk = anot.GetAttr<CheckBox>("CheckBox");
            if (chk == null) return;

            double x = 0.0;
            double y = 0.0;

            x = axisX.ValueToPixelPosition(anot.X);
            y = this.LabelAlignment == LabelAlignmentStyles.Bottom ?
                axisY.ValueToPixelPosition(axisY.Minimum) :
                axisY.ValueToPixelPosition(axisY.Maximum);

            if (chk.Visible)
            {
                chk.Location = new Point((int)x, (int)y);

                var lst = anot.GetAttr<List<Annotation>>("AnotList");
                if (lst != null &&
                    axisX.Minimum < anot.X && anot.X < axisX.Maximum)
                {
                    var anot1 = lst.Last();
                    var anot2 = lst.First();

                    double ratio = double.NaN;
                    double dx = anot2.X - anot1.X;
                    if (dx != 0) ratio = (anot.X - anot1.X) / dx;

                    x += this.LabelOffsetX;
                    y += this.LabelOffsetY;
                    using (Brush b = new SolidBrush(this.ForeColor))
                    {
                        e.Graphics.DrawString(
                            ratio.ToString(this.LabelFormat),
                            this.Font, b,
                            (float)x, (float)y);
                    }
                }
                else chk.Text = "";
            }



            //if (chk.Checked && axisX.Minimum < anot.X && anot.X < axisX.Maximum)
            //{
            //    x = anot.AxisX.ValueToPixelPosition(anot.X) + this.LabelOffsetX;
            //    y = this.LabelAlignment == LabelAlignmentStyles.Bottom ?
            //        anot.AxisY.ValueToPixelPosition(axisY.Maximum) + this.LabelOffsetY :
            //        anot.AxisY.ValueToPixelPosition(axisY.Minimum) + this.LabelOffsetY;

            //    using (Brush b = new SolidBrush(this.ForeColor))
            //    {
            //        e.Graphics.DrawString(
            //            anot.X.ToString(axisX.LabelStyle.Format),
            //            this.Font, b,
            //            (float)x, (float)y);
            //    }
            //}
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

            var chk = anot.GetAttr<CheckBox>("CheckBox");
            if (chk != null)
            {
                anot.SetAttr("CheckBox", null);
                chk.Dispose();
            }

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
