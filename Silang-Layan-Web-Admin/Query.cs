using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Query
{
	public class TempPeminjaman
	{
		public string NomorItem { get; set; }

		public string Title { get; set; }

		public DateTime TglPinjam { get; set; }

		public DateTime TglKembali { get; set; }
	}

	public static string TestConnection
	{
        get
        {
            switch (Connection.ServerType)
            {
                case Connection.EServerType.Oracle:
                    return "SELECT 1 FROM DUAL";
                case Connection.EServerType.MySQL:
                    return "SELECT 1";
                default:
                    return "";
            }
        }

	}

	public static string UserPasswordEncode 
	{
       get
        {
            switch (Connection.ServerType)
            {
                case Connection.EServerType.Oracle:
                    return "SELECT to_char(rawtohex(dbms_crypto.hash(utl_raw.cast_to_raw(" + Connection.ParameterSymbol + "userpassword" + "),3))) FROM dual";
                case Connection.EServerType.MySQL:
                    return "SELECT CONVERT(SHA1(" + Connection.ParameterSymbol + "userpassword) USING UTF8)";
                default:
                    return "";
            }
        }
	}
}
