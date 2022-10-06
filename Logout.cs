using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public class Logout : Page
{
	protected HtmlForm form1;

	protected void Page_Load(object sender, EventArgs e)
	{
		LogoutProvider.DoLogout();
	}
}
