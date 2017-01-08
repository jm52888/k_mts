using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartUtility : IChartUtility, IDisposable
	{
		#region Variable and Property

		[Browsable(false), DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag { get; set; }

		[Browsable(false)]
		public Chart Chart
		{
			get
			{
				if (this.Common != null)
					return this.Common.Chart;
				return (Chart)null;
			}
		}

		#endregion

		#region Constructor

		protected ChartUtility() { }
		public ChartUtility(IChartUtility parent)
		{
			Parent = parent;
		}

		#endregion

		#region IChartUtility

		private IChartUtility _Parent;
		[Browsable(false)]
		public IChartUtility Parent
		{
			get { return _Parent; }
			set { _Parent = value; }
		}

		private CommonService _Common;
		[Browsable(false)]
		public CommonService Common
		{
			get
			{
				if (_Common == null && _Parent != null)
					_Common = _Parent.Common;
				return _Common;
			}
			set
			{
				_Common = value;
			}
		}
		CommonService IChartUtility.Common
		{
			get { return this.Common; }
		}

		public virtual void Update() { }
		void IChartUtility.Update()
		{
			if (_Parent == null) return;
			_Parent.Update();
		}

		#endregion

		#region IDisposable

		protected virtual void Dispose(bool disposing) { }
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion	
	}
}
