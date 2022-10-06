using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _DefaultMaster : MasterPage
{
	protected string UserFullName = "";












	protected void Page_Load(object sender, EventArgs e)
	{
		if (UserProfileProvider.Current != null)
		{
			UserFullName = UserProfileProvider.Current.FullName;
		}
		else
		{
			LogoutProvider.DoLogout();
		}
	}
}
