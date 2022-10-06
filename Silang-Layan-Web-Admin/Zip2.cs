using System.Collections;
using System.IO;
using System.Web.UI;
using Ionic.Zip;

public class Zip2
{
	public static void ZipFile(Page page, ArrayList ListFile, string DirName, string ZipName)
	{
		ZipFile zipFile = new ZipFile();
		using (zipFile)
		{
			foreach (string item in ListFile)
			{
				if (File.Exists(item))
				{
					zipFile.AddFile(item, DirName);
				}
			}
		}
		page.Response.Clear();
		page.Response.AddHeader("Content-Disposition", "attachment; filename=" + ZipName);
		page.Response.ContentType = "application/zip";
		zipFile.Save((Stream)(object)page.Response.OutputStream);
		zipFile = null;
		page.Response.End();
	}

	public static void ZipDirectory(string DirName, string ZipPath)
	{
		ZipFile zipFile = new ZipFile();
		zipFile.AddDirectory(DirName);
		zipFile.Save(ZipPath);
	}
}
