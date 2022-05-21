// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.SP_ReceiveConnection
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer.Packets
{
  internal class SP_ReceiveConnection : Packet
  {
    public SP_ReceiveConnection()
    {
      this.newPacket(4608);
      this.addBlock((object) 1);
      this.addBlock((object) 77);
    }
  }
}
