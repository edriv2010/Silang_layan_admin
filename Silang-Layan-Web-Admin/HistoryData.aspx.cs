using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HistoryData : Page
{
	protected string TableName = "";

	protected string RefID = "";


	protected void Page_Load(object sender, EventArgs e)
	{
		LoginAuth.InitPage(Page, new int[1] { Enums.UserAuth.AllUser });
		if (!Page.IsPostBack && Page.Request["t"] != null && Page.Request["id"] != null)
		{
			TableName = Page.Request["t"].ToString();
			RefID = Page.Request["id"].ToString();
			LoadData(TableName, RefID);
		}
	}

	private void LoadData(string TableName, string RefID)
	{
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM HistoryData WHERE UPPER(TableName)='" + TableName.ToUpper() + "' AND IDRef='" + RefID + "' ORDER BY ActionDate DESC");
		dataTable.Columns.Add("No");
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			dataTable.Rows[i]["No"] = i + 1;
		}
		dgData.DataSource = dataTable;
		dgData.DataBind();
	}
}
