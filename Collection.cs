using System;
using System.Data;

public class Collection
{
	public static string GetLastestCounterNomorInduk(string Tahun)
	{
		string sQL = "SELECT MAX(SUBSTR(NoInduk,10,6)) from Collections where NoInduk like '" + Tahun + "%' AND LENGTH(NoInduk) = 15 AND NOT NoInduk LIKE '%/%' AND NOT NoInduk LIKE '%-%' AND REGEXP_LIKE(NoInduk, '^[[:digit:]]+$')";
		return Command.ExecScalar(sQL);
	}

	public static string GetNewNomorInduk(string CollectionID)
	{
		DataTable dataTable = Command.ExecDataAdapter("SELECT TanggalKirim,Worksheet_ID,Category_ID FROM Collections WHERE ID=" + CollectionID);
		if (dataTable.Rows.Count > 0)
		{
			if (string.IsNullOrEmpty(dataTable.Rows[0]["TanggalKirim"].ToString()))
			{
				throw new Exception("Item tidak mempunyai tanggal kirim!");
			}
			DateTime dateTime = (DateTime)dataTable.Rows[0]["TanggalKirim"];
			string text = dataTable.Rows[0]["Worksheet_ID"].ToString();
			string text2 = dataTable.Rows[0]["Category_ID"].ToString();
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				throw new Exception("Item tidak terhubung dengan jenis bahan / kategori koleksi.\r\nHarap pilih Jenis Bahan Pustaka/Kategori Koleksi terlebih dahulu!");
			}
			string text3 = Command.ExecScalar("SELECT Code FROM Worksheets WHERE ID=" + text);
			if (!string.IsNullOrEmpty(text3))
			{
				string text4 = Command.ExecScalar("SELECT Code FROM CollectionCategorys WHERE ID=" + text2);
				if (!string.IsNullOrEmpty(text4))
				{
					string tahun = "{dateTime:yyyy}";
					return GetNewNomorInduk(tahun, text3, text4);
				}
				throw new Exception("Item tidak terhubung dengan kategori koleksi!");
			}
			throw new Exception("Tidak dapat menemukan data koleksi!");
		}
		throw new Exception("Tidak dapat menemukan kode jenis bahan!");
	}

	public static string GetNewNomorInduk(string Tahun, string KodeWorksheet, string KodeLokasiGroup)
	{
		string lastestCounterNomorInduk = GetLastestCounterNomorInduk(Tahun);
		int num = 0;
		if (!string.IsNullOrEmpty(lastestCounterNomorInduk))
		{
			num = int.Parse(lastestCounterNomorInduk);
		}
		string text = "{num + 1:000000}";
		string text2 = Tahun + KodeWorksheet + KodeLokasiGroup + text;
		string value = Command.ExecScalar("SELECT NoInduk FROM Collections WHERE NoInduk = '" + text2 + "'");
		while (!string.IsNullOrEmpty(value))
		{
			num++;
			text = "{num + 1:000000}";
			text2 = Tahun + KodeWorksheet + KodeLokasiGroup + text;
			value = Command.ExecScalar("SELECT NoInduk FROM Collections WHERE NoInduk = '" + text2 + "'");
		}
		return text2;
	}
}
