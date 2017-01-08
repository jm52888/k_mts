using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class CommonService
	{
        #region Variable and Property

        public const int MAX_WIDTH = 290000000 >> 16;
        public const int MAX_HEIGHT = 290000000 >> 16;

        private IServiceProvider _container;

		#endregion

		#region Constructor

		protected CommonService() { }
		public CommonService(IServiceProvider container)
		{
			_container = container;
		}

		#endregion

		#region Property

		private Chart _chart;
		public Chart Chart
		{
			get
			{
				if (_chart == null)
					_chart = (Chart)_container.GetService(typeof(Chart));
				return _chart;
			}
		}

		private object _dataSource;
		public object DataSource
		{
			get { return _dataSource; }
			set { _dataSource = value; }
		}

		private ChartAreaCollection _chartAreas;
		public ChartAreaCollection ChartAreas
		{
			get
			{
				if (_chartAreas == null)
					_chartAreas = Chart.ChartAreas;
				return _chartAreas;
			}
		}

		private SeriesCollection _series;
		public SeriesCollection Series
		{
			get
			{
				if (_series == null)
					_series = Chart.Series;
				return _series;
			}
		}

		private AnnotationCollection _annotations;
		public AnnotationCollection Annotations
		{
			get
			{
				if (_annotations == null)
					_annotations = Chart.Annotations;
				return _annotations;
			}
		}

		private LegendCollection _legends;
		public LegendCollection Legends
		{
			get
			{
				if (_legends == null)
					_legends = Chart.Legends;
				return _legends;
			}
		}

		private TitleCollection _titles;
		public TitleCollection Titles
		{
			get
			{
				if (_titles == null)
					_titles = Chart.Titles;
				return _titles;
			}
		}

		private ChartAdapterCollection _adapters;
		public ChartAdapterCollection Adapters
		{
			get
			{
				if (_adapters == null)
					_adapters = (ChartAdapterCollection)_container.GetService(typeof(ChartAdapterCollection));
				return _adapters;
			}
		}

		private ChartToolCollection _tools;
		public ChartToolCollection Tools
		{
			get
			{
				if (_tools == null)
					_tools = (ChartToolCollection)_container.GetService(typeof(ChartToolCollection));
				return _tools;
			}
		}

		private ChartViewerCollection _viewers;
		public ChartViewerCollection Viewers
		{
			get
			{
				if (_viewers == null)
					_viewers = (ChartViewerCollection)_container.GetService(typeof(ChartViewerCollection));
				return _viewers;
			}
		}

		#endregion

		#region Method (Get + CurrentArea/MainSeries/MainAxis/SubAxis)

		public ChartArea GetCurrentArea(int x, int y)
		{
            foreach (var area in ChartAreas)
            {// LTRB   
                float x_min = area.Position.X;
                float x_max = x_min + area.Position.Width * Chart.Width / 100;
                float y_min = area.Position.Y;
                float y_max = y_min + area.Position.Height * Chart.Height / 100;

                if (x_min <= x && x <= x_max &&
                    y_min <= y && y <= y_max) return area;
            }
            return null;            
		}

		public Series GetMainSeries(ChartArea area)
		{
			var view = area as ChartView;
			if (view != null)
			{
				return this.Series.FindByName(view.MainSeries) ??
						this.Series.FirstOrDefault(s => s.ChartArea == view.Name);
			}
			else
			{
				return this.Series.FirstOrDefault(s => s.ChartArea == area.Name);
			}
		}

		public Axis GetMainAxisX(ChartArea area)
		{
			var ms = GetMainSeries(area);
			if (ms == null) return null;

			return ms.XAxisType == AxisType.Primary ? area.AxisX : area.AxisX2;
		}       

        public Axis GetMainAxisY(ChartArea area)
		{
			var ms = GetMainSeries(area);
			if (ms == null) return null;

			return ms.YAxisType == AxisType.Primary ? area.AxisY : area.AxisY2;
		}        

        public Axis GetSubAxisX(ChartArea area)
		{
			var ms = GetMainSeries(area);
			if (ms == null) return null;

			return ms.XAxisType == AxisType.Primary ? area.AxisX2 : area.AxisX;
		}

		public Axis GetSubAxisY(ChartArea area)
		{
			var ms = GetMainSeries(area);
			if (ms == null) return null;

			return ms.YAxisType == AxisType.Primary ? area.AxisY2 : area.AxisY;
		}

        public Axis GetMainAxisX(Series s)
        {
            var area = ChartAreas.FindByName(s.ChartArea);
            if (area == null) return null;

            return s.XAxisType == AxisType.Primary ? area.AxisX : area.AxisX2;
        }

        public Axis GetMainAxisY(Series s)
        {
            var area = ChartAreas.FindByName(s.ChartArea);
            if (area == null) return null;

            return s.YAxisType == AxisType.Primary ? area.AxisY : area.AxisY2;
        }

        public Axis GetSubAxisX(Series s)
        {
            var area = ChartAreas.FindByName(s.ChartArea);
            if (area == null) return null;

            return s.XAxisType == AxisType.Primary ? area.AxisX2 : area.AxisX;
        }

        public Axis GetSubAxisY(Series s)
        {
            var area = ChartAreas.FindByName(s.ChartArea);
            if (area == null) return null;

            return s.YAxisType == AxisType.Primary ? area.AxisY2 : area.AxisY;
        }

        #endregion
    }
}
