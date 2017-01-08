using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Globalization;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartNamedUtilityCollection<T> : ChartUtilityCollection<T>, IUtilityNameController
		where T : ChartNamedUtility
	{
		#region Property

		protected virtual string NamePrefix
		{
			get { return typeof(T).Name; }
		}

		#endregion

		#region Method

		public int IndexOf(string name)
		{
			int idx = 0;
			foreach (T util in this)
			{
				if (util.Name == name) return idx;
				++idx;
			}
			return -1;
		}

		public virtual T FindByName(string name)
		{
			foreach (T util in this)
			{
				if (util.Name == name)
					return util;
			}
			return null;
		}

		protected virtual string NextUniqueName()
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

		protected void VerifyNameReference(string name)
		{	
			if (this.Chart != null && /*!this.Chart.IsSerializing &&*/ !this.IsNameReferenceValid(name))
			{
				throw ChartExceptionFactory.NameNotFound(name, this.GetType().Name);
			}
		}

		protected bool IsNameReferenceValid(string name)
		{
			if (!string.IsNullOrEmpty(name) && !(name == "NotSet"))
				return this.IndexOf(name) >= 0;
			return true;
		}

		#endregion		

		#region Event

		public event EventHandler<UtilityNameChangedEventArgs> NameChanged;
		public event EventHandler<UtilityNameChangedEventArgs> NameChanging;

		#endregion

		#region [Collection<T>] Override Method

		protected virtual void FixNameReferences(T item) { }

		protected override void RemoveItem(int index)
		{
			ChartNamedUtility oldUtil = index < this.Count ? this[index] : default(T);
			if (this._cntColectionEditing == 0)
				OnUtilityNameChanged(new UtilityNameChangedEventArgs(oldUtil, null));
			base.RemoveItem(index);
			if (this._cntColectionEditing != 0)
				return;
			ChartNamedUtility newUtil = this.Count > 0 ? this[0] : default(T);
			OnUtilityNameChanged(new UtilityNameChangedEventArgs(oldUtil, newUtil));
		}
		protected override void InsertItem(int index, T item)
		{
			if (string.IsNullOrEmpty(item.Name))
				item.Name = this.NextUniqueName();
			else if (!this.IsUniqueName(item.Name))
				throw ChartExceptionFactory.NameAlreadyExists(item.Name, this.GetType().Name);

			this.FixNameReferences(item);
			base.InsertItem(index, item);
			if (this.Count != 1 || (object)item == null) return;
			OnUtilityNameChanged(new UtilityNameChangedEventArgs(null, item));
		}
		protected override void SetItem(int index, T item)
		{
			if (string.IsNullOrEmpty(item.Name))
				item.Name = this.NextUniqueName();
			else if (!this.IsUniqueName(item.Name) && this.IndexOf(item.Name) != index)
				throw ChartExceptionFactory.NameAlreadyExists(item.Name, this.GetType().Name);

			this.FixNameReferences(item);
			ChartNamedUtility oldUtil = index < this.Count ? this[index] : default(T);
			OnUtilityNameChanging(new UtilityNameChangedEventArgs(oldUtil, item));
			base.SetItem(index, item);
			OnUtilityNameChanged(new UtilityNameChangedEventArgs(oldUtil, item));
		}

		#endregion		

		#region Contructor

		public ChartNamedUtilityCollection(IChartUtility parent)
			: base(parent)
		{

		}

		#endregion

		#region IUtilityNameController

		private int _cntColectionEditing;
		public bool IsColectionEditing
		{
			get
			{
				return _cntColectionEditing == 0;
			}
			set
			{
				this._cntColectionEditing = this._cntColectionEditing + (value ? 1 : -1);
			}
		}

		private List<T> _Snapshot;
		public IList Snapshot
		{
			get { return _Snapshot; }
		}

		public bool IsUniqueName(string name)
		{
			return this.FindByName(name) == null;
		}

		public void DoSnapshot(bool save,
			EventHandler<UtilityNameChangedEventArgs> changingCallback, 
			EventHandler<UtilityNameChangedEventArgs> changedCallback)
		{
			if (save)
			{
				_Snapshot = new List<T>((IEnumerable<T>)this);

				if (changedCallback != null)
					this.NameChanging += changedCallback;
				if (changedCallback != null)
					this.NameChanged += changedCallback;				
			}
			else
			{
				if (changedCallback != null)
					this.NameChanging -= changedCallback;
				if (changedCallback != null)
					this.NameChanged -= changedCallback;
			
				_Snapshot.Clear();
				_Snapshot = null;
			}
		}

		public void OnUtilityNameChanged(UtilityNameChangedEventArgs e)
		{
			if (this.IsSuspended || NameChanged == null) return;
			this.NameChanged(this, e);				
		}

		public void OnUtilityNameChanging(UtilityNameChangedEventArgs e)
		{
			if (this.IsSuspended || NameChanging == null) return;
			this.NameChanging(this, e);
		}

		#endregion		

		#region Indexer

		public T this[string name]
		{
			get
			{
				int idx = this.IndexOf(name);
				if (idx == -1)
					throw ChartExceptionFactory.NameNotFound(name, this.GetType().Name);
				return this[idx];
			}
			set
			{
				int idx = this.IndexOf(name);
				int num = this.IndexOf(value);
				bool flag1 = idx > -1;
				bool flag2 = num > -1;
				
				if (!flag1 && !flag2) this.Add(value);
				else if (flag1 && !flag2) this[idx] = value;
				else if (!flag1 & flag2)
					throw ChartExceptionFactory.NameAlreadyExists(name, this.GetType().Name);
				else if (flag1 & flag2 && idx != num)
					throw ChartExceptionFactory.NameAlreadyExists(name, this.GetType().Name);				
			}
		}

		#endregion
	}
}
