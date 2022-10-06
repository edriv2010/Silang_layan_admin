using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;

public class CIP
{
	public class LocationInfo
	{
		public string CountryCode { get; set; }

		public string CountryName { get; set; }

		public string RegionName { get; set; }

		public string CityName { get; set; }

		public string ZipPostalCode { get; set; }

		public TimezoneInfo Timezone { get; set; }

		public LatLongInfo Position { get; set; }
	}

	public class TimezoneInfo
	{
		public string TimezoneName { get; set; }

		public string Gmtoffset { get; set; }

		public string Dstoffset { get; set; }
	}

	public class LatLongInfo
	{
		public string Latitude { get; set; }

		public string Longitude { get; set; }
	}

	public static LocationInfo HostIpToLocation(string ip)
	{
		string format = "http://api.ipinfodb.com/v2/ip_query.php?key={0}&ip={1}&timezone=true";
		format = string.Format(format, "f5eb9f340035b558c22be16c369acf37b306860f06155e36f3b77e97d5c4cc07", ip);
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(format);
		HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		XDocument xDocument = XDocument.Load(httpWebResponse.GetResponseStream());
		return (from x in xDocument.Descendants("Response")
			select new LocationInfo
			{
				CountryCode = x.Element("CountryCode").Value,
				CountryName = x.Element("CountryName").Value,
				RegionName = x.Element("RegionName").Value,
				CityName = x.Element("City").Value,
				ZipPostalCode = x.Element("ZipPostalCode").Value,
				Timezone = new TimezoneInfo
				{
					TimezoneName = x.Element("TimezoneName").Value,
					Gmtoffset = x.Element("Gmtoffset").Value,
					Dstoffset = x.Element("Dstoffset").Value
				},
				Position = new LatLongInfo
				{
					Latitude = x.Element("Latitude").Value,
					Longitude = x.Element("Longitude").Value
				}
			}).First();
	}

	public static void WebUserLogEntry(Page p)
	{
		if (!MyApplication.IsSaveIPLog)
		{
			return;
		}
		try
		{
			string fileName = Path.GetFileName(p.Request.PhysicalPath);
			string secondValue = HttpContext.Current.Request.UserHostAddress.ToString();
			HttpBrowserCapabilities browser = p.Request.Browser;
			string secondValue2 = browser.Browser.ToString();
			string secondValue3 = browser.Version.ToString();
			string secondValue4 = browser.MajorVersion.ToString();
			string secondValue5 = browser.MinorVersion.ToString();
			string secondValue6 = browser.Platform.ToString();
			string secondValue7 = browser.Beta.ToString();
			string secondValue8 = browser.Crawler.ToString();
			string secondValue9 = browser.AOL.ToString();
			string text = (string.IsNullOrEmpty(p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? p.Request.ServerVariables["REMOTE_ADDR"] : p.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
			string secondValue10 = Dns.GetHostEntry(Dns.GetHostName()).AddressList.GetValue(0).ToString();
			LocationInfo locationInfo = HostIpToLocation(text);
			TwoArrayList twoArrayList = new TwoArrayList();
			twoArrayList.Add("PageName", fileName);
			twoArrayList.Add("PublicIpAddress", secondValue);
			twoArrayList.Add("BrowserName", secondValue2);
			twoArrayList.Add("BrowserVersion", secondValue3);
			twoArrayList.Add("BrowserMajorVersion", secondValue4);
			twoArrayList.Add("BrowserMinorVersion", secondValue5);
			twoArrayList.Add("Platform", secondValue6);
			twoArrayList.Add("IsBeta", secondValue7);
			twoArrayList.Add("IsCrawler", secondValue8);
			twoArrayList.Add("IsAOL", secondValue9);
			twoArrayList.Add("IP", text);
			twoArrayList.Add("MACAddress", secondValue10);
			twoArrayList.Add("CountryCode", locationInfo.CountryCode);
			twoArrayList.Add("CountryName", locationInfo.CountryName);
			twoArrayList.Add("RegionName", locationInfo.RegionName);
			twoArrayList.Add("ZipPostalCode", locationInfo.ZipPostalCode);
			twoArrayList.Add("TimezoneName", locationInfo.Timezone.TimezoneName);
			twoArrayList.Add("Gmtoffset", locationInfo.Timezone.Gmtoffset);
			twoArrayList.Add("Dstoffset", locationInfo.Timezone.Dstoffset);
			twoArrayList.Add("Latitude", locationInfo.Position.Latitude);
			twoArrayList.Add("Longitude", locationInfo.Position.Longitude);
			twoArrayList.Add("CreatedDate", DateTime.Now);
			Command.ExecInsertOrUpdate("webuserlog", twoArrayList, Command.InsertOrUpdate.Insert);
		}
		catch (Exception ex)
		{
			Util.CreateLog(ex.Message);
		}
	}
}
