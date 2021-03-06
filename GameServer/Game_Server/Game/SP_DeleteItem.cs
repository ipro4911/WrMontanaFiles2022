// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_DeleteItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_DeleteItem : Packet
  {
    public SP_DeleteItem(Game_Server.User usr, string itemCode)
    {
      this.newPacket((ushort) 30224);
      this.addBlock((object) 1);
      this.addBlock((object) itemCode);
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      for (int c = 0; c < 5; ++c)
        this.addBlock((object) usr.GetEquipment(c));
    }
  }
}
