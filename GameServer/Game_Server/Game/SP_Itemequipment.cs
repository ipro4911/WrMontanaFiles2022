// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Itemequipment
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_Itemequipment : Packet
  {
    public SP_Itemequipment(SP_Itemequipment.ErrorCodes Code)
    {
      this.newPacket((ushort) 29970);
      this.addBlock((object) (int) Code);
    }

    public SP_Itemequipment(int Class, string equipment)
    {
      this.newPacket((ushort) 29970);
      this.addBlock((object) 1);
      this.addBlock((object) Class);
      this.addBlock((object) equipment);
    }

    internal enum ErrorCodes
    {
      AlreadyEquipped = 97090, // 0x00017B42
    }
  }
}
