using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public class DataUIProvider
{
	public struct LoadDataResult
	{
		public DataTable Data;

		public int ItemCount;
	}

	public delegate void DelegateToggleBind(bool IsToggleBind);

	public delegate void LoadPageNumberDelegate(int ItemCount);

	public delegate void LoadDataDelegate(int PageNumber, int MaksData);

	public enum HistoryAction
	{
		Add,
		Edit,
		Delete,
		Insert
	}

	public static LoadDataDelegate AcceptorLoadData;

	public static void InitPageList(Page Page, LoadDataDelegate AcceptorLoadData)
	{
		int num = 1;
		if (Page.Session[MySession.CurrentPage] != null)
		{
			string text = Page.Session[MySession.CurrentPage].ToString();
			if (text == Path.GetFileName(Page.Request.Path) + Page.MasterPageFile && Page.Session[MySession.CurrentIndexPage + text] != null && num != -1)
			{
				num = int.Parse(Page.Session[MySession.CurrentIndexPage + text].ToString());
			}
		}
		InitPageList(Page, null, null, null, AcceptorLoadData, num);
	}

	public static void InitPageList(Page Page, LoadDataDelegate AcceptorLoadData, int PageNumber)
	{
		InitPageList(Page, null, null, null, AcceptorLoadData, PageNumber);
	}

	public static void InitPageList(Page Page, string[] KriteriaPencarian, DropDownList ddlKriteria, TextBox txtKataKunci, LoadDataDelegate AcceptorLoadData, int MaxItemPerPage)
	{
		InitPageList(Page, KriteriaPencarian, ddlKriteria, txtKataKunci, AcceptorLoadData,  true, MaxItemPerPage);
	}

	public static void InitPageList(Page Page, string[] KriteriaPencarian, DropDownList ddlKriteria, TextBox txtKataKunci, LoadDataDelegate AcceptorLoadData, bool IsPostBackCheck, int MaxItemPerPage)
	{
		int initPageNumber = 1;
		if (Page.Session[MySession.CurrentPage] != null)
		{
			string text = Page.Session[MySession.CurrentPage].ToString();
			if (text == Path.GetFileName(Page.Request.Path) + Page.MasterPageFile && Page.Session[MySession.CurrentIndexPage + text] != null && Page.Session[MySession.CurrentIndexPage + text].ToString() != "-1")
			{
				initPageNumber = int.Parse(Page.Session[MySession.CurrentIndexPage + text].ToString());
			}
		}
		InitPageList(Page, KriteriaPencarian, ddlKriteria, txtKataKunci, AcceptorLoadData, initPageNumber, IsPostBackCheck, MaxItemPerPage);
	}

	public static void InitPageList(Page Page, string[] KriteriaPencarian, DropDownList ddlKriteria, TextBox txtKataKunci, LoadDataDelegate AcceptorLoadData, int InitPageNumber, int MaxItemPerPage)
	{
		InitPageList(Page, KriteriaPencarian, ddlKriteria, txtKataKunci, AcceptorLoadData, InitPageNumber, true, MaxItemPerPage);
	}

	public static void InitPageList(Page Page, string[] KriteriaPencarian, DropDownList ddlKriteria, TextBox txtKataKunci, LoadDataDelegate AcceptorLoadData, int InitPageNumber, bool IsPostBackCheck, int MaxItemPerPage)
	{
		Connection.SetConnection();
		if (!Page.IsPostBack || !IsPostBackCheck)
		{
			int num = InitPageNumber;
			if (Page.Session[MySession.CurrentPage] == null)
			{
				Page.Response.Redirect(MyApplication.LoginPage);
				return;
			}
			string text = Page.Session[MySession.CurrentPage].ToString();
			if (text == Path.GetFileName(Page.Request.Path) + Page.MasterPageFile && Page.Session[MySession.CurrentIndexPage + text] != null && Page.Session[MySession.CurrentIndexPage + text].ToString() != "-1" && num != -1)
			{
				num = int.Parse(Page.Session[MySession.CurrentIndexPage + text].ToString());
			}
			if (KriteriaPencarian != null && ddlKriteria != null && txtKataKunci != null)
			{
				LoadKriteriaPencarian(KriteriaPencarian, ddlKriteria);
				if (Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KriteriaPencarian"] != null)
				{
					ddlKriteria.SelectedValue = Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KriteriaPencarian"].ToString();
				}
				if (Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KataKunci"] != null)
				{
					txtKataKunci.Text = Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KataKunci"].ToString();
				}
			}
			DataUIProvider.AcceptorLoadData = AcceptorLoadData;
			AcceptorLoadData(num, MaxItemPerPage);
		}
		ScriptManager.RegisterStartupScript(Page, Page.GetType(), "NoData", "if ($('#ContentPlaceHolder1_dgData tr').length > 1){$('#ContentPlaceHolder1_Exporter1').show();$('#ContentPlaceHolder1_divPaging').show();$('#ContentPlaceHolder1_divDataTidakTersedia').hide();}else{$('#ContentPlaceHolder1_Exporter1').hide();$('#ContentPlaceHolder1_divPaging').hide();$('#ContentPlaceHolder1_divDataTidakTersedia').show();}", addScriptTags: true);
	}

	public static void InitPageList(Page Page, string[] KriteriaPencarian, Repeater rptFilter, string ddlKriteriaName, string txtKataKunciName, LoadDataDelegate AcceptorLoadData, int InitPageNumber, bool IsPostBackCheck, int MaxItemPerPage)
	{
		Connection.SetConnection();
		if (!Page.IsPostBack || !IsPostBackCheck)
		{
			int num = InitPageNumber;
			if (Page.Session[MySession.CurrentPage] == null)
			{
				Page.Response.Redirect(MyApplication.LoginPage);
				return;
			}
			string text = Page.Session[MySession.CurrentPage].ToString();
			if (text == Path.GetFileName(Page.Request.Path) + Page.MasterPageFile && Page.Session[MySession.CurrentIndexPage + text] != null && Page.Session[MySession.CurrentIndexPage + text].ToString() != "-1" && num != -1)
			{
				num = int.Parse(Page.Session[MySession.CurrentIndexPage + text].ToString());
			}
			if (KriteriaPencarian != null && rptFilter.Items.Count > 0)
			{
				int num2 = 0;
				foreach (RepeaterItem item in rptFilter.Items)
				{
					DropDownList dropDownList = (DropDownList)item.FindControl(ddlKriteriaName);
					LoadKriteriaPencarian(KriteriaPencarian, dropDownList);
					TextBox textBox = (TextBox)item.FindControl(txtKataKunciName);
					if (Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KriteriaPencarian" + num2] != null)
					{
						dropDownList.SelectedValue = Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KriteriaPencarian" + num2].ToString();
					}
					if (Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KataKunci" + num2] != null)
					{
						textBox.Text = Page.Session[Page.Session[MySession.CurrentPage].ToString() + "KataKunci" + num2].ToString();
					}
					num2++;
				}
			}
			AcceptorLoadData(num, MaxItemPerPage);
		}
		ScriptManager.RegisterStartupScript(Page, Page.GetType(), "NoData", "if ($('#ContentPlaceHolder1_dgData tr').length > 1){$('#ContentPlaceHolder1_Exporter1').show();$('#ContentPlaceHolder1_divPaging').show();$('#ContentPlaceHolder1_divDataTidakTersedia').hide();}else{$('#ContentPlaceHolder1_Exporter1').hide();$('#ContentPlaceHolder1_divPaging').hide();$('#ContentPlaceHolder1_divDataTidakTersedia').show();}", addScriptTags: true);
	}

	public static void LoadOrderType(RadioButtonList rbl)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("OrderTypeID");
		dataTable.Columns.Add("OrderTypeName");
		DataRow dataRow = null;
		dataRow = dataTable.NewRow();
		dataRow[0] = "ASC";
		dataRow[1] = "Asc";
		dataTable.Rows.Add(dataRow);
		dataRow = dataTable.NewRow();
		dataRow[0] = "DESC";
		dataRow[1] = "Desc";
		dataTable.Rows.Add(dataRow);
		rbl.DataSource = dataTable;
		rbl.DataValueField = "OrderTypeID";
		rbl.DataTextField = "OrderTypeName";
		rbl.DataBind();
		rbl.SelectedIndex = 0;
	}

	public static void LoadOrderBy(DropDownList ddl, string[,] FieldList)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("OrderByID");
		dataTable.Columns.Add("OrderByName");
		DataRow dataRow = null;
		for (int i = 0; i <= FieldList.GetUpperBound(0); i++)
		{
			dataRow = dataTable.NewRow();
			dataRow[0] = FieldList[i, 0];
			dataRow[1] = FieldList[i, 1];
			dataTable.Rows.Add(dataRow);
		}
		ddl.DataSource = dataTable;
		ddl.DataValueField = "OrderByID";
		ddl.DataTextField = "OrderByName";
		ddl.DataBind();
	}

	public static void LoadKriteriaPencarian(string[] KriteriaPencarian, DropDownList ddlKriteria)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("KriteriaPencarianID");
		dataTable.Columns.Add("KriteriaPencarianName");
		DataRow dataRow = null;
		for (int i = 0; i < KriteriaPencarian.Length; i++)
		{
			dataRow = dataTable.NewRow();
			string text = KriteriaPencarian[i];
			string[] array = text.Split(char.Parse("|"));
			if (array.Length == 1)
			{
				dataRow[0] = array[0];
				dataRow[1] = array[0];
			}
			else
			{
				dataRow[0] = array[0];
				dataRow[1] = array[1];
			}
			dataTable.Rows.Add(dataRow);
		}
		ddlKriteria.DataSource = dataTable;
		ddlKriteria.DataValueField = "KriteriaPencarianID";
		ddlKriteria.DataTextField = "KriteriaPencarianName";
		ddlKriteria.DataBind();
	}

	public static DataTable LoadMonth()
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("MonthID");
		dataTable.Columns.Add("MonthName");
		DataRow dataRow = null;
		for (int i = 1; i <= 12; i++)
		{
			dataRow = dataTable.NewRow();
			dataRow[0] = i;
			switch (i)
			{
			case 1:
				dataRow[1] = "Januari";
				break;
			case 2:
				dataRow[1] = "Februari";
				break;
			case 3:
				dataRow[1] = "Maret";
				break;
			case 4:
				dataRow[1] = "April";
				break;
			case 5:
				dataRow[1] = "Mei";
				break;
			case 6:
				dataRow[1] = "Juni";
				break;
			case 7:
				dataRow[1] = "Juli";
				break;
			case 8:
				dataRow[1] = "Agustus";
				break;
			case 9:
				dataRow[1] = "September";
				break;
			case 10:
				dataRow[1] = "Oktober";
				break;
			case 11:
				dataRow[1] = "November";
				break;
			case 12:
				dataRow[1] = "Desember";
				break;
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public static void LoadMonth(DropDownList ddlMonth, bool IsWithHeader)
	{
		DataTable dataTable = LoadMonth();
		if (IsWithHeader)
		{
			DataRow dataRow = null;
			dataRow = dataTable.NewRow();
			dataRow[0] = -1;
			dataTable.Rows.InsertAt(dataRow, 0);
		}
		ddlMonth.DataValueField = "MonthID";
		ddlMonth.DataTextField = "MonthName";
		ddlMonth.DataSource = dataTable;
		ddlMonth.DataBind();
	}

	public static string MonthName(int MonthIndex)
	{
        switch (MonthIndex)
        {
            case 1:
                return "Januari";
                break;
                    
            case 2:
                return "Februari";
                break;
            case 3:
                return "Naret";
                break;
            case 4:
                return "April";
                break;
            case 5:
                return "Nei";
                break;
            case 6:
                return "Juni";
                break;
            case 7:
                return "Juli";
                break;
            case 8:
                return "Agustus";
                break;
            case 9:
                return "September";
                break;
            case 10:
                return "Oktobers";
                break;
            case 11:
                return "November";
                break;
            case 12:
                return "Desember";
                break;
            default:
                return "";
                break;
        }
	}

	public static void LoadYear(DropDownList ddlYear, bool IsWithHeader)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("YearID");
		dataTable.Columns.Add("YearName");
		DataRow dataRow = null;
		if (IsWithHeader)
		{
			dataRow = dataTable.NewRow();
			dataRow[0] = -1;
			dataTable.Rows.InsertAt(dataRow, 0);
		}
		int year = DateTime.Now.Year;
		for (int i = 2000; i <= year; i++)
		{
			dataRow = dataTable.NewRow();
			dataRow[0] = i.ToString();
			dataRow[1] = i.ToString();
			dataTable.Rows.InsertAt(dataRow, 0);
		}
		ddlYear.DataValueField = "YearID";
		ddlYear.DataTextField = "YearName";
		ddlYear.DataSource = dataTable;
		ddlYear.DataBind();
	}

	public static void LoadDropDown(DropDownList ddl, string SQL, string DataValueField, string DataTextField)
	{
		DataTable dataSource = Command.ExecDataAdapter(SQL, null);
		ddl.DataValueField = DataValueField;
		ddl.DataTextField = DataTextField;
		ddl.DataSource = dataSource;
		ddl.DataBind();
	}

	public static void LoadDropDownWithEmpty(DropDownList ddl, string SQL, string DataValueField, string DataTextField)
	{
		DataTable dataTable = Command.ExecDataAdapter(SQL, null);
		DataRow dataRow = dataTable.NewRow();
		dataRow[0] = "0";
		dataRow[1] = "--Belum ditentukan--";
		dataTable.Rows.InsertAt(dataRow, 0);
		ddl.DataValueField = DataValueField;
		ddl.DataTextField = DataTextField;
		ddl.DataSource = dataTable;
		ddl.DataBind();
	}

	public static void LoadDropDownWithAll(DropDownList ddl, string SQL, string DataValueField, string DataTextField, string SemuaText = "--Semua--")
	{
		DataTable dataTable = Command.ExecDataAdapter(SQL, null);
		DataRow dataRow = dataTable.NewRow();
		dataRow[0] = "0";
		dataRow[1] = SemuaText;
		dataTable.Rows.InsertAt(dataRow, 0);
		ddl.DataValueField = DataValueField;
		ddl.DataTextField = DataTextField;
		ddl.DataSource = dataTable;
		ddl.DataBind();
	}

	public static void LoadNomorUrut(DropDownList ddl, int StartIndex, int EndIndex)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("NoUrutID");
		dataTable.Columns.Add("NoUrutName");
		DataRow dataRow = null;
		for (int i = StartIndex; i <= EndIndex; i++)
		{
			dataRow = dataTable.NewRow();
			dataRow[0] = i.ToString();
			dataRow[1] = i.ToString();
			dataTable.Rows.Add(dataRow);
		}
		ddl.DataValueField = "NoUrutID";
		ddl.DataTextField = "NoUrutName";
		ddl.DataSource = dataTable;
		ddl.DataBind();
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, null, true, null);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, bool IsBind)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, null, IsBind, null);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, TwoArrayList tar, bool IsBind)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, IsBind, null);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string ShowingFields, bool IsBind, DelegateToggleBind ToggleBind)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, "", ShowingFields, null, IsBind, ToggleBind);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, bool IsBind, DelegateToggleBind ToggleBind)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, null, IsBind, ToggleBind);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, string FieldPencarian, string KataKunci, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, TwoArrayList tar, bool IsBind, DelegateToggleBind ToggleBind)
	{
        if (ToggleBind != null)
        {
            ToggleBind.Invoke(true);
        }
		if (MaxItemPerPage > 0)
		{
			Page.Session.Add("MaxItemPerPage", MaxItemPerPage);
		}
		else
		{
			PageNumber = -1;
		}
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
		if (FieldPencarian == "TahunUpload")
		{
			FieldPencarian = "";
			KataKunci = "";
			OrderFields = "Catalogs.ID";
		}
		Connection.SetConnection();
		SQLAND = (string.IsNullOrEmpty(SQLAND) ? "" : (" " + SQLAND));
		string text3 = (string.IsNullOrEmpty(FieldPencarian) ? "" : FieldPencarian.Replace(".", ""));
		if (text3.Contains("(") || text3.Contains(")") || text3.Contains("=") || text3.Contains(" ") || text3.Length > 20)
		{
			text3 = text3.Substring(0, 20).Replace("(", "").Replace(")", "")
				.Replace("=", "")
				.Replace(" ", "")
				.Replace("'", "")
				.Replace(",", "");
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
						break;
					}
					if ((!FieldPencarian.ToUpper().Contains("ID") && !FieldPencarian.ToUpper().Contains("DATE") && !FieldPencarian.ToUpper().Contains("TANGGAL") && FieldPencarian.ToUpper() != "BIBID" && FieldPencarian.ToUpper() != "CONTROLNUMBER" && FieldPencarian.ToUpper() != "PUBLISHYEAR" && FieldPencarian.ToUpper() != "ISBN" && FieldPencarian.ToUpper() != "NOMORBARCODE" && FieldPencarian.ToUpper() != "MEMBERNO" && FieldPencarian.ToUpper() != "CURRENCY" && FieldPencarian.ToUpper() != "ISOPAC" && FieldPencarian.ToUpper() != "ISWORLDCATSENDED" && FieldPencarian.ToUpper() != "ISRDA" && FieldPencarian.ToUpper() != "WORKSHEET_ID" && !FieldPencarian.ToUpper().Contains("JUMLAH") && !FieldPencarian.ToUpper().Contains("EXISTS") && !FieldPencarian.ToUpper().Contains("UPPER")) || FieldPencarian.ToUpper() == "NOMORPANGGILJILID")
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND UPPER(" + FieldPencarian + ") LIKE " + Connection.ParameterSymbol + text3;
						break;
					}
					if (FieldPencarian.ToUpper() == "ISOPAC" || FieldPencarian.ToUpper() == "ISWORLDCATSENDED" || FieldPencarian.ToUpper() == "ISRDA" || FieldPencarian.ToUpper() == "WORKSHEET_ID" || FieldPencarian.ToUpper().Contains("JUMLAH") || (FieldPencarian.ToUpper().Contains("DATE") && !FieldPencarian.ToUpper().Contains("UPDATEBY")) || FieldPencarian.ToUpper().Contains("TANGGAL"))
					{
						if (FieldPencarian.ToUpper() == "TANGGALPEMBAHASAN")
						{
							string text4 = SQLAND;
							SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
						}
						else
						{
							string text4 = SQLAND;
							SQLAND = text4 + " AND " + FieldPencarian + " = " + Connection.ParameterSymbol + text3;
						}
					}
					else
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
					}
					if (FieldPencarian.ToUpper().Contains("EXISTS"))
					{
						SQLAND += ")";
					}
					break;
				case Connection.EServerType.MySQL:
				{
					KataKunci = KataKunci.Trim().Replace(":", "%").Replace("=", "%")
						.Replace("/", "%")
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
						tar.Add(text3, KataKunci.Trim().ToUpper());
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "1")
					{
						tar.Add(text3, KataKunci.Trim().ToUpper() + "%");
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "2")
					{
						tar.Add(text3, "%" + KataKunci.Trim().ToUpper());
					}
					else
					{
						tar.Add(text3, "%" + KataKunci.Trim().ToUpper() + "%");
					}
				}
				else
				{
					tar.Add(text3, "%" + KataKunci.Trim().ToUpper() + "%");
				}
			}
		}
		else if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				tar.SetItem2(i, tar.Item2(i).ToString().Trim()
					.Replace(":", "%")
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
				if (!MyApplication.IsCaseSencitive)
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
				}
				else if ((!tar.Item1(i).ToString().ToUpper()
					.Contains("ID") && !tar.Item1(i).ToString().ToUpper()
					.Contains("DATE") && !tar.Item1(i).ToString().ToUpper()
					.Contains("TANGGAL") && tar.Item1(i).ToString().ToUpper() != "BIBID" && tar.Item1(i).ToString().ToUpper() != "CONTROLNUMBER" && tar.Item1(i).ToString().ToUpper() != "PUBLISHYEAR" && tar.Item1(i).ToString().ToUpper() != "ISBN" && tar.Item1(i).ToString().ToUpper() != "NOMORBARCODE" && tar.Item1(i).ToString().ToUpper() != "MEMBERNO" && tar.Item1(i).ToString().ToUpper() != "CURRENCY" && tar.Item1(i).ToString().ToUpper() != "ISOPAC" && tar.Item1(i).ToString().ToUpper() != "ISWORLDCATSENDED" && tar.Item1(i).ToString().ToUpper() != "ISRDA" && tar.Item1(i).ToString().ToUpper() != "WORKSHEET_ID" && !tar.Item1(i).ToString().ToUpper()
					.Contains("JUMLAH") && !tar.Item1(i).ToString().ToUpper()
					.Contains("EXISTS") && !tar.Item1(i).ToString().ToUpper()
					.Contains("UPPER")) || tar.Item1(i).ToString().ToUpper() == "NOMORPANGGILJILID")
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND UPPER(", tar.Item1(i), ") LIKE ", Connection.ParameterSymbol, "Param", i);
					tar.SetItem2(i, tar.Item2(i).ToString().ToUpper());
				}
				else
				{
					if (tar.Item1(i).ToString().ToUpper() == "ISOPAC" || tar.Item1(i).ToString().ToUpper() == "ISWORLDCATSENDED" || tar.Item1(i).ToString().ToUpper() == "ISRDA" || tar.Item1(i).ToString().ToUpper() == "WORKSHEET_ID" || tar.Item1(i).ToString().ToUpper()
						.Contains("JUMLAH") || (tar.Item1(i).ToString().ToUpper()
						.Contains("DATE") && !tar.Item1(i).ToString().ToUpper()
						.Contains("UPDATEBY")) || tar.Item1(i).ToString().ToUpper()
						.Contains("TANGGAL"))
					{
						if (FieldPencarian.ToUpper() == "TANGGALPEMBAHASAN")
						{
							object obj = SQLAND;
							SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
						}
						else
						{
							object obj = SQLAND;
							SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " = ", Connection.ParameterSymbol, "Param", i);
						}
					}
					else
					{
						object obj = SQLAND;
						SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
					}
					if (tar.Item1(i).ToString().ToUpper()
						.Contains("EXISTS"))
					{
						SQLAND += ")";
					}
				}
				tar.SetItem1(i, "Param" + i);
			}
		}
		SQLAND = SQLAND.Replace("AND AND", "AND").Replace("OR AND", "OR");
		string text5 = "";
		int num = 0;
		try
		{
			text5 = "SELECT COUNT(*) FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			if (ShowingFields.ToUpper().StartsWith("SELECT * FROM (SELECT"))
			{
				text5 = "SELECT COUNT(*) FROM (" + ShowingFields + " FROM " + TableName + ")) " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			}
			if (!string.IsNullOrEmpty(SQLGROUPBY))
			{
				text5 = "SELECT COUNT(*) FROM (" + text5 + SQLGROUPBY + ")";
			}
			num = int.Parse(Command.ExecScalar(tar, text5, "0"));
		}
		catch (Exception ex)
		{
			Util.ShowAlertMessage(ex.Message);
			return default(LoadDataResult);
		}
		string text6 = "";
		if (MyApplication.IsCaseSencitive && Connection.ServerType == Connection.EServerType.Oracle)
		{
			string text7 = "";
			text7 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? Page.Session[text + MySession.CurrentSortExp].ToString() : OrderFields);
			if (!string.IsNullOrEmpty(text7))
			{
				string text8 = "";
				if (text7.ToUpper().Contains("CASE"))
				{
					text8 = text7;
				}
				else if (text7.ToUpper().StartsWith("EXISTS") && text7.ToUpper().EndsWith("UPPER(VALUE)"))
				{
					text8 = "UPPER(VALUE)";
				}
				if (string.IsNullOrEmpty(text8))
				{
					string[] array = text7.Trim().Split(char.Parse(","));
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Trim().Split(char.Parse(" "));
						if (array2[0].ToUpper().Contains("ID") || array2[0].ToUpper().Contains("DATE") || array2[0].ToUpper().Contains("TANGGAL") || array2[0].ToUpper() == "BIBID" || array2[0].ToUpper() == "PUBLISHYEAR" || array2[0].ToUpper() == "ISBN" || array2[0].ToUpper() == "NOMORBARCODE" || array2[0].ToUpper() == "MEMBERNO" || array2[0].ToUpper() == "CURRENCY" || array2[0].ToUpper() == "ISOPAC" || array2[0].ToUpper() == "ISWORLDCATSENDED" || array2[0].ToUpper() == "ISRDA" || array2[0].ToUpper() == "WORKSHEET_ID" || array2[0].ToUpper().Contains("JUMLAH") || array2[0].ToUpper().Contains("EXISTS") || array2[0].ToUpper().Contains("UPPER") || array2[0].ToUpper() == "STARTPOSITION" || array2[0].ToUpper() == "SORTNO" || array2[0].ToUpper() == "CODE" || array2[0].ToUpper() == "LASTSUCCESS")
						{
							text8 = text8 + array[i] + ",";
						}
						else if (i != array.Length - 1)
						{
							if (array2.Length > 1)
							{
								string text9 = "";
								for (int j = 1; j < array2.Length; j++)
								{
									text9 = text9 + " " + array2[j];
								}
								string text4 = text8;
								text8 = text4 + "UPPER(" + array2[0] + ")" + text9 + ",";
							}
							else
							{
								text8 = text8 + "UPPER(" + array2[0] + "),";
							}
						}
						else if (array2.Length > 1)
						{
							string text9 = "";
							for (int j = 1; j < array2.Length; j++)
							{
								text9 = text9 + " " + array2[j];
							}
							string text4 = text8;
							text8 = text4 + "UPPER(" + array2[0] + ") " + text9;
						}
						else
						{
							text8 = text8 + "UPPER(" + array2[0] + ")";
						}
					}
				}
				text6 = " ORDER BY " + text8.TrimEnd(char.Parse(","));
			}
		}
		Connection.EServerType serverType = Connection.ServerType;
		if (serverType == Connection.EServerType.MySQL)
		{
			text6 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? (" ORDER BY " + Page.Session[text + MySession.CurrentSortExp].ToString()) : (" ORDER BY " + OrderFields));
		}
		string text10 = "";
		string sQL = "";
		if (PageNumber != -1)
		{
			if (PageNumber <= 0)
			{
				PageNumber = 1;
				Page.Session.Add(MySession.CurrentIndexPage, 1);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
			int num2 = (int)Math.Ceiling((double)num / (double)MaxItemPerPage);
			if (PageNumber > num2)
			{
				PageNumber = 1;
				Page.Session.Add(MySession.CurrentIndexPage, 1);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				sQL = "SELECT * FROM (SELECT a.*,ROWNUM as No  FROM (SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + "  WHERE 1=1 " + SQLAND + " " + SQLGROUPBY + " " + text6 + ") a ) WHERE No >= " + ((PageNumber - 1) * MaxItemPerPage + 1) + " AND ROWNUM <= " + MaxItemPerPage;
				if (ShowingFields.ToUpper().StartsWith("SELECT * FROM (SELECT"))
				{
					sQL = "SELECT * FROM (SELECT a.*,ROWNUM as No  FROM ( " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + ")  WHERE 1=1 " + SQLAND + " " + SQLGROUPBY + " " + text6 + ") a ) WHERE No >= " + ((PageNumber - 1) * MaxItemPerPage + 1) + " AND ROWNUM <= " + MaxItemPerPage;
				}
				break;
			case Connection.EServerType.MySQL:
				text10 = " LIMIT " + (PageNumber - 1) * MaxItemPerPage + "," + MaxItemPerPage;
				sQL = "SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND + text6 + text10;
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
			if (dataTable.Columns.Count <= 0)
			{
				break;
			}
			if (PageNumber != -1)
			{
				dataTable.Columns["No"].SetOrdinal(0);
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxItemPerPage + (i + 1);
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
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxItemPerPage + (i + 1);
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
		if (IsBind)
		{
			dgData.DataSource = dataTable;
			dgData.DataBind();
		}
		LoadDataResult result = default(LoadDataResult);
		result.Data = dataTable;
		result.ItemCount = num;
		return result;
	}

	public static int LoadDataGetCount(Page Page, string TableName, string FieldPencarian, string KataKunci, string SQLJOIN, string SQLAND, string SQLGROUPBY, TwoArrayList tar)
	{
		string text = "";
		if (Page.Session[MySession.CurrentPage] != null)
		{
			text = Page.Session[MySession.CurrentPage].ToString();
		}
		else
		{
			Page.Session.Add(MySession.CurrentPage, Path.GetFileName(Page.Request.Path));
			text = Page.Session[MySession.CurrentPage].ToString();
		}
		string text2 = Page.Session[MySession.CurrentPage].ToString();
		Page.Session.Add(text2 + "KriteriaPencarian", FieldPencarian);
		Page.Session.Add(text2 + "KataKunci", KataKunci);
		Connection.SetConnection();
		SQLAND = (string.IsNullOrEmpty(SQLAND) ? "" : (" " + SQLAND));
		string text3 = (string.IsNullOrEmpty(FieldPencarian) ? "" : FieldPencarian.Replace(".", ""));
		if (text3.Contains("(") || text3.Contains(")") || text3.Contains("=") || text3.Contains(" ") || text3.Length > 20)
		{
			text3 = text3.Substring(0, 20).Replace("(", "").Replace(")", "")
				.Replace("=", "")
				.Replace(" ", "");
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
						break;
					}
					if ((!FieldPencarian.ToUpper().Contains("ID") && !FieldPencarian.ToUpper().Contains("DATE") && !FieldPencarian.ToUpper().Contains("TANGGAL") && FieldPencarian.ToUpper() != "BIBID" && FieldPencarian.ToUpper() != "CONTROLNUMBER" && FieldPencarian.ToUpper() != "PUBLISHYEAR" && FieldPencarian.ToUpper() != "ISBN" && FieldPencarian.ToUpper() != "NOMORBARCODE" && FieldPencarian.ToUpper() != "MEMBERNO" && FieldPencarian.ToUpper() != "CURRENCY" && FieldPencarian.ToUpper() != "ISOPAC" && FieldPencarian.ToUpper() != "ISWORLDCATSENDED" && FieldPencarian.ToUpper() != "ISRDA" && FieldPencarian.ToUpper() != "WORKSHEET_ID" && !FieldPencarian.ToUpper().Contains("JUMLAH") && !FieldPencarian.ToUpper().Contains("EXISTS") && !FieldPencarian.ToUpper().Contains("UPPER")) || FieldPencarian.ToUpper() == "NOMORPANGGILJILID" || FieldPencarian.ToUpper() == "VALIDATEBY")
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND UPPER(" + FieldPencarian + ") LIKE " + Connection.ParameterSymbol + text3;
						break;
					}
					if (FieldPencarian.ToUpper() == "ISOPAC" || FieldPencarian.ToUpper() == "ISWORLDCATSENDED" || FieldPencarian.ToUpper() == "ISRDA" || FieldPencarian.ToUpper() == "WORKSHEET_ID" || FieldPencarian.ToUpper().Contains("JUMLAH") || (FieldPencarian.ToUpper().Contains("DATE") && !FieldPencarian.ToUpper().Contains("UPDATEBY")) || FieldPencarian.ToUpper().Contains("TANGGAL"))
					{
						if (FieldPencarian.ToUpper() == "TANGGALPEMBAHASAN")
						{
							string text4 = SQLAND;
							SQLAND = text4 + " AND " + FieldPencarian + " = " + Connection.ParameterSymbol + text3;
						}
						else
						{
							string text4 = SQLAND;
							SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
						}
					}
					else
					{
						string text4 = SQLAND;
						SQLAND = text4 + " AND " + FieldPencarian + " LIKE " + Connection.ParameterSymbol + text3;
					}
					if (FieldPencarian.ToUpper().Contains("EXISTS"))
					{
						SQLAND += ")";
					}
					break;
				case Connection.EServerType.MySQL:
				{
					KataKunci = KataKunci.Trim().Replace(":", "%").Replace("=", "%")
						.Replace("/", "%")
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
						tar.Add(text3, KataKunci.Trim().ToUpper());
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "1")
					{
						tar.Add(text3, KataKunci.Trim().ToUpper() + "%");
					}
					else if (Page.Session[text + "TipePencarian"].ToString() == "2")
					{
						tar.Add(text3, "%" + KataKunci.Trim().ToUpper());
					}
					else
					{
						tar.Add(text3, "%" + KataKunci.Trim().ToUpper() + "%");
					}
				}
				else
				{
					tar.Add(text3, "%" + KataKunci.Trim().ToUpper() + "%");
				}
			}
		}
		else if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				tar.SetItem2(i, tar.Item2(i).ToString().Trim()
					.Replace(":", "%")
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
				if (!MyApplication.IsCaseSencitive)
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
				}
				else if ((!tar.Item1(i).ToString().ToUpper()
					.Contains("ID") && !tar.Item1(i).ToString().ToUpper()
					.Contains("DATE") && !tar.Item1(i).ToString().ToUpper()
					.Contains("TANGGAL") && tar.Item1(i).ToString().ToUpper() != "BIBID" && tar.Item1(i).ToString().ToUpper() != "CONTROLNUMBER" && tar.Item1(i).ToString().ToUpper() != "PUBLISHYEAR" && tar.Item1(i).ToString().ToUpper() != "ISBN" && tar.Item1(i).ToString().ToUpper() != "NOMORBARCODE" && tar.Item1(i).ToString().ToUpper() != "MEMBERNO" && tar.Item1(i).ToString().ToUpper() != "CURRENCY" && tar.Item1(i).ToString().ToUpper() != "ISOPAC" && tar.Item1(i).ToString().ToUpper() != "ISWORLDCATSENDED" && tar.Item1(i).ToString().ToUpper() != "ISRDA" && tar.Item1(i).ToString().ToUpper() != "WORKSHEET_ID" && !tar.Item1(i).ToString().ToUpper()
					.Contains("JUMLAH") && !tar.Item1(i).ToString().ToUpper()
					.Contains("EXISTS") && !tar.Item1(i).ToString().ToUpper()
					.Contains("UPPER")) || tar.Item1(i).ToString().ToUpper() == "NOMORPANGGILJILID" || tar.Item1(i).ToString().ToUpper() == "VALIDATEBY")
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND UPPER(", tar.Item1(i), ") LIKE ", Connection.ParameterSymbol, "Param", i);
					tar.SetItem2(i, tar.Item2(i).ToString().ToUpper());
				}
				else
				{
					if (tar.Item1(i).ToString().ToUpper() == "ISOPAC" || tar.Item1(i).ToString().ToUpper() == "ISWORLDCATSENDED" || tar.Item1(i).ToString().ToUpper() == "ISRDA" || tar.Item1(i).ToString().ToUpper() == "WORKSHEET_ID" || tar.Item1(i).ToString().ToUpper()
						.Contains("JUMLAH") || (tar.Item1(i).ToString().ToUpper()
						.Contains("DATE") && !tar.Item1(i).ToString().ToUpper()
						.Contains("UPDATEBY")) || tar.Item1(i).ToString().ToUpper()
						.Contains("TANGGAL"))
					{
						if (FieldPencarian.ToUpper() == "TANGGALPEMBAHASAN")
						{
							object obj = SQLAND;
							SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " = ", Connection.ParameterSymbol, "Param", i);
						}
						else
						{
							object obj = SQLAND;
							SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
						}
					}
					else
					{
						object obj = SQLAND;
						SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
					}
					if (tar.Item1(i).ToString().ToUpper()
						.Contains("EXISTS"))
					{
						SQLAND += ")";
					}
				}
				tar.SetItem1(i, "Param" + i);
			}
		}
		SQLAND = SQLAND.Replace("AND AND", "AND").Replace("OR AND", "OR");
		string text5 = "";
		int num = 0;
		try
		{
			text5 = "SELECT COUNT(*) FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			if (!string.IsNullOrEmpty(SQLGROUPBY))
			{
				text5 = "SELECT COUNT(*) FROM (" + text5 + SQLGROUPBY + ")";
			}
			return int.Parse(Command.ExecScalar(tar, text5, "0"));
		}
		catch (Exception ex)
		{
			Util.ShowAlertMessage(ex.Message);
			return 0;
		}
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, TwoArrayList FilterPencarian, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, TwoArrayList tar, bool IsBind)
	{
		return LoadData(PageNumber, Page, dgData, TableName, FilterPencarian, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, IsBind, null);
	}

	public static LoadDataResult LoadData(int PageNumber, Page Page, DataGrid dgData, string TableName, TwoArrayList FilterPencarian, string OrderFields, int MaxItemPerPage, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, TwoArrayList tar, bool IsBind, DelegateToggleBind ToggleBind)
	{
        if (ToggleBind != null)
        {
            ToggleBind.Invoke(true);
        }
		if (MaxItemPerPage > 0)
		{
			Page.Session.Add("MaxItemPerPage", MaxItemPerPage);
		}
		else
		{
			PageNumber = -1;
		}
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
		Connection.SetConnection();
		SQLAND = (string.IsNullOrEmpty(SQLAND) ? "" : (" " + SQLAND));
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		string text2 = Page.Session[MySession.CurrentPage].ToString();
		for (int i = 0; i < FilterPencarian.Count(); i++)
		{
			string text3 = FilterPencarian.Item1(i).Trim();
			string value = FilterPencarian.Item2(i).ToString().Trim();
			Page.Session.Add(text2 + "KriteriaPencarian" + i, text3);
			Page.Session.Add(text2 + "KataKunci" + i, value);
			string text4 = (string.IsNullOrEmpty(text3) ? "" : text3.Replace(".", ""));
			if (text4.Contains("(") || text4.Contains(")") || text4.Contains("=") || text4.Contains(" ") || text4.Length > 20)
			{
				text4 = "Param" + i;
				arrayList.Add(text4);
			}
			else
			{
				arrayList.Add(text4);
			}
			arrayList2.Add(value);
		}
		if (tar == null)
		{
			tar = new TwoArrayList();
			if (arrayList2.Count > 0)
			{
				for (int i = 0; i < FilterPencarian.Count(); i++)
				{
					string text3 = FilterPencarian.Item1(i).Trim();
					string value = FilterPencarian.Item2(i).ToString().Trim();
					string text4 = arrayList[i].ToString();
					if (string.IsNullOrEmpty(text3) || string.IsNullOrEmpty(value) || text3.EndsWith("!"))
					{
						continue;
					}
					switch (Connection.ServerType)
					{
					case Connection.EServerType.Oracle:
						if (!MyApplication.IsCaseSencitive)
						{
							string text5 = SQLAND;
							SQLAND = text5 + " AND " + text3 + " LIKE " + Connection.ParameterSymbol + text4;
							break;
						}
						if ((!text3.ToUpper().Contains("ID") && !text3.ToUpper().Contains("DATE") && text3.ToUpper() != "BIBID" && text3.ToUpper() != "CONTROLNUMBER" && text3.ToUpper() != "PUBLISHYEAR" && text3.ToUpper() != "ISBN" && text3.ToUpper() != "NOMORBARCODE" && text3.ToUpper() != "MEMBERNO" && text3.ToUpper() != "TANGGALKIRIM" && text3.ToUpper() != "CURRENCY" && text3.ToUpper() != "ISOPAC" && text3.ToUpper() != "ISWORLDCATSENDED" && text3.ToUpper() != "ISRDA" && text3.ToUpper() != "WORKSHEET_ID" && !text3.ToUpper().Contains("JUMLAH") && !text3.ToUpper().Contains("EXISTS") && !text3.ToUpper().Contains("UPPER")) || text3.ToUpper().Contains("CREATEBY") || text3.ToUpper().Contains("UPDATEBY"))
						{
							string text5 = SQLAND;
							SQLAND = text5 + " AND UPPER(" + text3 + ") LIKE " + Connection.ParameterSymbol + text4;
							break;
						}
						if (text3.ToUpper() == "ISOPAC" || text3.ToUpper() == "ISWORLDCATSENDED" || text3.ToUpper() == "ISRDA" || text3.ToUpper() == "WORKSHEET_ID" || text3.ToUpper().Contains("JUMLAH") || text3.ToUpper().Contains("DATE"))
						{
							string text5 = SQLAND;
							SQLAND = text5 + " AND " + text3 + " = " + Connection.ParameterSymbol + text4;
						}
						else
						{
							string text5 = SQLAND;
							SQLAND = text5 + " AND " + text3 + " LIKE " + Connection.ParameterSymbol + text4;
						}
						if (text3.ToUpper().Contains("EXISTS"))
						{
							SQLAND += ")";
						}
						break;
					case Connection.EServerType.MySQL:
					{
						value = value.Trim().Replace(":", "%").Replace("=", "%")
							.Replace("/", "%")
							.Replace("\t", "%");
						string text5 = SQLAND;
						SQLAND = text5 + " AND " + text3 + " LIKE " + Connection.ParameterSymbol + text4;
						break;
					}
					}
					if (Page.Session[text + "TipePencarian" + i] != null)
					{
						if (Page.Session[text + "TipePencarian" + i].ToString() == "0")
						{
							tar.Add(text4, value.Trim().ToUpper());
						}
						else if (Page.Session[text + "TipePencarian" + i].ToString() == "1")
						{
							tar.Add(text4, value.Trim().ToUpper() + "%");
						}
						else if (Page.Session[text + "TipePencarian" + i].ToString() == "2")
						{
							tar.Add(text4, "%" + value.Trim().ToUpper());
						}
						else
						{
							tar.Add(text4, "%" + value.Trim().ToUpper() + "%");
						}
					}
					else
					{
						tar.Add(text4, "%" + value.Trim().ToUpper() + "%");
					}
				}
			}
		}
		else if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				tar.SetItem2(i, tar.Item2(i).ToString().Trim()
					.Replace(":", "%")
					.Replace("=", "%")
					.Replace("/", "%")
					.Replace("\t", "%"));
				if (tar.Item1(i).Contains("(") || tar.Item1(i).Contains(")") || tar.Item1(i).Contains("=") || tar.Item1(i).Contains(" "))
				{
					tar.SetItem1(i, "Param" + i);
				}
				string text5 = SQLAND;
				SQLAND = text5 + " AND " + tar.Item1(i) + " LIKE " + Connection.ParameterSymbol + tar.Item1(i);
			}
		}
		else
		{
			for (int i = 0; i < tar.Count(); i++)
			{
				if (!MyApplication.IsCaseSencitive)
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
				}
				else if ((!tar.Item1(i).ToString().ToUpper()
					.Contains("ID") && !tar.Item1(i).ToString().ToUpper()
					.Contains("DATE") && tar.Item1(i).ToString().ToUpper() != "BIBID" && tar.Item1(i).ToString().ToUpper() != "CONTROLNUMBER" && tar.Item1(i).ToString().ToUpper() != "PUBLISHYEAR" && tar.Item1(i).ToString().ToUpper() != "ISBN" && tar.Item1(i).ToString().ToUpper() != "NOMORBARCODE" && tar.Item1(i).ToString().ToUpper() != "MEMBERNO" && tar.Item1(i).ToString().ToUpper() != "TANGGALKIRIM" && tar.Item1(i).ToString().ToUpper() != "CURRENCY" && tar.Item1(i).ToString().ToUpper() != "ISOPAC" && tar.Item1(i).ToString().ToUpper() != "ISWORLDCATSENDED" && tar.Item1(i).ToString().ToUpper() != "ISRDA" && tar.Item1(i).ToString().ToUpper() != "WORKSHEET_ID" && !tar.Item1(i).ToString().ToUpper()
					.Contains("JUMLAH") && !tar.Item1(i).ToString().ToUpper()
					.Contains("EXISTS") && !tar.Item1(i).ToString().ToUpper()
					.Contains("UPPER")) || tar.Item1(i).ToString().ToUpper()
					.Contains("CREATEBY") || tar.Item1(i).ToString().ToUpper()
					.Contains("UPDATEBY"))
				{
					object obj = SQLAND;
					SQLAND = string.Concat(obj, " AND UPPER(", tar.Item1(i), ") LIKE ", Connection.ParameterSymbol, "Param", i);
					tar.SetItem2(i, tar.Item2(i).ToString().ToUpper());
				}
				else
				{
					if (tar.Item1(i).ToString().ToUpper() == "ISOPAC" || tar.Item1(i).ToString().ToUpper() == "ISWORLDCATSENDED" || tar.Item1(i).ToString().ToUpper() == "ISRDA" || tar.Item1(i).ToString().ToUpper() == "WORKSHEET_ID" || tar.Item1(i).ToString().ToUpper()
						.Contains("JUMLAH") || tar.Item1(i).ToString().ToUpper()
						.Contains("DATE"))
					{
						object obj = SQLAND;
						SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " = ", Connection.ParameterSymbol, "Param", i);
					}
					else
					{
						object obj = SQLAND;
						SQLAND = string.Concat(obj, " AND ", tar.Item1(i), " LIKE ", Connection.ParameterSymbol, "Param", i);
					}
					if (tar.Item1(i).ToString().ToUpper()
						.Contains("EXISTS"))
					{
						SQLAND += ")";
					}
				}
				tar.SetItem1(i, "Param" + i);
			}
		}
		SQLAND = SQLAND.Replace("AND AND", "AND").Replace("OR AND", "OR");
		string text6 = "";
		int num = 0;
		try
		{
			text6 = "SELECT COUNT(*) FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			if (ShowingFields.ToUpper().StartsWith("SELECT * FROM (SELECT"))
			{
				text6 = "SELECT COUNT(*) FROM (" + ShowingFields + " FROM " + TableName + ")) " + SQLJOIN + " WHERE 1=1 " + SQLAND;
			}
			if (!string.IsNullOrEmpty(SQLGROUPBY))
			{
				text6 = "SELECT COUNT(*) FROM (" + text6 + SQLGROUPBY + ")";
			}
			num = int.Parse(Command.ExecScalar(tar, text6, "0"));
		}
		catch (Exception ex)
		{
			Util.ShowAlertMessage(ex.Message);
			return default(LoadDataResult);
		}
		string text7 = "";
		if (MyApplication.IsCaseSencitive && Connection.ServerType == Connection.EServerType.Oracle)
		{
			string text8 = "";
			text8 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? Page.Session[text + MySession.CurrentSortExp].ToString() : OrderFields);
			string text9 = "";
			if (text8.ToUpper().Contains("CASE"))
			{
				text9 = text8;
			}
			if (string.IsNullOrEmpty(text9))
			{
				string[] array = text8.Trim().Split(char.Parse(","));
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Trim().Split(char.Parse(" "));
					if (array2[0].ToUpper().Contains("ID") || array2[0].ToUpper().Contains("DATE") || array2[0].ToUpper() == "BIBID" || array2[0].ToUpper() == "PUBLISHYEAR" || array2[0].ToUpper() == "ISBN" || array2[0].ToUpper() == "NOMORBARCODE" || array2[0].ToUpper() == "MEMBERNO" || array2[0].ToUpper() == "TANGGALKIRIM" || array2[0].ToUpper() == "CURRENCY" || array2[0].ToUpper() == "ISOPAC" || array2[0].ToUpper() == "ISWORLDCATSENDED" || array2[0].ToUpper() == "ISRDA" || array2[0].ToUpper() == "WORKSHEET_ID" || array2[0].ToUpper().Contains("JUMLAH") || array2[0].ToUpper().Contains("EXISTS") || array2[0].ToUpper().Contains("UPPER"))
					{
						text9 = text9 + array[i] + ",";
					}
					else if (i != array.Length - 1)
					{
						if (array2.Length > 1)
						{
							string text5 = text9;
							text9 = text5 + "UPPER(" + array2[0] + ") " + array2[1] + ",";
						}
						else
						{
							text9 = text9 + "UPPER(" + array2[0] + "),";
						}
					}
					else if (array2.Length > 1)
					{
						string text5 = text9;
						text9 = text5 + "UPPER(" + array2[0] + ") " + array2[1];
					}
					else
					{
						text9 = text9 + "UPPER(" + array2[0] + ")";
					}
				}
			}
			text7 = " ORDER BY " + text9.TrimEnd(char.Parse(","));
		}
		Connection.EServerType serverType = Connection.ServerType;
		if (serverType == Connection.EServerType.MySQL)
		{
			text7 = ((Page.Session[text + MySession.CurrentSortExp] != null) ? (" ORDER BY " + Page.Session[text + MySession.CurrentSortExp].ToString()) : (" ORDER BY " + OrderFields));
		}
		string text10 = "";
		string sQL = "";
		if (PageNumber != -1)
		{
			if (PageNumber <= 0)
			{
				PageNumber = 1;
				Page.Session.Add(MySession.CurrentIndexPage, 1);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
			int num2 = (int)Math.Ceiling((double)num / (double)MaxItemPerPage);
			if (PageNumber > num2)
			{
				PageNumber = 1;
				Page.Session.Add(MySession.CurrentIndexPage, 1);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				sQL = "SELECT * FROM (SELECT a.*,ROWNUM as No  FROM (SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + "  WHERE 1=1 " + SQLAND + " " + SQLGROUPBY + " " + text7 + ") a ) WHERE No >= " + ((PageNumber - 1) * MaxItemPerPage + 1) + " AND ROWNUM <= " + MaxItemPerPage;
				if (ShowingFields.ToUpper().StartsWith("SELECT * FROM (SELECT"))
				{
					sQL = "SELECT * FROM (SELECT a.*,ROWNUM as No  FROM ( " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + ")  WHERE 1=1 " + SQLAND + " " + SQLGROUPBY + " " + text7 + ") a ) WHERE No >= " + ((PageNumber - 1) * MaxItemPerPage + 1) + " AND ROWNUM <= " + MaxItemPerPage;
				}
				break;
			case Connection.EServerType.MySQL:
				text10 = " LIMIT " + (PageNumber - 1) * MaxItemPerPage + "," + MaxItemPerPage;
				sQL = "SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND + text7 + text10;
				break;
			}
		}
		else
		{
			sQL = "SELECT " + ShowingFields + " FROM " + TableName + " " + SQLJOIN + " WHERE 1=1 " + SQLAND + text7;
		}
		DataTable dataTable = new DataTable();
		dataTable = Command.ExecDataAdapter(sQL, tar);
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			if (dataTable.Columns.Count <= 0)
			{
				break;
			}
			if (PageNumber != -1)
			{
				dataTable.Columns["No"].SetOrdinal(0);
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxItemPerPage + (i + 1);
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
					dataTable.Rows[i]["No"] = (PageNumber - 1) * MaxItemPerPage + (i + 1);
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
		if (IsBind)
		{
			dgData.DataSource = dataTable;
			dgData.DataBind();
		}
		LoadDataResult result = default(LoadDataResult);
		result.Data = dataTable;
		result.ItemCount = num;
		return result;
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, "", "", Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, "", "", Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, null, null, null, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, "", "", Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, null, null, null, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, Label lbJumlahCantuman, TwoArrayList tar)
	{
		return LoadData(PageNumber, MaxItemPerPage, "", "", Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, null, null, null, lbJumlahCantuman, tar);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, Label lbJumlahCantuman, TwoArrayList tar)
	{
		return LoadData(PageNumber, MaxItemPerPage, "", "", Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, null, null, null, lbJumlahCantuman, tar);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, string FieldPencarian, string KataKunci, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, string FieldPencarian, string KataKunci, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, string FieldPencarian, string KataKunci, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman, TwoArrayList tar)
	{
		return LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, tar);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, string FieldPencarian, string KataKunci, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman, TwoArrayList tar)
	{
		LoadDataResult result = LoadData(PageNumber, Page, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, true, null);
		if (result.Data != null)
		{
			int itemCount = result.ItemCount;
			int num = (int)Math.Ceiling((double)itemCount / (double)MaxItemPerPage);
			if (PageNumber > num)
			{
				PageNumber = 1;
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
			}
            if (AcceptorLoadPageNumber != null)
            {
                AcceptorLoadPageNumber.Invoke(itemCount);
            }
			//AcceptorLoadPageNumber?.Invoke(itemCount);
			string text = "Halaman ke-" + PageNumber.ToString("N0") + " dari " + num.ToString("N0") + ". (" + itemCount.ToString("N0") + " data)";
			if (lbPage != null)
			{
				lbPage.Text = text;
			}
			if (lbPage0 != null)
			{
				lbPage0.Text = text;
			}
			if (lbJumlahCantuman != null)
			{
				lbJumlahCantuman.Text = "Jumlah Data : " + itemCount.ToString("N0");
			}
		}
		return result;
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, TwoArrayList FilterPencarian, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, FilterPencarian, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, TwoArrayList FilterPencarian, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman)
	{
		return LoadData(PageNumber, MaxItemPerPage, FilterPencarian, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, null);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, TwoArrayList FilterPencarian, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman, TwoArrayList tar)
	{
		return LoadData(PageNumber, MaxItemPerPage, FilterPencarian, Page, dgData, TableName, OrderFields, SQLJOIN, SQLAND, "", ShowingFields, AcceptorLoadPageNumber, lbPage, lbPage0, lbJumlahCantuman, tar);
	}

	public static LoadDataResult LoadData(int PageNumber, int MaxItemPerPage, TwoArrayList FilterPencarian, Page Page, DataGrid dgData, string TableName, string OrderFields, string SQLJOIN, string SQLAND, string SQLGROUPBY, string ShowingFields, LoadPageNumberDelegate AcceptorLoadPageNumber, Label lbPage, Label lbPage0, Label lbJumlahCantuman, TwoArrayList tar)
	{
		LoadDataResult result = LoadData(PageNumber, Page, dgData, TableName, FilterPencarian, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, true, null);
		if (result.Data != null)
		{
			int itemCount = result.ItemCount;
			int num = (int)Math.Ceiling((double)itemCount / (double)MaxItemPerPage);
			if (PageNumber > num)
			{
				itemCount = result.ItemCount;
				PageNumber = num;
				int num2 = (int)Math.Ceiling((double)num / (double)MyApplication.MaxPageNumberDisplayed);
				Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), num2);
			}
            if (AcceptorLoadPageNumber != null)
            {
                AcceptorLoadPageNumber.Invoke(itemCount);
            }
			//AcceptorLoadPageNumber?.Invoke(itemCount);
			string text = "Halaman ke-" + PageNumber.ToString("N0") + " dari " + num.ToString("N0") + ". (" + itemCount.ToString("N0") + " data)";
			if (lbPage != null)
			{
				lbPage.Text = text;
			}
			if (lbPage0 != null)
			{
				lbPage0.Text = text;
			}
			if (lbJumlahCantuman != null)
			{
				lbJumlahCantuman.Text = "Jumlah Data : " + itemCount.ToString("N0");
			}
		}
		return result;
	}

	public static void DatagridOnEditCommand(Page Page, string PageAddOrEditURL, DataGridCommandEventArgs e, int IDIndex)
	{
		if (Page.Session[MySession.CurrentIDData] == null)
		{
			Page.Session.Add(MySession.CurrentIDData, e.Item.Cells[IDIndex].Text);
		}
		else
		{
			Page.Session[MySession.CurrentIDData] = e.Item.Cells[IDIndex].Text;
		}
		if (e.Item.Cells[3].Text.Trim().Length == 0)
		{
			string text = ((LinkButton)e.Item.Cells[IDIndex].Controls[0]).Text;
			if (!string.IsNullOrEmpty(text))
			{
				if (Page.Session[MySession.CurrentIDData] == null)
				{
					Page.Session.Add(MySession.CurrentIDData, text);
				}
				else
				{
					Page.Session[MySession.CurrentIDData] = text;
				}
			}
		}
		Page.Response.Redirect(PageAddOrEditURL + (PageAddOrEditURL.Contains("?") ? "&" : "?") + "edit=1&id=" + Page.Session[MySession.CurrentIDData].ToString());
	}

	public static void DatagridOnEditCommand2(Page Page, string PageAddOrEditURL, DataGridCommandEventArgs e, int IDIndex)
	{
		if (Page.Session[MySession.CurrentIDData] == null)
		{
			Page.Session.Add(MySession.CurrentIDData, IDIndex);
		}
		else
		{
			Page.Session[MySession.CurrentIDData] = IDIndex;
		}
		Page.Response.Redirect(PageAddOrEditURL + "?edit=1&id=" + Page.Session[MySession.CurrentIDData].ToString());
	}

	public static void DatagridOnEditCommand3(Page Page, string PageAddOrEditURL, DataGridCommandEventArgs e, string IDIndex)
	{
		if (Page.Session[MySession.CurrentIDData] == null)
		{
			Page.Session.Add(MySession.CurrentIDData, IDIndex);
		}
		else
		{
			Page.Session[MySession.CurrentIDData] = IDIndex;
		}
		Page.Response.Redirect(PageAddOrEditURL + "?edit=1&id=" + Page.Session[MySession.CurrentIDData].ToString());
	}

	public static void DatagridOnDeleteCommand(Page Page, DataGridCommandEventArgs e, MessageBoxUsc_MsgBoxUsc MsgBoxUsc, string TableName, DataGrid Datagrid, LoadDataDelegate AcceptorLoadData)
	{
		DatagridOnDeleteCommand(Page, e, MsgBoxUsc, TableName, Datagrid, AcceptorLoadData, 3);
	}

	public static void DatagridOnDeleteCommand(Page Page, DataGridCommandEventArgs e, MessageBoxUsc_MsgBoxUsc MsgBoxUsc, string TableName, DataGrid Datagrid, LoadDataDelegate AcceptorLoadData, int Indeks)
	{
		DataUIProvider.AcceptorLoadData = AcceptorLoadData;
		if (e.CommandName == "Delete")
		{
			if (Page.Session[MySession.CurrentIDData] == null)
			{
				Page.Session.Add(MySession.CurrentIDData, e.Item.Cells[Indeks].Text);
			}
			else
			{
				Page.Session[MySession.CurrentIDData] = e.Item.Cells[Indeks].Text;
			}
			MsgBoxUsc.AddMessage("Anda yakin akan menghapus data ini?.", MessageBoxUsc_MsgBoxUsc.enmMessageType.Attention, true, true, "");
		}
	}

	public static bool DeleteData(string TableName, string IDData)
	{
		string sQL = "DELETE FROM " + TableName + " WHERE id='" + IDData + "'";
		return Command.ExecNonQuery(sQL);
	}

	public static void DatagridOnSortCommand(Page Page, DataGridSortCommandEventArgs e, DataGrid Datagrid, LoadDataDelegate AcceptorLoadData, int MaxItemPerPage)
	{
		DataUIProvider.AcceptorLoadData = AcceptorLoadData;
		string text = e.SortExpression.ToString();
		string text2 = "";
		if (Page.Session[MySession.CurrentPage] != null)
		{
			text2 = Page.Session[MySession.CurrentPage].ToString();
		}
		else
		{
			text2 = Path.GetFileName(Page.Request.Path);
			Page.Session.Add(MySession.CurrentPage, text2);
		}
		if (Page.Session[text2 + "Current" + text] == null)
		{
			Page.Session.Add(text2 + "Current" + text, "ASC");
		}
		else if (Page.Session[text2 + "Current" + text].ToString() == "ASC")
		{
			Page.Session[text2 + "Current" + text] = "DESC";
		}
		else
		{
			Page.Session[text2 + "Current" + text] = "ASC";
		}
		string text3 = "";
		string[] array = text.Split(',');
		string text4 = Page.Session[text2 + "Current" + text].ToString();
		for (int i = 0; i < array.Length; i++)
		{
			string text5 = text3;
			text3 = text5 + array[i] + " " + text4 + ",";
		}
		text3 = text3.TrimEnd(',');
		if (text3.Trim().ToUpper().Contains("NULLS LAST ASC"))
		{
			text3 = text3.Replace("NULLS LAST ASC", "ASC NULLS LAST");
		}
		else if (text3.Trim().ToUpper().Contains("NULLS LAST DESC"))
		{
			text3 = text3.Replace("NULLS LAST DESC", "DESC NULLS LAST");
		}
		else if (text3.Trim().ToUpper().Contains("NULLS FIRST ASC"))
		{
			text3 = text3.Replace("NULLS FIRST ASC", "ASC NULLS FIRST");
		}
		else if (text3.Trim().ToUpper().Contains("NULLS FIRST DESC"))
		{
			text3 = text3.Replace("NULLS FIRST DESC", "DESC NULLS FIRST");
		}
		Page.Session.Add(text2 + MySession.CurrentSortExp, text3);
		Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), 1);
		Page.Session.Add(MySession.CurrentIndexPage + text2, 1);
		AcceptorLoadData(1, MaxItemPerPage);
	}

	public static void LoadPageNumber(Page Page, int ItemCount, Repeater RepeaterPage, int MaxItemPerPage)
	{
		int num = 1;
		if (Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()] == null)
		{
			Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), num);
		}
		else
		{
			num = int.Parse(Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()].ToString());
		}
		if (num <= 0)
		{
			num = 1;
		}
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("PageNumber");
		int num2 = (int)Math.Ceiling((double)ItemCount / (double)MaxItemPerPage);
		int num3 = MyApplication.MaxPageNumberDisplayed * (num - 1) + 1;
		int num4 = MyApplication.MaxPageNumberDisplayed * num;
		if (num4 > num2)
		{
			num4 = num2;
			num = (int)Math.Ceiling((double)num2 / (double)MyApplication.MaxPageNumberDisplayed);
			num3 = MyApplication.MaxPageNumberDisplayed * (num - 1) + 1;
			Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), num);
		}
		if (num3 > 0)
		{
			for (int i = num3; i <= num4; i++)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow[0] = i;
				dataTable.Rows.Add(dataRow);
			}
			DataRow dataRow2 = null;
			if (num4 < num2)
			{
				dataRow2 = dataTable.NewRow();
				dataRow2[0] = ">>";
				dataTable.Rows.Add(dataRow2);
			}
			if (num > 1)
			{
				dataRow2 = dataTable.NewRow();
				dataRow2[0] = "<<";
				dataTable.Rows.InsertAt(dataRow2, 0);
			}
		}
		RepeaterPage.DataSource = dataTable;
		RepeaterPage.DataBind();
	}

	public static void OnPageButtonClick(Page Page, object sender, EventArgs e, LoadDataDelegate AcceptorLoadData, int MaxItemPerPage)
	{
		DataUIProvider.AcceptorLoadData = AcceptorLoadData;
		LinkButton linkButton = (LinkButton)sender;
		int num = 0;
		if (linkButton.Text != "<<" && linkButton.Text != ">>")
		{
			num = int.Parse(linkButton.Text);
		}
		else
		{
			int num2 = 1;
			if (linkButton.Text == "<<")
			{
				if (Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()] == null)
				{
					num2 = 1;
				}
				else
				{
					num2 = int.Parse(Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()].ToString()) - 1;
					if (num2 < 1)
					{
						num2 = 1;
					}
					Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()] = num2;
				}
				num = num2 * MyApplication.MaxPageNumberDisplayed;
			}
			else
			{
				if (Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()] == null)
				{
					num2 = MyApplication.MaxPageNumberDisplayed;
				}
				else
				{
					num2 = int.Parse(Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()].ToString()) + 1;
					Page.Session["PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString()] = num2;
				}
				num = (num2 - 1) * MyApplication.MaxPageNumberDisplayed + 1;
			}
		}
		AcceptorLoadData(num, MaxItemPerPage);
	}

	public static void OnPageGoTo_TextChanged(Page Page, TextBox PageGoToTextbox, LoadDataDelegate AcceptorLoadData, int MaxItemPerPage)
	{
		if (PageGoToTextbox.Text.Trim().Length == 0)
		{
			PageGoToTextbox.Text = "1";
		}
		int num = int.Parse(PageGoToTextbox.Text);
		int num2 = (int)Math.Ceiling((double)num / (double)MyApplication.MaxPageNumberDisplayed);
		Page.Session.Add("PageNumberIndex" + Page.Session[MySession.CurrentPage].ToString(), num2);
		AcceptorLoadData(num, MaxItemPerPage);
	}

	public static void OnRepeaterPage_ItemDataBound(Page Page, object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
		{
			return;
		}
		LinkButton linkButton = (LinkButton)e.Item.FindControl("lbtPage");
		if (linkButton != null && Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] != null)
		{
			if (linkButton.Text == Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString())
			{
				linkButton.CssClass = "PageNumberSelected";
			}
			else
			{
				linkButton.CssClass = "PageNumberNormal";
			}
		}
	}

	public static void LoadPage(DropDownList ddlPage)
	{
		DataTable dataTable = new DataTable();
		DataRow dataRow = null;
		dataTable.Columns.Add("ID");
		dataTable.Columns.Add("Name");
		dataRow = dataTable.NewRow();
		dataRow[0] = "10";
		dataRow[1] = "10 per halaman";
		dataTable.Rows.Add(dataRow);
		dataRow = dataTable.NewRow();
		dataRow[0] = "20";
		dataRow[1] = "20 per halaman";
		dataTable.Rows.Add(dataRow);
		dataRow = dataTable.NewRow();
		dataRow[0] = "50";
		dataRow[1] = "50 per halaman";
		dataTable.Rows.Add(dataRow);
		dataRow = dataTable.NewRow();
		dataRow[0] = "100";
		dataRow[1] = "100 per halaman";
		dataTable.Rows.Add(dataRow);
		ddlPage.DataSource = dataTable;
		ddlPage.DataValueField = "ID";
		ddlPage.DataTextField = "Name";
		ddlPage.DataBind();
		Page page = ddlPage.Page;
		if (page != null && page.Session["MaxItemPerPage"] != null)
		{
			ddlPage.SelectedValue = page.Session["MaxItemPerPage"].ToString();
		}
	}

	public static void SaveHistory(Page Page, HistoryAction HistoryAction, string TableName, string IDRef, string Note)
	{
		if (!string.IsNullOrEmpty(Note.Trim()))
		{
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("Action", HistoryAction.ToString());
			twoArrayList.Add("TableName", TableName);
			twoArrayList.Add("IDRef", IDRef);
			twoArrayList.Add("ActionBy", Page.User.Identity.Name);
			twoArrayList.Add("ActionDate", DateTime.Now);
			twoArrayList.Add("ActionTerminal", Util.GetUserHost(Page));
			twoArrayList.Add("Note", Note);
			Command.ExecInsertOrUpdate("historydata", twoArrayList, Command.InsertOrUpdate.Insert);
		}
	}

	public static void SaveHistory(string ActionBy, string Terminal, HistoryAction HistoryAction, string TableName, string IDRef, string Note)
	{
		if (!string.IsNullOrEmpty(Note.Trim()))
		{
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("Action", HistoryAction.ToString());
			twoArrayList.Add("TableName", TableName);
			twoArrayList.Add("IDRef", IDRef);
			twoArrayList.Add("ActionBy", ActionBy);
			twoArrayList.Add("ActionDate", DateTime.Now);
			twoArrayList.Add("ActionTerminal", Terminal);
			twoArrayList.Add("Note", Note);
			Command.ExecInsertOrUpdate("historydata", twoArrayList, Command.InsertOrUpdate.Insert);
		}
	}

	public static void SaveHistory(Page Page, HistoryAction HistoryAction, string TableName, string IDRef, string Note, string PrefixText)
	{
		if (!string.IsNullOrEmpty(Note.Trim()))
		{
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("Action", HistoryAction.ToString());
			twoArrayList.Add("TableName", TableName);
			twoArrayList.Add("IDRef", IDRef);
			twoArrayList.Add("ActionBy", Page.User.Identity.Name);
			twoArrayList.Add("ActionDate", DateTime.Now);
			twoArrayList.Add("ActionTerminal", Util.GetUserHost(Page));
			twoArrayList.Add("Note", PrefixText + Note);
			Command.ExecInsertOrUpdate("historydata", twoArrayList, Command.InsertOrUpdate.Insert);
		}
	}

	public static void CreateWorksheetFolder(Page page, string WorsheetName)
	{
		try
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				WorsheetName = WorsheetName.Replace(oldChar, '_');
			}
			string path = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCover"]), WorsheetName);
			string path2 = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCoverThumb"]), WorsheetName);
			string text = ConfigurationManager.AppSettings["CatalogDirectoryFile"];
			string[] array = text.Split(char.Parse(";"));
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (!Directory.Exists(path2))
			{
				Directory.CreateDirectory(path2);
			}
			string[] array2 = array;
			foreach (string virtualPath in array2)
			{
				text = Path.Combine(page.MapPath(virtualPath), WorsheetName);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
			}
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message);
		}
	}

	public static void RenameWorksheetFolder(Page page, string PreviousWorsheetName, string NewWorsheetName)
	{
		try
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				PreviousWorsheetName = PreviousWorsheetName.Replace(oldChar, '_');
				NewWorsheetName = NewWorsheetName.Replace(oldChar, '_');
			}
			string text = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCover"]), PreviousWorsheetName);
			string text2 = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCoverThumb"]), PreviousWorsheetName);
			string text3 = ConfigurationManager.AppSettings["CatalogDirectoryFile"];
			string[] array = text3.Split(char.Parse(";"));
			string text4 = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCover"]), NewWorsheetName);
			string text5 = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCoverThumb"]), NewWorsheetName);
			if (Directory.Exists(text))
			{
				Directory.Move(text, text4);
			}
			else
			{
				Directory.CreateDirectory(text4);
				Directory.Move(text, text4);
			}
			if (Directory.Exists(text2))
			{
				Directory.Move(text2, text5);
			}
			else
			{
				Directory.CreateDirectory(text5);
				Directory.Move(text2, text5);
			}
			string[] array2 = array;
			foreach (string path in array2)
			{
				string text6 = Path.Combine(path, PreviousWorsheetName);
				string text7 = Path.Combine(path, NewWorsheetName);
				if (Directory.Exists(text6))
				{
					Directory.Move(text6, text7);
					continue;
				}
				Directory.CreateDirectory(text7);
				Directory.Move(text6, text7);
			}
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message);
		}
	}

	public static void DeleteWorksheetFolder(Page page, string WorsheetName)
	{
		try
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				WorsheetName = WorsheetName.Replace(oldChar, '_');
			}
			string path = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCover"]), WorsheetName);
			string path2 = Path.Combine(page.MapPath(ConfigurationManager.AppSettings["CatalogDirectoryCoverThumb"]), WorsheetName);
			string text = ConfigurationManager.AppSettings["CatalogDirectoryFile"];
			string[] array = text.Split(char.Parse(";"));
			if (Directory.Exists(path))
			{
				Directory.Delete(path);
			}
			if (Directory.Exists(path2))
			{
				Directory.Delete(path2);
			}
			string[] array2 = array;
			foreach (string virtualPath in array2)
			{
				text = Path.Combine(page.MapPath(virtualPath), WorsheetName);
				if (Directory.Exists(text))
				{
					Directory.Delete(text);
				}
			}
		}
		catch (Exception ex)
		{
			Util.RaiseMessage(ex.Message);
		}
	}
}
