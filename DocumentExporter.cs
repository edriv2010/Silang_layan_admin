using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


public class DocumentExporter
{
	public enum DocumentType
	{
		MicrosoftOffice,
		OpenOffice
	}

	public delegate DataTable LoadDataForExport();

	public static LoadDataForExport AcceptorLoadDataForExport;

	public static void ClearControls(Control control)
	{
		for (int num = control.Controls.Count - 1; num >= 0; num--)
		{
			ClearControls(control.Controls[num]);
		}
		if (control is TableCell)
		{
			return;
		}
		if (control.GetType().GetProperty("SelectedItem") != null)
		{
			LiteralControl literalControl = new LiteralControl();
			control.Parent.Controls.Add(literalControl);
			try
			{
				literalControl.Text = (string)control.GetType().GetProperty("SelectedItem").GetValue(control, null);
			}
			catch
			{
			}
			control.Parent.Controls.Remove(control);
		}
		else if (control.GetType().GetProperty("Text") != null)
		{
			LiteralControl literalControl = new LiteralControl();
			control.Parent.Controls.Add(literalControl);
			literalControl.Text = (string)control.GetType().GetProperty("Text").GetValue(control, null);
			control.Parent.Controls.Remove(control);
		}
	}

	public static void ExportToExcel(Page Page, DataGrid dgData, string FileName, LoadDataForExport AcceptorLoadDataForExport, DocumentType DocumentType = DocumentType.MicrosoftOffice)
	{
		DocumentExporter.AcceptorLoadDataForExport = AcceptorLoadDataForExport;
		dgData.Columns.Clear();
		dgData.AutoGenerateColumns = true;
		DataTable dataSource = AcceptorLoadDataForExport();
		dgData.DataSource = dataSource;
		dgData.DataBind();
		Page.Response.Clear();
		Page.Response.Buffer = true;
		Page.Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
		Page.Response.ContentType = "application/vnd.ms-excel";
		Page.Response.Charset = "";
		Page.EnableViewState = false;
		StringWriter stringWriter = new StringWriter();
		HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
		ClearControls(dgData);
		string value = "<style> TD { mso-number-format:\\@; } </style> ";
		htmlTextWriter.WriteLine(value);
		dgData.RenderControl(htmlTextWriter);
		Page.Response.Write(stringWriter.ToString());
		Page.Response.End();
	}

