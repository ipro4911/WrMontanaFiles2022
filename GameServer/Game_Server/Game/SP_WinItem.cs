// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_WinItem
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_WinItem : Packet
  {
    public SP_WinItem(Game_Server.User usr, string itemcode, int days)
    {
      this.newPacket((ushort) 30720);
      this.addBlock((object) 1111);
      this.addBlock((object) 1);
      this.addBlock((object) "CB09");
      this.addBlock((object) Inventory.Itemlist(usr));
      this.addBlock((object) usr.AvailableSlots);
      this.addBlock((object) itemcode);
      this.addBlock((object) days);
      this.addBlock((object) usr.dinar);
    }
  }
}
