// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_ZombieDropUse
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_ZombieDropUse : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive && room.channel != 3)
        return;
      switch (int.Parse(this.getBlock(7)))
      {
        case 0:
        case 2:
          --room.DropID;
          this.sendPacket = true;
          break;
        case 1:
          usr.Health = 1000;
          this.sendBlocks[10] = (object) usr.Health;
          goto case 0;
        case 3:
          int incubatorVehicleId = room.GetIncubatorVehicleId();
          Vehicle vehicleById = room.GetVehicleByID(incubatorVehicleId);
          if (vehicleById != null)
          {
            vehicleById.Health += 10000;
            if (vehicleById.Health > vehicleById.MaxHealth)
              vehicleById.Health = vehicleById.MaxHealth + 1;
            this.sendBlocks[10] = (object) vehicleById.Health;
            this.sendBlocks[11] = (object) vehicleById.MaxHealth;
            goto case 0;
          }
          else
            goto case 0;
        default:
          Log.WriteError("Unknown Zombie Drop ID: " + (object) int.Parse(this.getBlock(7)));
          goto case 0;
      }
    }

    internal enum DropType
    {
      Respawn,
      Medic,
      Ammo,
      Repair,
    }
  }
}
