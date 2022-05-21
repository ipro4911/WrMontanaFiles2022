// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.CP_NewUserHandler
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System.Data;

namespace LoginServer.Packets
{
  internal class CP_NewUserHandler : Handler
  {
    public override void Handle(User usr)
    {
      if (usr.userId > 0)
      {
        string input = DB.Stripslash(this.getBlock(0));
        DataTable dataTable = DB.runRead("SELECT * FROM users WHERE nickname='" + input + "'");
        bool flag = Generic.isAlphaNumeric(input);
        usr.firstlogin = true;
        if (flag)
        {
          if (dataTable.Rows.Count == 0)
          {
            usr.nickname = input;
            DB.runQuery("UPDATE users SET nickname='" + input + "', firstlogin='2', ticketid='" + (object) usr.sessionId + "', serverid='-1' WHERE id='" + (object) usr.userId + "'");
            usr.send((Packet) new SP_LoginPacket(usr));
          }
          else
            usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.AlreadyUsedNick, new object[0]));
        }
        else
          usr.send((Packet) new SP_LoginPacket(SP_LoginPacket.ErrorCodes.Nickname, new object[0]));
      }
      else
        usr.disconnect();
    }
  }
}
