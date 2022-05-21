// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.SP_PatchInformationUpdatePacket
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer.Packets
{
  internal class SP_PatchInformationUpdatePacket : Packet
  {
    public SP_PatchInformationUpdatePacket()
    {
      this.newPacket(4252);
      this.addBlock((object) ServersInformations.collected.Count);
      int num = 0;
      foreach (Server server in ServersInformations.collected.Values)
        num += Generic.getOnlinePlayers(server.id);
      this.addBlock((object) num);
    }
  }
}
