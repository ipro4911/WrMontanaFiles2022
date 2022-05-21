// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.SP_LoginPacket
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginServer.Packets
{
  internal class SP_LoginPacket : Packet
  {
    public SP_LoginPacket(SP_LoginPacket.ErrorCodes errcode, params object[] blocks)
    {
      this.newPacket(4352);
      this.addBlock((object) (int) errcode);
      foreach (object block in blocks)
        this.addBlock(block);
    }

    public SP_LoginPacket(User usr)
    {
      this.newPacket(4352);
      this.addBlock((object) 1);
      this.addBlock((object) usr.userId);
      this.addBlock((object) 0);
      this.addBlock((object) usr.username);
      this.addBlock((object) "NULL");
      this.addBlock((object) usr.nickname);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) (usr.rank > 2 ? 5 : 0));
      this.addBlock((object) "WarRock");
      if (usr.clanid != -1)
      {
        this.addBlock((object) usr.clanid);
        this.addBlock((object) usr.clanname);
        this.addBlock((object) usr.clanrank);
        this.addBlock((object) usr.claniconid);
      }
      else
      {
        this.addBlock((object) -1);
        this.addBlock((object) "NULL");
        this.Fill((object) -1, 2);
      }
      this.Fill((object) 0, 4);
      IEnumerable<Server> source = ServersInformations.collected.Values.Where<Server>((Func<Server, bool>) (s => s.minrank <= usr.rank));
      this.addBlock((object) source.Count<Server>());
      foreach (Server server in source)
      {
        this.addBlock((object) server.id);
        this.addBlock((object) server.name);
        this.addBlock((object) server.ip);
        this.addBlock((object) 5340);
        this.addBlock((object) (LoginServer.Generic.getOnlinePlayers(server.id) * LoginServer.Generic.ServerSlots(server.slot)));
        this.addBlock((object) server.flag);
      }
      this.Fill((object) 0, 2);
    }

    public enum ErrorCodes
    {
      Nickname = 72000, // 0x00011940
      WrongUser = 72010, // 0x0001194A
      WrongPW = 72020, // 0x00011954
      AlreadyLoggedIn = 72030, // 0x0001195E
      BannedTime = 73020, // 0x00011D3C
      Banned = 73050, // 0x00011D5A
      AlreadyUsedNick = 74070, // 0x00012156
      InvalidArea = 110040, // 0x0001ADD8
      ChargebackBan = 110043, // 0x0001ADDB
    }
  }
}
