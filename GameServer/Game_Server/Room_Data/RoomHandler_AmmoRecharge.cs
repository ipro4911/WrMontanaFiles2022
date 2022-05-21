// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_AmmoRecharge
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_AmmoRecharge : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (usr.LastAmmoRechargeTick > Generic.timestamp)
        return;
      usr.LastAmmoRechargeTick = Generic.timestamp + 4;
      usr.throwNades = (ushort) 0;
      usr.throwRockets = (ushort) 0;
      this.sendPacket = true;
    }
  }
}
