using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DataPerpustakaan : Page
{
	protected string TableName = "branchs";

	private string OrderFields = "Name";

	private string[] KriteriaPencarian = new string[2] { "Name|Nama", "Code|Kode" };

	protected string PageAddOrEditURL = "DataPerpustakaanAdd.aspx";

	protected string PageTitle = "Administrasi ► Data Perpustakaan";

	private string SQLJOIN = "";

	private string SQLAND = "";

	private string ShowingFields = "branchs.*,(CASE WHEN (SYSDATE-LASTONSTATUS)*24 <= 6 THEN 'ON' ELSE 'OFF' END) as ONOFF";

	private string ExportFields = "branchs.*,(CASE WHEN (SYSDATE-LASTONSTATUS)*24 <= 6 THEN 'ON' ELSE 'OFF' END) as ONOFF";

	protected int iPage = 0;

	protected bool IsAlreadyLoadData = false;

	protected string FieldPencarian = "";

	protected string KataKunci = "";

	protected string UserBranchId = "";





















	protected MessageBoxUsc_MsgBoxUsc MsgBoxUsc1;

	protected Exporter Exporter1;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			lbPageTitle.Text = Util.GetPageTitle(this);
			DataUIProvider.LoadPage(ddlPage);
		}
		LoginAuth.InitPage(Page, null);
		GetFilters();
		DataUIProvider.InitPageList(this, KriteriaPencarian, ddlKriteria, txtKataKunci, LoadData, int.Parse(ddlPage.SelectedValue));
		MsgBoxUsc1.MsgBoxAnswered += MessageAnsweredForDelete;
		Exporter1.ParentPage = this;
		Exporter1.dgData = dgData;
		Exporter1.TableName = TableName;
		Exporter1.FieldPencarian = ddlKriteria.SelectedValue;
		Exporter1.KataKunci = txtKataKunci.Text;
		Exporter1.OrderFields = OrderFields;
		Exporter1.SQLJOIN = SQLJOIN;
		Exporter1.ShowingFields = ExportFields;
	}

	private void GetFilters()
	{
		UserBranchId = Command.ExecScalar("SELECT Branch_Id FROM users WHERE Id = " + UserProfileProvider.Current.Id, "");
		if (!string.IsNullOrEmpty(UserBranchId))
		{
			SQLAND = " AND ID = " + UserBranchId;
			btnAddNew.Visible = false;
		}
		FieldPencarian = ddlKriteria.SelectedValue;
		KataKunci = txtKataKunci.Text;
	}

	protected void LoadData(int PageNumber, int MaxItemPerPage)
	{
		if (IsAlreadyLoadData)
		{
			return;
		}
		GetFilters();
		DataUIProvider.LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, this, dgData, TableName, OrderFields, SQLJOIN, SQLAND, ShowingFields, Load_PageNumber, lbPage, lbPage0, lbJumlahCantuman);
		if (!string.IsNullOrEmpty(UserBranchId))
		{
			dgData.Columns[1].Visible = false;
			for (int i = 0; i < dgData.Items.Count; i++)
			{
				dgData.Items[i].Cells[5].Enabled = false;
			}
		}
		IsAlreadyLoadData = true;
	}

	protected void Load_PageNumber(int ItemCount)
	{
		DataUIProvider.LoadPageNumber(this, ItemCount, RepeaterPage, int.Parse(ddlPage.SelectedValue));
	}

	protected void lbtPage_Click(object sender, EventArgs e)
	{
		DataUIProvider.OnPageButtonClick(this, sender, e, LoadData, int.Parse(ddlPage.SelectedValue));
	}

	protected void dgData_ItemCommand(object source, DataGridCommandEventArgs e)
	{
		if (e.CommandName == "Detail")
		{
			DataUIProvider.DatagridOnEditCommand(this, PageAddOrEditURL, e, 3);
		}
	}

	protected void dgData_EditCommand(object source, DataGridCommandEventArgs e)
	{
		DataUIProvider.DatagridOnEditCommand(this, PageAddOrEditURL, e, 3);
	}

	protected void dgData_DeleteCommand(object source, DataGridCommandEventArgs e)
	{
		DataUIProvider.DatagridOnDeleteCommand(this, e, MsgBoxUsc1, TableName, dgData, LoadData);
	}

	protected void MessageAnsweredForDelete(object sender, MessageBoxUsc_MsgBoxUsc.MsgBoxEventArgs e)
	{
		if (e.Answer != 0)
		{
			return;
		}
		dgData.EditItemIndex = -1;
		string text = Page.Session[MySession.CurrentIDData].ToString();
		if (int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM catalogs WHERE Branch_id=" + text, "0")) > 0)
		{
			MsgBoxUsc1.AddMessage("Perpustakaan ini mempunyai data Katalog, sehingga tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false, false, "");
			return;
		}
		if (int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM collections WHERE Branch_id=" + text, "0")) > 0)
		{
			MsgBoxUsc1.AddMessage("Perpustakaan ini mempunyai data Koleksi, sehingga tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false, false, "");
			return;
		}
		if (int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM members WHERE Branch_id=" + text, "0")) > 0)
		{
			MsgBoxUsc1.AddMessage("Perpustakaan ini mempunyai data Anggota, sehingga tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false, false, "");
			return;
		}
		if (int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM collectionloans WHERE Branch_id=" + text, "0")) > 0)
		{
			MsgBoxUsc1.AddMessage("Perpustakaan ini mempunyai data Peminjaman, sehingga tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false, false, "");
			return;
		}
		if (int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM departments WHERE Branch_id=" + text, "0")) > 0)
		{
			MsgBoxUsc1.AddMessage("Perpustakaan ini mempunyai data Bagian, sehingga tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false, false, "");
			return;
		}
		DataUIProvider.DeleteData(TableName, text);
		int pageNumber = 1;
		if (Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] != null)
		{
			pageNumber = int.Parse(Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString());
		}
		LoadData(pageNumber, int.Parse(ddlPage.SelectedValue));
	}

	protected void dgData_OnSortCommand(object sender, DataGridSortCommandEventArgs e)
	{
		DataUIProvider.DatagridOnSortCommand(this, e, dgData, LoadData, int.Parse(ddlPage.SelectedValue));
	}

	protected void ibPencarian_Click(object sender, ImageClickEventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}

	protected void txtKataKunci_TextChanged(object sender, EventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}

	protected void txtPageGoTo_TextChanged(object sender, EventArgs e)
	{
		DataUIProvider.OnPageGoTo_TextChanged(this, txtPageGoTo, LoadData, int.Parse(ddlPage.SelectedValue));
	}

	protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		DataUIProvider.OnRepeaterPage_ItemDataBound(this, sender, e);
	}

	protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}
}
