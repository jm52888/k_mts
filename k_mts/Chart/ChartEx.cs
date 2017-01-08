using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Input;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartEx : Chart
    {
        #region Variable and Property

        private ChartUtilityService _UtilityService;
        private ChartAdapterCollection _Adapters;
        private ChartViewerCollection _Viewers;
        private ChartToolCollection _Tools;

        private string _NonSerializableContent =
            "*.Common*,*.Parent*,*.Viewer*,*.Cursor*,DataPoint.*,*.Annotation*";

        public ChartViewerCollection Viewers { get { return _Viewers; } }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChartToolCollection Tools { get { return _Tools; } }
        public new object DataSource
        {
            get { return _UtilityService.Common.DataSource; }
            set { _UtilityService.Common.DataSource = value; }
        }

        private bool _UseMagnet = false;
        [DefaultValue(false)]
        public bool UseMagnet 
        {
            get { return _UseMagnet; }
            set { _UseMagnet = value; }
        }
        
        [Category("Converter")]
        public string AreaToView
        {
            get { return "Input Name"; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                var area = base.ChartAreas.FindByName(value);
                if (area == null) return;
                if (area.GetType().Equals(typeof(ChartView))) return;

                try
                {
                    var view = new ChartView();
                    ChartMethod.Assign(view, area);

                    base.ChartAreas.Remove(area);
                    base.ChartAreas.Add(view);

                    DataBind();

                    if (DesignMode) MessageBox.Show("Success!", "AreaToView");
                }
                catch
                {
                    if (DesignMode) MessageBox.Show("Error!", "AreaToView");
                }
            }
        }

        [Category("Converter")]
        public string ViewToArea
        {
            get { return "Input Name"; }
            set
            {
                if (string.IsNullOrWhiteSpace(value)) return;

                var view = base.ChartAreas.FindByName(value);
                if (view == null) return;
                if (view.GetType().Equals(typeof(ChartArea))) return;

                try
                {
                    if (DesignMode)
                    {
                        if (MessageBox.Show("You can lose view setting. continue?",
                            "ViewToArea", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    var area = new ChartArea();
                    ChartMethod.Assign(area, view);

                    base.ChartAreas.Remove(view);
                    base.ChartAreas.Add(area);

                    DataBind();

                    if (DesignMode) MessageBox.Show("Success!", "ViewToArea");
                }
                catch
                {
                    if (DesignMode) MessageBox.Show("Error!", "ViewToArea");
                }
            }
        }

        #endregion

        #region Constructor

        public ChartEx()
        {
            _InitService();
            _InitEvent();
            _InitTimer();
        }

        private void _InitService()
        {
            _UtilityService = new ChartUtilityService(this);

            // Adapters
            _Adapters = new ChartAdapterCollection(_UtilityService);
            _Adapters.Add(new ChartAdapter_DataSet());
            _Adapters.Add(new ChartAdapter_DataTable());
            _UtilityService.AddService(_Adapters.GetType(), _Adapters);

            // Tools
            _Tools = new ChartToolCollection(_UtilityService);
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                _Tools.Add(new ChartTool_Select());
                _Tools.Add(new ChartTool_Cursor());
                _Tools.Add(new ChartTool_Hand());
                _Tools.Add(new ChartTool_Line());
                _Tools.Add(new ChartTool_Line2());
                _Tools.Add(new ChartTool_Polyline());
                _Tools.Add(new ChartTool_ParallelLine());
                _Tools.Add(new ChartTool_InfiniteLine());
                _Tools.Add(new ChartTool_HorizontalLine());
                _Tools.Add(new ChartTool_HorizontalInfiniteLine());
                _Tools.Add(new ChartTool_HorizontalEqualLine());
                _Tools.Add(new ChartTool_VerticalLine());
                _Tools.Add(new ChartTool_VerticalEqualLine());
                _Tools.Add(new ChartTool_VerticalInfiniteLine());                
                _Tools.Add(new ChartTool_Ellipse());
                _Tools.Add(new ChartTool_Rectangle());
                _Tools.Add(new ChartTool_Triangle());
            }
            _UtilityService.AddService(_Tools.GetType(), _Tools);

            // Views
            _Viewers = new ChartViewerCollection(_UtilityService);
            _UtilityService.AddService(_Viewers.GetType(), _Viewers);
        }

        private void _InitEvent()
        {
            base.AnnotationPlaced += ChartEx_AnnotationPlaced;
            base.AnnotationPositionChanged += ChartEx_AnnotationPositionChanged;
            base.AnnotationPositionChanging += ChartEx_AnnotationPositionChanging;
            base.AnnotationSelectionChanged += ChartEx_AnnotationSelectionChanged;
            base.AnnotationTextChanged += ChartEx_AnnotationTextChanged;
        }

        private void _InitTimer()
        {
            _UpdateTimer.Tick += (s, e) => DataBind();
        }

        #endregion

        #region Method (DataBind, SaveFile, LoadFile)

        #region DataBind

        public event EventHandler<ChartView> DataUpdating;
        public event EventHandler<ChartView> DataUpdated;

        private DateTime _LastUpdated = DateTime.Now;
        private Timer _UpdateTimer = new Timer();

        private int _BindingDelay = 35;
        [DefaultValue(35)]
        public int BindingDelay
        {
            get { return _BindingDelay; }
            set { _BindingDelay = Math.Max(10, value); }
        }

        public new void DataBind()
        {
            if (ChartTimer.Now > _LastUpdated.AddMilliseconds(_BindingDelay) &&
                (_Tools.SelectedTool == null || !_Tools.SelectedTool.IsDrawing) &&
                ((Control.ModifierKeys & Keys.Control) == 0))
            {
                _UpdateTimer.Stop();
                _RaiseDataUpdating();
                _Adapters.Update();
                _Viewers.Update();
                _RaiseDataUpdated();

                _LastUpdated = ChartTimer.Now;
            }
            else if (!_UpdateTimer.Enabled)
            {
                _UpdateTimer.Interval = _BindingDelay;
                _UpdateTimer.Start();
            }
        }

        private void _RaiseDataUpdating()
        {
            foreach (var area in base.ChartAreas)
            {
                var view = area as ChartView;
                if (view == null) continue;

                var viewer = this._Viewers.FindByName(view.ChartViewer) ?? _Viewers.DefalutViewer;
                var h = viewer as IDataUpdateEventHandler;
                if (h != null) h.OnDataUpdating(this, view);
            }
            var h2 = Tools.SelectedTool as IDataUpdateEventHandler;
            if (h2 != null) h2.OnDataUpdating(this, null);

            if (DataUpdating != null) DataUpdating(this, null);
        }

        private void _RaiseDataUpdated()
        {
            foreach (var area in base.ChartAreas)
            {
                var view = area as ChartView;
                if (view == null) continue;

                var viewer = this._Viewers.FindByName(view.ChartViewer) ?? _Viewers.DefalutViewer;
                var h = viewer as IDataUpdateEventHandler;
                if (h != null) h.OnDataUpdated(this, view);
            }
            var h2 = Tools.SelectedTool as IDataUpdateEventHandler;
            if (h2 != null) h2.OnDataUpdated(this, null);
            
            if (DataUpdated != null) DataUpdated(this, null);
        }

        #endregion

        #region SaveFile

        public void SaveFile(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                _SaveEx(fileName);

                base.Serializer.NonSerializableContent = _NonSerializableContent;
                base.Serializer.Save(sw);
            }
        }

        private void _SaveEx(string fileName)
        {

        }

        #endregion

        #region LoadFile

        public void LoadFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                _ResetWhenLoading();
                _LoadEx(fileName);

                base.Serializer.IsResetWhenLoading = false;
                base.Serializer.NonSerializableContent = _NonSerializableContent;
                base.Serializer.Load(sr);

                this.DataBind();
            }
        }

        private void _ResetWhenLoading()
        {
            this.Tools.Release();

            base.Titles.Clear();
            base.Legends.Clear();
            base.Annotations.Clear();
            base.Series.Clear();
            base.ChartAreas.Clear();
        }

        private void _LoadEx(string fileName)
        {
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == typeof(ChartView).Name)
                        {
                            var area = new ChartView();
                            area.Name = reader.GetAttribute("Name");
                            base.ChartAreas.Add(area);
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Dispatch :: Annotation Event

        void ChartEx_AnnotationTextChanged(object sender, EventArgs e)
        {
            var anot = sender as Annotation;
            string toolName = anot.GetAttr("ToolName") as string;

            IAnnotationEventHandler h = _Tools.FindByName(toolName) as IAnnotationEventHandler;
            if (h != null) h.AnnotationTextChanged(anot);
        }
        void ChartEx_AnnotationSelectionChanged(object sender, EventArgs e)
        {
            var anot = sender as Annotation;
            string toolName = anot.GetAttr("ToolName") as string;

            IAnnotationEventHandler h = _Tools.FindByName(toolName) as IAnnotationEventHandler;
            if (h != null) h.AnnotationSelectionChanged(anot);
        }
        void ChartEx_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
        {
            var anot = e.Annotation;
            string toolName = anot.GetAttr("ToolName") as string;

            IAnnotationEventHandler h = _Tools.FindByName(toolName) as IAnnotationEventHandler;
            if (h != null) h.AnnotationPositionChanging(e);
        }
        void ChartEx_AnnotationPositionChanged(object sender, EventArgs e)
        {
            var anot = sender as Annotation;
            string toolName = anot.GetAttr("ToolName") as string;

            IAnnotationEventHandler h = _Tools.FindByName(toolName) as IAnnotationEventHandler;
            if (h != null) h.AnnotationPositionChanged(anot);
        }
        void ChartEx_AnnotationPlaced(object sender, EventArgs e)
        {
            var anot = sender as Annotation;
            string toolName = anot.GetAttr("ToolName") as string;

            IAnnotationEventHandler h = _Tools.FindByName(toolName) as IAnnotationEventHandler;
            if (h != null) h.AnnotationPlaced(anot);
        }

        #endregion

        #region Dispatch :: Mouse Event

        private MouseEventArgs _DoMagnet(MouseEventArgs e)
        {
            var area = _UtilityService.Common.GetCurrentArea(e.X, e.Y);
            if (area != null)
            {
                var ms = _UtilityService.Common.GetMainSeries(area);
                if (ms != null)
                {
                    var axisX = _UtilityService.Common.GetMainAxisX(area);
                    var axisY = _UtilityService.Common.GetMainAxisY(area);
                    
                    if (axisX != null && axisY != null)
                    {
                        double x = axisX.PixelPositionToValue(e.X);
                        double y = axisY.PixelPositionToValue(e.Y);

                        var dp = ms.Points.FindNearestPoint(new PointF((float)x, (float)y));
                        if (dp != null)
                        {
                            if (dp.YValues.Length == 1)
                            {
                                e = new MouseEventArgs(e.Button, e.Clicks,
                                    (int)axisX.ValueToPixelPosition(dp.XValue),
                                    (int)axisY.ValueToPixelPosition(dp.YValues[0]),
                                    e.Delta);
                            }
                            else
                            {
                                double[] arr = new double[dp.YValues.Length];
                                for (int i = 0; i < arr.Length; i++)
                                    arr[i] = Math.Abs(dp.YValues[i] - y);
                                double min = arr.Min();
                                int idx_min = Array.IndexOf(arr, min);

                                e = new MouseEventArgs(e.Button, e.Clicks,
                                    (int)axisX.ValueToPixelPosition(dp.XValue),
                                    (int)axisY.ValueToPixelPosition(dp.YValues[idx_min]),
                                    e.Delta);
                            }
                        }
                    }
                }
            }
            return e;
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;
            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);

            if (h == null || !h.OnMouseMove(e)) base.OnMouseMove(e);
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {   
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;

            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);
                
            if (h == null || !h.OnMouseDown(e)) base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;

            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);

            if (h == null || !h.OnMouseUp(e)) base.OnMouseUp(e);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;

            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);

            if (h == null || !h.OnMouseWheel(e)) base.OnMouseWheel(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;

            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);

            if (h == null || !h.OnMouseClick(e)) base.OnMouseClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            IMouseEventHandler h = _Tools.SelectedTool as IMouseEventHandler;

            if (_UseMagnet || (Control.ModifierKeys & Keys.Control) != 0)
                e = _DoMagnet(e);

            if (h == null || !h.OnMouseDoubleClick(e)) base.OnMouseDoubleClick(e);
        }

        #endregion

        #region Dispatch :: Keyboard Event

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            IKeyboardEventHandler h = _Tools.SelectedTool as IKeyboardEventHandler;
            if (h == null || !h.OnKeyDown(e)) base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            IKeyboardEventHandler h = _Tools.SelectedTool as IKeyboardEventHandler;
            if (h == null || !h.OnKeyUp(e)) base.OnKeyUp(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            IKeyboardEventHandler h = _Tools.SelectedTool as IKeyboardEventHandler;
            if (h == null || !h.OnKeyPress(e)) base.OnKeyPress(e);
        }

        #endregion

        #region Dispatch :: Paint Event

        protected void OnAnnotationPaint(PaintEventArgs e)
        {   
            foreach (var anot in base.Annotations)
            {
                var h = _Tools.GetCompatiabe(anot) as IAnnotationEventHandler;
                if (h == null) continue;
                
                var axisX = anot.AxisX;
                if (axisX == null) continue;

                var axisY = anot.AxisY;
                if (axisY == null) continue;

                if (h.IsAlwaysPainting)
                {
                    h.AnnotationPaint(anot, e);
                    continue;
                }

                var line = anot as LineAnnotation;
                if (line != null && line.IsInfinitive)
                {
                    if (double.IsNaN(line.Width) && line.Height == 0 &&
                        (line.Y < axisY.Minimum || axisY.Maximum < line.Y)) continue;
                    else if (double.IsNaN(line.Height) && line.Width == 0 &&
                        (line.X < axisX.Minimum || axisX.Maximum < line.X)) continue;
                }
                else
                {
                    var rect = new RectangleF(
                        (float)axisX.Minimum, (float)axisY.Minimum,
                        (float)(axisX.Maximum - axisX.Minimum), (float)(axisY.Maximum - axisY.Minimum));

                    if (!(rect.Contains((float)anot.X, (float)anot.Y) ||
                        rect.Contains((float)anot.X, (float)(anot.Y + anot.Height)) ||
                        rect.Contains((float)(anot.X + anot.Width), (float)anot.Y) ||
                        rect.Contains((float)(anot.X + anot.Width), (float)(anot.Y + anot.Height))))
                        continue;
                }
                h.AnnotationPaint(anot, e);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnAnnotationPaint(e);

            IPaintEventHandler h = _Tools.SelectedTool as IPaintEventHandler;
            if (h != null) h.OnPaint(e);
        }

        #endregion

        #region Dispatch :: Cursor Event

        protected override void OnCursorChanged(EventArgs e)
        {
            base.OnCursorChanged(e);

            var tool = _Tools.SelectedTool;
            if (tool == null ||
                tool.Cursor == null) return;

            if (this.Cursor != _Tools.SelectedTool.Cursor)
                this.Cursor = _Tools.SelectedTool.Cursor;
        }

        #endregion

        #region Dispatch :: Customize Event

        protected override void OnCustomize()
        {
            if (DesignMode) return;

            foreach (var area in base.ChartAreas)
            {
                var view = area as ChartView;
                if (view == null) continue;

                var viewer = this._Viewers.FindByName(view.ChartViewer) ?? _Viewers.DefalutViewer;
                var h = viewer as IChartCustomizer;
                if (h != null) h.OnCustomize(view);
            }
            base.OnCustomize();
        }
        protected override void OnCustomizeLegend(LegendItemsCollection legendItems, string legendName)
        {
            if (DesignMode) return;

            foreach (var area in base.ChartAreas)
            {
                var view = area as ChartView;
                if (view == null) continue;

                var viewer = this._Viewers.FindByName(view.ChartViewer) ?? _Viewers.DefalutViewer;
                var h = viewer as IChartCustomizer;
                if (h != null) h.OnCustomizeLegend(view, legendItems, legendName);
            }
            base.OnCustomizeLegend(legendItems, legendName);
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            var anots = base.Annotations.ToArray();
            foreach (var anot in anots)
            {
                var h = _Tools.GetCompatiabe(anot) as IAnnotationEditor;
                if (h != null) h.Remove(anot);
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
