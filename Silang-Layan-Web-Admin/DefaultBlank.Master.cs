using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class _DefaultBlankMaster : MasterPage
{
	public class UserProfile
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string FullName { get; set; }

		public string RoleId { get; set; }

		public string GateID { get; set; }

		public string GateDesc { get; set; }

		public string LocationID { get; set; }
	}




	protected void Page_Load(object sender, EventArgs e)
	{
		if (UserProfileProvider.Current == null)
		{
			FormsAuthentication.SetAuthCookie("deni", createPersistentCookie: true);
			FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "deni", DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), true, JsonConvert.SerializeObject(new UserProfile
			{
				Id = "1",
				Name = "deni",
				FullName = "Deni Syahreza",
				RoleId = "1",
				LocationID = "1",
				GateID = "1",
				GateDesc = "1"
			}));
			base.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
		}
	}
}
