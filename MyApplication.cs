using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

public class MyApplication
{
	public static string ApplicationName = "SILANG_LAYAN";

	public static string ApplicationVersion = "1.0";

	public static string ClientName = "Perpustakaan Nasional RI";

	public static string VendorName = "PT. Smarthub Technologies";

	public static string PublishYear = "2021";

	public static string LogPath = "C:\\SILANG_LAYAN_LOG\\";

	public static string LogExt = "log";

	public static string UploadedFolder = "Attachment";

	public static string XMLFolder = "xmldata";

	public static string LoginPage = "Default.aspx";

	public static string MainPage = "Default.aspx";

	public static int MaxItemPerPage = 10;

	public static int MaxPageNumberDisplayed = 10;

	public static bool IsDebug = false;

	public static int ApplicationID = -1;

	public static string BranchCode = "INLIS";

	public static string BIBIDCode = "0010-";

	public static int BranchID = 0;

	public static string Menus = "";

	public static string SuperAdminID = "1";

	public static string SuperAdminName = "SuperAdmin";

	public static string GeneralAdminID = "6";

	public static string GeneralAdminName = "Administrator";

	public static bool IsCaseSencitive = true;

	public static bool IsSaveIPLog = false;

	public static string CurrentErrorMessage = "";

	public static string SavedKeyApplicationSetting = "Gpd20EP7dAFY2MPB7btkneE/nJfAk4gzmLvne7PJwmM=";

	public static string SavedIVApplicationSetting = "N5wGPcqyOFU2Vxpk2TIcyw==";

	public static bool IsLucene = false;

	public static bool IsRDA = false;

	public static bool IsBIBIDAvailable = false;

	public static int TerbitanBerkalaWorksheetID = 13;

	public static int PeminjamanJumlahFamilyMemberMin = 3;

	public static void InitApplication()
	{
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
        if (page != null)
        {
            page.Session.Clear();
        }
        /*
		if (HttpContext.Current.Handler is Page page)
		{
			page.Session.Clear();
		}*/
		Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~/web.config");
		SystemWebSectionGroup systemWebSectionGroup = (SystemWebSectionGroup)configuration.GetSectionGroup("system.web");
		IsDebug = systemWebSectionGroup.Compilation.Debug;
		GetIsCaseSencitiveValue();
		Connection.SetConnection();
		BranchCode = GetBranchCode();
		BranchID = GetBranchID(BranchCode);
	}

	public static string GetBranchCode()
	{
		return ConfigurationManager.AppSettings["BranchCode"].ToString();
	}

	public static int GetBranchID(string BranchCode)
	{
		return int.Parse(Command.ExecScalar("SELECT id from branchs WHERE Code='" + BranchCode + "'", "0"));
	}

	public static void GetIsCaseSencitiveValue()
	{
		try
		{
			string text = ConfigurationManager.AppSettings["IsCaseSencitive"].ToString();
			if (text == "1")
			{
				IsCaseSencitive = true;
			}
			else
			{
				IsCaseSencitive = false;
			}
		}
		catch
		{
			Util.ShowAlertMessage("Cannot find IsCaseSencitive key on appSettings of web.config!");
		}
	}
}
