using System.IO;
using System.Web.UI;

public class LoginAuth
{
	public static void InitPage(Page Page, int[] AuthList)
	{
		string masterPageFile = Page.MasterPageFile;
		Page.Session.Add(MySession.CurrentPage, Path.GetFileName(Page.Request.Path) + masterPageFile);
		CheckLoginAuth(Page, AuthList);
	}

	public static void InitPage(Page Page, int[] AuthList, string PageURL)
	{
		Page.Session.Add(MySession.CurrentPage, PageURL);
		CheckLoginAuth(Page, AuthList);
	}

	public static void CheckLoginAuth(Page Page, int[] AuthList)
	{
		Connection.SetConnection();
		bool flag = false;
		bool flag2 = false;
		if (AuthList == null)
		{
			return;
		}
		for (int i = 0; i <= AuthList.GetUpperBound(0); i++)
		{
			if (AuthList[i].ToString() == Enums.UserAuth.ByPassAccess.ToString())
			{
				flag = true;
				return;
			}
			if (AuthList[i].ToString() == Enums.UserAuth.AllUser.ToString())
			{
				if (UserProfileProvider.Current == null)
				{
					flag = false;
					LogoutProvider.DoLogout();
					return;
				}
				if (IsLoginAuth(Page))
				{
					flag = true;
					return;
				}
				flag = false;
			}
			if (AuthList[i].ToString() == Enums.UserAuth.BypassPageAuth.ToString())
			{
				flag2 = true;
			}
			if (AuthList[i].ToString() == Enums.UserAuth.AdminOnly.ToString())
			{
				if (UserProfileProvider.Current == null)
				{
					flag = false;
					LogoutProvider.DoLogout();
				}
				else if (UserProfileProvider.Current.RoleId == MyApplication.SuperAdminID || UserProfileProvider.Current.RoleId == MyApplication.GeneralAdminID)
				{
					flag = true;
				}
				else
				{
					flag = false;
					LogoutProvider.DoLogout();
				}
				return;
			}
		}
		if (UserProfileProvider.Current.RoleId == MyApplication.SuperAdminName)
		{
			flag = true;
			return;
		}
		string fileName = Path.GetFileName(Page.Request.Path);
		if (!flag2)
		{
			if (fileName == MyApplication.MainPage)
			{
				flag = true;
				return;
			}
			flag = true;
		}
		else if (AuthList != null)
		{
			for (int i = 0; i <= AuthList.GetUpperBound(0); i++)
			{
				if (Page.Session[MySession.CurrentPage].ToString() == AuthList[i].ToString())
				{
					flag = true;
					return;
				}
			}
		}
		if (!flag)
		{
			LogoutProvider.DoLogout();
		}
	}

	public static bool IsLoginAuth(Page Page)
	{
		return Page.Request.IsAuthenticated;
	}
}
