// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Suicide
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Suicide : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || room.mode == 0 || (room.mode == 1 || room.channel == 3) || (usr.Health <= 0 || usr.LastSuicideTick + 5 > Generic.timestamp || usr.currentVehicle != null))
        return;
      usr.LastSuicideTick = Generic.timestamp;
      if (int.Parse(this.getBlock(7)) == 5)
      {
        room.send((Packet) new SP_EntitySuicide(usr.roomslot, SP_EntitySuicide.SuicideType.Suicide, true));
        usr.OnDie();
      }
      else
      {
        if (usr.Health > 0)
        {
          usr.OnDie();
          room.updateTime();
        }
        this.sendPacket = true;
      }
    }
  }
}
