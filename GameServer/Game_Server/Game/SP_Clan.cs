// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_Clan
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
  internal class SP_Clan : Packet
  {
    public SP_Clan(SP_Clan.ClanCodes type)
    {
      this.newPacket((ushort) 26384);
      this.addBlock((object) (int) type);
      this.addBlock((object) 1);
    }

    public enum ClanCodes
    {
      CreateClan = 1,
      ApplyClan = 2,
      LeaveClan = 3,
      Open = 4,
      MemberList = 5,
      SearchClan = 6,
      DisbandClan = 16, // 0x00000010
    }

    internal class SearchClan : Packet
    {
      public SearchClan(int page, int sortType, List<Game_Server.Clan> list)
      {
        int num = list.Count;
        if (num > 10)
          num = 10;
        this.newPacket((ushort) 26384);
        this.addBlock((object) 17);
        this.addBlock((object) 1);
        this.addBlock((object) page);
        this.addBlock((object) sortType);
        this.addBlock((object) num);
        for (int index = 10 * page; index < num; ++index)
        {
          Game_Server.Clan clan = list[index];
          if (clan != null)
          {
            this.addBlock((object) clan.id);
            this.addBlock((object) clan.Master);
            this.addBlock((object) clan.MasterEXP);
            this.addBlock((object) clan.name);
            this.addBlock((object) clan.GetRank());
            this.addBlock((object) clan.ClanUsers.Count);
            this.addBlock((object) clan.iconid);
            this.addBlock((object) (10 * page + (index + 1)));
            this.addBlock((object) clan.Description.Replace(' ', '\x001D'));
            this.addBlock((object) clan.GetCreationDate());
          }
        }
      }

      public SearchClan(List<Game_Server.Clan> list)
      {
        int count = list.Count;
        this.newPacket((ushort) 26384);
        this.addBlock((object) 6);
        this.addBlock((object) 1);
        this.addBlock((object) count);
        for (int index = 0; index < count; ++index)
        {
          Game_Server.Clan clan = list[index];
          if (clan != null)
          {
            this.addBlock((object) clan.id);
            this.addBlock((object) clan.Master);
            this.addBlock((object) clan.MasterEXP);
            this.addBlock((object) clan.name);
            this.addBlock((object) clan.GetRank());
            this.addBlock((object) clan.ClanUsers.Count);
            this.addBlock((object) clan.iconid);
            this.addBlock((object) (index + 1));
            this.addBlock((object) clan.Description.Replace(' ', '\x001D'));
            this.addBlock((object) clan.GetCreationDate());
          }
        }
      }
    }

    internal class CheckClan : Packet
    {
      public CheckClan(SP_Clan.CheckClan.ErrorCodes err)
      {
        this.newPacket((ushort) 26384);
        this.addBlock((object) 0);
        this.addBlock((object) (int) err);
      }

      internal enum ErrorCodes
      {
        NotExist = 1,
        Exist = 62003, // 0x0000F233
        NotFound = 63001, // 0x0000F619
      }
    }

    internal class Change : Packet
    {
      public Change()
      {
        this.newPacket((ushort) 26384);
        this.addBlock((object) 11);
        this.addBlock((object) 1);
      }

      public Change(int SubType, int UserID)
      {
        this.newPacket((ushort) 26384);
        this.addBlock((object) 10);
        this.addBlock((object) 1);
        this.addBlock((object) SubType);
        this.addBlock((object) UserID);
      }

      public Change(Game_Server.User usr, bool isNick)
      {
        this.newPacket((ushort) 26384);
        this.addBlock((object) (isNick ? 12 : 14));
        this.addBlock((object) 1);
        this.addBlock((object) Inventory.Itemlist(usr));
      }
    }

    internal class CreateClan : Packet
    {
      public CreateClan(string name, int clanId, uint dinar)
      {
        this.newPacket((ushort) 26384);
        this.addBlock((object) SP_Clan.ClanCodes.CreateClan);
        this.addBlock((object) 1);
        this.addBlock((object) clanId);
        this.addBlock((object) 2);
        this.addBlock((object) name);
        this.addBlock((object) dinar);
      }
    }

    internal class MyClanInformation : Packet
    {
      public MyClanInformation(Game_Server.User usr)
      {
        Game_Server.Clan clan = usr.clan;
        int num = clan.maxUsers / 20 - 1;
        int clanRank = ClanManager.GetClanRank(clan.id);
        int count = clan.ClanWars.Count;
        this.newPacket((ushort) 26384);
        this.addBlock((object) 4);
        this.addBlock((object) 1);
        this.addBlock((object) clan.clanRank(usr));
        this.addBlock((object) clan.name);
        this.addBlock((object) clan.Master);
        this.addBlock((object) clan.MasterEXP);
        this.addBlock((object) num);
        this.addBlock((object) clan.ClanUsers.Count);
        this.addBlock((object) (clan.pendingUsers.Count > 0 ? 1 : 0));
        this.addBlock((object) clan.win);
        this.addBlock((object) clan.lose);
        this.addBlock((object) clan.exp);
        this.addBlock((object) clanRank);
        this.addBlock((object) 0);
        this.addBlock((object) clan.Description.Replace(' ', '\x001D'));
        this.addBlock((object) clan.Announcment.Replace(' ', '\x001D'));
        this.addBlock((object) clan.iconid);
        this.addBlock((object) count);
        foreach (ClanWar clanWar in clan.ClanWars.Values.Where<ClanWar>((Func<ClanWar, bool>) (u => u != null)).Take<ClanWar>(count > 3 ? 3 : count))
        {
          this.addBlock((object) clanWar.versusClan);
          this.addBlock((object) clanWar.score);
          this.addBlock((object) (clanWar.won ? 1 : 0));
        }
      }
    }

    internal class UserList
    {
      internal class NormalUser : Packet
      {
        public NormalUser(Game_Server.User usr, Game_Server.Clan Clan)
        {
          int num = Clan.maxUsers / 20 - 1;
          this.newPacket((ushort) 26384);
          this.addBlock((object) 7);
          this.addBlock((object) 1);
          this.addBlock((object) Clan.id);
          this.addBlock((object) Clan.name);
          this.addBlock((object) Clan.Master);
          this.addBlock((object) Clan.MasterEXP);
          this.addBlock((object) num);
          this.addBlock((object) Clan.ClanUsers.Count);
          this.addBlock((object) Clan.Description.Replace(' ', '\x001D'));
          this.addBlock((object) Clan.iconid);
        }

        public NormalUser(Game_Server.User User, List<Game_Server.Clan> list)
        {
          this.newPacket((ushort) 26384);
          this.addBlock((object) 6);
          this.addBlock((object) 1);
          this.addBlock((object) list.Count);
          foreach (Game_Server.Clan clan in list)
          {
            int num = clan.maxUsers / 20 - 1;
            this.addBlock((object) clan.id);
            this.addBlock((object) clan.Master);
            this.addBlock((object) clan.MasterEXP);
            this.addBlock((object) clan.name);
            this.addBlock((object) num);
            this.addBlock((object) clan.ClanUsers.Count);
            this.addBlock((object) clan.iconid);
          }
        }

        public NormalUser(Game_Server.Clan c)
        {
          this.newPacket((ushort) 26384);
          this.addBlock((object) 5);
          this.addBlock((object) 1);
          this.addBlock((object) c.ClanUsers.Count);
          foreach (ClanUsers allUser in c.getAllUsers())
          {
            this.addBlock((object) allUser.id);
            this.addBlock((object) allUser.clanrank);
            this.addBlock((object) allUser.EXP);
            this.addBlock((object) allUser.nickname);
            this.addBlock((object) allUser.ClanJoinDate);
            this.addBlock((object) allUser.ClanJoinDate);
            this.addBlock((object) 36);
          }
        }
      }

      internal class Pending : Packet
      {
        public Pending()
        {
          this.newPacket((ushort) 26384);
          this.addBlock((object) 3);
          this.addBlock((object) 1);
        }

        public Pending(int Subtype, int ClanID)
        {
          this.newPacket((ushort) 26384);
          this.addBlock((object) 9);
          this.addBlock((object) 1);
          this.addBlock((object) Subtype);
          this.addBlock((object) ClanID);
        }

        public Pending(Game_Server.User usr)
        {
          List<ClanPendingUsers> clanPendingUsersList = new List<ClanPendingUsers>((IEnumerable<ClanPendingUsers>) usr.clan.pendingUsers.Values);
          this.newPacket((ushort) 26384);
          this.addBlock((object) 5);
          this.addBlock((object) 1);
          this.addBlock((object) clanPendingUsersList.Count);
          foreach (ClanPendingUsers clanPendingUsers in clanPendingUsersList)
          {
            this.addBlock((object) clanPendingUsers.id);
            this.addBlock((object) 9);
            this.addBlock((object) clanPendingUsers.EXP);
            this.addBlock((object) clanPendingUsers.nickname);
            this.addBlock((object) clanPendingUsers.ClanJoinDate);
            this.addBlock((object) clanPendingUsers.ClanJoinDate);
            this.addBlock((object) 36);
          }
        }
      }
    }
  }
}
