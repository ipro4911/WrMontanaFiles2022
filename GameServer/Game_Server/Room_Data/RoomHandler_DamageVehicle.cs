// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_DamageVehicle
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_DamageVehicle : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (!room.gameactive || room.sleep && room.bombDefused || this.sendBlocks.Length < 27)
        return;
      if (room.firstInGameTS > Game_Server.Generic.timestamp)
      {
        usr.disconnect();
      }
      else
      {
        if (!room.gameactive || usr.Health <= 0)
          return;
        if (room.Vehicles.Count == 0)
        {
          Log.WriteError("No vehicles for map " + (object) room.mapid);
        }
        else
        {
          int Type = int.Parse(this.getBlock(6));
          int num1 = int.Parse(this.getBlock(7));
          int onVehicleKill = Game_Server.Configs.Server.Experience.OnVehicleKill;
          string str = this.getBlock(27).Substring(0, 4);
          int num2 = int.Parse(this.getBlock(15));
          bool flag = this.getBlock(14) == "1";
          if (num1 < 0 || num1 > room.Vehicles.Count)
            return;
          Vehicle vehicleById = room.GetVehicleByID(num1);
          if (vehicleById.Side == room.GetSide(usr) || vehicleById.SpawnProtection > 0 || (vehicleById.Health <= 0 || vehicleById == null))
            return;
          int num3 = Type == 1 ? 0 : 1;
          int num4;
          if (usr.currentVehicle != null)
          {
            if (!usr.currentVehicle.IsRightVehicle(str) && usr.channel != 3)
              return;
            num4 = int.Parse(this.getBlock(13)) != 0 ? ItemManager.GetVehicleDamage(usr.currentSeat.SubCTCode, Type) : ItemManager.GetVehicleDamage(usr.currentSeat.MainCTCode, Type);
            if (room.channel == 3 && room.mode == 11 && room.GetIncubatorVehicleId() == num1)
            {
              if (!room.isZombieWeapon(str))
                return;
              num4 = 150 * (room.zombiedifficulty + 1);
            }
          }
          else
          {
            num4 = ItemManager.GetVehicleDamage(str, Type);
            if (room.channel == 3 && room.mode == 11 && room.GetIncubatorVehicleId() == num1)
            {
              if (!room.isZombieWeapon(str))
                return;
              num4 = 100 * (room.zombiedifficulty + 1);
            }
          }
          if (num3 == 1 && num2 > 0 && (num2 < 100 && flag))
            num4 = (int) Math.Ceiling((double) (num4 * num2) / 100.0);
          if (vehicleById.Code == "EN01")
            num4 = 250;
          else if (vehicleById.Code == "EJ05" && str.StartsWith("DK"))
            num4 = 800;
          else if (vehicleById.Code == "EN17")
            num4 *= 15;
          vehicleById.Health -= num4;
          this.sendBlocks[3] = (object) 104;
          this.sendBlocks[8] = (object) usr.roomslot;
          this.sendBlocks[16] = (object) vehicleById.Health;
          this.sendBlocks[17] = (object) (vehicleById.Health + num4);
          this.sendBlocks[27] = (object) str;
          if (vehicleById.Health >= 0 && vehicleById.Code == "EN17" && num4 != 0)
            usr.timeattackDamagedDoor += num4;
          if (vehicleById.Health <= 0 && vehicleById.Code == "EN17")
          {
            room.timeattack.PreparingStage = true;
            room.timeattack.Destructed = true;
            room.timeattack.Stage3.Stop();
            room.timeattack.milliSec = room.timeattack.Stage3.ElapsedMilliseconds;
            room.send((Packet) new SP_ScoreboardInformations(room, room.timeattack.milliSec));
            room.timeattack.milliSec = 0L;
            if (room.zombiedifficulty == 1)
              room.timeleft += 240000;
            else
              room.send((Packet) new SP_TimeAttackEnd(room));
          }
          if (vehicleById.Health <= 0)
          {
            room.CheckForMission(usr, num1);
            if (vehicleById.Players.Count > 0)
            {
              object[] sendBlocks = this.sendBlocks;
              sendBlocks[3] = (object) 152;
              foreach (VehicleSeat vehicleSeat in (IEnumerable<VehicleSeat>) vehicleById.Seats.Values)
              {
                if (vehicleSeat.seatOwner != null && vehicleSeat.seatOwner.currentVehicle == vehicleById)
                {
                  if (!room.firstblood)
                  {
                    room.firstblood = true;
                    byte[] bytes = new SP_CustomSound(SP_CustomSound.Sounds.FirstBlood).GetBytes();
                    foreach (Game_Server.User user in room.users.Values.Where<Game_Server.User>((Func<Game_Server.User, bool>) (r => r.mapLoaded)))
                      user.sendBuffer(bytes);
                  }
                  if (vehicleSeat.seatOwner.Health > 0)
                  {
                    int roomslot = vehicleSeat.seatOwner.roomslot;
                    vehicleSeat.seatOwner.OnDie();
                    usr.rPoints += onVehicleKill;
                    ++usr.rKills;
                    if (room.mode == 8)
                    {
                      usr.TotalWarPoint += 5;
                      vehicleSeat.seatOwner.TotalWarSupport += 2;
                    }
                    sendBlocks[7] = (object) roomslot;
                    sendBlocks[19] = room.mode == 8 ? (object) usr.TotalWarPoint : sendBlocks[19];
                    sendBlocks[20] = room.mode == 8 ? (object) usr.TotalWarPoint : sendBlocks[20];
                    sendBlocks[21] = (object) (onVehicleKill.ToString() + ".00");
                    sendBlocks[22] = (object) "2.000";
                    sendBlocks[27] = (object) str;
                    room.send((Packet) new SP_RoomData(sendBlocks));
                  }
                }
              }
            }
            if (vehicleById.Players.Count > 0 && room.mode == 8)
            {
              switch (room.GetSide(usr))
              {
                case 0:
                  room.TotalWarDerb += 5;
                  room.TotalWarNIU += 2;
                  break;
                case 1:
                  room.TotalWarNIU += 5;
                  room.TotalWarDerb += 2;
                  break;
              }
            }
            room.updateTime();
            room.send((Packet) new SP_RoomVehicleExplode(room.id, vehicleById.ID, usr.roomslot));
            if ((num1 == 25 || num1 == 24 || num1 == 23) && (room.mapid == 42 && room.mode == 5))
              room.EndGame();
            else if (num1 == 0 && room.mapid == 60 && room.mode == 5)
              room.EndGame();
            if (room.explosive != null)
            {
              room.explosive.CheckForNewRound();
            }
            else
            {
              if (room.heromode == null)
                return;
              room.heromode.CheckForNewRound();
            }
          }
          else
            this.sendPacket = true;
        }
      }
    }
  }
}
