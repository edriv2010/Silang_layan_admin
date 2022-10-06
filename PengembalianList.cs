using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public class PengembalianList : Page
{
	protected string TableName = "collectionloanitems";

	private string OrderFields = "ActualReturn DESC";

	private string[] KriteriaPencarian = new string[10] { "members.memberno|No. Anggota", "members.FullName|Nama Anggota", "NomorBarcode|Item ID", "collectionloanitems.Title|Judul", "collectionloanitems.Author|Pengarang", "collectionloanitems.Publisher|Penerbit", "LoanDate|Tgl Pinjam", "DueDate|Tgl Kembali", "ActualReturn|Tgl Dikembalikan", "LateDays|Terlambat" };

	protected string PageAddOrEditURL = "PengembalianAdd.aspx";

	protected string PageTitle = "Sirkulasi ► Daftar Pengembalian";

	private string SQLJOIN = " LEFT JOIN pelanggaran ON CollectionLoan_id=NoPinjam AND Collection_id=NoItem INNER JOIN members ON collectionloanitems.Member_id = members.ID INNER JOIN collections ON collectionloanitems.collection_id = collections.id";

	private string SQLAND = "and collectionloanitems.LoanStatus = 'Return' and collectionloanitems.Member_id IS NOT NULL ";

	private string ShowingFields = "collectionloanitems.id,CollectionLoan_id as idpinjam,collections.id as ItemID,members.id as MemberID,members.MemberNo as MemberNo,members.FullName as MemberName,NomorBarcode,collectionloanitems.Title,collectionloanitems.Author,collectionloanitems.Publisher, LoanDate, DueDate, ActualReturn, LateDays,(SELECT 'Sudah Kirim' FROM pm_kirim WHERE pm_kirim.Collection_Id = collectionloanitems.Collection_Id AND ROWNUM <=1) as StatusKirim";

	private string ExportFields = "collectionloanitems.id,members.MemberNo as \"No Anggota\", members.FullName as \"Nama Anggota\",NomorBarcode as \"Item ID\", collectionloanitems.Title as Judul, collectionloanitems.Author as Pengarang,collectionloanitems.Publisher as Penerbit, LoanDate as \"Tgl Pinjam\", DueDate as \"Tgl Harus Kembali\",ActualReturn as \"Tgl Dikembalikan\", LateDays as Terlambat,(SELECT 'Sudah Kirim' FROM pm_kirim WHERE pm_kirim.Collection_Id = collectionloanitems.Collection_Id AND ROWNUM <=1) as StatusKirim";

	protected int iPage = 0;

	protected bool IsAlreadyLoadData = false;

	protected string FieldPencarian = "";

	protected string KataKunci = "";

	private string UserBranchId = "";

	protected ScriptManager ScriptManager1;

	protected Label lbPageTitle;

	protected UpdatePanel UpdatePanel1;

	protected UpdateProgress UpdateProgress1;

	protected Button btKirim;

	protected Label lbPage;

	protected DropDownList ddlPage;

	protected DropDownList ddlKriteria;

	protected TextBox txtKataKunci;

	protected ImageButton ibPencarian;

	protected DataGrid dgData;

	protected HtmlGenericControl divDataTidakTersedia;

	protected Label lbPage0;

	protected HtmlGenericControl divPaging;

	protected Label lbPageNumber;

	protected Repeater RepeaterPage;

	protected Label lbGoToPage;

	protected TextBox txtPageGoTo;

	protected UpdateProgress UpdateProgress2;

	protected Label lbJumlahCantuman;

	protected HtmlInputHidden HiddenCheckeds;

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
		if (!Page.IsPostBack)
		{
		}
		ScriptManager.RegisterClientScriptInclude(this, GetType(), "chosenselect", "js/chosen/chosen.jquery.js");
		ScriptManager.RegisterStartupScript(this, GetType(), "SetChosenSelect", "SetChoosenSelect();", addScriptTags: true);
		ScriptManager.RegisterStartupScript(this, GetType(), "RegisterCheckbox", "RegisterCheckbox();", addScriptTags: true);
		UserBranchId = Command.ExecScalar("SELECT Branch_Id FROM users WHERE Id = " + UserProfileProvider.Current.Id, "");
		Session.Remove("TempPinjamPengembalian");
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
		Exporter1.ShowingFields = ExportFields;
	}

	private void GetFilters()
	{
		string branchId = UserProfileProvider.Current.BranchId;
		if (!string.IsNullOrEmpty(branchId) && branchId != "0")
		{
			SQLAND = SQLAND + " AND PengembalianBranch_Id = " + branchId;
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
		DataTable data = DataUIProvider.LoadData(PageNumber, MaxItemPerPage, FieldPencarian, KataKunci, this, dgData, TableName, OrderFields, SQLJOIN, SQLAND, ShowingFields, Load_PageNumber, lbPage, lbPage0, lbJumlahCantuman).Data;
		if (ViewState["ListItem"] != null)
		{
			string[] array = (string[])ViewState["ListItem"];
			string[] array2 = array;
			foreach (string text in array2)
			{
				for (int j = 0; j < data.Rows.Count; j++)
				{
					if (data.Rows[j]["ID"].ToString() == text)
					{
						((CheckBox)dgData.Items[j].Cells[2].FindControl("cbCheck")).Checked = true;
						break;
					}
				}
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
		if (e.Answer == MessageBoxUsc_MsgBoxUsc.enmAnswer.OK)
		{
			dgData.EditItemIndex = -1;
			DataUIProvider.DeleteData(TableName, Page.Session[MySession.CurrentIDData].ToString());
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

	protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
	{
		LoadData(1, int.Parse(ddlPage.SelectedValue));
	}

	protected void btKirim_Click(object sender, EventArgs e)
	{
		string value = HiddenCheckeds.Value;
		string[] array = value.Split(char.Parse("|"));
		ViewState.Add("ListItem", array);
		TwoArrayList twoArrayList = new TwoArrayList();
		if (array.Length == 0)
		{
			Util.ShowAlertMessage("Tidak ada item yang dipilih");
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		string[] array2 = array;
		foreach (string text in array2)
		{
			num++;
			string text2 = Command.ExecScalar("SELECT Collection_Id FROM COLLECTIONLOANITEMS WHERE Id = " + text);
			if (!string.IsNullOrEmpty(text2))
			{
				string value2 = AddItem(num, text2, DateTime.Now);
				stringBuilder.AppendLine(value2);
			}
		}
		LoadData(1, int.Parse(ddlPage.SelectedValue));
		Util.ShowAlertMessage(stringBuilder.ToString());
	}

	private string AddItem(int Number, string CollectionId, DateTime TanggalKirim)
	{
		string text = "";
		string text2 = Command.ExecScalar("SELECT NomorBarcode FROM collections WHERE Id = " + CollectionId, "0");
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT COUNT(*) FROM pm_kirim WHERE Collection_ID = " + CollectionId + " AND TO_CHAR(Tanggal_Kirim,'YYYY-MM-DD') = '" + TanggalKirim.ToString("yyyy-MM-dd") + "'", "0")))
		{
			return Number + ". Item dengan Nomor Barcode " + text2 + " sudah ada di tanggal kirim " + TanggalKirim.ToString("yyyy-MM-dd") + "<br/>";
		}
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("Collection_Id", CollectionId);
		twoArrayList.Add("User_Id", UserProfileProvider.Current.Id);
		twoArrayList.Add("Branch_Id", UserBranchId);
		twoArrayList.Add("Tanggal_Kirim", TanggalKirim);
		twoArrayList.Add("CREATEBY", UserProfileProvider.Current.Name);
		twoArrayList.Add("CREATEDATE", DateTime.Now);
		twoArrayList.Add("CREATETERMINAL", Util.GetUserHost(this));
		if (Command.ExecInsertOrUpdate("pm_kirim", twoArrayList, Command.InsertOrUpdate.Insert, null))
		{
			twoArrayList.Clear();
			twoArrayList.Add("Status", "Pengiriman dari Perpustakaan Mitra");
			Command.ExecInsertOrUpdate("collections", twoArrayList, Command.InsertOrUpdate.Update, " WHERE id=" + CollectionId);
			return Number + ". Data pengiriman telah ditambahkan untuk Nomor Barcode " + text2 + "<br/>";
		}
		return Number + ". Simpan Error untuk Nomor Barcode " + text2 + "!\n\n" + Session[MySession.CurrentErrorMessage].ToString() + "<br/>";
	}
}
