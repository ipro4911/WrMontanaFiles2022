// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.Explosive
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Room_Data;
using System;
using System.Collections.Generic;

namespace Game_Server.GameModes
{
  internal class Explosive
  {
    private Room room;

    ~Explosive()
    {
      GC.Collect();
    }

    public void sendNewRound(int WinningTeam)
    {
      if (this.room.waitExplosiveTime < 5 || this.room.EndGamefreeze)
        return;
      List<User> userList = new List<User>();
      userList.AddRange((IEnumerable<User>) this.room.users.Values);
      userList.AddRange((IEnumerable<User>) this.room.spectators.Values);
      WinningTeam = !this.room.bombPlanted || this.room.bombDefused ? 1 : 0;
      this.room.send((Packet) new SP_RoomDataNewRound(this.room, WinningTeam, false));
      this.room.send((Packet) new SP_InitializeNewRound(this.room));
      this.room.updateTime();
      this.room.timespent = 0;
      this.room.timeleft = this.room.mode != 15 ? 90000 : 180000;
      this.room.bombDefused = false;
      this.room.bombPlanted = false;
      this.room.sleep = false;
      this.room.isNewRound = false;
      this.room.waitExplosiveTime = 0;
      this.room.Placements.Clear();
    }

        public void prepareRound(int WinningTeam)
        {
            if (room.isNewRound == false)
            {
                switch (WinningTeam)
                {
                    case (int)Room.Side.Derbaran: room.DerbRounds++; break;
                    case (int)Room.Side.NIU: room.NIURounds++; break;
                }
                room.isNewRound = true;
            }

            if ((WinningTeam == (int)Room.Side.Derbaran && room.DerbRounds >= room.explosiveRounds || WinningTeam == (int)Room.Side.NIU && room.NIURounds >= room.explosiveRounds) && !room.EndGamefreeze)
            {
                room.EndGame();
            }
            else
            {
                room.sleep = true;
                room.waitExplosiveTime = 0;

                room.send(new SP_RoomDataNewRound(room, WinningTeam, true));

                foreach (User usr in room.tempPlayers)
                {
                    usr.isSpawned = false;
                    usr.throwNades = usr.throwRockets = 0;
                }
            }
        }

        public void CheckForNewRound()
        {
            if (room.mode == 0 && room.channel == 1 && !room.sleep)
            {
                if (room.AliveDerb == 0 && room.AliveNIU > 0 && room.bombPlanted == false)
                {
                    prepareRound((int)Room.Side.NIU);
                }
                else if (room.AliveNIU == 0 && room.AliveDerb > 0)
                {
                    prepareRound((int)Room.Side.Derbaran);
                }
                else if (room.AliveNIU == 0 && room.AliveDerb == 0)
                {
                    prepareRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU));
                }
            }
        }

        public void Update()
        {
            if (room != null)
            {
                if (room.users.Count > 1 && room.gameactive)
                {
                    if (room.isNewRound)
                    {
                        room.waitExplosiveTime++;

                        if (room.waitExplosiveTime >= 5)
                        {
                            if (room.AliveDerb == 0 && room.AliveNIU > 0 && room.bombPlanted == false)
                            {
                                sendNewRound((int)Room.Side.NIU);
                            }
                            else if (room.AliveNIU == 0 && room.AliveDerb > 0)
                            {
                                sendNewRound((int)Room.Side.Derbaran);
                            }
                            else
                            {
                                sendNewRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU));
                            }
                        }
                    }
                    else
                    {
                        if (room.NIURounds >= room.explosiveRounds || room.DerbRounds >= room.explosiveRounds) { room.EndGame(); return; }
                        if (room.timeleft <= 0) { prepareRound((room.bombPlanted ? (int)Room.Side.Derbaran : (int)Room.Side.NIU)); }
                        CheckForNewRound();
                    }
                }
            }
        }

        public Explosive(Room room)
        {
            this.room = room;
        }
    }
}