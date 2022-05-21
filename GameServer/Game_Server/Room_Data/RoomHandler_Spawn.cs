// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Spawn
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Spawn : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (!room.gameactive || usr.IsAlive() && room.mode != 1)
        return;
      int newMode = room.new_mode;
      int newModeSub = room.new_mode_sub;
      switch (newMode)
      {
        case 1:
          if (newModeSub == 0)
          {
            Item obj = ItemManager.GetItem("DA02");
            if (obj != null)
            {
              usr.weapon = obj.ID;
              break;
            }
            break;
          }
          break;
        case 3:
          if (newModeSub == 0)
          {
            Item obj = ItemManager.GetItem("DB01");
            if (obj != null)
            {
              usr.weapon = obj.ID;
              break;
            }
            break;
          }
          break;
        //case 4:
          switch (newModeSub)
          {
            case 0:
              Item obj1 = ItemManager.GetItem("DN01");
              if (obj1 != null)
              {
                usr.weapon = obj1.ID;
                break;
              }
              break;
            case 1:
              Item obj2 = ItemManager.GetItem("D202");
              if (obj2 != null)
              {
                usr.weapon = obj2.ID;
                break;
              }
              break;
          }
      //  case 5:
          switch (newModeSub)
          {
            case 0:
              Item obj3 = ItemManager.GetItem("DB25");
              if (obj3 != null)
              {
                usr.weapon = obj3.ID;
                break;
              }
              break;
            case 1:
              Item obj4 = ItemManager.GetItem("DC74");
              if (obj4 != null)
              {
                usr.weapon = obj4.ID;
                break;
              }
              break;
            case 2:
              Item obj5 = ItemManager.GetItem("DG42");
              if (obj5 != null)
              {
                usr.weapon = obj5.ID;
                break;
              }
              break;
          }
        case 6:
          if (newModeSub == 1)
          {
            Item obj6 = ItemManager.GetItem("DA06");
            if (obj6 != null)
            {
              usr.weapon = obj6.ID;
              break;
            }
            break;
          }
          break;
      }
      usr.Class = int.Parse(this.getBlock(7));
      if (usr.Class < 0 || usr.Class > 4)
      {
        Log.WriteLine(usr.nickname + " -> Invalid branch at spawn");
      }
      else
      {
        ++room.SpawnLocation;
        if (room.SpawnLocation >= 15)
          room.SpawnLocation = 0;
        if (room.mode == 1)
        {
          this.sendBlocks[10] = (object) room.SpawnLocation;
          this.sendBlocks[11] = (object) room.SpawnLocation;
          this.sendBlocks[12] = (object) room.SpawnLocation;
        }
        if (room.channel == 3)
        {
          ++room.SpawnedZombieplayers;
          if (room.SpawnedZombieplayers >= room.users.Count && !room.FirstWaveSent)
          {
            room.SendFirstWave = true;
            room.zombieRunning = true;
            if (room.zombie != null)
              room.SleepTime = 15;
            if (room.timeattack != null && usr.rDeaths <= 0)
            {
              if (room.zombiedifficulty == 0)
              {
                room.send((Packet) new SP_Unknown((ushort) 30053, new object[4]
                {
                  (object) 0,
                  (object) 0,
                  (object) 5,
                  (object) 5
                }));
                room.send((Packet) new SP_TimeAttackStage(room, 2, 300));
                room.timeattack.zombieForStage = 300;
              }
              else
              {
                room.send((Packet) new SP_Unknown((ushort) 30053, new object[4]
                {
                  (object) 0,
                  (object) 1,
                  (object) 4,
                  (object) 3
                }));
                room.send((Packet) new SP_TimeAttackStage(room, 2, 500));
                room.timeattack.zombieForStage = 500;
              }
              room.timeattack.Stage1.Start();
              ++room.timeattack.IntoPassing;
            }
          }
        }
        usr.classCode = this.getBlock(27);
        usr.Health = 1000;
        usr.Plantings = usr.skillPoints = 0;
        usr.spawnprotection = 3;
        usr.currentVehicle = (Vehicle) null;
        usr.HPLossTick = 0;
        usr.rKillSinceSpawn = 0;
        usr.throwNades = usr.throwRockets = (ushort) 0;
        usr.currentSeat = (VehicleSeat) null;
        if (room.mapid == 68 || room.mapid == 67)
          usr.spawnprotection = 10;
        room.firstspawn = true;
        usr.ExplosiveAlive = true;
        usr.isSpawned = true;
        this.sendPacket = true;
      }
    }
  }
}
