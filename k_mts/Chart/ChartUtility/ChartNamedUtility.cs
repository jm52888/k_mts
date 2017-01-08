using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartNamedUtility : ChartUtility
	{
		#region Property

		private string _Name = string.Empty;
		public virtual string Name
		{
			get { return _Name; }
			set
			{
				if (!(_Name != value))
					return;

				if (this.Parent is IUtilityNameController)
				{
					IUtilityNameController ctrl = this.Parent as IUtilityNameController;
					if (!ctrl.IsUniqueName(value))
						throw ChartExceptionFactory.NameAlreadyExists(value, ctrl.GetType().Name);

					UtilityNameChangedEventArgs e = new UtilityNameChangedEventArgs(this, _Name, value);
					ctrl.OnUtilityNameChanging(e);
					_Name = value;
					ctrl.OnUtilityNameChanged(e);
				}
				else
					this._Name = value;

				this.Update();
			}
		}

		#endregion

		#region Constructor

		protected ChartNamedUtility() { }
		protected ChartNamedUtility(string name)
		{
			_Name = name;
		}
		public ChartNamedUtility(IChartUtility parent, string name)
			: base(parent)
		{
			_Name = name;
		}

		#endregion

		#region Override

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this._Name))
				return this.GetType().Name + '-' + _Name;
			else return this.GetType().Name;
		}

		#endregion
	}
}
