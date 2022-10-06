using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MARC;

public class Catalog
{
	public enum BIBIDStatus
	{
		New,
		Available
	}

	public struct BIBID
	{
		public BIBIDStatus Status;

		public string Value;
	}

	public struct MARCPlusRuas
	{
		public string MARC;

		public string MARC_LOC;

		public List<ControlField> ListControlField;

		public List<VariableField> ListVariableField;

		public Record Record;
	}

	public struct CatalogDatas
	{
		public DataTable Data;

		public int DataCount;
	}

	public static string GetNewControlNumber(int FormatID)
	{
		string text = MyApplication.BranchCode;
		string text2 = new string(char.Parse("0"), 15);
		string text3 = text + text2;
		string text4 = "";
		if (FormatID == 2)
		{
			text = "AUTH";
			text4 = Command.ExecScalar("SELECT MAX(ID) FROM Auth_Header", "");
		}
		else
		{
			text4 = Command.ExecScalar("SELECT MAX(ControlNumber) FROM Catalogs WHERE ControlNumber LIKE '" + text + "0%'", "");
		}
		double num = 0.0;
		if (text4 != "")
		{
			text4 = Regex.Match(text4.Replace(text, ""), "[\\d].*[\\d]").Value.Replace("-", "");
			string text5 = text4;
			if (text5.Length < 15)
			{
				text5 += new string(char.Parse("0"), 15 - text5.Length);
			}
			num = double.Parse(text5);
		}
		double num2 = num + 1.0;
		return text + string.Format("{0:" + text2 + "}", num2);
	}

	public static BIBID GetNewBIBID(int FormatID)
	{
		BIBID result = default(BIBID);
		result.Status = BIBIDStatus.New;
		if (FormatID == 2)
		{
			string text = new string(char.Parse("0"), 11);
			int num = int.Parse(Command.ExecScalar("SELECT MAX(ID) FROM auth_header", "0"));
			string text2 = "AUTH-" + string.Format("{0:" + text + "}", num + 1);
			while (!string.IsNullOrEmpty(Command.ExecScalar("SELECT ID FROM auth_header WHERE AUTH_ID = '" + text2 + "'")))
			{
				num++;
				text2 = "AUTH-" + string.Format("{0:" + text + "}", num + 1);
			}
			result.Value = text2;
			return result;
		}
		string text3 = "";
		string bIBIDCode = MyApplication.BIBIDCode;
		string text4 = "{DateTime.Now:MMyy}";
		string text5 = new string(char.Parse("0"), 11);
		if (Connection.ServerType == Connection.EServerType.Oracle)
		{
			text5 = new string(char.Parse("0"), 6);
		}
		int num2 = (bIBIDCode + text4).Length + 1;
		int length = (bIBIDCode + text4 + text5).Length;
		while (true)
		{
			if (MyApplication.IsBIBIDAvailable)
			{
				text3 = Command.ExecScalar("SELECT MIN(BIBID) FROM bibidavailable", "");
			}
			if (text3 == "")
			{
				text3 = Command.ExecScalar("SELECT SUBSTR(MAX(BIBID)," + num2 + ") FROM Catalogs WHERE BIBID LIKE '" + bIBIDCode + text4 + "%' AND LENGTH(BIBID)=" + length, "");
			}
			else
			{
				result.Status = BIBIDStatus.Available;
			}
			if (result.Status == BIBIDStatus.Available)
			{
				result.Value = text3;
				int num3 = int.Parse(Command.ExecScalar("SELECT COUNT(*) FROM catalogs WHERE BIBID='" + text3 + "'", "0"));
				if (num3 > 0)
				{
					Command.ExecNonQuery("DELETE FROM bibidavailable WHERE BIBID='" + text3 + "'");
					continue;
				}
				break;
			}
			double num4 = 0.0;
			if (!string.IsNullOrEmpty(text3))
			{
				num4 = double.Parse(text3);
			}
			double num5 = num4 + 1.0;
			string value = bIBIDCode + string.Format("{0:" + text4 + text5 + "}", num5);
			result.Value = value;
			break;
		}
		return result;
	}

	public static Record GetMARC(RecordFactory.ListLeader ListLeader, List<ControlField> ControlFields, List<VariableField> VariableFields, MarcConverter.MARCFORMAT Format, int WorskheetID)
	{
		MarcConverter marcConverter = new MarcConverter();
		return marcConverter.ConvertAsRecord(ListLeader, ControlFields, VariableFields, Format, WorskheetID);
	}

