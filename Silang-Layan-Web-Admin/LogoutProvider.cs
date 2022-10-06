using System.Web;
using System.Web.Security;

public static class LogoutProvider
{
	public static void DoLogout()
	{
		HttpContext.Current.Session.Clear();
		FormsAuthentication.SignOut();
		MyApplication.Menus = "";
		HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
	}
}
