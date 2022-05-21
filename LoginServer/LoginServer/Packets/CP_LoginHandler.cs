// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.CP_LoginHandler
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Data;
using System.Globalization;

namespace LoginServer.Packets
{
  internal class CP_LoginHandler : Handler
  {
    public override void Handle(User usr)
    {
      string str = this.getBlock(2).Trim();
      if (str.Length <= 0 || str.Length > 20)
        return;
      string Input1 = this.getBlock(3).Trim();
      int num;
      try
      {
        num = int.Parse(DB.runReadOnce("id", "SELECT * FROM users WHERE username='" + str + "'").ToString());
      }
      catch
      {
        num = 0;
      }
      if (num > 0)
      {
        DataTable dataTable1 = DB.runRead("SELECT id, username, password, salt, online, nickname, rank, firstlogin, banned, bantime, clanid, clanrank FROM users WHERE id='" + (object) num + "'");
        if (dataTable1.Rows.Count > 0)
        {
          DataRow row1 = dataTable1.Rows[0];
          string Input2 = row1["salt"].ToString();
          if (Generic.convertToMD5(Generic.convertToMD5(Input1) + Generic.convertToMD5(Input2)) == row1["password"].ToString())
          {
            usr.userId = int.Parse(row1["id"].ToString());
            usr.username = row1["username"].ToString();
            usr.nickname = row1["nickname"].ToString();
            usr.rank = int.Parse(row1["rank"].ToString());
            usr.clanid = int.Parse(row1["clanid"].ToString());
            usr.clanrank = int.Parse(row1["clanrank"].ToString());
            bool flag1 = row1["online"].ToString() == "1";
            usr.firstlogin = row1["firstlogin"].ToString() == "0";
            bool flag2 = usr.rank == 0 || row1["banned"].ToString() == "1";
            string s = row1["bantime"].ToString();
            if (!flag2)
            {
              if (!flag1)
              {
                if (usr.clanid != -1)
                {
                  DataTable dataTable2 = DB.runRead("SELECT name, iconid FROM clans WHERE id='" + (object) usr.clanid + "'");
                  if (dataTable2.Rows.Count > 0)
                  {
                    DataRow row2 = dataTable2.Rows[0];
                    usr.clanname = row2["name"].ToString();
                    usr.claniconid = long.Parse(row2["iconid"].ToString());
                  }
                }
                DB.runQuery("UPDATE users SET ticketid='" + (object) usr.sessionId + "' WHERE id='" + (object) usr.userId + "'");
                if (usr.firstlogin || usr.nickname.Length <= 0)
                {
                  usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.Nickname, new object[0]));
                  Log.WriteLine("Connection from " + usr.ip + " logged in successfully in as new user (" + str + ")");
                }
                else
                {
                  usr.send((Packet) new SP_LoginPacket(usr));
                  Log.WriteLine("Connection from " + usr.ip + " logged in successfully as " + usr.nickname);
                }
              }
              else
              {
                usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.AlreadyLoggedIn, new object[0]));
                Log.WriteError("Connection from " + usr.ip + " tried to log on " + str + " but he is logged in");
              }
            }
            else
            {
              DateTime result;
              DateTime.TryParseExact(s, "yyMMddHH", (IFormatProvider) null, DateTimeStyles.None, out result);
              DateTime now = DateTime.Now;
              if (result.Year < now.Year)
              {
                usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.Banned, new object[0]));
              }
              else
              {
                TimeSpan timeSpan = result - now;
                usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.Banned, new object[1]
                {
                  (object) timeSpan.Minutes
                }));
              }
              Log.WriteError("Connection from " + usr.ip + " as " + str + " but he/she is banned");
            }
          }
          else
          {
            usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.WrongPW, new object[0]));
            Log.WriteError("Connection from " + usr.ip + " tried to log on as " + str + " with a wrong password");
          }
        }
        else
        {
          usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.WrongUser, new object[0]));
          Log.WriteError("Connection from " + usr.ip + " failed to log on as " + str);
        }
      }
      else
      {
        usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.WrongUser, new object[0]));
        Log.WriteError("Connection from " + usr.ip + " failed to log on as " + str);
      }
    }
  }
}
