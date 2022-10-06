using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ChangePassword : Page
{







	protected void btSave_Click(object sender, EventArgs e)
	{
		if (IsAllInputValid())
		{
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("Password", Util.EncryptSHA1(txtNewPassword.Text).ToUpper());
			if (!Command.ExecInsertOrUpdate("USERS", twoArrayList, Command.InsertOrUpdate.Update, " WHERE ID = " + UserProfileProvider.Current.Id))
			{
				Util.ShowAlertMessage("Gagal merubah password!", "Peringatan");
			}
			else
			{
				Util.ShowAlertMessage("Password telah diubah", "Sukses", "", "fa fa-thumbs-up");
			}
		}
	}

	private bool IsAllInputValid()
	{
		if (txtOldPassword.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Masukkan password lama!");
			txtOldPassword.Focus();
			return false;
		}
		if (txtNewPassword.Text.Trim().Length != 0)
		{
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("ID", UserProfileProvider.Current.Id);
			string text = Command.ExecScalar(twoArrayList, "SELECT password FROM USERS WHERE ID = " + Connection.ParameterSymbol + "ID", "");
			twoArrayList.Clear();
			twoArrayList.Add("Password", txtOldPassword.Text);
			string text2 = Util.EncryptSHA1(txtOldPassword.Text).ToUpper();
			if (!(text.ToUpper() != text2.ToUpper()))
			{
				return true;
			}
			Util.ShowAlertMessage("Password lama tidak sama!");
			txtOldPassword.Focus();
			return false;
		}
		Util.ShowAlertMessage("Masukkan password baru!", "Peringatan");
		txtNewPassword.Focus();
		return false;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAuth.InitPage(Page, new int[1] { Enums.UserAuth.AllUser });
		ScriptManager.RegisterStartupScript(this, GetType(), "RegisterScripts", "RegisterScripts();", addScriptTags: true);
		if (Page.IsPostBack)
		{
		}
	}
}
