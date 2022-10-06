using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public class Login : Page
{
	public class RecaptchaApiResponse
	{
		public bool Success;

		public List<string> ErrorCodes;
	}

	public class ReCaptchaClass
	{
		private string m_Success;

		private List<string> m_ErrorCodes;

		[JsonProperty("error-codes")]
		public List<string> ErrorCodes
		{
			get
			{
				return m_ErrorCodes;
			}
			set
			{
				m_ErrorCodes = value;
			}
		}

		[JsonProperty("success")]
		public string Success
		{
			get
			{
				return m_Success;
			}
			set
			{
				m_Success = value;
			}
		}

		public static string Validate(string EncodedResponse)
		{
			WebClient webClient = new WebClient();
			string arg = "6LeY4yEUAAAAAOfbsaMv0BIZgLK7_xzN6km9GSjW";
			string value = webClient.DownloadString("https://www.google.com/recaptcha/api/siteverify?secret={arg}&response={EncodedResponse}");
			return JsonConvert.DeserializeObject<ReCaptchaClass>(value).Success;
		}
	}

	public class UserProfile
	{
		public string Id;

		public string Name;

		public string FullName;

		public string RoleId;

		public string BranchId;
	}

	protected TextBox txtUserName;

	protected TextBox txtPassword;

	protected Button btSubmit;

	protected HtmlTableRow trCapctha;

	private bool IsAllInputValid()
	{
		bool result;
		if (txtUserName.Text.Trim().Length == 0)
		{
			Util.ShowAlertMessage("Nama Pengguna tidak boleh kosong!");
			txtUserName.Focus();
			result = false;
		}
		else if (txtPassword.Text.Trim().Length != 0)
		{
			if (!MyApplication.IsDebug && ((!(ReCaptchaClass.Validate(base.Request.Form["g-Recaptcha-Response"]) == "True")) ? true : false))
			{
				Util.ShowAlertMessage("Cek I'm not a robot");
				return false;
			}
			result = true;
		}
		else
		{
			Util.ShowAlertMessage("Kata Sandi tidak boleh kosong!");
			txtPassword.Focus();
			result = false;
		}
		return result;
	}

	protected bool IsLoginValid()
	{
		try
		{
			if (IsAllInputValid())
			{
				Connection.SetConnection();
				TwoArrayList twoArrayList = new TwoArrayList();
				twoArrayList.Clear();
				twoArrayList.Add("username", txtUserName.Text);
				twoArrayList.Add("password", Util.EncryptSHA1(txtPassword.Text).ToUpper());
				string[] array = new string[5]
				{
					"SELECT ID FROM USERS WHERE UserName=",
					Connection.ParameterSymbol,
					"username AND UPPER(Password) = ",
					Connection.ParameterSymbol,
					"password AND IsActive=1"
				};
				int num = int.Parse(Command.ExecScalar(twoArrayList, string.Concat(array), "0"));
				if (num != 0)
				{
					DataTable dataTable = Command.ExecDataAdapter("SELECT ID,UserName,FullName,Role_Id,Branch_Id,IsActive FROM USERS WHERE ID=" + num);
					if (dataTable.Rows.Count <= 0)
					{
						Util.ShowAlertMessage("Nama Pengguna atau Kata Sandi salah!");
						return false;
					}
					FormsAuthentication.SetAuthCookie(txtUserName.Text, createPersistentCookie: true);
					string text = txtUserName.Text;
					DateTime now = DateTime.Now;
					DateTime expiration = DateTime.Now.Add(FormsAuthentication.Timeout);
					UserProfile userProfile = new UserProfile();
					userProfile.Id = string.Concat(dataTable.Rows[0]["ID"]);
					userProfile.Name = string.Concat(dataTable.Rows[0]["UserName"]);
					userProfile.FullName = string.Concat(dataTable.Rows[0]["FullName"]);
					userProfile.RoleId = string.Concat(dataTable.Rows[0]["Role_Id"]);
					userProfile.BranchId = string.Concat(dataTable.Rows[0]["Branch_Id"]);
					UserProfile value = userProfile;
					FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, text, now, expiration, true, JsonConvert.SerializeObject(value));
					base.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
					return true;
				}
				Util.ShowAlertMessage("Nama Pengguna atau Kata Sandi salah!");
				return false;
			}
			return false;
		}
		catch (Exception ex)
		{
			Util.ShowAlertMessage(ex.Message);
			return false;
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		CIP.WebUserLogEntry(this);
		bool flag = UserProfileProvider.Current != null;
		if (base.Request.IsAuthenticated || flag)
		{
			RedirectPage();
		}
		if (MyApplication.IsDebug)
		{
			trCapctha.Visible = false;
		}
		if (!Page.IsPostBack)
		{
			Connection.SetConnection();
			if (Command.TestConnection())
			{
				MyApplication.InitApplication();
				txtUserName.Focus();
			}
			else
			{
				Util.ShowAlertMessage("Tidak dapat melakukan koneksi ke server!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
			}
			return;
		}
		Connection.SetConnection();
		if (!Command.TestConnection())
		{
			Util.ShowAlertMessage("Tidak dapat melakukan koneksi ke server!\n\n" + Session[MySession.CurrentErrorMessage].ToString());
		}
		else if (IsLoginValid())
		{
			RedirectPage();
		}
	}

	private void RedirectPage()
	{
		string text = (base.Request.QueryString["returnUrl"] ?? "").Trim();
		base.Response.Redirect((text.Length > 0) ? text : "Default.aspx");
	}
}
