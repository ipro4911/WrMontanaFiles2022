// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_StorageInventoryUpdate
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_StorageInventoryUpdate : Packet
  {
    public SP_StorageInventoryUpdate(SP_StorageInventoryUpdate.ErrorCode code)
    {
      this.newPacket((ushort) 30720);
      this.addBlock((object) 1400);
      this.addBlock((object) (uint) code);
    }

    public SP_StorageInventoryUpdate(Game_Server.User usr, int action, int index, string itemCode)
    {
      this.newPacket((ushort) 30720);
      this.addBlock((object) 1400);
      this.addBlock((object) 1);
      this.addBlock((object) action);
      this.addBlock((object) usr.storageInventoryMax);
      this.addBlock((object) index);
      this.addBlock((object) itemCode);
      this.addBlock((object) Inventory.Storage(usr));
      this.addBlock((object) Inventory.Itemlist(usr));
    }

    internal enum ErrorCode : uint
    {
      NoInventoryFreeSpace = 97070, // 0x00017B2E
      NoStorageFreeSpace = 97071, // 0x00017B2F
    }
  }
}
