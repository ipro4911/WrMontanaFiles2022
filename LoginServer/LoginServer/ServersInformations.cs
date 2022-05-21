// Decompiled with JetBrains decompiler
// Type: LoginServer.ServersInformations
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System.Collections.Generic;
using System.Data;

namespace LoginServer
{
  internal class ServersInformations
  {
    public static Dictionary<int, Server> collected = new Dictionary<int, Server>();

    public static void loadServers()
    {
      DataTable dataTable = DB.runRead("SELECT * FROM servers WHERE visible='1' ORDER BY serverid ASC");
      for (int key = 0; key < dataTable.Rows.Count; ++key)
      {
        DataRow row = dataTable.Rows[key];
        Server server = new Server(int.Parse(row["serverid"].ToString()), row["name"].ToString(), row["ip"].ToString(), int.Parse(row["flag"].ToString()), int.Parse(row["minrank"].ToString()), int.Parse(row["slot"].ToString()));
        ServersInformations.collected.Add(key, server);
      }
      Log.WriteLine("Loaded " + (object) dataTable.Rows.Count + " (visible) servers!");
    }
  }
}
