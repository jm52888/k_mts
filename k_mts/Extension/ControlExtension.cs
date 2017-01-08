using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;

namespace System.Windows.Forms
{
	public static class ControlExtension
	{
		#region 속성설정(SetAttribute, GetAttribute)

		public static void SetAttribute(this Control ctrl, string name, object obj)
		{
			Dictionary<string, object> dic = ctrl.Tag as Dictionary<string, object>;
			if (dic == null)
			{
				dic = new Dictionary<string, object>();
				if (ctrl.Tag != null)
					dic["Tag"] = ctrl.Tag;
				ctrl.Tag = dic;
			}
			dic[name] = obj;
		}

		public static object GetAttribute(this Control ctrl, string name)
		{
			Dictionary<string, object> dic = ctrl.Tag as Dictionary<string, object>;
			if (dic == null) return null;
			
			object value;
			dic.TryGetValue(name, out value);
			return value;
		}

		public static T GetAttribute<T>(this Control ctrl, string name)
		{
			Dictionary<string, object> dic = ctrl.Tag as Dictionary<string, object>;
			if (dic == null) return default(T);

			object value;
			dic.TryGetValue(name, out value);
			return (T)value;
		}
		
		#endregion

		#region 연속추가(Adds) : Add + 's'

		public static void Adds(this Control.ControlCollection cc, params Control[] ctrls)
		{
			foreach (Control ctrl in ctrls) cc.Add(ctrl);
		}

		#endregion

		#region 연속실행(Invokes, Invokes<T>) : Invoke + 's'

		public static void Invokes(this Control.ControlCollection cc, Action<Control> act, bool bCheckInvokeRequired = true)
		{
			foreach (Control c in cc)
			{
				if (bCheckInvokeRequired && c.InvokeRequired)
					c.Invoke(act, c);
				else act(c);
			}
		}

		public static void Invokes(this ICollection<Control> cc, Action<Control> act, bool bCheckInvokeRequired = true)
		{
			foreach (Control c in cc)
			{
				if (bCheckInvokeRequired && c.InvokeRequired)
					c.Invoke(act, c);
				else act(c);
			}
		}

		public static void Invokes<T>(this Control.ControlCollection cc, Action<T> act, bool bCheckInvokeRequired = true)
			where T : Control
		{
			foreach (Control c in cc)
			{
				T ctrl = c as T;
				if (ctrl != null)
				{
					if (bCheckInvokeRequired && ctrl.InvokeRequired)
						ctrl.Invoke(act, ctrl);
					else act(ctrl);
				}
			}
		}

		public static void Invokes<T>(this ICollection<Control> cc, Action<T> act, bool bCheckInvokeRequired = true)
			where T : Control
		{
			foreach (Control c in cc)
			{
				T ctrl = c as T;
				if (ctrl != null)
				{
					if (bCheckInvokeRequired && ctrl.InvokeRequired)
						ctrl.Invoke(act, ctrl);
					else act(ctrl);
				}
			}
		}

		#endregion

		#region 컨트롤 보이기 (ShowForm, ShowDialog)

		/// <summary>
		/// Display the control to user in a new form.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="title"></param>
		/// <param name="startPosition"></param>
		/// <param name="dockStyle"></param>
		public static Form ShowForm(this Control ctrl, string title = "",
			FormBorderStyle style = FormBorderStyle.Sizable,
			FormStartPosition startPosition = FormStartPosition.CenterScreen,
			DockStyle dockStyle = DockStyle.Fill)
		{
			Form f = ctrl as Form;
			f = f ?? new Form();

			f.FormBorderStyle = style;
			f.StartPosition = startPosition;
			f.Text = title;
			f.ClientSize = new Size(ctrl.Width, ctrl.Height);

			if (!(ctrl is Form))
			{
				ctrl.Dock = dockStyle;
				ctrl.Parent = f;
			}
			f.Show();
			return f;
		}

		/// <summary>
		/// Display the control to user in a new dialog form.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="title"></param>
		/// <param name="startPosition"></param>
		/// <param name="dockStyle"></param>
		public static Form ShowDialog(this Control ctrl, string title = "",
			FormBorderStyle style = FormBorderStyle.Sizable,
			FormStartPosition startPosition = FormStartPosition.CenterScreen,
			DockStyle dockStyle = DockStyle.Fill)
		{
			Form f = ctrl as Form;
			f = f ?? new Form();

			f.FormBorderStyle = style;
			f.StartPosition = startPosition;
			f.Text = title;
			f.ClientSize = new Size(ctrl.Width, ctrl.Height);

			if (!(ctrl is Form))
			{
				ctrl.Dock = dockStyle;
				ctrl.Parent = f;
			}
			f.ShowDialog();
			return f;
		}

		/// <summary>
		/// Display the control to user in a new popup form.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="title"></param>
		/// <param name="startPosition"></param>
		/// <param name="dockStyle"></param>
		public static Form ShowPopup(this Control ctrl, string title = "",
			FormBorderStyle style = FormBorderStyle.SizableToolWindow,
			FormStartPosition startPosition = FormStartPosition.CenterScreen,
			DockStyle dockStyle = DockStyle.Fill)
		{
			Form f = ctrl as Form;
			f = f ?? new Form();

			f.FormBorderStyle = style;
			f.StartPosition = startPosition;
			f.Text = title;
			f.ClientSize = new Size(ctrl.Width, ctrl.Height);

			if (!(ctrl is Form))
			{
				ctrl.Dock = dockStyle;
				ctrl.Parent = f;
			}
			f.Show();
			return f;
		}

		/// <summary>
		/// Display the control to user in a new popup form.
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="title"></param>
		/// <param name="startPosition"></param>
		/// <param name="dockStyle"></param>
		public static Form ShowPopupDialog(this Control ctrl, string title = "",
			FormBorderStyle style = FormBorderStyle.SizableToolWindow,
			FormStartPosition startPosition = FormStartPosition.CenterScreen,
			DockStyle dockStyle = DockStyle.Fill)
		{
			Form f = ctrl as Form;
			f = f ?? new Form();

			f.FormBorderStyle = style;
			f.StartPosition = startPosition;
			f.Text = title;
			f.ClientSize = new Size(ctrl.Width, ctrl.Height);

			if (!(ctrl is Form))
			{
				ctrl.Dock = dockStyle;
				ctrl.Parent = f;
			}
			f.ShowDialog();
			return f;
		}

		#endregion

		#region 부모 윈도우폼 획득 (GetParentForm)

		public static Form GetParentForm(this Control ctrl)
		{
			if (ctrl.Parent == null) return null;
			return ctrl.Parent as Form ?? ctrl.Parent.GetParentForm();
		}

        #endregion

        #region 자식 컨트롤 검색 (GetChild)

        public static Control GetChild(this Control parent, int idx = 0)
		{
			if (idx >= parent.Controls.Count) return null;
			return parent.Controls[idx];
		}

		public static T GetChild<T>(this Control parent, int idx = 0)
			where T : Control
		{
			foreach (Control ctrl in parent.Controls)
			{
				if (ctrl is T && --idx < 0)
					return (T)ctrl;
			}
			return null;
		}

		#endregion
	}
}