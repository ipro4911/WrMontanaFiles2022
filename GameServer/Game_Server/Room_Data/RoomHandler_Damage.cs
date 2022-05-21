// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Damage
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using Game_Server.Managers;
using System;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Damage : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      if (!room.gameactive || room.sleep && room.bombDefused || room.firstInGameTS > Generic.timestamp)
        return;
      int index = int.Parse(this.getBlock(7));
      uint num1 = uint.Parse(this.getBlock(15)) - usr.sessionId;
      string str1 = this.getBlock(22);
      int id = int.Parse(this.getBlock(8));
      int.Parse(this.getBlock(13));
      int num2 = this.getBlock(14) == "1" ? 1 : 0;
      string str2 = this.getBlock(27).Substring(0, 4);
      if (ItemManager.GetItem(str2) == null)
        return;
      int onKillPoints = Game_Server.Configs.Server.Experience.OnKillPoints;
      int Type1 = 1;
      switch (num1)
      {
        case 1237:
          Type1 = 0;
          break;
        case 1239:
          Type1 = 1;
          break;
        case 1241:
          Type1 = 2;
          break;
      }
      if (str2.StartsWith("DM") || str2.StartsWith("DN") || (str2.StartsWith("DJ") || str2.StartsWith("DK")) && num1 >= 1237U)
        Type1 = 0;
      int num3 = ItemManager.GetDamage(str2, Type1);
      if (usr.currentVehicle != null)
      {
        bool flag = int.Parse(this.getBlock(13)) == 0;
        if (str2 == usr.currentVehicle.Code)
        {
          onKillPoints += Game_Server.Configs.Server.Experience.OnVehicleKillAdditional;
          num3 = !flag ? ItemManager.GetDamage(usr.currentSeat.SubCTCode, 2) : ItemManager.GetDamage(usr.currentSeat.MainCTCode, 2);
        }
      }
      else if (room.new_mode == 5)
        num3 = 1000;
      bool flag1 = str2.StartsWith("DN") || str2.StartsWith("DM");
      int num4 = new int[2]{ 75, 100 }[flag1 ? 0 : 1];
      if (num1 > 0U && (long) num1 < (long) num4)
        num3 = (int) Math.Ceiling((double) ((long) num3 * (long) num1) / 100.0);
      if ((usr.Health <= 0 || !usr.IsAlive()) && (num1 < 0U || num1 > 100U) || !usr.IsWhitelistedWeapon(str2) && usr.currentVehicle != null && str2 != usr.currentVehicle.Code)
        return;
      bool flag2 = false;
      if (room.channel == 3)
      {
        if (index >= 4)
        {
          if (Type1 == 0 && !str2.StartsWith("DA") && (!str2.StartsWith("DN") && !str2.StartsWith("DJ")) && (!str2.StartsWith("DM") && !str2.StartsWith("DK")))
          {
            str1 = "99.0000";
            flag2 = true;
          }
          Zombie zombieById = room.GetZombieByID(index);
          if (room.isZombieWeapon(str2) && zombieById.Damage != 800 && zombieById.Name != "Lover")
            return;
          if (zombieById != null && zombieById.Health > 0)
          {
            if (zombieById.timestamp > Generic.timestamp)
              return;
            int points = zombieById.Points;
            if (flag2)
              points *= 2;
            zombieById.Health -= num3;
            this.sendBlocks[16] = (object) zombieById.Health;
            this.sendBlocks[17] = (object) (zombieById.Health + num3);
            this.sendBlocks[27] = (object) str2;
            if (room.timeattack != null)
            {
              if (zombieById.Name == "Breaker")
                usr.timeattackBossDamage += num3;
              if (zombieById.Health <= 0 && zombieById.Name == "Breaker")
              {
                room.timeattack.PreparingStage = true;
                room.timeattack.BossKilled = true;
                room.timeattack.Stage4.Stop();
                room.timeattack.milliSec = room.timeattack.Stage4.ElapsedMilliseconds;
                room.send((Packet) new SP_ScoreboardInformations(room, room.timeattack.milliSec));
                room.timeattack.milliSec = 0L;
                room.send((Packet) new SP_TimeAttackEnd(room));
              }
              if (room.KilledZombies == room.timeattack.zombieForStage && room.timeattack.Stage == 0)
              {
                room.timeattack.Stage1.Stop();
                room.timeattack.milliSec = room.timeattack.Stage1.ElapsedMilliseconds;
                room.timeleft += 480000;
                room.send((Packet) new SP_ScoreboardInformations(room, room.timeattack.milliSec));
                room.timeattack.milliSec = 0L;
                room.timeattack.PreparingStage = true;
              }
            }
            if (zombieById.Health <= 0)
            {
              ++room.KillsBeforeDrop;
              if ((room.zombie != null ? room.zombie.Wave : room.timeattack.Stage) >= (room.zombie != null ? 3 : 0) && room.KillsBeforeDrop >= 3)
              {
                int Type2 = room.RandomDrop();
                room.send((Packet) new SP_ZombieDrop(usr, index, room.DropID, Type2));
                ++room.DropID;
                room.KillsBeforeDrop = 0;
              }
              if (room.zombie != null && zombieById.Type == 7 && usr.skillPoints < 20)
              {
                if (!usr.HasItem("CC51"))
                {
                  ++usr.skillPoints;
                  if (usr.skillPoints == 5 || usr.skillPoints == 10 || usr.skillPoints == 20)
                    room.send((Packet) new SP_ZombieSkillpointUpdate(usr));
                }
                else
                {
                  usr.skillPoints += 2;
                  if (usr.skillPoints == 6 || usr.skillPoints == 10 || usr.skillPoints == 20)
                    room.send((Packet) new SP_ZombieSkillpointUpdate(usr));
                }
              }
              usr.rPoints += points;
              ++usr.rKills;
              zombieById.Health = 0;
              zombieById.respawn = Generic.timestamp + 4;
              ++room.KilledZombies;
              room.ZombiePoints += points;
              this.sendBlocks[3] = (object) 152;
              this.sendBlocks[8] = (object) id;
              this.sendBlocks[22] = (object) str1;
            }
          }
        }
        else if (index <= 3 && id >= 4)
        {
          Zombie zombieById = room.GetZombieByID(id);
          if (id == usr.roomslot || zombieById == null || (zombieById.Health <= 0 || usr.Health <= 0) || zombieById.ID == -1)
            return;
          if (!usr.GMMode)
            usr.Health -= zombieById.Damage;
          this.sendBlocks[8] = (object) id;
          this.sendBlocks[16] = (object) usr.Health;
          this.sendBlocks[17] = (object) (usr.Health + zombieById.Damage);
          this.sendBlocks[27] = (object) str2;
          if (usr.Health <= 0)
          {
            this.sendBlocks[3] = (object) 152;
            usr.OnDie();
          }
          if (usr.rDeaths == 5 && room.timeattack != null && room.zombiedifficulty == 0 || usr.rDeaths == 3 && room.timeattack != null && room.zombiedifficulty == 1)
          {
            room.send((Packet) new SP_TimeAttackEndLose(room));
            room.EndGame();
          }
        }
      }
      else
      {
        Game_Server.User user = room.users[index];
        if (user == null || user.spawnprotection > 0 || room.GetSide(usr) == room.GetSide(user) && room.mode != 1)
          return;
        if (num1 == 1237U && !str2.StartsWith("DA") && (!str2.StartsWith("DN") && !str2.StartsWith("DJ")) && (!str2.StartsWith("DM") && !str2.StartsWith("DK")))
        {
          num3 += 200;
          str1 = "99.0000";
          flag2 = true;
        }
        if (user.Health >= 0)
        {
          if (room.mode == 16 && (room.mapid == 90 || room.mapid == 91))
            user.Health -= 250;
          else
            user.Health -= num3;
          this.sendBlocks[8] = (object) id;
          this.sendBlocks[16] = (object) user.Health;
          this.sendBlocks[17] = (object) (user.Health + num3);
          this.sendBlocks[18] = (object) onKillPoints;
          this.sendBlocks[22] = (object) str1;
          this.sendBlocks[27] = (object) str2;
          if (user.Health <= 0)
          {
            if (room.heromode != null && (user.roomslot == room.derbHeroUsr || user.roomslot == room.niuHeroUsr))
              onKillPoints += 20;
            user.OnDie();
            if (room.mode == 8)
            {
              usr.TotalWarPoint = 0;
              user.TotalWarSupport += 2;
              this.sendBlocks[19] = (object) usr.TotalWarPoint;
              this.sendBlocks[20] = (object) usr.TotalWarSupport;
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
            if (flag2)
            {
              usr.rPoints += Game_Server.Configs.Server.Experience.OnHSKillPoints;
              ++usr.rHeadShots;
              if (room.new_mode != 6 && room.new_mode_sub != 0)
                usr.send((Packet) new SP_CustomSound(SP_CustomSound.Sounds.HeadShot));
            }
            ++usr.rKillSinceSpawn;
            if (!room.firstblood)
            {
              room.firstblood = true;
              usr.rPoints += 3;
              usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.FirstKill));
            }
            if (usr.lastKillUser == user.roomslot)
            {
              usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.RevengeKill));
              ++usr.rPoints;
            }
            if (flag1)
            {
              usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.GrenadeKill));
              ++usr.rPoints;
            }
            else if (flag2)
            {
              usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.HeadShot));
              ++usr.rPoints;
            }
            switch (usr.rKillSinceSpawn)
            {
              case 2:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.DoubleKill));
                ++usr.rPoints;
                break;
              case 3:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.TripleKill));
                ++usr.rPoints;
                break;
              case 4:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.QuadraKill));
                usr.rPoints += 2;
                break;
              case 5:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.PentaKill));
                usr.rPoints += 2;
                break;
              case 6:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.HexaKill));
                usr.rPoints += 3;
                break;
              case 7:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.UltraKill));
                usr.rPoints += 3;
                break;
              case 8:
                usr.send((Packet) new SP_KillAnimation(SP_KillAnimation.Type.Assasin));
                usr.rPoints += 2;
                break;
            }
            user.lastKillUser = usr.roomslot;
            if (Generic.random(0, 500) < 20)
              usr.RandomGunsmithResource();
            if (usr.rKills > room.highestkills)
              room.highestkills = usr.rKills;
            if (room.KillsDerbaranLeft == 30 || room.KillsNIULeft == 30 || (room.NIURounds >= 6 || room.DerbRounds >= 6))
              this.lobbychanges = true;
            if (user.currentVehicle != null)
              onKillPoints += 5;
            this.sendBlocks[3] = (object) 152;
            ++usr.rKills;
            usr.rPoints += onKillPoints;
            if (room.explosive != null)
            {
              this.sendPacket = false;
              room.send((Packet) new SP_RoomData(this.sendBlocks));
              room.explosive.CheckForNewRound();
              return;
            }
            if (room.heromode != null)
            {
              this.sendPacket = false;
              room.send((Packet) new SP_RoomData(this.sendBlocks));
              room.heromode.CheckForNewRound();
              return;
            }
          }
        }
      }
      this.sendPacket = true;
    }
  }
}
