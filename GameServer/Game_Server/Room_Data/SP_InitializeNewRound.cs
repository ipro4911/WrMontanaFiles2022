// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.SP_InitializeNewRound
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class SP_InitializeNewRound : Packet
  {
    public SP_InitializeNewRound(Room r)
    {
      this.newPacket((ushort) 30000);
      this.addBlock((object) 1);
      this.addBlock((object) -1);
      this.addBlock((object) r.id);
      this.addBlock((object) 2);
      this.addBlock((object) 403);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) 3);
      this.addBlock((object) 363);
      this.addBlock((object) 0);
      this.addBlock((object) 1);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
    }
  }
}
