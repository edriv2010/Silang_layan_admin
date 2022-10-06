using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MARC;

public partial class DataUserAdd : Page
{




















	protected string EditID = "";

	protected string BackPage = "DataUser.aspx";

	protected string AddOrEdit = "Tambah";

	protected string TableName = "USERS";

	protected string DeletePage = "DataUserAdd.aspx";

	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAuth.InitPage(Page, new int[1] { Enums.UserAuth.AdminOnly });
		ScriptManager.RegisterClientScriptInclude(this, GetType(), "chosenselect", "js/chosen/chosen.jquery.js");
		ScriptManager.RegisterStartupScript(this, GetType(), "SetChosenSelect", "SetChoosenSelect();", addScriptTags: true);
		if (!Page.IsPostBack)
		{
			if (Page.Request["del"] != null && Page.Request["id"] != null)
			{
				DoDelete();
				return;
			}
			LoadHakAkses();
			cbAktif.Checked = true;
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
			string text = Page.Request["id"].ToString();
			string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
			if (text2 == MyApplication.SuperAdminName)
			{
				Util.ShowAlertMessage("User ini tidak boleh diedit!");
				return;
			}
		}
		else
		{
			lbCreateBy.Text = base.User.Identity.Name;
			lbCreateDate.Text = "";
			lbCreateTerminal.Text = Util.GetUserHost(this);
		}
		if (Page.Request["id"] != null && (MyApplication.SuperAdminName == UserProfileProvider.Current.RoleId || MyApplication.GeneralAdminName == UserProfileProvider.Current.RoleId))
		{
			btResetPassword.Visible = true;
		}
		else
		{
			btResetPassword.Visible = false;
		}
		txtNamaUser.Focus();
		ScriptManager.RegisterStartupScript(this, GetType(), "RegisterScripts", "RegisterScripts();", addScriptTags: true);
	}

	private void LoadHakAkses()
	{
		ddlHakAkses.Items.Clear();
		ddlHakAkses.Items.Add(MyApplication.GeneralAdminName);
	}

	protected void LoadData()
	{
		string sQL = "SELECT * FROM " + TableName + " WHERE id='" + EditID + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			txtNamaUser.Text = dataTable.Rows[0]["UserName"].ToString();
			HiddenUserNameOriginal.Value = dataTable.Rows[0]["UserName"].ToString();
			txtNamaLengkap.Text = dataTable.Rows[0]["FullName"].ToString();
			txtEmail.Text = dataTable.Rows[0]["EmailAddress"].ToString();
			txtNoHp.Text = dataTable.Rows[0]["NoHP"].ToString();
			ddlHakAkses.SelectedValue = dataTable.Rows[0]["HakAkses"].ToString();
			cbAktif.Checked = Util.ConvertToBoolean(dataTable.Rows[0]["IsActive"]);
			cbIsValidatorAuthority.Checked = Util.ConvertToBoolean(dataTable.Rows[0]["ISVALIDATORAUTHORITY"]);
			string text = dataTable.Rows[0]["CREATEBY"].ToString();
			string text2 = (string.IsNullOrEmpty(text) ? "" : Command.ExecScalar("SELECT FullName FROM USERS WHERE UserName='" + text + "'"));
			string text3 = dataTable.Rows[0]["UPDATEBY"].ToString();
			string text4 = (string.IsNullOrEmpty(text3) ? "" : Command.ExecScalar("SELECT FullName FROM USERS WHERE UserName='" + text3 + "'"));
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
		if (!string.IsNullOrEmpty(EditID))
		{
			string text = Page.Request["id"].ToString();
			string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
			if (Page.Request["id"] != null && text2 == MyApplication.SuperAdminName)
			{
				Util.ShowAlertMessage("Data user ini tidak boleh dimodifikasi!");
				return;
			}
		}
		if (txtNamaUser.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Nama User harus diisi!");
			txtNamaUser.Focus();
			return;
		}
		if ((EditID == "" || (EditID != "" && !txtNamaUser.Text.Equals(HiddenUserNameOriginal.Value))) && int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM USERS WHERE Username='" + txtNamaUser.Text + "'", "0")) > 0)
		{
			Util.ShowAlertMessage("Nama User : '" + txtNamaUser.Text + "' sudah ada!");
			txtNamaUser.Focus();
			return;
		}
		if (txtNamaLengkap.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Nama Lengkap harus diisi!");
			txtNamaLengkap.Focus();
			return;
		}
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("UserName", txtNamaUser.Text);
		twoArrayList.Add("FullName", txtNamaLengkap.Text);
		twoArrayList.Add("EmailAddress", txtEmail.Text);
		twoArrayList.Add("NoHp", txtNoHp.Text);
		twoArrayList.Add("HakAkses", ddlHakAkses.SelectedItem.Text);
		twoArrayList.Add("IsActive", cbAktif.Checked ? 1 : 0);
		twoArrayList.Add("ISVALIDATORAUTHORITY", cbIsValidatorAuthority.Checked ? 1 : 0);
		if (EditID == "")
		{
			twoArrayList.Add("Password", (txtNamaUser.Text + string.Empty).EncryptSHA1().ToString().ToUpper());
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
				Util.ShowAlertMessage("Data User telah ditambahkan dan disimpan", "Sukses", "", "fa fa-thumbs-up");
				ClearSaveControls();
			}
			else
			{
				Util.ShowAlertMessage("Saving Error!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
			}
			return;
		}
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM " + TableName + " WHERE id=" + EditID);
		string text3 = "";
		if (dataTable.Rows.Count > 0)
		{
			if (dataTable.Rows[0]["UserName"].ToString() != txtNamaUser.Text)
			{
				string text4 = text3;
				text3 = text4 + "Nama User : " + dataTable.Rows[0]["UserName"].ToString() + " --> " + txtNamaUser.Text + "<br />";
			}
			if (dataTable.Rows[0]["FullName"].ToString() != txtNamaLengkap.Text)
			{
				string text4 = text3;
				text3 = text4 + "Nama Lengkap : " + dataTable.Rows[0]["FullName"].ToString() + " --> " + txtNamaLengkap.Text + "<br />";
			}
			if (dataTable.Rows[0]["EmailAddress"].ToString() != txtEmail.Text)
			{
				string text4 = text3;
				text3 = text4 + "Email : " + dataTable.Rows[0]["EmailAddress"].ToString() + " --> " + txtEmail.Text + "<br />";
			}
			if (dataTable.Rows[0]["NoHp"].ToString() != txtNoHp.Text)
			{
				string text4 = text3;
				text3 = text4 + "No. Handphone : " + dataTable.Rows[0]["NoHp"].ToString() + " --> " + txtNoHp.Text + "<br />";
			}
			if (dataTable.Rows[0]["HakAkses"].ToString() != ddlHakAkses.SelectedItem.Text)
			{
				string text4 = text3;
				text3 = text4 + "Hak Akses : " + dataTable.Rows[0]["HakAkses"].ToString() + "' --> " + ddlHakAkses.SelectedItem.Text + "<br />";
			}
			if (dataTable.Rows[0]["IsActive"].ToString() != (cbAktif.Checked ? "1" : "0"))
			{
				string text4 = text3;
				text3 = text4 + "Aktif : " + dataTable.Rows[0]["IsActive"].ToString() + " --> " + (cbAktif.Checked ? "True" : "False") + "<br />";
			}
			if (dataTable.Rows[0]["ISVALIDATORAUTHORITY"].ToString() != (cbIsValidatorAuthority.Checked ? "1" : "0"))
			{
				string text4 = text3;
				text3 = text4 + "Validator Authority : " + dataTable.Rows[0]["ISVALIDATORAUTHORITY"].ToString() + " --> " + (cbIsValidatorAuthority.Checked ? "True" : "False") + "<br />";
			}
		}
		if (Command.ExecInsertOrUpdate(TableName, twoArrayList, Command.InsertOrUpdate.Update, " WHERE id=" + EditID))
		{
			DataUIProvider.SaveHistory(this, DataUIProvider.HistoryAction.Edit, TableName, EditID, text3);
			Util.ShowAlertMessage("Data User telah disimpan", "Sukses", "", "fa fa-thumbs-up");
		}
		else
		{
			Util.ShowAlertMessage("Saving Error!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
		}
	}

	protected void ClearSaveControls()
	{
		Util.ClearControls(Controls);
		txtNamaUser.Focus();
	}

	private void DoDelete()
	{
		string text = Page.Request["id"].ToString();
		string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
		if (Page.Request["id"] != null && text2 == MyApplication.SuperAdminName)
		{
			base.Response.Clear();
			base.Response.Write("Data user ini tidak boleh dihapus!");
			base.Response.End();
		}
		else if (DataUIProvider.DeleteData(TableName, text))
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

	private void ResetPassword()
	{
		string text = Page.Request["id"].ToString();
		string text2 = Command.ExecScalar("SELECT HakAkses FROM USERS WHERE ID=" + text);
		if (Page.Request["id"] != null && text2 == MyApplication.SuperAdminName)
		{
			Util.ShowAlertMessage("Data user ini tidak boleh dimodifikasi!");
			return;
		}
		EditID = Page.Request["id"].ToString();
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("Password", (txtNamaUser.Text + string.Empty).EncryptSHA1().ToString().ToUpper());
		if (Command.ExecInsertOrUpdate(TableName, twoArrayList, Command.InsertOrUpdate.Update, " WHERE id=" + EditID))
		{
			Util.ShowAlertMessage("Password telah di-reset");
		}
	}

	protected void btResetPassword_Click(object sender, EventArgs e)
	{
		ResetPassword();
	}
}
