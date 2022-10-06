using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DataPerpustakaanAdd : Page
{
	protected string EditID = "";

	protected string BackPage = "DataPerpustakaan.aspx";

	protected string AddOrEdit = "Tambah";

	protected string TableName = "branchs";

	protected string DeletePage = "DataPerpustakaanAdd.aspx";

















	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAuth.InitPage(Page, null);
		if (!Page.IsPostBack)
		{
			if (Page.Request["del"] != null && Page.Request["id"] != null)
			{
				DoDelete();
				return;
			}
			if (Page.Request["edit"] != null && Page.Request["id"] != null)
			{
				EditID = Page.Request["id"].ToString();
				AddOrEdit = "Edit";
				LoadData();
			}
		}
		if (Page.Request["edit"] != null && Page.Request["id"] != null)
		{
			EditID = Page.Request["id"].ToString();
			AddOrEdit = "Edit";
			trHistoryData.Visible = true;
		}
		else
		{
			lbCreateBy.Text = base.User.Identity.Name;
			lbCreateDate.Text = "";
			lbCreateTerminal.Text = Util.GetUserHost(this);
		}
		txtKode.Focus();
	}

	protected void LoadData()
	{
		string sQL = "SELECT * FROM " + TableName + " WHERE id='" + EditID + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			txtKode.Text = dataTable.Rows[0]["Code"].ToString();
			HiddenCodeOriginal.Value = dataTable.Rows[0]["Code"].ToString();
			txtNama.Text = dataTable.Rows[0]["Name"].ToString();
			HiddenNameOriginal.Value = dataTable.Rows[0]["Name"].ToString();
			cbIsServiceReg.Checked = Util.ConvertToBoolean(dataTable.Rows[0]["IsServiceReg"]);
			string text = dataTable.Rows[0]["CREATEBY"].ToString();
			string text2 = (string.IsNullOrEmpty(text) ? "" : Command.ExecScalar("SELECT FullName FROM Users WHERE UserName='" + text + "'"));
			string text3 = dataTable.Rows[0]["UPDATEBY"].ToString();
			string text4 = (string.IsNullOrEmpty(text3) ? "" : Command.ExecScalar("SELECT FullName FROM Users WHERE UserName='" + text3 + "'"));
			lbCreateBy.Text = text + (string.IsNullOrEmpty(text2) ? "" : (" (" + text2 + ")"));
			lbCreateDate.Text = (string.IsNullOrEmpty(dataTable.Rows[0]["CREATEDATE"].ToString()) ? "" : string.Format("{0:dd MMMM yyyy, HH:mm:ss}", (DateTime)dataTable.Rows[0]["CREATEDATE"]));
			lbCreateTerminal.Text = dataTable.Rows[0]["CREATETERMINAL"].ToString();
			lbUpdateBy.Text = text3 + (string.IsNullOrEmpty(text4) ? "" : (" (" + text4 + ")"));
			lbUpdateDate.Text = (string.IsNullOrEmpty(dataTable.Rows[0]["UPDATEDATE"].ToString()) ? "" : string.Format("{0:dd MMMM yyyy, HH:mm:ss}", (DateTime)dataTable.Rows[0]["UPDATEDATE"]));
			lbUpdateTerminal.Text = dataTable.Rows[0]["UPDATETERMINAL"].ToString();
		}
	}

	protected void btSave_Click(object sender, EventArgs e)
	{
		if (txtKode.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Kode Perpustakaan harus diisi!");
			txtKode.Focus();
			return;
		}
		if ((EditID == "" || (EditID != "" && !txtKode.Text.Equals(HiddenCodeOriginal.Value))) && int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM branchs WHERE UPPER(Code) = '" + txtKode.Text.ToUpper() + "'", "0")) > 0)
		{
			Util.ShowAlertMessage("Kode Perpustakaan : '" + txtKode.Text + "' sudah ada!");
			txtKode.Focus();
			return;
		}
		if (txtNama.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Nama Perpustakaan harus diisi!");
			txtNama.Focus();
			return;
		}
		if ((EditID == "" || (EditID != "" && !txtNama.Text.Equals(HiddenNameOriginal.Value))) && int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM branchs WHERE UPPER(Name) = '" + txtNama.Text.ToUpper() + "'", "0")) > 0)
		{
			Util.ShowAlertMessage("Nama Perpustakaan : '" + txtNama.Text + "' sudah ada!");
			txtNama.Focus();
			return;
		}
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("Code", txtKode.Text);
		twoArrayList.Add("Name", txtNama.Text);
		twoArrayList.Add("IsServiceReg", cbIsServiceReg.Checked ? 1 : 0);
		if (EditID == "")
		{
			twoArrayList.Add("IsDelete", 0);
			twoArrayList.Add("CREATEBY", base.User.Identity.Name);
			twoArrayList.Add("CREATEDATE", DateTime.Now);
			twoArrayList.Add("CREATETERMINAL", Util.GetUserHost(this));
		}
		twoArrayList.Add("UPDATEBY", base.User.Identity.Name);
		twoArrayList.Add("UPDATEDATE", DateTime.Now);
		twoArrayList.Add("UPDATETERMINAL", Util.GetUserHost(this));
		if (EditID == "")
		{
			if (Command.ExecInsertOrUpdate(TableName, twoArrayList, Command.InsertOrUpdate.Insert, null))
			{
				Util.ShowAlertMessage("Data Perpustakaan telah ditambahkan dan disimpan");
				ClearSaveControls();
			}
			else
			{
				Util.ShowAlertMessage("Saving Error!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
			}
			return;
		}
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM " + TableName + " WHERE id=" + EditID);
		string text = "";
		if (dataTable.Rows.Count > 0)
		{
			if (dataTable.Rows[0]["Code"].ToString() != txtKode.Text)
			{
				string text2 = text;
				text = text2 + "Kode : " + dataTable.Rows[0]["Code"].ToString() + " --> " + txtKode.Text + "<br />";
			}
			if (dataTable.Rows[0]["Name"].ToString() != txtNama.Text)
			{
				string text2 = text;
				text = text2 + "Nama Perpustakaan : " + dataTable.Rows[0]["Name"].ToString() + " --> " + txtNama.Text + "<br />";
			}
			if (Util.ConvertToBoolean(dataTable.Rows[0]["IsServiceReg"].ToString()) != cbIsServiceReg.Checked)
			{
				string text2 = text;
				text = text2 + "Service Diregistrasi : " + dataTable.Rows[0]["IsServiceReg"].ToString() + " --> " + (cbIsServiceReg.Checked ? "True" : "False") + "<br />";
			}
		}
		if (Command.ExecInsertOrUpdate(TableName, twoArrayList, Command.InsertOrUpdate.Update, " WHERE id=" + EditID))
		{
			DataUIProvider.SaveHistory(this, DataUIProvider.HistoryAction.Edit, TableName, EditID, text);
			Util.ShowAlertMessage("Data Perpustakaan telah disimpan");
		}
		else
		{
			Util.ShowAlertMessage("Saving Error!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
		}
	}

	protected void ClearSaveControls()
	{
		txtKode.Text = "";
		txtNama.Text = "";
		txtKode.Focus();
	}

	private void DoDelete()
	{
		if (DataUIProvider.DeleteData(TableName, Page.Request["id"].ToString()))
		{
			base.Response.Clear();
			base.Response.Write("OK");
			base.Response.End();
		}
		else
		{
			base.Response.Clear();
			base.Response.Write(Session[MySession.CurrentErrorMessage].ToString());
			base.Response.End();
		}
	}
}
