using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Win32;

using k_mts;

public static class Global
{
	#region Global Variable and Property

	public static MainForm Main;
	public static DataSet DB { get; private set; } // internal db
	public static Dictionary<string, MyChartGroup> ChartGroups{ get; private set; }
	public static Dictionary<string, System.Windows.Forms.Cursor> Cursors { get; private set; }

	#endregion

	#region Program :: Information

	public static string ProgName { get { return "K-MTS"; } }
	public static string Version { get { return "v1.1"; } }
	public static string FullName { get { return ProgName + " " + Version; } }

	public static string DataPath 
	{
		get { return Path.Combine(Environment.CurrentDirectory, "Data"); }
	}
	public static string ChartPath
	{
		get { return Path.Combine(Environment.CurrentDirectory, "System", "Chart"); }
	}
	public static string CursorPath
	{
		get { return Path.Combine(Environment.CurrentDirectory, "System", "Cursor"); }
	}

	#endregion

	#region Program :: Init

	public static bool Init()
	{
		try
		{
			_InitDB();
			_InitCursor();						
			_InitSound();
			_InitCommCtrl();
			_InitChart();

			return true;
		}
		catch (Exception ex)
		{			
			MessageBox.Show(ex.Message, "프로그램 초기화 실패");
			return false;
		}
	}

	private static void _InitCommCtrl()
	{
		MsgBox.DefaultTitle = Global.ProgName;
	}

	private static void _InitSound()
	{
		
	}	

	private static void _InitCursor()
	{
		Cursors = new Dictionary<string, System.Windows.Forms.Cursor>();

		Cursors["Blue"] = new System.Windows.Forms.Cursor(WinApi.
			LoadCursorFromFile(Path.Combine(CursorPath, "blue.cur")));

		Cursors["Red"] = new System.Windows.Forms.Cursor(WinApi.
			LoadCursorFromFile(Path.Combine(CursorPath, "red.cur")));
	}	

	private static void _InitChart()
	{
		ChartGroups = new Dictionary<string, MyChartGroup>();

		DataTable dt = new DataTable();
		dt.LoadText(Path.Combine(ChartPath, "chartlist.txt"), '\t', false);

		foreach (DataRow r in dt.Rows)
		{
			MyChartGroup g = new MyChartGroup();
			g.Name = r[0].ToString();
			g.Dock = DockStyle.Fill;
			g.LoadChart(ChartPath);
			
			ChartGroups.Add(g.Name, g);
		}
	}

	private static void _InitDB()
	{
		DB = new DataSet("k_mts_db");
	}

	#endregion

	#region Program :: Uninit

	public static void Uninit()
	{
		try
		{
			_UninitDB();			
			_UninitCursor();
			_UninitSound();
			_UninitCommCtrl();
			_UninitChart(); // it should be called before disposed.
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "프로그램 종료작업 실패");
		}
	}

	private static void _UninitCommCtrl()
	{

	}

	private static void _UninitChart()
	{
        //foreach (var g in ChartGroups.Values)
        //    g.SaveChart(ChartPath);
	}

	private static void _UninitSound()
	{

	}

	private static void _UninitCursor()
	{
		foreach (var cur in Cursors.Values)
			cur.Dispose();
	}

	private static void _UninitDB()
	{

	}
		
	#endregion

	#region Notification

	public static void SetTitle(string msg = null)
	{
		if (msg == null)
			Main.Text = Global.FullName;
		else Main.Text = Global.FullName + " :: " + msg;
	}

	#endregion

	#region Internal DB

	public static DataTable FindTable(string tblName)
	{
		if (DB.Tables.Contains(tblName))
			return DB.Tables[tblName];
		else return null;
	}

	public static DataTable FindTable(params string[] paths)
	{
		string tblName = "";
		int nLen = paths.Length;
		if (nLen == 0) return null;
		for (int i = 0; i < nLen; i++)
		{
			if (i == nLen - 1)
				tblName += paths[i];
			else tblName += paths[i] + ".";
		}

		if (DB.Tables.Contains(tblName))
			return DB.Tables[tblName];
		else return null;
	}

	#endregion
}