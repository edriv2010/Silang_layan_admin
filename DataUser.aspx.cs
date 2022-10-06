using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DataUser : Page
{
















	protected MessageBoxUsc_MsgBoxUsc MsgBoxUsc1;


	protected Exporter Exporter1;

	protected string TableName = "USERS";

	private string OrderFields = "UserName";

	private string[] KriteriaPencarian = new string[8] { "UserName|Nama User", "FullName|Nama Lengkap", "EmailAddress|Email", "NoHP|No. Handphone", "HakAkses|Hak Akses", "Propinsi.NAMAPROPINSI|Provinsi", "Kabupaten.NAMAKAB|Kabupaten", "IsActive|Aktif" };

	protected string PageAddOrEditURL = "DataUserAdd.aspx";

	protected string PageTitle = "Pengaturan Umum ► Data User";

	private string SQLJOIN = " LEFT JOIN Propinsi ON USERS.Propinsi_id = Propinsi.id LEFT JOIN Kabupaten ON USERS.Kabupaten_id = Kabupaten.id";

	private string ShowingFields = "USERS.*, Propinsi.NAMAPROPINSI as PropinsiName, Kabupaten.NAMAKAB as KabupatenName";

	private string ExportFields = "UserName as \"Nama User\", USERS.FullName as \"Nama Lengkap\", EmailAddress as Email, NoHP as \"No. Handphone\", HakAkses as \"Hak Akses\", Propinsi.NAMAPROPINSI as Provinsi, Kabupaten.NAMAKAB as Kabupaten, IsActive as Aktif";

	protected int iPage = 0;

	protected bool IsAlreadyLoadData = false;

	protected string FieldPencarian = "";

	protected string KataKunci = "";

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			DataUIProvider.LoadPage(ddlPage);
		}
		LoginAuth.InitPage(Page, new int[1] { Enums.UserAuth.AdminOnly });
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
		ScriptManager.RegisterStartupScript(this, GetType(), "RegisterScript", "RegisterScript();", addScriptTags: true);
	}

	private void GetFilters()
	{
		FieldPencarian = ddlKriteria.SelectedValue;
		KataKunci = txtKataKunci.Text;
	}

	protected void LoadData(int PageNumber, int MaxItemPerPage)
	{
		if (!IsAlreadyLoadData)
		{
			GetFilters();
			DataUIProvider.LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, this, dgData, TableName, OrderFields, SQLJOIN, "", ShowingFields, Load_PageNumber, null, lbPage0, lbJumlahCantuman);
			IsAlreadyLoadData = true;
		}
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
			string text = e.Item.Cells[3].Text;
			string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
			if (text2 == MyApplication.SuperAdminName)
			{
				Util.ShowAlertMessage("User ini tidak boleh diedit!");
			}
			else
			{
				DataUIProvider.DatagridOnEditCommand(this, PageAddOrEditURL, e, 3);
			}
		}
	}

	protected void dgData_EditCommand(object source, DataGridCommandEventArgs e)
	{
		string text = e.Item.Cells[3].Text;
		string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
		if (text2 == MyApplication.SuperAdminName)
		{
			Util.ShowAlertMessage("User ini tidak boleh diedit!");
		}
		else
		{
			DataUIProvider.DatagridOnEditCommand(this, PageAddOrEditURL, e, 3);
		}
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
		string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
		if (text2 == MyApplication.SuperAdminName)
		{
			MsgBoxUsc1.AddMessage("User ini tidak boleh dihapus!", MessageBoxUsc_MsgBoxUsc.enmMessageType.Error, false,false, "");
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

	protected void dgData_OnItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && e.Item.Cells[1].Controls.Count > 0)
		{
			LinkButton linkButton = (LinkButton)e.Item.Cells[1].Controls[0];
			linkButton.Text = "<i class=\"fa fa-trash-o\"></i> Hapus";
			linkButton.CssClass = "DeleteButton";
		}
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
