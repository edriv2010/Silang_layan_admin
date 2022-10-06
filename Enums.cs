using System;
using System.ComponentModel;
using System.Reflection;

public class Enums
{
	public class UserAuth
	{
		public static int ByPassAccess = 0;

		public static int AdminOnly = 1;

		public static int AllUser = -1;

		public static int BypassPageAuth = -2;
	}

	public static string stringValueOf(Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
		if (array.Length > 0)
		{
			return array[0].Description;
		}
		return value.ToString();
	}

	public static object enumValueOf(string value, Type enumType)
	{
		string[] names = Enum.GetNames(enumType);
		string[] array = names;
		foreach (string value2 in array)
		{
			if (stringValueOf((Enum)Enum.Parse(enumType, value2)).Equals(value))
			{
				return Enum.Parse(enumType, value2);
			}
		}
		throw new ArgumentException("The string is not a description or value of the specified enum.");
	}
}
