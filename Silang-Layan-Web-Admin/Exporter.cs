using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public class Exporter : UserControl
{
	public delegate void ToggleBindDelegate(bool IsToggleBind);

	protected DropDownList ddlExportData;

	protected TextBox txtMaksJumlahData;

	protected ImageButton btExportToExcel;

	protected ImageButton btExportToWord;

	protected ImageButton btExportToXML;

	public Page ParentPage;

	public DataGrid dgData;

	public string TableName = "";

	public string FieldPencarian = "";

	public string KataKunci = "";

	public string OrderFields = "";

	public string SQLJOIN = "";

	public string SQLAND = "";

	public string SQLGROUPBY = "";

	public string ShowingFields = "";

	public ToggleBindDelegate IsToggleBind;

	public TwoArrayList tar;

	public TwoArrayList FilterMultiplePencarian;

	public bool IsExporting = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		IsExporting = false;
	}

	private void ToggleBind(bool IsToggleBind)
	{
		if (IsToggleBind)
		{
			this.IsToggleBind(IsToggleBind: true);
		}
	}
    public DataTable LoadDataPage()
    {
        int MaxItemPerPage = ParentPage.Session["MaxItemPerPage"] == null ? MyApplication.MaxItemPerPage : int.Parse(ParentPage.Session["MaxItemPerPage"].ToString());
        int PageNumber = ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] == null ? 1 : int.Parse(ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString());
        if (FilterMultiplePencarian == null)
        {
            if (IsToggleBind == null)
            {
                return DataUIProvider.LoadData(PageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false).Data;
            }
            else
            {
                return DataUIProvider.LoadData(PageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
            }
        }
        else
        {
            if (IsToggleBind == null)
            {
                return DataUIProvider.LoadData(PageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false).Data;
            }
            else
            {
                return DataUIProvider.LoadData(PageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, MaxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
            }
        }
    }

    /*
	public DataTable LoadDataPage()
	{
		int maxItemPerPage = ((ParentPage.Session["MaxItemPerPage"] == null) ? MyApplication.MaxItemPerPage : int.Parse(ParentPage.Session["MaxItemPerPage"].ToString()));
		int pageNumber = ((ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] == null) ? 1 : int.Parse(ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString()));
		if (FilterMultiplePencarian == null)
		{
			if (IsToggleBind == null)
			{
				return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar,  false).Data;
			}
			return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;).Data;
		}
		if (IsToggleBind == null)
		{
			return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar,false).Data;
		}
		return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
	}
    */
	protected DataTable LoadDataAll()
	{
		if (FilterMultiplePencarian == null)
		{
			if (IsToggleBind == null)
			{
				return DataUIProvider.LoadData(-1, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, -1, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false).Data;
			}
			return DataUIProvider.LoadData(-1, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, -1, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
		}
		if (IsToggleBind == null)
		{
			return DataUIProvider.LoadData(-1, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, -1, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false).Data;
		}
		return DataUIProvider.LoadData(-1, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, -1, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
	}

	protected DataTable LoadDataMaksRestrict()
	{
		int num = ((ParentPage.Session["MaxItemPerPage"] == null) ? MyApplication.MaxItemPerPage : int.Parse(ParentPage.Session["MaxItemPerPage"].ToString()));
		int pageNumber = ((ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] == null) ? 1 : int.Parse(ParentPage.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString()));
		int maxItemPerPage = (string.IsNullOrEmpty(txtMaksJumlahData.Text) ? num : int.Parse(txtMaksJumlahData.Text));
		if (FilterMultiplePencarian == null)
		{
			if (IsToggleBind == null)
			{
				return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, IsBind: false).Data;
			}
			return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FieldPencarian, KataKunci, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
		}
		if (IsToggleBind == null)
		{
			return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, IsBind: false).Data;
		}
		return DataUIProvider.LoadData(pageNumber, ParentPage, dgData, TableName, FilterMultiplePencarian, OrderFields, maxItemPerPage, SQLJOIN, SQLAND, SQLGROUPBY, ShowingFields, tar, false, ToggleBind).Data;
	}

	protected void btExportToExcel_Click(object sender, ImageClickEventArgs e)
	{
		IsExporting = true;
		switch (ddlExportData.SelectedIndex)
		{
		case 0:
			DocumentExporter.ExportToExcel(ParentPage, dgData, TableName, LoadDataPage);
			IsExporting = false;
			break;
		case 1:
			DocumentExporter.ExportToExcel(ParentPage, dgData, TableName, LoadDataAll);
			IsExporting = false;
			break;
		case 2:
			DocumentExporter.ExportToExcel(ParentPage, dgData, TableName, LoadDataMaksRestrict);
			IsExporting = false;
			break;
		}
	}

	protected void btExportToWord_Click(object sender, ImageClickEventArgs e)
	{
		IsExporting = true;
		switch (ddlExportData.SelectedIndex)
		{
		case 0:
			DocumentExporter.ExportToWord(ParentPage, dgData, TableName, LoadDataPage);
			IsExporting = false;
			break;
		case 1:
			DocumentExporter.ExportToWord(ParentPage, dgData, TableName, LoadDataAll);
			IsExporting = false;
			break;
		case 2:
			DocumentExporter.ExportToWord(ParentPage, dgData, TableName, LoadDataMaksRestrict);
			IsExporting = false;
			break;
		}
	}

	protected void btExportToXML_Click(object sender, ImageClickEventArgs e)
	{
		IsExporting = true;
		switch (ddlExportData.SelectedIndex)
		{
		case 0:
			DocumentExporter.ExportToXML(ParentPage, TableName, LoadDataPage);
			IsExporting = false;
			break;
		case 1:
			DocumentExporter.ExportToXML(ParentPage, TableName, LoadDataAll);
			IsExporting = false;
			break;
		case 2:
			DocumentExporter.ExportToXML(ParentPage, TableName, LoadDataMaksRestrict);
			IsExporting = false;
			break;
		}
	}
}
