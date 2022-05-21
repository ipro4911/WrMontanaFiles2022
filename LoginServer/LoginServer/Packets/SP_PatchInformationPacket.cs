// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.SP_PatchInformationPacket
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Configs;

namespace LoginServer.Packets
{
  internal class SP_PatchInformationPacket : Packet
  {
    public SP_PatchInformationPacket()
    {
      this.newPacket(4112);
      this.addBlock((object) Patch.Format);
      this.addBlock((object) Patch.Launcher);
      this.addBlock((object) Patch.Updater);
      this.addBlock((object) Patch.Client);
      this.addBlock((object) Patch.Sub);
      this.addBlock((object) Patch.Option);
      this.addBlock((object) Patch.UpdateUrl);
    }
  }
}
