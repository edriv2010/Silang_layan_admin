using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class CRoutine
{
	public delegate void DoAdd();

	public delegate void LoadDataAdd();

	public delegate void DisableControls();

	public delegate void LoadDataInvokerNoParameter();

	public delegate void LoadDataInvoker(int PageNumber, string ShowingFields, string FieldPencarian, string KataKunci);

	public struct LoadDataResult
	{
		public DataTable Data;

		public int ItemCount;
	}

	public static void InitPageAdd(Page Page, string PageTitle, string TableName, Label lbSubTitle, Label lbError, Label lbInfo, LinkButton lbAdd, LinkButton lbEdit, LinkButton lbDelete, Button btSave, MessageBoxUsc_MsgBoxUsc uscMsgBox1, MessageBoxUsc_MsgBoxUsc.MsgBoxEventHandler MessageAnswered, DoAdd DoAdd, LoadDataAdd LoadData, DisableControls DisableControls, out string DataID)
	{
		DataID = "";
		lbError.Visible = false;
		lbInfo.Visible = false;
		uscMsgBox1.MsgBoxAnswered += MessageAnswered;
		if (Page.Request["id"] != null)
		{
			DataID = Page.Request["id"].ToString();
		}
		if (!Page.IsPostBack)
		{
			lbSubTitle.Text = "Detail " + PageTitle;
			Page.Session.Add("CurrentAction", "View");
			Page.Session.Add("CurrentTitle", PageTitle);
			if (Page.Request["act"] != null)
			{
				if (Page.Session["CurrentUserLoginID"] == null)
				{
					Page.Response.Redirect("Login2.aspx");
					return;
				}
				if (Page.Request["act"].ToString() == "add")
				{
					DoAdd();
					return;
				}
			}
			LoadData();
			btSave.Visible = false;
			DisableControls();
		}
		if (Page.Session["CurrentRedirect"] != null && Page.Session["CurrentRedirect"].ToString() == "Add")
		{
			lbInfo.Visible = true;
			Page.Session.Remove("CurrentRedirect");
		}
		if (Page.Session["CurrentUserLoginID"] == null)
		{
			if (lbAdd != null)
			{
				lbAdd.Visible = false;
			}
			lbEdit.Visible = false;
			lbDelete.Visible = false;
		}
	}

	public static void InitPageList(Page Page, string PageURL, string Title, int[] AuthList, out string FieldPencarian, out string KataKunci, string ShowingFields, string[] KriteriaPencarian, HtmlContainerControl divTools, LoadDataInvoker AcceptorLoadData)
	{
		Page.Session.Add("CurrentPage", PageURL);
		Page.Session.Add("CurrentTitle", Title);
		int num = 1;
		if (Page.Session[MySession.CurrentPage] != null)
		{
			string text = Page.Session[MySession.CurrentPage].ToString();
			if (text == Path.GetFileName(Page.Request.Path))
			{
				if (Page.Session[text + "CurrentIndexPage"] != null && num != -1)
				{
					num = int.Parse(Page.Session[text + "CurrentIndexPage"].ToString());
				}
				else
				{
					Page.Session.Add(text + "CurrentIndexPage", 1);
				}
			}
		}
		KataKunci = "";
		FieldPencarian = "";
		if (Page.Request["kr"] != null)
		{
			FieldPencarian = Page.Request["kr"].ToString();
			Page.Session.Add(Page.Session["CurrentPage"].ToString() + "CurrentSelTextKriteriaPencarian", FieldPencarian);
		}
		else if (Page.Session["CurrentPage"] != null && Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentSelTextKriteriaPencarian"] != null)
		{
			FieldPencarian = Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentSelTextKriteriaPencarian"].ToString();
		}
		if (Page.Request["kk"] != null)
		{
			KataKunci = Page.Request["kk"].ToString();
			Page.Session.Add(Page.Session["CurrentPage"].ToString() + "CurrentKataKunci", KataKunci);
		}
		else if (Page.Session["CurrentPage"] != null && Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentKataKunci"] != null)
		{
			KataKunci = Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentKataKunci"].ToString();
		}
		if (Page.Session["CurrentUserLoginID"] == null && divTools != null)
		{
			divTools.Visible = false;
		}
		if (!Page.IsPostBack)
		{
			if (Page.Session["CurrentPage"] != null && Page.Session["CurrentPage"].ToString() == PageURL && Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentIndexPage"] != null && Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentIndexPage"] != null)
			{
				num = int.Parse(Page.Session[Page.Session["CurrentPage"].ToString() + "CurrentIndexPage"].ToString());
			}
			Page.Session.Add(Page.Session["CurrentPage"].ToString() + "CurrentKriteriaPencarian", KriteriaPencarian);
			AcceptorLoadData(num, ShowingFields, FieldPencarian, KataKunci);
		}
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxData, string SQLJOIN, string SQLAND, string ShowingFields, TwoArrayList tar)
	{
		if (PageNumber != -1 && PageNumber <= 0)
		{
			PageNumber = 1;
		}
		string text = "";
		if (Page.Session[MySession.CurrentPage] != null)
		{
			text = Page.Session[MySession.CurrentPage].ToString();
			Page.Session.Add(MySession.CurrentIndexPage + text, PageNumber);
		}
		else
		{
			Page.Session.Add(MySession.CurrentPage, Path.GetFileName(Page.Request.Path));
			text = Page.Session[MySession.CurrentPage].ToString();
			Page.Session.Add(MySession.CurrentIndexPage + text, PageNumber);
		}
		string text2 = Page.Session[MySession.CurrentPage].ToString();
		Page.Session.Add(text2 + "KriteriaPencarian", FieldPencarian);
		Page.Session.Add(text2 + "KataKunci", KataKunci);
		Connection.SetConnection();
		SQLAND = (string.IsNullOrEmpty(SQLAND) ? "" : (" " + SQLAND));
		string text3 = (string.IsNullOrEmpty(FieldPencarian) ? "" : FieldPencarian.Replace(".", ""));
		if (text3.Contains("(") || text3.Contains(")") || text3.Contains("=") || text3.Contains(" "))
		{
			text3 = "Param1";
		}
		if (tar == null)
		{
			tar = new TwoArrayList();
			if (!string.IsNullOrEmpty(KataKunci) && KataKunci != "")
			{
				switch (Connection.ServerType)
				{
				case Connection.EServerType.Oracle:
					if (!MyApplication.IsCaseSencitive)
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
					}
					else
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND UPPER(" + FieldPencarian + ") LIKE UPPER(" + Connection.ParameterSymbol + text3 + ")";
					}
					break;
				case Connection.EServerType.MySQL:
				{
					KataKunci = KataKunci.Replace(":", "%").Replace("=", "%").Replace("/", "%")
						.Replace("\t", "%");
					string text4 = SQLAND;
					SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
					break;
				}
				}
				if (Page.Session[text + "TipePencarian"] != null)
				{
					if (Page.Session[text + "TipePencarian"].ToString() == "0")
					{
						tar.Add(text3, KataKunci.ToUpper());
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "1")
					{
						tar.Add(text3, KataKunci.ToUpper() + "%");
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "2")
					{
						tar.Add(text3, "%" + KataKunci.ToUpper());
					}
					else
					{
						tar.Add(text3, "%" + KataKunci.ToUpper() + "%");
					}
				}
				else
				{
					tar.Add(text3, "%" + KataKunci.ToUpper() + "%");
				}
			}
		}
		else if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				tar.SetItem2(i, tar.Item2(i).ToString().Replace(":", "%")
					.Replace("=", "%")
					.Replace("/", "%")
					.Replace("\t", "%"));
				if (tar.Item1(i).Contains("(") || tar.Item1(i).Contains(")") || tar.Item1(i).Contains("=") || tar.Item1(i).Contains(" "))
				{
					tar.SetItem1(i, "Param" + i);
				}
				string text4 = SQLAND;
				SQLAND = text4 + " AND " + tar.Item1(i) + " LIKE " + Connection.ParameterSymbol + tar.Item1(i);
			}
		}
		else
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				if (tar.Item1(i).Contains("(") || tar.Item1(i).Contains(")") || tar.Item1(i).Contains("=") || tar.Item1(i).Contains(" "))
				{
					tar.SetItem1(i, "Param" + i);
				}
				if (!MyApplication.IsCaseSencitive)
				{
					string text4 = SQLAND;
					SQLAND = text4 + " AND " + tar.Item1(i) + " LIKE " + Connection.ParameterSymbol + tar.Item1(i);
				}
				else
				{
					string text4 = SQLAND;
					SQLAND = text4 + " AND UPPER(" + tar.Item1(i) + ") LIKE UPPER(" + Connection.ParameterSymbol + tar.Item1(i).ToUpper() + ")";
				}
			}
		}
		SQLAND = SQLAND.Replace("AND AND", "AND").Replace("OR AND", "OR");
		string text5 = "";
		int num = 0;
		try
		{
			text5 = "SELECT COUNT(*) FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			num = int.Parse(Command.ExecScalar(tar, text5, "0"));
		}
		catch (Exception ex)
		{
			Util.ShowAlertMessage(ex.Message);
			return default(LoadDataResult);
		}
		string text6 = "";
		switch (Connection.ServerType)
		{
		case Connection.EServerType.MySQL:
			text6 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? (" ORDER BY " + Page.Session[text + MySession.CurrentSortExp].ToString()) : (" ORDER BY " + OrderFields));
			break;
		case Connection.EServerType.Oracle:
			text6 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? (" ORDER BY " + Page.Session[text + MySession.CurrentSortExp].ToString()) : (" ORDER BY " + OrderFields));
			break;
		}
		string text7 = "";
		string sQL = "";
		if (PageNumber != -1)
		{
			if (PageNumber <= 0)
			{
				PageNumber = 1;
				Page.Session.Add(MySession.CurrentIndexPage, 1);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				sQL = "SELECT *  FROM ( SELECT a.*, ROWNUM as No  FROM ( SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + "  WHERE 1=1 " + SQLAND + " " + text6 + ") a  WHERE ROWNUM <= " + PageNumber * MaxData + ")  WHERE No >= " + ((PageNumber - 1) * MaxData + 1);
				break;
			case Connection.EServerType.MySQL:
				text7 = " LIMIT " + (PageNumber - 1) * MaxData + "," + MaxData;
				sQL = "SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND + text6 + text7;
				break;
			}
		}
		else
		{
			sQL = "SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND + text6;
		}
		DataTable dataTable = new DataTable();
		dataTable = Command.ExecDataAdapter(sQL, tar);
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			if (PageNumber != -1)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxData + (i + 1);
				}
				break;
			}
			dataTable.Columns.Add("No");
			dataTable.Columns["No"].SetOrdinal(0);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				dataTable.Rows[i]["No"] = i + 1;
			}
			break;
		}
		case Connection.EServerType.MySQL:
			dataTable.Columns.Add("No");
			dataTable.Columns["No"].SetOrdinal(0);
			if (PageNumber != -1)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxData + (i + 1);
				}
			}
			else
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["No"] = i + 1;
				}
			}
			break;
		}
		dataTable.TableName = TableName;
		dgData.DataSource = dataTable;
		dgData.DataBind();
		LoadDataResult result = default(LoadDataResult);
		result.Data = dataTable;
		result.ItemCount = num;
		return result;
	}

	public static void Load_PageNumber(int ItemCount, int MaxItemPerPage, Repeater RepeaterPage)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("PageNumber");
		for (int i = 1; i <= (int)Math.Ceiling((double)ItemCount / (double)MaxItemPerPage); i++)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow[0] = i;
			dataTable.Rows.Add(dataRow);
		}
		RepeaterPage.DataSource = dataTable;
		RepeaterPage.DataBind();
	}

	public static void dgData_OnSortCommand(Page Page, DataGridSortCommandEventArgs e, LoadDataInvokerNoParameter AcceptorLoadData)
	{
		string text = e.SortExpression.ToString();
		if (Page.Session[Page.Session["CurrentPage"].ToString() + "Current" + text] == null)
		{
			Page.Session.Add(Page.Session["CurrentPage"].ToString() + "Current" + text, "ASC");
		}
		else if (Page.Session[Page.Session["CurrentPage"].ToString() + "Current" + text].ToString() == "ASC")
		{
			Page.Session[Page.Session["CurrentPage"].ToString() + "Current" + text] = "DESC";
		}
		else
		{
			Page.Session[Page.Session["CurrentPage"].ToString() + "Current" + text] = "ASC";
		}
		Page.Session.Add(Page.Session["CurrentPage"].ToString() + "CurrentSortExp", text + " " + Page.Session[Page.Session["CurrentPage"].ToString() + "Current" + text].ToString());
		AcceptorLoadData();
	}

	public static bool IsLoginAuth(Page Page)
	{
		if (Page.Session["CurrentUserLoginID"] == null)
		{
			return false;
		}
		return true;
	}

	public static void SetTitle(Page Page, string Title)
	{
		Page.Session.Add("CurrentTitle", Title);
	}
}
