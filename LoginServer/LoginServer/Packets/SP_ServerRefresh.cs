// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.SP_ServerRefresh
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginServer.Packets
{
  internal class SP_ServerRefresh : Packet
  {
    public SP_ServerRefresh(User usr)
    {
      this.newPacket(4609);
      IEnumerable<Server> source = ServersInformations.collected.Values.Where<Server>((Func<Server, bool>) (s => s.minrank <= usr.rank));
      this.addBlock((object) source.Count<Server>());
      this.addBlock((object) "");
      foreach (Server server in source)
      {
        this.addBlock((object) server.id);
        this.addBlock((object) server.name);
        this.addBlock((object) server.ip);
        this.addBlock((object) 5340);
        this.addBlock((object) (LoginServer.Generic.getOnlinePlayers(server.id) * LoginServer.Generic.ServerSlots(server.slot)));
        this.addBlock((object) server.flag);
      }
    }
  }
}
