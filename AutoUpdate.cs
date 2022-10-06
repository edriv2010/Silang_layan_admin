using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;

public class AutoUpdate
{
	public static string ConfigAutoUpdateFile = "AutoUpdate.config";

	public static string ConfigLastUpdateFile = "LastUpdate.config";

	public static string ConfigScriptFile = "Scripts.txt";

	public static string ConfigCurrentVersionFile = "CurrentVersion.config";

	public static string LocalUpdateConfigFolder = "~/config/";

	public static void DoUpdate()
	{
		if (HttpContext.Current == null)
		{
			return;
		}
		Page page = HttpContext.Current.Handler as Page;
		WebClient webClient = new WebClient();
		string text = page.MapPath(LocalUpdateConfigFolder);
		string path = text + "\\" + ConfigAutoUpdateFile;
		string text2 = text + "DownloadInfo.txt";
		string text3 = "";
		string text4 = "";
		string text5 = page.MapPath("~") + "/";
		try
		{
			string text6 = "";
			if (File.Exists(path))
			{
				text6 = File.ReadAllText(path);
			}
			Util.CreateLog("AUTO UPDATE : Downloading file starting for getting latest version information : " + text6);
			webClient.DownloadFile(text6, text2);
			Util.CreateLog("AUTO UPDATE : Downloading file ending for getting latest version information : " + text6 + ". And saving at : " + text2);
			//text3 = File.ReadAllText(text2);
             //int i1 = text3.LastIndexOf("/")
			text4 = text3[text3.LastIndexOf("/")] + "/";
            //text4 = text3[..text3.LastIndexOf("/")] + "/";
            //text4 = text3[..text3.LastIndexOf("/")] + "/";
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			Util.CreateLog("AUTO UPDATE : Aquired information for current update url : " + text3);
		}
		catch (Exception ex)
		{
			Util.CreateLog("AUTO UPDATE : " + ex.Message);
			Util.ShowAlertMessage("AUTO UPDATE : " + ex.Message);
			return;
		}
		try
		{
			Util.CreateLog("AUTO UPDATE : Downloading file starting for getting update list information : " + text3);
			webClient.DownloadFile(text3, text2);
			text3 = File.ReadAllText(text2);
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			Util.CreateLog("AUTO UPDATE : Aquired information for current update file list url : " + text3);
		}
		catch (Exception ex)
		{
			Util.CreateLog("AUTO UPDATE : " + ex.Message);
			Util.ShowAlertMessage("AUTO UPDATE : " + ex.Message);
			return;
		}
		string[] array = null;
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		try
		{
			array = text3.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Trim().Split(char.Parse(";"));
				string text7 = array2[0];
				string value = array2[1];
				if (text7.Length > 0)
				{
					arrayList.Add(text7);
					arrayList2.Add(value);
					Util.CreateLog("AUTO UPDATE : Downloading new update file : " + text4 + text7 + ".new");
					webClient.DownloadFile(text4 + text7, text5 + text7 + ".new");
				}
			}
			try
			{
				for (int i = 0; i < array.Length; i++)
				{
					string text8 = page.MapPath(arrayList[i].ToString()) + ".new";
					string path2 = page.MapPath(arrayList2[i].ToString());
					if (!Directory.Exists(path2))
					{
						Util.CreateDirectories(new DirectoryInfo(path2));
					}
					string text9 = page.MapPath(arrayList2[i].ToString() + arrayList[i].ToString());
					Util.CreateLog("AUTO UPDATE : Replacing or Copying New File from New Update File : " + text8 + "-->" + text9);
					File.Copy(text8, text9, overwrite: true);
					File.Delete(text8);
				}
			}
			catch (Exception ex)
			{
				Util.CreateLog("AUTO UPDATE : " + ex.Message);
				Util.ShowAlertMessage("AUTO UPDATE : " + ex.Message);
				return;
			}
			int num = text4.LastIndexOf("/");
			string text10 = text4.Substring(0, text4.Length - 1);
			num = text10.LastIndexOf("/");
			text10 = text10.Substring(num + 1, text10.Length - num - 1);
			string path3 = page.MapPath(LocalUpdateConfigFolder) + ConfigScriptFile;
			if (File.Exists(path3))
			{
				string sQL = File.ReadAllText(path3);
				if (!Command.ExecNonQuery(sQL))
				{
					Util.CreateLog("AUTO UPDATE : " + page.Session[MySession.CurrentErrorMessage].ToString());
					Util.ShowAlertMessage("AUTO UPDATE : " + page.Session[MySession.CurrentErrorMessage].ToString());
					return;
				}
			}
			File.WriteAllText(page.MapPath(LocalUpdateConfigFolder) + ConfigCurrentVersionFile, text10);
			File.WriteAllText(page.MapPath(LocalUpdateConfigFolder) + ConfigLastUpdateFile, DateTime.Now.ToShortDateString());
			Util.CreateLog("AUTO UPDATE : Update Successfull");
			Util.ShowAlertMessage("AUTO UPDATE : Sukses melakukan update. Silahkan logout untuk refresh aplikasi");
		}
		catch (Exception ex)
		{
			Util.CreateLog("AUTO UPDATE : " + ex.Message);
			Util.ShowAlertMessage("AUTO UPDATE : " + ex.Message);
		}
	}

	public static string GetCurrentVersion()
	{
		try
		{
			if (HttpContext.Current == null)
			{
				return "";
			}
			Page page = HttpContext.Current.Handler as Page;
			string path = page.MapPath(LocalUpdateConfigFolder + ConfigCurrentVersionFile);
			return File.ReadAllText(path);
		}
		catch
		{
			return "1.0";
		}
	}

	public static string GetLastestVersion()
	{
		if (HttpContext.Current == null)
		{
			return "";
		}
		Page page = HttpContext.Current.Handler as Page;
		WebClient webClient = new WebClient();
		string text = page.MapPath(LocalUpdateConfigFolder);
		string path = text + "\\" + ConfigAutoUpdateFile;
		string text2 = text + "DownloadInfo.txt";
		string text3 = "";
		string text4 = "";
		string text5 = page.MapPath("~") + "/";
		try
		{
			string text6 = "";
			if (File.Exists(path))
			{
				text6 = File.ReadAllText(path);
			}
			Util.CreateLog("AUTO UPDATE : Downloading file starting for getting latest version information : " + text6);
			webClient.DownloadFile(text6, text2);
			Util.CreateLog("AUTO UPDATE : Downloading file ending for getting latest version information : " + text6 + ". And saving at : " + text2);
			text3 = File.ReadAllText(text2);
			text4 = text3[text3.LastIndexOf("/")] + "/";
			if (File.Exists(text2))
			{
				File.Delete(text2);
			}
			Util.CreateLog("AUTO UPDATE : Aquired information for current update url : " + text3);
		}
		catch (Exception ex)
		{
			Util.CreateLog(ex.Message);
			return "";
		}
		string text7 = "";
		try
		{
			int num = text4.LastIndexOf("/");
			text7 = text4.Substring(0, text4.Length - 1);
			num = text7.LastIndexOf("/");
			text7 = text7.Substring(num + 1, text7.Length - num - 1);
		}
		catch (Exception ex)
		{
			Util.CreateLog(ex.Message);
		}
		return text7;
	}
}
