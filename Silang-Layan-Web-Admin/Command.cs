using System;
using System.Data;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

public class Command
{
	public enum InsertOrUpdate
	{
		Insert,
		Update
	}

	public class ExecInsertOrUpdateWithTrans
	{
		private OracleConnection connOracle;

		private OracleConnection connOracleINLIS3;

		private OracleConnection connOracleLama;

		private OracleCommand cmdOracle;

		private OracleCommand cmdOracleINLIS3;

		private OracleCommand cmdOracleLama;

		private OracleTransaction transOracle = null;

		private OracleTransaction transOracleINLIS3 = null;

		private OracleTransaction transOracleLama = null;

		private MySqlConnection connMySQL;

		private MySqlCommand cmdMySQL;

		private MySqlTransaction transMySQL = null;

		public void BeginTransaction()
		{
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					connOracle = new OracleConnection(Connection.ConnectionStringOracle);
					cmdOracle = new OracleCommand();
					connOracle.Open();
					cmdOracle = connOracle.CreateCommand();
					transOracle = connOracle.BeginTransaction();
					cmdOracle.Connection = connOracle;
					cmdOracle.Transaction = transOracle;
					break;
				}
				catch (OracleException ex3)
				{
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex2)
				{
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
			case Connection.EServerType.MySQL:
				try
				{
					connMySQL = new MySqlConnection(Connection.ConnectionStringMySQL);
					cmdMySQL = new MySqlCommand();
					connMySQL.Open();
					cmdMySQL = connMySQL.CreateCommand();
					transMySQL = connMySQL.BeginTransaction();
					cmdMySQL.Connection = connMySQL;
					cmdMySQL.Transaction = transMySQL;
					break;
				}
				catch (MySqlException ex)
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
					Util.RaiseMessage(ex.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex2)
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				finally
				{
				}
			}
		}

