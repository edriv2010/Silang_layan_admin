using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public class Field
{
	public static string GetCleanSubFieldContent(string InputString, int FieldID, string SubFieldCode)
	{
		if (InputString.Trim() == "$a")
		{
			return "";
		}
		if (InputString.Trim().Length < 3 && InputString.Contains("$"))
		{
			return "";
		}
		if (!InputString.Trim().Contains("$"))
		{
			InputString = "$a " + InputString.Trim();
		}
		string text = "";
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code FROM fielddatas WHERE Field_id=" + FieldID + " ORDER BY SORTNO");
		text = Regex.Match(InputString, "(\\$" + SubFieldCode + ")(.*)").Value;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text2 = dataTable.Rows[i][0].ToString();
			string value = Regex.Match(InputString, "(\\$" + SubFieldCode + ")(.*?)(\\$" + text2 + ")").Value;
			if (!string.IsNullOrEmpty(value))
			{
				return Util.CollapseSpaces(value.Replace("$" + SubFieldCode, "").Replace("$" + text2, "").Trim());
			}
		}
		return Util.CollapseSpaces(text.Replace("$" + SubFieldCode, "").Trim());
	}

	public static ArrayList GetListCleanSubFieldContent(string InputString, int FieldID, string SubFieldCode)
	{
		if (InputString.Trim() == "$a")
		{
			return new ArrayList();
		}
		if (InputString.Trim().Length < 3 && InputString.Contains("$"))
		{
			return new ArrayList();
		}
		if (!InputString.Trim().Contains("$"))
		{
			InputString = "$a " + InputString.Trim();
		}
		string text = "";
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code FROM fielddatas WHERE Field_id=" + FieldID + " ORDER BY SORTNO");
		text = Regex.Match(InputString, "(\\$" + SubFieldCode + ")(.*)").Value;
		ArrayList arrayList = new ArrayList();
		string[] array = text.Split(new string[1] { "$" + SubFieldCode }, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			string text3 = Util.CollapseSpaces(text2.Replace("$" + SubFieldCode, "").Trim());
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				string text4 = dataTable.Rows[j][0].ToString();
				string value = Regex.Match(text3, "(\\$" + SubFieldCode + ")(.*?)(\\$" + text4 + ")").Value;
				if (!string.IsNullOrEmpty(value))
				{
					text3 = Util.CollapseSpaces(value.Replace("$" + SubFieldCode, "").Replace("$" + text4, "").Trim());
					break;
				}
			}
			arrayList.Add(text3);
		}
		return arrayList;
	}

	public static string GetSubtituteSubFieldContent(string InputString, int FieldID)
	{
		InputString = InputString.Trim();
		InputString = Util.CollapseSpaces(InputString);
		string text = "$";
		DataTable dataTable = Command.ExecDataAdapter("SELECT Code,Delimiter FROM fielddatas WHERE Field_id=" + FieldID);
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			string text2 = dataTable.Rows[i][0].ToString();
			string text3 = dataTable.Rows[i][1].ToString();
			while (InputString.Contains(text + text2 + " " + text + text2))
			{
				InputString = InputString.Replace(text + text2 + " " + text + text2, text + text2);
			}
			while (InputString.Contains(text + text2 + text + text2))
			{
				InputString = InputString.Replace(text + text2 + text + text2, text + text2);
			}
			if (string.IsNullOrEmpty(text3))
			{
				text3 = " ";
				InputString = Regex.Replace(InputString, "(\\" + text + ")[" + text2 + "]\\s?", text3).Trim();
				InputString = Util.CollapseSpaces(InputString);
			}
			else
			{
				InputString = Regex.Replace(InputString, "(\\" + text + ")[" + text2 + "]\\s?", " " + text3 + " ").Trim();
				InputString = Util.CollapseSpaces(InputString);
			}
		}
		if (dataTable.Rows.Count == 0)
		{
			string text3 = " ";
			InputString = Regex.Replace(InputString, "(\\" + text + ")[a]\\s?", text3).Trim();
			InputString = Util.CollapseSpaces(InputString);
		}
		return InputString;
	}

	public static string GetFixedDefaultValue(int WorksheetID, int FieldID)
	{
		string newValue = "StartPosition,Length,DefaultValue";
		string text = "SELECT SUM(Length) FROM worksheetfields INNER JOIN Worksheetfielditems ON Worksheetfielditems.worksheetfield_id = worksheetfields.id WHERE Field_id=" + FieldID + " AND Worksheet_id=" + WorksheetID + " ORDER BY StartPosition";
		int count = int.Parse(Command.ExecScalar(text, "0"));
		DataTable dataTable = Command.ExecDataAdapter(text.Replace("SUM(Length)", newValue));
		string text2 = new string(char.Parse("*"), count);
		try
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int startIndex = int.Parse(dataTable.Rows[i]["StartPosition"].ToString());
				int num = int.Parse(dataTable.Rows[i]["Length"].ToString());
				string text3 = dataTable.Rows[i]["DefaultValue"].ToString();
				text2 = text2.Remove(startIndex, num);
				text2 = text2.Insert(startIndex, text3.Substring(0, Math.Min(text3.Length, num)));
			}
		}
		catch
		{
		}
		return text2;
	}

	public static DataTable GetWorksheetFieldItems(int WorksheetID, int FieldID, out int WorksheetFieldID)
	{
		WorksheetFieldID = int.Parse(Command.ExecScalar("SELECT ID FROM worksheetfields WHERE Worksheet_id=" + WorksheetID + " AND Field_id=" + FieldID, "0"));
		if (WorksheetFieldID == 0)
		{
			return null;
		}
		return Command.ExecDataAdapter("SELECT * FROM worksheetfielditems WHERE Worksheetfield_id=" + WorksheetFieldID + " ORDER BY StartPosition");
	}

	public static string ProcessFixedField(int WorksheetFieldID, string[] Values, DataTable dt)
	{
		if (WorksheetFieldID == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (DataRow row in dt.Rows)
		{
			string text = "#";
			if (Values.Length > num)
			{
				text = Values[num];
			}
			int num2 = int.Parse(row["Length"].ToString());
			if (num2 > 0)
			{
				text = text.PadRight(num2, '#');
			}
			stringBuilder.Append(text);
			num++;
		}
		return stringBuilder.ToString();
	}

	public static string[] ProcessFixedField(int WorksheetFieldID, string Values, DataTable dt)
	{
		if (WorksheetFieldID == 0)
		{
			return null;
		}
		int length = Values.Length;
		int num = int.Parse(Command.ExecScalar("SELECT SUM(Length) FROM worksheetfielditems WHERE Worksheetfield_id=" + WorksheetFieldID, "0"));
		int num2 = num - length;
		string[] array = new string[dt.Rows.Count];
		int num3 = 0;
		foreach (DataRow row in dt.Rows)
		{
			try
			{
				array[num3] = Values.Substring(int.Parse(row["StartPosition"].ToString()), int.Parse(row["Length"].ToString()));
			}
			catch
			{
				array[num3] = "";
			}
			num3++;
		}
		return array;
	}
}
