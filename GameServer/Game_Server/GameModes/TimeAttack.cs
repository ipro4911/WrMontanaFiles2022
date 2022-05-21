// Decompiled with JetBrains decompiler
// Type: Game_Server.GameModes.TimeAttack
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Game_Server.GameModes
{
  internal class TimeAttack
  {
    public int sleepBeforeEverything = 5;
    public Stopwatch Stage1 = new Stopwatch();
    public Stopwatch Stage2 = new Stopwatch();
    public Stopwatch Stage3 = new Stopwatch();
    public Stopwatch Stage4 = new Stopwatch();
    public int waitBeforeSupplyBoxItemsOut = -1;
    public int[] chooses = new int[4]{ -1, -1, -1, -1 };
    public Room room;
    public int LastTick;
    public int Stage;
    public int zombieForStage;
    public long milliSec;
    public int IntoPassing;
    public int PowPlayer;
    public bool Destructed;
    public bool BossKilled;
    public bool IsTimeOpenDoor;
    public int TimeToOpenDoor;
    public bool PreparingStage;
        internal int stage1ZombieCount;
        internal object time;
        public bool ElapsedMilliseconds;

        ~TimeAttack()
    {
      GC.Collect();
    }

    public void RunTimeAttack()
    {
      if (this.LastTick == DateTime.Now.Second)
        return;
      this.LastTick = DateTime.Now.Second;
      if (!this.room.zombieRunning)
        return;
      if (this.sleepBeforeEverything >= 0)
      {
        --this.sleepBeforeEverything;
      }
      else
      {
        if (this.room.zombiedifficulty == 0 && this.Destructed)
        {
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) this.room.users.Values)
          {
            if (user.RandomSupplyBoxSelected)
              this.room.EndGame();
          }
        }
        if (this.room.zombiedifficulty == 1 && this.BossKilled)
        {
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) this.room.users.Values)
          {
            if (user.RandomSupplyBoxSelected)
              this.room.EndGame();
          }
        }
        if (this.room.timeleft <= 0)
        {
          this.room.send((Packet) new SP_TimeAttackEndLose(this.room));
          this.room.EndGame();
        }
        if (this.IsTimeOpenDoor)
        {
          this.TimeToOpenDoor += 1000;
          if (this.PowPlayer >= this.IntoPassing && this.Stage == 0)
          {
            this.room.send((Packet) new SP_Unknown((ushort) 30053, new object[1]
            {
              (object) 5
            }));
            this.room.send((Packet) new SP_Unknown((ushort) 30053, new object[1]
            {
              (object) 4
            }));
            if (this.TimeToOpenDoor >= 6000)
            {
              this.room.send((Packet) new SP_Unknown((ushort) 30053, new object[1]
              {
                (object) 2
              }));
              this.Stage2.Start();
              this.IsTimeOpenDoor = false;
              this.PowPlayer = 0;
              this.TimeToOpenDoor = 0;
              ++this.Stage;
              this.ResetZombie();
              this.PreparingStage = false;
            }
          }
          if (this.PowPlayer >= this.IntoPassing && this.Stage == 1)
          {
            this.room.send((Packet) new SP_ZombieNewStage(this.room, 5));
            this.room.send((Packet) new SP_ZombieNewStage(this.room, 4));
            if (this.TimeToOpenDoor >= 6000)
            {
              this.room.send((Packet) new SP_Unknown((ushort) 30053, new object[1]
              {
                (object) 2
              }));
              this.Stage3.Start();
              this.IsTimeOpenDoor = false;
              this.PowPlayer = 0;
              this.TimeToOpenDoor = 0;
              ++this.Stage;
              this.ResetZombie();
              this.PreparingStage = false;
            }
          }
          if (this.PowPlayer >= this.IntoPassing && this.Stage == 2 && this.room.zombiedifficulty == 1)
          {
            this.room.send((Packet) new SP_ZombieNewStage(this.room, 5));
            this.room.send((Packet) new SP_ZombieNewStage(this.room, 4));
            if (this.TimeToOpenDoor >= 6000)
            {
              this.room.send((Packet) new SP_Unknown((ushort) 30053, new object[1]
              {
                (object) 2
              }));
              this.Stage4.Start();
              this.IsTimeOpenDoor = false;
              this.PowPlayer = 0;
              this.TimeToOpenDoor = 0;
              ++this.Stage;
              this.ResetZombie();
              this.PreparingStage = false;
            }
          }
        }
        else
          this.TimeToOpenDoor = 0;
        this.room.timeleft -= 1000;
        if (this.room.Zombies.Where<KeyValuePair<int, Zombie>>((Func<KeyValuePair<int, Zombie>, bool>) (r => r.Value.Health > 0)).Count<KeyValuePair<int, Zombie>>() >= 20 || this.PreparingStage)
          return;
        Random random = new Random();
        switch (this.Stage)
        {
          case 0:
            this.room.SpawnZombie(0);
            if (random.Next(0, 2) == 0)
              this.room.SpawnZombie(1);
            if (this.room.SpawnedZombies > 30)
            {
              for (int index = 0; index < 3; ++index)
              {
                if (random.Next(0, 5) == 0)
                  this.room.SpawnZombie(5);
              }
              if (random.Next(0, 6) == 0)
                this.room.SpawnZombie(6);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(2);
            }
            if (this.room.SpawnedZombies > 100)
            {
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(3);
              if (random.Next(0, 5) == 0)
                this.room.SpawnZombie(8);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(7);
            }
            if (this.room.SpawnedZombies <= 200)
              break;
            if (random.Next(0, 3) == 0)
              this.room.SpawnZombie(4);
            if (this.room.zombiedifficulty <= 0 || random.Next(0, 5) != 0)
              break;
            this.room.SpawnZombie(9);
            break;
          case 1:
            this.room.SpawnZombie(0);
            if (random.Next(0, 2) == 0)
              this.room.SpawnZombie(1);
            if (this.room.SpawnedZombies > 30)
            {
              if (this.room.spawnedBombers < 1)
                this.room.SpawnZombie(14);
              for (int index = 0; index < 3; ++index)
              {
                if (random.Next(0, 5) == 0)
                  this.room.SpawnZombie(5);
              }
              if (random.Next(0, 6) == 0)
                this.room.SpawnZombie(6);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(2);
            }
            if (this.room.SpawnedZombies > 100)
            {
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(3);
              if (random.Next(0, 5) == 0)
                this.room.SpawnZombie(8);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(7);
            }
            if (this.room.SpawnedZombies <= 200)
              break;
            if (random.Next(0, 3) == 0)
              this.room.SpawnZombie(4);
            if (this.room.zombiedifficulty <= 0 || random.Next(0, 5) != 0)
              break;
            this.room.SpawnZombie(9);
            break;
          case 2:
            this.room.SpawnZombie(0);
            if (this.room.spawnedDefeders < 1)
              this.room.SpawnZombie(15);
            if (random.Next(0, 2) == 0)
              this.room.SpawnZombie(1);
            if (this.room.SpawnedZombies > 30)
            {
              for (int index = 0; index < 3; ++index)
              {
                if (random.Next(0, 5) == 0)
                  this.room.SpawnZombie(5);
              }
              if (random.Next(0, 6) == 0)
                this.room.SpawnZombie(6);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(2);
            }
            if (this.room.SpawnedZombies > 100)
            {
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(3);
              if (random.Next(0, 5) == 0)
                this.room.SpawnZombie(8);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(7);
            }
            if (this.room.SpawnedZombies <= 200)
              break;
            if (random.Next(0, 3) == 0)
              this.room.SpawnZombie(4);
            if (this.room.zombiedifficulty <= 0 || random.Next(0, 5) != 0)
              break;
            this.room.SpawnZombie(9);
            break;
          case 3:
            this.room.SpawnZombie(0);
            if (this.room.spawnedBreakers == 0)
              this.room.SpawnZombie(16);
            if (random.Next(0, 2) == 0)
              this.room.SpawnZombie(1);
            if (this.room.SpawnedZombies > 30)
            {
              if (this.room.mapid == 55 && this.room.spawnedBusters < 2)
                this.room.SpawnZombie(10);
              for (int index = 0; index < 3; ++index)
              {
                if (random.Next(0, 5) == 0)
                  this.room.SpawnZombie(5);
                if (random.Next(0, 6) == 0)
                  this.room.SpawnZombie(6);
              }
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(2);
            }
            if (this.room.SpawnedZombies > 100)
            {
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(3);
              if (random.Next(0, 5) == 0)
                this.room.SpawnZombie(8);
              if (random.Next(0, 3) == 0)
                this.room.SpawnZombie(7);
            }
            if (this.room.SpawnedZombies <= 200)
              break;
            if (random.Next(0, 3) == 0)
              this.room.SpawnZombie(4);
            if (this.room.zombiedifficulty <= 0 || random.Next(0, 5) != 0)
              break;
            this.room.SpawnZombie(9);
            break;
        }
      }
    }

    public void ResetZombie()
    {
      this.room.spawnedMadmans = 0;
      this.room.spawnedManiacs = 0;
      this.room.spawnedGrinders = 0;
      this.room.spawnedGrounders = 0;
      this.room.spawnedHeavys = 0;
      this.room.spawnedGrowlers = 0;
      this.room.spawnedLovers = 0;
      this.room.spawnedHandgemans = 0;
      this.room.spawnedChariots = 0;
      this.room.spawnedCrushers = 0;
      this.room.spawnedBusters = 0;
      this.room.spawnedCrashers = 0;
      this.room.spawnedEnvys = 0;
      this.room.spawnedClaws = 0;
      this.room.spawnedBombers = 0;
      this.room.spawnedDefeders = 0;
      this.room.spawnedBreakers = 0;
      this.room.spawnedMadSoldiers = 0;
      this.room.spawnedMadPrisoners = 0;
      this.room.spawnedSuperHeavYs = 0;
      this.room.spawnedLadYs = 0;
      this.room.spawnedMidgets = 0;
      this.room.SpawnedZombies = 0;
      this.room.Zombies.Clear();
      for (int index = 0; index < 28; ++index)
        this.room.Zombies.Add(index + 4, new Zombie(index + 4, 0, 0, 0));
    }

    public void Update()
    {
      if (this.waitBeforeSupplyBoxItemsOut > 0)
      {
        --this.waitBeforeSupplyBoxItemsOut;
        if (this.waitBeforeSupplyBoxItemsOut <= 0 && Array.IndexOf((Array) this.chooses, (object) "-1") > 0)
        {
          this.waitBeforeSupplyBoxItemsOut = -1;
          foreach (Game_Server.User user in (IEnumerable<Game_Server.User>) this.room.users.Values)
          {
            if (user.timeattackBoxChoose <= -1)
            {
              user.timeattackBoxChoose = ((IEnumerable<int>) this.chooses).Where<int>((Func<int, bool>) (r => r == -1)).FirstOrDefault<int>();
              this.chooses[user.timeattackBoxChoose] = user.userId;
            }
          }
        }
      }
      this.RunTimeAttack();
    }

    public TimeAttack(Room room)
    {
      this.room = room;
      this.Stage = 0;
      this.sleepBeforeEverything = 5;
      room.zombieRunning = false;
      room.ZombiePoints = room.SpawnedZombies = room.SpawnedZombieplayers = room.KilledZombies = room.KillsBeforeDrop = room.ZombieSpawnPlace = 0;
      room.spawnedMadmans = room.spawnedManiacs = room.spawnedGrinders = room.spawnedGrounders = room.spawnedHeavys = room.spawnedGrowlers = room.spawnedLovers = room.spawnedHandgemans = room.spawnedChariots = room.spawnedCrushers = room.spawnedBusters = room.spawnedCrashers = room.spawnedEnvys = room.spawnedClaws = room.spawnedBombers = room.spawnedDefeders = room.spawnedBreakers = room.spawnedMadSoldiers = room.spawnedMadPrisoners = room.spawnedSuperHeavYs = room.spawnedLadYs = room.spawnedMidgets = 0;
      room.Zombies.Clear();
      for (int index = 0; index < 28; ++index)
        room.Zombies.Add(index + 4, new Zombie(index + 4, 0, 0, 0));
    }
  }
}
