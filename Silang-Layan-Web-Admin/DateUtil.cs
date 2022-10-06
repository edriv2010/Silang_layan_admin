public class DateUtil
{
	public static string RotateDateTime(string DateTimeValue)
	{
		string[] array = DateTimeValue.Split(char.Parse("/"));
		if (array.Length != 3)
		{
			return "";
		}
		return array[1] + "/" + array[0] + "/" + array[2];
	}
}
