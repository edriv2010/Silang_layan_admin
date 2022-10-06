using System;
using System.IO;
using System.Web.UI;
using ICSharpCode.SharpZipLib.Zip;

public class Zip
{
	public static bool Unzipfile(Page page, string sfile, string UnzipBasePath)
	{
		try
		{
			ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(sfile));
			DirectoryInfo directoryInfo = new DirectoryInfo(UnzipBasePath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			ZipEntry nextEntry;
			while ((nextEntry = zipInputStream.GetNextEntry()) != null)
			{
				string directoryName = Path.GetDirectoryName(nextEntry.Name);
				string fileName = Path.GetFileName(nextEntry.Name);
				if (nextEntry.IsDirectory)
				{
					Directory.CreateDirectory(UnzipBasePath + directoryName);
				}
				if (!(fileName != string.Empty))
				{
					continue;
				}
				string path = UnzipBasePath + nextEntry.Name;
				string directoryName2 = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName2))
				{
					Directory.CreateDirectory(directoryName2);
				}
				FileStream fileStream = File.Create(path);
				int num = 2048;
				byte[] array = new byte[2048];
				while (true)
				{
					bool flag = true;
					num = zipInputStream.Read(array, 0, array.Length);
					if (num > 0)
					{
						fileStream.Write(array, 0, num);
						continue;
					}
					break;
				}
				fileStream.Close();
			}
			zipInputStream.Close();
			return true;
		}
		catch (Exception ex)
		{
			page.Session.Add(MySession.CurrentErrorMessage, ex.Message);
			return false;
		}
	}
}
