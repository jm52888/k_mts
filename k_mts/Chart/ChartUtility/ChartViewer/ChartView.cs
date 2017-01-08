using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartView : ChartArea
	{
		#region Variable and Property

		protected string _ChartViewer = null;
		[Category("ChartView"), DefaultValue(null)]
		public string ChartViewer
		{
			get { return _ChartViewer; }
			set { _ChartViewer = value; }
		}

		protected string _MainSeries = null;
		[Category("ChartView"), DefaultValue(null)]
		public string MainSeries
		{
			get { return _MainSeries; }
			set { _MainSeries = value; }
		}

		protected int _ViewStart = 0;
		[Category("ChartView"), DefaultValue(0)]
		public int ViewStart
		{
			get { return _ViewStart; }
			set { _ViewStart = Math.Max(0, Math.Min(_ViewEnd, value)); }
		}

		protected int _ViewEnd = int.MaxValue;
		[Category("ChartView")]
		public int ViewEnd
		{
			get { return _ViewEnd; }
			set { _ViewEnd = value; }
		}

		protected int _ViewCount = 100;
		[Category("ChartView")]
		public int ViewCount
		{
			get { return _ViewCount; }
			set { _ViewCount = Math.Max(1, value); }
		}

		private int _MarginLeft = 1;
		[Category("ChartView"), DefaultValue(1)]
		public int MarginLeft
		{
			get { return _MarginLeft; }
			set { _MarginLeft = Math.Max(0, Math.Min(50, value)); }
		}

		private int _MarginRight = 0;
		[Category("ChartView"), DefaultValue(0)]
		public int MarginRight
		{
			get { return _MarginRight; }
			set { _MarginRight = Math.Max(0, Math.Min(50, value)); }
		}

		private int _MarginTop = 10;		
		[Category("ChartView"), DefaultValue(10)]
		public int MarginTop
		{
			get { return _MarginTop; }
			set { _MarginTop = Math.Max(0, Math.Min(50, value)); }
		}

		private int _MarginBottom = 10;
		[Category("ChartView"), DefaultValue(10)]
		public int MarginBottom
		{
			get { return _MarginBottom; }
			set { _MarginBottom = Math.Max(0, Math.Min(50, value)); }
		}

		private bool _UseAxisIntervalRatio = true;
		[Category("ChartView"), DefaultValue(true)]		
		public bool UseAxisIntervalRatio
		{
			get { return _UseAxisIntervalRatio; }
			set { _UseAxisIntervalRatio = value; }
		}

		private double _AxisXIntervalRatio = 25.0;
		[Category("ChartView"), DefaultValue(25.0)]		
		public double AxisXIntervalRatio
		{
			get { return _AxisXIntervalRatio; }
			set { _AxisXIntervalRatio = Math.Max(0, Math.Min(100, value)); }
		}

		private double _AxisYIntervalRatio = 25.0;
		[Category("ChartView"), DefaultValue(25.0)]
		public double AxisYIntervalRatio
		{
			get { return _AxisYIntervalRatio; }
			set { _AxisYIntervalRatio = Math.Max(0, Math.Min(100, value)); }
		}

		private bool _UseAutoStepSize = true;
		[Category("ChartView"), DefaultValue(true)]
		public bool UseAutoStepSize
		{
			get { return _UseAutoStepSize; }
			set { _UseAutoStepSize = value; }
		}

		private double _StepSize = 1.0;
		[Category("ChartView"), DefaultValue(1.0)]
		public double StepSize
		{
			get { return _StepSize; }
			set { _StepSize = value; }
		}

		private double _MinimumHeight = 0.00000001;
		[Category("ChartView"), DefaultValue(0.00000001)]
		public double MinmumHeight
		{
			get { return _MinimumHeight; }
			set { _MinimumHeight = Math.Max(0, value); }
		}

		private double _MinimumWidth = 0.00000001;
		[Category("ChartView"), DefaultValue(0.00000001)]
		public double MinimumWidth
		{
			get { return _MinimumWidth; }
			set { _MinimumWidth = value; }
		}

		#endregion

		#region Constructor

		public ChartView()
		{

		}

		#endregion		
	}
}
