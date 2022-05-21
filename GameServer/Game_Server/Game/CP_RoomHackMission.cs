// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_RoomHackMission
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_RoomHackMission : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      Room room = usr.room;
      int num1 = room.HackPercentage.BaseA + room.HackPercentage.BaseB;
      if (!room.gameactive || num1 >= 100)
        return;
      usr.hackingBase = int.Parse(this.getBlock(3));
      int Type1 = 2;
      if (this.getBlock(1) == "0" && this.getBlock(2) == "0" && (this.getBlock(3) == "0" || this.getBlock(3) == "1"))
        usr.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, 0, usr.hackingBase));
      else if (this.getBlock(1) == "1" && this.getBlock(2) == "6" && (this.getBlock(3) == "0" && !room.PickuppedC4))
      {
        if (room.mapid == 60)
        {
          if (room.GetSide(usr) != 0)
            return;
          room.PickuppedC4 = true;
          usr.hasC4 = true;
          room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
          {
            (object) 0,
            (object) 0,
            (object) 1,
            (object) 6,
            (object) 0,
            (object) 0,
            (object) -1,
            (object) 0
          }));
          room.send((Packet) new SP_Unknown((ushort) 30000, new object[15]
          {
            (object) 1,
            (object) usr.roomslot,
            (object) usr.room.id,
            (object) 2,
            (object) 155,
            (object) 0,
            (object) 0,
            (object) 92,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0
          }));
        }
        else
          Log.WriteError("Tried to pickup C4 in room " + (object) room.mapid);
      }
      else if (this.getBlock(1) == "1" && this.getBlock(2) == "0" && this.getBlock(3) == "0")
      {
        if (room.mapid == 60)
        {
          room.PickuppedC4 = false;
          usr.hasC4 = false;
          room.SiegeWarC4User = usr;
          room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
          {
            (object) 0,
            (object) 0,
            (object) 1,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0,
            (object) 0
          }));
          room.SiegeWarTime = 3;
        }
        else
          Log.WriteError("Tried to use C4 in room " + (object) room.mapid);
      }
      else if (int.Parse(this.getBlock(2)) == 3)
      {
        usr.isHacking = false;
        int Type2 = 3;
        room.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, Type2, usr.hackingBase));
      }
      else
      {
        if (room == null || usr.LastHackTick > Generic.timestamp && usr.LastHackTick > 0)
          return;
        if (usr.hackingBase != usr.LastHackBase)
          usr.hackTick = 0;
        if (usr.LastHackTick < Generic.timestamp || usr.LastHackTick == 0)
        {
          usr.LastHackTick = Generic.timestamp + 1;
          ++usr.hackTick;
          ++usr.rPoints;
          usr.hackTick = 0;
          if (usr.hackingBase == 0)
            ++room.HackPercentage.BaseA;
          else
            ++room.HackPercentage.BaseB;
        }
        int num2 = room.HackPercentage.BaseA + room.HackPercentage.BaseB;
        if (usr.GMMode)
          num2 = 100;
        usr.LastHackBase = usr.hackingBase;
        ++usr.hackPercentage;
        usr.isHacking = true;
        if (num2 >= 100)
        {
          if (room.mapid == 60)
          {
            if (room.Mission1 == null)
            {
              room.Mission1 = usr.nickname;
              usr.rPoints += Game_Server.Configs.Server.Experience.OnMissionHack;
              room.send((Packet) new SP_Unknown((ushort) 29985, new object[6]
              {
                (object) 0,
                (object) 0,
                (object) 1,
                (object) 1,
                (object) 99,
                (object) 0
              }));
              room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
              {
                (object) 0,
                (object) 0,
                (object) 0,
                (object) 4,
                (object) 1,
                (object) 100,
                (object) -1,
                (object) 0
              }));
              room.send((Packet) new SP_Unknown((ushort) 29985, new object[8]
              {
                (object) 0,
                (object) -1,
                (object) 1,
                (object) 5,
                (object) -1,
                (object) 0,
                (object) -1,
                (object) 0
              }));
              room.flags[2] = 1;
              room.flags[3] = 0;
              room.flags[0] = room.flags[1] = -1;
              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
              {
                (object) 1,
                (object) -1,
                (object) room.id,
                (object) 2,
                (object) 156,
                (object) 0,
                (object) 1,
                (object) 2,
                (object) 1,
                (object) -1,
                (object) 2,
                (object) 0,
                (object) 92,
                (object) -1,
                (object) 0,
                (object) 0,
                (object) 1333333,
                (object) -1166666,
                (object) 1333333,
                (object) 0,
                (object) 3689.667,
                (object) 969.9617,
                (object) 4332.0752,
                (object) 64.4469,
                (object) 37.4174,
                (object) -290.5969,
                (object) 0,
                (object) 0,
                (object) "DU02"
              }));
              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
              {
                (object) 1,
                (object) -1,
                (object) room.id,
                (object) 2,
                (object) 156,
                (object) 0,
                (object) 1,
                (object) 3,
                (object) 0,
                (object) -1,
                (object) 2,
                (object) 0,
                (object) 92,
                (object) -1,
                (object) 0,
                (object) 0,
                (object) 1333333,
                (object) -1166666,
                (object) 1333333,
                (object) 0,
                (object) 3689.667,
                (object) 969.9617,
                (object) 4332.0752,
                (object) 64.4469,
                (object) 37.4174,
                (object) -290.5969,
                (object) 0,
                (object) 0,
                (object) "DU02"
              }));
              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
              {
                (object) 1,
                (object) -1,
                (object) room.id,
                (object) 2,
                (object) 156,
                (object) 0,
                (object) 1,
                (object) 0,
                (object) -1,
                (object) 0,
                (object) 0,
                (object) 0,
                (object) 92,
                (object) -1,
                (object) 0,
                (object) 0,
                (object) 1333333,
                (object) -1166666,
                (object) 1333333,
                (object) 0,
                (object) 3689.667,
                (object) 969.9617,
                (object) 4332.0752,
                (object) 64.4469,
                (object) 37.4174,
                (object) -290.5969,
                (object) 0,
                (object) 0,
                (object) "DU02"
              }));
              room.send((Packet) new SP_Unknown((ushort) 30000, new object[29]
              {
                (object) 1,
                (object) -1,
                (object) room.id,
                (object) 2,
                (object) 156,
                (object) 0,
                (object) 1,
                (object) 1,
                (object) -1,
                (object) 1,
                (object) 1,
                (object) 0,
                (object) 92,
                (object) -1,
                (object) 0,
                (object) 0,
                (object) 1333333,
                (object) -1166666,
                (object) 1333333,
                (object) 0,
                (object) 3689.667,
                (object) 969.9617,
                (object) 4332.0752,
                (object) 64.4469,
                (object) 37.4174,
                (object) -290.5969,
                (object) 0,
                (object) 0,
                (object) "DU02"
              }));
              room.PickuppedC4 = false;
            }
          }
          else if (room.timeattack != null)
          {
            room.timeattack.Stage2.Stop();
            room.timeattack.milliSec = room.timeattack.Stage2.ElapsedMilliseconds;
            room.timeleft += 360000;
            room.send((Packet) new SP_ScoreboardInformations(room, room.timeattack.milliSec));
            room.timeattack.milliSec = 0L;
                room.timeattack.PreparingStage = true;
          }
        }
        room.send((Packet) new SP_RoomHackMission(usr.roomslot, usr.hackingBase == 0 ? room.HackPercentage.BaseA : room.HackPercentage.BaseB, Type1, usr.hackingBase));
      }
    }
  }
}
