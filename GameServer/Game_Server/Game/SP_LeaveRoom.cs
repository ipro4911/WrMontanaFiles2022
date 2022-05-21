// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_LeaveRoom
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_LeaveRoom : Packet
  {
    public SP_LeaveRoom(int sessionId, Room r, int oldSlot, int newMaster)
    {
      this.newPacket((ushort) 29504);
      this.addBlock((object) 1);
      this.addBlock((object) sessionId);
      this.addBlock((object) oldSlot);
      this.addBlock((object) 1);
      this.addBlock((object) newMaster);
      this.addBlock((object) 0);
      this.addBlock((object) 0);
    }

    public SP_LeaveRoom(Game_Server.User usr, Room r, int oldSlot, int newMaster)
    {
      this.newPacket((ushort) 29504);
      this.addBlock((object) 1);
      this.addBlock((object) usr.sessionId);
      this.addBlock((object) oldSlot);
      this.addBlock((object) 1);
      this.addBlock((object) newMaster);
      this.addBlock((object) usr.exp);
      this.addBlock((object) usr.dinar);
    }
  }
}
