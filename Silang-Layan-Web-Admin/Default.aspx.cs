using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Default : Page
{
	public class GraphEntity
	{
		public string Name;

		public int Value;
	}

	protected string DisplayJumlahAnggota = "";

	protected string DisplayJumlahPengembalian = "";

	protected string DisplayJumlahPengiriman = "";



	protected void Page_Load(object sender, EventArgs e)
	{
		Page.Session.Add(MySession.CurrentPage, Path.GetFileName(base.Request.Path));
		LoginAuth.InitPage(Page, new int[1] { Enums.UserAuth.AllUser });
		if (!Page.IsPostBack)
		{
			LoadTahun();
		}
		LoadJumlah();
	}

	private void LoadTahun()
	{
		ddlTahun.Items.Clear();
		ddlTahun.Items.Add("--Semua Tahun--");
		for (int num = DateTime.Now.Year; num > DateTime.Now.Year - 10; num--)
		{
			ddlTahun.Items.Add(num.ToString());
		}
		ddlTahun.SelectedIndex = 1;
	}

	private void LoadJumlah()
	{
		string text = "";
		string text2 = "";
		string text3 = Command.ExecScalar("SELECT Branch_Id FROM users WHERE Id = " + UserProfileProvider.Current.Id, "");
		if (!string.IsNullOrEmpty(text3) && text3 != "0")
		{
			text = " AND Branch_Id = " + text3;
			text2 = " AND PengembalianBranch_Id = " + text3;
		}
		DisplayJumlahAnggota = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM MEMBERS WHERE 1=1" + text, "0")).ToString("N0");
		DisplayJumlahPengembalian = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM COLLECTIONLOANITEMS WHERE 1=1" + text2, "0")).ToString("N0");
		DisplayJumlahPengiriman = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM PM_KIRIM WHERE 1=1" + text, "0")).ToString("N0");
	}

	protected void ddlTahun_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	[WebMethod]
	public static List<GraphEntity> GetJumlahAnggotaPerPeriode(string Tahun, string Name)
	{
		string text = "";
		string text2 = Command.ExecScalar("SELECT Branch_Id FROM users WHERE Id = " + UserProfileProvider.Current.Id, "");
		if (!string.IsNullOrEmpty(text2) && text2 != "0")
		{
			text = " AND Branch_Id = " + text2;
		}
		List<GraphEntity> list = new List<GraphEntity>();
		if (!(Tahun != "--Semua Tahun--"))
		{
			Tahun = DateTime.Now.Year.ToString();
			DateTime dateTime = new DateTime(int.Parse(Tahun), 1, 1).AddYears(-9);
			for (int i = 1; i <= 10; i++)
			{
				int value = 0;
				if (Name == "Anggota")
				{
					int year = dateTime.Year;
					string[] array = new string[5]
					{
						"SELECT COUNT(*) FROM MEMBERS WHERE 1=1",
						text,
						" AND (TO_CHAR(MEMBERS.CreateDate,'YYYY') <= '",
						year.ToString(),
						"')"
					};
					value = int.Parse(Command.ExecScalar(string.Concat(array), "0"));
				}
				GraphEntity graphEntity = new GraphEntity();
				graphEntity.Name = dateTime.Year.ToString();
				graphEntity.Value = value;
				list.Add(graphEntity);
				dateTime = dateTime.AddYears(1);
			}
		}
		else
		{
			DateTime dateTime = new DateTime(int.Parse(Tahun), 1, 1);
			for (int i = 1; i <= 12; i++)
			{
				string name = new DateTime(dateTime.Year, dateTime.Month, 1).ToString("MMM", CultureInfo.CreateSpecificCulture("id"));
				int value = 0;
				if (Name == "Anggota")
				{
					string[] array = new string[5]
					{
						"SELECT COUNT(*) FROM MEMBERS WHERE 1=1",
						text,
						" AND TO_CHAR(MEMBERS.CreateDate,'YYYY-MM') <= '",
						dateTime.ToString("yyyy-MM"),
						"'"
					};
					value = int.Parse(Command.ExecScalar(string.Concat(array), "0"));
				}
				GraphEntity graphEntity2 = new GraphEntity();
				graphEntity2.Name = name;
				graphEntity2.Value = value;
				GraphEntity item = graphEntity2;
				list.Add(item);
				dateTime = dateTime.AddMonths(1);
			}
		}
		return list;
	}
}