	public static void ExportToWord(Page Page, DataGrid dgData, string FileName, LoadDataForExport AcceptorLoadDataForExport, DocumentType DocumentType = DocumentType.MicrosoftOffice)
	{
		DocumentExporter.AcceptorLoadDataForExport = AcceptorLoadDataForExport;
		dgData.Columns.Clear();
		dgData.AutoGenerateColumns = true;
		DataTable dataSource = AcceptorLoadDataForExport();
		dgData.DataSource = dataSource;
		dgData.DataBind();
		Page.Response.Clear();
		Page.Response.Buffer = true;
		Page.Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".doc");
		Page.Response.ContentType = "application/vnd.ms-word";
		Page.Response.Charset = "";
		Page.EnableViewState = false;
		StringWriter stringWriter = new StringWriter();
		HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
		ClearControls(dgData);
		dgData.RenderControl(writer);
		Page.Response.Write(stringWriter.ToString());
		Page.Response.End();
	}

	public static void ExportToXML(Page Page, string FileName, LoadDataForExport AcceptorLoadDataForExport)
	{
		DocumentExporter.AcceptorLoadDataForExport = AcceptorLoadDataForExport;
		DataTable dataTable = AcceptorLoadDataForExport();
		string fileName = Page.MapPath("~/" + MyApplication.XMLFolder + "/" + FileName + ".xml");
		dataTable.WriteXml(fileName);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<script type='text/javascript'>");
		stringBuilder.Append("w = ((screen.width - 600) / 2);");
		stringBuilder.Append("h = ((screen.height - 400) / 2);");
		stringBuilder.Append("var Link = '" + MyApplication.XMLFolder + "/" + FileName + ".xml';");
		stringBuilder.Append("newwindow = window.open(Link, '_blank', 'width=600,height=400,left=' + w + ',top=' + h + ',resizable=yes,scrollbars=yes,toolbar=yes,location=yes,directories=no,status=yes,menubar=yes,copyhistory=no');");
		stringBuilder.Append("if (window.focus) {newwindow.focus()}");
		stringBuilder.Append("</script>");
		ScriptManager.RegisterClientScriptBlock(Page, typeof(UpdatePanel), Guid.NewGuid().ToString(), stringBuilder.ToString(), addScriptTags: false);
	}

	public static void ExportToPDF(Page Page, string FileName, string Contents)
	{
		Page.Response.ContentType = "application/pdf";
		Page.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
		Page.Response.Charset = "iso-8859-2";
		Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
		StringWriter stringWriter = new StringWriter();
		HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
		htmlTextWriter.Write(Contents);
		StringReader reader = new StringReader(stringWriter.ToString());
		Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
		HTMLWorker hTMLWorker = new HTMLWorker(document);
		PdfWriter.GetInstance(document, (Stream)(object)Page.Response.OutputStream);
		document.Open();
		hTMLWorker.Parse((TextReader)(object)reader);
		document.Close();
		Page.Response.Write(document);
		Page.Response.End();
	}

	public static void ExportDataTableToExcel(Page Page, string FileName, DataTable dt, DocumentType DocumentType = DocumentType.MicrosoftOffice)
	{
		string value = "attachment; filename=" + FileName + ".xls";
		Page.Response.Clear();
		Page.Response.AddHeader("content-disposition", value);
		if (DocumentType == DocumentType.OpenOffice)
		{
			Page.Response.ContentType = "application/vnd.scalc.exe - o %1";
		}
		else
		{
			Page.Response.ContentType = "application/vnd.ms-excel";
		}
		string text = "";
		foreach (DataColumn column in dt.Columns)
		{
			Page.Response.Write(text + column.ColumnName);
			text = "\t";
		}
		Page.Response.Write("\n");
		foreach (DataRow row in dt.Rows)
		{
			text = "";
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				Page.Response.Write(text + "=\"" + row[i].ToString() + "\"");
				text = "\t";
			}
			Page.Response.Write("\n");
		}
		Page.Response.End();
	}

	public static void ExportDataTableToWord(Page Page, string FileName, DataTable dt, DocumentType DocumentType = DocumentType.MicrosoftOffice)
	{
		string value = "attachment; filename=" + FileName + ".doc";
		Page.Response.Clear();
		Page.Response.AddHeader("content-disposition", value);
		if (DocumentType == DocumentType.OpenOffice)
		{
			Page.Response.ContentType = "application/vnd.scalc.exe - o %1";
		}
		else
		{
			Page.Response.ContentType = "application/vnd.ms-word";
		}
		string text = "";
		foreach (DataColumn column in dt.Columns)
		{
			Page.Response.Write(text + column.ColumnName);
			text = "\t";
		}
		Page.Response.Write("\n");
		foreach (DataRow row in dt.Rows)
		{
			text = "";
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				Page.Response.Write(text + row[i].ToString());
				text = "\t";
			}
			Page.Response.Write("\n");
		}
		Page.Response.End();
	}

	public static void ExportDataTableToXML(Page Page, string FileName, DataTable dt)
	{
		dt.TableName = FileName;
		string fileName = Page.MapPath("~/" + MyApplication.XMLFolder + "/" + FileName + ".xml");
		dt.WriteXml(fileName);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<script type='text/javascript'>");
		stringBuilder.Append("w = ((screen.width - 600) / 2);");
		stringBuilder.Append("h = ((screen.height - 400) / 2);");
		stringBuilder.Append("var Link = '" + MyApplication.XMLFolder + "/" + FileName + ".xml';");
		stringBuilder.Append("newwindow = window.open(Link, '_blank', 'width=600,height=400,left=' + w + ',top=' + h + ',resizable=yes,scrollbars=yes,toolbar=yes,location=yes,directories=no,status=yes,menubar=yes,copyhistory=no');");
		stringBuilder.Append("if (window.focus) {newwindow.focus()}");
		stringBuilder.Append("</script>");
		ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), Guid.NewGuid().ToString(), stringBuilder.ToString(), addScriptTags: false);
	}
}
