// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_Clan
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_Clan : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      switch (int.Parse(this.getBlock(0)))
      {
        case 0:
          string ClanName = this.getBlock(1).Replace(" ", "");
          if (ClanName.Length <= 4)
            break;
          ClanManager.CheckForDuplicate(usr, ClanName);
          break;
        case 1:
          string Name = this.getBlock(1).Replace(" ", "");
          if (Name.Length <= 4)
            break;
          ClanManager.AddClan(usr, Name);
          break;
        case 2:
          int ID1 = int.Parse(this.getBlock(1));
          if (ID1 <= 0)
            break;
          string ClanJoinDate1 = DateTime.Now.ToString("dd/MM/yyyy");
          Game_Server.Clan clan1 = ClanManager.GetClan(ID1);
          if (clan1 == null || clan1.ClanUsers.Count >= clan1.maxUsers)
            break;
          ClanPendingUsers clanPendingUsers1 = new ClanPendingUsers(usr.userId, usr.nickname, usr.exp.ToString(), ClanJoinDate1);
          clan1.pendingUsers.TryAdd(usr.userId, clanPendingUsers1);
          usr.clan = clan1;
          usr.send((Packet) new SP_Clan(SP_Clan.ClanCodes.ApplyClan));
          if (usr.clan != null)
            DB.RunQuery("UPDATE clans_invite SET clanid='" + (object) ID1 + "' WHERE userid='" + (object) usr.userId + "'");
          else
            DB.RunQuery("INSERT INTO clans_invite (userid, clanid) VALUES ('" + (object) usr.userId + "', '" + (object) ID1 + "')");
          DB.RunQuery("UPDATE users SET clanrank='9', clanid='" + (object) ID1 + "' WHERE id='" + (object) usr.userId + "'");
          break;
        case 3:
          if (usr.clan == null)
            break;
          Game_Server.Clan clan2 = usr.clan;
          if (clan2.pendingUsers.ContainsKey(usr.userId))
          {
            ClanPendingUsers clanPendingUsers2;
            clan2.pendingUsers.TryRemove(usr.userId, out clanPendingUsers2);
          }
          int num1 = clan2.clanRank(usr);
          if (num1 == 2)
          {
            foreach (Game_Server.User user in clan2.GetUsers())
              user.clan = (Game_Server.Clan) null;
            ClanManager.RemoveClan(usr);
            DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE clanid='" + (object) clan2.id + "'");
            DB.RunQuery("DELETE FROM clans WHERE id='" + (object) clan2.id + "'");
            break;
          }
          usr.clan = (Game_Server.Clan) null;
          if (num1 == 9)
          {
            ClanPendingUsers clanPendingUsers2;
            clan2.pendingUsers.TryRemove(usr.userId, out clanPendingUsers2);
          }
          else
          {
            ClanUsers clanUsers;
            clan2.ClanUsers.TryRemove(usr.userId, out clanUsers);
          }
          usr.send((Packet) new SP_Clan(SP_Clan.ClanCodes.LeaveClan));
          DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + (object) usr.userId + "'");
          DB.RunQuery("UPDATE clans SET count='" + (object) usr.clan.ClanUsers.Count + "' WHERE id='" + (object) usr.clan.id + "'");
          break;
        case 4:
          if (usr.clan == null)
            break;
          usr.send((Packet) new SP_Clan.MyClanInformation(usr));
          break;
        case 5:
          if (usr.clan == null)
            break;
          int num2 = int.Parse(this.getBlock(1));
          int num3 = usr.clan.clanRank(usr);
          switch (num2)
          {
            case 1:
              usr.send((Packet) new SP_Clan.UserList.NormalUser(usr.clan));
              return;
            case 2:
              if (num3 != 1 && num3 != 2)
                return;
              usr.send((Packet) new SP_Clan.UserList.Pending(usr));
              return;
            default:
              return;
          }
        case 6:
          CP_Clan.SearchType searchType = (CP_Clan.SearchType) int.Parse(this.getBlock(1));
          int.Parse(this.getBlock(1));
          string key = this.getBlock(2);
          List<Game_Server.Clan> clanList = new List<Game_Server.Clan>();
          List<Game_Server.Clan> list1;
          switch (searchType)
          {
            case CP_Clan.SearchType.Name:
              list1 = new List<Game_Server.Clan>((IEnumerable<Game_Server.Clan>) ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (c =>
              {
                if (c != null)
                  return c.name.ToLower().Contains(key.ToLower());
                return false;
              })).ToArray<Game_Server.Clan>());
              break;
            case CP_Clan.SearchType.Master:
              list1 = new List<Game_Server.Clan>((IEnumerable<Game_Server.Clan>) ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (c =>
              {
                if (c != null)
                  return c.Master.ToLower().Contains(key.ToLower());
                return false;
              })).ToArray<Game_Server.Clan>());
              break;
            default:
              list1 = new List<Game_Server.Clan>((IEnumerable<Game_Server.Clan>) ClanManager.Clans.Values.Where<Game_Server.Clan>((Func<Game_Server.Clan, bool>) (c =>
              {
                if (c == null)
                  return false;
                if (!c.Master.ToLower().Contains(key.ToLower()))
                  return c.name.ToLower().Contains(key.ToLower());
                return true;
              })).ToArray<Game_Server.Clan>());
              break;
          }
          if (key.Length >= 3)
          {
            usr.send((Packet) new SP_Clan.SearchClan(list1));
            break;
          }
          usr.send((Packet) new SP_Clan.CheckClan(SP_Clan.CheckClan.ErrorCodes.NotFound));
          break;
        case 7:
          int ID2 = int.Parse(this.getBlock(1));
          if (ID2 <= 0)
            break;
          Game_Server.Clan clan3 = ClanManager.GetClan(ID2);
          if (clan3 == null)
            break;
          usr.send((Packet) new SP_Clan.UserList.NormalUser(usr, clan3));
          break;
        case 8:
          if (usr.clan == null)
            break;
          int num4 = usr.clan.clanRank(usr);
          if (num4 < 1 || num4 == 9 || usr.clan == null)
            break;
          string str = DB.Stripslash(this.getBlock(2));
          if (this.getBlock(1) == "0")
            usr.clan.Description = str;
          else
            usr.clan.Announcment = str;
          DB.RunQuery("UPDATE clans SET description='" + usr.clan.Description + "', announcment='" + usr.clan.Announcment + "' WHERE id='" + (object) usr.clan.id + "'");
          break;
        case 9:
          if (usr.clan == null)
            break;
          int Subtype = int.Parse(this.getBlock(1));
          int num5 = int.Parse(this.getBlock(2));
          Game_Server.Clan clan4 = usr.clan;
          switch (Subtype)
          {
            case 0:
              if (clan4.ClanUsers.Count >= clan4.maxUsers)
              {
                usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> No more slot available for the clan, please expand if is possible", usr.sessionId, usr.nickname));
                return;
              }
              string ClanJoinDate2 = DateTime.Now.ToString("dd/MM/yyyy");
              DataTable dataTable1 = DB.RunReader("SELECT * FROM users WHERE id='" + (object) num5 + "'");
              if (dataTable1.Rows.Count > 0)
              {
                DataRow row = dataTable1.Rows[0];
                DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + (object) num5 + "'");
                DB.RunQuery("UPDATE clans SET count='" + (object) clan4.ClanUsers.Count + "' WHERE id='" + (object) clan4.id + "'");
                DB.RunQuery("UPDATE users SET clanid='" + (object) clan4.id + "', clanrank='0', clanjoindate='" + ClanJoinDate2 + "' WHERE id='" + (object) num5 + "'");
                if (clan4.pendingUsers.ContainsKey(num5))
                {
                  ClanPendingUsers clanPendingUsers2;
                  clan4.pendingUsers.TryRemove(num5, out clanPendingUsers2);
                }
                ClanUsers clanUsers = new ClanUsers(num5, row["nickname"].ToString(), row["exp"].ToString(), ClanJoinDate2, 0);
                clan4.ClanUsers.TryAdd(num5, clanUsers);
              }
              Game_Server.User user1 = UserManager.GetUser(num5);
              if (user1 != null)
              {
                user1.clan = clan4;
                clan4.Users.TryAdd(num5, user1);
              }
              if (clan4.pendingUsers.ContainsKey(num5))
              {
                ClanPendingUsers clanPendingUsers2;
                clan4.pendingUsers.TryRemove(num5, out clanPendingUsers2);
                break;
              }
              break;
            case 1:
              DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + (object) num5 + "'");
              DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + (object) num5 + "'");
              Game_Server.User user2 = UserManager.GetUser(num5);
              if (user2 != null)
                user2.clan = (Game_Server.Clan) null;
              if (clan4.pendingUsers.ContainsKey(num5))
              {
                ClanPendingUsers clanPendingUsers2;
                clan4.pendingUsers.TryRemove(num5, out clanPendingUsers2);
                break;
              }
              break;
          }
          DB.RunQuery("DELETE FROM clans_invite WHERE userid='" + (object) num5 + "'");
          usr.send((Packet) new SP_Clan.UserList.Pending(Subtype, num5));
          break;
        case 10:
          if (usr.clan == null)
            break;
          int SubType = int.Parse(this.getBlock(1));
          int userId = int.Parse(this.getBlock(2));
          if (usr.clan.clanRank(usr) < 1)
            break;
          int num6 = 0;
          switch (SubType)
          {
            case 0:
              num6 = 1;
              DB.RunQuery("UPDATE users SET clanrank='1' WHERE id='" + (object) userId + "'");
              break;
            case 1:
              num6 = 0;
              DB.RunQuery("UPDATE users SET clanrank='0' WHERE id='" + (object) userId + "'");
              break;
            case 2:
              DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + (object) userId + "'");
              DB.RunQuery("UPDATE clans SET count='" + (object) usr.clan.ClanUsers.Count + "' WHERE id='" + (object) usr.clan.id + "'");
              Game_Server.User user3;
              usr.clan.Users.TryRemove(userId, out user3);
              ClanUsers clanUsers1;
              usr.clan.ClanUsers.TryRemove(userId, out clanUsers1);
              break;
          }
          if (SubType != 2)
          {
            usr.clan.ClanUsers.Values.Where<ClanUsers>((Func<ClanUsers, bool>) (s => s.id == userId)).First<ClanUsers>().clanrank = num6;
          }
          else
          {
            Game_Server.User user4 = UserManager.GetUser(userId);
            if (user4 != null)
              user4.clan = (Game_Server.Clan) null;
          }
          usr.send((Packet) new SP_Clan.Change(SubType, userId));
          break;
        case 11:
          if (usr.clan == null)
            break;
          int num7 = int.Parse(this.getBlock(1));
          Game_Server.Clan clan = usr.clan;
          if (clan == null)
            break;
          DataTable dataTable2 = DB.RunReader("SELECT * FROM users WHERE id='" + (object) num7 + "'");
          if (dataTable2.Rows.Count <= 0)
            break;
          DataRow row1 = dataTable2.Rows[0];
          if (num7 == usr.userId)
            break;
          DB.RunQuery("UPDATE users SET clanrank='0' WHERE id='" + (object) usr.userId + "'");
          DB.RunQuery("UPDATE users SET clanrank='2' WHERE id='" + (object) num7 + "'");
          clan.Master = row1["nickname"].ToString();
          clan.MasterEXP = row1["exp"].ToString();
          clan.ClanUsers.Values.Where<ClanUsers>((Func<ClanUsers, bool>) (r => string.Compare(r.nickname, clan.Master, true) == 0)).First<ClanUsers>().clanrank = 2;
          clan.ClanUsers.Values.Where<ClanUsers>((Func<ClanUsers, bool>) (r => string.Compare(r.nickname, usr.nickname, true) == 0)).First<ClanUsers>().clanrank = 0;
          byte[] bytes = new SP_Chat("ClanSystem", SP_Chat.ChatType.Clan, "ClanSystem >> " + usr.nickname + " passed master to " + clan.Master + " :/", (uint) clan.id, "NULL").GetBytes();
          foreach (Game_Server.User user4 in (IEnumerable<Game_Server.User>) clan.Users.Values)
            user4.sendBuffer(bytes);
          usr.send((Packet) new SP_Clan.Change());
          break;
        case 12:
          if (usr.clan == null)
            break;
          string block = this.getBlock(1);
          Game_Server.Clan clanByName = ClanManager.GetClanByName(block);
          if (usr.clan == null)
            break;
          if (clanByName == null)
          {
            if (!usr.HasItem("CB02"))
              break;
            DB.RunQuery("UPDATE clans SET name='" + block + "' WHERE id='" + (object) usr.clan.id + "'");
            clanByName.name = block;
            usr.deleteItem("CB02");
            usr.send((Packet) new SP_Clan.Change(usr, true));
            break;
          }
          usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Whisper, "SYSTEM >> A clan has already this name, please choose another one", usr.sessionId, usr.nickname));
          break;
        case 14:
          if (usr.clan == null)
            break;
          uint num8 = uint.Parse(this.getBlock(1));
          if (!usr.HasItem("CB54") || usr.clan == null)
            break;
          DB.RunQuery("UPDATE clans SET iconid='" + (object) num8 + "' WHERE id='" + (object) usr.clan.id + "'");
          usr.clan.iconid = num8;
          usr.deleteItem("CB54");
          usr.send((Packet) new SP_Clan.Change(usr, false));
          break;
        case 16:
          if (usr.clan == null)
            break;
          ClanManager.RemoveClan(usr);
          break;
        case 17:
          int page = int.Parse(this.getBlock(1));
          int sortType = int.Parse(this.getBlock(2));
          List<Game_Server.Clan> list2 = new List<Game_Server.Clan>();
          switch (sortType)
          {
            case 0:
              list2 = ClanManager.Clans.Values.OrderByDescending<Game_Server.Clan, int>((Func<Game_Server.Clan, int>) (r => r.exp)).Skip<Game_Server.Clan>(page * 10).Take<Game_Server.Clan>(10).ToList<Game_Server.Clan>();
              break;
            case 1:
              list2 = ClanManager.Clans.Values.OrderBy<Game_Server.Clan, int>((Func<Game_Server.Clan, int>) (r => r.exp)).Skip<Game_Server.Clan>(page * 10).Take<Game_Server.Clan>(10).ToList<Game_Server.Clan>();
              break;
          }
          usr.send((Packet) new SP_Clan.SearchClan(page, sortType, list2));
          break;
        default:
          Log.WriteError("Unknown Clan Subtype " + (object) 16);
          break;
      }
    }

    internal enum Subtype
    {
      CheckForDuplicate = 0,
      AddClan = 1,
      ApplyClan = 2,
      LeaveClan = 3,
      MyClan = 4,
      Members = 5,
      SearchClan = 6,
      ClanInfo = 7,
      ChangeAnnDec = 8,
      JoinAction = 9,
      RankAction = 10, // 0x0000000A
      Promote = 11, // 0x0000000B
      NickChange = 12, // 0x0000000C
      MarkChange = 14, // 0x0000000E
      DisbandClan = 16, // 0x00000010
      NewSearchClan = 17, // 0x00000011
    }

    internal enum SearchType
    {
      Name,
      Master,
    }
  }
}