		public void CommitTransaction()
		{
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Commit();
					}
					break;
				}
				catch (OracleException ex3)
				{
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex2)
				{
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
			case Connection.EServerType.MySQL:
				try
				{
					if (transMySQL != null && transMySQL.Connection != null)
					{
						transMySQL.Commit();
					}
					break;
				}
				catch (MySqlException ex)
				{
					Util.RaiseMessage(ex.Message + Environment.NewLine);
					break;
				}
				catch (Exception ex2)
				{
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
					break;
				}
				finally
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
				}
			}
		}

		public static object GenerateSQLInsertCommand(object CmdObj, string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
		{
			object result = null;
			string text = null;
			string text2 = null;
			string text3 = null;
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
			{
				OracleCommand oracleCommand = (OracleCommand)CmdObj;
				oracleCommand.Parameters.Clear();
				switch (InsertOrUpdate)
				{
				case InsertOrUpdate.Insert:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						text = text + FieldValues.Item1(i) + ",";
						text2 = text2 + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						text = text.Remove(text.Length - 1, 1);
						text2 = text2.Remove(text2.Length - 1, 1);
					}
					oracleCommand.CommandText = "INSERT INTO " + TableName + "(" + text + ") VALUES (" + text2 + ")";
					break;
				}
				case InsertOrUpdate.Update:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						string text4 = text3;
						text3 = text4 + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						text3 = text3.Remove(text3.Length - 1, 1);
					}
					oracleCommand.CommandText = "UPDATE " + TableName + " SET " + text3 + SQLWhere;
					break;
				}
				}
				result = oracleCommand;
				break;
			}
			case Connection.EServerType.MySQL:
			{
				MySqlCommand mySqlCommand = (MySqlCommand)CmdObj;
				mySqlCommand.Parameters.Clear();
				switch (InsertOrUpdate)
				{
				case InsertOrUpdate.Insert:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						text = text + FieldValues.Item1(i) + ",";
						text2 = text2 + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						text = text.Remove(text.Length - 1, 1);
						text2 = text2.Remove(text2.Length - 1, 1);
					}
					mySqlCommand.CommandText = "INSERT INTO " + TableName + "(" + text + ") VALUES (" + text2 + ")";
					break;
				}
				case InsertOrUpdate.Update:
				{
					int i;
					for (i = 0; i <= FieldValues.Count() - 1; i++)
					{
						string text4 = text3;
						text3 = text4 + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
						mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
					}
					if (i > 0)
					{
						text3 = text3.Remove(text3.Length - 1, 1);
					}
					mySqlCommand.CommandText = "UPDATE " + TableName + " SET " + text3 + SQLWhere;
					break;
				}
				}
				result = mySqlCommand;
				break;
			}
			default:
				Util.RaiseMessage("Server Type doesn't recognized!");
				break;
			}
			return result;
		}

		public bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
		{
			bool result = false;
			switch (Connection.ServerType)
			{
			case Connection.EServerType.Oracle:
				try
				{
					cmdOracle = (OracleCommand)GenerateSQLInsertCommand(cmdOracle, TableName, FieldValues, InsertOrUpdate, SQLWhere);
					cmdOracle.ExecuteNonQuery();
					result = true;
				}
				catch (OracleException ex3)
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Rollback();
					}
					Util.RaiseMessage(ex3.Message + Environment.NewLine);
				}
				catch (Exception ex2)
				{
					if (transOracle != null && transOracle.Connection != null)
					{
						transOracle.Rollback();
					}
					Util.RaiseMessage(ex2.Message + Environment.NewLine);
				}
				finally
				{
					if (cmdOracle != null)
					{
						cmdOracle.Dispose();
					}
					if (connOracle != null)
					{
						connOracle.Close();
					}
					if (connOracle != null)
					{
						connOracle.Dispose();
					}
				}
				break;
			case Connection.EServerType.MySQL:
				try
				{
					cmdMySQL = (MySqlCommand)GenerateSQLInsertCommand(cmdMySQL, TableName, FieldValues, InsertOrUpdate, SQLWhere);
					cmdMySQL.ExecuteNonQuery();
					result = true;
				}
				catch (MySqlException ex)
				{
					try
					{
						if (transMySQL != null && transMySQL.Connection != null)
						{
							transMySQL.Rollback();
						}
					}
					finally
					{
						Util.RaiseMessage(ex.Message + Environment.NewLine);
					}
				}
				catch (Exception ex2)
				{
					try
					{
						if (transMySQL != null && transMySQL.Connection != null)
						{
							transMySQL.Rollback();
						}
					}
					finally
					{
						Util.RaiseMessage(ex2.Message + Environment.NewLine);
					}
				}
				finally
				{
					if (cmdMySQL != null)
					{
						cmdMySQL.Dispose();
					}
					if (connMySQL != null)
					{
						connMySQL.Close();
					}
					if (connMySQL != null)
					{
						connMySQL.Dispose();
					}
				}
				break;
			}
			return result;
		}
	}

	public static bool TestConnection()
	{
		bool result = true;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection oracleConnection = null;
			OracleCommand oracleCommand = null;
			try
			{
				oracleConnection = new OracleConnection(Connection.ConnectionStringOracle);
				oracleCommand = new OracleCommand();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.CommandText = Query.TestConnection;
				oracleConnection.Open();
				oracleCommand.ExecuteScalar();
			}
			catch (OracleException ex3)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex3.Message);
				result = false;
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex2.Message);
				result = false;
			}
			finally
			{
				if (oracleConnection != null) oracleConnection.Close();
                    if (oracleConnection != null) oracleConnection.Dispose();
                    if (oracleCommand!= null) oracleCommand.Dispose();

                //oracleConnection?.Close();
				//oracleConnection?.Dispose();
				//oracleCommand?.Dispose();
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection mySqlConnection = null;
			MySqlCommand mySqlCommand = null;
			try
			{
				mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
				mySqlCommand = new MySqlCommand();
				mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.CommandText = Query.TestConnection;
				mySqlConnection.Open();
				mySqlCommand.ExecuteScalar();
			}
			catch (MySqlException ex)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex.Message);
				result = false;
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage("Can not connect to Database Server!" + Environment.NewLine + ex2.Message);
				result = false;
			}
			finally
			{
                if (mySqlConnection != null) mySqlConnection.Close();
                    if (mySqlConnection != null) mySqlConnection.Dispose();
                    if (mySqlCommand!= null) mySqlCommand.Dispose();
				
                
			}
			break;
		}
		}
		return result;
	}

	public static string ExecScalar(string SQL)
	{
		return ExecScalar(null, SQL, "");
	}

	public static string ExecScalar(string SQL, string NullValue)
	{
		return ExecScalar(null, SQL, NullValue);
	}

	public static string ExecScalar(TwoArrayList Parameter, string SQL)
	{
		return ExecScalar(Parameter, SQL, "");
	}

	public static string ExecScalar(TwoArrayList Parameter, string SQL, string NullValue)
	{
		string result = null;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection oracleConnection = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand oracleCommand = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				oracleCommand.CommandText = SQL;
				oracleCommand.Connection = oracleConnection;
				oracleConnection.Open();
				object obj = oracleCommand.ExecuteScalar();
				result = ((obj != null && !(obj.ToString() == "")) ? obj.ToString() : NullValue);
				obj = null;
			}
			catch (OracleException ex3)
			{
				result = NullValue;
				Util.RaiseMessage(ex3.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				result = NullValue;
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (oracleCommand != null) oracleCommand.Dispose();
                    if (oracleConnection != null) oracleConnection.Close();
                    if (oracleConnection != null) oracleConnection.Dispose();

				//oracleCommand?.Dispose();
				//oracleConnection?.Close();
				//oracleConnection?.Dispose();
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand mySqlCommand = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				mySqlCommand.CommandText = SQL;
				mySqlCommand.Connection = mySqlConnection;
				mySqlConnection.Open();
				object obj = mySqlCommand.ExecuteScalar();
				result = ((obj != null && !(obj.ToString() == "")) ? obj.ToString() : NullValue);
				obj = null;
			}
			catch (MySqlException ex)
			{
				result = NullValue;
				Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				result = NullValue;
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                if (mySqlCommand != null) mySqlCommand.Dispose();
                    if (mySqlConnection != null) mySqlConnection.Close();
                    if (mySqlConnection != null) mySqlConnection.Dispose();

				//mySqlCommand?.Dispose();
				//mySqlConnection?.Close();
				//mySqlConnection?.Dispose();
			}
			break;
		}
		}
		return result;
	}

	public static DataTable ExecDataAdapter(string SQL)
	{
		return ExecDataAdapter(SQL, null);
	}

	public static DataTable ExecDataAdapter(string SQL, TwoArrayList Parameter)
	{
		DataTable dataTable = new DataTable();
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection oracleConnection = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand oracleCommand = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				oracleCommand.CommandText = SQL;
				oracleCommand.Connection = oracleConnection;
				oracleConnection.Open();
				OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
				oracleDataAdapter.Fill(dataTable);
				oracleDataAdapter.Dispose();
			}
			catch (OracleException ex3)
			{
				Util.RaiseMessage(ex3.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
				//oracleCommand?.Dispose();
				//oracleConnection?.Close();
				//oracleConnection?.Dispose();

                    if (oracleCommand != null) oracleCommand.Dispose();
                    if (oracleConnection != null) oracleConnection.Close();
                    if (oracleConnection != null) oracleConnection.Dispose();

			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand mySqlCommand = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				mySqlCommand.CommandText = SQL;
				mySqlCommand.Connection = mySqlConnection;
				mySqlConnection.Open();
				MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
				mySqlDataAdapter.Fill(dataTable);
				mySqlDataAdapter.Dispose();
			}
			catch (MySqlException ex)
			{
				Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                    if (mySqlCommand != null) mySqlCommand.Dispose();
                    if (mySqlConnection != null) mySqlConnection.Close();
                    if (mySqlConnection != null) mySqlConnection.Dispose();

				//mySqlCommand?.Dispose();
				//mySqlConnection?.Close();
				//mySqlConnection?.Dispose();
			}
			break;
		}
		}
		return dataTable;
	}

	public static bool ExecNonQuery(string SQL)
	{
		return ExecNonQuery(SQL, null);
	}

	public static bool ExecNonQuery(string SQL, TwoArrayList Parameter)
	{
		bool result = false;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection oracleConnection = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand oracleCommand = new OracleCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
					oracleCommand.BindByName = true;
				}
				oracleCommand.CommandText = SQL;
				oracleCommand.Connection = oracleConnection;
				oracleConnection.Open();
				oracleCommand.ExecuteNonQuery();
				result = true;
			}
			catch (OracleException ex3)
			{
				Util.RaiseMessage(ex3.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? oracleCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                    if (oracleCommand != null) oracleCommand.Dispose();
                    if (oracleConnection != null) oracleConnection.Close();
                    if (oracleConnection != null) oracleConnection.Dispose();

				//oracleCommand?.Dispose();
				//oracleConnection?.Close();
				//oracleConnection?.Dispose();
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand mySqlCommand = new MySqlCommand();
			try
			{
				if (Parameter != null)
				{
					for (int i = 0; i <= Parameter.Count() - 1; i++)
					{
						mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
					}
				}
				mySqlCommand.CommandText = SQL;
				mySqlCommand.Connection = mySqlConnection;
				mySqlConnection.Open();
				mySqlCommand.ExecuteNonQuery();
				result = true;
			}
			catch (MySqlException ex)
			{
				Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
			}
			finally
			{
                    if (mySqlCommand != null) mySqlCommand.Dispose();
                    if (mySqlConnection != null) mySqlConnection.Close();
                    if (mySqlConnection != null) mySqlConnection.Dispose();

				
			}
			break;
		}
		}
		return result;
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		return ExecInsertOrUpdate(TableName, FieldValues, InsertOrUpdate, SQLWhere, null);
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere, TwoArrayList ParameterWhere)
	{
		bool result = false;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleConnection oracleConnection = new OracleConnection(Connection.ConnectionStringOracle);
			OracleCommand oracleCommand = new OracleCommand();
			try
			{
				oracleCommand = (OracleCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				if (ParameterWhere != null)
				{
					for (int i = 0; i <= ParameterWhere.Count() - 1; i++)
					{
						oracleCommand.Parameters.Add(Connection.ParameterSymbol + ParameterWhere.Item1(i), ParameterWhere.Item2(i));
					}
				}
				oracleConnection.Open();
				oracleCommand.Connection = oracleConnection;
				oracleCommand.ExecuteNonQuery();
				result = true;
			}
			catch (OracleException ex3)
			{
				Util.RaiseMessage(ex3.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
				    
                if (oracleCommand!=null)oracleCommand.Dispose();
				if(oracleConnection!=null)oracleConnection.Close();
				if(oracleConnection!=null)oracleConnection.Dispose();
			}
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
			MySqlCommand mySqlCommand = new MySqlCommand();
			try
			{
				mySqlCommand = (MySqlCommand)GenerateSQLInsertCommand(TableName, FieldValues, InsertOrUpdate, SQLWhere);
				mySqlConnection.Open();
				mySqlCommand.Connection = mySqlConnection;
				mySqlCommand.ExecuteNonQuery();
				result = true;
			}
			catch (MySqlException ex)
			{
				Util.RaiseMessage(ex.Message + Environment.NewLine + ". For table : " + TableName);
			}
			catch (Exception ex2)
			{
				Util.RaiseMessage(ex2.Message + Environment.NewLine + ". For table : " + TableName);
			}
			finally
			{
				if(mySqlCommand!=null)mySqlCommand.Dispose();
                if (mySqlConnection != null) mySqlConnection.Close();
                if (mySqlConnection != null) mySqlCommand.Dispose();
			}
			break;
		}
		}
		return result;
	}

	public static object GenerateSQLInsertCommand(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		object result = null;
		string text = null;
		string text2 = null;
		string text3 = null;
		switch (Connection.ServerType)
		{
		case Connection.EServerType.Oracle:
		{
			OracleCommand oracleCommand = new OracleCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				for (int i = 0; i <= FieldValues.Count() - 1; i++)
				{
					text = text + FieldValues.Item1(i) + ",";
					text2 = text2 + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					oracleCommand.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				text = text.TrimEnd(char.Parse(","));
				text2 = text2.TrimEnd(char.Parse(","));
				oracleCommand.CommandText = "INSERT INTO " + TableName + "(" + text + ") VALUES (" + text2 + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					string text4 = text3;
					text3 = text4 + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					oracleCommand.Parameters.Add(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					text3 = text3.Remove(text3.Length - 1, 1);
				}
				oracleCommand.CommandText = "UPDATE " + TableName + " SET " + text3 + SQLWhere;
				break;
			}
			}
			result = oracleCommand;
			break;
		}
		case Connection.EServerType.MySQL:
		{
			MySqlCommand mySqlCommand = new MySqlCommand();
			switch (InsertOrUpdate)
			{
			case InsertOrUpdate.Insert:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					text = text + FieldValues.Item1(i) + ",";
					text2 = text2 + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					text = text.Remove(text.Length - 1, 1);
					text2 = text2.Remove(text2.Length - 1, 1);
				}
				mySqlCommand.CommandText = "INSERT INTO " + TableName + "(" + text + ") VALUES (" + text2 + ")";
				break;
			}
			case InsertOrUpdate.Update:
			{
				int i;
				for (i = 0; i <= FieldValues.Count() - 1; i++)
				{
					string text4 = text3;
					text3 = text4 + FieldValues.Item1(i) + " = " + Connection.ParameterSymbol + FieldValues.Item1(i) + ",";
					mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + FieldValues.Item1(i), FieldValues.Item2(i));
				}
				if (i > 0)
				{
					text3 = text3.Remove(text3.Length - 1, 1);
				}
				mySqlCommand.CommandText = "UPDATE " + TableName + " SET " + text3 + SQLWhere;
				break;
			}
			}
			result = mySqlCommand;
			break;
		}
		default:
			Util.RaiseMessage("Server Type doesn't recognized!");
			break;
		}
		return result;
	}

	public static bool ExecInsertOrUpdate(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate)
	{
		return ExecInsertOrUpdate(TableName, FieldValues, InsertOrUpdate, "");
	}

	public static string ExecScalarMySQL(string SQL, string NullValue)
	{
		return ExecScalarMySQL(null, SQL, NullValue);
	}

	public static string ExecScalarMySQL(TwoArrayList Parameter, string SQL, string NullValue)
	{
		string result = null;
		MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand mySqlCommand = new MySqlCommand();
		try
		{
			if (Parameter != null)
			{
				for (int i = 0; i <= Parameter.Count() - 1; i++)
				{
					mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + Parameter.Item1(i), Parameter.Item2(i));
				}
			}
			mySqlCommand.CommandText = SQL;
			mySqlCommand.Connection = mySqlConnection;
			mySqlConnection.Open();
			object obj = mySqlCommand.ExecuteScalar();
			result = ((obj != null && !(obj.ToString() == "")) ? obj.ToString() : NullValue);
			obj = null;
		}
		catch (MySqlException ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		finally
		{
			if(mySqlCommand!=null)mySqlCommand.Dispose();
			if(mySqlConnection != null)mySqlConnection.Close();
            if (mySqlConnection != null) mySqlConnection.Dispose();
		}
		return result;
	}

	public static DataTable ExecDataAdapterMySQL(string SQL, TwoArrayList Parameter = null)
	{
		DataTable dataTable = new DataTable();
		MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand mySqlCommand = new MySqlCommand();
		try
		{
			if (Parameter != null)
			{
				for (int i = 0; i <= Parameter.Count() - 1; i++)
				{
					mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbol + Parameter.Item1(i), Parameter.Item2(i));
				}
			}
			mySqlCommand.CommandText = SQL;
			mySqlCommand.Connection = mySqlConnection;
			mySqlConnection.Open();
			MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
			mySqlDataAdapter.Fill(dataTable);
			mySqlDataAdapter.Dispose();
		}
		catch (MySqlException ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		finally
		{
			if(mySqlCommand!=null)mySqlCommand.Dispose();
            if (mySqlConnection != null) mySqlConnection.Close();
			if(mySqlConnection!=null)mySqlCommand.Dispose();
		}
		return dataTable;
	}

	public static bool ExecNonQueryMySQL(string SQL)
	{
		bool result = false;
		MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand mySqlCommand = new MySqlCommand();
		try
		{
			mySqlCommand.CommandText = SQL;
			mySqlCommand.Connection = mySqlConnection;
			mySqlConnection.Open();
			mySqlCommand.ExecuteNonQuery();
			result = true;
		}
		catch (MySqlException ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		catch (Exception ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine + (MyApplication.IsDebug ? mySqlCommand.CommandText : "") + Environment.NewLine);
		}
		finally
		{
			if(mySqlCommand!=null)mySqlCommand.Dispose();
			if(mySqlConnection!=null)mySqlConnection.Close();
            if (mySqlConnection != null) mySqlConnection.Dispose();
		}
		return result;
	}

	public static bool ExecInsertOrUpdateMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate)
	{
		return ExecInsertOrUpdateMySQL(TableName, FieldValues, InsertOrUpdate, "");
	}

	public static object GenerateSQLInsertCommandMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		object obj = null;
		string text = null;
		string text2 = null;
		string text3 = null;
		MySqlCommand mySqlCommand = new MySqlCommand();
		switch (InsertOrUpdate)
		{
		case InsertOrUpdate.Insert:
		{
			int i;
			for (i = 0; i <= FieldValues.Count() - 1; i++)
			{
				text = text + FieldValues.Item1(i) + ",";
				text2 = text2 + Connection.ParameterSymbolMySQL + FieldValues.Item1(i) + ",";
				mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + FieldValues.Item1(i), FieldValues.Item2(i));
			}
			if (i > 0)
			{
				text = text.Remove(text.Length - 1, 1);
				text2 = text2.Remove(text2.Length - 1, 1);
			}
			mySqlCommand.CommandText = "INSERT INTO " + TableName + "(" + text + ") VALUES (" + text2 + ")";
			break;
		}
		case InsertOrUpdate.Update:
		{
			int i;
			for (i = 0; i <= FieldValues.Count() - 1; i++)
			{
				string text4 = text3;
				text3 = text4 + FieldValues.Item1(i) + " = " + Connection.ParameterSymbolMySQL + FieldValues.Item1(i) + ",";
				mySqlCommand.Parameters.AddWithValue(Connection.ParameterSymbolMySQL + FieldValues.Item1(i), FieldValues.Item2(i));
			}
			if (i > 0)
			{
				text3 = text3.Remove(text3.Length - 1, 1);
			}
			mySqlCommand.CommandText = "UPDATE " + TableName + " SET " + text3 + SQLWhere;
			break;
		}
		}
		return mySqlCommand;
	}

	public static bool ExecInsertOrUpdateMySQL(string TableName, TwoArrayList FieldValues, InsertOrUpdate InsertOrUpdate, string SQLWhere)
	{
		bool result = false;
		MySqlConnection mySqlConnection = new MySqlConnection(Connection.ConnectionStringMySQL);
		MySqlCommand mySqlCommand = new MySqlCommand();
		try
		{
			mySqlCommand = (MySqlCommand)GenerateSQLInsertCommandMySQL(TableName, FieldValues, InsertOrUpdate, SQLWhere);
			mySqlConnection.Open();
			mySqlCommand.Connection = mySqlConnection;
			mySqlCommand.ExecuteNonQuery();
			result = true;
		}
		catch (MySqlException ex)
		{
			Util.RaiseMessage(ex.Message + Environment.NewLine);
		}
		catch (Exception ex2)
		{
			Util.RaiseMessage(ex2.Message + Environment.NewLine);
		}
		finally
		{
			if(mySqlCommand!=null)mySqlCommand.Dispose();
			if(mySqlConnection!=null)mySqlConnection.Close();
            if (mySqlConnection != null) mySqlConnection.Dispose();
		}
		return result;
	}
}
