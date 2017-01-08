using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartUtilityCollection<T> : Collection<T>, IChartUtility, IDisposable
		where T : ChartUtility
	{
		#region Property

		[Browsable(false), DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object Tag { get; set; }

		private int _cntSuspended;
		public bool IsSuspended
		{
			get { return _cntSuspended > 0; }
		}

		public Chart Chart
		{
			get
			{
				if (this.Common == null)
					return null;
				return this.Common.Chart;
			}
		}

		#endregion

		#region Method

		public void SuspendUpdates()
		{
			_cntSuspended += 1;
		}
		public void ResumeUpdates()
		{
			if (_cntSuspended > 0)
				_cntSuspended -= 1;

			if (_cntSuspended == 0)
				this.Update();
		}

		#endregion

		#region Override Method [Collection<T>]

		protected virtual void Initialize(T item) { }
		protected virtual void Deinitialize(T item) { }

		protected override void ClearItems()
		{
			this.SuspendUpdates();
			while (this.Count > 0)
				this.RemoveItem(0);
			this.ResumeUpdates();
		}
		protected override void RemoveItem(int index)
		{
			this.Deinitialize(this[index]);
			this[index].Parent = null;
			base.RemoveItem(index);
			this.Update();
		}
		protected override void InsertItem(int index, T item)
		{
			this.Initialize(item);
			item.Parent = this;
			base.InsertItem(index, item);
			this.Update();
		}
		protected override void SetItem(int index, T item)
		{
			this.Initialize(item);
			item.Parent = this;
			base.SetItem(index, item);
			this.Update();
		}

		#endregion

		#region Constructor

		protected ChartUtilityCollection() { }
		public ChartUtilityCollection(IChartUtility parent)
		{
			_Parent = parent;
		}

		#endregion

		#region IChartUtility

		private IChartUtility _Parent;
		public IChartUtility Parent
		{
			get { return _Parent; }
			set { _Parent = value; }
		}

		private CommonService _Common;
		public CommonService Common
		{
			get
			{
				if (_Common == null && Parent != null)
					_Common = this._Parent.Common;
				return _Common;
			}
			set
			{
				_Common = value;
			}
		}

		public virtual void Update()
		{
			if (_Parent == null || IsSuspended) return;
			_Parent.Update();
		}
		void IChartUtility.Update()
		{
			this.Update();
		}

		#endregion

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing) return;

			foreach (T util in this)
				util.Dispose();
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion		
	}
}
