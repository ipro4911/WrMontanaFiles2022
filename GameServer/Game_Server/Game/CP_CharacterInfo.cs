// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CharacterInfo
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Game_Server.Game
{
  internal class CP_CharacterInfo : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (this.blocks.Length < 17 || this.blocks[18] != "dnjfhr^")
        usr.disconnect();
      int num1 = int.Parse(this.getBlock(1));
      if (num1 > 0)
      {
        int num2 = int.Parse(this.getBlock(0));
        string block1 = this.getBlock(3);
        string block2 = this.getBlock(4);
        if (block1.Length > 0 && block1.Length <= 16 && (num2 >= 0 && num1 >= 0))
        {
          DataTable dataTable1 = DB.RunReader("SELECT * FROM users WHERE username='" + DB.Stripslash(block1) + "' AND id='" + (object) num1 + "'");
          if (dataTable1.Rows.Count > 0)
          {
            try
            {
              DataRow row1 = dataTable1.Rows[0];
              int result1 = 0;
              int.TryParse(row1["ticketid"].ToString(), out result1);
              if (result1 == num2 && result1 != -1 || Game_Server.Configs.Server.Debug)
              {
                usr.userId = num1;
                usr.username = row1["username"].ToString();
                usr.nickname = row1["nickname"].ToString();
                usr.exp = int.Parse(row1["exp"].ToString());
                usr.dinar = Convert.ToInt32(row1["dinar"].ToString());
                usr.kills = int.Parse(row1["kills"].ToString());
                usr.deaths = int.Parse(row1["deaths"].ToString());
                usr.premium = byte.Parse(row1["premium"].ToString());
                uint.TryParse(row1["premiumExpire"].ToString(), out usr.premiumExpire);
                usr.cash = int.Parse(row1["cash"].ToString());
                usr.rank = int.Parse(row1["rank"].ToString());
                usr.coupons = int.Parse(row1["coupons"].ToString());
                usr.todaycoupons = int.Parse(row1["todaycoupon"].ToString());
                usr.clanId = int.Parse(row1["clanid"].ToString());
                usr.headshots = int.Parse(row1["headshots"].ToString());
                uint.TryParse(row1["mutedExpire"].ToString(), out usr.mutedexpire);
                usr.firstlogin = int.Parse(row1["firstlogin"].ToString());
                usr.country = row1["country"].ToString();
                usr.coupontime = int.Parse(row1["coupontime"].ToString());
                if (usr.premium == (byte) 0)
                  usr.storageInventoryMax = 8;
                else if (usr.premium == (byte) 1)
                  usr.storageInventoryMax = 12;
                else if (usr.premium == (byte) 2)
                  usr.storageInventoryMax = 16;
                else if (usr.premium == (byte) 3)
                  usr.storageInventoryMax = 20;
                else if (usr.premium == (byte) 4)
                  usr.storageInventoryMax = 24;
                usr.medalid = int.Parse(row1["medalid"].ToString());
                uint.TryParse(row1["donationexpire"].ToString(), out usr.donationexpire);
                int result2 = 0;
                int.TryParse(row1["randombox"].ToString(), out result2);
                usr.RandomBoxToday = result2 == 1;
                usr.rewardEvent.doneToday = int.Parse(row1["loginEventToday"].ToString()) == 1;
                int.TryParse(row1["loginEventProgress"].ToString(), out usr.rewardEvent.progress);
                int.TryParse(row1["killcount"].ToString(), out usr.eventcount);
                ushort.TryParse(row1["wonMatchs"].ToString(), out usr.wonMatchs);
                ushort.TryParse(row1["lostMatchs"].ToString(), out usr.lostMatchs);
                int result3 = 0;
                int.TryParse(row1["lastjoin"].ToString(), out result3);
                if (string.Compare(row1["retailcode"].ToString(), "null", true) != 0)
                {
                  usr.retail = row1["retailcode"].ToString();
                  int.TryParse(row1["retailclass"].ToString(), out usr.retailclass);
                }
                string color1 = row1["chat_color"].ToString();
                if (color1 != "" && color1.Length >= 6)
                  usr.chatColor = Game_Server.Generic.ConvertHexToRGB(color1);
                usr.ticketId = num2;
                if (usr.rank > 0)
                {
                  if (row1["Lastdaystats"].ToString() == DateTime.Now.ToString("dd-MM-yyyy"))
                    usr.dailystats = true;
                  if (usr.dinar < 0)
                    usr.dinar = 0;
                  if (usr.cash < 0)
                    usr.cash = 0;
                  if (usr.rank < 5)
                  {
                    Country country = Program.ipLookup.getCountry(usr.IP);
                    usr.country = country.getCode();
                  }
                  usr.clan = ClanManager.GetClan(usr.clanId);
                  if (usr.clan != null)
                  {
                    if (usr.clan.clanRank(usr) != 9)
                    {
                      if (usr.clan.ClanUsers.ContainsKey(usr.userId))
                      {
                        ClanUsers clanUser = usr.clan.ClanUsers[usr.userId];
                        if (usr.exp.ToString() != clanUser.EXP)
                          clanUser.EXP = usr.exp.ToString();
                        if (string.Compare(usr.nickname, block2, true) == 0)
                          clanUser.nickname = usr.nickname;
                      }
                      if (!usr.clan.Users.ContainsKey(usr.userId))
                        usr.clan.Users.TryAdd(usr.userId, usr);
                    }
                    else if (usr.clan.pendingUsers.ContainsKey(usr.userId))
                    {
                      ClanPendingUsers pendingUser = usr.clan.pendingUsers[usr.userId];
                      if (usr.exp.ToString() != pendingUser.EXP)
                        pendingUser.EXP = usr.exp.ToString();
                      if (string.Compare(usr.nickname, block2, true) == 0)
                        pendingUser.nickname = usr.nickname;
                    }
                  }
                  else if (usr.clanId != -1)
                    DB.RunQuery("UPDATE users SET clanid='-1', clanrank='0' WHERE id='" + usr.userId + "'");
                  usr.accesslevel = usr.rank > 2 ? 3 : 0;
                  for (int index1 = 0; index1 < 5; ++index1)
                  {
                    for (int index2 = 0; index2 < 8; ++index2)
                    {
                      if (index2 == 0)
                        usr.equipment[index1, index2] = "DA02";
                      else if (index2 == 1)
                        usr.equipment[index1, index2] = "DB01";
                      else if (index2 == 2)
                      {
                        switch (index1)
                        {
                          case 0:
                          case 1:
                            usr.equipment[index1, index2] = "DF01";
                            continue;
                          case 2:
                            usr.equipment[index1, index2] = "DG05";
                            continue;
                          case 3:
                            usr.equipment[index1, index2] = "DC02";
                            continue;
                          case 4:
                            usr.equipment[index1, index2] = "DJ01";
                            continue;
                          default:
                            continue;
                        }
                      }
                      else if (index2 == 3)
                      {
                        switch (index1)
                        {
                          case 0:
                            usr.equipment[index1, index2] = "DR01";
                            continue;
                          case 1:
                            usr.equipment[index1, index2] = "DQ01";
                            continue;
                          case 2:
                          case 3:
                            usr.equipment[index1, index2] = "DN01";
                            continue;
                          case 4:
                            usr.equipment[index1, index2] = "DL01";
                            continue;
                          default:
                            continue;
                        }
                      }
                      else
                        usr.equipment[index1, index2] = "^";
                    }
                  }
                  int warRockDateTime = Game_Server.Generic.WarRockDateTime;
                  DataTable dataTable2 = DB.RunReader("SELECT class0, class1, class2, class3, class4, inventory, storage FROM equipment WHERE ownerid='" + (object) usr.userId + "'");
                  if (dataTable2.Rows.Count > 0)
                  {
                    DataRow row2 = dataTable2.Rows[0];
                    usr.inventory = row2["inventory"].ToString().Split(',');
                    usr.storageInventory = row2["storage"].ToString().Split(',');
                    for (int index = 0; index < usr.inventory.Length; ++index)
                    {
                      string str1 = usr.inventory[index];
                      string str2 = str1.Split('-')[0];
                      usr.inventory[index] = "^";
                      if (str1 != "^" && str2.Length == 4)
                        usr.inventory[index] = str1;
                    }
                    bool flag1 = false;
                    for (int index = 0; index < usr.inventory.Length; ++index)
                    {
                      if (usr.inventory[index] != "^")
                      {
                        try
                        {
                          string[] strArray = usr.inventory[index].Split('-');
                          string str = strArray[0];
                          if (str.Length == 4)
                          {
                            int result4 = 0;
                            int.TryParse(strArray[3], out result4);
                            if (result4 < warRockDateTime)
                            {
                              usr.expiredItems.Add(str);
                              usr.inventory[index] = "^";
                              flag1 = true;
                            }
                          }
                        }
                        catch
                        {
                        }
                      }
                    }
                    if (flag1)
                      DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
                    string[] strArray1 = new string[5]
                    {
                      row2["class0"].ToString(),
                      row2["class1"].ToString(),
                      row2["class2"].ToString(),
                      row2["class3"].ToString(),
                      row2["class4"].ToString()
                    };
                    bool flag2 = false;
                    string[] strArray2 = usr.AvailableSlots.Split(',');
                    for (int branch = 0; branch < 5; ++branch)
                    {
                      string[] strArray3 = strArray1[branch].Split(',');
                      for (int slot = 0; slot < 8; ++slot)
                      {
                        string str1 = strArray3[slot];
                        string str2 = strArray3[slot];
                        try
                        {
                          Item obj = (Item) null;
                          if (!str2.Contains("-"))
                          {
                            str2 = str2.StartsWith("I") ? usr.GetItemByID(str2) : str2;
                            obj = ItemManager.GetItem(str2);
                          }
                          bool flag3 = false;
                          if (str1 != "^")
                          {
                            if (str1.StartsWith("I") && str1.Contains("-"))
                            {
                              string[] strArray4 = str1.Split('-');
                              if (usr.HasItem(strArray4[0]) && usr.HasItem(strArray4[1]))
                                flag3 = true;
                            }
                            bool flag4 = true;
                            bool flag5 = false;
                            if (!str1.Contains("-"))
                              flag5 = usr.HasItem(str1);
                            if (usr.IsWhitelistedWeapon(str1) || flag5 || flag3)
                            {
                              usr.equipment[branch, slot] = str1;
                              flag4 = false;
                            }
                            if (slot >= 4)
                            {
                              int index = slot - 4;
                              flag4 = !(strArray2[index] == "T") || !(str1 != usr.retail);
                            }
                            if (obj != null)
                              flag4 = !obj.UseableSlot(slot) || !obj.UseableBranch(branch) && branch != 7;
                            if (flag4)
                            {
                              usr.equipment[branch, slot] = "^";
                              flag2 = true;
                            }
                          }
                          else
                            usr.equipment[branch, slot] = "^";
                        }
                        catch (Exception ex)
                        {
                          Log.WriteError("Error loading: " + str2);
                        }
                      }
                    }
                    if (flag2)
                      usr.SaveEquipment();
                  }
                  DataTable dataTable3 = DB.RunReader("SELECT class_0, class_1, class_2, class_3, class_4, inventory FROM users_costumes WHERE ownerid='" + (object) usr.userId + "'");
                  if (dataTable3.Rows.Count > 0)
                  {
                    DataRow row2 = dataTable3.Rows[0];
                    string[] strArray1 = new string[5]
                    {
                      row2["class_0"].ToString(),
                      row2["class_1"].ToString(),
                      row2["class_2"].ToString(),
                      row2["class_3"].ToString(),
                      row2["class_4"].ToString()
                    };
                    for (int index = 0; index < 5; ++index)
                      usr.costumes_char[index] = strArray1[index];
                    usr.costume = row2["inventory"].ToString().Split(',');
                    bool flag = false;
                    for (int index = 0; index < usr.costume.Length; ++index)
                    {
                      if (usr.costume[index] != "^")
                      {
                        try
                        {
                          string[] strArray2 = usr.costume[index].Split('-');
                          string str = strArray2[0];
                          if (str.Length == 4)
                          {
                            int result4 = 0;
                            int.TryParse(strArray2[3], out result4);
                            if (result4 < warRockDateTime)
                            {
                              usr.expiredItems.Add(str);
                              usr.costume[index] = "^";
                              flag = true;
                            }
                          }
                        }
                        catch
                        {
                        }
                      }
                    }
                    if (flag)
                      DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
                  }
                  else
                    DB.RunQuery("INSERT INTO users_costumes (ownerid) VALUES ('" + (object) usr.userId + "')");
                  int length1 = usr.inventory.Length;
                  int length2 = usr.costume.Length;
                  int length3 = usr.storageInventory.Length;
                  if (length1 < Game_Server.Configs.Server.Player.MaxInventorySlot)
                  {
                    Array.Resize<string>(ref usr.inventory, Game_Server.Configs.Server.Player.MaxInventorySlot);
                    for (int index = length1; index < Game_Server.Configs.Server.Player.MaxInventorySlot; ++index)
                      usr.inventory[index] = "^";
                    DB.RunQuery("UPDATE equipment SET inventory='" + Inventory.Itemlist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
                  }
                  if (length2 < Game_Server.Configs.Server.Player.MaxCostumeSlot)
                  {
                    Array.Resize<string>(ref usr.costume, Game_Server.Configs.Server.Player.MaxCostumeSlot);
                    for (int index = length2; index < Game_Server.Configs.Server.Player.MaxCostumeSlot; ++index)
                      usr.costume[index] = "^";
                    DB.RunQuery("UPDATE users_costumes SET inventory='" + Inventory.Costumelist(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
                  }
                  if (length3 < usr.storageInventoryMax)
                  {
                    Array.Resize<string>(ref usr.storageInventory, usr.storageInventoryMax);
                    for (int index = length3; index < usr.storageInventoryMax; ++index)
                      usr.storageInventory[index] = "^";
                    DB.RunQuery("UPDATE equipment SET stoarge='" + Inventory.Storage(usr) + "' WHERE ownerid='" + (object) usr.userId + "'");
                  }
                  usr.CheckForCostume();
                  usr.LoadInboxItems();
                  usr.LoadRetails();
                  if (UserManager.addUser(usr))
                  {
                    int id = 1;
                    if (!usr.CheckForEvent(id) && Game_Server.Configs.Server.LoginEvent.enabled && Inventory.GetFreeItemSlotCount(usr) > 0)
                    {
                      string[] items = Game_Server.Configs.Server.LoginEvent.items;
                      int index = Game_Server.Generic.random(0, items.Length - 1);
                      string Code = items[index];
                      if (ItemManager.GetItem(Code) != null)
                      {
                        int days = new Random().Next(Game_Server.Configs.Server.LoginEvent.MinDays, Game_Server.Configs.Server.LoginEvent.MaxDays);
                        Inventory.AddItem(usr, Code, days);
                        usr.AddEvent(id, false);
                        usr.send((Packet) new SP_RandomBoxEvent(usr, Code));
                      }
                      else
                        Log.WriteError(Code + " is not a valid item @ log in event!");
                    }
                    usr.send((Packet) new SP_CharacterInfo(usr));
                    UserManager.UpdateUserlist(usr);
                    if (usr.expiredItems.Count > 0)
                    {
                      int num3 = (int) Math.Ceiling((Decimal) usr.expiredItems.Count / new Decimal(30));
                      for (int index = 0; index < num3; ++index)
                        usr.send((Packet) new SP_UpdateInventory(usr, usr.expiredItems.Skip<string>(index * 30).Take<string>(30).ToList<string>()));
                      usr.expiredItems.Clear();
                    }
                    if (usr.expiredCostumes.Count > 0)
                    {
                      int num3 = (int) Math.Ceiling((Decimal) usr.expiredCostumes.Count / new Decimal(30));
                      for (int index = 0; index < num3; ++index)
                        usr.send((Packet) new SP_UpdateInventory(usr, usr.expiredCostumes.Skip<string>(index * 30).Take<string>(30).ToList<string>()));
                      usr.expiredCostumes.Clear();
                    }
                    usr.PremiumTimeLeft();
                    usr.PingTime = DateTime.Now;
                    usr.send((Packet) new SP_PingInformation(usr));
                    usr.LoadOutboxItems();
                    usr.send((Packet) new SP_StorageInventoryList(usr));
                    usr.send((Packet) new SP_MyRank(usr));
                    usr.LoadFriends();
                    if (usr.rank == 2)
                    {
                      DateTime now = DateTime.Now;
                      TimeSpan timeSpan = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) usr.donationexpire) - now;
                      Color color2 = Color.FromArgb(15, 192, 252);
                      usr.send((Packet) new SP_ColoredChat(Game_Server.Configs.Server.SystemName + " >> Your donator status will expire in " + (object) ((int) Math.Round(timeSpan.TotalDays) - 1) + " days", SP_ColoredChat.ChatType.Normal, color2));
                    }
                    if (usr.firstlogin == 2)
                    {
                      usr.CheckForFirstLogin();
                      usr.send((Packet) new SP_UpdateInventory(usr, (List<string>) null));
                    }
                    else if (result3 <= Game_Server.Generic.timestamp - 5184000)
                    {
                      usr.ComeBackReward();
                      usr.send((Packet) new SP_UpdateInventory(usr, (List<string>) null));
                    }
                    foreach (TempItem inBoxItem in usr.InBoxItems)
                    {
                      string str = "day(s)";
                      if (inBoxItem.days >= (ushort) 3600)
                        str = "One use / Permanent";
                      usr.send((Packet) new SP_Chat("SYSTEM", SP_Chat.ChatType.Room_ToAll, "SYSTEM >> You've got " + inBoxItem.name + " for " + (object) inBoxItem.days + " " + str + "!", 998U, "NULL"));
                    }
                    usr.sessionStart = Game_Server.Generic.timestamp;
                    usr.send((Packet) new SP_AntiCheat(usr));
                    usr.AntiCheatTick = (uint) (Game_Server.Generic.timestamp + Game_Server.Configs.Server.AntiCheat.routinetick);
                  }
                  else
                  {
                    Log.WriteError(usr.nickname + " > logged in but couldn't be added to the stuck");
                    usr.disconnect();
                  }
                }
                else
                {
                  Log.WriteError(usr.nickname + " > logged in as banned user");
                  usr.disconnect();
                }
              }
              else
                Log.WriteError(block1 + " tried to login with wrong ticket id!");
            }
            catch
            {
              Log.WriteError("Error parsing user information for user " + block1);
            }
          }
          else
          {
            Log.WriteError("No user data found for user " + block1);
            usr.disconnect();
          }
        }
        else
        {
          Log.WriteError(block1 + " -> error with " + (block1.Length == 0 ? " username length" : " Ticket ID"));
          usr.disconnect();
        }
        DB.RunQuery("UPDATE users SET ticketid='-1' WHERE id='" + (object) usr.userId + "'");
      }
      else
      {
        Log.WriteError(usr.nickname + " > logged in - invalid request");
        usr.disconnect();
      }
    }
  }
}
