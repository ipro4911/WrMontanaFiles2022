// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.RankingList
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Data;

namespace Game_Server.Game
{
  internal class RankingList
  {
    public static short hour = -1;
    public static List<RankingList.User> UserByEXP = new List<RankingList.User>();
    public static List<RankingList.User> UserByWins = new List<RankingList.User>();
    public static List<RankingList.User> UserByKills = new List<RankingList.User>();
    public static List<RankingList.Clan> ClanByEXP = new List<RankingList.Clan>();
    public static List<RankingList.Clan> ClanByWins = new List<RankingList.Clan>();
    public static List<RankingList.Clan> ClanByMembers = new List<RankingList.Clan>();

    public static void Load()
    {
      if ((int) RankingList.hour == DateTime.Now.Hour)
        return;
      RankingList.hour = (short) DateTime.Now.Hour;
      RankingList.UserByEXP.Clear();
      RankingList.UserByWins.Clear();
      RankingList.UserByKills.Clear();
      RankingList.ClanByEXP.Clear();
      RankingList.ClanByWins.Clear();
      RankingList.ClanByMembers.Clear();
      DataTable dataTable1 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY exp DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable1.Rows.Count; ++index)
      {
        DataRow row = dataTable1.Rows[index];
        if (row != null)
        {
          RankingList.User user = new RankingList.User();
          user.nickname = row["nickname"].ToString();
          int ID = int.Parse(row["clanid"].ToString());
          Game_Server.Clan clan = ClanManager.GetClan(ID);
          if (ID >= 0 && clan != null)
          {
            user.clanname = clan.name;
            user.claniconid = (int) clan.iconid;
          }
          else
          {
            user.clanname = "NULL";
            user.claniconid = -1;
          }
          user.exp = uint.Parse(row["exp"].ToString());
          user.kills = uint.Parse(row["kills"].ToString());
          user.deaths = uint.Parse(row["deaths"].ToString());
          user.wins = uint.Parse(row["wonMatchs"].ToString());
          user.loses = uint.Parse(row["lostMatchs"].ToString());
          RankingList.UserByEXP.Add(user);
        }
      }
      DataTable dataTable2 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY wonMatchs DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable2.Rows.Count; ++index)
      {
        DataRow row = dataTable2.Rows[index];
        if (row != null)
        {
          RankingList.User user = new RankingList.User();
          user.nickname = row["nickname"].ToString();
          int ID = int.Parse(row["clanid"].ToString());
          Game_Server.Clan clan = ClanManager.GetClan(ID);
          if (ID >= 0 && clan != null)
          {
            user.clanname = clan.name;
            user.claniconid = (int) clan.iconid;
          }
          else
          {
            user.clanname = "NULL";
            user.claniconid = -1;
          }
          user.exp = uint.Parse(row["exp"].ToString());
          user.kills = uint.Parse(row["kills"].ToString());
          user.deaths = uint.Parse(row["deaths"].ToString());
          user.wins = uint.Parse(row["wonMatchs"].ToString());
          user.loses = uint.Parse(row["lostMatchs"].ToString());
          RankingList.UserByWins.Add(user);
        }
      }
      DataTable dataTable3 = DB.RunReader("SELECT * FROM users WHERE rank < 4 AND rank > 0 AND banned != '1' ORDER BY kills DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable3.Rows.Count; ++index)
      {
        DataRow row = dataTable3.Rows[index];
        if (row != null)
        {
          RankingList.User user = new RankingList.User();
          user.nickname = row["nickname"].ToString();
          int ID = int.Parse(row["clanid"].ToString());
          Game_Server.Clan clan = ClanManager.GetClan(ID);
          if (ID >= 0 && clan != null)
          {
            user.clanname = clan.name;
            user.claniconid = (int) clan.iconid;
          }
          else
          {
            user.clanname = "NULL";
            user.claniconid = -1;
          }
          user.exp = uint.Parse(row["exp"].ToString());
          user.kills = uint.Parse(row["kills"].ToString());
          user.deaths = uint.Parse(row["deaths"].ToString());
          user.wins = uint.Parse(row["wonMatchs"].ToString());
          user.loses = uint.Parse(row["lostMatchs"].ToString());
          RankingList.UserByKills.Add(user);
        }
      }
      DataTable dataTable4 = DB.RunReader("SELECT * FROM clans ORDER BY exp DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable4.Rows.Count; ++index)
      {
        DataRow row = dataTable4.Rows[index];
        if (row != null)
          RankingList.ClanByEXP.Add(new RankingList.Clan()
          {
            id = uint.Parse(row["iconid"].ToString()),
            name = row["name"].ToString(),
            claniconid = int.Parse(row["iconid"].ToString()),
            wins = uint.Parse(row["win"].ToString()),
            loses = uint.Parse(row["lose"].ToString()),
            exp = uint.Parse(row["exp"].ToString()),
            usercount = uint.Parse(row["count"].ToString())
          });
      }
      DataTable dataTable5 = DB.RunReader("SELECT * FROM clans ORDER BY win DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable5.Rows.Count; ++index)
      {
        DataRow row = dataTable5.Rows[index];
        if (row != null)
          RankingList.ClanByWins.Add(new RankingList.Clan()
          {
            id = uint.Parse(row["iconid"].ToString()),
            name = row["name"].ToString(),
            claniconid = int.Parse(row["iconid"].ToString()),
            wins = uint.Parse(row["win"].ToString()),
            loses = uint.Parse(row["lose"].ToString()),
            exp = uint.Parse(row["exp"].ToString()),
            usercount = uint.Parse(row["count"].ToString())
          });
      }
      DataTable dataTable6 = DB.RunReader("SELECT * FROM clans ORDER BY count DESC LIMIT 0, 100");
      for (int index = 0; index < dataTable6.Rows.Count; ++index)
      {
        DataRow row = dataTable6.Rows[index];
        if (row != null)
          RankingList.ClanByMembers.Add(new RankingList.Clan()
          {
            id = uint.Parse(row["iconid"].ToString()),
            name = row["name"].ToString(),
            claniconid = int.Parse(row["iconid"].ToString()),
            wins = uint.Parse(row["win"].ToString()),
            loses = uint.Parse(row["lose"].ToString()),
            exp = uint.Parse(row["exp"].ToString()),
            usercount = uint.Parse(row["count"].ToString())
          });
      }
    }

    internal class User
    {
      public uint id;
      public uint kills;
      public uint exp;
      public uint deaths;
      public uint wins;
      public uint loses;
      public int claniconid;
      public string nickname;
      public string clanname;
    }

    internal class Clan
    {
      public uint id;
      public uint wins;
      public uint loses;
      public uint usercount;
      public uint exp;
      public int claniconid;
      public string name;

      public int GetRank()
      {
        if (this.usercount >= 81U)
          return 9;
        if (this.usercount >= 61U)
          return 8;
        if (this.usercount >= 51U)
          return 7;
        if (this.usercount >= 41U)
          return 6;
        if (this.usercount >= 31U)
          return 5;
        if (this.usercount >= 21U)
          return 4;
        if (this.usercount >= 11U)
          return 3;
        return this.usercount >= 6U ? 2 : 1;
      }
    }
  }
}
