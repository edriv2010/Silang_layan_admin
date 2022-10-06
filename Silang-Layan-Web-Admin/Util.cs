using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using SD = System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.Security;
using System.Web;
public class Util
{

	public static string EncryptSHA1(string plain)
	{
		return FormsAuthentication.HashPasswordForStoringInConfigFile(plain, "SHA1");
	}

	public static string GetPageTitle(Page Page)
	{
		string text = "";
		string fileName = Path.GetFileName(Page.Request.Path);
		string text2 = Command.ExecScalar("SELECT ID FROM modules WHERE URL='" + fileName + "'", "");
		ArrayList arrayList = new ArrayList();
		while (text2 != "")
		{
			string value = Command.ExecScalar("SELECT Name FROM modules WHERE ID='" + text2 + "'", "");
			arrayList.Add(value);
			text2 = Command.ExecScalar("SELECT ParentID FROM modules WHERE ID='" + text2 + "'", "");
		}
		for (int num = arrayList.Count - 1; num >= 0; num--)
		{
			string value = arrayList[num].ToString();
			text = ((num <= 0) ? (text + value) : (text + value + " ► "));
		}
		return text;
	}

	public static void RaiseMessage(string MessageContent)
	{
		MyApplication.CurrentErrorMessage = MessageContent;
		if (HttpContext.Current.Session != null)
		{
			HttpContext.Current.Session.Add(MySession.CurrentErrorMessage, MessageContent);
		}
		CreateLog(MessageContent);
		if (MyApplication.IsDebug)
		{
			ShowAlertMessage(MessageContent);
		}
		MyApplication.CurrentErrorMessage = MessageContent.Replace("\n", "\\n").Replace("\r", "\\r");
	}

	public static void CreateLog(string LogEntry)
	{
		try
		{
			string text = "";
			try
			{
				if (HttpContext.Current != null)
				{
					Page page = HttpContext.Current.Handler as Page;
					text = Path.GetFileName(page.Request.Path);
				}
			}
			catch
			{
			}
			if (!Directory.Exists(MyApplication.LogPath))
			{
				Directory.CreateDirectory(MyApplication.LogPath);
			}
			string logPath = MyApplication.LogPath;
            
            string path = logPath + "\\" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "." + MyApplication.LogExt;
            LogEntry = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now) + " : " + text + "," + LogEntry + Environment.NewLine;
			File.AppendAllText(path, LogEntry);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string HexConverter(Color c)
	{
		return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
	}

	public static string GetRandomHexColor()
	{
        var rd = new Random();
        return string.Format("{0:X6}", rd.Next(16777216));
        /*

		Random random = new Random();
		return String.Format(("{0:X6}", random.Next(16777216));
         */
	}

	public static string RemoveInvalidCharacter(string Str)
	{
		return Regex.Replace(Str, "[^\\w\\- ]", "");
	}

	public static bool Ping(string HostName)
	{
		Ping ping = new Ping();
		PingReply pingReply = ping.Send(HostName);
		if (pingReply.Status == IPStatus.Success)
		{
			return true;
		}
		return false;
	}

	public static string GetServerIPAddress()
	{
		List<IPAddress> list = new List<IPAddress>();
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
			{
				if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
				{
					return unicastAddress.Address.ToString();
				}
			}
		}
		return "";
	}

	public static string GetUserHost(Page p)
	{
		return string.IsNullOrEmpty(p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? p.Request.ServerVariables["REMOTE_ADDR"] : p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
	}

	public static bool IsUrl()
	{
		string text = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
		string value = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		return text != null && text.IndexOf(value) == 7;
	}

	public static void ShowAlertMessage(string error, string title = "Peringatan!", string extended_script = "", string icon = "fa fa-warning")
	{
         if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
		if (page != null )
		{
			error = error.Replace("'", "`");
			error = error.Replace("\\", "\\\\");
			error = error.Replace("\r", " ");
			error = error.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "AlertThis = function() { $.alert({title: '" + title + "'," + ((!string.IsNullOrEmpty(icon)) ? ("icon: '" + icon + "',") : "") + "content: '" + error + "',buttons: {    okay: {        text: 'OK',        btnClass: 'btn-green'    }}}); setTimeout(function () { $('.btn-green').focus(); }, 100); }; setTimeout(function () { AlertThis(); " + extended_script + " }, 100);", addScriptTags: true);
		}
	}

