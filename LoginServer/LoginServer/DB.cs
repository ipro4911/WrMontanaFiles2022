// Decompiled with JetBrains decompiler
// Type: LoginServer.DB
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace LoginServer
{
  public static class DB
  {
    private static string strConnection;
    private static MySqlConnection dbConnection;

    private static void dbConnection_StateChange(object usr, StateChangeEventArgs ev)
    {
      if (ev.CurrentState == ConnectionState.Broken)
      {
        Thread.Sleep(1000);
        DB.dbConnection.Close();
      }
      if (ev.CurrentState != ConnectionState.Closed)
        return;
      Thread.Sleep(1000);
      Log.WriteLine("Reconnecting to SQL Server 1...");
      DB.dbConnection = new MySqlConnection(DB.strConnection);
      DB.dbConnection.StateChange += new StateChangeEventHandler(DB.dbConnection_StateChange);
      Thread.Sleep(2000);
      DB.dbConnection.Open();
      Log.WriteLine("Reconnection to database 1 successful.");
    }

    public static bool openConnection(
      string dbHost,
      int dbPort,
      string dbName,
      string dbUsername,
      string dbPassword,
      int dbPoolsize)
    {
      try
      {
        Log.WriteLine("Connecting to " + dbName + " at " + dbHost + ":" + (object) dbPort + " for user '" + dbUsername + "'");
        DB.strConnection = "Server=" + dbHost + ";Port=" + (object) dbPort + ";Database=" + dbName + ";User=" + dbUsername + ";Password=" + dbPassword + ";Pooling=Yes;Min pool size=0;Max pool size=" + (object) dbPoolsize + ";Connection timeout=1;";
        DB.dbConnection = new MySqlConnection(DB.strConnection);
        DB.dbConnection.StateChange += new StateChangeEventHandler(DB.dbConnection_StateChange);
        DB.dbConnection.Open();
        if (DB.dbConnection.State == ConnectionState.Open)
        {
          Log.WriteLine("Connection to database successfull.");
          return true;
        }
        Log.WriteError("Failed to connect to " + dbName + " at " + dbHost + ":" + (object) dbPort + " for user '" + dbUsername + "'");
        return false;
      }
      catch (Exception ex)
      {
        Log.WriteError("Failed to connect! Error thrown was: " + ex.Message);
        return false;
      }
    }

    public static void closeConnection()
    {
      Log.WriteLine("Closing database connection...");
      try
      {
        DB.dbConnection.Close();
        Log.WriteLine("Database connection closed.");
      }
      catch
      {
        Log.WriteError("No database connection.");
      }
    }

    public static void runQuery(string query)
    {
      DB.QueryAsync(new Statment(query));
    }

    private static void QueryAsync(Statment statement)
    {
      MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection);
      mySqlConnection.Open();
      MySqlCommand command = mySqlConnection.CreateCommand();
      command.CommandText = statement.query;
      foreach (KeyValuePair<string, object> parameter in statement.parameters)
        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
      command.Prepare();
      command.BeginExecuteNonQuery(new AsyncCallback(DB.QueryAsyncCallback), (object) new DB.QueryObject<bool>((Action<bool>) null, command));
    }

    private static void QueryAsyncCallback(IAsyncResult iAr)
    {
      DB.QueryObject<bool> asyncState = (DB.QueryObject<bool>) iAr.AsyncState;
      try
      {
        MySqlConnection mySqlConnection = (MySqlConnection) null;
        using (MySqlCommand sqlCommand = asyncState.MySQLCommand)
        {
          sqlCommand.EndExecuteNonQuery(iAr);
          mySqlConnection = sqlCommand.Connection;
        }
        mySqlConnection?.Dispose();
        if (asyncState.OriginalCallback == null)
          return;
        asyncState.OriginalCallback(true);
      }
      catch
      {
        if (asyncState.OriginalCallback == null)
          return;
        asyncState.OriginalCallback(false);
      }
    }

    public static void runQueryNotAsync(string query)
    {
      DB.Query(new Statment(query));
    }

    private static void Query(Statment statement)
    {
      using (MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection))
      {
        mySqlConnection.Open();
        using (MySqlCommand command = mySqlConnection.CreateCommand())
        {
          command.CommandText = statement.query;
          foreach (KeyValuePair<string, object> parameter in statement.parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          command.Prepare();
          command.ExecuteNonQuery();
        }
      }
    }

    public static DataTable runRead(string Query)
    {
      return DB.Read(new Statment(Query));
    }

    public static object runReadOnce(string var, string Query)
    {
      DataTable dataTable = DB.Read(new Statment(Query + " LIMIT 1"));
      if (dataTable.Rows.Count > 0)
        return (object) dataTable.Rows[0][var].ToString();
      return new object();
    }

    private static DataTable Read(Statment statement)
    {
      DataTable dataTable = new DataTable();
      using (MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection))
      {
        mySqlConnection.Open();
        using (MySqlCommand command = mySqlConnection.CreateCommand())
        {
          command.CommandText = statement.query;
          foreach (KeyValuePair<string, object> parameter in statement.parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          command.Prepare();
          dataTable.Load((IDataReader) command.ExecuteReader());
        }
      }
      return dataTable;
    }

    public static bool checkExists(string Query)
    {
      try
      {
        return new MySqlCommand(Query + " LIMIT 1", DB.dbConnection).ExecuteReader().HasRows;
      }
      catch (Exception ex)
      {
        Log.WriteError("Error '" + ex.Message + "' at '" + Query + "'");
        return false;
      }
    }

    public static string Stripslash(string Query)
    {
      try
      {
        return Query.Replace("\\", "\\").Replace("'", "'").Replace("'", "`");
      }
      catch
      {
        return "";
      }
    }

    private struct QueryObject<T>
    {
      public Action<T> OriginalCallback;
      public MySqlCommand MySQLCommand;

      public QueryObject(Action<T> callback, MySqlCommand cmd)
      {
        this.OriginalCallback = callback;
        this.MySQLCommand = cmd;
      }
    }
  }
}
