using System;
using System.Data;

public class Sirkulasi
{
	public enum RewardAkumulasi
	{
		Penambahan,
		Pengurangan
	}

	public class RewardPoint
	{
		public RewardAkumulasi Akumulasi = RewardAkumulasi.Penambahan;

		public int Point = 0;
	}

	public class RewardKategori
	{
		public int Id = 0;

		public string Name = "";

		public int Range_Awal = 0;

		public int Range_Akhir = 0;

		public int Penambahan_Pinjam_Buku = 0;

		public int Penambahan_Hari_Pinjam = 0;
	}

	public static int GetWarningLoanDueDay(double ItemID, string Member_Id)
	{
		string text = "0";
		string sQL = "SELECT WarningLoanDueDay FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.Id = " + Member_Id;
		DataTable dataTable = Command.ExecDataAdapter(sQL);
		if (dataTable.Rows.Count > 0)
		{
			text = dataTable.Rows[0]["WarningLoanDueDay"].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		sQL = "SELECT cr.WarningLoanDueDay FROM collections cl INNER JOIN collectionrules cr ON (cl.Rule_id = cr.ID) WHERE cl.ID =" + ItemID;
		dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			if (text == "0")
			{
				text = dataTable.Rows[0]["WarningLoanDueDay"].ToString();
			}
			else if (dataTable.Rows[0]["WarningLoanDueDay"].ToString() != "0")
			{
				text = Math.Max(int.Parse(text), int.Parse(dataTable.Rows[0]["WarningLoanDueDay"].ToString())).ToString();
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		return int.Parse(text);
	}

	public static int GetWarningLoanDueDayByMemberNo(double ItemID, string MemberNo)
	{
		string text = "0";
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("MemberNo", MemberNo);
		string sQL = "SELECT WarningLoanDueDay FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.MemberNo = " + Connection.ParameterSymbol + "MemberNo";
		DataTable dataTable = Command.ExecDataAdapter(sQL, twoArrayList);
		if (dataTable.Rows.Count > 0)
		{
			text = dataTable.Rows[0]["WarningLoanDueDay"].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		sQL = "SELECT cr.WarningLoanDueDay FROM collections cl INNER JOIN collectionrules cr ON (cl.Rule_id = cr.ID) WHERE cl.ID =" + ItemID;
		dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			if (text == "0")
			{
				text = dataTable.Rows[0]["WarningLoanDueDay"].ToString();
			}
			else if (dataTable.Rows[0]["WarningLoanDueDay"].ToString() != "0")
			{
				text = Math.Max(int.Parse(text), int.Parse(dataTable.Rows[0]["WarningLoanDueDay"].ToString())).ToString();
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		return int.Parse(text);
	}

	public static bool IsCheckHoliday(DateTime CheckDate)
	{
		string text = "";
		DataTable dataTable = new DataTable();
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
                text = "SELECT Dates FROM holidays  WHERE Dates = to_date('" + String.Format("{0:MM/dd/yyyy},CheckDate") + "','MM/dd/yyyy')";
			dataTable = Command.ExecDataAdapter(text, null);
			break;
		case Connection.EServerType.MySQL:
            text = "SELECT Dates FROM holidays WHERE Dates = '" + String.Format("{0:yyyy-MM-dd}",CheckDate) + "'";
			dataTable = Command.ExecDataAdapter(text, null);
			break;
		}
		int count = dataTable.Rows.Count;
		return count > 0;
	}

	public static int GetCollectionCount(string ID)
	{
		string sQL = "SELECT CollectionCount FROM collectionloans WHERE ID = " + ID;
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			if (!string.IsNullOrEmpty(dataTable.Rows[0]["CollectionCount"].ToString()))
			{
				return int.Parse(dataTable.Rows[0]["CollectionCount"].ToString());
			}
			return 0;
		}
		return 0;
	}

	public static double PenaltyAmountDaily(int Collection_ID, string Member_Id)
	{
		string text = "0";
		string sQL = "SELECT DendaPerTenor as PenaltyAmountDaily FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.Id = " + Member_Id;
		DataTable dataTable = Command.ExecDataAdapter(sQL);
		if (dataTable.Rows.Count > 0)
		{
			text = dataTable.Rows[0]["PenaltyAmountDaily"].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		sQL = "SELECT cr.PenaltyAmountDaily FROM collectionloanitems cli INNER JOIN collections c ON (cli.Collection_id = c.ID)  INNER JOIN collectionrules cr ON (c.Rule_id = cr.ID) WHERE cli.Collection_id = " + Collection_ID;
		dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count == 0)
		{
			if (string.IsNullOrEmpty(dataTable.Rows[0]["PenaltyAmountDaily"].ToString()))
			{
				dataTable.Rows[0]["PenaltyAmountDaily"] = "0";
			}
			if (text == "0")
			{
				text = dataTable.Rows[0]["PenaltyAmountDaily"].ToString();
			}
			else if (dataTable.Rows[0]["PenaltyAmountDaily"].ToString() != "0")
			{
				text = Math.Max(double.Parse(text), double.Parse(dataTable.Rows[0]["PenaltyAmountDaily"].ToString())).ToString();
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		return double.Parse(text);
	}

	public static string getTipeSirkulasi()
	{
		string sQL = "select value from settingparameters where Name='TipeSirkulasi'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return dataTable.Rows[0]["value"].ToString().Trim();
	}

	public static int getCollectionID(string kode)
	{
		string text = "";
		text = ((!(getTipeSirkulasi() == "BARCODE")) ? ("SELECT ID FROM collections WHERE RFID = '" + kode + "'") : ("SELECT ID FROM collections WHERE NomorBarcode = '" + kode + "'"));
		DataTable dataTable = Command.ExecDataAdapter(text, null);
		return int.Parse(dataTable.Rows[0]["ID"].ToString().Trim());
	}

	public static string getCollectionRFID(string kode)
	{
		string text = "";
		text = ((!(getTipeSirkulasi() == "BARCODE")) ? ("SELECT RFID FROM collections WHERE RFID = '" + kode + "'") : ("SELECT RFID FROM collections WHERE NomorBarcode = '" + kode + "'"));
		DataTable dataTable = Command.ExecDataAdapter(text, null);
		return dataTable.Rows[0]["RFID"].ToString().Trim();
	}

	public static string getCollectionBarcode(string kode)
	{
		string text = "";
		text = ((!(getTipeSirkulasi() == "BARCODE")) ? ("SELECT NomorBarcode FROM collections WHERE RFID = '" + kode + "'") : ("SELECT NomorBarcode FROM collections WHERE NomorBarcode = '" + kode + "'"));
		DataTable dataTable = Command.ExecDataAdapter(text, null);
		return dataTable.Rows[0]["NomorBarcode"].ToString().Trim();
	}

	public static string getControlNumberById(string ID)
	{
		string sQL = "SELECT ControlNumber FROM catalogs WHERE ID = '" + ID + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return dataTable.Rows[0]["ControlNumber"].ToString().Trim();
	}

	public static bool IsPelanggaranExist(string NoPinjam, string CollectionId)
	{
		string sQL = "select NoPinjam from pelanggaran where NoPinjam ='" + NoPinjam.Trim() + "' AND NoItem = '" + CollectionId + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			return true;
		}
		return false;
	}

	public static int SisaExtendLoan(string NoPinjam)
	{
		MyApplication.PeminjamanJumlahFamilyMemberMin = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='PeminjamanJumlahFamilyMemberMin'", "3"));
		string text = "0";
		string text2 = "";
		DataTable dataTable = Command.ExecDataAdapter("SELECT Collection_Id,Member_Id FROM COLLECTIONLOANITEMS WHERE CollectionLoan_id = " + NoPinjam);
		if (dataTable.Rows.Count == 0)
		{
			return 0;
		}
		string text3 = dataTable.Rows[0]["Collection_Id"].ToString();
		string text4 = dataTable.Rows[0]["Member_Id"].ToString();
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanIsActive'", "0")))
		{
			string text5 = Command.ExecScalar("SELECT NoKK FROM members WHERE Id = " + text4);
			if (!string.IsNullOrEmpty(text5))
			{
				int num = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM members WHERE NoKK = '" + text5 + "'", "0"));
				if (num >= MyApplication.PeminjamanJumlahFamilyMemberMin)
				{
					text = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanCountPerpanjang'", "0")).ToString();
					goto IL_02ba;
				}
			}
		}
		text2 = "SELECT CountPerpanjang FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.Id = " + text4;
		DataTable dataTable2 = Command.ExecDataAdapter(text2);
		if (dataTable2.Rows.Count > 0)
		{
			text = dataTable2.Rows[0]["CountPerpanjang"].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		text2 = "SELECT collectionrules.CountPerpanjang FROM collections INNER JOIN collectionrules ON collections.Rule_id = collectionrules.ID WHERE collections.Id = '" + text3 + "'";
		dataTable2 = Command.ExecDataAdapter(text2, null);
		if (dataTable2.Rows.Count > 0)
		{
			if (string.IsNullOrEmpty(dataTable2.Rows[0]["CountPerpanjang"].ToString()))
			{
				dataTable2.Rows[0]["CountPerpanjang"] = "0";
			}
			if (text == "0")
			{
				text = dataTable2.Rows[0]["CountPerpanjang"].ToString();
			}
			else if (dataTable2.Rows[0]["CountPerpanjang"].ToString() != "0")
			{
				text = Math.Min(int.Parse(text), int.Parse(dataTable2.Rows[0]["CountPerpanjang"].ToString())).ToString();
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		goto IL_02ba;
		IL_02ba:
		int num2 = 0;
		string sQL = "SELECT extendcount FROM collectionloans WHERE id='" + NoPinjam + "'";
		DataTable dataTable3 = Command.ExecDataAdapter(sQL, null);
		num2 = ((!string.IsNullOrEmpty(dataTable3.Rows[0]["extendcount"].ToString())) ? int.Parse(dataTable3.Rows[0]["extendcount"].ToString().Trim()) : 0);
		int num3 = int.Parse(text) - num2;
		if (num3 < 0)
		{
			num3 = 0;
		}
		return num3;
	}

	public static int ExtendLoan(string NoPinjam)
	{
		int num = 0;
		string sQL = "SELECT extendcount FROM collectionloans WHERE id='" + NoPinjam + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows[0]["extendcount"].ToString().Trim() == null || dataTable.Rows[0]["extendcount"].ToString().Trim() == "")
		{
			return 0;
		}
		return int.Parse(dataTable.Rows[0]["extendcount"].ToString().Trim());
	}

	public static int CekMaksJumlahPeminjaman(string MemberNo)
	{
		MyApplication.PeminjamanJumlahFamilyMemberMin = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='PeminjamanJumlahFamilyMemberMin'", "3"));
		string text = "";
		string text2 = int.MaxValue.ToString();
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanIsActive'", "0")))
		{
			string text3 = Command.ExecScalar("SELECT NoKK FROM members WHERE MemberNo = '" + MemberNo + "'");
			if (!string.IsNullOrEmpty(text3))
			{
				int num = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM members WHERE NoKK = '" + text3 + "'", "0"));
				if (num >= MyApplication.PeminjamanJumlahFamilyMemberMin)
				{
					int num2 = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanMaxPinjamKoleksi'", "0"));
					int num3 = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONLOANITEMS WHERE Member_Id IN (SELECT Id FROM MEMBERS WHERE NOKK='" + text3 + "') AND LoanStatus = 'Loan'", "0"));
					return num2 - num3;
				}
			}
		}
		TwoArrayList twoArrayList = new TwoArrayList();
		twoArrayList.Add("MemberNo", MemberNo);
		text = "SELECT MaxPinjamKoleksi FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.MemberNo = " + Connection.ParameterSymbol + "MemberNo";
		DataTable dataTable = Command.ExecDataAdapter(text, twoArrayList);
		if (dataTable.Rows.Count > 0)
		{
			text2 = dataTable.Rows[0]["MaxPinjamKoleksi"].ToString();
		}
		else
		{
			text = "SELECT Value FROM settingparameters WHERE Name = 'MaksJumlahPeminjaman'";
			text2 = Command.ExecScalar(text, int.MaxValue.ToString());
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		string text4 = Command.ExecScalar("SELECT Id FROM MEMBERS WHERE MemberNo = '" + MemberNo + "'");
		if (!string.IsNullOrEmpty(text4))
		{
			RewardKategori memberRewardKategori = GetMemberRewardKategori(double.Parse(text4));
			text2 += memberRewardKategori.Penambahan_Pinjam_Buku;
		}
		return int.Parse(text2);
	}

	public static int CekLamaPinjam(string ItemId, string Member_Id, string KeyTypeId = "ID")
	{
		MyApplication.PeminjamanJumlahFamilyMemberMin = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='PeminjamanJumlahFamilyMemberMin'", "3"));
		string text = "";
		string text2 = "0";
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanIsActive'", "0")))
		{
			string text3 = Command.ExecScalar("SELECT NoKK FROM members WHERE Id = " + Member_Id);
			if (!string.IsNullOrEmpty(text3))
			{
				int num = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM members WHERE NoKK = '" + text3 + "'", "0"));
				if (num >= MyApplication.PeminjamanJumlahFamilyMemberMin)
				{
					return int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanMaxLoanDays'", "0"));
				}
			}
		}
		text = "SELECT MaxLoanDays FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.Id = " + Member_Id;
		DataTable dataTable = Command.ExecDataAdapter(text);
		if (dataTable.Rows.Count > 0)
		{
			text2 = dataTable.Rows[0]["MaxLoanDays"].ToString();
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		string text4 = "";
		text4 = ((KeyTypeId == "BARCODE") ? (" WHERE collections.NomorBARCODE = '" + ItemId + "'") : ((!(KeyTypeId == "RFID")) ? (" WHERE collections.Id = '" + ItemId + "'") : (" WHERE collections.RFID = '" + ItemId + "'")));
		text = "SELECT collectionrules.MaxLoanDays FROM collections INNER JOIN collectionrules ON collections.Rule_id = collectionrules.ID" + text4;
		dataTable = Command.ExecDataAdapter(text, null);
		if (dataTable.Rows.Count > 0)
		{
			if (string.IsNullOrEmpty(dataTable.Rows[0]["MaxLoanDays"].ToString()))
			{
				dataTable.Rows[0]["MaxLoanDays"] = "0";
			}
			if (text2 == "0")
			{
				text2 = dataTable.Rows[0]["MaxLoanDays"].ToString();
			}
			else if (dataTable.Rows[0]["MaxLoanDays"].ToString() != "0")
			{
				text2 = Math.Min(int.Parse(text2), int.Parse(dataTable.Rows[0]["MaxLoanDays"].ToString())).ToString();
			}
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		RewardKategori memberRewardKategori = GetMemberRewardKategori(double.Parse(Member_Id));
		text2 += memberRewardKategori.Penambahan_Hari_Pinjam;
		return int.Parse(text2);
	}

	public static int CekAntrianPinjaman(string Collection_Id)
	{
		return int.Parse(Command.ExecScalar("select count(1) from collectionloanqueue where collection_id=" + Collection_Id + " and sysdate < queueexpired  and queuestatus=0", "0"));
	}

	public static int CekDayPerpanjang(string Collection_Id, string Member_Id)
	{
		MyApplication.PeminjamanJumlahFamilyMemberMin = int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='PeminjamanJumlahFamilyMemberMin'", "3"));
		string text = "";
		string text2 = "0";
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanIsActive'", "0")))
		{
			string text3 = Command.ExecScalar("SELECT NoKK FROM members WHERE Id = " + Member_Id);
			if (!string.IsNullOrEmpty(text3))
			{
				int num = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM members WHERE NoKK = '" + text3 + "'", "0"));
				if (num >= MyApplication.PeminjamanJumlahFamilyMemberMin)
				{
					return int.Parse(Command.ExecScalar("SELECT Value FROM SETTINGPARAMETERS WHERE Name='FamilyLoanDayPerpanjang'", "0"));
				}
			}
		}
		text = "SELECT DayPerpanjang FROM members INNER JOIN jenis_anggota ON UPPER(members.JenisAnggota) = UPPER(jenis_anggota.Name) WHERE members.Id = " + Member_Id;
		DataTable dataTable = Command.ExecDataAdapter(text);
		if (dataTable.Rows.Count > 0)
		{
			text2 = dataTable.Rows[0]["DayPerpanjang"].ToString();
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		text = "SELECT collectionrules.DayPerpanjang FROM collections INNER JOIN collectionrules ON collections.Rule_id = collectionrules.ID WHERE collections.Id = '" + Collection_Id + "'";
		dataTable = Command.ExecDataAdapter(text, null);
		if (dataTable.Rows.Count > 0)
		{
			if (string.IsNullOrEmpty(dataTable.Rows[0]["DayPerpanjang"].ToString()))
			{
				dataTable.Rows[0]["DayPerpanjang"] = "0";
			}
			if (text2 == "0")
			{
				text2 = dataTable.Rows[0]["DayPerpanjang"].ToString();
			}
			else if (dataTable.Rows[0]["DayPerpanjang"].ToString() != "0")
			{
				text2 = Math.Min(int.Parse(text2), int.Parse(dataTable.Rows[0]["DayPerpanjang"].ToString())).ToString();
			}
		}
		if (text2 == "0")
		{
			text = "SELECT Value FROM SETTINGPARAMETERS WHERE Name='JumlahHariPerpanjanganPeminjaman'";
			text2 = Command.ExecScalar(text, "0");
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = "0";
		}
		return int.Parse(text2);
	}

	public static double GetMemberId(string NoPinjam)
	{
		string sQL = "SELECT member_id FROM collectionloanitems WHERE CollectionLoan_id='" + NoPinjam + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return Convert.ToDouble(dataTable.Rows[0]["member_id"].ToString());
	}

	public static double GetMemberIDFromMemberNo(string MemberNo)
	{
		string sQL = "SELECT ID FROM members WHERE MemberNo = '" + MemberNo + "'";
		return double.Parse(Command.ExecScalar(sQL, "0"));
	}

	public static string JenisDenda(int jumlSuspend, double PinaltyAmount)
	{
		string result = "";
		if (jumlSuspend == 0 && PinaltyAmount == 0.0)
		{
			result = "";
		}
		else if (jumlSuspend != 0 && PinaltyAmount == 0.0)
		{
			result = "SUSPEND HARI";
		}
		else if (jumlSuspend == 0 && PinaltyAmount != 0.0)
		{
			result = "BAYAR UANG";
		}
		else if (jumlSuspend != 0 && PinaltyAmount != 0.0)
		{
			result = "BAYAR UANG DAN SUSPEND HARI";
		}
		return result;
	}

	public static string DueDate(string NoPinjam, string NoItem)
	{
		string sQL = "SELECT DueDate FROM collectionloanitems WHERE CollectionLoan_id=" + NoPinjam + " AND Collection_id=" + NoItem;
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return dataTable.Rows[0]["DueDate"].ToString();
	}

	public static string BaruBolehPinjam(string NoMember)
	{
		string sQL = "";
		string text = "";
		if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			sQL = "SELECT DATE_ADD(Create_Date,INTERVAL JumlahSuspend + 1 DAY) as BolehPinjam FROM pelanggaran,members WHERE members.ID = MemberId AND members.MemberNo = '" + NoMember + "' AND JumlahSuspend > 0 ORDER BY BolehPinjam DESC NULLS LAST";
		}
		else if (Connection.ServerType == Connection.EServerType.Oracle)
		{
			sQL = "SELECT (pl.Create_Date + (pl.JumlahSuspend + 1)) as BolehPinjam FROM pelanggaran pl,members mb WHERE mb.ID = pl.MemberId AND mb.MemberNo = '" + NoMember + "' AND JumlahSuspend > 0 ORDER BY BolehPinjam DESC NULLS LAST";
		}
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0]["BolehPinjam"].ToString();
		}
		return DateTime.Now.ToString();
	}

	public static bool IsItemCanLoanOnLocation(string ItemId, bool IsNomorBarcode)
	{
		string text = "";
		text = (IsNomorBarcode ? ("SELECT Location_Id FROM COLLECTIONS WHERE NomorBarcode = '" + ItemId + "'") : ("SELECT Location_Id FROM COLLECTIONS WHERE ID = " + ItemId));
		string text2 = Command.ExecScalar(text);
		if (text2 == "62" || text2 == "75" || text2 == "76")
		{
			return true;
		}
		return false;
	}

	public static bool IsItemReferensi(string ItemId, bool IsNomorBarcode)
	{
		string text = "";
		text = (IsNomorBarcode ? (" WHERE NomorBarcode = '" + ItemId + "'") : (" WHERE COLLECTIONS.ID = " + ItemId));
		if (Util.ConvertToBoolean(Command.ExecScalar("SELECT IsReferensi FROM COLLECTIONS" + text, "0")))
		{
			return true;
		}
		string text2 = Command.ExecScalar("SELECT Catalogs.CallNumber FROM COLLECTIONS INNER JOIN Catalogs ON Collections.Catalog_Id = Catalogs.ID" + text, "");
		if (!string.IsNullOrEmpty(text2) && text2.ToUpper().StartsWith("R"))
		{
			return true;
		}
		return false;
	}

	public static bool IsItemCanLoanOnAmount(string ItemId, bool IsNomorBarcode)
	{
		string text = "";
		text = (IsNomorBarcode ? Command.ExecScalar("SELECT Catalog_Id FROM COLLECTIONS WHERE NomorBarcode = '" + ItemId + "'") : Command.ExecScalar("SELECT Catalog_Id FROM COLLECTIONS WHERE ID = " + ItemId));
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		int num = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONS WHERE Catalog_Id = " + text + " AND UPPER(Status) = 'TERSEDIA'", "0"));
		return num > 1;
	}

	public static string IsItemCanLoanWithoutQueue(string ItemId, string MemberNo)
	{
		string result = null;
		int num = int.Parse(Command.ExecScalar("select count(1) from collectionloanqueue inner join collections on collections.id=collectionloanqueue.collection_id where collections.nomorbarcode='" + ItemId + "' and sysdate < queueexpired  and queuestatus=0", "0"));
		if (num > 0)
		{
			result = "0";
			DataTable dataTable = Command.ExecDataAdapter("select id,memberno from (select collectionloanqueue.id,members.memberno from collectionloanqueue inner join members on members.id=collectionloanqueue.member_id inner join collections on collections.id=collectionloanqueue.collection_id where collections.nomorbarcode='" + ItemId + "' and sysdate < queueexpired  and queuestatus=0 order by collectionloanqueue.createdate asc) where rownum=1");
			if (dataTable.Rows.Count > 0)
			{
				string text = dataTable.Rows[0]["MemberNo"].ToString();
				if (MemberNo.Trim() == text)
				{
					result = dataTable.Rows[0]["Id"].ToString();
				}
			}
		}
		return result;
	}

	public static string getJudulStruk()
	{
		string sQL = "select value from settingparameters where Name='SirJudulStruk'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return dataTable.Rows[0]["value"].ToString().Trim();
	}

	public static string getIDCollection(string nomor_barcode)
	{
		string sQL = "select ID from collections where NomorBarcode='" + nomor_barcode.Trim() + "'";
		DataTable dataTable = Command.ExecDataAdapter(sQL, null);
		return dataTable.Rows[0]["ID"].ToString().Trim();
	}

	public static int GetJumlahPeminjaman(double MemberId)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONLOANITEMS WHERE MEMBER_ID = " + MemberId, "0"));
	}

	public static int GetJumlahPengembalianBukuTepatWaktu(double MemberId)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONLOANITEMS WHERE MEMBER_ID = " + MemberId + " AND LOANSTATUS = 'Return' AND TO_CHAR(ActualReturn, 'YYYY-MM-DD') <= TO_CHAR(DueDate, 'YYYY-MM-DD')", "0"));
	}

	public static int GetJumlahTerlambatMengembalikanBuku(double MemberId)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONLOANITEMS WHERE MEMBER_ID = " + MemberId + " AND LOANSTATUS = 'Return' AND TO_CHAR(ActualReturn, 'YYYY-MM-DD') > TO_CHAR(DueDate, 'YYYY-MM-DD')", "0"));
	}

	public static int GetJumlahBukuYangDikembalikanRusak(double MemberId)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM PELANGGARAN WHERE MEMBERID = " + MemberId + " AND JENISPELANGGARAN = 2", "0"));
	}

	public static int GetJumlahBukuYangDikembalikanHilang(double MemberId)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM PELANGGARAN WHERE MEMBERID = " + MemberId + " AND JENISPELANGGARAN = 3", "0"));
	}

	public static bool IsMemberTidakAktifMeminjamBuku(double MemberId, int Bulan)
	{
		return int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM (SELECT MAX(LoanDate) as LoanDate FROM COLLECTIONLOANITEMS WHERE MEMBER_ID = " + MemberId + " ) a WHERE MONTHS_BETWEEN(SYSDATE, LoanDate) > 1", "0")) > 0;
	}

	public static RewardPoint GetSettingRewardPoint(int Id)
	{
		RewardPoint rewardPoint = new RewardPoint();
		DataTable dataTable = Command.ExecDataAdapter("SELECT Akumulasi,Point FROM SIR_REWARD_KRITERIA WHERE Id = " + Id);
		if (dataTable.Rows.Count > 0)
		{
			rewardPoint.Akumulasi = ((!(dataTable.Rows[0]["Akumulasi"].ToString() == "Penambahan")) ? RewardAkumulasi.Pengurangan : RewardAkumulasi.Penambahan);
			rewardPoint.Point = int.Parse(dataTable.Rows[0]["Point"].ToString());
		}
		return rewardPoint;
	}

	public static RewardPoint GetRewardPeminjaman()
	{
		return GetSettingRewardPoint(1);
	}

	public static RewardPoint GetRewardPengembalianBukuTepatWaktu()
	{
		return GetSettingRewardPoint(2);
	}

	public static RewardPoint GetRewardTerlambatMengembalikanBuku()
	{
		return GetSettingRewardPoint(3);
	}

	public static RewardPoint GetRewardBukuYangDikembalikanRusak()
	{
		return GetSettingRewardPoint(4);
	}

	public static RewardPoint GetRewardBukuYangDikembalikanHilang()
	{
		return GetSettingRewardPoint(5);
	}

	public static RewardPoint GetRewardTidakAktifMeminjamBuku()
	{
		return GetSettingRewardPoint(6);
	}

	public static int GetMemberPoint(double MemberId)
	{
		int jumlahPeminjaman = GetJumlahPeminjaman(MemberId);
		int jumlahPengembalianBukuTepatWaktu = GetJumlahPengembalianBukuTepatWaktu(MemberId);
		int jumlahTerlambatMengembalikanBuku = GetJumlahTerlambatMengembalikanBuku(MemberId);
		int jumlahBukuYangDikembalikanRusak = GetJumlahBukuYangDikembalikanRusak(MemberId);
		int jumlahBukuYangDikembalikanHilang = GetJumlahBukuYangDikembalikanHilang(MemberId);
		bool flag = IsMemberTidakAktifMeminjamBuku(MemberId, 1);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		if (jumlahPeminjaman > 0)
		{
			RewardPoint rewardPeminjaman = GetRewardPeminjaman();
			num = jumlahPeminjaman * rewardPeminjaman.Point;
			if (rewardPeminjaman.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num *= -1;
			}
		}
		if (jumlahPengembalianBukuTepatWaktu > 0)
		{
			RewardPoint rewardPengembalianBukuTepatWaktu = GetRewardPengembalianBukuTepatWaktu();
			num2 = jumlahPengembalianBukuTepatWaktu * rewardPengembalianBukuTepatWaktu.Point;
			if (rewardPengembalianBukuTepatWaktu.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num2 *= -1;
			}
		}
		if (jumlahTerlambatMengembalikanBuku > 0)
		{
			RewardPoint rewardTerlambatMengembalikanBuku = GetRewardTerlambatMengembalikanBuku();
			num3 = jumlahTerlambatMengembalikanBuku * rewardTerlambatMengembalikanBuku.Point;
			if (rewardTerlambatMengembalikanBuku.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num3 *= -1;
			}
		}
		if (jumlahBukuYangDikembalikanRusak > 0)
		{
			RewardPoint rewardBukuYangDikembalikanRusak = GetRewardBukuYangDikembalikanRusak();
			num4 = jumlahBukuYangDikembalikanRusak * rewardBukuYangDikembalikanRusak.Point;
			if (rewardBukuYangDikembalikanRusak.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num4 *= -1;
			}
		}
		if (jumlahBukuYangDikembalikanHilang > 0)
		{
			RewardPoint rewardBukuYangDikembalikanHilang = GetRewardBukuYangDikembalikanHilang();
			num5 = jumlahBukuYangDikembalikanHilang * rewardBukuYangDikembalikanHilang.Point;
			if (rewardBukuYangDikembalikanHilang.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num5 *= -1;
			}
		}
		if (flag)
		{
			RewardPoint rewardTidakAktifMeminjamBuku = GetRewardTidakAktifMeminjamBuku();
			num6 = rewardTidakAktifMeminjamBuku.Point;
			if (rewardTidakAktifMeminjamBuku.Akumulasi == RewardAkumulasi.Pengurangan)
			{
				num6 *= -1;
			}
		}
		int num7 = num + num2 + num3 + num4 + num5 + num6;
		return (num7 > 0) ? num7 : 0;
	}

	public static string GetMemberRewardKategoriId(double MemberId)
	{
		int memberPoint = GetMemberPoint(MemberId);
		string text = Command.ExecScalar("SELECT Id FROM SIR_REWARD_KATEGORI WHERE " + memberPoint + " >= Range_Awal AND " + memberPoint + " <= Range_Akhir");
		if (string.IsNullOrEmpty(text))
		{
			text = Command.ExecScalar("SELECT Id FROM SIR_REWARD_KATEGORI ORDER BY Range_Akhir DESC");
		}
		return text;
	}

	public static RewardKategori GetMemberRewardKategori(double MemberId)
	{
		int memberPoint = GetMemberPoint(MemberId);
		RewardKategori rewardKategori = new RewardKategori();
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM SIR_REWARD_KATEGORI WHERE " + memberPoint + " >= Range_Awal AND " + memberPoint + " <= Range_Akhir");
		if (dataTable.Rows.Count > 0)
		{
			rewardKategori.Id = int.Parse(dataTable.Rows[0]["Id"].ToString());
			rewardKategori.Name = dataTable.Rows[0]["Name"].ToString();
			rewardKategori.Range_Awal = int.Parse(dataTable.Rows[0]["Range_Awal"].ToString());
			rewardKategori.Range_Akhir = int.Parse(dataTable.Rows[0]["Range_Akhir"].ToString());
			rewardKategori.Penambahan_Pinjam_Buku = int.Parse(dataTable.Rows[0]["Penambahan_Pinjam_Buku"].ToString());
			rewardKategori.Penambahan_Hari_Pinjam = int.Parse(dataTable.Rows[0]["Penambahan_Hari_Pinjam"].ToString());
		}
		return rewardKategori;
	}
}