	public static void ShowAlertError()
	{
        //string error = "Gagal menyimpan data, hubungi team pengembang sistem.";
        
		string text = "Gagal menyimpan data, hubungi team pengembang sistem.";
		System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;

        if (page != null)
		{
			text = text.Replace("'", "`");
			text = text.Replace("\\", "\\\\");
			text = text.Replace("\r", " ");
			text = text.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + text + "');", addScriptTags: true);
		}
	}

	public static void ShowMsgAndGoToPage(string msg, string url)
	{
        if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
		if (page != null )
		{
			msg = msg.Replace("'", "`");
			msg = msg.Replace("\\", "\\\\");
			msg = msg.Replace("\r", " ");
			msg = msg.Replace("\n", "\\n");
			ScriptManager.RegisterStartupScript(page, page.GetType(), "err_msg", "alert('" + msg + "');window.location ='" + url + "';", addScriptTags: true);
		}
	}

	public static void GoToPage(string url)
	{
        if (HttpContext.Current == null) { return; }
        System.Web.UI.Page page = HttpContext.Current.Handler as System.Web.UI.Page;
		if (page != null)
		{
			ScriptManager.RegisterStartupScript(page, page.GetType(), "gotoPage", "window.location ='" + url + "';", addScriptTags: true);
		}
	}

	public static void ClearControls(ControlCollection ctrls)
	{
		foreach (Control ctrl in ctrls)
		{
			if (ctrl is TextBox)
			{
				((TextBox)ctrl).Text = string.Empty;
			}
			ClearControls(ctrl.Controls);
		}
	}

	public static string GetPostRequest(string Key)
	{
		string text = "";
		if (HttpContext.Current.Request[Key] != null)
		{
			text = HttpContext.Current.Request[Key];
		}
		return text.Trim();
	}

	public static string GetRequest(string Key)
	{
		string text = "";
		if (HttpContext.Current.Request[Key] != null)
		{
			text = HttpContext.Current.Request[Key];
		}
		return text.Trim().ToLower();
	}

	public static bool IsExist(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsStartsWithExist(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				return true;
			}
		}
		return false;
	}

	public static int GetIndex(string[] List, string CheckString)
	{
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				return i;
			}
		}
		return -1;
	}

	public static int GetIndex(DataTable dt, string ColumnName, string CheckString)
	{
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			if (dt.Rows[i][ColumnName].ToString() == CheckString)
			{
				return i;
			}
		}
		return -1;
	}

	public static ArrayList GetStartsWithIndex(string[] List, string CheckString)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				arrayList.Add(i);
			}
		}
		return arrayList;
	}

	public static ArrayList GetIndexs(string[] List, string CheckString)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i] == CheckString)
			{
				arrayList.Add(i);
			}
		}
		return arrayList;
	}

	public static ArrayList GetIndexsStartWith(string[] List, string CheckString)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < List.Length; i++)
		{
			if (List[i].StartsWith(CheckString))
			{
				arrayList.Add(i);
			}
		}
		return arrayList;
	}

	public static ArrayList GetIndexs(string[] List, ArrayList ListCheckString)
	{
		ArrayList arrayList = new ArrayList();
		if (List == null)
		{
			return arrayList;
		}
		for (int i = 0; i < List.Length; i++)
		{
			foreach (string item in ListCheckString)
			{
				if (List[i] == item)
				{
					arrayList.Add(i);
				}
			}
		}
		return arrayList;
	}

	public static string UpperFirst(string s)
	{
		return Regex.Replace(s, "\\b[a-z]\\w+", delegate(Match match)
		{
			string text = match.ToString();
			return char.ToUpper(text[0]) + text.Substring(1);
		});
	}

	public static string CollapseSpaces(string value)
	{
		return Regex.Replace(value, "\\s+", " ");
	}

	public static void SelectText(TextBox txt)
	{
        // Is there a ScriptManager on the page?
        if (ScriptManager.GetCurrent(txt.Page) != null && ScriptManager.GetCurrent(txt.Page).IsInAsyncPostBack)
            // Set ctrlToSelect
            ScriptManager.RegisterStartupScript(txt.Page,
                                       txt.Page.GetType(),
                                       "SetFocusInUpdatePanel-" + txt.ClientID,
                                       String.Format("ctrlToSelect='{0}';", txt.ClientID),
                                       true);
        else
            txt.Page.ClientScript.RegisterStartupScript(txt.Page.GetType(),
                                             "Select-" + txt.ClientID,
                                             String.Format("document.getElementById('{0}').select();", txt.ClientID),
                                             true);

	}

	public static string GetRoman(int number)
	{
		if (number <= 0)
		{
			return "";
		}
		int[] array = new int[13]
		{
			1, 4, 5, 9, 10, 40, 50, 90, 100, 400,
			500, 900, 1000
		};
		string[] array2 = new string[13]
		{
			"I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD",
			"D", "M", "M"
		};
		string text = "";
		int num = number;
		for (int num2 = array.Length - 1; num2 >= 0; num2--)
		{
			while (num >= array[num2])
			{
				num -= array[num2];
				text += array2[num2];
			}
		}
		return text;
	}

	public static bool ConvertToBoolean(object Value)
	{
		if (Value == null || string.IsNullOrEmpty(Value.ToString()))
		{
			return false;
		}
		if (Value.ToString().ToLower() == "false" || Value.ToString() == "0")
		{
			return false;
		}
		return true;
	}

    public static int MonthIndexEn(string MonthName)
    {
        switch (MonthName)
        {
            case "Jan":
                return 1;
            case "Feb":
                return 2;
            case "Mar":
                return 3;
            case "Apr":
                return 4;
            case "May":
                return 5;
            case "Jun":
                return 6;
            case "Jul":
                return 7;
            case "Aug":
                return 8;
            case "Sep":
                return 9;
            case "Oct":
                return 10;
            case "Nov":
                return 11;
            case "Dec":
                return 12;
            default:
                return 0;
        }
    }


	public static int MonthIndex(string MonthName)
	{
        switch (MonthName)
        {
            case "Jan":
                return 1;
            case "Feb":
                return 2;
            case "Mar":
                return 3;
            case "Apr":
                return 4;
            case "Mei":
                return 5;
            case "Jun":
                return 6;
            case "Jul":
                return 7;
            case "Agust":
                return 8;
            case "Sep":
                return 9;
            case "Okt":
                return 10;
            case "Nop":
                return 11;
            case "Des":
                return 12;
            default:
                return 0;
        }
	}

	public static int MonthIndexCombo(string MonthName)
	{
        switch (MonthName)
        {
            case "Jan":
                return 1;
            case "Feb":
                return 2;
            case "Mar":
                return 3;
            case "Apr":
                return 4;
            case "Mei":
                return 5;
            case "May":
                return 5;
            case "Jun":
                return 6;
            case "Jul":
                return 7;
            case "Agust":
                return 8;
            case "Aug":
                return 8;
            case "Sep":
                return 9;
            case "Okt":
                return 10;
            case "Nop":
                return 11;
            case "Des":
                return 12;
            case "Oct":
                return 10;
            case "Nov":
                return 11;
            case "Dec":
                return 12;
            default:
                return 0;
        }
	}

	public static void CreateDirectories(DirectoryInfo instance)
	{
		if (instance.Parent != null)
		{
			CreateDirectories(instance.Parent);
		}
		if (!instance.Exists)
		{
			instance.Create();
		}
	}

	public static bool IsValidEmail(string EmailAddress)
	{
        try
        {
            System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(EmailAddress);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }

	}

	public static bool IsValidPhoneNumber(string PhoneNumber)
	{
		if (!Regex.IsMatch(PhoneNumber, "^\\+?(\\d[\\d-. ]+)?(\\([\\d-. ]+\\))?[\\d-. ]+\\d$"))
		{
			return false;
		}
		return true;
	}

	public static void DeleteFolder(string FolderName)
	{
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(FolderName);
			directoryInfo.Delete(recursive: true);
		}
		catch
		{
			ShowAlertMessage("Cannot delete directory : " + FolderName);
		}
	}

	private static ImageCodecInfo GetEncoderInfo(string mimeType)
	{
		ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
		for (int i = 0; i < imageEncoders.Length; i++)
		{
			if (imageEncoders[i].MimeType == mimeType)
			{
				return imageEncoders[i];
			}
		}
		return null;
	}

	public static void ResizeImage(string fileName, string outputFileName, int NewWidth, int NewHeight, int newResolution, string newCodec, int qualityLevel, bool IsStretch)
	{
		System.Drawing.Image image = System.Drawing.Image.FromFile(fileName);
		float num = (float)image.Height / (float)image.Width;
		int height = (int)((float)NewWidth * num);
		if (IsStretch)
		{
			height = NewHeight;
		}
		int width = NewWidth;
		if (NewWidth == 0)
		{
			width = (int)((float)NewHeight * ((float)image.Width / (float)image.Height));
			height = NewHeight;
		}
		Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
		ImageCodecInfo encoderInfo = GetEncoderInfo(newCodec);
		Encoder quality = Encoder.Quality;
		EncoderParameters encoderParameters = new EncoderParameters(1);
		EncoderParameter encoderParameter = new EncoderParameter(quality, qualityLevel);
		encoderParameters.Param[0] = encoderParameter;
		bitmap.SetResolution(newResolution, newResolution);
		Graphics graphics = Graphics.FromImage(bitmap);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.CompositingQuality = CompositingQuality.HighQuality;
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		graphics.DrawImage(image, 0, 0, width, height);
		bitmap.Save(outputFileName, encoderInfo, encoderParameters);
		image.Dispose();
		bitmap.Dispose();
		graphics.Dispose();
	}

	public static string BytesToString(long byteCount)
	{
		string[] array = new string[7] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
		if (byteCount == 0)
		{
			return "0" + array[0];
		}
		long num = Math.Abs(byteCount);
		int num2 = Convert.ToInt32(Math.Floor(Math.Log(num, 1024.0)));
		double num3 = Math.Round((double)num / Math.Pow(1024.0, num2), 1);
		return (double)Math.Sign(byteCount) * num3 + " " + array[num2];
	}

	public static string EscapeURL(string Input)
	{
		return HttpUtility.JavaScriptStringEncode(Input).ToString().Replace("\"", "&quot;")
			.Replace("+", "%2B");
	}

	public static string TrimEnd(string inputText, string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			while (!string.IsNullOrEmpty(inputText) && inputText.EndsWith(value, StringComparison.CurrentCultureIgnoreCase))
			{
				inputText = inputText.Substring(0, inputText.Length - value.Length);
			}
		}
		return inputText;
	}

	public static string ReplaceAt(string str, int index, int length, string replace)
	{
		return str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);
	}

	public static string BytesToStringKB(long byteCount)
	{
        return string.Format("{0:N0} KB", byteCount / 1024);
	}

	public static string GetMimeType(string fileName)
	{
        string mimeType = "application/unknown";
        string ext = System.IO.Path.GetExtension(fileName).ToLower();
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
            mimeType = regKey.GetValue("Content Type").ToString();
        return mimeType;
	}

	public static string FacetGenerator(string Value)
	{
		return Value.Replace(":", "").Replace("[", "").Replace("]", "")
			.Replace("/", "")
			.TrimEnd(',', ':', '.', ' ', '/')
			.Trim();
	}

	public static string TruncateAtWord(string input, int length)
	{
		if (input == null || input.Length < length)
		{
			return input;
		}
        int num = input.LastIndexOf(" ", length, StringComparison.Ordinal);
        return string.Format("{0}...", input.Substring(0, (num > 0) ? num : length).Trim());
        /*
            int num = input.LastIndexOf(" ", length, StringComparison.Ordinal);
		return string.Format("{input.Substring(0, (num > 0) ? num : length).Trim()}...";
        */
	}

	public static string DigitGrouping(string Input)
	{
        if (string.IsNullOrEmpty(Input))
        {
            return "0";
        }
        else
        {
            return decimal.Parse(Input).ToString("#,##0", new System.Globalization.CultureInfo("id-ID"));
        }
        /*
		if (string.IsNullOrEmpty(Input))
		{
			return "0";
		}
        return decimal.Parse(Input).ToString("#,##0", new DigitGrouping.CultureInfo("id-ID"));
         */
	}
}