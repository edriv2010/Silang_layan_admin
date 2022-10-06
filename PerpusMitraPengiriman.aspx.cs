using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PerpusMitraPengiriman : Page
{






















	protected MessageBoxUsc_MsgBoxUsc MsgBoxUsc1;

	protected Exporter Exporter1;

	protected string TableName = "pm_kirim";

	private string OrderFields = "pm_kirim.CreateDate DESC";

	private string[] KriteriaPencarian = new string[6] { "NomorBarcode|Item ID", "NoInduk|No. Induk", "Title|Judul", "Author|Pengarang", "Publikasi|Publikasi", "pm_kirim.CreateBy|CreateBy" };

	private string SQLJOIN = " LEFT JOIN collections ON pm_kirim.Collection_ID=collections.ID";

	private string ShowingFields = "pm_kirim.ID,NomorBarcode,NoInduk,Title,Author,Publikasi,Tanggal_Kirim,pm_kirim.CreateDate as CreateDate,pm_kirim.CreateBy as CreateBy";

	private string SQLAND = "";

	protected int iPage = 0;

	protected bool IsAlreadyLoadData = false;

	protected bool IsMasterBlank = false;

	protected string FieldPencarian = "";

	protected string KataKunci = "";

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)
		{
			DataUIProvider.LoadPage(ddlPage);
			txtTanggalKirim.Text = DateTime.Now.ToString("yyyy-MM-dd");
			txtFilterTanggalKirimAwal.Text = DateTime.Now.ToString("yyyy-MM-dd");
			txtFilterTanggalKirimAkhir.Text = DateTime.Now.ToString("yyyy-MM-dd");
		}
		LoginAuth.InitPage(Page, null);
		ScriptManager.RegisterStartupScript(this, GetType(), "RegisterScript", "RegisterScript();", addScriptTags: true);
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
		Exporter1.SQLAND = SQLAND;
		Exporter1.ShowingFields = ShowingFields;
		if (!Page.IsPostBack)
		{
			txtNomorBarcode.Focus();
		}
	}

	private void GetFilters()
	{
		if (!string.IsNullOrEmpty(txtFilterTanggalKirimAwal.Text) && !string.IsNullOrEmpty(txtFilterTanggalKirimAkhir.Text))
		{
			SQLAND = " AND TO_CHAR(TANGGAL_KIRIM,'YYYY-MM-DD') >= '" + txtFilterTanggalKirimAwal.Text + "' AND TO_CHAR(TANGGAL_KIRIM,'YYYY-MM-DD') <= '" + txtFilterTanggalKirimAkhir.Text + "'";
		}
		else if (!string.IsNullOrEmpty(txtFilterTanggalKirimAwal.Text))
		{
			SQLAND = " AND TO_CHAR(TANGGAL_KIRIM,'YYYY-MM-DD') >= '" + txtFilterTanggalKirimAwal.Text + "'";
		}
		else if (!string.IsNullOrEmpty(txtFilterTanggalKirimAkhir.Text))
		{
			SQLAND = " AND TO_CHAR(TANGGAL_KIRIM,'YYYY-MM-DD') <= '" + txtFilterTanggalKirimAkhir.Text + "'";
		}
		FieldPencarian = ddlKriteria.SelectedValue;
		KataKunci = txtKataKunci.Text;
	}

	protected void LoadData(int PageNumber, int MaxItemPerPage)
	{
		if (!IsAlreadyLoadData)
		{
			GetFilters();
			DataTable data = DataUIProvider.LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, this, dgData, TableName, OrderFields, SQLJOIN, SQLAND, ShowingFields, Load_PageNumber, lbPage, lbPage0, lbJumlahCantuman).Data;
			dgData.DataSource = data;
			dgData.DataBind();
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
		if (!(e.CommandName == "Edit"))
		{
		}
	}

	protected void dgData_EditCommand(object source, DataGridCommandEventArgs e)
	{
	}

	protected void dgData_DeleteCommand(object source, DataGridCommandEventArgs e)
	{
		if (e.CommandName == "Delete")
		{
			if (Page.Session[MySession.CurrentIDData] == null)
			{
				Page.Session.Add(MySession.CurrentIDData, ((LinkButton)e.Item.Cells[3].Controls[0]).Text);
			}
			else
			{
				Page.Session[MySession.CurrentIDData] = ((LinkButton)e.Item.Cells[3].Controls[0]).Text;
			}
			MsgBoxUsc1.AddMessage("Anda yakin akan menghapus data ini?.", MessageBoxUsc_MsgBoxUsc.enmMessageType.Attention, true,true, "");
		}
	}

	protected void MessageAnsweredForDelete(object sender, MessageBoxUsc_MsgBoxUsc.MsgBoxEventArgs e)
	{
		if (e.Answer == MessageBoxUsc_MsgBoxUsc.enmAnswer.OK)
		{
			dgData.EditItemIndex = -1;
			DataUIProvider.DeleteData(TableName, int.Parse(Page.Session[MySession.CurrentIDData].ToString()).ToString());
			int pageNumber = 1;
			if (Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()] != null)
			{
				pageNumber = int.Parse(Page.Session[MySession.CurrentIndexPage + Page.Session[MySession.CurrentPage].ToString()].ToString());
			}
			LoadData(pageNumber, int.Parse(ddlPage.SelectedValue));
		}
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

	private void CheckNomorBarcode()
	{
		string text = txtNomorBarcode.Text;
		if (text.Trim().Length == 0)
		{
			return;
		}
		string text2 = Command.ExecScalar("SELECT ID FROM collections WHERE NomorBarcode = '" + text + "'", "0");
		if (text2 == "0")
		{
			Util.ShowAlertMessage("Item tidak terdaftar!");
			txtNomorBarcode.Focus();
			Util.SelectText(txtNomorBarcode);
			return;
		}
		DateTime tanggalKirim;
		try
		{
			tanggalKirim = DateTime.Parse(txtTanggalKirim.Text.Trim());
		}
		catch (Exception)
		{
			Util.ShowAlertMessage("Format Tanggal Kirim tidak dapat dikenali!");
			return;
		}
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT COUNT(*) FROM " + TableName + " WHERE Collection_ID = " + text2 + " AND TO_CHAR(Tanggal_Kirim,'YYYY-MM-DD') = '" + tanggalKirim.ToString("yyyy-MM-dd") + "'", "0")))
		{
			Util.ShowAlertMessage("Item dengan Nomor ini sudah ada di tanggal kirim " + tanggalKirim.ToString("yyyy-MM-dd"));
			txtNomorBarcode.Focus();
			Util.SelectText(txtNomorBarcode);
		}
		else
		{
			AddItem(text2, tanggalKirim);
		}
	}

	private void AddItem(string CollectionId, DateTime TanggalKirim)
	{
		string secondValue = Command.ExecScalar("SELECT Branch_Id FROM users WHERE Id = " + UserProfileProvider.Current.Id, "");
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("Collection_Id", CollectionId);
		twoArrayList.Add("User_Id", UserProfileProvider.Current.Id);
		twoArrayList.Add("Branch_Id", secondValue);
		twoArrayList.Add("Tanggal_Kirim", TanggalKirim);
		twoArrayList.Add("CREATEBY", UserProfileProvider.Current.Name);
		twoArrayList.Add("CREATEDATE", DateTime.Now);
		twoArrayList.Add("CREATETERMINAL", Util.GetUserHost(this));
		if (Command.ExecInsertOrUpdate(TableName, twoArrayList, Command.InsertOrUpdate.Insert, null))
		{
			twoArrayList.Clear();
			twoArrayList.Add("Status", "Pengiriman dari Perpustakaan Mitra");
			Command.ExecInsertOrUpdate("collections", twoArrayList, Command.InsertOrUpdate.Update, " WHERE id=" + CollectionId);
			txtNomorBarcode.Text = "";
			txtNomorBarcode.Focus();
			LoadData(1, int.Parse(ddlPage.SelectedValue));
		}
		else
		{
			Util.ShowAlertMessage("Saving Error!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
		}
	}

	protected void txtNomorBarcode_TextChanged(object sender, EventArgs e)
	{
		CheckNomorBarcode();
	}

	protected void txtFilterTanggalKirimAwal_TextChanged(object sender, EventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}

	protected void txtFilterTanggalKirimAkhir_TextChanged(object sender, EventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}
}
