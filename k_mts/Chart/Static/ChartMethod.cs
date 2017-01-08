using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class ChartMethod
	{
        #region Load/Save

        public static void Load(object setting, Dictionary<string, object> src)
		{
			Dictionary<string, object> _backup = new Dictionary<string, object>();
			ChartMethod.Save(setting, _backup);

			try
			{
				foreach (var prop in setting.GetType().GetProperties())
				{
					if (prop.CanRead && prop.CanWrite && src.ContainsKey(prop.Name))
						prop.SetValue(setting, src[prop.Name]);
				}
			}
			catch (Exception ex)
			{
				ChartMethod.Load(setting, _backup);
				throw ex;
			}
		}

		public static void Save(object setting, Dictionary<string, object> dest)
		{
			dest.Clear();
			foreach (var prop in setting.GetType().GetProperties())
			{
				if (prop.CanRead && prop.CanWrite && prop.PropertyType.IsSerializable)
					dest.Add(prop.Name, prop.GetValue(setting));
			}
		}

		#endregion

		#region Assign

		public static void Assign(object dest, object src) // TODO : Option - BindingFlags
		{
			var destProps = dest.GetType().GetProperties();
			var srcProps = src.GetType().GetProperties();

			foreach (var destProp in destProps)
			{
				if (destProp.CanWrite)
				{
					var srcProp = srcProps.FirstOrDefault(
						p => p.Name == destProp.Name && p.PropertyType == destProp.PropertyType);
					if (srcProp == null) continue;

					destProp.SetValue(dest, srcProp.GetValue(src));
				}
			}
		}

		#endregion

//		#region ReplaceMethod

//		public static void ReplaceMethod<Target, Source>(string targetName, string sourceName, BindingFlags flags = 0)
//		{
//			MethodInfo targetMethod = typeof(Target).GetMethod(targetName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | flags);
//			MethodInfo sourceMethod = typeof(Source).GetMethod(sourceName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | flags);
//			RuntimeHelpers.PrepareMethod(targetMethod.MethodHandle);
//			RuntimeHelpers.PrepareMethod(sourceMethod.MethodHandle);

//			unsafe
//			{
//				if (IntPtr.Size == 4)
//				{
//					int* inj = (int*)sourceMethod.MethodHandle.Value.ToPointer() + 2;
//					int* tar = (int*)targetMethod.MethodHandle.Value.ToPointer() + 2;
//#if DEBUG
//					Console.WriteLine("\nVersion x84 Debug\n");

//					byte* injInst = (byte*)*inj;
//					byte* tarInst = (byte*)*tar;

//					int* injSrc = (int*)(injInst + 1);
//					int* tarSrc = (int*)(tarInst + 1);

//					*tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
//#else
//					Console.WriteLine("\nVersion x84 Release\n");
//					*tar = *inj;
//#endif
//				}
//				else
//				{

//					long* inj = (long*)sourceMethod.MethodHandle.Value.ToPointer() + 1;
//					long* tar = (long*)targetMethod.MethodHandle.Value.ToPointer() + 1;
//#if DEBUG
//					Console.WriteLine("\nVersion x64 Debug\n");
//					byte* injInst = (byte*)*inj;
//					byte* tarInst = (byte*)*tar;


//					int* injSrc = (int*)(injInst + 1);
//					int* tarSrc = (int*)(tarInst + 1);

//					*tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
//#else
//					Console.WriteLine("\nVersion x64 Release\n");
//					*tar = *inj;
//#endif
//				}
//			}
//		}

//		#endregion
	}
}
