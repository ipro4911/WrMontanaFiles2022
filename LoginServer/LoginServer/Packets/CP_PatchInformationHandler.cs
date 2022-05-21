// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.CP_PatchInformationHandler
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Configs;

namespace LoginServer.Packets
{
  internal class CP_PatchInformationHandler : Handler
  {
    public override void Handle(User usr)
    {
      usr.send((Packet) new SP_PatchInformationPacket());
      Log.WriteLine("Replying to updater with informations [" + Patch.UpdateUrl + "]");
    }
  }
}