	public static string GetMARC(string CatalogID, MarcConverter.MARCFORMAT FormatType)
	{
		return GetMARC(CatalogID, FormatType, "catalogs");
	}

	public static string GetMARC(string CatalogID, MarcConverter.MARCFORMAT FormatType, string TableName)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return "";
		}
		if (FormatType == MarcConverter.MARCFORMAT.PERPUSNAS)
		{
			return Command.ExecScalar("SELECT MARC FROM " + TableName + " WHERE id=" + CatalogID, "");
		}
		return Command.ExecScalar("SELECT MARC_LOC FROM " + TableName + " WHERE id=" + CatalogID, "");
	}

	public static string GetMARCAuthority(string CatalogID, MarcConverter.MARCFORMAT FormatType)
	{
		return GetMARCAuthority(CatalogID, FormatType, "GetMARCAuthority");
	}

	public static string GetMARCAuthority(string CatalogID, MarcConverter.MARCFORMAT FormatType, string TableName)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return "";
		}
		if (FormatType == MarcConverter.MARCFORMAT.PERPUSNAS)
		{
			return Command.ExecScalar("SELECT MARC FROM " + TableName + " WHERE id=" + CatalogID, "");
		}
		return Command.ExecScalar("SELECT MARC_LOC FROM " + TableName + " WHERE id=" + CatalogID, "");
	}

	public static string GetMARCXML(string CatalogID, string FieldDelimiter, string SubFieldDelimiter, MarcConverter.XMLFORMAT xmlFormat)
	{
		return GetMARCXML(CatalogID, FieldDelimiter, SubFieldDelimiter, "catalogs", xmlFormat);
	}

	public static string GetMARCXML(string CatalogID, string FieldDelimiter, string SubFieldDelimiter, string TableName, MarcConverter.XMLFORMAT xmlFormat)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return "";
		}
		string text = Command.ExecScalar("SELECT MARC FROM " + TableName + " WHERE id=" + CatalogID, "");
		if (string.IsNullOrEmpty(text))
		{
			Util.ShowAlertMessage("Data tidak mempunyai data MARC");
			return "";
		}
		MarcConverter marcConverter = new MarcConverter();
		Record r = marcConverter.ConvertAsRecord(text, FieldDelimiter, SubFieldDelimiter, MarcConverter.MARCFORMAT.PERPUSNAS);
		r = CheckBIBID(r, CatalogID);
		return marcConverter.ExportXMLFormat(r, xmlFormat);
	}

	public static string GetMARCXMLAuthority(string CatalogID, string FieldDelimiter, string SubFieldDelimiter, MarcConverter.XMLFORMAT xmlFormat)
	{
		return GetMARCXMLAuthority(CatalogID, FieldDelimiter, SubFieldDelimiter, "Auth_Header", xmlFormat);
	}

	public static string GetMARCXMLAuthority(string AuthHeaderID, string FieldDelimiter, string SubFieldDelimiter, string TableName, MarcConverter.XMLFORMAT xmlFormat)
	{
		string text = Command.ExecScalar("SELECT MARC FROM " + TableName + " WHERE id='" + AuthHeaderID + "'", "");
		if (string.IsNullOrEmpty(text))
		{
			Util.ShowAlertMessage("Data tidak mempunyai data MARC");
			return "";
		}
		MarcConverter marcConverter = new MarcConverter();
		return marcConverter.ExportXMLFormat(marcConverter.ConvertAsRecord(text, FieldDelimiter, SubFieldDelimiter, MarcConverter.MARCFORMAT.PERPUSNAS), xmlFormat);
	}

	public static string GetFieldContent(string CatalogID, string TagCode, string SubRuas, string FieldDelimiter, string SubFieldDelimiter, MarcConverter.MARCFORMAT FormatType)
	{
		string mARC = GetMARC(CatalogID, FormatType);
		if (string.IsNullOrEmpty(mARC))
		{
			Util.ShowAlertMessage("Data MARC tidak ada!");
			return "";
		}
		string tagData = "";
		MarcConverter marcConverter = new MarcConverter();
		Record r = marcConverter.ConvertAsRecord(mARC, FieldDelimiter, SubFieldDelimiter, MarcConverter.MARCFORMAT.PERPUSNAS);
		r = CheckBIBID(r, CatalogID);
		r.GetTagData(TagCode, SubRuas, SubFieldDelimiter, out tagData);
		return tagData;
	}

	public static string GetFieldContentFromMARC(string MARC, string TagCode, string SubRuas, string FieldDelimiter, string SubFieldDeliminator)
	{
		string tagData = "";
		MarcConverter marcConverter = new MarcConverter();
		marcConverter.ConvertAsRecord(MARC, FieldDelimiter, SubFieldDeliminator, MarcConverter.MARCFORMAT.PERPUSNAS).GetTagData(TagCode, SubRuas, SubFieldDeliminator, out tagData);
		return tagData;
	}

	public static ArrayList GetFields(string MARC, string TagCode, string FieldDelimiter, string SubFieldDeliminator)
	{
		MarcConverter marcConverter = new MarcConverter();
		ArrayList arrayList = new ArrayList();
		string text = Command.ExecScalar("SELECT ID FROM fields WHERE tag='" + TagCode + "'");
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Delimiter FROM fielddatas WHERE Field_id=" + text);
		arrayList = marcConverter.ConvertAsRecord(MARC, FieldDelimiter, SubFieldDeliminator, MarcConverter.MARCFORMAT.PERPUSNAS).GetTagDatas(TagCode, SubFieldDeliminator);
		if (arrayList == null)
		{
			return new ArrayList();
		}
		for (int i = 0; i < arrayList.Count; i++)
		{
			string text2 = arrayList[i].ToString();
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				string text3 = dataTable.Rows[j]["Code"].ToString();
				string text4 = dataTable.Rows[j]["Delimiter"].ToString();
				text2 = text2.Replace("$" + text3, text4);
				if (!string.IsNullOrEmpty(text4))
				{
					text2 = text2.Replace(text4 + " " + text4, text4).Replace(text4 + text4, text4);
				}
			}
			arrayList[i] = text2;
		}
		return arrayList;
	}

	public static ArrayList GetFields(string MARC, string TagCode, string SubRuas, string FieldDelimiter, string SubFieldDeliminator)
	{
		MarcConverter marcConverter = new MarcConverter();
		string text = Command.ExecScalar("SELECT ID FROM fields WHERE tag='" + TagCode + "'");
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Delimiter FROM fielddatas WHERE Field_id=" + text);
		return marcConverter.ConvertAsRecord(MARC, FieldDelimiter, SubFieldDeliminator, MarcConverter.MARCFORMAT.PERPUSNAS).GetTagDatas(TagCode, SubRuas, SubFieldDeliminator);
	}

	public static DataTable GetFieldsOnTable(Record r, string TagCode, string FieldDelimiter, string SubFieldDeliminator)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.Add("Value");
		string text = Command.ExecScalar("SELECT ID FROM fields WHERE tag='" + TagCode + "' AND Format_ID=1");
		DataTable dataTable2 = Command.ExecDataAdapter("SELECT Code,Delimiter FROM fielddatas WHERE Field_id=" + text);
		dataTable = r.GetTagDatasOnTable(TagCode, SubFieldDeliminator);
		if (dataTable == null)
		{
			return new DataTable();
		}
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text2 = dataTable.Rows[i][0].ToString();
			string text3 = "";
			switch (TagCode)
			{
			default:
				if (!(TagCode == "711"))
				{
					break;
				}
				goto case "100";
			case "100":
			case "110":
			case "111":
			case "700":
			case "710":
				text3 = Field.GetCleanSubFieldContent(text2, int.Parse(text), "e");
				break;
			}
			for (int j = 0; j < dataTable2.Rows.Count; j++)
			{
				string text4 = dataTable2.Rows[j]["Code"].ToString();
				string text5 = dataTable2.Rows[j]["Delimiter"].ToString();
				text2 = ((!string.IsNullOrEmpty(text5)) ? text2.Replace("$" + text4, text5).Replace(text5 + text5, text5) : text2.Replace("$" + text4, text5));
			}
			if (!string.IsNullOrEmpty(text3))
			{
				text2 = text2.Replace(text3, "(" + text3 + ")");
			}
			dataTable.Rows[i][0] = text2;
		}
		return dataTable;
	}

	public static DataTable GetFieldsOnTable(Record r, string TagCode, string SubRuas, string FieldDelimiter, string SubFieldDeliminator)
	{
		MarcConverter marcConverter = new MarcConverter();
		string text = Command.ExecScalar("SELECT ID FROM fields WHERE tag='" + TagCode + "' AND Format_ID=1");
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Delimiter FROM fielddatas WHERE Field_id=" + text);
		return r.GetTagDatasOnTable(TagCode, SubRuas, SubFieldDeliminator);
	}

	private Record LoadTagFromMARC(string MARC)
	{
		if (string.IsNullOrEmpty(MARC))
		{
			return null;
		}
		string text = "PERPUSNAS";
		try
		{
			MarcConverter marcConverter = new MarcConverter();
			Record result = null;
			if (text == "PERPUSNAS")
			{
				result = marcConverter.ConvertAsRecord(MARC, "^", "$", MarcConverter.MARCFORMAT.PERPUSNAS);
			}
			else if (text == "LOC")
			{
				result = marcConverter.ConvertAsRecord(MARC, char.ConvertFromUtf32(30), char.ConvertFromUtf32(31), MarcConverter.MARCFORMAT.LOC);
			}
			return result;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static Record CheckBIBID(Record r, string CatalogID)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return r;
		}
		if (r.VariableField != null)
		{
			VariableField variableField = r.VariableField.Where((VariableField x) => x.Tag == "035").FirstOrDefault();
			if (variableField == null)
			{
				string text = Command.ExecScalar("SELECT BIBID FROM CATALOGS WHERE ID=" + CatalogID);
				DataField dataField = new DataField();
				dataField.Tag = "a";
				dataField.Value = text;
				dataField.Length = text.Length;
				DataField item = dataField;
				r.VariableField.Insert(0, new VariableField
				{
					Tag = "035",
					Name = "BIB ID",
					Fixed = false,
					Repeatable = false,
					Indicator1 = "#",
					Indicator2 = "#",
					Value = "$a " + text,
					Length = ("$a " + text).Length,
					DataFields = new List<DataField> { item }
				});
			}
		}
		return r;
	}

	public static string GetTag006Value(string TagCode, string Value, string WorksheetID, string FieldID)
	{
		string text = "";
		if (TagCode == "008" && !string.IsNullOrEmpty(Value))
		{
			string text2 = Command.ExecScalar("SELECT ID FROM WorksheetFields WHERE Worksheet_id=" + WorksheetID + " AND Field_id=" + FieldID);
			if (!string.IsNullOrEmpty(text2))
			{
				DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM WorksheetfieldItems WHERE Worksheetfield_id=" + text2 + " ORDER BY StartPosition");
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string text3 = "";
					int length = int.Parse(dataTable.Rows[i]["Length"].ToString());
					string text4 = dataTable.Rows[i]["DefaultValue"].ToString();
					string value = dataTable.Rows[i]["IdemTag"].ToString().Trim();
					int startIndex = int.Parse(dataTable.Rows[i]["IdemStartPosition"].ToString());
					text3 = text4;
					if (!string.IsNullOrEmpty(value))
					{
						text3 = Value.Substring(startIndex, length);
					}
					text += text3;
				}
			}
		}
		return text;
	}

	public static string GetTag006Value(string TagCode, string Value, string WorksheetID, string FieldID, string Tag006Pos00)
	{
		string text = "";
		if (TagCode == "008" && !string.IsNullOrEmpty(Value))
		{
			string text2 = Command.ExecScalar("SELECT ID FROM WorksheetFields WHERE Worksheet_id=" + WorksheetID + " AND Field_id=" + FieldID);
			if (!string.IsNullOrEmpty(text2))
			{
				DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM WorksheetfieldItems WHERE Worksheetfield_id=" + text2 + " ORDER BY StartPosition");
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string text3 = "";
					int length = int.Parse(dataTable.Rows[i]["Length"].ToString());
					string text4 = dataTable.Rows[i]["DefaultValue"].ToString();
					string value = dataTable.Rows[i]["IdemTag"].ToString().Trim();
					int startIndex = int.Parse(dataTable.Rows[i]["IdemStartPosition"].ToString());
					if (!string.IsNullOrEmpty(Tag006Pos00.Trim()))
					{
						text4 = Tag006Pos00;
					}
					text3 = text4;
					if (!string.IsNullOrEmpty(value))
					{
						text3 = Value.Substring(startIndex, length);
					}
					text += text3;
				}
			}
		}
		return text;
	}

	public static MARCPlusRuas ConvertTagsToMARC(DataTable dtTags, int WorksheetID)
	{
		List<ControlField> list = new List<ControlField>();
		List<VariableField> list2 = new List<VariableField>();
		for (int i = 0; i <= dtTags.Rows.Count - 1; i++)
		{
			string text = dtTags.Rows[i]["TAG"].ToString();
			string text2 = dtTags.Rows[i]["VALUE"].ToString();
			string text3 = dtTags.Rows[i]["INDICATOR1"].ToString().Trim();
			string text4 = dtTags.Rows[i]["INDICATOR2"].ToString().Trim();
			if (int.Parse(text) <= 10)
			{
				ControlField controlField = new ControlField();
				controlField.Tag = text;
				controlField.Name = "";
				controlField.Fixed = true;
				controlField.Repeatable = false;
				controlField.Value = text2;
				controlField.Length = controlField.Value.Length;
				list.Add(controlField);
				continue;
			}
			string text5 = text2;
			if (!string.IsNullOrEmpty(text5) && !(text5 == "$a") && (text5.Length > 2 || !text5.StartsWith("$")))
			{
				if (string.IsNullOrEmpty(text3) || string.IsNullOrWhiteSpace(text3))
				{
					text3 = "#";
				}
				if (string.IsNullOrEmpty(text4) || string.IsNullOrWhiteSpace(text4))
				{
					text4 = "#";
				}
				text3 = text3.Substring(0, 1);
				text4 = text4.Substring(0, 1);
				if (!text5.StartsWith("$"))
				{
					text5 = "$a " + text5;
				}
				VariableField variableField = new VariableField();
				variableField.Tag = text;
				variableField.Indicator1 = text3;
				variableField.Indicator2 = text4;
				variableField.Value = text5;
				variableField.Length = (variableField.Indicator1 + variableField.Indicator2 + text5).Length;
				list2.Add(variableField);
			}
		}
		List<VariableField> list3 = new List<VariableField>();
		if (list2.Count > 0)
		{
			for (int j = 0; j <= list2.Count - 1; j++)
			{
				string value = list2[j].Value;
				VariableField variableField2 = new VariableField();
				variableField2.Tag = list2[j].Tag;
				variableField2.Indicator1 = list2[j].Indicator1;
				variableField2.Indicator2 = list2[j].Indicator2;
				variableField2.Value = value;
				variableField2.Length = (variableField2.Indicator1 + variableField2.Indicator2 + variableField2.Value).Length;
				string[] array = value.Split(new string[1] { "$" }, StringSplitOptions.RemoveEmptyEntries);
				List<DataField> list4 = new List<DataField>();
				string[] array2 = array;
				foreach (string text6 in array2)
				{
					DataField dataField = new DataField();
					string text7 = Util.CollapseSpaces(text6.Trim());
					if (text7.Length > 1)
					{
						if (int.Parse(variableField2.Tag) < 10 || int.Parse(variableField2.Tag) == 35)
						{
							dataField.Tag = "";
							dataField.Value = Util.CollapseSpaces(text7.Trim());
						}
						else
						{
							dataField.Tag = text7.Substring(0, 1);
							dataField.Value = Util.CollapseSpaces(text7.Substring(1).Trim());
						}
						dataField.Length = dataField.Value.Length;
						list4.Add(dataField);
					}
				}
				variableField2.DataFields = list4;
				list3.Add(variableField2);
			}
		}
		RecordFactory.ListLeader listLeader = new RecordFactory.ListLeader();
		SetLeaderValue(listLeader, false, WorksheetID);
		MarcConverter marcConverter = new MarcConverter();
		string mARC = marcConverter.ConvertAsMARC(listLeader, list, list3, MarcConverter.MARCFORMAT.PERPUSNAS, WorksheetID);
		string mARC_LOC = marcConverter.ConvertAsMARC(listLeader, list, list3, MarcConverter.MARCFORMAT.LOC, WorksheetID);
		Record record = new Record();
		record.ControlField = list;
		record.VariableField = list3;
		MARCPlusRuas result = default(MARCPlusRuas);
		result.MARC = mARC;
		result.MARC_LOC = mARC_LOC;
		result.ListControlField = list;
		result.ListVariableField = list3;
		result.Record = record;
		return result;
	}

	public static string GetBriefInformation(string CatalogID)
	{
		return GetBriefInformation(CatalogID, "catalogs");
	}

	public static string GetBriefInformation(string CatalogID, string TableName)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return "";
		}
		string result = "";
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM " + TableName + " WHERE id=" + CatalogID);
		if (dataTable.Rows.Count > 0)
		{
			string text = dataTable.Rows[0]["CallNumber"].ToString();
			string text2 = dataTable.Rows[0]["Author"].ToString();
			string text3 = dataTable.Rows[0]["Title"].ToString();
			string text4 = dataTable.Rows[0]["Edition"].ToString();
			string text5 = dataTable.Rows[0]["PublishLocation"].ToString() + " : " + dataTable.Rows[0]["Publisher"].ToString() + " : " + dataTable.Rows[0]["PublishYear"].ToString();
			string text6 = dataTable.Rows[0]["Paging"].ToString() + " ; " + dataTable.Rows[0]["Ill"].ToString() + " ; " + dataTable.Rows[0]["Sizes"].ToString() + " ; " + dataTable.Rows[0]["Item"].ToString();
			string text7 = dataTable.Rows[0]["Subject"].ToString();
			string text8 = dataTable.Rows[0]["ISBN"].ToString();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Nomor Panggil\t: " + text);
			stringBuilder.AppendLine("Pengarang\t: " + text2);
			stringBuilder.AppendLine("Judul\t: " + text3);
			stringBuilder.AppendLine("Edisi\t: " + text4);
			stringBuilder.AppendLine("Terbitan\t: " + text5);
			stringBuilder.AppendLine("Deskripsi\t: " + text6);
			stringBuilder.AppendLine("Subjek\t: " + text7);
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine("ISBN\t: " + text8);
			}
			else
			{
				string fieldContentFromMARC = GetFieldContentFromMARC(dataTable.Rows[0]["MARC"].ToString(), "022", "a", "^", "$");
				if (!string.IsNullOrEmpty(fieldContentFromMARC))
				{
					stringBuilder.AppendLine("ISSN\t: " + fieldContentFromMARC);
				}
			}
			result = stringBuilder.ToString();
		}
		return result;
	}

	public static string GetFullInformation(string CatalogID)
	{
		return GetFullInformation(CatalogID, "catalogs");
	}

	public static string GetFullInformation(string CatalogID, string TableName)
	{
		if (string.IsNullOrEmpty(CatalogID))
		{
			return "";
		}
		string result = "";
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM " + TableName + " WHERE id=" + CatalogID);
		if (dataTable.Rows.Count > 0)
		{
			string text = dataTable.Rows[0]["Worksheet_id"].ToString();
			string text2 = Command.ExecScalar("SELECT Name FROM worksheets WHERE ID=" + text);
			string text3 = (string.IsNullOrEmpty(dataTable.Rows[0]["CallNumber"].ToString()) ? GetFieldContentFromMARC(dataTable.Rows[0]["MARC"].ToString(), "090", "a", "^", "$") : dataTable.Rows[0]["CallNumber"].ToString());
			string text4 = dataTable.Rows[0]["Author"].ToString();
			string text5 = dataTable.Rows[0]["Title"].ToString();
			string text6 = (dataTable.Rows[0]["Publisher"].ToString().Contains(":") ? dataTable.Rows[0]["Publisher"].ToString() : (dataTable.Rows[0]["PublishLocation"].ToString() + (dataTable.Rows[0]["PublishLocation"].ToString().Contains(":") ? " " : " : ") + dataTable.Rows[0]["Publisher"].ToString() + (dataTable.Rows[0]["Publisher"].ToString().Contains(",") ? " " : ", ") + dataTable.Rows[0]["PublishYear"].ToString()));
			string text7 = dataTable.Rows[0]["Description"].ToString();
			string text8 = dataTable.Rows[0]["Note"].ToString();
			string text9 = dataTable.Rows[0]["Subject"].ToString();
			string text10 = dataTable.Rows[0]["ISBN"].ToString();
			string text11 = dataTable.Rows[0]["DeweyNo"].ToString();
			string fieldContentFromMARC = GetFieldContentFromMARC(dataTable.Rows[0]["MARC"].ToString(), "440", "a", "^", "$");
			string fieldContentFromMARC2 = GetFieldContentFromMARC(dataTable.Rows[0]["MARC"].ToString(), "250", "a", "^", "$");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Jenis Bahan Pustaka\t: " + text2);
			stringBuilder.AppendLine("Nomor Panggil\t: " + text3);
			stringBuilder.AppendLine("Pengarang\t: " + text4);
			stringBuilder.AppendLine("Judul\t: " + text5);
			if (!string.IsNullOrEmpty(fieldContentFromMARC))
			{
				stringBuilder.AppendLine("Judul Seri\t: " + fieldContentFromMARC);
			}
			if (!string.IsNullOrEmpty(fieldContentFromMARC2))
			{
				stringBuilder.AppendLine("Edisi\t: " + fieldContentFromMARC2);
			}
			stringBuilder.AppendLine("Terbitan\t: " + text6);
			stringBuilder.AppendLine("Deskripsi\t: " + text7);
			stringBuilder.AppendLine("Subjek\t: " + text9);
			if (!string.IsNullOrEmpty(text8))
			{
				stringBuilder.AppendLine("Catatan\t: " + text8);
			}
			if (!string.IsNullOrEmpty(text10))
			{
				stringBuilder.AppendLine("ISBN\t: " + text10);
			}
			else
			{
				string fieldContentFromMARC3 = GetFieldContentFromMARC(dataTable.Rows[0]["MARC"].ToString(), "022", "a", "^", "$");
				if (!string.IsNullOrEmpty(fieldContentFromMARC3))
				{
					stringBuilder.AppendLine("ISSN\t: " + fieldContentFromMARC3);
				}
			}
			stringBuilder.AppendLine("DDC\t: " + text11);
			result = stringBuilder.ToString();
		}
		return result;
	}

	public static string CreateXMLSemantic(double CatalogID, bool IsUpdate)
	{
		DataTable dataTable = Command.ExecDataAdapter("SELECT * FROM catalogs WHERE ID=" + CatalogID);
		if (dataTable.Rows.Count > 0)
		{
			string text = Util.CollapseSpaces(dataTable.Rows[0]["Title"].ToString().Replace("\"", "&quot;").Trim());
			string text2 = Util.CollapseSpaces(dataTable.Rows[0]["Author"].ToString().Replace("\"", "&quot;").Trim());
			string text3 = Util.CollapseSpaces(dataTable.Rows[0]["Publisher"].ToString().Replace("\"", "&quot;").Trim());
			string text4 = Util.CollapseSpaces(dataTable.Rows[0]["Subject"].ToString().Replace("\"", "&quot;").Trim());
			string text5 = Util.CollapseSpaces(dataTable.Rows[0]["DeweyNo"].ToString().Replace("\"", "&quot;").Trim());
			string text6 = Util.CollapseSpaces(dataTable.Rows[0]["PublishLocation"].ToString().Replace("\"", "&quot;").Trim());
			string text7 = Util.CollapseSpaces(dataTable.Rows[0]["ISBN"].ToString().Replace("\"", "&quot;").Trim());
			string text8 = Util.CollapseSpaces(dataTable.Rows[0]["Description"].ToString().Replace("\"", "&quot;").Trim());
			string text9 = "";
			string text10 = "";
			if (!IsUpdate)
			{
				text9 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				text10 = text9;
			}
			else if (!string.IsNullOrEmpty(dataTable.Rows[0]["CreateDate"].ToString()))
			{
				text9 = ((DateTime)dataTable.Rows[0]["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss");
				text10 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>");
			stringBuilder.AppendLine("<CogitoFocusStandardInputFormat Version=\"1.0\">");
			stringBuilder.AppendLine(" <Document TYPE=\"SECTIONS\">");
			stringBuilder.AppendLine("     <SECTION NAME=\"TITLE\"><![CDATA[" + text + ", " + text2 + "]]></SECTION>");
			stringBuilder.AppendLine("     <SECTION NAME=\"BODY\"><![CDATA[" + text4 + ", " + text2 + "]]></SECTION>");
			stringBuilder.AppendLine(" </Document>");
			stringBuilder.AppendLine(" <Metabase>");
			stringBuilder.AppendLine("     <Item name=\"ORIGFILENAME\" value=\"http://dev.pnri.go.id/KatalogAdd.aspx?id=" + CatalogID + "\" />");
			stringBuilder.AppendLine("     <Item name=\"DATEEDIT\" value=\"" + text10 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"DOCTYPEID\" value=\"text / html\" />");
			stringBuilder.AppendLine("     <Item name=\"DATEACQ\" value=\"" + text9 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"SIZE\" value=\"[SIZE]\" />");
			stringBuilder.AppendLine(" </Metabase>");
			stringBuilder.AppendLine(" <Metacustom>");
			stringBuilder.AppendLine("     <Item name=\"TITLE\" value=\"" + text + "\" />");
			stringBuilder.AppendLine("     <Item name=\"AUTHOR\" value=\"" + text2 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"PUBLISHER\" value=\"" + text3 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"SUBJECT\" value=\"" + text4 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"NOPANGGIL\" value=\"" + text5 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"LOCATION\" value=\"" + text6 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"ISBN\" value=\"" + text7 + "\" />");
			stringBuilder.AppendLine("     <Item name=\"DESKRIPSIFISIK\" value=\"" + text8 + "\" />");
			stringBuilder.AppendLine(" </Metacustom>");
			stringBuilder.AppendLine("</CogitoFocusStandardInputFormat>");
			stringBuilder = stringBuilder.Replace("[SIZE]", stringBuilder.Length.ToString());
			return stringBuilder.ToString();
		}
		return "";
	}

	public static CatalogDatas GetCatalogs(int LibraryID, string CriteriaName, string Keyword, int PageNumber, int PageSize)
	{
		return default(CatalogDatas);
	}

	public static void SetLeaderValue(RecordFactory.ListLeader ListLeader, bool IsNew, int WorksheetID)
	{
		if (IsNew)
		{
			ListLeader.recordstatus = Leader.RecordStatus.n;
		}
		else
		{
			ListLeader.recordstatus = Leader.RecordStatus.c;
		}
		switch (WorksheetID)
		{
		case 1:
			ListLeader.typeofrecord = Leader.TypeOfRecord.a;
			break;
		case 3:
			ListLeader.typeofrecord = Leader.TypeOfRecord.g;
			break;
		case 5:
			ListLeader.typeofrecord = Leader.TypeOfRecord.e;
			break;
		case 7:
			ListLeader.typeofrecord = Leader.TypeOfRecord.g;
			break;
		case 8:
			ListLeader.typeofrecord = Leader.TypeOfRecord.j;
			break;
		case 9:
			ListLeader.typeofrecord = Leader.TypeOfRecord.p;
			break;
		case 10:
			ListLeader.typeofrecord = Leader.TypeOfRecord.j;
			break;
		case 11:
			ListLeader.typeofrecord = Leader.TypeOfRecord.g;
			break;
		case 12:
			ListLeader.typeofrecord = Leader.TypeOfRecord.t;
			break;
		case 13:
			ListLeader.typeofrecord = Leader.TypeOfRecord.a;
			break;
		case 41:
			ListLeader.typeofrecord = Leader.TypeOfRecord.a;
			break;
		case 21:
			ListLeader.typeofrecord = Leader.TypeOfRecord.k;
			break;
		case 20:
			ListLeader.typeofrecord = Leader.TypeOfRecord.m;
			break;
		case 102:
			ListLeader.typeofrecord = Leader.TypeOfRecord.a;
			break;
		default:
			ListLeader.typeofrecord = Leader.TypeOfRecord.a;
			break;
		}
		switch (WorksheetID)
		{
		case 13:
			ListLeader.bibliographiclevel = Leader.BibliographicLevel.s;
			break;
		case 102:
			ListLeader.bibliographiclevel = Leader.BibliographicLevel.a;
			break;
		default:
			ListLeader.bibliographiclevel = Leader.BibliographicLevel.m;
			break;
		}
	}

	public static Hashtable LoadBentukKaryaTulis()
	{
		Hashtable hashtable = new Hashtable();
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Name as Name FROM refferenceitems WHERE Refference_id = 17 ORDER BY Code");
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string key = dataTable.Rows[i]["Code"].ToString();
			string value = dataTable.Rows[i]["Name"].ToString();
			if (hashtable[key] == null)
			{
				hashtable.Add(key, value);
			}
		}
		return hashtable;
	}

	public static Hashtable LoadKelompokSasaran()
	{
		Hashtable hashtable = new Hashtable();
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Name as Name FROM refferenceitems WHERE Refference_id = 2 ORDER BY Code");
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string key = dataTable.Rows[i]["Code"].ToString();
			string value = dataTable.Rows[i]["Name"].ToString();
			if (hashtable[key] == null)
			{
				hashtable.Add(key, value);
			}
		}
		return hashtable;
	}
}
