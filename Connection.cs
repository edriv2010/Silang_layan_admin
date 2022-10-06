using System;
using System.Configuration;

public class Connection
{
	public enum EServerType
	{
		Oracle,
		MySQL,
		PostgreSQL,
		MsSQL,
		MsAccess2003,
		MsAccess2007
	}

	public static string ConnectionString = null;

	public static string ConnectionStringOracle = null;

	public static string ConnectionStringMySQL = null;

	public static string ConnectionStringPostgreSQL = null;

	public static string ConnectionStringMsSQL = null;

	public static string ConnectionStringMsAccess2003 = null;

	public static string ConnectionStringMsAccess2007 = null;

	public static string ConnectionStringElse = null;

	public static string ParameterSymbolOracle = ":";

	public static string ParameterSymbolMySQL = "?";

	public static string ParameterSymbolPostgreSQL = "?";

	public static string ParameterSymbolMsSQL = "@";

	public static string ParameterSymbolMsAccess2003 = "@";

	public static string ParameterSymbolMsAccess2007 = "@";

	public static string ParameterSymbolElse = "@";

	public static EServerType ServerType;

	public static string ParameterSymbol;

	public static void SetConnectionString(EServerType ServerType, string ConnString)
	{
		Connection.ServerType = ServerType;
		switch (ServerType)
		{
		case EServerType.Oracle:
			ParameterSymbol = ParameterSymbolOracle;
			ConnectionStringOracle = ConnString;
			break;
		case EServerType.MySQL:
			ParameterSymbol = ParameterSymbolMySQL;
			ConnectionStringMySQL = ConnString;
			break;
		case EServerType.PostgreSQL:
			ParameterSymbol = ParameterSymbolPostgreSQL;
			ConnectionStringPostgreSQL = ConnString;
			break;
		case EServerType.MsSQL:
			ParameterSymbol = ParameterSymbolMsSQL;
			ConnectionStringMsSQL = ConnString;
			break;
		case EServerType.MsAccess2003:
			ParameterSymbol = ParameterSymbolMsAccess2003;
			ConnectionStringMsAccess2003 = ConnString;
			break;
		case EServerType.MsAccess2007:
			ParameterSymbol = ParameterSymbolMsAccess2007;
			ConnectionStringMsAccess2007 = ConnString;
			break;
		default:
			ParameterSymbol = ParameterSymbolElse;
			ConnectionStringElse = ConnString;
			break;
		}
	}

	public static void SetConnection()
	{
		if (string.IsNullOrEmpty(ConnectionString))
		{
			string text = ConfigurationManager.AppSettings["Driver"];
			string text2 = "";
			switch (text.ToUpper())
			{
			case "ORACLE":
				ServerType = EServerType.Oracle;
				text2 = "Oracle";
				break;
			case "MYSQL":
				ServerType = EServerType.MySQL;
				text2 = "MySQL";
				break;
			case "POSTGRESQL":
				ServerType = EServerType.PostgreSQL;
				text2 = "PostgreSQL";
				break;
			case "MSSQL":
				ServerType = EServerType.MsSQL;
				text2 = "MsSQL";
				break;
			case "MSACCESS2003":
				ServerType = EServerType.MsAccess2003;
				text2 = "MsAccess2003";
				break;
			case "MSACCESS2007":
				ServerType = EServerType.MsAccess2007;
				text2 = "MsAccess2007";
				break;
			}
			CCryptography.Rijndael.Key = Convert.FromBase64String(MyApplication.SavedKeyApplicationSetting);
			CCryptography.Rijndael.IV = Convert.FromBase64String(MyApplication.SavedIVApplicationSetting);
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString" + text2].ConnectionString;
			connectionString = CCryptography.Rijndael.Decrypt(connectionString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
			SetConnectionString(ServerType, connectionString);
		}
		else
		{
			ServerType = EServerType.Oracle;
			ParameterSymbol = ParameterSymbolOracle;
		}
	}

	public static void SetConnectionOracle()
	{
		if (string.IsNullOrEmpty(ConnectionStringOracle))
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStringOracle"].ConnectionString;
			connectionString = CCryptography.Rijndael.Decrypt(connectionString, MyApplication.SavedKeyApplicationSetting, MyApplication.SavedIVApplicationSetting);
			SetConnectionString(EServerType.Oracle, connectionString);
		}
		else
		{
			ServerType = EServerType.Oracle;
			ParameterSymbol = ParameterSymbolOracle;
		}
	}

	public static void SetConnectionMySQL()
	{
		if (string.IsNullOrEmpty(ConnectionStringMySQL))
		{
			SetConnectionString(EServerType.MySQL, ConfigurationManager.ConnectionStrings["ConnectionStringMySQL"].ConnectionString);
			return;
		}
		ServerType = EServerType.MySQL;
		ParameterSymbol = ParameterSymbolMySQL;
	}

	public static void SetConnectionMySQL2()
	{
		if (string.IsNullOrEmpty(ConnectionStringMySQL))
		{
			SetConnectionString(EServerType.MySQL, ConfigurationManager.ConnectionStrings["ConnectionStringMySQL"].ConnectionString);
		}
	}

	public static void SetConnectionPostgreSQL()
	{
		if (string.IsNullOrEmpty(ConnectionStringPostgreSQL))
		{
			SetConnectionString(EServerType.PostgreSQL, ConfigurationManager.ConnectionStrings["ConnectionStringPostgreSQL"].ConnectionString);
			return;
		}
		ServerType = EServerType.PostgreSQL;
		ParameterSymbol = ParameterSymbolPostgreSQL;
	}

	public static void SetConnectionMsSQL()
	{
		if (string.IsNullOrEmpty(ConnectionStringMsSQL))
		{
			SetConnectionString(EServerType.MsSQL, ConfigurationManager.ConnectionStrings["ConnectionStringMsSQL"].ConnectionString);
			return;
		}
		ServerType = EServerType.MsSQL;
		ParameterSymbol = ParameterSymbolMsSQL;
	}

	public static void SetConnectionMsAccess2003()
	{
		if (string.IsNullOrEmpty(ConnectionStringMsAccess2003))
		{
			SetConnectionString(EServerType.MsAccess2003, ConfigurationManager.ConnectionStrings["ConnectionStringMsAccess2003"].ConnectionString);
			return;
		}
		ServerType = EServerType.MsAccess2003;
		ParameterSymbol = ParameterSymbolMsAccess2003;
	}

	public static void SetConnectionMsAccess2007()
	{
		if (string.IsNullOrEmpty(ConnectionStringMsAccess2007))
		{
			SetConnectionString(EServerType.MsAccess2007, ConfigurationManager.ConnectionStrings["ConnectionStringMsAccess2007"].ConnectionString);
			return;
		}
		ServerType = EServerType.MsAccess2007;
		ParameterSymbol = ParameterSymbolMsAccess2007;
	}
}
