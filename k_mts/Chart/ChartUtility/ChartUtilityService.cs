using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Design;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartUtilityService : ServiceContainer, IChartUtility
	{
		#region Constructor

		protected ChartUtilityService() { }
		public ChartUtilityService(Chart chart)
		{
			base.AddService(typeof(Chart), chart);			
			_Common = new CommonService(this);
		}

		#endregion

		#region IChartUtility

		public IChartUtility Parent
		{
			get { return null; } // ChartUtilityService is top most utility
			set { }
		}

		private CommonService _Common;
		public CommonService Common
		{
			get { return _Common; }
		}

		public void Update()
		{
			
		}

		#endregion		
	}
}
