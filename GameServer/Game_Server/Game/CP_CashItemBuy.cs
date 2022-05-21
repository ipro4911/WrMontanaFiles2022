// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_CashItemBuy
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8B492E61-2D86-4BEC-B3F6-D0D746B9F821
// Assembly location: C:\Users\updates-acc\Desktop\Nuova cartella (2)\WrMontana Public2\WrMontana Public\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game_Server.Game
{
    internal class CP_CashItemBuy : Handler
    {
        private string itemcode;

        public override void Handle(Game_Server.User usr)
        {
            if (usr.room != null)
                return;
            switch ((CP_CashItemBuy.SubCodes)int.Parse(this.getBlock(0)))
            {
                case CP_CashItemBuy.SubCodes.OnItemBuy:
                    string upper = this.getBlock(6).ToUpper();
                    int num1 = int.Parse(this.getBlock(3));
                    ushort Days = (ushort)Inventory.GetDaysFromPeriod(num1);
                    Item obj1 = ItemManager.GetItem(upper);
                    if (obj1 == null)
                        break;
                    if (Days > (ushort)0)
                    {
                        if (obj1.Buyable || ((IEnumerable<string>)Game_Server.Configs.Server.ItemShop.hiddenItems).Contains<string>(upper))
                        {
                            if (Inventory.GetFreeItemSlotCount(usr) > 0)
                            {
                                uint cashPrice = (uint)obj1.GetCashPrice(num1);
                                usr.RefreshCash();
                                usr.LoadOutboxItems();
                                usr.RefreshDinars();
                                if (cashPrice > 0U)
                                {
                                    bool flag = upper.ToLower().StartsWith("cz") || upper.ToLower().StartsWith("cb");
                                    int num2 = (int)((long)usr.cash - (long)cashPrice);
                                    if (obj1.Premium && usr.premium < (byte)1)
                                    {
                                        usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.PremiumUsersOnly, new object[0]));
                                        break;
                                    }
                                    if (obj1.accruable && Inventory.GetEAItem(usr, obj1.Code) >= (int)obj1.maxAccrueCount)
                                    {
                                        usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[0]));
                                        break;
                                    }
                                    if ((int)usr.level < obj1.Level)
                                        usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.LevelLow, new object[0]));
                                    if (num2 < 0)
                                    {
                                        usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NotEnoughDinar, new object[0]));
                                        break;
                                    }
                                    if (flag)
                                        Days = (ushort)3600;
                                    ushort count = 1;
                                    if (obj1.accruable)
                                    {
                                        ushort eaCount = (ushort)obj1.GetEACount(num1);
                                        if (eaCount >= (ushort)1)
                                            count = eaCount;
                                    }
                                    if (upper == "CB53")
                                    {
                                        if (usr.clan == null || usr.clan.maxUsers >= 100)
                                        {
                                            usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[0]));
                                            break;
                                        }
                                        usr.clan.maxUsers += 20;
                                        usr.send((Packet)new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Clan, Game_Server.Configs.Server.SystemName + " >> Clan expanded, please re-open clan tab to see changes.", 999U, Game_Server.Configs.Server.SystemName));
                                        usr.send((Packet)new SP_DinarItemBuy(usr, upper));
                                        usr.send((Packet)new SP_Clan.MyClanInformation(usr));
                                        DB.RunQuery("UPDATE clans SET maxusers='" + usr.clan.maxUsers.ToString() + "' WHERE id='" + usr.clan.id.ToString() + "'");
                                        break;
                                    }
                                    if ((upper.StartsWith("CZ") || upper.StartsWith("CC") || (upper.StartsWith("CR") || upper.StartsWith("CB"))) && (!upper.StartsWith("CC0") && upper != "CC38"))
                                        Days = (ushort)3600;
                                    Inventory.AddOutBoxItem(usr, upper, Days, count);
                                    usr.cash = num2;
                                    DB.RunQuery("UPDATE users SET cash=" + num2.ToString() + " WHERE id='" + usr.userId.ToString() + "'");
                                    usr.send((Packet)new SP_CashItemBuy(usr));
                                    usr.send((Packet)new SP_OutboxSend(usr));
                                    break;
                                }
                                usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.CannotBeBougth, new object[0]));
                                break;
                            }
                            usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.InventoryFull, new object[0]));
                            break;
                        }
                        usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid, new object[0]));
                        break;
                    }
                    usr.send((Packet)new SP_DinarItemBuy(SP_DinarItemBuy.ErrorCodes.NoLongerValid, new object[0]));
                    break;
                case CP_CashItemBuy.SubCodes.OnItemUse:
                    string block1 = this.getBlock(4);
                    if (!usr.HasItem(block1))
                        break;
                    if (PackageManager.AddItem(usr, block1))
                    {
                        usr.send((Packet)new SP_UseItem(usr, block1));
                        break;
                    }
                    string str1 = block1;
                    int num3;
                    switch (str1)
                    {
                        case "CB01":
                            string block2 = this.getBlock(5);
                            if (!Game_Server.Generic.IsAlphaNumeric(block2) || (uint)DB.RunReader("SELECT * FROM users WHERE nickname='" + block2 + "'").Rows.Count > 0U)
                                return;
                            DB.RunQuery("INSERT INTO changenick_logs (userId, oldnick, newnick, date, timestamp) VALUES ('" + usr.userId.ToString() + "', '" + usr.nickname + "', '" + block2 + "', '" + Game_Server.Generic.currentDate + "', '" + Game_Server.Generic.timestamp.ToString() + "')");
                            Log.WriteLine("---" + usr.nickname + " is now known as " + block2 + "---");
                            usr.nickname = block2;
                            DB.RunQuery("UPDATE users SET nickname='" + usr.nickname + "' WHERE id='" + usr.userId.ToString() + "'");
                            usr.deleteItem(block1);
                            usr.send((Packet)new SP_CashItemUse(usr, block1));
                            if (usr.clan.clanRank(usr) != 9)
                            {
                                ClanUsers user = usr.clan.GetUser(usr.userId);
                                if (user == null)
                                    return;
                                user.EXP = usr.exp.ToString();
                                user.nickname = usr.nickname;
                                return;
                            }
                            ClanPendingUsers pendingUser = usr.clan.getPendingUser(usr.userId);
                            if (pendingUser == null)
                                return;
                            pendingUser.EXP = usr.exp.ToString();
                            pendingUser.nickname = usr.nickname;
                            return;
                        case "CB03":
                            usr.kills = usr.deaths = 0;
                            DB.RunQuery("UPDATE users SET kills = '" + usr.kills.ToString() + "', deaths = '" + usr.deaths.ToString() + "' WHERE id='" + usr.userId.ToString() + "'");
                            usr.deleteItem(block1);
                            usr.send((Packet)new SP_CashItemUse(usr, block1));
                            return;
                        case "CB09":
                            if (!usr.HasItem("CB08"))
                            {
                                usr.send((Packet)new SP_CashItemUse(SP_CashItemUse.ErrCode.NeedSupplyBox, usr, "CB08"));
                                return;
                            }
                            int num4 = Game_Server.Generic.random(0, 5);
                            string itemcode1 = (string)null;
                            switch (num4)
                            {
                                case 0:
                                    itemcode1 = "CZ85";
                                    break;
                                case 1:
                                    itemcode1 = "DU51";
                                    break;
                                case 2:
                                    itemcode1 = "DC85";
                                    break;
                                case 3:
                                    itemcode1 = "DF12";
                                    break;
                                case 4:
                                    itemcode1 = "DE18";
                                    break;
                                case 5:
                                    itemcode1 = "DE28";
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode1, 7);
                            Inventory.DecreaseEAItem(usr, "CB09");
                            Inventory.DecreaseEAItem(usr, "CB08");
                            usr.send((Packet)new SP_WinItem(usr, itemcode1, 7));
                            return;
                        case "CB90":
                            int action1 = int.Parse(this.getBlock(1));
                            int index11 = int.Parse(this.getBlock(2));
                            string block4 = this.getBlock(3);
                            int num5 = usr.storageInventoryMax + 4;
                            if (num5 >= 80)
                            {
                                usr.send((Packet)new SP_CustomMessageBox("Fehler du lappen"));
                                return;
                            }
                            usr.MaxSlots = num5;
                            usr.PlusInvSlots += 5;
                            DB.RunQuery("UPDATE users SET storageInventory='" + usr.PlusInvSlots.ToString() + "' WHERE id='" + usr.userId.ToString() + "'");
                            Inventory.DecreaseEAItem(usr, "CB90");
                            usr.send((Packet)new SP_CashItemUse(usr, this.itemcode));
                            usr.SaveEquipment();
                            usr.send((Packet)new SP_StorageInventoryUpdate(usr, action1, index11, block4));
                            usr.send((Packet)new SP_UpdateInventory(usr, (List<string>)null));
                            //  usr.send((Packet)new SP_StorageInventoryList(usr, Inventory.Storage));
                            return;
                        case "CB91":
                            int action2 = int.Parse(this.getBlock(1));
                            int index12 = int.Parse(this.getBlock(2));
                            string block5 = this.getBlock(3);
                            int num6 = usr.storageInventoryMax + 8;
                            if (num6 >= 80)
                            {
                                usr.send((Packet)new SP_CustomMessageBox("Fehler du lappen"));
                                return;
                            }
                            usr.MaxSlots = num6;
                            usr.PlusInvSlots += 8;
                            DB.RunQuery("UPDATE users SET storageInventory='" + usr.PlusInvSlots.ToString() + "' WHERE id='" + usr.userId.ToString() + "'");
                            Inventory.DecreaseEAItem(usr, "CB90");
                            usr.send((Packet)new SP_CashItemUse(usr, this.itemcode));
                            usr.SaveEquipment();
                            usr.send((Packet)new SP_StorageInventoryUpdate(usr, action2, index12, block5));
                            usr.send((Packet)new SP_UpdateInventory(usr, (List<string>)null));
                            //  usr.send((Packet)new SP_StorageInventoryList(usr, Inventory.Storage));
                            return;
                        case "CC36":
                        case "CC37":
                        case "CC56":
                        case "CC57":
                            Item obj2 = ItemManager.GetItem(block1);
                            if (obj2 == null)
                                return;
                            List<PackageItem> packageItems = obj2.packageItems;
                            if (packageItems.Count <= 0 || Inventory.GetFreeItemSlotCount(usr) <= 0)
                                return;
                            int index1 = Game_Server.Generic.random(0, packageItems.Count - 1);
                            if (index1 == 0 && Game_Server.Generic.random(100, 1000) > 200)
                                index1 = Game_Server.Generic.random(1, packageItems.Count - 1);
                            PackageItem packageItem = packageItems[index1];
                            if (ItemManager.GetItem(packageItem.item) != null)
                            {
                                int days = (int)packageItem.days;
                                Inventory.AddItem(usr, packageItem.item, days);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, packageItem.item, days));
                                return;
                            }
                            Log.WriteError(packageItem.ToString() + " is not a valid item @ random box!");
                            return;
                        case "CR01":
                            int num7 = Game_Server.Generic.random(0, 5);
                            int days1 = 1;
                            string itemcode2 = (string)null;
                            switch (num7)
                            {
                                case 0:
                                    itemcode2 = "D706";
                                    days1 = -1;
                                    break;
                                case 1:
                                    itemcode2 = "D706";
                                    days1 = 15;
                                    break;
                                case 2:
                                    itemcode2 = "D706";
                                    days1 = 30;
                                    break;
                                case 3:
                                    itemcode2 = "CF01";
                                    days1 = 15;
                                    break;
                                case 4:
                                    itemcode2 = "CI01";
                                    days1 = 15;
                                    break;
                                case 5:
                                    itemcode2 = "DS03";
                                    days1 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode2, days1);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode2, days1));
                            return;
                        case "CR02":
                            int num8 = Game_Server.Generic.random(0, 5);
                            int num9 = 1;
                            string itemcode3 = (string)null;
                           // int num9;
                            switch (num8)
                            {
                                case 0:
                                    itemcode3 = "DT34";
                                    num9 = 90;
                                    break;
                                case 1:
                                    itemcode3 = "DT34";
                                    num9 = 30;
                                    break;
                                case 2:
                                    itemcode3 = "DT34";
                                    num9 = 15;
                                    break;
                                case 3:
                                    itemcode3 = "CF01";
                                    num9 = 15;
                                    break;
                                case 4:
                                    itemcode3 = "CI01";
                                    num9 = 15;
                                    break;
                                case 5:
                                    itemcode3 = "DS03";
                                    num9 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode3, num9);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode3, num9));
                            return;
                        case "CR03":
                            int num10 = Game_Server.Generic.random(0, 5);
                            int days3 = 1;
                            string itemcode4 = (string)null;
                            switch (num10)
                            {
                                case 0:
                                    itemcode4 = "D817";
                                    days3 = -1;
                                    break;
                                case 1:
                                    itemcode4 = "D817";
                                    days3 = 15;
                                    break;
                                case 2:
                                    itemcode4 = "D817";
                                    days3 = 30;
                                    break;
                                case 3:
                                    itemcode4 = "CF01";
                                    days3 = 15;
                                    break;
                                case 4:
                                    itemcode4 = "CI01";
                                    days3 = 15;
                                    break;
                                case 5:
                                    itemcode4 = "DS03";
                                    days3 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode4, days3);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode4, days3));
                            return;
                        case "CR04":
                            int num11 = Game_Server.Generic.random(0, 3);
                            int days4 = 1;
                            string itemcode5 = (string)null;
                            switch (num11)
                            {
                                case 0:
                                    itemcode5 = "DS21";
                                    days4 = -1;
                                    break;
                                case 1:
                                    itemcode5 = "DS23";
                                    days4 = -1;
                                    break;
                                case 2:
                                    itemcode5 = "DS25";
                                    days4 = -1;
                                    break;
                                case 3:
                                    itemcode5 = "DS27";
                                    days4 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode5, days4);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode5, days4));
                            return;
                        case "CY42":
                            int num711 = Game_Server.Generic.random(0, 24);
                            int days420 = 1;
                            string itemcode7201 = (string)null;
                            switch (num711)
                            {
                                case 0:
                                    itemcode7201 = "GF16";
                                    days420 = 365;
                                    break;
                                case 1:
                                    itemcode7201 = "GF16";
                                    days420 =  90;
                                    break;
                                case 2:
                                    itemcode7201 = "GF16";
                                    days420 = 60;
                                    break;
                                case 3:
                                    itemcode7201 = "GF16";
                                    days420 = 30;
                                    break;
                                case 4:
                                    itemcode7201 = "GF16";
                                    days420 = 15;
                                    break;
                                case 5:
                                    itemcode7201 = "GG04";
                                    days420 = 365;
                                    break;
                                case 6:
                                    itemcode7201 = "GG04";
                                    days420 = 90;
                                    break;
                                case 7:
                                    itemcode7201 = "GG04";
                                    days420 = 60;
                                    break;
                                case 8:
                                    itemcode7201 = "GG04";
                                    days420 = 30;
                                    break;
                                case 9:
                                    itemcode7201 = "GG04";
                                    days420 = 15;
                                    break;
                                case 10:
                                    itemcode7201 = "DE87";
                                    days420 = 365;
                                    break;
                                case 11:
                                    itemcode7201 = "DE87";
                                    days420 = 90;
                                    break;
                                case 12:
                                    itemcode7201 = "DE87";
                                    days420 = 60;
                                    break;
                                case 13:
                                    itemcode7201 = "DE87";
                                    days420 = 30;
                                    break;
                                case 14:
                                    itemcode7201 = "DE87";
                                    days420 = 15;
                                    break;
                                case 15:
                                    itemcode7201 = "DJ59";
                                    days420 = 365;
                                    break;
                                case 16:
                                    itemcode7201 = "DJ59";
                                    days420 = 90;
                                    break;
                                case 17:
                                    itemcode7201 = "DJ59";
                                    days420 = 60;
                                    break;
                                case 18:
                                    itemcode7201 = "DJ59";
                                    days420 = 30;
                                    break;
                                case 19:
                                    itemcode7201 = "DJ59";
                                    days420 = 15;
                                    break;
                                case 20:
                                    itemcode7201 = "CZ73";
                                    days420 = 1;
                                    break;
                                case 21:
                                    itemcode7201 = "DS01";
                                    days420 = 15;
                                    break;
                                case 22:
                                    itemcode7201 = "CI01";
                                    days420 = 15;
                                    break;
                                case 23:
                                    itemcode7201 = "DS10";
                                    days420 = 15;
                                    break;
                                case 24:
                                    itemcode7201 = "CB09";
                                    days420 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode7201, days420);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode7201, days420));
                            return;
                        case "CR05":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] items1 = Game_Server.Configs.Server.ChristmasBoxEvent.items;
                            int index2 = Game_Server.Generic.random(0, items1.Length - 1);
                            string str2 = items1[index2];
                            if (ItemManager.GetItem(str2) != null)
                            {
                                int days5 = new Random().Next(Game_Server.Configs.Server.ChristmasBoxEvent.MinDays, Game_Server.Configs.Server.ChristmasBoxEvent.MaxDays);
                                if (str2.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str2, days5);
                                else
                                    Inventory.AddItem(usr, str2, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str2, days5));
                                return;
                            }
                            Log.WriteError(str2 + " is not a valid item @ random box event!");
                            return;
                        case "CR06":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] attendanceBox1 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                            int index3 = Game_Server.Generic.random(0, attendanceBox1.Length - 1);
                            string str3 = attendanceBox1[index3];
                            if (ItemManager.GetItem(str3) != null)
                            {
                                int days5 = new Random().Next(3, 30);
                                if (str3.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str3, days5);
                                else
                                    Inventory.AddItem(usr, str3, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str3, days5));
                                return;
                            }
                            Log.WriteError(str3 + " is not a valid item @ attendance box box event!");
                            return;
                        case "CR07":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] attendanceBox2 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                            int index4 = Game_Server.Generic.random(0, attendanceBox2.Length - 1);
                            string str4 = attendanceBox2[index4];
                            if (ItemManager.GetItem(str4) != null)
                            {
                                int days5 = new Random().Next(3, 30);
                                if (str4.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str4, days5);
                                else
                                    Inventory.AddItem(usr, str4, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str4, days5));
                                return;
                            }
                            Log.WriteError(str4 + " is not a valid item @ attendance box box event!");
                            return;
                        case "CR08":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] attendanceBox3 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                            int index5 = Game_Server.Generic.random(0, attendanceBox3.Length - 1);
                            string str5 = attendanceBox3[index5];
                            if (ItemManager.GetItem(str5) != null)
                            {
                                int days5 = new Random().Next(3, 30);
                                if (str5.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str5, days5);
                                else
                                    Inventory.AddItem(usr, str5, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str5, days5));
                                return;
                            }
                            Log.WriteError(str5 + " is not a valid item @ attendance box box event!");
                            return;
                        case "CR09":
                            int num12 = Game_Server.Generic.random(0, 5);
                            int days6 = 1;
                            string itemcode6 = (string)null;
                            switch (num12)
                            {
                                case 0:
                                    itemcode6 = "DC80";
                                    days6 = 30;
                                    break;
                                case 1:
                                    itemcode6 = "DC80";
                                    days6 = -1;
                                    break;
                                case 2:
                                    itemcode6 = "DF95";
                                    days6 = 7;
                                    break;
                                case 3:
                                    itemcode6 = "DF15";
                                    days6 = 7;
                                    break;
                                case 4:
                                    itemcode6 = "DE35";
                                    days6 = 7;
                                    break;
                                case 5:
                                    itemcode6 = "DF71";
                                    days6 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode6, days6);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode6, days6));
                            return;
                        case "CY40":
                            int num123 = Game_Server.Generic.random(0, 15);
                            int days612 = 1;
                            string itemcode613 = (string)null;
                            switch (num123)
                            {
                                case 0:
                                    itemcode613 = "DE86";
                                    days612 = 365;
                                    break;
                                case 1:
                                    itemcode613 = "DE86";
                                    days612 = 90;
                                    break;
                                case 2:
                                    itemcode613 = "DE86";
                                    days612 = 60;
                                    break;
                                case 3:
                                    itemcode613 = "DE86";
                                    days612 = 30;
                                    break;
                                case 4:
                                    itemcode613 = "DE86";
                                    days612 = 15;
                                    break;
                                case 5:
                                    itemcode613 = "DE86";
                                    days612 = 7;
                                    break;
                                case 6:
                                    itemcode613 = "DJ58";
                                    days612 = 365;
                                    break;
                                case 7:
                                    itemcode613 = "DJ58";
                                    days612 = 90;
                                    break;
                                case 8:
                                    itemcode613 = "DJ58";
                                    days612 = 60;
                                    break;
                                case 9:
                                    itemcode613 = "DJ58";
                                    days612 = 30;
                                    break;
                                case 10:
                                    itemcode613 = "DJ58";
                                    days612 = 15;
                                    break;
                                case 11:
                                    itemcode613 = "DJ58";
                                    days612 = 7;
                                    break;
                                case 12:
                                    itemcode613 = "CZ73";
                                    days612 = -1;
                                    break;
                                case 13:
                                    itemcode613 = "DS01";
                                    days612 = 15;
                                    break;
                                case 14:
                                    itemcode613 = "CI01";
                                    days612 = 7;
                                    break;
                                case 15:
                                    itemcode613 = "DS10";
                                    days612 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode613, days612);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode613, days612));
                            return;
                        case "CY41":
                            int num124 = Game_Server.Generic.random(0, 5);
                            int days613 = 1;
                            string itemcode612 = (string)null;
                            switch (num124)
                            {
                                case 0:
                                    itemcode612 = "DD25";
                                    days613 = 365;
                                    break;
                                case 1:
                                    itemcode612 = "DD25";
                                    days613 = 90;
                                    break;
                                case 2:
                                    itemcode612 = "DD25";
                                    days613 = 60;
                                    break;
                                case 3:
                                    itemcode612 = "DD25";
                                    days613 = 30;
                                    break;
                                case 4:
                                    itemcode612 = "DD25";
                                    days613 = 15;
                                    break;
                                case 5:
                                    itemcode612 = "DD25";
                                    days613 = 7;
                                    break;
                                case 6:
                                    itemcode612 = "GG03";
                                    days613 = 365;
                                    break;
                                case 7:
                                    itemcode612 = "GG03";
                                    days613 = 90;
                                    break;
                                case 8:
                                    itemcode612 = "GG03";
                                    days613 = 60;
                                    break;
                                case 9:
                                    itemcode612 = "GG03";
                                    days613 = 30;
                                    break;
                                case 10:
                                    itemcode612 = "GG03";
                                    days613 = 15;
                                    break;
                                case 11:
                                    itemcode612 = "GG03";
                                    days613 = 7;
                                    break;
                                case 12:
                                    itemcode612 = "CZ73";
                                    days613 = -1;
                                    break;
                                case 13:
                                    itemcode612 = "DS01";
                                    days613 = 7;
                                    break;
                                case 14:
                                    itemcode612 = "CI01";
                                    days613 = 7;
                                    break;
                                case 15:
                                    itemcode612 = "DS10";
                                    days613 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode612, days613);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode612, days613));
                            return;
                        case "CY32":
                            int num1201 = Game_Server.Generic.random(0, 49);
                            int days640 = 1;
                            string itemcode640 = (string)null;
                            switch (num1201)
                            {
                                case 0:
                                    itemcode640 = "D706";
                                    days640 = 365;
                                    break;
                                case 1:
                                    itemcode640 = "D706";
                                    days640 = 45;
                                    break;
                                case 2:
                                    itemcode640 = "D706";
                                    days640 = 30;
                                    break;
                                case 3:
                                    itemcode640 = "D706";
                                    days640 = 15;
                                    break;
                                case 4:
                                    itemcode640 = "D706";
                                    days640 = 7;
                                    break;
                                case 5:
                                    itemcode640 = "D804";
                                    days640 = 365;
                                    break;
                                case 6:
                                    itemcode640 = "D804";
                                    days640 = 45;
                                    break;
                                case 7:
                                    itemcode640 = "D804";
                                    days640 = 30;
                                    break;
                                case 8:
                                    itemcode640 = "D804";
                                    days640 = 15;
                                    break;
                                case 9:
                                    itemcode640 = "D804";
                                    days640 = 7;
                                    break;
                                case 49:
                                    itemcode640 = "D833";
                                    days640 = 365;
                                    break;
                                case 48:
                                    itemcode640 = "D833";
                                    days640 = 45;
                                    break;
                                case 47:
                                    itemcode640 = "D833";
                                    days640 = 30;
                                    break;
                                case 46:
                                    itemcode640 = "D833";
                                    days640 = 15;
                                    break;
                                case 10:
                                    itemcode640 = "D833";
                                    days640 = 7;
                                    break;
                                case 11:
                                    itemcode640 = "D608";
                                    days640 = 365;
                                    break;
                                case 12:
                                    itemcode640 = "D608";
                                    days640 = 45;
                                    break;
                                case 13:
                                    itemcode640 = "D608";
                                    days640 = 30;
                                    break;
                                case 14:
                                    itemcode640 = "D608";
                                    days640 = 15;
                                    break;
                                case 15:
                                    itemcode640 = "D608";
                                    days640 = 7;
                                    break;
                                case 16:
                                    itemcode640 = "D828";
                                    days640 = 365;
                                    break;
                                case 17:
                                    itemcode640 = "D828";
                                    days640 = 45;
                                    break;
                                case 18:
                                    itemcode640 = "D828";
                                    days640 = 30;
                                    break;
                                case 19:
                                    itemcode640 = "D828";
                                    days640 = 15;
                                    break;
                                case 20:
                                    itemcode640 = "D828";
                                    days640 = 7;
                                    break;
                                case 21:
                                    itemcode640 = "D607";
                                    days640 = 365;
                                    break;
                                case 22:
                                    itemcode640 = "D607";
                                    days640 = 45;
                                    break;
                                case 23:
                                    itemcode640 = "D607";
                                    days640 = 30;
                                    break;
                                case 24:
                                    itemcode640 = "D607";
                                    days640 = 15;
                                    break;
                                case 25:
                                    itemcode640 = "D607";
                                    days640 = 7;
                                    break;
                                case 26:
                                    itemcode640 = "D911";
                                    days640 = 365;
                                    break;
                                case 27:
                                    itemcode640 = "D911";
                                    days640 = 45;
                                    break;
                                case 28:
                                    itemcode640 = "D911";
                                    days640 = 30;
                                    break;
                                case 29:
                                    itemcode640 = "D911";
                                    days640 = 15;
                                    break;
                                case 30:
                                    itemcode640 = "D911";
                                    days640 = 7;
                                    break;
                                case 31:
                                    itemcode640 = "D831";
                                    days640 = 365;
                                    break;
                                case 32:
                                    itemcode640 = "D831";
                                    days640 = 45;
                                    break;
                                case 33:
                                    itemcode640 = "D831";
                                    days640 = 30;
                                    break;
                                case 34:
                                    itemcode640 = "D831";
                                    days640 = 15;
                                    break;
                                case 35:
                                    itemcode640 = "D831";
                                    days640 = 7;
                                    break;
                                case 36:
                                    itemcode640 = "D910";
                                    days640 = 365;
                                    break;
                                case 37:
                                    itemcode640 = "D910";
                                    days640 = 45;
                                    break;
                                case 38:
                                    itemcode640 = "D910";
                                    days640 = 30;
                                    break;
                                case 39:
                                    itemcode640 = "D910";
                                    days640 = 15;
                                    break;
                                case 40:
                                    itemcode640 = "D910";
                                    days640 = 7;
                                    break;
                                case 41:
                                    itemcode640 = "D832";
                                    days640 = 365;
                                    break;
                                case 42:
                                    itemcode640 = "D832";
                                    days640 = 45;
                                    break;
                                case 43:
                                    itemcode640 = "D832";
                                    days640 = 30;
                                    break;
                                case 44:
                                    itemcode640 = "D832";
                                    days640 = 15;
                                    break;
                                case 45:
                                    itemcode640 = "D832";
                                    days640 = 7;
                                    break;


                            }
                            Inventory.AddItem(usr, itemcode640, days640);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode640, days640));
                            return;
                        case "CR10":
                            int num13 = Game_Server.Generic.random(0, 5);
                            int days7 = 1;
                            string itemcode7 = (string)null;
                            switch (num13)
                            {
                                case 0:
                                    itemcode7 = "DC77";
                                    days7 = 30;
                                    break;
                                case 1:
                                    itemcode7 = "DC77";
                                    days7 = -1;
                                    break;
                                case 2:
                                    itemcode7 = "DC81";
                                    days7 = 7;
                                    break;
                                case 3:
                                    itemcode7 = "DC82";
                                    days7 = 7;
                                    break;
                                case 4:
                                    itemcode7 = "DE25";
                                    days7 = 7;
                                    break;
                                case 5:
                                    itemcode7 = "DE57";
                                    days7 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode7, days7);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode7, days7));
                            return;
                        case "CR11":
                            int num14 = Game_Server.Generic.random(0, 5);
                            int days8 = 1;
                            string itemcode8 = (string)null;
                            switch (num14)
                            {
                                case 0:
                                    itemcode8 = "DG46";
                                    days8 = 30;
                                    break;
                                case 1:
                                    itemcode8 = "DG46";
                                    days8 = -1;
                                    break;
                                case 2:
                                    itemcode8 = "DG51";
                                    days8 = 7;
                                    break;
                                case 3:
                                    itemcode8 = "DG58";
                                    days8 = 7;
                                    break;
                                case 4:
                                    itemcode8 = "DG22";
                                    days8 = 7;
                                    break;
                                case 5:
                                    itemcode8 = "DG45";
                                    days8 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode8, days8);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode8, days8));
                            return;
                        case "CR12":
                            int num15 = Game_Server.Generic.random(0, 5);
                            int days9 = 1;
                            string itemcode9 = (string)null;
                            switch (num15)
                            {
                                case 0:
                                    itemcode9 = "DF34";
                                    days9 = 30;
                                    break;
                                case 1:
                                    itemcode9 = "CZ85";
                                    days9 = 1;
                                    break;
                                case 2:
                                    itemcode9 = "CB09";
                                    days9 = 1;
                                    break;
                                case 3:
                                    itemcode9 = "CZ81";
                                    days9 = 1;
                                    break;
                                case 4:
                                    itemcode9 = "CZ84";
                                    days9 = 30;
                                    break;
                                case 5:
                                    itemcode9 = "CF01";
                                    days9 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode9, days9);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode9, days9));
                            return;
                        case "CR13":
                            int num16 = Game_Server.Generic.random(0, 7);
                            int days10 = 1;
                            string itemcode10 = (string)null;
                            switch (num16)
                            {
                                case 0:
                                    itemcode10 = "DC68";
                                    days10 = 30;
                                    break;
                                case 1:
                                    itemcode10 = "DF17";
                                    days10 = 30;
                                    break;
                                case 2:
                                    itemcode10 = "DG24";
                                    days10 = 30;
                                    break;
                                case 3:
                                    itemcode10 = "DG23";
                                    days10 = 30;
                                    break;
                                case 4:
                                    itemcode10 = "DC67";
                                    days10 = 30;
                                    break;
                                case 5:
                                    itemcode10 = "DF68";
                                    days10 = 30;
                                    break;
                                case 6:
                                    itemcode10 = "DJ69";
                                    days10 = 30;
                                    break;
                                case 7:
                                    itemcode10 = "DC36";
                                    days10 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode10, days10);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode10, days10));
                            return;
                        case "CR14":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] items2 = Game_Server.Configs.Server.RandomBoxEvent.items;
                            int index6 = Game_Server.Generic.random(0, items2.Length - 1);
                            string str6 = items2[index6];
                            if (ItemManager.GetItem(str6) != null)
                            {
                                int days5 = new Random().Next(Game_Server.Configs.Server.RandomBoxEvent.MinDays, Game_Server.Configs.Server.RandomBoxEvent.MaxDays);
                                if (str6.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str6, days5);
                                else
                                    Inventory.AddItem(usr, str6, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str6, days5));
                                return;
                            }
                            Log.WriteError(str6 + " is not a valid item @ random box event!");
                            return;
                        case "CR15":
                            int num17 = Game_Server.Generic.random(0, 20);
                            int days11 = 1;
                            string itemcode11 = (string)null;
                            switch (num17)
                            {
                                case 0:
                                    itemcode11 = "GF08";
                                    days11 = -1;
                                    break;
                                case 1:
                                    itemcode11 = "GF08";
                                    days11 = 180;
                                    break;
                                case 2:
                                    itemcode11 = "GF08";
                                    days11 = 90;
                                    break;
                                case 3:
                                    itemcode11 = "GF08";
                                    days11 = 60;
                                    break;
                                case 4:
                                    itemcode11 = "GF08";
                                    days11 = 45;
                                    break;
                                case 5:
                                    itemcode11 = "GF08";
                                    days11 = 30;
                                    break;
                                case 6:
                                    itemcode11 = "DG94";
                                    days11 = -1;
                                    break;
                                case 7:
                                    itemcode11 = "DG94";
                                    days11 = 180;
                                    break;
                                case 8:
                                    itemcode11 = "DG94";
                                    days11 = 90;
                                    break;
                                case 9:
                                    itemcode11 = "DG94";
                                    days11 = 60;
                                    break;
                                case 10:
                                    itemcode11 = "DG94";
                                    days11 = 30;
                                    break;
                                case 11:
                                    itemcode11 = "DD21";
                                    days11 = -1;
                                    break;
                                case 12:
                                    itemcode11 = "DD21";
                                    days11 = 180;
                                    break;
                                case 13:
                                    itemcode11 = "DD21";
                                    days11 = 90;
                                    break;
                                case 14:
                                    itemcode11 = "DD21";
                                    days11 = 60;
                                    break;
                                case 15:
                                    itemcode11 = "DD21";
                                    days11 = 30;
                                    break;
                                case 16:
                                    itemcode11 = "DT18";
                                    days11 = -1;
                                    break;
                                case 17:
                                    itemcode11 = "DT18";
                                    days11 = 180;
                                    break;
                                case 18:
                                    itemcode11 = "DT18";
                                    days11 = 90;
                                    break;
                                case 19:
                                    itemcode11 = "DT18";
                                    days11 = 60;
                                    break;
                                case 20:
                                    itemcode11 = "DT18";
                                    days11 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode11, days11);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode11, days11));
                            return;
                        case "CR16":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] attendanceBox4 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                            int index7 = Game_Server.Generic.random(0, attendanceBox4.Length - 1);
                            string str7 = attendanceBox4[index7];
                            if (ItemManager.GetItem(str7) != null)
                            {
                                int days5 = new Random().Next(3, 30);
                                if (str7.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str7, days5);
                                else
                                    Inventory.AddItem(usr, str7, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str7, days5));
                                return;
                            }
                            Log.WriteError(str7 + " is not a valid item @ attendance box box event!");
                            return;
                        case "CR17":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] attendanceBox5 = Game_Server.Configs.Server.ItemShop.attendanceBox;
                            int index8 = Game_Server.Generic.random(0, attendanceBox5.Length - 1);
                            string str8 = attendanceBox5[index8];
                            if (ItemManager.GetItem(str8) != null)
                            {
                                int days5 = new Random().Next(3, 30);
                                if (str8.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str8, days5);
                                else
                                    Inventory.AddItem(usr, str8, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str8, days5));
                                return;
                            }
                            Log.WriteError(str8 + " is not a valid item @ attendance box box event!");
                            return;
                        case "CR18":
                            int num18 = Game_Server.Generic.random(0, 30);
                            string itemcode12 = (string)null;
                            switch (num18)
                            {
                                case 0:
                                    itemcode12 = "DC21";
                                    break;
                                case 1:
                                    itemcode12 = "DC22";
                                    break;
                                case 2:
                                    itemcode12 = "DC23";
                                    break;
                                case 3:
                                    itemcode12 = "DC24";
                                    break;
                                case 4:
                                    itemcode12 = "DC25";
                                    break;
                                case 5:
                                    itemcode12 = "DC26";
                                    break;
                                case 6:
                                    itemcode12 = "DC27";
                                    break;
                                case 7:
                                    itemcode12 = "DC28";
                                    break;
                                case 8:
                                    itemcode12 = "DC29";
                                    break;
                                case 9:
                                    itemcode12 = "DC30";
                                    break;
                                case 10:
                                    itemcode12 = "DC41";
                                    break;
                                case 11:
                                    itemcode12 = "DC42";
                                    break;
                                case 12:
                                    itemcode12 = "DC43";
                                    break;
                                case 13:
                                    itemcode12 = "DC44";
                                    break;
                                case 14:
                                    itemcode12 = "DC45";
                                    break;
                                case 15:
                                    itemcode12 = "DC46";
                                    break;
                                case 16:
                                    itemcode12 = "DC47";
                                    break;
                                case 17:
                                    itemcode12 = "DC48";
                                    break;
                                case 18:
                                    itemcode12 = "DC49";
                                    break;
                                case 19:
                                    itemcode12 = "DC50";
                                    break;
                                case 20:
                                    itemcode12 = "DC51";
                                    break;
                                case 21:
                                    itemcode12 = "DC52";
                                    break;
                                case 22:
                                    itemcode12 = "DC53";
                                    break;
                                case 23:
                                    itemcode12 = "DC54";
                                    break;
                                case 24:
                                    itemcode12 = "DC55";
                                    break;
                                case 25:
                                    itemcode12 = "DC56";
                                    break;
                                case 26:
                                    itemcode12 = "DC57";
                                    break;
                                case 27:
                                    itemcode12 = "DC58";
                                    break;
                                case 28:
                                    itemcode12 = "DC59";
                                    break;
                                case 29:
                                    itemcode12 = "DC60";
                                    break;
                                case 30:
                                    itemcode12 = "DC62";
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode12, -1);
                            Inventory.DecreaseEAItem(usr, "CR18");
                            usr.send((Packet)new SP_WinItem(usr, itemcode12, -1));
                            return;
                        case "CR19":
                            int num19 = Game_Server.Generic.random(0, 8);
                            string itemcode13 = (string)null;
                            switch (num19)
                            {
                                case 0:
                                    itemcode13 = "D807";
                                    break;
                                case 1:
                                    itemcode13 = "D808";
                                    break;
                                case 2:
                                    itemcode13 = "D809";
                                    break;
                                case 3:
                                    itemcode13 = "D810";
                                    break;
                                case 4:
                                    itemcode13 = "D811";
                                    break;
                                case 5:
                                    itemcode13 = "D812";
                                    break;
                                case 6:
                                    itemcode13 = "D822";
                                    break;
                                case 7:
                                    itemcode13 = "D825";
                                    break;
                                case 8:
                                    itemcode13 = "D826";
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode13, -1);
                            Inventory.DecreaseEAItem(usr, "CR19");
                            usr.send((Packet)new SP_WinItem(usr, itemcode13, -1));
                            return;
                        case "CR20":
                            int num20 = Game_Server.Generic.random(0, 5);
                            int days12 = 1;
                            string itemcode14 = (string)null;
                            switch (num20)
                            {
                                case 0:
                                    itemcode14 = "DF50";
                                    days12 = -1;
                                    break;
                                case 1:
                                    itemcode14 = "DF50";
                                    days12 = 30;
                                    break;
                                case 2:
                                    itemcode14 = "DF50";
                                    days12 = 15;
                                    break;
                                case 3:
                                    itemcode14 = "CF01";
                                    days12 = 15;
                                    break;
                                case 4:
                                    itemcode14 = "CI01";
                                    days12 = 15;
                                    break;
                                case 5:
                                    itemcode14 = "DS03";
                                    days12 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode14, days12);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode14, days12));
                            return;
                        case "CR21":
                            int num21 = Game_Server.Generic.random(0, 5);
                            int days13 = 1;
                            string itemcode15 = (string)null;
                            switch (num21)
                            {
                                case 0:
                                    itemcode15 = "DC85";
                                    days13 = -1;
                                    break;
                                case 1:
                                    itemcode15 = "DC85";
                                    days13 = 30;
                                    break;
                                case 2:
                                    itemcode15 = "DC85";
                                    days13 = 15;
                                    break;
                                case 3:
                                    itemcode15 = "CF01";
                                    days13 = 15;
                                    break;
                                case 4:
                                    itemcode15 = "CI01";
                                    days13 = 15;
                                    break;
                                case 5:
                                    itemcode15 = "DS03";
                                    days13 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode15, days13);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode15, days13));
                            return;
                        case "CR22":
                            int num22 = Game_Server.Generic.random(0, 5);
                            int days14 = 1;
                            string itemcode16 = (string)null;
                            switch (num22)
                            {
                                case 0:
                                    itemcode16 = "DJ22";
                                    days14 = -1;
                                    break;
                                case 1:
                                    itemcode16 = "DJ22";
                                    days14 = 30;
                                    break;
                                case 2:
                                    itemcode16 = "DJ22";
                                    days14 = 15;
                                    break;
                                case 3:
                                    itemcode16 = "CF01";
                                    days14 = 15;
                                    break;
                                case 4:
                                    itemcode16 = "CI01";
                                    days14 = 15;
                                    break;
                                case 5:
                                    itemcode16 = "DS03";
                                    days14 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode16, days14);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode16, days14));
                            return;
                        case "CR23":
                            int num23 = Game_Server.Generic.random(0, 5);
                            int days15 = 1;
                            string itemcode17 = (string)null;
                            switch (num23)
                            {
                                case 0:
                                    itemcode17 = "DG48";
                                    days15 = -1;
                                    break;
                                case 1:
                                    itemcode17 = "DG48";
                                    days15 = 30;
                                    break;
                                case 2:
                                    itemcode17 = "DG48";
                                    days15 = 15;
                                    break;
                                case 3:
                                    itemcode17 = "CF01";
                                    days15 = 15;
                                    break;
                                case 4:
                                    itemcode17 = "CI01";
                                    days15 = 15;
                                    break;
                                case 5:
                                    itemcode17 = "DS03";
                                    days15 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode17, days15);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode17, days15));
                            return;
                        case "CR24":
                            int num24 = Game_Server.Generic.random(0, 3);
                            int days16 = 1;
                            string itemcode18 = (string)null;
                            switch (num24)
                            {
                                case 0:
                                    itemcode18 = "DC61";
                                    days16 = 30;
                                    break;
                                case 1:
                                    itemcode18 = "DC61";
                                    days16 = -1;
                                    break;
                                case 2:
                                    itemcode18 = "DC34";
                                    days16 = 30;
                                    break;
                                case 3:
                                    itemcode18 = "DC34";
                                    days16 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode18, days16);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode18, days16));
                            return;
                        case "CR25":
                            int num25 = Game_Server.Generic.random(0, 6);
                            int days17 = 1;
                            string itemcode19 = (string)null;
                            switch (num25)
                            {
                                case 0:
                                    itemcode19 = "DC61";
                                    days17 = 30;
                                    break;
                                case 1:
                                    itemcode19 = "DC61";
                                    days17 = -1;
                                    break;
                                case 2:
                                    itemcode19 = "DC34";
                                    days17 = 30;
                                    break;
                                case 3:
                                    itemcode19 = "DC34";
                                    days17 = -1;
                                    break;
                                case 4:
                                    itemcode19 = "CZ84";
                                    days17 = -1;
                                    break;
                                case 5:
                                    itemcode19 = "CZ85";
                                    days17 = -1;
                                    break;
                                case 6:
                                    itemcode19 = "CZ86";
                                    days17 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode19, days17);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode19, days17));
                            return;
                        case "CR26":
                            int num26 = Game_Server.Generic.random(0, 5);
                            int days18 = 1;
                            string itemcode20 = (string)null;
                            switch (num26)
                            {
                                case 0:
                                    itemcode20 = "DF48";
                                    days18 = 30;
                                    break;
                                case 1:
                                    itemcode20 = "DF48";
                                    days18 = 15;
                                    break;
                                case 2:
                                    itemcode20 = "DF48";
                                    days18 = -1;
                                    break;
                                case 3:
                                    itemcode20 = "CF01";
                                    days18 = 15;
                                    break;
                                case 4:
                                    itemcode20 = "CZ81";
                                    days18 = 1;
                                    break;
                                case 5:
                                    itemcode20 = "DS01";
                                    days18 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode20, days18);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode20, days18));
                            return;
                        case "CR27":
                            int num27 = Game_Server.Generic.random(0, 10);
                            int days19 = 1;
                            string itemcode21 = (string)null;
                            switch (num27)
                            {
                                case 0:
                                    itemcode21 = "DC98";
                                    days19 = 30;
                                    break;
                                case 1:
                                    itemcode21 = "DC98";
                                    days19 = -1;
                                    break;
                                case 2:
                                    itemcode21 = "DC98";
                                    days19 = 15;
                                    break;
                                case 3:
                                    itemcode21 = "CF01";
                                    days19 = 15;
                                    break;
                                case 4:
                                    itemcode21 = "DG30";
                                    days19 = 30;
                                    break;
                                case 5:
                                    itemcode21 = "DS01";
                                    days19 = 15;
                                    break;
                                case 6:
                                    itemcode21 = "CA01";
                                    days19 = 15;
                                    break;
                                case 7:
                                    itemcode21 = "DS03";
                                    days19 = 15;
                                    break;
                                case 8:
                                    itemcode21 = "DC03";
                                    days19 = 30;
                                    break;
                                case 9:
                                    itemcode21 = "DB10";
                                    days19 = 30;
                                    break;
                                case 10:
                                    itemcode21 = "CD07";
                                    days19 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode21, days19);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode21, days19));
                            return;
                        case "CR28":
                            int num28 = Game_Server.Generic.random(0, 21);
                            int days20 = 1;
                            string itemcode22 = (string)null;
                            switch (num28)
                            {
                                case 0:
                                    itemcode22 = "DF33";
                                    days20 = 30;
                                    break;
                                case 1:
                                    itemcode22 = "DF33";
                                    days20 = -1;
                                    break;
                                case 2:
                                    itemcode22 = "DC98";
                                    days20 = 15;
                                    break;
                                case 5:
                                    itemcode22 = "DF06";
                                    days20 = 15;
                                    break;
                                case 6:
                                    itemcode22 = "CZ84";
                                    days20 = 1;
                                    break;
                                case 7:
                                    itemcode22 = "CZ85";
                                    days20 = 15;
                                    break;
                                case 8:
                                    itemcode22 = "DU01";
                                    days20 = 7;
                                    break;
                                case 9:
                                    itemcode22 = "CZ81";
                                    days20 = 1;
                                    break;
                                case 10:
                                    itemcode22 = "DS01";
                                    days20 = 7;
                                    break;
                                case 11:
                                    itemcode22 = "DU01";
                                    days20 = 7;
                                    break;
                                case 12:
                                    itemcode22 = "DS03";
                                    days20 = 7;
                                    break;
                                case 13:
                                    itemcode22 = "DF35";
                                    days20 = 7;
                                    break;
                                case 14:
                                    itemcode22 = "DF06";
                                    days20 = -1;
                                    break;
                                case 15:
                                    itemcode22 = "DC01";
                                    days20 = 30;
                                    break;
                                case 16:
                                    itemcode22 = "CZ73";
                                    days20 = 1;
                                    break;
                                case 17:
                                    itemcode22 = "DB10";
                                    days20 = 30;
                                    break;
                                case 18:
                                    itemcode22 = "DF07";
                                    days20 = 30;
                                    break;
                                case 19:
                                    itemcode22 = "DC01";
                                    days20 = -1;
                                    break;
                                case 20:
                                    itemcode22 = "CF01";
                                    days20 = 30;
                                    break;
                                case 21:
                                    itemcode22 = "DF07";
                                    days20 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode22, days20);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode22, days20));
                            return;
                        case "CR29":
                            int num29 = Game_Server.Generic.random(0, 5);
                            int days21 = 1;
                            string itemcode23 = (string)null;
                            switch (num29)
                            {
                                case 0:
                                    itemcode23 = "DC79";
                                    days21 = 30;
                                    break;
                                case 1:
                                    itemcode23 = "DC79";
                                    days21 = -1;
                                    break;
                                case 2:
                                    itemcode23 = "DC80";
                                    days21 = 7;
                                    break;
                                case 3:
                                    itemcode23 = "DC98";
                                    days21 = 7;
                                    break;
                                case 4:
                                    itemcode23 = "DE30";
                                    days21 = 7;
                                    break;
                                case 5:
                                    itemcode23 = "DE46";
                                    days21 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode23, days21);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode23, days21));
                            return;
                        case "CR30":
                            int num30 = Game_Server.Generic.random(0, 6);
                            int days22 = 1;
                            string itemcode24 = (string)null;
                            switch (num30)
                            {
                                case 0:
                                    itemcode24 = "DF39";
                                    days22 = 30;
                                    break;
                                case 1:
                                    itemcode24 = "DF39";
                                    days22 = 7;
                                    break;
                                case 2:
                                    itemcode24 = "DF95";
                                    days22 = 7;
                                    break;
                                case 3:
                                    itemcode24 = "DF15";
                                    days22 = 7;
                                    break;
                                case 4:
                                    itemcode24 = "DE35";
                                    days22 = 7;
                                    break;
                                case 5:
                                    itemcode24 = "DF71";
                                    days22 = 7;
                                    break;
                                case 6:
                                    itemcode24 = "CB09";
                                    days22 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode24, days22);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode24, days22));
                            return;
                        case "CR31":
                            int num31 = Game_Server.Generic.random(0, 6);
                            int days23 = 1;
                            string itemcode25 = (string)null;
                            switch (num31)
                            {
                                case 0:
                                    itemcode25 = "DG45";
                                    days23 = 30;
                                    break;
                                case 1:
                                    itemcode25 = "CZ85";
                                    days23 = 1;
                                    break;
                                case 2:
                                    itemcode25 = "CB09";
                                    days23 = 1;
                                    break;
                                case 3:
                                    itemcode25 = "CF02";
                                    days23 = 7;
                                    break;
                                case 4:
                                    itemcode25 = "DS01";
                                    days23 = 30;
                                    break;
                                case 5:
                                    itemcode25 = "DB10";
                                    days23 = 1;
                                    break;
                                case 6:
                                    itemcode25 = "CZ84";
                                    days23 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode25, days23);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode25, days23));
                            return;
                        case "CR32":
                            int num32 = Game_Server.Generic.random(0, 6);
                            int days24 = 1;
                            string itemcode26 = (string)null;
                            switch (num32)
                            {
                                case 0:
                                    itemcode26 = "DG24";
                                    days24 = 30;
                                    break;
                                case 1:
                                    itemcode26 = "CZ85";
                                    days24 = 1;
                                    break;
                                case 2:
                                    itemcode26 = "CB09";
                                    days24 = 1;
                                    break;
                                case 3:
                                    itemcode26 = "CF02";
                                    days24 = 7;
                                    break;
                                case 4:
                                    itemcode26 = "DS01";
                                    days24 = 30;
                                    break;
                                case 5:
                                    itemcode26 = "DB10";
                                    days24 = 1;
                                    break;
                                case 6:
                                    itemcode26 = "CZ81";
                                    days24 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode26, days24);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode26, days24));
                            return;
                        case "CR33":
                            int num33 = Game_Server.Generic.random(0, 6);
                            int days25 = 1;
                            string itemcode27 = (string)null;
                            switch (num33)
                            {
                                case 0:
                                    itemcode27 = "DF17";
                                    days25 = 30;
                                    break;
                                case 1:
                                    itemcode27 = "CZ85";
                                    days25 = 1;
                                    break;
                                case 2:
                                    itemcode27 = "CB09";
                                    days25 = 1;
                                    break;
                                case 3:
                                    itemcode27 = "CF01";
                                    days25 = 7;
                                    break;
                                case 4:
                                    itemcode27 = "DS01";
                                    days25 = 30;
                                    break;
                                case 5:
                                    itemcode27 = "DB10";
                                    days25 = 1;
                                    break;
                                case 6:
                                    itemcode27 = "CZ81";
                                    days25 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode27, days25);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode27, days25));
                            return;
                        case "CR34":
                            int num34 = Game_Server.Generic.random(0, 5);
                            int days26 = 1;
                            string itemcode28 = (string)null;
                            switch (num34)
                            {
                                case 0:
                                    itemcode28 = "DC80";
                                    days26 = 30;
                                    break;
                                case 1:
                                    itemcode28 = "DC80";
                                    days26 = -1;
                                    break;
                                case 2:
                                    itemcode28 = "DB10";
                                    days26 = 7;
                                    break;
                                case 3:
                                    itemcode28 = "CZ81";
                                    days26 = 1;
                                    break;
                                case 4:
                                    itemcode28 = "CZ85";
                                    days26 = 1;
                                    break;
                                case 5:
                                    itemcode28 = "CB09";
                                    days26 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode28, days26);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode28, days26));
                            return;
                        case "CR35":
                            int num35 = Game_Server.Generic.random(0, 5);
                            int days27 = 1;
                            string itemcode29 = (string)null;
                            switch (num35)
                            {
                                case 0:
                                    itemcode29 = "DG46";
                                    days27 = 30;
                                    break;
                                case 1:
                                    itemcode29 = "DG46";
                                    days27 = -1;
                                    break;
                                case 2:
                                    itemcode29 = "DF95";
                                    days27 = 7;
                                    break;
                                case 3:
                                    itemcode29 = "CZ84";
                                    days27 = 1;
                                    break;
                                case 4:
                                    itemcode29 = "CB09";
                                    days27 = 1;
                                    break;
                                case 5:
                                    itemcode29 = "CZ81";
                                    days27 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode29, days27);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode29, days27));
                            return;
                        case "CR36":
                            int num36 = Game_Server.Generic.random(0, 6);
                            int days28 = 1;
                            string itemcode30 = (string)null;
                            switch (num36)
                            {
                                case 0:
                                    itemcode30 = "DE16";
                                    days28 = 30;
                                    break;
                                case 1:
                                    itemcode30 = "CZ85";
                                    days28 = 1;
                                    break;
                                case 2:
                                    itemcode30 = "CB09";
                                    days28 = 1;
                                    break;
                                case 3:
                                    itemcode30 = "CF02";
                                    days28 = 7;
                                    break;
                                case 4:
                                    itemcode30 = "DS01";
                                    days28 = 30;
                                    break;
                                case 5:
                                    itemcode30 = "DB10";
                                    days28 = 1;
                                    break;
                                case 6:
                                    itemcode30 = "CZ81";
                                    days28 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode30, days28);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode30, days28));
                            return;
                        case "CR37":
                            int num37 = Game_Server.Generic.random(0, 6);
                            int days29 = 1;
                            string itemcode31 = (string)null;
                            switch (num37)
                            {
                                case 0:
                                    itemcode31 = "DF73";
                                    days29 = 30;
                                    break;
                                case 1:
                                    itemcode31 = "DC31";
                                    days29 = 7;
                                    break;
                                case 2:
                                    itemcode31 = "CB09";
                                    days29 = 1;
                                    break;
                                case 3:
                                    itemcode31 = "CF02";
                                    days29 = 7;
                                    break;
                                case 4:
                                    itemcode31 = "DS01";
                                    days29 = 30;
                                    break;
                                case 5:
                                    itemcode31 = "DB10";
                                    days29 = 1;
                                    break;
                                case 6:
                                    itemcode31 = "CZ81";
                                    days29 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode31, days29);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode31, days29));
                            return;
                        case "CR38":
                            int num38 = Game_Server.Generic.random(0, 12);
                            int days30 = 1;
                            string itemcode32 = (string)null;
                            switch (num38)
                            {
                                case 0:
                                    itemcode32 = "DF52";
                                    days30 = 30;
                                    break;
                                case 1:
                                    itemcode32 = "DF39";
                                    days30 = 7;
                                    break;
                                case 2:
                                    itemcode32 = "DF95";
                                    days30 = 7;
                                    break;
                                case 3:
                                    itemcode32 = "DF15";
                                    days30 = 7;
                                    break;
                                case 4:
                                    itemcode32 = "DE35";
                                    days30 = 7;
                                    break;
                                case 5:
                                    itemcode32 = "DF71";
                                    days30 = 7;
                                    break;
                                case 6:
                                    itemcode32 = "CB09";
                                    days30 = 1;
                                    break;
                                case 7:
                                    itemcode32 = "DF06";
                                    days30 = -1;
                                    break;
                                case 8:
                                    itemcode32 = "DG03";
                                    days30 = -1;
                                    break;
                                case 9:
                                    itemcode32 = "CZ84";
                                    days30 = 1;
                                    break;
                                case 10:
                                    itemcode32 = "CZ85";
                                    days30 = 1;
                                    break;
                                case 11:
                                    itemcode32 = "DC01";
                                    days30 = -1;
                                    break;
                                case 12:
                                    itemcode32 = "CZ81";
                                    days30 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode32, days30);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode32, days30));
                            return;
                        case "CR39":
                            int num39 = Game_Server.Generic.random(0, 9);
                            int days31 = 1;
                            string itemcode33 = (string)null;
                            switch (num39)
                            {
                                case 0:
                                    itemcode33 = "DF52";
                                    days31 = 30;
                                    break;
                                case 1:
                                    itemcode33 = "DF18";
                                    days31 = 15;
                                    break;
                                case 2:
                                    itemcode33 = "CZ81";
                                    days31 = 1;
                                    break;
                                case 3:
                                    itemcode33 = "DF15";
                                    days31 = 7;
                                    break;
                                case 4:
                                    itemcode33 = "DE35";
                                    days31 = 7;
                                    break;
                                case 5:
                                    itemcode33 = "DB33";
                                    days31 = 7;
                                    break;
                                case 6:
                                    itemcode33 = "CB09";
                                    days31 = 1;
                                    break;
                                case 7:
                                    itemcode33 = "CZ83";
                                    days31 = 1;
                                    break;
                                case 8:
                                    itemcode33 = "CZ85";
                                    days31 = 1;
                                    break;
                                case 9:
                                    itemcode33 = "CZ81";
                                    days31 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode33, days31);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode33, days31));
                            return;
                        case "CR40":
                            int num40 = Game_Server.Generic.random(0, 5);
                            int days32 = 1;
                            string itemcode34 = (string)null;
                            switch (num40)
                            {
                                case 0:
                                    itemcode34 = "DF65";
                                    days32 = 30;
                                    break;
                                case 1:
                                    itemcode34 = "CZ84";
                                    days32 = 1;
                                    break;
                                case 2:
                                    itemcode34 = "CZ81";
                                    days32 = 1;
                                    break;
                                case 3:
                                    itemcode34 = "CB09";
                                    days32 = 1;
                                    break;
                                case 4:
                                    itemcode34 = "CF01";
                                    days32 = 7;
                                    break;
                                case 5:
                                    itemcode34 = "DB10";
                                    days32 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode34, days32);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode34, days32));
                            return;
                        case "CR41":
                            int num41 = Game_Server.Generic.random(0, 5);
                            int days33 = 1;
                            string itemcode35 = (string)null;
                            switch (num41)
                            {
                                case 0:
                                    itemcode35 = "DC93";
                                    days33 = 30;
                                    break;
                                case 1:
                                    itemcode35 = "CZ84";
                                    days33 = 1;
                                    break;
                                case 2:
                                    itemcode35 = "CZ81";
                                    days33 = 1;
                                    break;
                                case 3:
                                    itemcode35 = "CB09";
                                    days33 = 1;
                                    break;
                                case 4:
                                    itemcode35 = "CF02";
                                    days33 = 7;
                                    break;
                                case 5:
                                    itemcode35 = "DB10";
                                    days33 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode35, days33);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode35, days33));
                            return;
                        case "CR42":
                            int num42 = Game_Server.Generic.random(0, 5);
                            int days34 = 1;
                            string itemcode36 = (string)null;
                            switch (num42)
                            {
                                case 0:
                                    itemcode36 = "DD07";
                                    days34 = 30;
                                    break;
                                case 1:
                                    itemcode36 = "CZ84";
                                    days34 = 1;
                                    break;
                                case 2:
                                    itemcode36 = "CZ81";
                                    days34 = 1;
                                    break;
                                case 3:
                                    itemcode36 = "CB09";
                                    days34 = 1;
                                    break;
                                case 4:
                                    itemcode36 = "CF02";
                                    days34 = 7;
                                    break;
                                case 5:
                                    itemcode36 = "DB10";
                                    days34 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode36, days34);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode36, days34));
                            return;
                        case "CR43":
                            int num43 = Game_Server.Generic.random(0, 6);
                            int days35 = 1;
                            string itemcode37 = (string)null;
                            switch (num43)
                            {
                                case 0:
                                    itemcode37 = "DG68";
                                    days35 = -1;
                                    break;
                                case 1:
                                    itemcode37 = "DG68";
                                    days35 = 15;
                                    break;
                                case 2:
                                    itemcode37 = "DG53";
                                    days35 = 7;
                                    break;
                                case 3:
                                    itemcode37 = "DC84";
                                    days35 = 7;
                                    break;
                                case 4:
                                    itemcode37 = "DF84";
                                    days35 = 7;
                                    break;
                                case 5:
                                    itemcode37 = "DA75";
                                    days35 = 7;
                                    break;
                                case 6:
                                    itemcode37 = "DG68";
                                    days35 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode37, days35);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode37, days35));
                            return;
                        case "CR44":
                            int num44 = Game_Server.Generic.random(0, 5);
                            int days36 = 1;
                            string itemcode38 = (string)null;
                            switch (num44)
                            {
                                case 0:
                                    itemcode38 = "DF36";
                                    days36 = 30;
                                    break;
                                case 1:
                                    itemcode38 = "CZ84";
                                    days36 = 1;
                                    break;
                                case 2:
                                    itemcode38 = "CZ81";
                                    days36 = 1;
                                    break;
                                case 3:
                                    itemcode38 = "CB09";
                                    days36 = 1;
                                    break;
                                case 4:
                                    itemcode38 = "DS01";
                                    days36 = 7;
                                    break;
                                case 5:
                                    itemcode38 = "DB10";
                                    days36 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode38, days36);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode38, days36));
                            return;
                        case "CR45":
                            int num45 = Game_Server.Generic.random(0, 6);
                            int days37 = 1;
                            string itemcode39 = (string)null;
                            switch (num45)
                            {
                                case 0:
                                    itemcode39 = "DG51";
                                    days37 = 30;
                                    break;
                                case 1:
                                    itemcode39 = "CZ81";
                                    days37 = 1;
                                    break;
                                case 2:
                                    itemcode39 = "DC80";
                                    days37 = 30;
                                    break;
                                case 3:
                                    itemcode39 = "DC98";
                                    days37 = 30;
                                    break;
                                case 4:
                                    itemcode39 = "DE30";
                                    days37 = 30;
                                    break;
                                case 5:
                                    itemcode39 = "DE46";
                                    days37 = 30;
                                    break;
                                case 6:
                                    itemcode39 = "CB09";
                                    days37 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode39, days37);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode39, days37));
                            return;
                        case "CR46":
                            int num46 = Game_Server.Generic.random(0, 6);
                            int days38 = 1;
                            string itemcode40 = (string)null;
                            switch (num46)
                            {
                                case 0:
                                    itemcode40 = "DF96";
                                    days38 = 30;
                                    break;
                                case 1:
                                    itemcode40 = "CZ81";
                                    days38 = 1;
                                    break;
                                case 2:
                                    itemcode40 = "DC19";
                                    days38 = 30;
                                    break;
                                case 3:
                                    itemcode40 = "DC98";
                                    days38 = 30;
                                    break;
                                case 4:
                                    itemcode40 = "DE30";
                                    days38 = 30;
                                    break;
                                case 5:
                                    itemcode40 = "DE46";
                                    days38 = 30;
                                    break;
                                case 6:
                                    itemcode40 = "CB09";
                                    days38 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode40, days38);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode40, days38));
                            return;
                        case "CR47":
                            int num47 = Game_Server.Generic.random(0, 28);
                            int days39 = 1;
                            string itemcode41 = (string)null;
                            switch (num47)
                            {
                                case 0:
                                    itemcode41 = "DE07";
                                    days39 = 30;
                                    break;
                                case 1:
                                    itemcode41 = "DE07";
                                    days39 = -1;
                                    break;
                                case 2:
                                    itemcode41 = "DC73";
                                    days39 = 30;
                                    break;
                                case 3:
                                    itemcode41 = "DC73";
                                    days39 = -1;
                                    break;
                                case 4:
                                    itemcode41 = "DE33";
                                    days39 = 30;
                                    break;
                                case 5:
                                    itemcode41 = "DE33";
                                    days39 = -1;
                                    break;
                                case 6:
                                    itemcode41 = "DE30";
                                    days39 = -1;
                                    break;
                                case 7:
                                    itemcode41 = "DE30";
                                    days39 = -1;
                                    break;
                                case 8:
                                    itemcode41 = "DE28";
                                    days39 = -1;
                                    break;
                                case 9:
                                    itemcode41 = "DE28";
                                    days39 = 30;
                                    break;
                                case 10:
                                    itemcode41 = "DG45";
                                    days39 = 30;
                                    break;
                                case 11:
                                    itemcode41 = "DG45";
                                    days39 = -1;
                                    break;
                                case 12:
                                    itemcode41 = "DG22";
                                    days39 = 30;
                                    break;
                                case 13:
                                    itemcode41 = "DG22";
                                    days39 = -1;
                                    break;
                                case 14:
                                    itemcode41 = "DG24";
                                    days39 = 7;
                                    break;
                                case 15:
                                    itemcode41 = "DG24";
                                    days39 = 7;
                                    break;
                                case 16:
                                    itemcode41 = "DG28";
                                    days39 = -1;
                                    break;
                                case 17:
                                    itemcode41 = "DG28";
                                    days39 = 30;
                                    break;
                                case 18:
                                    itemcode41 = "DG50";
                                    days39 = -1;
                                    break;
                                case 19:
                                    itemcode41 = "DG50";
                                    days39 = 30;
                                    break;
                                case 20:
                                    itemcode41 = "CD01";
                                    days39 = 7;
                                    break;
                                case 21:
                                    itemcode41 = "CD02";
                                    days39 = 7;
                                    break;
                                case 22:
                                    itemcode41 = "CD03";
                                    days39 = 7;
                                    break;
                                case 23:
                                    itemcode41 = "CD04";
                                    days39 = 7;
                                    break;
                                case 24:
                                    itemcode41 = "CD05";
                                    days39 = 7;
                                    break;
                                case 25:
                                    itemcode41 = "CD06";
                                    days39 = 7;
                                    break;
                                case 26:
                                    itemcode41 = "CD07";
                                    days39 = 7;
                                    break;
                                case 27:
                                    itemcode41 = "CK02";
                                    days39 = 1;
                                    break;
                                case 28:
                                    itemcode41 = "CB09";
                                    days39 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode41, days39);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode41, days39));
                            return;
                        case "CR48":
                            int num48 = Game_Server.Generic.random(0, 6);
                            int days40 = 1;
                            string itemcode42 = (string)null;
                            switch (num48)
                            {
                                case 0:
                                    itemcode42 = "DJ16";
                                    days40 = 30;
                                    break;
                                case 1:
                                    itemcode42 = "CB09";
                                    days40 = 7;
                                    break;
                                case 2:
                                    itemcode42 = "CZ81";
                                    days40 = 7;
                                    break;
                                case 3:
                                    itemcode42 = "CZ84";
                                    days40 = 7;
                                    break;
                                case 4:
                                    itemcode42 = "DF04";
                                    days40 = -1;
                                    break;
                                case 5:
                                    itemcode42 = "DF71";
                                    days40 = 7;
                                    break;
                                case 6:
                                    itemcode42 = "DJ16";
                                    days40 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode42, days40);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode42, days40));
                            return;
                        case "CR49":
                            int num49 = Game_Server.Generic.random(0, 5);
                            int days41 = 1;
                            string itemcode43 = (string)null;
                            switch (num49)
                            {
                                case 0:
                                    itemcode43 = "DF66";
                                    days41 = 30;
                                    break;
                                case 1:
                                    itemcode43 = "DF66";
                                    days41 = -1;
                                    break;
                                case 2:
                                    itemcode43 = "DC94";
                                    days41 = 7;
                                    break;
                                case 3:
                                    itemcode43 = "DG61";
                                    days41 = 7;
                                    break;
                                case 4:
                                    itemcode43 = "DF67";
                                    days41 = 7;
                                    break;
                                case 5:
                                    itemcode43 = "DE09";
                                    days41 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode43, days41);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode43, days41));
                            return;
                        case "CR50":
                            int num50 = Game_Server.Generic.random(0, 5);
                            int days42 = 1;
                            string itemcode44 = (string)null;
                            switch (num50)
                            {
                                case 0:
                                    itemcode44 = "DE25";
                                    days42 = 30;
                                    break;
                                case 1:
                                    itemcode44 = "CZ84";
                                    days42 = 1;
                                    break;
                                case 2:
                                    itemcode44 = "CB09";
                                    days42 = 1;
                                    break;
                                case 3:
                                    itemcode44 = "CZ81";
                                    days42 = 1;
                                    break;
                                case 4:
                                    itemcode44 = "CZ84";
                                    days42 = 30;
                                    break;
                                case 5:
                                    itemcode44 = "CF01";
                                    days42 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode44, days42);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode44, days42));
                            return;
                        case "CR51":
                            int num51 = Game_Server.Generic.random(0, 5);
                            int days43 = 1;
                            string itemcode45 = (string)null;
                            switch (num51)
                            {
                                case 0:
                                    itemcode45 = "DF54";
                                    days43 = 30;
                                    break;
                                case 1:
                                    itemcode45 = "CZ85";
                                    days43 = 1;
                                    break;
                                case 2:
                                    itemcode45 = "CB09";
                                    days43 = 1;
                                    break;
                                case 3:
                                    itemcode45 = "CZ81";
                                    days43 = 1;
                                    break;
                                case 4:
                                    itemcode45 = "CZ84";
                                    days43 = 30;
                                    break;
                                case 5:
                                    itemcode45 = "CF01";
                                    days43 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode45, days43);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode45, days43));
                            return;
                        case "CR52":
                            int num52 = Game_Server.Generic.random(0, 5);
                            int days44 = 1;
                            string itemcode46 = (string)null;
                            switch (num52)
                            {
                                case 0:
                                    itemcode46 = "DF53";
                                    days44 = 30;
                                    break;
                                case 1:
                                    itemcode46 = "DF53";
                                    days44 = 7;
                                    break;
                                case 2:
                                    itemcode46 = "DF47";
                                    days44 = 7;
                                    break;
                                case 3:
                                    itemcode46 = "DF25";
                                    days44 = 7;
                                    break;
                                case 4:
                                    itemcode46 = "DF95";
                                    days44 = 7;
                                    break;
                                case 5:
                                    itemcode46 = "DF14";
                                    days44 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode46, days44);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode46, days44));
                            return;
                        case "CR53":
                            int num53 = Game_Server.Generic.random(0, 13);
                            int days45 = 1;
                            string itemcode47 = (string)null;
                            switch (num53)
                            {
                                case 0:
                                    itemcode47 = "DJ25";
                                    days45 = 90;
                                    break;
                                case 1:
                                    itemcode47 = "DJ25";
                                    days45 = -1;
                                    break;
                                case 2:
                                    itemcode47 = "DJ31";
                                    days45 = -1;
                                    break;
                                case 3:
                                    itemcode47 = "DJ31";
                                    days45 = 90;
                                    break;
                                case 4:
                                    itemcode47 = "DJ32";
                                    days45 = -1;
                                    break;
                                case 5:
                                    itemcode47 = "DJ32";
                                    days45 = 90;
                                    break;
                                case 6:
                                    itemcode47 = "DJ32";
                                    days45 = -1;
                                    break;
                                case 7:
                                    itemcode47 = "DG54";
                                    days45 = 90;
                                    break;
                                case 8:
                                    itemcode47 = "DG54";
                                    days45 = -1;
                                    break;
                                case 9:
                                    itemcode47 = "DG55";
                                    days45 = 90;
                                    break;
                                case 10:
                                    itemcode47 = "DG55";
                                    days45 = -1;
                                    break;
                                case 11:
                                    itemcode47 = "DG59";
                                    days45 = 90;
                                    break;
                                case 12:
                                    itemcode47 = "DG59";
                                    days45 = -1;
                                    break;
                                case 13:
                                    itemcode47 = "DG71";
                                    days45 = 90;
                                    break;
                                case 14:
                                    itemcode47 = "DG71";
                                    days45 = -1;
                                    break;
                                case 15:
                                    itemcode47 = "DG72";
                                    days45 = 90;
                                    break;
                                case 16:
                                    itemcode47 = "DG72";
                                    days45 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode47, days45);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode47, days45));
                            return;
                        case "CR54":
                            int num54 = Game_Server.Generic.random(0, 13);
                            int days46 = 1;
                            string itemcode48 = (string)null;
                            switch (num54)
                            {
                                case 0:
                                    itemcode48 = "DF80";
                                    days46 = 90;
                                    break;
                                case 1:
                                    itemcode48 = "DF80";
                                    days46 = -1;
                                    break;
                                case 2:
                                    itemcode48 = "DF81";
                                    days46 = -1;
                                    break;
                                case 3:
                                    itemcode48 = "DF81";
                                    days46 = 90;
                                    break;
                                case 4:
                                    itemcode48 = "DF58";
                                    days46 = -1;
                                    break;
                                case 5:
                                    itemcode48 = "DF58";
                                    days46 = 90;
                                    break;
                                case 6:
                                    itemcode48 = "DF69";
                                    days46 = -1;
                                    break;
                                case 7:
                                    itemcode48 = "DF69";
                                    days46 = 90;
                                    break;
                                case 8:
                                    itemcode48 = "DF75";
                                    days46 = -1;
                                    break;
                                case 9:
                                    itemcode48 = "DF75";
                                    days46 = 90;
                                    break;
                                case 10:
                                    itemcode48 = "DF79";
                                    days46 = -1;
                                    break;
                                case 11:
                                    itemcode48 = "DF79";
                                    days46 = 90;
                                    break;
                                case 12:
                                    itemcode48 = "DF89";
                                    days46 = -1;
                                    break;
                                case 13:
                                    itemcode48 = "DF89";
                                    days46 = 90;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode48, days46);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode48, days46));
                            return;
                        case "CR55":
                            int num55 = Game_Server.Generic.random(0, 93);
                            int days47 = 1;
                            string itemcode49 = (string)null;
                            switch (num55)
                            {
                                case 0:
                                    itemcode49 = "DC87";
                                    days47 = 90;
                                    break;
                                case 1:
                                    itemcode49 = "DC87";
                                    days47 = 90;
                                    break;
                                case 2:
                                    itemcode49 = "DC88";
                                    days47 = 90;
                                    break;
                                case 3:
                                    itemcode49 = "DC96";
                                    days47 = 90;
                                    break;
                                case 4:
                                    itemcode49 = "DC89";
                                    days47 = 90;
                                    break;
                                case 5:
                                    itemcode49 = "DC90";
                                    days47 = 90;
                                    break;
                                case 6:
                                    itemcode49 = "DC97";
                                    days47 = 90;
                                    break;
                                case 7:
                                    itemcode49 = "DE17";
                                    days47 = 90;
                                    break;
                                case 8:
                                    itemcode49 = "DE18";
                                    days47 = 90;
                                    break;
                                case 9:
                                    itemcode49 = "DE19";
                                    days47 = 90;
                                    break;
                                case 10:
                                    itemcode49 = "DE20";
                                    days47 = 90;
                                    break;
                                case 11:
                                    itemcode49 = "DE21";
                                    days47 = 90;
                                    break;
                                case 12:
                                    itemcode49 = "DE51";
                                    days47 = 90;
                                    break;
                                case 14:
                                    itemcode49 = "DC87";
                                    days47 = -1;
                                    break;
                                case 15:
                                    itemcode49 = "DC88";
                                    days47 = -1;
                                    break;
                                case 16:
                                    itemcode49 = "DC96";
                                    days47 = -1;
                                    break;
                                case 17:
                                    itemcode49 = "DC89";
                                    days47 = -1;
                                    break;
                                case 18:
                                    itemcode49 = "DC90";
                                    days47 = -1;
                                    break;
                                case 19:
                                    itemcode49 = "DC97";
                                    days47 = -1;
                                    break;
                                case 20:
                                    itemcode49 = "DE17";
                                    days47 = -1;
                                    break;
                                case 21:
                                    itemcode49 = "DE18";
                                    days47 = -1;
                                    break;
                                case 22:
                                    itemcode49 = "DE19";
                                    days47 = -1;
                                    break;
                                case 23:
                                    itemcode49 = "DE20";
                                    days47 = -1;
                                    break;
                                case 24:
                                    itemcode49 = "DE21";
                                    days47 = -1;
                                    break;
                                case 25:
                                    itemcode49 = "DE51";
                                    days47 = -1;
                                    break;
                                case 26:
                                    itemcode49 = "DC21";
                                    days47 = -1;
                                    break;
                                case 27:
                                    itemcode49 = "DC21";
                                    days47 = 90;
                                    break;
                                case 28:
                                    itemcode49 = "DC22";
                                    days47 = -1;
                                    break;
                                case 29:
                                    itemcode49 = "DC22";
                                    days47 = 90;
                                    break;
                                case 30:
                                    itemcode49 = "DC23";
                                    days47 = 90;
                                    break;
                                case 31:
                                    itemcode49 = "DC23";
                                    days47 = -1;
                                    break;
                                case 32:
                                    itemcode49 = "DC24";
                                    days47 = 90;
                                    break;
                                case 33:
                                    itemcode49 = "DC24";
                                    days47 = -1;
                                    break;
                                case 34:
                                    itemcode49 = "DC25";
                                    days47 = -1;
                                    break;
                                case 35:
                                    itemcode49 = "DC25";
                                    days47 = 90;
                                    break;
                                case 36:
                                    itemcode49 = "DC26";
                                    days47 = 90;
                                    break;
                                case 37:
                                    itemcode49 = "DC26";
                                    days47 = -1;
                                    break;
                                case 38:
                                    itemcode49 = "DC27";
                                    days47 = 90;
                                    break;
                                case 39:
                                    itemcode49 = "DC27";
                                    days47 = -1;
                                    break;
                                case 40:
                                    itemcode49 = "DC28";
                                    days47 = -1;
                                    break;
                                case 41:
                                    itemcode49 = "DC28";
                                    days47 = 90;
                                    break;
                                case 42:
                                    itemcode49 = "DC29";
                                    days47 = 90;
                                    break;
                                case 43:
                                    itemcode49 = "DC29";
                                    days47 = -1;
                                    break;
                                case 44:
                                    itemcode49 = "DC30";
                                    days47 = 90;
                                    break;
                                case 45:
                                    itemcode49 = "DC30";
                                    days47 = -1;
                                    break;
                                case 46:
                                    itemcode49 = "DC41";
                                    days47 = 90;
                                    break;
                                case 47:
                                    itemcode49 = "DC41";
                                    days47 = -1;
                                    break;
                                case 48:
                                    itemcode49 = "DC42";
                                    days47 = 90;
                                    break;
                                case 49:
                                    itemcode49 = "DC42";
                                    days47 = -1;
                                    break;
                                case 50:
                                    itemcode49 = "DC43";
                                    days47 = 90;
                                    break;
                                case 51:
                                    itemcode49 = "DC44";
                                    days47 = -1;
                                    break;
                                case 52:
                                    itemcode49 = "DC44";
                                    days47 = 90;
                                    break;
                                case 53:
                                    itemcode49 = "DC45";
                                    days47 = -1;
                                    break;
                                case 54:
                                    itemcode49 = "DC45";
                                    days47 = 90;
                                    break;
                                case 55:
                                    itemcode49 = "DC44";
                                    days47 = -1;
                                    break;
                                case 56:
                                    itemcode49 = "DC45";
                                    days47 = 90;
                                    break;
                                case 57:
                                    itemcode49 = "DC45";
                                    days47 = -1;
                                    break;
                                case 58:
                                    itemcode49 = "DC46";
                                    days47 = 90;
                                    break;
                                case 59:
                                    itemcode49 = "DC46";
                                    days47 = -1;
                                    break;
                                case 60:
                                    itemcode49 = "DC47";
                                    days47 = 90;
                                    break;
                                case 61:
                                    itemcode49 = "DC47";
                                    days47 = -1;
                                    break;
                                case 62:
                                    itemcode49 = "DC48";
                                    days47 = 90;
                                    break;
                                case 63:
                                    itemcode49 = "DC48";
                                    days47 = -1;
                                    break;
                                case 64:
                                    itemcode49 = "DC49";
                                    days47 = 90;
                                    break;
                                case 65:
                                    itemcode49 = "DC49";
                                    days47 = -1;
                                    break;
                                case 66:
                                    itemcode49 = "DC50";
                                    days47 = 90;
                                    break;
                                case 67:
                                    itemcode49 = "DC50";
                                    days47 = -1;
                                    break;
                                case 68:
                                    itemcode49 = "DC51";
                                    days47 = 90;
                                    break;
                                case 69:
                                    itemcode49 = "DC52";
                                    days47 = 90;
                                    break;
                                case 70:
                                    itemcode49 = "DC51";
                                    days47 = -1;
                                    break;
                                case 71:
                                    itemcode49 = "DC52";
                                    days47 = -1;
                                    break;
                                case 72:
                                    itemcode49 = "DC53";
                                    days47 = 90;
                                    break;
                                case 73:
                                    itemcode49 = "DC53";
                                    days47 = -1;
                                    break;
                                case 74:
                                    itemcode49 = "DC54";
                                    days47 = -1;
                                    break;
                                case 75:
                                    itemcode49 = "DC54";
                                    days47 = 90;
                                    break;
                                case 76:
                                    itemcode49 = "DC55";
                                    days47 = 90;
                                    break;
                                case 77:
                                    itemcode49 = "DC56";
                                    days47 = 90;
                                    break;
                                case 78:
                                    itemcode49 = "DC55";
                                    days47 = -1;
                                    break;
                                case 79:
                                    itemcode49 = "DC56";
                                    days47 = -1;
                                    break;
                                case 80:
                                    itemcode49 = "DC57";
                                    days47 = 90;
                                    break;
                                case 81:
                                    itemcode49 = "DC57";
                                    days47 = -1;
                                    break;
                                case 82:
                                    itemcode49 = "DC58";
                                    days47 = 90;
                                    break;
                                case 83:
                                    itemcode49 = "DC58";
                                    days47 = -1;
                                    break;
                                case 84:
                                    itemcode49 = "DC59";
                                    days47 = 90;
                                    break;
                                case 85:
                                    itemcode49 = "DC59";
                                    days47 = -1;
                                    break;
                                case 86:
                                    itemcode49 = "DC60";
                                    days47 = 90;
                                    break;
                                case 87:
                                    itemcode49 = "DC60";
                                    days47 = -1;
                                    break;
                                case 88:
                                    itemcode49 = "DC66";
                                    days47 = 90;
                                    break;
                                case 89:
                                    itemcode49 = "DC66";
                                    days47 = -1;
                                    break;
                                case 90:
                                    itemcode49 = "DC62";
                                    days47 = 90;
                                    break;
                                case 91:
                                    itemcode49 = "DC62";
                                    days47 = -1;
                                    break;
                                case 92:
                                    itemcode49 = "DC63";
                                    days47 = 90;
                                    break;
                                case 93:
                                    itemcode49 = "DC63";
                                    days47 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode49, days47);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode49, days47));
                            return;
                        case "CR56":
                            int num56 = Game_Server.Generic.random(0, 15);
                            int days48 = 1;
                            string itemcode50 = (string)null;
                            switch (num56)
                            {
                                case 0:
                                    itemcode50 = "DA23";
                                    days48 = 7;
                                    break;
                                case 1:
                                    itemcode50 = "DA24";
                                    days48 = 7;
                                    break;
                                case 2:
                                    itemcode50 = "DA25";
                                    days48 = 7;
                                    break;
                                case 3:
                                    itemcode50 = "DA26";
                                    days48 = 7;
                                    break;
                                case 4:
                                    itemcode50 = "DA27";
                                    days48 = 7;
                                    break;
                                case 5:
                                    itemcode50 = "DA28";
                                    days48 = 7;
                                    break;
                                case 6:
                                    itemcode50 = "DA29";
                                    days48 = 7;
                                    break;
                                case 7:
                                    itemcode50 = "DA30";
                                    days48 = 7;
                                    break;
                                case 8:
                                    itemcode50 = "DA31";
                                    days48 = 7;
                                    break;
                                case 9:
                                    itemcode50 = "DA32";
                                    days48 = 7;
                                    break;
                                case 10:
                                    itemcode50 = "DA33";
                                    days48 = 7;
                                    break;
                                case 11:
                                    itemcode50 = "DA34";
                                    days48 = 7;
                                    break;
                                case 12:
                                    itemcode50 = "DA35";
                                    days48 = 7;
                                    break;
                                case 13:
                                    itemcode50 = "DA36";
                                    days48 = 7;
                                    break;
                                case 14:
                                    itemcode50 = "DA37";
                                    days48 = 7;
                                    break;
                                case 15:
                                    itemcode50 = "DA38";
                                    days48 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode50, days48);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode50, days48));
                            return;
                        case "CR57":
                            int num57 = Game_Server.Generic.random(0, 21);
                            int days49 = 1;
                            string itemcode51 = (string)null;
                            switch (num57)
                            {
                                case 0:
                                    itemcode51 = "DF51";
                                    days49 = 30;
                                    break;
                                case 1:
                                    itemcode51 = "DF51";
                                    days49 = -1;
                                    break;
                                case 2:
                                    itemcode51 = "DJ23";
                                    days49 = -1;
                                    break;
                                case 3:
                                    itemcode51 = "DJ23";
                                    days49 = 30;
                                    break;
                                case 4:
                                    itemcode51 = "DC86";
                                    days49 = 30;
                                    break;
                                case 5:
                                    itemcode51 = "DC86";
                                    days49 = -1;
                                    break;
                                case 6:
                                    itemcode51 = "DG50";
                                    days49 = -1;
                                    break;
                                case 7:
                                    itemcode51 = "DG50";
                                    days49 = 30;
                                    break;
                                case 8:
                                    itemcode51 = "DB30";
                                    days49 = -1;
                                    break;
                                case 10:
                                    itemcode51 = "DA71";
                                    days49 = 30;
                                    break;
                                case 11:
                                    itemcode51 = "DA71";
                                    days49 = -1;
                                    break;
                                case 12:
                                    itemcode51 = "DB38";
                                    days49 = 30;
                                    break;
                                case 13:
                                    itemcode51 = "DB38";
                                    days49 = -1;
                                    break;
                                case 14:
                                    itemcode51 = "DF83";
                                    days49 = -1;
                                    break;
                                case 15:
                                    itemcode51 = "DF83";
                                    days49 = 30;
                                    break;
                                case 16:
                                    itemcode51 = "DE56";
                                    days49 = 30;
                                    break;
                                case 17:
                                    itemcode51 = "DE56";
                                    days49 = -1;
                                    break;
                                case 18:
                                    itemcode51 = "DG75";
                                    days49 = 30;
                                    break;
                                case 19:
                                    itemcode51 = "DG75";
                                    days49 = -1;
                                    break;
                                case 20:
                                    itemcode51 = "DJ34";
                                    days49 = 30;
                                    break;
                                case 21:
                                    itemcode51 = "DJ34";
                                    days49 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode51, days49);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode51, days49));
                            return;
                        case "CR58":
                            int num58 = Game_Server.Generic.random(0, 11);
                            int days50 = 1;
                            string itemcode52 = (string)null;
                            switch (num58)
                            {
                                case 0:
                                    itemcode52 = "DF36";
                                    days50 = -1;
                                    break;
                                case 1:
                                    itemcode52 = "DF34";
                                    days50 = -1;
                                    break;
                                case 2:
                                    itemcode52 = "DF65";
                                    days50 = -1;
                                    break;
                                case 3:
                                    itemcode52 = "DF93";
                                    days50 = 30;
                                    break;
                                case 4:
                                    itemcode52 = "DF36";
                                    days50 = 30;
                                    break;
                                case 5:
                                    itemcode52 = "DF34";
                                    days50 = 30;
                                    break;
                                case 6:
                                    itemcode52 = "DF65";
                                    days50 = 30;
                                    break;
                                case 7:
                                    itemcode52 = "DF93";
                                    days50 = 30;
                                    break;
                                case 8:
                                    itemcode52 = "DC34";
                                    days50 = 30;
                                    break;
                                case 9:
                                    itemcode52 = "DC34";
                                    days50 = -1;
                                    break;
                                case 10:
                                    itemcode52 = "DC93";
                                    days50 = 30;
                                    break;
                                case 11:
                                    itemcode52 = "DC93";
                                    days50 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode52, days50);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode52, days50));
                            return;
                        case "CR59":
                            int num59 = Game_Server.Generic.random(0, 15);
                            int days51 = 1;
                            string itemcode53 = (string)null;
                            switch (num59)
                            {
                                case 0:
                                    itemcode53 = "DF53";
                                    days51 = 30;
                                    break;
                                case 1:
                                    itemcode53 = "DC99";
                                    days51 = 30;
                                    break;
                                case 2:
                                    itemcode53 = "DG52";
                                    days51 = 30;
                                    break;
                                case 3:
                                    itemcode53 = "DF55";
                                    days51 = 30;
                                    break;
                                case 4:
                                    itemcode53 = "DT11";
                                    days51 = 30;
                                    break;
                                case 5:
                                    itemcode53 = "DA75";
                                    days51 = 30;
                                    break;
                                case 6:
                                    itemcode53 = "DB40";
                                    days51 = 30;
                                    break;
                                case 7:
                                    itemcode53 = "DM10";
                                    days51 = 30;
                                    break;
                                case 8:
                                    itemcode53 = "DF53";
                                    days51 = -1;
                                    break;
                                case 9:
                                    itemcode53 = "DC99";
                                    days51 = -1;
                                    break;
                                case 10:
                                    itemcode53 = "DG52";
                                    days51 = -1;
                                    break;
                                case 11:
                                    itemcode53 = "DF55";
                                    days51 = -1;
                                    break;
                                case 12:
                                    itemcode53 = "DT11";
                                    days51 = -1;
                                    break;
                                case 13:
                                    itemcode53 = "DA75";
                                    days51 = -1;
                                    break;
                                case 14:
                                    itemcode53 = "DB40";
                                    days51 = -1;
                                    break;
                                case 15:
                                    itemcode53 = "DM10";
                                    days51 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode53, days51);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode53, days51));
                            return;
                        case "CR60":
                            int num60 = Game_Server.Generic.random(0, 21);
                            int days52 = 1;
                            string itemcode54 = (string)null;
                            switch (num60)
                            {
                                case 0:
                                    itemcode54 = "DC40";
                                    days52 = -1;
                                    break;
                                case 1:
                                    itemcode54 = "DF14";
                                    days52 = -1;
                                    break;
                                case 2:
                                    itemcode54 = "DJ07";
                                    days52 = -1;
                                    break;
                                case 3:
                                    itemcode54 = "DG37";
                                    days52 = -1;
                                    break;
                                case 4:
                                    itemcode54 = "DJ15";
                                    days52 = -1;
                                    break;
                                case 5:
                                    itemcode54 = "DC95";
                                    days52 = -1;
                                    break;
                                case 6:
                                    itemcode54 = "DF26";
                                    days52 = -1;
                                    break;
                                case 7:
                                    itemcode54 = "DG36";
                                    days52 = -1;
                                    break;
                                case 8:
                                    itemcode54 = "DC72";
                                    days52 = -1;
                                    break;
                                case 9:
                                    itemcode54 = "DF27";
                                    days52 = -1;
                                    break;
                                case 10:
                                    itemcode54 = "DC40";
                                    days52 = 30;
                                    break;
                                case 11:
                                    itemcode54 = "DF14";
                                    days52 = 30;
                                    break;
                                case 12:
                                    itemcode54 = "DJ07";
                                    days52 = -1;
                                    break;
                                case 13:
                                    itemcode54 = "DG37";
                                    days52 = 30;
                                    break;
                                case 14:
                                    itemcode54 = "DJ15";
                                    days52 = 30;
                                    break;
                                case 15:
                                    itemcode54 = "DC95";
                                    days52 = 30;
                                    break;
                                case 16:
                                    itemcode54 = "DF26";
                                    days52 = 30;
                                    break;
                                case 17:
                                    itemcode54 = "DG36";
                                    days52 = 30;
                                    break;
                                case 18:
                                    itemcode54 = "DC72";
                                    days52 = 30;
                                    break;
                                case 19:
                                    itemcode54 = "DF27";
                                    days52 = 30;
                                    break;
                                case 20:
                                    itemcode54 = "CZ85";
                                    days52 = 1;
                                    break;
                                case 21:
                                    itemcode54 = "CF01";
                                    days52 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode54, days52);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode54, days52));
                            return;
                        case "CR61":
                            int num61 = Game_Server.Generic.random(0, 23);
                            int days53 = 1;
                            string itemcode55 = (string)null;
                            switch (num61)
                            {
                                case 0:
                                    itemcode55 = "DD32";
                                    days53 = -1;
                                    break;
                                case 1:
                                    itemcode55 = "DD32";
                                    days53 = 30;
                                    break;
                                case 2:
                                    itemcode55 = "DI06";
                                    days53 = -1;
                                    break;
                                case 3:
                                    itemcode55 = "DI06";
                                    days53 = 30;
                                    break;
                                case 4:
                                    itemcode55 = "DG26";
                                    days53 = -1;
                                    break;
                                case 5:
                                    itemcode55 = "DG26";
                                    days53 = 30;
                                    break;
                                case 6:
                                    itemcode55 = "DK31";
                                    days53 = -1;
                                    break;
                                case 7:
                                    itemcode55 = "DK31";
                                    days53 = 30;
                                    break;
                                case 8:
                                    itemcode55 = "GF16";
                                    days53 = 30;
                                    break;
                                case 9:
                                    itemcode55 = "DF86";
                                    days53 = -1;
                                    break;
                                case 10:
                                    itemcode55 = "DF86";
                                    days53 = 30;
                                    break;
                                case 11:
                                    itemcode55 = "DG79";
                                    days53 = -1;
                                    break;
                                case 12:
                                    itemcode55 = "DG79";
                                    days53 = 30;
                                    break;
                                case 13:
                                    itemcode55 = "DE59";
                                    days53 = -1;
                                    break;
                                case 14:
                                    itemcode55 = "DE59";
                                    days53 = 30;
                                    break;
                                case 15:
                                    itemcode55 = "DJ37";
                                    days53 = 30;
                                    break;
                                case 16:
                                    itemcode55 = "DF37";
                                    days53 = -1;
                                    break;
                                case 17:
                                    itemcode55 = "GF16";
                                    days53 = -1;
                                    break;
                                case 18:
                                    itemcode55 = "GG04";
                                    days53 = 30;
                                    break;
                                case 19:
                                    itemcode55 = "GG04";
                                    days53 = -1;
                                    break;
                                case 20:
                                    itemcode55 = "DE87";
                                    days53 = 30;
                                    break;
                                case 21:
                                    itemcode55 = "DE87";
                                    days53 = -1;
                                    break;
                                case 22:
                                    itemcode55 = "DJ59";
                                    days53 = 30;
                                    break;
                                case 23:
                                    itemcode55 = "DJ59";
                                    days53 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode55, days53);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode55, days53));
                            return;
                        case "CR62":
                            int num62 = Game_Server.Generic.random(0, 2);
                            int days54 = 1;
                            string itemcode56 = (string)null;
                            switch (num62)
                            {
                                case 0:
                                    itemcode56 = "DF89";
                                    days54 = 30;
                                    break;
                                case 1:
                                    itemcode56 = "DF89";
                                    days54 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode56, days54);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode56, days54));
                            return;
                        case "CR63":
                            int num63 = Game_Server.Generic.random(0, 58);
                            int days55 = 1;
                            string itemcode57 = (string)null;
                            switch (num63)
                            {
                                case 0:
                                    itemcode57 = "DB10";
                                    days55 = -1;
                                    break;
                                case 1:
                                    itemcode57 = "DB10";
                                    days55 = 30;
                                    break;
                                case 2:
                                    itemcode57 = "DB10";
                                    days55 = -1;
                                    break;
                                case 3:
                                    itemcode57 = "DC33";
                                    days55 = 30;
                                    break;
                                case 4:
                                    itemcode57 = "DC33";
                                    days55 = -1;
                                    break;
                                case 5:
                                    itemcode57 = "DF35";
                                    days55 = 30;
                                    break;
                                case 6:
                                    itemcode57 = "DF35";
                                    days55 = -1;
                                    break;
                                case 7:
                                    itemcode57 = "DG13";
                                    days55 = 30;
                                    break;
                                case 8:
                                    itemcode57 = "DG13";
                                    days55 = -1;
                                    break;
                                case 9:
                                    itemcode57 = "DJ33";
                                    days55 = -1;
                                    break;
                                case 10:
                                    itemcode57 = "DJ33";
                                    days55 = 30;
                                    break;
                                case 11:
                                    itemcode57 = "DC64";
                                    days55 = 30;
                                    break;
                                case 12:
                                    itemcode57 = "DC64";
                                    days55 = -1;
                                    break;
                                case 13:
                                    itemcode57 = "DB16";
                                    days55 = 30;
                                    break;
                                case 14:
                                    itemcode57 = "DB16";
                                    days55 = -1;
                                    break;
                                case 15:
                                    itemcode57 = "DF37";
                                    days55 = 30;
                                    break;
                                case 16:
                                    itemcode57 = "DF37";
                                    days55 = -1;
                                    break;
                                case 17:
                                    itemcode57 = "DC39";
                                    days55 = -1;
                                    break;
                                case 18:
                                    itemcode57 = "DF95";
                                    days55 = 30;
                                    break;
                                case 19:
                                    itemcode57 = "DC39";
                                    days55 = 30;
                                    break;
                                case 20:
                                    itemcode57 = "DF95";
                                    days55 = -1;
                                    break;
                                case 21:
                                    itemcode57 = "DE07";
                                    days55 = 30;
                                    break;
                                case 22:
                                    itemcode57 = "DE07";
                                    days55 = 30;
                                    break;
                                case 23:
                                    itemcode57 = "DA45";
                                    days55 = -1;
                                    break;
                                case 24:
                                    itemcode57 = "DF77";
                                    days55 = -1;
                                    break;
                                case 25:
                                    itemcode57 = "DF77";
                                    days55 = 30;
                                    break;
                                case 26:
                                    itemcode57 = "DB63";
                                    days55 = 30;
                                    break;
                                case 27:
                                    itemcode57 = "DH09";
                                    days55 = 30;
                                    break;
                                case 28:
                                    itemcode57 = "DB63";
                                    days55 = -1;
                                    break;
                                case 29:
                                    itemcode57 = "DH09";
                                    days55 = -1;
                                    break;
                                case 30:
                                    itemcode57 = "DF60";
                                    days55 = 30;
                                    break;
                                case 31:
                                    itemcode57 = "DF60";
                                    days55 = -1;
                                    break;
                                case 32:
                                    itemcode57 = "DE30";
                                    days55 = 30;
                                    break;
                                case 33:
                                    itemcode57 = "DE30";
                                    days55 = -1;
                                    break;
                                case 34:
                                    itemcode57 = "DE35";
                                    days55 = 30;
                                    break;
                                case 35:
                                    itemcode57 = "DE35";
                                    days55 = -1;
                                    break;
                                case 36:
                                    itemcode57 = "DE37";
                                    days55 = 30;
                                    break;
                                case 37:
                                    itemcode57 = "DE37";
                                    days55 = -1;
                                    break;
                                case 38:
                                    itemcode57 = "DE38";
                                    days55 = 30;
                                    break;
                                case 39:
                                    itemcode57 = "DE38";
                                    days55 = -1;
                                    break;
                                case 40:
                                    itemcode57 = "DE40";
                                    days55 = 30;
                                    break;
                                case 41:
                                    itemcode57 = "DE40";
                                    days55 = -1;
                                    break;
                                case 42:
                                    itemcode57 = "DE41";
                                    days55 = 30;
                                    break;
                                case 43:
                                    itemcode57 = "DE41";
                                    days55 = -1;
                                    break;
                                case 44:
                                    itemcode57 = "DE43";
                                    days55 = 30;
                                    break;
                                case 45:
                                    itemcode57 = "DE43";
                                    days55 = -1;
                                    break;
                                case 46:
                                    itemcode57 = "DE52";
                                    days55 = 30;
                                    break;
                                case 47:
                                    itemcode57 = "DE52";
                                    days55 = -1;
                                    break;
                                case 48:
                                    itemcode57 = "DA45";
                                    days55 = 30;
                                    break;
                                case 49:
                                    itemcode57 = "DF77";
                                    days55 = -1;
                                    break;
                                case 50:
                                    itemcode57 = "DF77";
                                    days55 = 30;
                                    break;
                                case 51:
                                    itemcode57 = "DA72";
                                    days55 = -1;
                                    break;
                                case 52:
                                    itemcode57 = "DA72";
                                    days55 = 30;
                                    break;
                                case 53:
                                    itemcode57 = "DE53";
                                    days55 = -1;
                                    break;
                                case 54:
                                    itemcode57 = "DE53";
                                    days55 = 30;
                                    break;
                                case 55:
                                    itemcode57 = "DN14";
                                    days55 = -1;
                                    break;
                                case 56:
                                    itemcode57 = "DN14";
                                    days55 = 30;
                                    break;
                                case 57:
                                    itemcode57 = "DB49";
                                    days55 = -1;
                                    break;
                                case 58:
                                    itemcode57 = "DB49";
                                    days55 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode57, days55);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode57, days55));
                            return;
                        case "CR64":
                            int num64 = Game_Server.Generic.random(0, 29);
                            int days56 = 1;
                            string itemcode58 = (string)null;
                            switch (num64)
                            {
                                case 0:
                                    itemcode58 = "DB22";
                                    days56 = -1;
                                    break;
                                case 1:
                                    itemcode58 = "DB22";
                                    days56 = 30;
                                    break;
                                case 2:
                                    itemcode58 = "DB22";
                                    days56 = 90;
                                    break;
                                case 3:
                                    itemcode58 = "DF24";
                                    days56 = 30;
                                    break;
                                case 4:
                                    itemcode58 = "DF24";
                                    days56 = -1;
                                    break;
                                case 5:
                                    itemcode58 = "DF24";
                                    days56 = 90;
                                    break;
                                case 6:
                                    itemcode58 = "DC80";
                                    days56 = 90;
                                    break;
                                case 7:
                                    itemcode58 = "DC80";
                                    days56 = 30;
                                    break;
                                case 8:
                                    itemcode58 = "DC80";
                                    days56 = -1;
                                    break;
                                case 9:
                                    itemcode58 = "DF34";
                                    days56 = -1;
                                    break;
                                case 10:
                                    itemcode58 = "DF34";
                                    days56 = 30;
                                    break;
                                case 11:
                                    itemcode58 = "DF34";
                                    days56 = 90;
                                    break;
                                case 12:
                                    itemcode58 = "DF42";
                                    days56 = -1;
                                    break;
                                case 13:
                                    itemcode58 = "DF42";
                                    days56 = 30;
                                    break;
                                case 14:
                                    itemcode58 = "DF42";
                                    days56 = 90;
                                    break;
                                case 15:
                                    itemcode58 = "DC82";
                                    days56 = 30;
                                    break;
                                case 16:
                                    itemcode58 = "DC82";
                                    days56 = -1;
                                    break;
                                case 17:
                                    itemcode58 = "DC82";
                                    days56 = 90;
                                    break;
                                case 18:
                                    itemcode58 = "DG46";
                                    days56 = -1;
                                    break;
                                case 19:
                                    itemcode58 = "DG46";
                                    days56 = 30;
                                    break;
                                case 20:
                                    itemcode58 = "DG46";
                                    days56 = 90;
                                    break;
                                case 21:
                                    itemcode58 = "DF76";
                                    days56 = 30;
                                    break;
                                case 22:
                                    itemcode58 = "DF76";
                                    days56 = 90;
                                    break;
                                case 23:
                                    itemcode58 = "DF76";
                                    days56 = -1;
                                    break;
                                case 24:
                                    itemcode58 = "DG60";
                                    days56 = -1;
                                    break;
                                case 25:
                                    itemcode58 = "DG60";
                                    days56 = 30;
                                    break;
                                case 26:
                                    itemcode58 = "DG60";
                                    days56 = 90;
                                    break;
                                case 27:
                                    itemcode58 = "DJ28";
                                    days56 = 30;
                                    break;
                                case 28:
                                    itemcode58 = "DJ28";
                                    days56 = 90;
                                    break;
                                case 29:
                                    itemcode58 = "DJ28";
                                    days56 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode58, days56);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode58, days56));
                            return;
                        case "CR65":
                            int num65 = Game_Server.Generic.random(0, 50);
                            int days57 = 1;
                            string itemcode59 = (string)null;
                            switch (num65)
                            {
                                case 0:
                                    itemcode59 = "DC26";
                                    days57 = -1;
                                    break;
                                case 1:
                                    itemcode59 = "D812";
                                    days57 = -1;
                                    break;
                                case 2:
                                    itemcode59 = "DD10";
                                    days57 = -1;
                                    break;
                                case 3:
                                    itemcode59 = "DE28";
                                    days57 = -1;
                                    break;
                                case 4:
                                    itemcode59 = "DF58";
                                    days57 = -1;
                                    break;
                                case 5:
                                    itemcode59 = "DF59";
                                    days57 = -1;
                                    break;
                                case 6:
                                    itemcode59 = "DG54";
                                    days57 = -1;
                                    break;
                                case 7:
                                    itemcode59 = "DJ25";
                                    days57 = -1;
                                    break;
                                case 8:
                                    itemcode59 = "DF81";
                                    days57 = -1;
                                    break;
                                case 9:
                                    itemcode59 = "DF91";
                                    days57 = -1;
                                    break;
                                case 10:
                                    itemcode59 = "DE62";
                                    days57 = -1;
                                    break;
                                case 11:
                                    itemcode59 = "DT15";
                                    days57 = -1;
                                    break;
                                case 12:
                                    itemcode59 = "DB42";
                                    days57 = -1;
                                    break;
                                case 13:
                                    itemcode59 = "GF17";
                                    days57 = -1;
                                    break;
                                case 14:
                                    itemcode59 = "GG05";
                                    days57 = -1;
                                    break;
                                case 15:
                                    itemcode59 = "DH14";
                                    days57 = -1;
                                    break;
                                case 16:
                                    itemcode59 = "DJ60";
                                    days57 = -1;
                                    break;
                                case 17:
                                    itemcode59 = "DC26";
                                    days57 = 90;
                                    break;
                                case 18:
                                    itemcode59 = "D812";
                                    days57 = 90;
                                    break;
                                case 19:
                                    itemcode59 = "DD10";
                                    days57 = 90;
                                    break;
                                case 20:
                                    itemcode59 = "DE28";
                                    days57 = 90;
                                    break;
                                case 21:
                                    itemcode59 = "DF58";
                                    days57 = 90;
                                    break;
                                case 22:
                                    itemcode59 = "DF59";
                                    days57 = 90;
                                    break;
                                case 23:
                                    itemcode59 = "DG54";
                                    days57 = 90;
                                    break;
                                case 24:
                                    itemcode59 = "DJ25";
                                    days57 = 90;
                                    break;
                                case 25:
                                    itemcode59 = "DF81";
                                    days57 = 90;
                                    break;
                                case 26:
                                    itemcode59 = "DF91";
                                    days57 = 90;
                                    break;
                                case 27:
                                    itemcode59 = "DE62";
                                    days57 = 90;
                                    break;
                                case 28:
                                    itemcode59 = "DT15";
                                    days57 = 90;
                                    break;
                                case 29:
                                    itemcode59 = "DB42";
                                    days57 = 90;
                                    break;
                                case 30:
                                    itemcode59 = "GF17";
                                    days57 = 90;
                                    break;
                                case 31:
                                    itemcode59 = "GG05";
                                    days57 = 90;
                                    break;
                                case 32:
                                    itemcode59 = "DH14";
                                    days57 = 90;
                                    break;
                                case 33:
                                    itemcode59 = "DJ60";
                                    days57 = 90;
                                    break;
                                case 34:
                                    itemcode59 = "DC26";
                                    days57 = 30;
                                    break;
                                case 35:
                                    itemcode59 = "D812";
                                    days57 = 30;
                                    break;
                                case 36:
                                    itemcode59 = "DD10";
                                    days57 = 30;
                                    break;
                                case 37:
                                    itemcode59 = "DE28";
                                    days57 = 30;
                                    break;
                                case 38:
                                    itemcode59 = "DF58";
                                    days57 = 30;
                                    break;
                                case 39:
                                    itemcode59 = "DF59";
                                    days57 = 30;
                                    break;
                                case 40:
                                    itemcode59 = "DG54";
                                    days57 = 30;
                                    break;
                                case 41:
                                    itemcode59 = "DJ25";
                                    days57 = 30;
                                    break;
                                case 42:
                                    itemcode59 = "DF81";
                                    days57 = 30;
                                    break;
                                case 43:
                                    itemcode59 = "DF91";
                                    days57 = 30;
                                    break;
                                case 44:
                                    itemcode59 = "DE62";
                                    days57 = 30;
                                    break;
                                case 45:
                                    itemcode59 = "DT15";
                                    days57 = 30;
                                    break;
                                case 46:
                                    itemcode59 = "DB42";
                                    days57 = 30;
                                    break;
                                case 47:
                                    itemcode59 = "GF17";
                                    days57 = 30;
                                    break;
                                case 48:
                                    itemcode59 = "GG05";
                                    days57 = 30;
                                    break;
                                case 49:
                                    itemcode59 = "DH14";
                                    days57 = 30;
                                    break;
                                case 50:
                                    itemcode59 = "DJ60";
                                    days57 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode59, days57);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode59, days57));
                            return;
                        case "CR66":
                            int num66 = Game_Server.Generic.random(0, 5);
                            int days58 = 1;
                            string itemcode60 = (string)null;
                            switch (num66)
                            {
                                case 0:
                                    itemcode60 = "DF85";
                                    days58 = -1;
                                    break;
                                case 1:
                                    itemcode60 = "DF85";
                                    days58 = 30;
                                    break;
                                case 2:
                                    itemcode60 = "DF50";
                                    days58 = 15;
                                    break;
                                case 3:
                                    itemcode60 = "CF01";
                                    days58 = 15;
                                    break;
                                case 4:
                                    itemcode60 = "CI01";
                                    days58 = 15;
                                    break;
                                case 5:
                                    itemcode60 = "CZ85";
                                    days58 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode60, days58);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode60, days58));
                            return;
                        case "CR67":
                            int num67 = Game_Server.Generic.random(0, 6);
                            int days59 = 1;
                            string itemcode61 = (string)null;
                            switch (num67)
                            {
                                case 0:
                                    itemcode61 = "DF93";
                                    days59 = -1;
                                    break;
                                case 1:
                                    itemcode61 = "DF93";
                                    days59 = 30;
                                    break;
                                case 2:
                                    itemcode61 = "DF50";
                                    days59 = 15;
                                    break;
                                case 3:
                                    itemcode61 = "CF01";
                                    days59 = 15;
                                    break;
                                case 4:
                                    itemcode61 = "CI01";
                                    days59 = 15;
                                    break;
                                case 5:
                                    itemcode61 = "CB09";
                                    days59 = 1;
                                    break;
                                case 6:
                                    itemcode61 = "CZ84";
                                    days59 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode61, days59);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode61, days59));
                            return;
                        case "CR68":
                            int num68 = Game_Server.Generic.random(0, 34);
                            int days60 = 1;
                            string itemcode62 = (string)null;
                            switch (num68)
                            {
                                case 0:
                                    itemcode62 = "DG22";
                                    days60 = 30;
                                    break;
                                case 1:
                                    itemcode62 = "DG22";
                                    days60 = -1;
                                    break;
                                case 2:
                                    itemcode62 = "DG24";
                                    days60 = 30;
                                    break;
                                case 3:
                                    itemcode62 = "DG24";
                                    days60 = -1;
                                    break;
                                case 4:
                                    itemcode62 = "DG28";
                                    days60 = 30;
                                    break;
                                case 5:
                                    itemcode62 = "DG28";
                                    days60 = -1;
                                    break;
                                case 6:
                                    itemcode62 = "DG45";
                                    days60 = 30;
                                    break;
                                case 7:
                                    itemcode62 = "DG45";
                                    days60 = -1;
                                    break;
                                case 8:
                                    itemcode62 = "DG46";
                                    days60 = 30;
                                    break;
                                case 9:
                                    itemcode62 = "DG46";
                                    days60 = -1;
                                    break;
                                case 10:
                                    itemcode62 = "DG50";
                                    days60 = 30;
                                    break;
                                case 11:
                                    itemcode62 = "DG50";
                                    days60 = -1;
                                    break;
                                case 12:
                                    itemcode62 = "DG51";
                                    days60 = 30;
                                    break;
                                case 13:
                                    itemcode62 = "DG51";
                                    days60 = -1;
                                    break;
                                case 14:
                                    itemcode62 = "DG55";
                                    days60 = -1;
                                    break;
                                case 15:
                                    itemcode62 = "DG55";
                                    days60 = 30;
                                    break;
                                case 16:
                                    itemcode62 = "DG58";
                                    days60 = -1;
                                    break;
                                case 17:
                                    itemcode62 = "DG58";
                                    days60 = 30;
                                    break;
                                case 18:
                                    itemcode62 = "DG59";
                                    days60 = 30;
                                    break;
                                case 19:
                                    itemcode62 = "DG59";
                                    days60 = -1;
                                    break;
                                case 20:
                                    itemcode62 = "DG71";
                                    days60 = 30;
                                    break;
                                case 21:
                                    itemcode62 = "DG71";
                                    days60 = -1;
                                    break;
                                case 22:
                                    itemcode62 = "DG82";
                                    days60 = 30;
                                    break;
                                case 23:
                                    itemcode62 = "DG82";
                                    days60 = -1;
                                    break;
                                case 24:
                                    itemcode62 = "DG85";
                                    days60 = 30;
                                    break;
                                case 25:
                                    itemcode62 = "DG85";
                                    days60 = -1;
                                    break;
                                case 26:
                                    itemcode62 = "DG86";
                                    days60 = 30;
                                    break;
                                case 27:
                                    itemcode62 = "DG86";
                                    days60 = -1;
                                    break;
                                case 28:
                                    itemcode62 = "DG88";
                                    days60 = 30;
                                    break;
                                case 29:
                                    itemcode62 = "DG91";
                                    days60 = -1;
                                    break;
                                case 30:
                                    itemcode62 = "DG91";
                                    days60 = 30;
                                    break;
                                case 31:
                                    itemcode62 = "DG95";
                                    days60 = -1;
                                    break;
                                case 32:
                                    itemcode62 = "DG95";
                                    days60 = 30;
                                    break;
                                case 33:
                                    itemcode62 = "DG97";
                                    days60 = -1;
                                    break;
                                case 34:
                                    itemcode62 = "DG97";
                                    days60 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode62, days60);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode62, days60));
                            return;
                        case "CR69":
                            int num69 = Game_Server.Generic.random(0, 5);
                            int days61 = 1;
                            string itemcode63 = (string)null;
                            switch (num69)
                            {
                                case 0:
                                    itemcode63 = "DE68";
                                    days61 = 30;
                                    break;
                                case 1:
                                    itemcode63 = "DE68";
                                    days61 = -1;
                                    break;
                                case 2:
                                    itemcode63 = "DF95";
                                    days61 = 7;
                                    break;
                                case 3:
                                    itemcode63 = "CZ84";
                                    days61 = 1;
                                    break;
                                case 4:
                                    itemcode63 = "CB09";
                                    days61 = 1;
                                    break;
                                case 5:
                                    itemcode63 = "CZ81";
                                    days61 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode63, days61);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode63, days61));
                            return;
                        case "CR70":
                            int num70 = Game_Server.Generic.random(0, 18);
                            int days62 = 1;
                            string itemcode64 = (string)null;
                            switch (num70)
                            {
                                case 0:
                                    itemcode64 = "BH10";
                                    days62 = 30;
                                    break;
                                case 1:
                                    itemcode64 = "BH10";
                                    days62 = -1;
                                    break;
                                case 2:
                                    itemcode64 = "BH10";
                                    days62 = 15;
                                    break;
                                case 3:
                                    itemcode64 = "BH1C";
                                    days62 = 30;
                                    break;
                                case 4:
                                    itemcode64 = "BH1C";
                                    days62 = 15;
                                    break;
                                case 5:
                                    itemcode64 = "BH1C";
                                    days62 = -1;
                                    break;
                                case 6:
                                    itemcode64 = "BH15";
                                    days62 = 30;
                                    break;
                                case 7:
                                    itemcode64 = "BH15";
                                    days62 = 15;
                                    break;
                                case 8:
                                    itemcode64 = "BH15";
                                    days62 = -1;
                                    break;
                                case 9:
                                    itemcode64 = "BH21";
                                    days62 = 30;
                                    break;
                                case 10:
                                    itemcode64 = "BH21";
                                    days62 = 15;
                                    break;
                                case 11:
                                    itemcode64 = "BH21";
                                    days62 = -1;
                                    break;
                                case 12:
                                    itemcode64 = "BH1B";
                                    days62 = 30;
                                    break;
                                case 13:
                                    itemcode64 = "BH1B";
                                    days62 = 15;
                                    break;
                                case 14:
                                    itemcode64 = "BH1B";
                                    days62 = -1;
                                    break;
                                case 15:
                                    itemcode64 = "BH27";
                                    days62 = 30;
                                    break;
                                case 16:
                                    itemcode64 = "BH27";
                                    days62 = 15;
                                    break;
                                case 17:
                                    itemcode64 = "BH27";
                                    days62 = -1;
                                    break;
                                case 18:
                                    itemcode64 = "BH27";
                                    days62 = 30;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode64, days62);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode64, days62));
                            return;
                        case "CR71":
                            int num71 = Game_Server.Generic.random(0, 17);
                            int days63 = 1;
                            string itemcode65 = (string)null;
                            switch (num71)
                            {
                                case 0:
                                    itemcode65 = "BH11";
                                    days63 = 30;
                                    break;
                                case 1:
                                    itemcode65 = "BH11";
                                    days63 = -1;
                                    break;
                                case 2:
                                    itemcode65 = "BH11";
                                    days63 = 15;
                                    break;
                                case 3:
                                    itemcode65 = "BH28";
                                    days63 = 30;
                                    break;
                                case 4:
                                    itemcode65 = "BH28";
                                    days63 = 15;
                                    break;
                                case 5:
                                    itemcode65 = "BH28";
                                    days63 = -1;
                                    break;
                                case 6:
                                    itemcode65 = "BH17";
                                    days63 = 30;
                                    break;
                                case 7:
                                    itemcode65 = "BH17";
                                    days63 = 15;
                                    break;
                                case 8:
                                    itemcode65 = "BH17";
                                    days63 = -1;
                                    break;
                                case 9:
                                    itemcode65 = "BH23";
                                    days63 = 30;
                                    break;
                                case 10:
                                    itemcode65 = "BH23";
                                    days63 = 15;
                                    break;
                                case 11:
                                    itemcode65 = "BH23";
                                    days63 = -1;
                                    break;
                                case 12:
                                    itemcode65 = "BH18";
                                    days63 = 30;
                                    break;
                                case 13:
                                    itemcode65 = "BH18";
                                    days63 = 15;
                                    break;
                                case 14:
                                    itemcode65 = "BH18";
                                    days63 = -1;
                                    break;
                                case 15:
                                    itemcode65 = "BH24";
                                    days63 = 30;
                                    break;
                                case 16:
                                    itemcode65 = "BH24";
                                    days63 = 15;
                                    break;
                                case 17:
                                    itemcode65 = "BH24";
                                    days63 = -1;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode65, days63);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode65, days63));
                            return;
                        case "CR72":
                            int num72 = Game_Server.Generic.random(0, 17);
                            int days64 = 1;
                            string itemcode66 = (string)null;
                            switch (num72)
                            {
                                case 0:
                                    itemcode66 = "BH12";
                                    days64 = 30;
                                    break;
                                case 1:
                                    itemcode66 = "BH12";
                                    days64 = -1;
                                    break;
                                case 2:
                                    itemcode66 = "BH12";
                                    days64 = 15;
                                    break;
                                case 3:
                                    itemcode66 = "BH1E";
                                    days64 = 30;
                                    break;
                                case 4:
                                    itemcode66 = "BH1E";
                                    days64 = 15;
                                    break;
                                case 5:
                                    itemcode66 = "BH1E";
                                    days64 = -1;
                                    break;
                                case 6:
                                    itemcode66 = "BH16";
                                    days64 = 30;
                                    break;
                                case 7:
                                    itemcode66 = "BH16";
                                    days64 = 15;
                                    break;
                                case 8:
                                    itemcode66 = "BH16";
                                    days64 = -1;
                                    break;
                                case 9:
                                    itemcode66 = "BH22";
                                    days64 = 30;
                                    break;
                                case 10:
                                    itemcode66 = "BH22";
                                    days64 = 15;
                                    break;
                                case 11:
                                    itemcode66 = "BH22";
                                    days64 = -1;
                                    break;
                                case 12:
                                    itemcode66 = "BH1A";
                                    days64 = 30;
                                    break;
                                case 13:
                                    itemcode66 = "BH1A";
                                    days64 = 15;
                                    break;
                                case 14:
                                    itemcode66 = "BH1A";
                                    days64 = -1;
                                    break;
                                case 15:
                                    itemcode66 = "BH26";
                                    days64 = 30;
                                    break;
                                case 16:
                                    itemcode66 = "BH26";
                                    days64 = 15;
                                    break;
                                case 17:
                                    itemcode66 = "BH26";
                                    days64 = -1;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode66, days64);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode66, days64));
                            return;
                        case "CR73":
                            int num73 = Game_Server.Generic.random(0, 17);
                            int days65 = 1;
                            string itemcode67 = (string)null;
                            switch (num73)
                            {
                                case 0:
                                    itemcode67 = "BH13";
                                    days65 = 30;
                                    break;
                                case 1:
                                    itemcode67 = "BH13";
                                    days65 = -1;
                                    break;
                                case 2:
                                    itemcode67 = "BH13";
                                    days65 = 15;
                                    break;
                                case 3:
                                    itemcode67 = "BH1F";
                                    days65 = 30;
                                    break;
                                case 4:
                                    itemcode67 = "BH1F";
                                    days65 = 15;
                                    break;
                                case 5:
                                    itemcode67 = "BH1F";
                                    days65 = -1;
                                    break;
                                case 6:
                                    itemcode67 = "BH14";
                                    days65 = 30;
                                    break;
                                case 7:
                                    itemcode67 = "BH14";
                                    days65 = 15;
                                    break;
                                case 8:
                                    itemcode67 = "BH14";
                                    days65 = -1;
                                    break;
                                case 9:
                                    itemcode67 = "BH20";
                                    days65 = 30;
                                    break;
                                case 10:
                                    itemcode67 = "BH20";
                                    days65 = 15;
                                    break;
                                case 11:
                                    itemcode67 = "BH20";
                                    days65 = -1;
                                    break;
                                case 12:
                                    itemcode67 = "BH19";
                                    days65 = 30;
                                    break;
                                case 13:
                                    itemcode67 = "BH19";
                                    days65 = 15;
                                    break;
                                case 14:
                                    itemcode67 = "BH19";
                                    days65 = -1;
                                    break;
                                case 15:
                                    itemcode67 = "BH25";
                                    days65 = 30;
                                    break;
                                case 16:
                                    itemcode67 = "BH25";
                                    days65 = 15;
                                    break;
                                case 17:
                                    itemcode67 = "BH25";
                                    days65 = -1;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode67, days65);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode67, days65));
                            return;
                        case "CR74":
                            int num74 = Game_Server.Generic.random(0, 46);
                            int days66 = 1;
                            string itemcode68 = (string)null;
                            switch (num74)
                            {
                                case 0:
                                    itemcode68 = "DG22";
                                    days66 = 30;
                                    break;
                                case 1:
                                    itemcode68 = "DG22";
                                    days66 = -1;
                                    break;
                                case 2:
                                    itemcode68 = "DG24";
                                    days66 = 30;
                                    break;
                                case 3:
                                    itemcode68 = "DG24";
                                    days66 = -1;
                                    break;
                                case 4:
                                    itemcode68 = "DG28";
                                    days66 = 30;
                                    break;
                                case 5:
                                    itemcode68 = "DG28";
                                    days66 = -1;
                                    break;
                                case 6:
                                    itemcode68 = "DG45";
                                    days66 = 30;
                                    break;
                                case 7:
                                    itemcode68 = "DG45";
                                    days66 = -1;
                                    break;
                                case 8:
                                    itemcode68 = "DG46";
                                    days66 = 30;
                                    break;
                                case 9:
                                    itemcode68 = "DG46";
                                    days66 = -1;
                                    break;
                                case 10:
                                    itemcode68 = "DG50";
                                    days66 = 30;
                                    break;
                                case 11:
                                    itemcode68 = "DG50";
                                    days66 = -1;
                                    break;
                                case 12:
                                    itemcode68 = "DG51";
                                    days66 = 30;
                                    break;
                                case 13:
                                    itemcode68 = "DG51";
                                    days66 = -1;
                                    break;
                                case 14:
                                    itemcode68 = "DG55";
                                    days66 = -1;
                                    break;
                                case 15:
                                    itemcode68 = "DG55";
                                    days66 = 30;
                                    break;
                                case 16:
                                    itemcode68 = "DG58";
                                    days66 = -1;
                                    break;
                                case 17:
                                    itemcode68 = "DG58";
                                    days66 = 30;
                                    break;
                                case 18:
                                    itemcode68 = "DG59";
                                    days66 = 30;
                                    break;
                                case 19:
                                    itemcode68 = "DG59";
                                    days66 = -1;
                                    break;
                                case 20:
                                    itemcode68 = "DG71";
                                    days66 = 30;
                                    break;
                                case 21:
                                    itemcode68 = "DG71";
                                    days66 = -1;
                                    break;
                                case 22:
                                    itemcode68 = "DG82";
                                    days66 = 30;
                                    break;
                                case 23:
                                    itemcode68 = "DG82";
                                    days66 = -1;
                                    break;
                                case 24:
                                    itemcode68 = "DG85";
                                    days66 = 30;
                                    break;
                                case 25:
                                    itemcode68 = "DG85";
                                    days66 = -1;
                                    break;
                                case 26:
                                    itemcode68 = "DG86";
                                    days66 = 30;
                                    break;
                                case 27:
                                    itemcode68 = "DG86";
                                    days66 = -1;
                                    break;
                                case 28:
                                    itemcode68 = "DG88";
                                    days66 = 30;
                                    break;
                                case 29:
                                    itemcode68 = "DG91";
                                    days66 = -1;
                                    break;
                                case 30:
                                    itemcode68 = "DG91";
                                    days66 = 30;
                                    break;
                                case 31:
                                    itemcode68 = "DG95";
                                    days66 = -1;
                                    break;
                                case 32:
                                    itemcode68 = "DG95";
                                    days66 = 30;
                                    break;
                                case 33:
                                    itemcode68 = "DG97";
                                    days66 = -1;
                                    break;
                                case 34:
                                    itemcode68 = "DG97";
                                    days66 = 30;
                                    break;
                                case 35:
                                    itemcode68 = "GG08";
                                    days66 = -1;
                                    break;
                                case 36:
                                    itemcode68 = "GG08";
                                    days66 = 30;
                                    break;
                                case 37:
                                    itemcode68 = "GG10";
                                    days66 = -1;
                                    break;
                                case 38:
                                    itemcode68 = "GG10";
                                    days66 = 30;
                                    break;
                                case 39:
                                    itemcode68 = "GG17";
                                    days66 = 30;
                                    break;
                                case 40:
                                    itemcode68 = "GG17";
                                    days66 = -1;
                                    break;
                                case 41:
                                    itemcode68 = "GG27";
                                    days66 = 30;
                                    break;
                                case 42:
                                    itemcode68 = "GG27";
                                    days66 = -1;
                                    break;
                                case 43:
                                    itemcode68 = "GG28";
                                    days66 = 30;
                                    break;
                                case 44:
                                    itemcode68 = "GG28";
                                    days66 = -1;
                                    break;
                                case 45:
                                    itemcode68 = "GG32";
                                    days66 = 30;
                                    break;
                                case 46:
                                    itemcode68 = "GG32";
                                    days66 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode68, days66);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode68, days66));
                            return;
                        case "CR75":
                            int num75 = Game_Server.Generic.random(0, 47);
                            int days67 = 1;
                            string itemcode69 = (string)null;
                            switch (num75)
                            {
                                case 0:
                                    itemcode69 = "DC33";
                                    days67 = 30;
                                    break;
                                case 1:
                                    itemcode69 = "DC33";
                                    days67 = -1;
                                    break;
                                case 2:
                                    itemcode69 = "DC93";
                                    days67 = 30;
                                    break;
                                case 3:
                                    itemcode69 = "DC93";
                                    days67 = -1;
                                    break;
                                case 4:
                                    itemcode69 = "DE07";
                                    days67 = 30;
                                    break;
                                case 5:
                                    itemcode69 = "DE07";
                                    days67 = -1;
                                    break;
                                case 6:
                                    itemcode69 = "DC73";
                                    days67 = 30;
                                    break;
                                case 7:
                                    itemcode69 = "DC73";
                                    days67 = -1;
                                    break;
                                case 8:
                                    itemcode69 = "DC78";
                                    days67 = 30;
                                    break;
                                case 9:
                                    itemcode69 = "DC78";
                                    days67 = -1;
                                    break;
                                case 10:
                                    itemcode69 = "DC79";
                                    days67 = 30;
                                    break;
                                case 11:
                                    itemcode69 = "DC79";
                                    days67 = -1;
                                    break;
                                case 12:
                                    itemcode69 = "DC79";
                                    days67 = 30;
                                    break;
                                case 13:
                                    itemcode69 = "DC80";
                                    days67 = -1;
                                    break;
                                case 14:
                                    itemcode69 = "DC80";
                                    days67 = -1;
                                    break;
                                case 15:
                                    itemcode69 = "DC98";
                                    days67 = 30;
                                    break;
                                case 16:
                                    itemcode69 = "DC98";
                                    days67 = -1;
                                    break;
                                case 17:
                                    itemcode69 = "DE28";
                                    days67 = 30;
                                    break;
                                case 18:
                                    itemcode69 = "DE28";
                                    days67 = 30;
                                    break;
                                case 19:
                                    itemcode69 = "DE30";
                                    days67 = -1;
                                    break;
                                case 20:
                                    itemcode69 = "DE30";
                                    days67 = 30;
                                    break;
                                case 21:
                                    itemcode69 = "DE31";
                                    days67 = -1;
                                    break;
                                case 22:
                                    itemcode69 = "DE31";
                                    days67 = 30;
                                    break;
                                case 23:
                                    itemcode69 = "DE33";
                                    days67 = -1;
                                    break;
                                case 24:
                                    itemcode69 = "DE33";
                                    days67 = 30;
                                    break;
                                case 25:
                                    itemcode69 = "DE45";
                                    days67 = -1;
                                    break;
                                case 26:
                                    itemcode69 = "DE45";
                                    days67 = 30;
                                    break;
                                case 27:
                                    itemcode69 = "DE46";
                                    days67 = -1;
                                    break;
                                case 28:
                                    itemcode69 = "DE46";
                                    days67 = 30;
                                    break;
                                case 29:
                                    itemcode69 = "DE49";
                                    days67 = -1;
                                    break;
                                case 30:
                                    itemcode69 = "DE55";
                                    days67 = 30;
                                    break;
                                case 31:
                                    itemcode69 = "DE55";
                                    days67 = -1;
                                    break;
                                case 32:
                                    itemcode69 = "DE60";
                                    days67 = 30;
                                    break;
                                case 33:
                                    itemcode69 = "DE60";
                                    days67 = -1;
                                    break;
                                case 34:
                                    itemcode69 = "DE64";
                                    days67 = 30;
                                    break;
                                case 35:
                                    itemcode69 = "DE64";
                                    days67 = -1;
                                    break;
                                case 36:
                                    itemcode69 = "DE65";
                                    days67 = 30;
                                    break;
                                case 37:
                                    itemcode69 = "DE65";
                                    days67 = -1;
                                    break;
                                case 38:
                                    itemcode69 = "DE66";
                                    days67 = 30;
                                    break;
                                case 39:
                                    itemcode69 = "DE66";
                                    days67 = 30;
                                    break;
                                case 40:
                                    itemcode69 = "DE67";
                                    days67 = -1;
                                    break;
                                case 41:
                                    itemcode69 = "DE67";
                                    days67 = 30;
                                    break;
                                case 42:
                                    itemcode69 = "DE69";
                                    days67 = -1;
                                    break;
                                case 43:
                                    itemcode69 = "DE69";
                                    days67 = 30;
                                    break;
                                case 44:
                                    itemcode69 = "DE81";
                                    days67 = -1;
                                    break;
                                case 45:
                                    itemcode69 = "DE81";
                                    days67 = 30;
                                    break;
                                case 46:
                                    itemcode69 = "DE94";
                                    days67 = -1;
                                    break;
                                case 47:
                                    itemcode69 = "DE94";
                                    days67 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode69, days67);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode69, days67));
                            return;
                        case "CR76":
                            int num76 = Game_Server.Generic.random(0, 7);
                            int days68 = 1;
                            string itemcode70 = (string)null;
                            switch (num76)
                            {
                                case 0:
                                    itemcode70 = "DF99";
                                    days68 = -1;
                                    break;
                                case 1:
                                    itemcode70 = "DG88";
                                    days68 = -1;
                                    break;
                                case 2:
                                    itemcode70 = "DE69";
                                    days68 = -1;
                                    break;
                                case 3:
                                    itemcode70 = "DJ44";
                                    days68 = -1;
                                    break;
                                case 4:
                                    itemcode70 = "DF99";
                                    days68 = 30;
                                    break;
                                case 5:
                                    itemcode70 = "DG88";
                                    days68 = 30;
                                    break;
                                case 6:
                                    itemcode70 = "DE69";
                                    days68 = 30;
                                    break;
                                case 7:
                                    itemcode70 = "DJ44";
                                    days68 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode70, days68);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode70, days68));
                            return;
                        case "CR77":
                            int num77 = Game_Server.Generic.random(0, 14);
                            int days69 = 1;
                            string itemcode71 = (string)null;
                            switch (num77)
                            {
                                case 0:
                                    itemcode71 = "D806";
                                    days69 = -1;
                                    break;
                                case 1:
                                    itemcode71 = "D806";
                                    days69 = 90;
                                    break;
                                case 2:
                                    itemcode71 = "D806";
                                    days69 = 30;
                                    break;
                                case 3:
                                    itemcode71 = "D604";
                                    days69 = -1;
                                    break;
                                case 4:
                                    itemcode71 = "D604";
                                    days69 = 30;
                                    break;
                                case 5:
                                    itemcode71 = "D604";
                                    days69 = 90;
                                    break;
                                case 6:
                                    itemcode71 = "D909";
                                    days69 = -1;
                                    break;
                                case 7:
                                    itemcode71 = "D909";
                                    days69 = 30;
                                    break;
                                case 8:
                                    itemcode71 = "D909";
                                    days69 = 90;
                                    break;
                                case 9:
                                    itemcode71 = "D829";
                                    days69 = -1;
                                    break;
                                case 10:
                                    itemcode71 = "D829";
                                    days69 = 30;
                                    break;
                                case 11:
                                    itemcode71 = "D829";
                                    days69 = 90;
                                    break;
                                case 12:
                                    itemcode71 = "D830";
                                    days69 = -1;
                                    break;
                                case 13:
                                    itemcode71 = "D830";
                                    days69 = 30;
                                    break;
                                case 14:
                                    itemcode71 = "D830";
                                    days69 = 90;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode71, days69);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode71, days69));
                            return;
                        case "CR78":
                            int num78 = Game_Server.Generic.random(0, 7);
                            int days70 = 1;
                            string itemcode72 = (string)null;
                            switch (num78)
                            {
                                case 0:
                                    itemcode72 = "GF04";
                                    days70 = 30;
                                    break;
                                case 1:
                                    itemcode72 = "DB45";
                                    days70 = 30;
                                    break;
                                case 2:
                                    itemcode72 = "DC01";
                                    days70 = -1;
                                    break;
                                case 3:
                                    itemcode72 = "DB03";
                                    days70 = 30;
                                    break;
                                case 4:
                                    itemcode72 = "CF02";
                                    days70 = 15;
                                    break;
                                case 5:
                                    itemcode72 = "CB09";
                                    days70 = 1;
                                    break;
                                case 6:
                                    itemcode72 = "CZ81";
                                    days70 = 90;
                                    break;
                                case 7:
                                    itemcode72 = "DF06";
                                    days70 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode72, days70);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode72, days70));
                            return;
                        case "CR80":
                            int num79 = Game_Server.Generic.random(0, 8);
                            int days71 = 1;
                            string itemcode73 = (string)null;
                            switch (num79)
                            {
                                case 0:
                                    itemcode73 = "DD26";
                                    days71 = 30;
                                    break;
                                case 1:
                                    itemcode73 = "BK15";
                                    days71 = 30;
                                    break;
                                case 2:
                                    itemcode73 = "CM07";
                                    days71 = 5000;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode73, days71);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode73, days71));
                            return;
                        case "CR86":
                            int num80 = Game_Server.Generic.random(0, 7);
                            int days72 = 1;
                            string itemcode74 = (string)null;
                            switch (num80)
                            {
                                case 0:
                                    itemcode74 = "DT17";
                                    days72 = 30;
                                    break;
                                case 1:
                                    itemcode74 = "DB45";
                                    days72 = 30;
                                    break;
                                case 2:
                                    itemcode74 = "DC01";
                                    days72 = -1;
                                    break;
                                case 3:
                                    itemcode74 = "DB03";
                                    days72 = 30;
                                    break;
                                case 4:
                                    itemcode74 = "CF02";
                                    days72 = 15;
                                    break;
                                case 5:
                                    itemcode74 = "CB09";
                                    days72 = 1;
                                    break;
                                case 6:
                                    itemcode74 = "CZ81";
                                    days72 = 90;
                                    break;
                                case 7:
                                    itemcode74 = "DF06";
                                    days72 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode74, days72);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode74, days72));
                            return;
                        case "CR87":
                            int num81 = Game_Server.Generic.random(0, 4);
                            int days73 = 1;
                            string itemcode75 = (string)null;
                            switch (num81)
                            {
                                case 0:
                                    itemcode75 = "DG91";
                                    days73 = 30;
                                    break;
                                case 1:
                                    itemcode75 = "DG91";
                                    days73 = -1;
                                    break;
                                case 2:
                                    itemcode75 = "DS01";
                                    days73 = 7;
                                    break;
                                case 3:
                                    itemcode75 = "CI01";
                                    days73 = 7;
                                    break;
                                case 4:
                                    itemcode75 = "CC05";
                                    days73 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode75, days73);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode75, days73));
                            return;
                        case "CR89":
                            int num82 = Game_Server.Generic.random(0, 77);
                            int days74 = 1;
                            string itemcode76 = (string)null;
                            switch (num82)
                            {
                                case 0:
                                    itemcode76 = "DF35";
                                    days74 = 30;
                                    break;
                                case 1:
                                    itemcode76 = "DF35";
                                    days74 = -1;
                                    break;
                                case 2:
                                    itemcode76 = "DF14";
                                    days74 = 30;
                                    break;
                                case 3:
                                    itemcode76 = "DF14";
                                    days74 = -1;
                                    break;
                                case 4:
                                    itemcode76 = "DF65";
                                    days74 = 30;
                                    break;
                                case 5:
                                    itemcode76 = "DF65";
                                    days74 = -1;
                                    break;
                                case 6:
                                    itemcode76 = "DF95";
                                    days74 = 30;
                                    break;
                                case 7:
                                    itemcode76 = "DF95";
                                    days74 = -1;
                                    break;
                                case 8:
                                    itemcode76 = "DF25";
                                    days74 = 30;
                                    break;
                                case 9:
                                    itemcode76 = "DF25";
                                    days74 = -1;
                                    break;
                                case 10:
                                    itemcode76 = "DF47";
                                    days74 = 30;
                                    break;
                                case 11:
                                    itemcode76 = "DF47";
                                    days74 = -1;
                                    break;
                                case 12:
                                    itemcode76 = "DF51";
                                    days74 = 30;
                                    break;
                                case 13:
                                    itemcode76 = "DF51";
                                    days74 = -1;
                                    break;
                                case 14:
                                    itemcode76 = "DF52";
                                    days74 = -1;
                                    break;
                                case 15:
                                    itemcode76 = "DF52";
                                    days74 = 30;
                                    break;
                                case 16:
                                    itemcode76 = "DF53";
                                    days74 = -1;
                                    break;
                                case 17:
                                    itemcode76 = "DF53";
                                    days74 = 30;
                                    break;
                                case 18:
                                    itemcode76 = "DF58";
                                    days74 = 30;
                                    break;
                                case 19:
                                    itemcode76 = "DF58";
                                    days74 = -1;
                                    break;
                                case 20:
                                    itemcode76 = "DF60";
                                    days74 = 30;
                                    break;
                                case 21:
                                    itemcode76 = "DF60";
                                    days74 = -1;
                                    break;
                                case 22:
                                    itemcode76 = "DF61";
                                    days74 = 30;
                                    break;
                                case 23:
                                    itemcode76 = "DF61";
                                    days74 = -1;
                                    break;
                                case 24:
                                    itemcode76 = "DF62";
                                    days74 = 30;
                                    break;
                                case 25:
                                    itemcode76 = "DF62";
                                    days74 = -1;
                                    break;
                                case 26:
                                    itemcode76 = "DF63";
                                    days74 = 30;
                                    break;
                                case 27:
                                    itemcode76 = "DF63";
                                    days74 = -1;
                                    break;
                                case 28:
                                    itemcode76 = "DF64";
                                    days74 = 30;
                                    break;
                                case 29:
                                    itemcode76 = "DF64";
                                    days74 = -1;
                                    break;
                                case 30:
                                    itemcode76 = "DF69";
                                    days74 = 30;
                                    break;
                                case 31:
                                    itemcode76 = "DF69";
                                    days74 = -1;
                                    break;
                                case 32:
                                    itemcode76 = "DF74";
                                    days74 = 30;
                                    break;
                                case 33:
                                    itemcode76 = "DF74";
                                    days74 = -1;
                                    break;
                                case 34:
                                    itemcode76 = "DF75";
                                    days74 = 30;
                                    break;
                                case 35:
                                    itemcode76 = "DF75";
                                    days74 = -1;
                                    break;
                                case 36:
                                    itemcode76 = "DF79";
                                    days74 = 30;
                                    break;
                                case 37:
                                    itemcode76 = "DF79";
                                    days74 = -1;
                                    break;
                                case 38:
                                    itemcode76 = "DF85";
                                    days74 = 30;
                                    break;
                                case 39:
                                    itemcode76 = "DF85";
                                    days74 = 30;
                                    break;
                                case 40:
                                    itemcode76 = "DF86";
                                    days74 = -1;
                                    break;
                                case 41:
                                    itemcode76 = "DF86";
                                    days74 = 30;
                                    break;
                                case 42:
                                    itemcode76 = "DF89";
                                    days74 = -1;
                                    break;
                                case 43:
                                    itemcode76 = "DF89";
                                    days74 = 30;
                                    break;
                                case 44:
                                    itemcode76 = "DF94";
                                    days74 = -1;
                                    break;
                                case 45:
                                    itemcode76 = "DF94";
                                    days74 = 30;
                                    break;
                                case 46:
                                    itemcode76 = "DF97";
                                    days74 = -1;
                                    break;
                                case 47:
                                    itemcode76 = "DF98";
                                    days74 = 30;
                                    break;
                                case 48:
                                    itemcode76 = "DF98";
                                    days74 = -1;
                                    break;
                                case 49:
                                    itemcode76 = "DF99";
                                    days74 = 30;
                                    break;
                                case 50:
                                    itemcode76 = "DF99";
                                    days74 = -1;
                                    break;
                                case 51:
                                    itemcode76 = "DF35";
                                    days74 = 15;
                                    break;
                                case 52:
                                    itemcode76 = "DF14";
                                    days74 = 15;
                                    break;
                                case 53:
                                    itemcode76 = "DF65";
                                    days74 = 15;
                                    break;
                                case 54:
                                    itemcode76 = "DF95";
                                    days74 = 15;
                                    break;
                                case 55:
                                    itemcode76 = "DF25";
                                    days74 = 15;
                                    break;
                                case 56:
                                    itemcode76 = "DF47";
                                    days74 = 15;
                                    break;
                                case 57:
                                    itemcode76 = "DF50";
                                    days74 = 15;
                                    break;
                                case 58:
                                    itemcode76 = "DF51";
                                    days74 = 15;
                                    break;
                                case 59:
                                    itemcode76 = "DF52";
                                    days74 = 15;
                                    break;
                                case 60:
                                    itemcode76 = "DF53";
                                    days74 = 15;
                                    break;
                                case 61:
                                    itemcode76 = "DF58";
                                    days74 = 15;
                                    break;
                                case 62:
                                    itemcode76 = "DF60";
                                    days74 = 15;
                                    break;
                                case 63:
                                    itemcode76 = "DF61";
                                    days74 = 15;
                                    break;
                                case 64:
                                    itemcode76 = "DF62";
                                    days74 = 15;
                                    break;
                                case 65:
                                    itemcode76 = "DF63";
                                    days74 = 15;
                                    break;
                                case 66:
                                    itemcode76 = "DF64";
                                    days74 = 15;
                                    break;
                                case 67:
                                    itemcode76 = "DF69";
                                    days74 = 15;
                                    break;
                                case 68:
                                    itemcode76 = "DF74";
                                    days74 = 15;
                                    break;
                                case 69:
                                    itemcode76 = "DF75";
                                    days74 = 15;
                                    break;
                                case 70:
                                    itemcode76 = "DF79";
                                    days74 = 15;
                                    break;
                                case 71:
                                    itemcode76 = "DF85";
                                    days74 = 15;
                                    break;
                                case 72:
                                    itemcode76 = "DF86";
                                    days74 = 15;
                                    break;
                                case 73:
                                    itemcode76 = "DF89";
                                    days74 = 15;
                                    break;
                                case 74:
                                    itemcode76 = "DF94";
                                    days74 = 15;
                                    break;
                                case 75:
                                    itemcode76 = "DF97";
                                    days74 = 15;
                                    break;
                                case 76:
                                    itemcode76 = "DF98";
                                    days74 = 15;
                                    break;
                                case 77:
                                    itemcode76 = "DF99";
                                    days74 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode76, days74);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode76, days74));
                            return;
                        case "CR93":
                            int num83 = Game_Server.Generic.random(0, 4);
                            int days75 = 1;
                            string itemcode77 = (string)null;
                            switch (num83)
                            {
                                case 0:
                                    itemcode77 = "D910";
                                    days75 = 60;
                                    break;
                                case 1:
                                    itemcode77 = "D910";
                                    days75 = 90;
                                    break;
                                case 2:
                                    itemcode77 = "DI06";
                                    days75 = 180;
                                    break;
                                case 3:
                                    itemcode77 = "DI06";
                                    days75 = 360;
                                    break;
                                case 4:
                                    itemcode77 = "DG26";
                                    days75 = 5000;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode77, days75);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode77, days75));
                            return;
                        case "CR94":
                            int num84 = Game_Server.Generic.random(0, 4);
                            int days76 = 1;
                            string itemcode78 = (string)null;
                            switch (num84)
                            {
                                case 0:
                                    itemcode78 = "D831";
                                    days76 = 60;
                                    break;
                                case 1:
                                    itemcode78 = "D831";
                                    days76 = 90;
                                    break;
                                case 2:
                                    itemcode78 = "D831";
                                    days76 = 180;
                                    break;
                                case 3:
                                    itemcode78 = "D831";
                                    days76 = 360;
                                    break;
                                case 4:
                                    itemcode78 = "D831";
                                    days76 = 5000;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode78, days76);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode78, days76));
                            return;
                        case "CR95":
                            int num85 = Game_Server.Generic.random(0, 9);
                            int days77 = 1;
                            string itemcode79 = (string)null;
                            switch (num85)
                            {
                                case 0:
                                    itemcode79 = "D608";
                                    days77 = 60;
                                    break;
                                case 1:
                                    itemcode79 = "D608";
                                    days77 = 90;
                                    break;
                                case 2:
                                    itemcode79 = "D608";
                                    days77 = 180;
                                    break;
                                case 3:
                                    itemcode79 = "D608";
                                    days77 = 360;
                                    break;
                                case 4:
                                    itemcode79 = "D608";
                                    days77 = 5000;
                                    break;
                                case 5:
                                    itemcode79 = "D911";
                                    days77 = 60;
                                    break;
                                case 6:
                                    itemcode79 = "D911";
                                    days77 = 90;
                                    break;
                                case 7:
                                    itemcode79 = "D911";
                                    days77 = 180;
                                    break;
                                case 8:
                                    itemcode79 = "D911";
                                    days77 = 360;
                                    break;
                                case 9:
                                    itemcode79 = "D911";
                                    days77 = 5000;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode79, days77);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode79, days77));
                            return;
                        case "CR96":
                            int num86 = Game_Server.Generic.random(0, 6);
                            int days78 = 1;
                            string itemcode80 = (string)null;
                            switch (num86)
                            {
                                case 0:
                                    itemcode80 = "GF02";
                                    days78 = 30;
                                    break;
                                case 1:
                                    itemcode80 = "GF02";
                                    days78 = -1;
                                    break;
                                case 2:
                                    itemcode80 = "DF50";
                                    days78 = 15;
                                    break;
                                case 3:
                                    itemcode80 = "CF01";
                                    days78 = 15;
                                    break;
                                case 4:
                                    itemcode80 = "CI01";
                                    days78 = 15;
                                    break;
                                case 5:
                                    itemcode80 = "CB09";
                                    days78 = 1;
                                    break;
                                case 6:
                                    itemcode80 = "CZ84";
                                    days78 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode80, days78);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode80, days78));
                            return;
                        case "CR98":
                            int num87 = Game_Server.Generic.random(0, 9);
                            int days79 = 1;
                            string itemcode81 = (string)null;
                            switch (num87)
                            {
                                case 0:
                                    itemcode81 = "D832";
                                    days79 = 60;
                                    break;
                                case 1:
                                    itemcode81 = "D832";
                                    days79 = 90;
                                    break;
                                case 2:
                                    itemcode81 = "D832";
                                    days79 = 180;
                                    break;
                                case 3:
                                    itemcode81 = "D832";
                                    days79 = 360;
                                    break;
                                case 4:
                                    itemcode81 = "D832";
                                    days79 = 5000;
                                    break;
                                case 5:
                                    itemcode81 = "D911";
                                    days79 = 60;
                                    break;
                                case 6:
                                    itemcode81 = "D911";
                                    days79 = 90;
                                    break;
                                case 7:
                                    itemcode81 = "D911";
                                    days79 = 180;
                                    break;
                                case 8:
                                    itemcode81 = "D911";
                                    days79 = 360;
                                    break;
                                case 9:
                                    itemcode81 = "D911";
                                    days79 = 5000;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode81, days79);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode81, days79));
                            return;
                        case "CR99":
                            int num88 = Game_Server.Generic.random(0, 5);
                            int days80 = 1;
                            string itemcode82 = (string)null;
                            switch (num88)
                            {
                                case 0:
                                    itemcode82 = "DE68";
                                    days80 = 30;
                                    break;
                                case 1:
                                    itemcode82 = "DE68";
                                    days80 = -1;
                                    break;
                                case 2:
                                    itemcode82 = "DF95";
                                    days80 = 7;
                                    break;
                                case 3:
                                    itemcode82 = "CZ84";
                                    days80 = 1;
                                    break;
                                case 4:
                                    itemcode82 = "CB09";
                                    break;
                                case 5:
                                    itemcode82 = "CZ81";
                                    num3 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode82, days80);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode82, days80));
                            return;
                        case "CY02":
                            int num89 = Game_Server.Generic.random(0, 5);
                            int days81 = 1;
                            string itemcode83 = (string)null;
                            switch (num89)
                            {
                                case 0:
                                    itemcode83 = "DC15";
                                    days81 = -1;
                                    break;
                                case 1:
                                    itemcode83 = "DC15";
                                    days81 = 15;
                                    break;
                                case 2:
                                    itemcode83 = "DC15";
                                    days81 = 30;
                                    break;
                                case 3:
                                    itemcode83 = "CF01";
                                    days81 = 15;
                                    break;
                                case 4:
                                    itemcode83 = "CIO1";
                                    days81 = 15;
                                    break;
                                case 5:
                                    itemcode83 = "CA01";
                                    days81 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode83, days81);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode83, days81));
                            return;
                        case "CY04":
                            int num90 = Game_Server.Generic.random(0, 5);
                            int days82 = 1;
                            string itemcode84 = (string)null;
                            switch (num90)
                            {
                                case 0:
                                    itemcode84 = "DC17";
                                    days82 = -1;
                                    break;
                                case 1:
                                    itemcode84 = "DC17";
                                    days82 = 15;
                                    break;
                                case 2:
                                    itemcode84 = "DC17";
                                    days82 = 30;
                                    break;
                                case 3:
                                    itemcode84 = "CF01";
                                    days82 = 15;
                                    break;
                                case 4:
                                    itemcode84 = "CIO1";
                                    days82 = 15;
                                    break;
                                case 5:
                                    itemcode84 = "CA01";
                                    days82 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode84, days82);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode84, days82));
                            return;
                        case "CY09":
                            int num91 = Game_Server.Generic.random(0, 5);
                            int days83 = 1;
                            string itemcode85 = (string)null;
                            switch (num91)
                            {
                                case 0:
                                    itemcode85 = "DG33";
                                    days83 = -1;
                                    break;
                                case 1:
                                    itemcode85 = "DG33";
                                    days83 = 15;
                                    break;
                                case 3:
                                    itemcode85 = "CF01";
                                    days83 = 15;
                                    break;
                                case 4:
                                    itemcode85 = "CIO1";
                                    days83 = 15;
                                    break;
                                case 5:
                                    itemcode85 = "CA01";
                                    days83 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode85, days83);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode85, days83));
                            return;
                        case "CY10":
                            int num92 = Game_Server.Generic.random(0, 5);
                            int days84 = 1;
                            string itemcode86 = (string)null;
                            switch (num92)
                            {
                                case 0:
                                    itemcode86 = "D501";
                                    days84 = -1;
                                    break;
                                case 1:
                                    itemcode86 = "D501";
                                    days84 = 15;
                                    break;
                                case 2:
                                    itemcode86 = "D501";
                                    days84 = 30;
                                    break;
                                case 3:
                                    itemcode86 = "CF01";
                                    days84 = 15;
                                    break;
                                case 4:
                                    itemcode86 = "CIO1";
                                    days84 = 15;
                                    break;
                                case 5:
                                    itemcode86 = "DS03";
                                    days84 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode86, days84);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode86, days84));
                            return;
                        case "CY12":
                            int num93 = Game_Server.Generic.random(0, 5);
                            int days85 = 1;
                            string itemcode87 = (string)null;
                            switch (num93)
                            {
                                case 0:
                                    itemcode87 = "D806";
                                    days85 = -1;
                                    break;
                                case 1:
                                    itemcode87 = "D806";
                                    days85 = 15;
                                    break;
                                case 2:
                                    itemcode87 = "D806";
                                    days85 = 30;
                                    break;
                                case 3:
                                    itemcode87 = "CF01";
                                    days85 = 15;
                                    break;
                                case 4:
                                    itemcode87 = "CIO1";
                                    days85 = 15;
                                    break;
                                case 5:
                                    itemcode87 = "DS03";
                                    days85 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode87, days85);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode87, days85));
                            return;
                        case "CY23":
                            int num94 = Game_Server.Generic.random(0, 5);
                            int days86 = 1;
                            string itemcode88 = (string)null;
                            switch (num94)
                            {
                                case 0:
                                    itemcode88 = "D705";
                                    days86 = -1;
                                    break;
                                case 1:
                                    itemcode88 = "D705";
                                    days86 = 15;
                                    break;
                                case 2:
                                    itemcode88 = "D705";
                                    days86 = 30;
                                    break;
                                case 3:
                                    itemcode88 = "CF01";
                                    days86 = 15;
                                    break;
                                case 4:
                                    itemcode88 = "CIO1";
                                    days86 = 15;
                                    break;
                                case 5:
                                    itemcode88 = "DS03";
                                    days86 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode88, days86);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode88, days86));
                            return;
                        case "CY24":
                            int num95 = Game_Server.Generic.random(0, 5);
                            int days87 = 1;
                            string itemcode89 = (string)null;
                            switch (num95)
                            {
                                case 0:
                                    itemcode89 = "D505";
                                    days87 = -1;
                                    break;
                                case 1:
                                    itemcode89 = "D505";
                                    days87 = 15;
                                    break;
                                case 2:
                                    itemcode89 = "D705";
                                    days87 = 30;
                                    break;
                                case 3:
                                    itemcode89 = "CF01";
                                    days87 = 15;
                                    break;
                                case 4:
                                    itemcode89 = "CIO1";
                                    days87 = 15;
                                    break;
                                case 5:
                                    itemcode89 = "DS03";
                                    days87 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode89, days87);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode89, days87));
                            return;
                        case "CY29":
                            int num96 = Game_Server.Generic.random(0, 7);
                            int days88 = 1;
                            string itemcode90 = (string)null;
                            switch (num96)
                            {
                                case 0:
                                    itemcode90 = "DE78";
                                    days88 = 45;
                                    break;
                                case 1:
                                    itemcode90 = "DE78";
                                    days88 = 60;
                                    break;
                                case 2:
                                    itemcode90 = "DE78";
                                    days88 = 90;
                                    break;
                                case 3:
                                    itemcode90 = "DS03";
                                    days88 = 30;
                                    break;
                                case 4:
                                    itemcode90 = "CF02";
                                    days88 = 15;
                                    break;
                                case 5:
                                    itemcode90 = "CB09";
                                    days88 = 1;
                                    break;
                                case 6:
                                    itemcode90 = "CZ81";
                                    days88 = 90;
                                    break;
                                case 7:
                                    itemcode90 = "CF01";
                                    days88 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode90, days88);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode90, days88));
                            return;
                        case "CY33":
                            int num97 = Game_Server.Generic.random(0, 50);
                            int days89 = 1;
                            string itemcode91 = (string)null;
                            switch (num97)
                            {
                                case 0:
                                    itemcode91 = "DG97";
                                    days89 = 365;
                                    break;
                                case 2:
                                    itemcode91 = "DG97";
                                    days89 = 90;
                                    break;
                                case 3:
                                    itemcode91 = "DG97";
                                    days89 = 60;
                                    break;
                                case 4:
                                    itemcode91 = "DG97";
                                    days89 = 45;
                                    break;
                                case 5:
                                    itemcode91 = "DG97";
                                    days89 = 30;
                                    break;
                                case 6:
                                    itemcode91 = "DG97";
                                    days89 = 15;
                                    break;
                                case 7:
                                    itemcode91 = "DG97";
                                    days89 = 7;
                                    break;
                                case 8:
                                    itemcode91 = "DE81";
                                    days89 = 365;
                                    break;
                                case 9:
                                    itemcode91 = "DE81";
                                    days89 = 90;
                                    break;
                                case 10:
                                    itemcode91 = "DE81";
                                    days89 = 60;
                                    break;
                                case 11:
                                    itemcode91 = "DE81";
                                    days89 = 45;
                                    break;
                                case 12:
                                    itemcode91 = "DE81";
                                    days89 = 30;
                                    break;
                                case 13:
                                    itemcode91 = "DE81";
                                    days89 = 15;
                                    break;
                                case 14:
                                    itemcode91 = "DE81";
                                    days89 = 7;
                                    break;
                                case 15:
                                    itemcode91 = "DT19";
                                    days89 = 365;
                                    break;
                                case 16:
                                    itemcode91 = "DT19";
                                    days89 = 90;
                                    break;
                                case 17:
                                    itemcode91 = "DT19";
                                    days89 = 45;
                                    break;
                                case 18:
                                    itemcode91 = "DT19";
                                    days89 = 30;
                                    break;
                                case 19:
                                    itemcode91 = "DT19";
                                    days89 = 15;
                                    break;
                                case 20:
                                    itemcode91 = "DT19";
                                    days89 = 7;
                                    break;
                                case 21:
                                    itemcode91 = "DE86";
                                    days89 = 365;
                                    break;
                                case 22:
                                    itemcode91 = "DE86";
                                    days89 = 90;
                                    break;
                                case 23:
                                    itemcode91 = "DE86";
                                    days89 = 60;
                                    break;
                                case 24:
                                    itemcode91 = "DE86";
                                    days89 = 45;
                                    break;
                                case 25:
                                    itemcode91 = "DE86";
                                    days89 = 30;
                                    break;
                                case 26:
                                    itemcode91 = "DE86";
                                    days89 = 15;
                                    break;
                                case 27:
                                    itemcode91 = "DE86";
                                    days89 = 7;
                                    break;
                                case 28:
                                    itemcode91 = "DJ58";
                                    days89 = 365;
                                    break;
                                case 29:
                                    itemcode91 = "DJ58";
                                    days89 = 90;
                                    break;
                                case 30:
                                    itemcode91 = "DJ58";
                                    days89 = 60;
                                    break;
                                case 31:
                                    itemcode91 = "DJ58";
                                    days89 = 45;
                                    break;
                                case 32:
                                    itemcode91 = "DJ58";
                                    days89 = 30;
                                    break;
                                case 33:
                                    itemcode91 = "DJ58";
                                    days89 = 15;
                                    break;
                                case 34:
                                    itemcode91 = "DJ58";
                                    days89 = 7;
                                    break;
                                case 35:
                                    itemcode91 = "DS01";
                                    days89 = 30;
                                    break;
                                case 36:
                                    itemcode91 = "DS10";
                                    days89 = 15;
                                    break;
                                case 37:
                                    itemcode91 = "CB09";
                                    days89 = 1;
                                    break;
                                case 38:
                                    itemcode91 = "DT19";
                                    days89 = 60;
                                    break;
                                case 39:
                                    itemcode91 = "CI01";
                                    days89 = 7;
                                    break;
                                case 40:
                                    itemcode91 = "CF01";
                                    days89 = 15;
                                    break;
                                case 41:
                                    itemcode91 = "CF02";
                                    days89 = 10;
                                    break;
                                case 42:
                                    itemcode91 = "DU02";
                                    days89 = 15;
                                    break;
                                case 43:
                                    itemcode91 = "DS03";
                                    days89 = 15;
                                    break;
                                case 44:
                                    itemcode91 = "CD02";
                                    days89 = 15;
                                    break;
                                case 45:
                                    itemcode91 = "CD01";
                                    days89 = 15;
                                    break;
                                case 46:
                                    itemcode91 = "CD03";
                                    days89 = 15;
                                    break;
                                case 47:
                                    itemcode91 = "CD04";
                                    days89 = 15;
                                    break;
                                case 48:
                                    itemcode91 = "CD05";
                                    days89 = 15;
                                    break;
                                case 49:
                                    itemcode91 = "CD06";
                                    days89 = 15;
                                    break;
                                case 50:
                                    itemcode91 = "CD07";
                                    days89 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode91, days89);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode91, days89));
                            return;
                        case "CY39":
                            int num98 = Game_Server.Generic.random(0, 24);
                            int days90 = 1;
                            string itemcode92 = (string)null;
                            switch (num98)
                            {
                                case 0:
                                    itemcode92 = "BF5A";
                                    days90 = 365;
                                    break;
                                case 1:
                                    itemcode92 = "BF5A";
                                    days90 = 90;
                                    break;
                                case 2:
                                    itemcode92 = "BF5A";
                                    days90 = 30;
                                    break;
                                case 3:
                                    itemcode92 = "BF5B";
                                    days90 = 365;
                                    break;
                                case 4:
                                    itemcode92 = "BF5B";
                                    days90 = 90;
                                    break;
                                case 5:
                                    itemcode92 = "BF5B";
                                    days90 = 30;
                                    break;
                                case 6:
                                    itemcode92 = "BF4A";
                                    days90 = 365;
                                    break;
                                case 7:
                                    itemcode92 = "BF4A";
                                    days90 = 90;
                                    break;
                                case 8:
                                    itemcode92 = "BF4A";
                                    days90 = 30;
                                    break;
                                case 9:
                                    itemcode92 = "BF4B";
                                    days90 = 365;
                                    break;
                                case 10:
                                    itemcode92 = "BF4B";
                                    days90 = 90;
                                    break;
                                case 11:
                                    itemcode92 = "BF4B";
                                    days90 = 30;
                                    break;
                                case 12:
                                    itemcode92 = "BF5E";
                                    days90 = 365;
                                    break;
                                case 13:
                                    itemcode92 = "BF5E";
                                    days90 = 90;
                                    break;
                                case 14:
                                    itemcode92 = "BF5E";
                                    days90 = 30;
                                    break;
                                case 15:
                                    itemcode92 = "BF5F";
                                    days90 = 365;
                                    break;
                                case 16:
                                    itemcode92 = "BF5F";
                                    days90 = 90;
                                    break;
                                case 17:
                                    itemcode92 = "BF5F";
                                    days90 = 30;
                                    break;
                                case 18:
                                    itemcode92 = "CZ81";
                                    days90 = 1;
                                    break;
                                case 19:
                                    itemcode92 = "CB09";
                                    days90 = 1;
                                    break;
                                case 20:
                                    itemcode92 = "CZ75";
                                    days90 = 1;
                                    break;
                                case 21:
                                    itemcode92 = "CD01";
                                    days90 = 7;
                                    break;
                                case 22:
                                    itemcode92 = "DS03";
                                    days90 = 7;
                                    break;
                                case 23:
                                    itemcode92 = "DS01";
                                    days90 = 7;
                                    break;
                                case 24:
                                    itemcode92 = "CF01";
                                    days90 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode92, days90);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode92, days90));
                            return;
                        case "CY43":
                            int num99 = Game_Server.Generic.random(0, 27);
                            int days91 = 1;
                            string itemcode93 = (string)null;
                            int num100;
                            switch (num99)
                            {
                                case 0:
                                    itemcode93 = "GF17";
                                    num100 = 365;
                                    break;
                                case 1:
                                    itemcode93 = "GF17";
                                    num100 = 90;
                                    break;
                                case 2:
                                    itemcode93 = "GF17";
                                    num100 = 60;
                                    break;
                                case 3:
                                    itemcode93 = "GF17";
                                    num100 = 30;
                                    break;
                                case 4:
                                    itemcode93 = "GF17";
                                    num100 = 15;
                                    break;
                                case 5:
                                    itemcode93 = "GG05";
                                    num100 = 365;
                                    break;
                                case 7:
                                    itemcode93 = "GG05";
                                    num100 = 90;
                                    break;
                                case 8:
                                    itemcode93 = "GG05";
                                    num100 = 60;
                                    break;
                                case 9:
                                    itemcode93 = "GG05";
                                    num100 = 30;
                                    break;
                                case 10:
                                    itemcode93 = "GG05";
                                    num100 = 15;
                                    break;
                                case 11:
                                    itemcode93 = "DH14";
                                    num100 = 365;
                                    break;
                                case 12:
                                    itemcode93 = "DH14";
                                    num100 = 90;
                                    break;
                                case 13:
                                    itemcode93 = "DH14";
                                    num100 = 60;
                                    break;
                                case 14:
                                    itemcode93 = "DH14";
                                    num100 = 30;
                                    break;
                                case 15:
                                    itemcode93 = "DH14";
                                    num100 = 15;
                                    break;
                                case 16:
                                    itemcode93 = "DJ60";
                                    num100 = 365;
                                    break;
                                case 17:
                                    itemcode93 = "DJ60";
                                    num100 = 90;
                                    break;
                                case 18:
                                    itemcode93 = "DJ60";
                                    num100 = 60;
                                    break;
                                case 19:
                                    itemcode93 = "DJ60";
                                    num100 = 30;
                                    break;
                                case 20:
                                    itemcode93 = "DJ60";
                                    num100 = 15;
                                    break;
                                case 21:
                                    itemcode93 = "CZ73";
                                    num100 = 7;
                                    break;
                                case 22:
                                    itemcode93 = "DS01";
                                    num100 = 7;
                                    break;
                                case 23:
                                    itemcode93 = "CI01";
                                    num100 = 7;
                                    break;
                                case 24:
                                    itemcode93 = "DS10";
                                    num100 = 15;
                                    break;
                                case 25:
                                    itemcode93 = "CF01";
                                    num100 = 15;
                                    break;
                                case 26:
                                    itemcode93 = "CZ85";
                                    num100 = 15;
                                    break;
                                case 27:
                                    itemcode93 = "CD02";
                                    num100 = 7;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode93, days91);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode93, days91));
                            return;
                        case "CY44":
                            int num101 = Game_Server.Generic.random(0, 15);
                            int days92 = 1;
                            string itemcode94 = (string)null;
                            switch (num101)
                            {
                                case 0:
                                    itemcode94 = "DF42";
                                    days92 = 30;
                                    break;
                                case 1:
                                    itemcode94 = "DF42";
                                    days92 = -1;
                                    break;
                                case 2:
                                    itemcode94 = "CI01";
                                    days92 = 30;
                                    break;
                                case 3:
                                    itemcode94 = "DC31";
                                    days92 = 30;
                                    break;
                                case 4:
                                    itemcode94 = "DD01";
                                    days92 = 30;
                                    break;
                                case 5:
                                    itemcode94 = "CF02";
                                    days92 = 30;
                                    break;
                                case 6:
                                    itemcode94 = "CF01";
                                    days92 = 30;
                                    break;
                                case 7:
                                    itemcode94 = "DD01";
                                    days92 = 30;
                                    break;
                                case 8:
                                    itemcode94 = "DJ14";
                                    days92 = 30;
                                    break;
                                case 9:
                                    itemcode94 = "DF06";
                                    days92 = 30;
                                    break;
                                case 10:
                                    itemcode94 = "DG03";
                                    days92 = 30;
                                    break;
                                case 11:
                                    itemcode94 = "DG31";
                                    days92 = 30;
                                    break;
                                case 12:
                                    itemcode94 = "DS03";
                                    days92 = 30;
                                    break;
                                case 13:
                                    itemcode94 = "DU01";
                                    days92 = 30;
                                    break;
                                case 14:
                                    itemcode94 = "DS03";
                                    days92 = 30;
                                    break;
                                case 15:
                                    itemcode94 = "D601";
                                    days92 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode94, days92);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode94, days92));
                            return;
                        case "CY45":
                            int num102 = Game_Server.Generic.random(0, 21);
                            int days93 = 1;
                            string itemcode95 = (string)null;
                            switch (num102)
                            {
                                case 0:
                                    itemcode95 = "DB53";
                                    days93 = 30;
                                    break;
                                case 1:
                                    itemcode95 = "DB53";
                                    days93 = -1;
                                    break;
                                case 2:
                                    itemcode95 = "GF20";
                                    days93 = 30;
                                    break;
                                case 3:
                                    itemcode95 = "GF20";
                                    days93 = -1;
                                    break;
                                case 4:
                                    itemcode95 = "DE91";
                                    days93 = 30;
                                    break;
                                case 5:
                                    itemcode95 = "DE91";
                                    days93 = -1;
                                    break;
                                case 6:
                                    itemcode95 = "GG09";
                                    days93 = 30;
                                    break;
                                case 7:
                                    itemcode95 = "GG09";
                                    days93 = -1;
                                    break;
                                case 8:
                                    itemcode95 = "DJ62";
                                    days93 = 30;
                                    break;
                                case 9:
                                    itemcode95 = "DJ62";
                                    days93 = -1;
                                    break;
                                case 10:
                                    itemcode95 = "D608";
                                    days93 = 30;
                                    break;
                                case 11:
                                    itemcode95 = "D608";
                                    days93 = -1;
                                    break;
                                case 12:
                                    itemcode95 = "D832";
                                    days93 = 30;
                                    break;
                                case 13:
                                    itemcode95 = "D832";
                                    days93 = -1;
                                    break;
                                case 14:
                                    itemcode95 = "D911";
                                    days93 = 30;
                                    break;
                                case 15:
                                    itemcode95 = "D911";
                                    days93 = -1;
                                    break;
                                case 16:
                                    itemcode95 = "CB09";
                                    days93 = 1;
                                    break;
                                case 17:
                                    itemcode95 = "CK02";
                                    days93 = 1;
                                    break;
                                case 18:
                                    itemcode95 = "CI01";
                                    days93 = 30;
                                    break;
                                case 19:
                                    itemcode95 = "CA04";
                                    days93 = 30;
                                    break;
                                case 20:
                                    itemcode95 = "CZ75";
                                    days93 = 1;
                                    break;
                                case 21:
                                    itemcode95 = "BA26";
                                    days93 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode95, days93);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode95, days93));
                            return;
                        case "CY46":
                            int num103 = Game_Server.Generic.random(0, 63);
                            int days94 = 1;
                            string itemcode96 = (string)null;
                            switch (num103)
                            {
                                case 0:
                                    itemcode96 = "GF53";
                                    days94 = 30;
                                    break;
                                case 1:
                                    itemcode96 = "GF53";
                                    days94 = -1;
                                    break;
                                case 2:
                                    itemcode96 = "GA09";
                                    days94 = -1;
                                    break;
                                case 3:
                                    itemcode96 = "GA09";
                                    days94 = 30;
                                    break;
                                case 4:
                                    itemcode96 = "GF51";
                                    days94 = 30;
                                    break;
                                case 5:
                                    itemcode96 = "GF51";
                                    days94 = -1;
                                    break;
                                case 6:
                                    itemcode96 = "GF37";
                                    days94 = -1;
                                    break;
                                case 7:
                                    itemcode96 = "GF37";
                                    days94 = 30;
                                    break;
                                case 8:
                                    itemcode96 = "DF92";
                                    days94 = -1;
                                    break;
                                case 9:
                                    itemcode96 = "DF92";
                                    days94 = 30;
                                    break;
                                case 10:
                                    itemcode96 = "DF86";
                                    days94 = 30;
                                    break;
                                case 11:
                                    itemcode96 = "DF86";
                                    days94 = -1;
                                    break;
                                case 12:
                                    itemcode96 = "DF99";
                                    days94 = 30;
                                    break;
                                case 13:
                                    itemcode96 = "DF99";
                                    days94 = -1;
                                    break;
                                case 14:
                                    itemcode96 = "GF24";
                                    days94 = 30;
                                    break;
                                case 15:
                                    itemcode96 = "GF24";
                                    days94 = -1;
                                    break;
                                case 16:
                                    itemcode96 = "GF54";
                                    days94 = 30;
                                    break;
                                case 17:
                                    itemcode96 = "GF54";
                                    days94 = -1;
                                    break;
                                case 18:
                                    itemcode96 = "GA10";
                                    days94 = 30;
                                    break;
                                case 19:
                                    itemcode96 = "GA10";
                                    days94 = -1;
                                    break;
                                case 20:
                                    itemcode96 = "GF56";
                                    days94 = 30;
                                    break;
                                case 21:
                                    itemcode96 = "GF56";
                                    days94 = -1;
                                    break;
                                case 22:
                                    itemcode96 = "GF19";
                                    days94 = 30;
                                    break;
                                case 23:
                                    itemcode96 = "GF19";
                                    days94 = -1;
                                    break;
                                case 24:
                                    itemcode96 = "GF28";
                                    days94 = 30;
                                    break;
                                case 25:
                                    itemcode96 = "GF28";
                                    days94 = -1;
                                    break;
                                case 26:
                                    itemcode96 = "GF40";
                                    days94 = 30;
                                    break;
                                case 27:
                                    itemcode96 = "GF40";
                                    days94 = -1;
                                    break;
                                case 28:
                                    itemcode96 = "GF55";
                                    days94 = 30;
                                    break;
                                case 29:
                                    itemcode96 = "GF55";
                                    days94 = -1;
                                    break;
                                case 30:
                                    itemcode96 = "GF56";
                                    days94 = 30;
                                    break;
                                case 31:
                                    itemcode96 = "GF56";
                                    days94 = -1;
                                    break;
                                case 32:
                                    itemcode96 = "GF19";
                                    days94 = 30;
                                    break;
                                case 33:
                                    itemcode96 = "GF19";
                                    days94 = -1;
                                    break;
                                case 34:
                                    itemcode96 = "GF28";
                                    days94 = 30;
                                    break;
                                case 35:
                                    itemcode96 = "GF28";
                                    days94 = -1;
                                    break;
                                case 36:
                                    itemcode96 = "GF40";
                                    days94 = 30;
                                    break;
                                case 37:
                                    itemcode96 = "GF40";
                                    days94 = -1;
                                    break;
                                case 38:
                                    itemcode96 = "DB75";
                                    days94 = 30;
                                    break;
                                case 39:
                                    itemcode96 = "DB75";
                                    days94 = -1;
                                    break;
                                case 40:
                                    itemcode96 = "DB74";
                                    days94 = 30;
                                    break;
                                case 41:
                                    itemcode96 = "DB74";
                                    days94 = -1;
                                    break;
                                case 42:
                                    itemcode96 = "GF48";
                                    days94 = 30;
                                    break;
                                case 43:
                                    itemcode96 = "GF48";
                                    days94 = -1;
                                    break;
                                case 44:
                                    itemcode96 = "GF52";
                                    days94 = 30;
                                    break;
                                case 45:
                                    itemcode96 = "GF52";
                                    days94 = -1;
                                    break;
                                case 46:
                                    itemcode96 = "DN05";
                                    days94 = 30;
                                    break;
                                case 47:
                                    itemcode96 = "DN05";
                                    days94 = -1;
                                    break;
                                case 48:
                                    itemcode96 = "DM09";
                                    days94 = 30;
                                    break;
                                case 49:
                                    itemcode96 = "DM09";
                                    days94 = -1;
                                    break;
                                case 50:
                                    itemcode96 = "GF57";
                                    days94 = 30;
                                    break;
                                case 51:
                                    itemcode96 = "GF57";
                                    days94 = -1;
                                    break;
                                case 52:
                                    itemcode96 = "DB77";
                                    days94 = -1;
                                    break;
                                case 53:
                                    itemcode96 = "DB77";
                                    days94 = -1;
                                    break;
                                case 54:
                                    itemcode96 = "GF59";
                                    days94 = -1;
                                    break;
                                case 55:
                                    itemcode96 = "GF59";
                                    days94 = -1;
                                    break;
                                case 56:
                                    itemcode96 = "DB78";
                                    days94 = -1;
                                    break;
                                case 57:
                                    itemcode96 = "DB78";
                                    days94 = -1;
                                    break;
                                case 58:
                                    itemcode96 = "DF51";
                                    days94 = 30;
                                    break;
                                case 59:
                                    itemcode96 = "DF51";
                                    days94 = -1;
                                    break;
                                case 60:
                                    itemcode96 = "GF44";
                                    days94 = 30;
                                    break;
                                case 61:
                                    itemcode96 = "GF44";
                                    days94 = -1;
                                    break;
                                case 62:
                                    itemcode96 = "GF30";
                                    days94 = 30;
                                    break;
                                case 63:
                                    itemcode96 = "GF30";
                                    days94 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode96, days94);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode96, days94));
                            return;
                        case "CY47":
                            int num104 = Game_Server.Generic.random(0, 53);
                            int days95 = 1;
                            string itemcode97 = (string)null;
                            switch (num104)
                            {
                                case 0:
                                    itemcode97 = "GG45";
                                    days95 = 30;
                                    break;
                                case 1:
                                    itemcode97 = "GG45";
                                    days95 = -1;
                                    break;
                                case 2:
                                    itemcode97 = "GA09";
                                    days95 = -1;
                                    break;
                                case 3:
                                    itemcode97 = "GA09";
                                    days95 = 30;
                                    break;
                                case 4:
                                    itemcode97 = "GG43";
                                    days95 = 30;
                                    break;
                                case 5:
                                    itemcode97 = "GG43";
                                    days95 = -1;
                                    break;
                                case 6:
                                    itemcode97 = "GG29";
                                    days95 = -1;
                                    break;
                                case 7:
                                    itemcode97 = "GG29";
                                    days95 = 30;
                                    break;
                                case 8:
                                    itemcode97 = "DG83";
                                    days95 = -1;
                                    break;
                                case 9:
                                    itemcode97 = "DG83";
                                    days95 = 30;
                                    break;
                                case 10:
                                    itemcode97 = "DG79";
                                    days95 = 30;
                                    break;
                                case 11:
                                    itemcode97 = "DG79";
                                    days95 = -1;
                                    break;
                                case 12:
                                    itemcode97 = "DG70";
                                    days95 = 30;
                                    break;
                                case 13:
                                    itemcode97 = "DG70";
                                    days95 = -1;
                                    break;
                                case 14:
                                    itemcode97 = "DG88";
                                    days95 = 30;
                                    break;
                                case 15:
                                    itemcode97 = "DG88";
                                    days95 = -1;
                                    break;
                                case 16:
                                    itemcode97 = "GG15";
                                    days95 = 30;
                                    break;
                                case 17:
                                    itemcode97 = "GG15";
                                    days95 = -1;
                                    break;
                                case 18:
                                    itemcode97 = "GG46";
                                    days95 = 30;
                                    break;
                                case 19:
                                    itemcode97 = "GG46";
                                    days95 = -1;
                                    break;
                                case 20:
                                    itemcode97 = "GA10";
                                    days95 = 30;
                                    break;
                                case 21:
                                    itemcode97 = "GA10";
                                    days95 = -1;
                                    break;
                                case 22:
                                    itemcode97 = "GG08";
                                    days95 = 30;
                                    break;
                                case 23:
                                    itemcode97 = "GG08";
                                    days95 = -1;
                                    break;
                                case 24:
                                    itemcode97 = "GG49";
                                    days95 = 30;
                                    break;
                                case 25:
                                    itemcode97 = "GG49";
                                    days95 = -1;
                                    break;
                                case 26:
                                    itemcode97 = "DB76";
                                    days95 = 30;
                                    break;
                                case 27:
                                    itemcode97 = "DB76";
                                    days95 = -1;
                                    break;
                                case 28:
                                    itemcode97 = "GG47";
                                    days95 = 30;
                                    break;
                                case 29:
                                    itemcode97 = "GG47";
                                    days95 = -1;
                                    break;
                                case 30:
                                    itemcode97 = "DB75";
                                    days95 = 30;
                                    break;
                                case 31:
                                    itemcode97 = "DB75";
                                    days95 = -1;
                                    break;
                                case 32:
                                    itemcode97 = "DB74";
                                    days95 = 30;
                                    break;
                                case 33:
                                    itemcode97 = "DB74";
                                    days95 = -1;
                                    break;
                                case 34:
                                    itemcode97 = "GG40";
                                    days95 = 30;
                                    break;
                                case 35:
                                    itemcode97 = "GG40";
                                    days95 = -1;
                                    break;
                                case 36:
                                    itemcode97 = "DM05";
                                    days95 = 30;
                                    break;
                                case 37:
                                    itemcode97 = "DM05";
                                    days95 = -1;
                                    break;
                                case 38:
                                    itemcode97 = "DN05";
                                    days95 = 30;
                                    break;
                                case 39:
                                    itemcode97 = "DN05";
                                    days95 = -1;
                                    break;
                                case 40:
                                    itemcode97 = "DB77";
                                    days95 = 30;
                                    break;
                                case 41:
                                    itemcode97 = "DB77";
                                    days95 = -1;
                                    break;
                                case 42:
                                    itemcode97 = "GG50";
                                    days95 = -1;
                                    break;
                                case 43:
                                    itemcode97 = "GG50";
                                    days95 = 30;
                                    break;
                                case 44:
                                    itemcode97 = "DB78";
                                    days95 = -1;
                                    break;
                                case 45:
                                    itemcode97 = "DB78";
                                    days95 = 30;
                                    break;
                                case 46:
                                    itemcode97 = "GG52";
                                    days95 = -1;
                                    break;
                                case 47:
                                    itemcode97 = "GG52";
                                    days95 = 30;
                                    break;
                                case 48:
                                    itemcode97 = "DG50";
                                    days95 = -1;
                                    break;
                                case 49:
                                    itemcode97 = "DG50";
                                    days95 = 30;
                                    break;
                                case 50:
                                    itemcode97 = "GG21";
                                    days95 = -1;
                                    break;
                                case 51:
                                    itemcode97 = "GG21";
                                    days95 = 30;
                                    break;
                                case 52:
                                    itemcode97 = "GG34";
                                    days95 = -1;
                                    break;
                                case 53:
                                    itemcode97 = "GG34";
                                    days95 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode97, days95);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode97, days95));
                            return;
                        case "CY48":
                            int num105 = Game_Server.Generic.random(0, 63);
                            int days96 = 1;
                            string itemcode98 = (string)null;
                            switch (num105)
                            {
                                case 0:
                                    itemcode98 = "GE06";
                                    days96 = 30;
                                    break;
                                case 1:
                                    itemcode98 = "GE06";
                                    days96 = -1;
                                    break;
                                case 2:
                                    itemcode98 = "GA09";
                                    days96 = -1;
                                    break;
                                case 3:
                                    itemcode98 = "GA09";
                                    days96 = 30;
                                    break;
                                case 4:
                                    itemcode98 = "GE05";
                                    days96 = 30;
                                    break;
                                case 5:
                                    itemcode98 = "GE05";
                                    days96 = -1;
                                    break;
                                case 6:
                                    itemcode98 = "GC09";
                                    days96 = -1;
                                    break;
                                case 7:
                                    itemcode98 = "GC09";
                                    days96 = 30;
                                    break;
                                case 8:
                                    itemcode98 = "DE63";
                                    days96 = -1;
                                    break;
                                case 9:
                                    itemcode98 = "DE63";
                                    days96 = 30;
                                    break;
                                case 10:
                                    itemcode98 = "DE59";
                                    days96 = 30;
                                    break;
                                case 11:
                                    itemcode98 = "DE59";
                                    days96 = -1;
                                    break;
                                case 12:
                                    itemcode98 = "DE22";
                                    days96 = 30;
                                    break;
                                case 13:
                                    itemcode98 = "DE22";
                                    days96 = -1;
                                    break;
                                case 14:
                                    itemcode98 = "DE69";
                                    days96 = 30;
                                    break;
                                case 15:
                                    itemcode98 = "DE69";
                                    days96 = -1;
                                    break;
                                case 16:
                                    itemcode98 = "DE96";
                                    days96 = 30;
                                    break;
                                case 17:
                                    itemcode98 = "DE96";
                                    days96 = -1;
                                    break;
                                case 18:
                                    itemcode98 = "GA10";
                                    days96 = 30;
                                    break;
                                case 19:
                                    itemcode98 = "GA10";
                                    days96 = -1;
                                    break;
                                case 20:
                                    itemcode98 = "GE07";
                                    days96 = 30;
                                    break;
                                case 21:
                                    itemcode98 = "GE07";
                                    days96 = -1;
                                    break;
                                case 22:
                                    itemcode98 = "GC23";
                                    days96 = 30;
                                    break;
                                case 23:
                                    itemcode98 = "GC23";
                                    days96 = -1;
                                    break;
                                case 24:
                                    itemcode98 = "DE90";
                                    days96 = 30;
                                    break;
                                case 25:
                                    itemcode98 = "DE90";
                                    days96 = -1;
                                    break;
                                case 26:
                                    itemcode98 = "GC22";
                                    days96 = 30;
                                    break;
                                case 27:
                                    itemcode98 = "GC22";
                                    days96 = -1;
                                    break;
                                case 28:
                                    itemcode98 = "DD34";
                                    days96 = 30;
                                    break;
                                case 29:
                                    itemcode98 = "DD34";
                                    days96 = -1;
                                    break;
                                case 30:
                                    itemcode98 = "DB76";
                                    days96 = 30;
                                    break;
                                case 31:
                                    itemcode98 = "DB76";
                                    days96 = -1;
                                    break;
                                case 32:
                                    itemcode98 = "GE01";
                                    days96 = 30;
                                    break;
                                case 33:
                                    itemcode98 = "GE01";
                                    days96 = -1;
                                    break;
                                case 34:
                                    itemcode98 = "GE08";
                                    days96 = 30;
                                    break;
                                case 35:
                                    itemcode98 = "GE08";
                                    days96 = -1;
                                    break;
                                case 36:
                                    itemcode98 = "DB76";
                                    days96 = 30;
                                    break;
                                case 37:
                                    itemcode98 = "DB75";
                                    days96 = -1;
                                    break;
                                case 38:
                                    itemcode98 = "GE02";
                                    days96 = 30;
                                    break;
                                case 39:
                                    itemcode98 = "GE02";
                                    days96 = -1;
                                    break;
                                case 40:
                                    itemcode98 = "DB74";
                                    days96 = 30;
                                    break;
                                case 41:
                                    itemcode98 = "DB74";
                                    days96 = -1;
                                    break;
                                case 42:
                                    itemcode98 = "GC18";
                                    days96 = 30;
                                    break;
                                case 43:
                                    itemcode98 = "GC18";
                                    days96 = -1;
                                    break;
                                case 44:
                                    itemcode98 = "DM05";
                                    days96 = 30;
                                    break;
                                case 45:
                                    itemcode98 = "DM05";
                                    days96 = -1;
                                    break;
                                case 46:
                                    itemcode98 = "DN05";
                                    days96 = 30;
                                    break;
                                case 47:
                                    itemcode98 = "DN05";
                                    days96 = -1;
                                    break;
                                case 48:
                                    itemcode98 = "GC24";
                                    days96 = -1;
                                    break;
                                case 49:
                                    itemcode98 = "GC24";
                                    days96 = 30;
                                    break;
                                case 50:
                                    itemcode98 = "GF57";
                                    days96 = -1;
                                    break;
                                case 51:
                                    itemcode98 = "GF57";
                                    days96 = 30;
                                    break;
                                case 52:
                                    itemcode98 = "GE10";
                                    days96 = -1;
                                    break;
                                case 53:
                                    itemcode98 = "GE10";
                                    days96 = 30;
                                    break;
                                case 54:
                                    itemcode98 = "DH20";
                                    days96 = -1;
                                    break;
                                case 55:
                                    itemcode98 = "DH20";
                                    days96 = 30;
                                    break;
                                case 56:
                                    itemcode98 = "DB78";
                                    days96 = -1;
                                    break;
                                case 57:
                                    itemcode98 = "DB78";
                                    days96 = 30;
                                    break;
                                case 58:
                                    itemcode98 = "GC03";
                                    days96 = -1;
                                    break;
                                case 59:
                                    itemcode98 = "GC03";
                                    days96 = 30;
                                    break;
                                case 60:
                                    itemcode98 = "DE92";
                                    days96 = -1;
                                    break;
                                case 61:
                                    itemcode98 = "DE92";
                                    days96 = 30;
                                    break;
                                case 62:
                                    itemcode98 = "GC13";
                                    days96 = -1;
                                    break;
                                case 63:
                                    itemcode98 = "GC13";
                                    days96 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode98, days96);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode98, days96));
                            return;
                        case "CY49":
                            int num106 = Game_Server.Generic.random(0, 59);
                            int days97 = 1;
                            string itemcode99 = (string)null;
                            switch (num106)
                            {
                                case 0:
                                    itemcode99 = "DT07";
                                    days97 = 30;
                                    break;
                                case 1:
                                    itemcode99 = "DT07";
                                    days97 = -1;
                                    break;
                                case 2:
                                    itemcode99 = "GA09";
                                    days97 = -1;
                                    break;
                                case 3:
                                    itemcode99 = "GA09";
                                    days97 = 30;
                                    break;
                                case 4:
                                    itemcode99 = "DJ78";
                                    days97 = 30;
                                    break;
                                case 5:
                                    itemcode99 = "DJ78";
                                    days97 = -1;
                                    break;
                                case 6:
                                    itemcode99 = "DH18";
                                    days97 = -1;
                                    break;
                                case 7:
                                    itemcode99 = "DH18";
                                    days97 = 30;
                                    break;
                                case 8:
                                    itemcode99 = "DJ41";
                                    days97 = -1;
                                    break;
                                case 9:
                                    itemcode99 = "DJ41";
                                    days97 = 30;
                                    break;
                                case 10:
                                    itemcode99 = "DJ37";
                                    days97 = 30;
                                    break;
                                case 11:
                                    itemcode99 = "DJ37";
                                    days97 = -1;
                                    break;
                                case 12:
                                    itemcode99 = "DJ30";
                                    days97 = 30;
                                    break;
                                case 13:
                                    itemcode99 = "DJ30";
                                    days97 = -1;
                                    break;
                                case 14:
                                    itemcode99 = "DJ44";
                                    days97 = 30;
                                    break;
                                case 15:
                                    itemcode99 = "DJ44";
                                    days97 = -1;
                                    break;
                                case 16:
                                    itemcode99 = "DT40";
                                    days97 = 30;
                                    break;
                                case 17:
                                    itemcode99 = "DT40";
                                    days97 = -1;
                                    break;
                                case 18:
                                    itemcode99 = "GA10";
                                    days97 = 30;
                                    break;
                                case 19:
                                    itemcode99 = "GA10";
                                    days97 = -1;
                                    break;
                                case 20:
                                    itemcode99 = "DH19";
                                    days97 = 30;
                                    break;
                                case 21:
                                    itemcode99 = "DH19";
                                    days97 = -1;
                                    break;
                                case 22:
                                    itemcode99 = "DH10";
                                    days97 = 30;
                                    break;
                                case 23:
                                    itemcode99 = "DH10";
                                    days97 = -1;
                                    break;
                                case 24:
                                    itemcode99 = "DT38";
                                    days97 = 30;
                                    break;
                                case 25:
                                    itemcode99 = "DT38";
                                    days97 = -1;
                                    break;
                                case 26:
                                    itemcode99 = "DJ17";
                                    days97 = 30;
                                    break;
                                case 27:
                                    itemcode99 = "DJ17";
                                    days97 = -1;
                                    break;
                                case 28:
                                    itemcode99 = "DT41";
                                    days97 = 30;
                                    break;
                                case 29:
                                    itemcode99 = "DT41";
                                    days97 = -1;
                                    break;
                                case 30:
                                    itemcode99 = "DT22";
                                    days97 = 30;
                                    break;
                                case 31:
                                    itemcode99 = "DT22";
                                    days97 = -1;
                                    break;
                                case 32:
                                    itemcode99 = "DJ73";
                                    days97 = 30;
                                    break;
                                case 33:
                                    itemcode99 = "DJ73";
                                    days97 = -1;
                                    break;
                                case 34:
                                    itemcode99 = "DJ47";
                                    days97 = 30;
                                    break;
                                case 35:
                                    itemcode99 = "DJ47";
                                    days97 = -1;
                                    break;
                                case 36:
                                    itemcode99 = "D913";
                                    days97 = 30;
                                    break;
                                case 37:
                                    itemcode99 = "D913";
                                    days97 = -1;
                                    break;
                                case 38:
                                    itemcode99 = "DH19";
                                    days97 = 30;
                                    break;
                                case 39:
                                    itemcode99 = "DH19";
                                    days97 = -1;
                                    break;
                                case 40:
                                    itemcode99 = "DB74";
                                    days97 = 30;
                                    break;
                                case 41:
                                    itemcode99 = "DB74";
                                    days97 = -1;
                                    break;
                                case 42:
                                    itemcode99 = "DM05";
                                    days97 = 30;
                                    break;
                                case 43:
                                    itemcode99 = "DM05";
                                    days97 = -1;
                                    break;
                                case 44:
                                    itemcode99 = "DN05";
                                    days97 = 30;
                                    break;
                                case 45:
                                    itemcode99 = "DN05";
                                    days97 = -1;
                                    break;
                                case 46:
                                    itemcode99 = "DB77";
                                    days97 = 30;
                                    break;
                                case 47:
                                    itemcode99 = "DB77";
                                    days97 = -1;
                                    break;
                                case 48:
                                    itemcode99 = "DK04";
                                    days97 = -1;
                                    break;
                                case 49:
                                    itemcode99 = "DK04";
                                    days97 = -1;
                                    break;
                                case 50:
                                    itemcode99 = "DH20";
                                    days97 = -1;
                                    break;
                                case 51:
                                    itemcode99 = "DH20";
                                    days97 = -1;
                                    break;
                                case 52:
                                    itemcode99 = "DB78";
                                    days97 = -1;
                                    break;
                                case 53:
                                    itemcode99 = "DB78";
                                    days97 = -1;
                                    break;
                                case 54:
                                    itemcode99 = "DJ64";
                                    days97 = -1;
                                    break;
                                case 55:
                                    itemcode99 = "DJ64";
                                    days97 = 30;
                                    break;
                                case 56:
                                    itemcode99 = "DT36";
                                    days97 = -1;
                                    break;
                                case 57:
                                    itemcode99 = "DT36";
                                    days97 = 30;
                                    break;
                                case 58:
                                    itemcode99 = "DT26";
                                    days97 = 30;
                                    break;
                                case 59:
                                    itemcode99 = "DT26";
                                    days97 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode99, days97);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode99, days97));
                            return;
                        case "CY50":
                            int num107 = Game_Server.Generic.random(0, 23);
                            int days98 = 1;
                            string itemcode100 = (string)null;
                            switch (num107)
                            {
                                case 0:
                                    itemcode100 = "BH2A";
                                    days98 = 30;
                                    break;
                                case 1:
                                    itemcode100 = "BH2A";
                                    days98 = -1;
                                    break;
                                case 2:
                                    itemcode100 = "BH2A";
                                    days98 = 15;
                                    break;
                                case 3:
                                    itemcode100 = "BH2E";
                                    days98 = 30;
                                    break;
                                case 4:
                                    itemcode100 = "BH2E";
                                    days98 = 15;
                                    break;
                                case 5:
                                    itemcode100 = "BH2E";
                                    days98 = -1;
                                    break;
                                case 6:
                                    itemcode100 = "BH2B";
                                    days98 = 30;
                                    break;
                                case 7:
                                    itemcode100 = "BH2B";
                                    days98 = 15;
                                    break;
                                case 8:
                                    itemcode100 = "BH2B";
                                    days98 = -1;
                                    break;
                                case 9:
                                    itemcode100 = "BH2B";
                                    days98 = 30;
                                    break;
                                case 10:
                                    itemcode100 = "BH2B";
                                    days98 = 15;
                                    break;
                                case 11:
                                    itemcode100 = "BH2B";
                                    days98 = -1;
                                    break;
                                case 12:
                                    itemcode100 = "BH32";
                                    days98 = 30;
                                    break;
                                case 13:
                                    itemcode100 = "BH32";
                                    days98 = 15;
                                    break;
                                case 14:
                                    itemcode100 = "BH32";
                                    days98 = -1;
                                    break;
                                case 15:
                                    itemcode100 = "BH34";
                                    days98 = 30;
                                    break;
                                case 16:
                                    itemcode100 = "BH34";
                                    days98 = 15;
                                    break;
                                case 17:
                                    itemcode100 = "BH34";
                                    days98 = -1;
                                    break;
                                case 18:
                                    itemcode100 = "BH29";
                                    days98 = 30;
                                    break;
                                case 19:
                                    itemcode100 = "BH29";
                                    days98 = 15;
                                    break;
                                case 20:
                                    itemcode100 = "BH29";
                                    days98 = -1;
                                    break;
                                case 21:
                                    itemcode100 = "BH2D";
                                    days98 = 30;
                                    break;
                                case 22:
                                    itemcode100 = "BH2D";
                                    days98 = 15;
                                    break;
                                case 23:
                                    itemcode100 = "BH2D";
                                    days98 = -1;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode100, days98);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode100, days98));
                            return;
                        case "CY51":
                            int num108 = Game_Server.Generic.random(0, 21);
                            int days99 = 1;
                            string itemcode101 = (string)null;
                            switch (num108)
                            {
                                case 0:
                                    itemcode101 = "BH10";
                                    days99 = 30;
                                    break;
                                case 1:
                                    itemcode101 = "BH10";
                                    days99 = -1;
                                    break;
                                case 2:
                                    itemcode101 = "BH11";
                                    days99 = 30;
                                    break;
                                case 3:
                                    itemcode101 = "BH11";
                                    days99 = -1;
                                    break;
                                case 4:
                                    itemcode101 = "BH12";
                                    days99 = 30;
                                    break;
                                case 5:
                                    itemcode101 = "BH12";
                                    days99 = -1;
                                    break;
                                case 6:
                                    itemcode101 = "BH13";
                                    days99 = 30;
                                    break;
                                case 7:
                                    itemcode101 = "BH13";
                                    days99 = -1;
                                    break;
                                case 8:
                                    itemcode101 = "BH14";
                                    days99 = 30;
                                    break;
                                case 9:
                                    itemcode101 = "BH14";
                                    days99 = -1;
                                    break;
                                case 10:
                                    itemcode101 = "BH15";
                                    days99 = 30;
                                    break;
                                case 11:
                                    itemcode101 = "BH15";
                                    days99 = -1;
                                    break;
                                case 12:
                                    itemcode101 = "BH16";
                                    days99 = 30;
                                    break;
                                case 13:
                                    itemcode101 = "BH16";
                                    days99 = -1;
                                    break;
                                case 14:
                                    itemcode101 = "BH17";
                                    days99 = 30;
                                    break;
                                case 15:
                                    itemcode101 = "BH17";
                                    days99 = -1;
                                    break;
                                case 16:
                                    itemcode101 = "BH18";
                                    days99 = 30;
                                    break;
                                case 17:
                                    itemcode101 = "BH18";
                                    days99 = -1;
                                    break;
                                case 18:
                                    itemcode101 = "BH19";
                                    days99 = 30;
                                    break;
                                case 19:
                                    itemcode101 = "BH19";
                                    days99 = -1;
                                    break;
                                case 20:
                                    itemcode101 = "BH1A";
                                    days99 = 30;
                                    break;
                                case 21:
                                    itemcode101 = "BH1A";
                                    days99 = -1;
                                    break;
                                case 22:
                                    itemcode101 = "BH1B";
                                    days99 = 30;
                                    break;
                                case 23:
                                    itemcode101 = "BH1B";
                                    days99 = -1;
                                    break;
                                case 24:
                                    itemcode101 = "BH1C";
                                    days99 = 30;
                                    break;
                                case 25:
                                    itemcode101 = "BH1C";
                                    days99 = -1;
                                    break;
                                case 26:
                                    itemcode101 = "BH28";
                                    days99 = 30;
                                    break;
                                case 27:
                                    itemcode101 = "BH28";
                                    days99 = -1;
                                    break;
                                case 28:
                                    itemcode101 = "BH29";
                                    days99 = 30;
                                    break;
                                case 29:
                                    itemcode101 = "BH29";
                                    days99 = -1;
                                    break;
                                case 30:
                                    itemcode101 = "BH2A";
                                    days99 = 30;
                                    break;
                                case 31:
                                    itemcode101 = "BH2A";
                                    days99 = -1;
                                    break;
                                case 32:
                                    itemcode101 = "BH2B";
                                    days99 = 30;
                                    break;
                                case 33:
                                    itemcode101 = "BH2B";
                                    days99 = -1;
                                    break;
                                case 34:
                                    itemcode101 = "BH2C";
                                    days99 = 30;
                                    break;
                                case 35:
                                    itemcode101 = "BH2C";
                                    days99 = -1;
                                    break;
                                case 36:
                                    itemcode101 = "BH2D";
                                    days99 = 30;
                                    break;
                                case 37:
                                    itemcode101 = "BH2D";
                                    days99 = -1;
                                    break;
                                case 38:
                                    itemcode101 = "BH2E";
                                    days99 = 30;
                                    break;
                                case 39:
                                    itemcode101 = "BH2E";
                                    days99 = -1;
                                    break;
                                case 40:
                                    itemcode101 = "BH2F";
                                    days99 = 30;
                                    break;
                                case 41:
                                    itemcode101 = "BH2F";
                                    days99 = -1;
                                    break;
                                case 42:
                                    itemcode101 = "BH30";
                                    days99 = 30;
                                    break;
                                case 43:
                                    itemcode101 = "BH30";
                                    days99 = -1;
                                    break;
                                case 44:
                                    itemcode101 = "BH31";
                                    days99 = 30;
                                    break;
                                case 45:
                                    itemcode101 = "BH31";
                                    days99 = -1;
                                    break;
                                case 46:
                                    itemcode101 = "BH32";
                                    days99 = 30;
                                    break;
                                case 47:
                                    itemcode101 = "BH32";
                                    days99 = -1;
                                    break;
                                case 48:
                                    itemcode101 = "BH33";
                                    days99 = 30;
                                    break;
                                case 49:
                                    itemcode101 = "BH33";
                                    days99 = -1;
                                    break;
                                case 50:
                                    itemcode101 = "BH34";
                                    days99 = 30;
                                    break;
                                case 51:
                                    itemcode101 = "BH34";
                                    days99 = -1;
                                    break;
                                case 52:
                                    itemcode101 = "BH1E";
                                    days99 = 30;
                                    break;
                                case 53:
                                    itemcode101 = "BH1E";
                                    days99 = -1;
                                    break;
                                case 54:
                                    itemcode101 = "BH20";
                                    days99 = 30;
                                    break;
                                case 55:
                                    itemcode101 = "BH20";
                                    days99 = -1;
                                    break;
                                case 56:
                                    itemcode101 = "BH21";
                                    days99 = 30;
                                    break;
                                case 57:
                                    itemcode101 = "BH21";
                                    days99 = -1;
                                    break;
                                case 58:
                                    itemcode101 = "BH22";
                                    days99 = 30;
                                    break;
                                case 59:
                                    itemcode101 = "BH22";
                                    days99 = -1;
                                    break;
                                case 60:
                                    itemcode101 = "BH23";
                                    days99 = 30;
                                    break;
                                case 61:
                                    itemcode101 = "BH23";
                                    days99 = -1;
                                    break;
                                case 62:
                                    itemcode101 = "BH24";
                                    days99 = 30;
                                    break;
                                case 63:
                                    itemcode101 = "BH24";
                                    days99 = -1;
                                    break;
                                case 64:
                                    itemcode101 = "BH25";
                                    days99 = 30;
                                    break;
                                case 65:
                                    itemcode101 = "BH25";
                                    days99 = -1;
                                    break;
                                case 66:
                                    itemcode101 = "BH26";
                                    days99 = 30;
                                    break;
                                case 67:
                                    itemcode101 = "BH26";
                                    days99 = -1;
                                    break;
                                case 68:
                                    itemcode101 = "BH27";
                                    days99 = 30;
                                    break;
                                case 69:
                                    itemcode101 = "BH27";
                                    days99 = -1;
                                    break;
                                case 70:
                                    itemcode101 = "BH35";
                                    days99 = 30;
                                    break;
                                case 71:
                                    itemcode101 = "BH35";
                                    days99 = -1;
                                    break;
                                case 72:
                                    itemcode101 = "BH36";
                                    days99 = 30;
                                    break;
                                case 73:
                                    itemcode101 = "BH36";
                                    days99 = -1;
                                    break;
                                case 74:
                                    itemcode101 = "BH37";
                                    days99 = 30;
                                    break;
                                case 75:
                                    itemcode101 = "BH37";
                                    days99 = -1;
                                    break;
                                case 76:
                                    itemcode101 = "BH38";
                                    days99 = 30;
                                    break;
                                case 77:
                                    itemcode101 = "BH38";
                                    days99 = -1;
                                    break;
                                case 78:
                                    itemcode101 = "BH39";
                                    days99 = 30;
                                    break;
                                case 79:
                                    itemcode101 = "BH39";
                                    days99 = -1;
                                    break;
                                case 80:
                                    itemcode101 = "BH3A";
                                    days99 = 30;
                                    break;
                                case 81:
                                    itemcode101 = "BH3A";
                                    days99 = -1;
                                    break;
                                case 82:
                                    itemcode101 = "BH3B";
                                    days99 = 30;
                                    break;
                                case 83:
                                    itemcode101 = "BH3B";
                                    days99 = -1;
                                    break;
                                case 84:
                                    itemcode101 = "BH3C";
                                    days99 = 30;
                                    break;
                                case 85:
                                    itemcode101 = "BH3C";
                                    days99 = -1;
                                    break;
                                case 86:
                                    itemcode101 = "BH50";
                                    days99 = 30;
                                    break;
                                case 87:
                                    itemcode101 = "BH50";
                                    days99 = -1;
                                    break;
                                case 88:
                                    itemcode101 = "BH51";
                                    days99 = 30;
                                    break;
                                case 89:
                                    itemcode101 = "BH51";
                                    days99 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode101, days99);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode101, days99));
                            return;
                        case "CY52":
                            int num109 = Game_Server.Generic.random(0, 58);
                            int days100 = 1;
                            string itemcode102 = (string)null;
                            switch (num109)
                            {
                                case 0:
                                    itemcode102 = "BV01";
                                    days100 = -1;
                                    break;
                                case 1:
                                    itemcode102 = "BV02";
                                    days100 = -1;
                                    break;
                                case 2:
                                    itemcode102 = "BV03";
                                    days100 = -1;
                                    break;
                                case 3:
                                    itemcode102 = "BV11";
                                    days100 = -1;
                                    break;
                                case 4:
                                    itemcode102 = "BV15";
                                    days100 = -1;
                                    break;
                                case 5:
                                    itemcode102 = "DV16";
                                    days100 = -1;
                                    break;
                                case 6:
                                    itemcode102 = "DV18";
                                    days100 = -1;
                                    break;
                                case 7:
                                    itemcode102 = "DV19";
                                    days100 = -1;
                                    break;
                                case 8:
                                    itemcode102 = "DV20";
                                    days100 = -1;
                                    break;
                                case 9:
                                    itemcode102 = "BV01";
                                    days100 = 30;
                                    break;
                                case 10:
                                    itemcode102 = "BV01";
                                    days100 = 15;
                                    break;
                                case 11:
                                    itemcode102 = "BV02";
                                    days100 = 15;
                                    break;
                                case 12:
                                    itemcode102 = "BV02";
                                    days100 = 30;
                                    break;
                                case 13:
                                    itemcode102 = "BV03";
                                    days100 = 15;
                                    break;
                                case 14:
                                    itemcode102 = "BV03";
                                    days100 = 30;
                                    break;
                                case 15:
                                    itemcode102 = "BV11";
                                    days100 = 30;
                                    break;
                                case 16:
                                    itemcode102 = "BV11";
                                    days100 = 15;
                                    break;
                                case 17:
                                    itemcode102 = "BV11";
                                    days100 = 30;
                                    break;
                                case 18:
                                    itemcode102 = "BV15";
                                    days100 = 15;
                                    break;
                                case 19:
                                    itemcode102 = "BV15";
                                    days100 = 30;
                                    break;
                                case 20:
                                    itemcode102 = "BV16";
                                    days100 = 15;
                                    break;
                                case 21:
                                    itemcode102 = "BV16";
                                    days100 = 30;
                                    break;
                                case 22:
                                    itemcode102 = "BV18";
                                    days100 = 30;
                                    break;
                                case 23:
                                    itemcode102 = "BV18";
                                    days100 = 15;
                                    break;
                                case 24:
                                    itemcode102 = "BV19";
                                    days100 = 30;
                                    break;
                                case 25:
                                    itemcode102 = "BV19";
                                    days100 = 15;
                                    break;
                                case 26:
                                    itemcode102 = "BV20";
                                    days100 = 15;
                                    break;
                                case 27:
                                    itemcode102 = "BV20";
                                    days100 = 30;
                                    break;
                                case 28:
                                    itemcode102 = "BV17";
                                    days100 = 30;
                                    break;
                                case 29:
                                    itemcode102 = "BV17";
                                    days100 = 15;
                                    break;
                                case 30:
                                    itemcode102 = "BV20";
                                    days100 = -1;
                                    break;
                                case 31:
                                    itemcode102 = "BV08";
                                    days100 = -1;
                                    break;
                                case 32:
                                    itemcode102 = "BV08";
                                    days100 = 30;
                                    break;
                                case 33:
                                    itemcode102 = "BV08";
                                    days100 = 15;
                                    break;
                                case 34:
                                    itemcode102 = "BV10";
                                    days100 = -1;
                                    break;
                                case 35:
                                    itemcode102 = "BV10";
                                    days100 = 30;
                                    break;
                                case 36:
                                    itemcode102 = "BV10";
                                    days100 = 15;
                                    break;
                                case 37:
                                    itemcode102 = "BV04";
                                    days100 = -1;
                                    break;
                                case 38:
                                    itemcode102 = "BV04";
                                    days100 = 30;
                                    break;
                                case 39:
                                    itemcode102 = "BV04";
                                    days100 = 15;
                                    break;
                                case 40:
                                    itemcode102 = "BV05";
                                    days100 = -1;
                                    break;
                                case 41:
                                    itemcode102 = "BV05";
                                    days100 = 30;
                                    break;
                                case 42:
                                    itemcode102 = "BV05";
                                    days100 = 15;
                                    break;
                                case 43:
                                    itemcode102 = "BV06";
                                    days100 = -1;
                                    break;
                                case 44:
                                    itemcode102 = "BV06";
                                    days100 = 30;
                                    break;
                                case 45:
                                    itemcode102 = "BV06";
                                    days100 = 15;
                                    break;
                                case 46:
                                    itemcode102 = "BV06";
                                    days100 = -1;
                                    break;
                                case 47:
                                    itemcode102 = "BV07";
                                    days100 = 30;
                                    break;
                                case 48:
                                    itemcode102 = "BV07";
                                    days100 = 15;
                                    break;
                                case 49:
                                    itemcode102 = "BV07";
                                    days100 = -1;
                                    break;
                                case 50:
                                    itemcode102 = "BV08";
                                    days100 = 30;
                                    break;
                                case 51:
                                    itemcode102 = "BV08";
                                    days100 = 15;
                                    break;
                                case 52:
                                    itemcode102 = "BV08";
                                    days100 = -1;
                                    break;
                                case 53:
                                    itemcode102 = "BV10";
                                    days100 = 30;
                                    break;
                                case 54:
                                    itemcode102 = "BV10";
                                    days100 = 15;
                                    break;
                                case 55:
                                    itemcode102 = "BV10";
                                    days100 = -1;
                                    break;
                                case 56:
                                    itemcode102 = "BV14";
                                    days100 = 30;
                                    break;
                                case 57:
                                    itemcode102 = "BV14";
                                    days100 = 15;
                                    break;
                                case 58:
                                    itemcode102 = "BV14";
                                    days100 = -1;
                                    break;
                            }
                            Inventory.AddCostume(usr, itemcode102, days100);
                            usr.send((Packet)new SP_UpdateInventory(usr, usr.expiredItems));
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode102, days100));
                            return;
                        case "CY53":
                            int num110 = Game_Server.Generic.random(0, 129);
                            int days101 = 1;
                            string itemcode103 = (string)null;
                            switch (num110)
                            {
                                case 0:
                                    itemcode103 = "D501";
                                    days101 = 30;
                                    break;
                                case 1:
                                    itemcode103 = "D501";
                                    days101 = -1;
                                    break;
                                case 2:
                                    itemcode103 = "D502";
                                    days101 = 30;
                                    break;
                                case 3:
                                    itemcode103 = "D502";
                                    days101 = -1;
                                    break;
                                case 4:
                                    itemcode103 = "D601";
                                    days101 = 30;
                                    break;
                                case 5:
                                    itemcode103 = "D601";
                                    days101 = -1;
                                    break;
                                case 6:
                                    itemcode103 = "D602";
                                    days101 = 30;
                                    break;
                                case 7:
                                    itemcode103 = "D602";
                                    days101 = -1;
                                    break;
                                case 8:
                                    itemcode103 = "D701";
                                    days101 = 30;
                                    break;
                                case 9:
                                    itemcode103 = "D701";
                                    days101 = -1;
                                    break;
                                case 10:
                                    itemcode103 = "D702";
                                    days101 = 30;
                                    break;
                                case 11:
                                    itemcode103 = "D702";
                                    days101 = -1;
                                    break;
                                case 12:
                                    itemcode103 = "D501";
                                    days101 = 30;
                                    break;
                                case 13:
                                    itemcode103 = "D801";
                                    days101 = -1;
                                    break;
                                case 14:
                                    itemcode103 = "D801";
                                    days101 = 30;
                                    break;
                                case 15:
                                    itemcode103 = "D802";
                                    days101 = -1;
                                    break;
                                case 16:
                                    itemcode103 = "D802";
                                    days101 = 30;
                                    break;
                                case 17:
                                    itemcode103 = "D901";
                                    days101 = -1;
                                    break;
                                case 18:
                                    itemcode103 = "D901";
                                    days101 = 30;
                                    break;
                                case 19:
                                    itemcode103 = "D902";
                                    days101 = -1;
                                    break;
                                case 20:
                                    itemcode103 = "D902";
                                    days101 = 30;
                                    break;
                                case 21:
                                    itemcode103 = "D805";
                                    days101 = -1;
                                    break;
                                case 22:
                                    itemcode103 = "D805";
                                    days101 = 30;
                                    break;
                                case 23:
                                    itemcode103 = "D806";
                                    days101 = -1;
                                    break;
                                case 24:
                                    itemcode103 = "D806";
                                    days101 = 30;
                                    break;
                                case 25:
                                    itemcode103 = "D807";
                                    days101 = -1;
                                    break;
                                case 26:
                                    itemcode103 = "D807";
                                    days101 = 30;
                                    break;
                                case 27:
                                    itemcode103 = "D808";
                                    days101 = -1;
                                    break;
                                case 28:
                                    itemcode103 = "D808";
                                    days101 = 30;
                                    break;
                                case 29:
                                    itemcode103 = "D809";
                                    days101 = -1;
                                    break;
                                case 30:
                                    itemcode103 = "D809";
                                    days101 = 30;
                                    break;
                                case 31:
                                    itemcode103 = "D813";
                                    days101 = -1;
                                    break;
                                case 32:
                                    itemcode103 = "D813";
                                    days101 = 30;
                                    break;
                                case 33:
                                    itemcode103 = "D814";
                                    days101 = 30;
                                    break;
                                case 34:
                                    itemcode103 = "D814";
                                    days101 = -1;
                                    break;
                                case 35:
                                    itemcode103 = "D815";
                                    days101 = 30;
                                    break;
                                case 36:
                                    itemcode103 = "D815";
                                    days101 = -1;
                                    break;
                                case 37:
                                    itemcode103 = "D603";
                                    days101 = 30;
                                    break;
                                case 38:
                                    itemcode103 = "D603";
                                    days101 = -1;
                                    break;
                                case 39:
                                    itemcode103 = "D604";
                                    days101 = 30;
                                    break;
                                case 40:
                                    itemcode103 = "D604";
                                    days101 = -1;
                                    break;
                                case 41:
                                    itemcode103 = "D504";
                                    days101 = 30;
                                    break;
                                case 42:
                                    itemcode103 = "D504";
                                    days101 = -1;
                                    break;
                                case 43:
                                    itemcode103 = "D704";
                                    days101 = 30;
                                    break;
                                case 44:
                                    itemcode103 = "D704";
                                    days101 = -1;
                                    break;
                                case 45:
                                    itemcode103 = "D705";
                                    days101 = 30;
                                    break;
                                case 46:
                                    itemcode103 = "D705";
                                    days101 = -1;
                                    break;
                                case 47:
                                    itemcode103 = "D505";
                                    days101 = 30;
                                    break;
                                case 48:
                                    itemcode103 = "D505";
                                    days101 = -1;
                                    break;
                                case 49:
                                    itemcode103 = "D705";
                                    days101 = 30;
                                    break;
                                case 50:
                                    itemcode103 = "D705";
                                    days101 = -1;
                                    break;
                                case 51:
                                    itemcode103 = "D810";
                                    days101 = 30;
                                    break;
                                case 52:
                                    itemcode103 = "D810";
                                    days101 = -1;
                                    break;
                                case 53:
                                    itemcode103 = "D811";
                                    days101 = 30;
                                    break;
                                case 54:
                                    itemcode103 = "D811";
                                    days101 = -1;
                                    break;
                                case 55:
                                    itemcode103 = "D812";
                                    days101 = 30;
                                    break;
                                case 56:
                                    itemcode103 = "D812";
                                    days101 = -1;
                                    break;
                                case 57:
                                    itemcode103 = "D816";
                                    days101 = 30;
                                    break;
                                case 58:
                                    itemcode103 = "D816";
                                    days101 = -1;
                                    break;
                                case 59:
                                    itemcode103 = "D817";
                                    days101 = 30;
                                    break;
                                case 60:
                                    itemcode103 = "D817";
                                    days101 = -1;
                                    break;
                                case 61:
                                    itemcode103 = "D818";
                                    days101 = 30;
                                    break;
                                case 62:
                                    itemcode103 = "D818";
                                    days101 = -1;
                                    break;
                                case 63:
                                    itemcode103 = "D819";
                                    days101 = 30;
                                    break;
                                case 64:
                                    itemcode103 = "D819";
                                    days101 = -1;
                                    break;
                                case 65:
                                    itemcode103 = "D820";
                                    days101 = 30;
                                    break;
                                case 66:
                                    itemcode103 = "D820";
                                    days101 = -1;
                                    break;
                                case 67:
                                    itemcode103 = "D821";
                                    days101 = 30;
                                    break;
                                case 68:
                                    itemcode103 = "D821";
                                    days101 = -1;
                                    break;
                                case 69:
                                    itemcode103 = "D506";
                                    days101 = 30;
                                    break;
                                case 70:
                                    itemcode103 = "D506";
                                    days101 = -1;
                                    break;
                                case 71:
                                    itemcode103 = "D507";
                                    days101 = 30;
                                    break;
                                case 72:
                                    itemcode103 = "D507";
                                    days101 = -1;
                                    break;
                                case 73:
                                    itemcode103 = "D905";
                                    days101 = 30;
                                    break;
                                case 74:
                                    itemcode103 = "D905";
                                    days101 = -1;
                                    break;
                                case 75:
                                    itemcode103 = "D906";
                                    days101 = 30;
                                    break;
                                case 76:
                                    itemcode103 = "D906";
                                    days101 = -1;
                                    break;
                                case 77:
                                    itemcode103 = "D605";
                                    days101 = 30;
                                    break;
                                case 78:
                                    itemcode103 = "D605";
                                    days101 = -1;
                                    break;
                                case 79:
                                    itemcode103 = "D605";
                                    days101 = 30;
                                    break;
                                case 80:
                                    itemcode103 = "D606";
                                    days101 = -1;
                                    break;
                                case 81:
                                    itemcode103 = "D606";
                                    days101 = 30;
                                    break;
                                case 82:
                                    itemcode103 = "D706";
                                    days101 = -1;
                                    break;
                                case 83:
                                    itemcode103 = "D706";
                                    days101 = 30;
                                    break;
                                case 84:
                                    itemcode103 = "D707";
                                    days101 = -1;
                                    break;
                                case 85:
                                    itemcode103 = "D707";
                                    days101 = 30;
                                    break;
                                case 86:
                                    itemcode103 = "D822";
                                    days101 = -1;
                                    break;
                                case 87:
                                    itemcode103 = "D822";
                                    days101 = 30;
                                    break;
                                case 88:
                                    itemcode103 = "D823";
                                    days101 = -1;
                                    break;
                                case 89:
                                    itemcode103 = "D823";
                                    days101 = 30;
                                    break;
                                case 90:
                                    itemcode103 = "D907";
                                    days101 = -1;
                                    break;
                                case 91:
                                    itemcode103 = "D907";
                                    days101 = 30;
                                    break;
                                case 92:
                                    itemcode103 = "DC92";
                                    days101 = -1;
                                    break;
                                case 93:
                                    itemcode103 = "DC92";
                                    days101 = 30;
                                    break;
                                case 94:
                                    itemcode103 = "D824";
                                    days101 = -1;
                                    break;
                                case 95:
                                    itemcode103 = "D824";
                                    days101 = 30;
                                    break;
                                case 96:
                                    itemcode103 = "D825";
                                    days101 = -1;
                                    break;
                                case 97:
                                    itemcode103 = "D825";
                                    days101 = 30;
                                    break;
                                case 98:
                                    itemcode103 = "D826";
                                    days101 = -1;
                                    break;
                                case 99:
                                    itemcode103 = "D826";
                                    days101 = 30;
                                    break;
                                case 100:
                                    itemcode103 = "D908";
                                    days101 = -1;
                                    break;
                                case 101:
                                    itemcode103 = "D908";
                                    days101 = 30;
                                    break;
                                case 102:
                                    itemcode103 = "D607";
                                    days101 = -1;
                                    break;
                                case 103:
                                    itemcode103 = "D607";
                                    days101 = 30;
                                    break;
                                case 104:
                                    itemcode103 = "D828";
                                    days101 = -1;
                                    break;
                                case 105:
                                    itemcode103 = "D828";
                                    days101 = 30;
                                    break;
                                case 106:
                                    itemcode103 = "D909";
                                    days101 = -1;
                                    break;
                                case 107:
                                    itemcode103 = "D909";
                                    days101 = 30;
                                    break;
                                case 108:
                                    itemcode103 = "D829";
                                    days101 = -1;
                                    break;
                                case 109:
                                    itemcode103 = "D830";
                                    days101 = 30;
                                    break;
                                case 110:
                                    itemcode103 = "D910";
                                    days101 = -1;
                                    break;
                                case 111:
                                    itemcode103 = "D910";
                                    days101 = 30;
                                    break;
                                case 112:
                                    itemcode103 = "D831";
                                    days101 = -1;
                                    break;
                                case 113:
                                    itemcode103 = "D831";
                                    days101 = 30;
                                    break;
                                case 114:
                                    itemcode103 = "D608";
                                    days101 = -1;
                                    break;
                                case 115:
                                    itemcode103 = "D608";
                                    days101 = 30;
                                    break;
                                case 116:
                                    itemcode103 = "D832";
                                    days101 = -1;
                                    break;
                                case 117:
                                    itemcode103 = "D832";
                                    days101 = 30;
                                    break;
                                case 118:
                                    itemcode103 = "D911";
                                    days101 = -1;
                                    break;
                                case 119:
                                    itemcode103 = "D911";
                                    days101 = 30;
                                    break;
                                case 120:
                                    itemcode103 = "D708";
                                    days101 = -1;
                                    break;
                                case 121:
                                    itemcode103 = "D708";
                                    days101 = 30;
                                    break;
                                case 122:
                                    itemcode103 = "D833";
                                    days101 = -1;
                                    break;
                                case 123:
                                    itemcode103 = "D833";
                                    days101 = 30;
                                    break;
                                case 124:
                                    itemcode103 = "D609";
                                    days101 = -1;
                                    break;
                                case 125:
                                    itemcode103 = "D609";
                                    days101 = 30;
                                    break;
                                case 126:
                                    itemcode103 = "D834";
                                    days101 = -1;
                                    break;
                                case (int)sbyte.MaxValue:
                                    itemcode103 = "D834";
                                    days101 = 30;
                                    break;
                                case 128:
                                    itemcode103 = "D913";
                                    days101 = -1;
                                    break;
                                case 129:
                                    itemcode103 = "D913";
                                    days101 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode103, days101);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode103, days101));
                            return;
                        case "CY54":
                            int num111 = Game_Server.Generic.random(0, 47);
                            int days102 = 1;
                            string itemcode104 = (string)null;
                            switch (num111)
                            {
                                case 0:
                                    itemcode104 = "DA10";
                                    days102 = 30;
                                    break;
                                case 1:
                                    itemcode104 = "DA10";
                                    days102 = 15;
                                    break;
                                case 2:
                                    itemcode104 = "DA10";
                                    days102 = -1;
                                    break;
                                case 3:
                                    itemcode104 = "DA04";
                                    days102 = 30;
                                    break;
                                case 4:
                                    itemcode104 = "DA04";
                                    days102 = -1;
                                    break;
                                case 5:
                                    itemcode104 = "DA04";
                                    days102 = 15;
                                    break;
                                case 6:
                                    itemcode104 = "DA40";
                                    days102 = 30;
                                    break;
                                case 7:
                                    itemcode104 = "DA40";
                                    days102 = -1;
                                    break;
                                case 8:
                                    itemcode104 = "DA40";
                                    days102 = 15;
                                    break;
                                case 9:
                                    itemcode104 = "DA41";
                                    days102 = 30;
                                    break;
                                case 10:
                                    itemcode104 = "DA41";
                                    days102 = -1;
                                    break;
                                case 11:
                                    itemcode104 = "DA41";
                                    days102 = 15;
                                    break;
                                case 12:
                                    itemcode104 = "DA45";
                                    days102 = 30;
                                    break;
                                case 13:
                                    itemcode104 = "DA45";
                                    days102 = -1;
                                    break;
                                case 14:
                                    itemcode104 = "DA45";
                                    days102 = 15;
                                    break;
                                case 15:
                                    itemcode104 = "DA71";
                                    days102 = 30;
                                    break;
                                case 16:
                                    itemcode104 = "DA71";
                                    days102 = -1;
                                    break;
                                case 17:
                                    itemcode104 = "DA71";
                                    days102 = 15;
                                    break;
                                case 18:
                                    itemcode104 = "DA72";
                                    days102 = 30;
                                    break;
                                case 19:
                                    itemcode104 = "DA72";
                                    days102 = -1;
                                    break;
                                case 20:
                                    itemcode104 = "DA72";
                                    days102 = 15;
                                    break;
                                case 21:
                                    itemcode104 = "DA46";
                                    days102 = 30;
                                    break;
                                case 22:
                                    itemcode104 = "DA46";
                                    days102 = -1;
                                    break;
                                case 23:
                                    itemcode104 = "DA46";
                                    days102 = 15;
                                    break;
                                case 24:
                                    itemcode104 = "DA75";
                                    days102 = 30;
                                    break;
                                case 25:
                                    itemcode104 = "DA75";
                                    days102 = -1;
                                    break;
                                case 26:
                                    itemcode104 = "DA75";
                                    days102 = 15;
                                    break;
                                case 27:
                                    itemcode104 = "DA48";
                                    days102 = 30;
                                    break;
                                case 28:
                                    itemcode104 = "DA48";
                                    days102 = -1;
                                    break;
                                case 29:
                                    itemcode104 = "DA48";
                                    days102 = 30;
                                    break;
                                case 30:
                                    itemcode104 = "DA84";
                                    days102 = 15;
                                    break;
                                case 31:
                                    itemcode104 = "DA84";
                                    days102 = -1;
                                    break;
                                case 32:
                                    itemcode104 = "DA84";
                                    days102 = 30;
                                    break;
                                case 33:
                                    itemcode104 = "DA87";
                                    days102 = 15;
                                    break;
                                case 34:
                                    itemcode104 = "DA87";
                                    days102 = -1;
                                    break;
                                case 35:
                                    itemcode104 = "DA87";
                                    days102 = 30;
                                    break;
                                case 36:
                                    itemcode104 = "DA90";
                                    days102 = 15;
                                    break;
                                case 37:
                                    itemcode104 = "DA90";
                                    days102 = -1;
                                    break;
                                case 38:
                                    itemcode104 = "DA90";
                                    days102 = 15;
                                    break;
                                case 39:
                                    itemcode104 = "DA91";
                                    days102 = -1;
                                    break;
                                case 40:
                                    itemcode104 = "DA91";
                                    days102 = 15;
                                    break;
                                case 41:
                                    itemcode104 = "DA91";
                                    days102 = 30;
                                    break;
                                case 42:
                                    itemcode104 = "DA92";
                                    days102 = -1;
                                    break;
                                case 43:
                                    itemcode104 = "DA92";
                                    days102 = 15;
                                    break;
                                case 44:
                                    itemcode104 = "DA92";
                                    days102 = 30;
                                    break;
                                case 45:
                                    itemcode104 = "DA98";
                                    days102 = -1;
                                    break;
                                case 46:
                                    itemcode104 = "DA98";
                                    days102 = 15;
                                    break;
                                case 47:
                                    itemcode104 = "DA98";
                                    days102 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode104, days102);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode104, days102));
                            return;
                        case "CY55":
                            int num112 = Game_Server.Generic.random(0, 15);
                            int days103 = 1;
                            string itemcode105 = (string)null;
                            switch (num112)
                            {
                                case 0:
                                    itemcode105 = "DF42";
                                    days103 = 30;
                                    break;
                                case 1:
                                    itemcode105 = "DF42";
                                    days103 = -1;
                                    break;
                                case 2:
                                    itemcode105 = "CI01";
                                    days103 = 30;
                                    break;
                                case 3:
                                    itemcode105 = "DC31";
                                    days103 = 30;
                                    break;
                                case 4:
                                    itemcode105 = "DD01";
                                    days103 = 30;
                                    break;
                                case 5:
                                    itemcode105 = "CF02";
                                    days103 = 30;
                                    break;
                                case 6:
                                    itemcode105 = "CF01";
                                    days103 = 30;
                                    break;
                                case 7:
                                    itemcode105 = "DD01";
                                    days103 = 30;
                                    break;
                                case 8:
                                    itemcode105 = "DJ14";
                                    days103 = 30;
                                    break;
                                case 9:
                                    itemcode105 = "DF06";
                                    days103 = 30;
                                    break;
                                case 10:
                                    itemcode105 = "DG03";
                                    days103 = 30;
                                    break;
                                case 11:
                                    itemcode105 = "DG31";
                                    days103 = 30;
                                    break;
                                case 12:
                                    itemcode105 = "DS03";
                                    days103 = 30;
                                    break;
                                case 13:
                                    itemcode105 = "DU01";
                                    days103 = 30;
                                    break;
                                case 14:
                                    itemcode105 = "DS03";
                                    days103 = 30;
                                    break;
                                case 15:
                                    itemcode105 = "D601";
                                    days103 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode105, days103);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode105, days103));
                            return;
                        case "CY56":
                            int num113 = Game_Server.Generic.random(0, 29);
                            int days104 = 1;
                            string itemcode106 = (string)null;
                            switch (num113)
                            {
                                case 0:
                                    itemcode106 = "GF21";
                                    days104 = 15;
                                    break;
                                case 1:
                                    itemcode106 = "GF21";
                                    days104 = 30;
                                    break;
                                case 2:
                                    itemcode106 = "GF21";
                                    days104 = -1;
                                    break;
                                case 3:
                                    itemcode106 = "DF71";
                                    days104 = 15;
                                    break;
                                case 4:
                                    itemcode106 = "DF71";
                                    days104 = 30;
                                    break;
                                case 5:
                                    itemcode106 = "DF71";
                                    days104 = -1;
                                    break;
                                case 6:
                                    itemcode106 = "DD27";
                                    days104 = 15;
                                    break;
                                case 7:
                                    itemcode106 = "DD27";
                                    days104 = 30;
                                    break;
                                case 8:
                                    itemcode106 = "DD27";
                                    days104 = -1;
                                    break;
                                case 9:
                                    itemcode106 = "DE85";
                                    days104 = 15;
                                    break;
                                case 10:
                                    itemcode106 = "DE85";
                                    days104 = 30;
                                    break;
                                case 11:
                                    itemcode106 = "DE85";
                                    days104 = -1;
                                    break;
                                case 12:
                                    itemcode106 = "GG14";
                                    days104 = 15;
                                    break;
                                case 13:
                                    itemcode106 = "GG14";
                                    days104 = 30;
                                    break;
                                case 14:
                                    itemcode106 = "GG14 ";
                                    days104 = -1;
                                    break;
                                case 15:
                                    itemcode106 = "D816";
                                    days104 = 15;
                                    break;
                                case 16:
                                    itemcode106 = "D816";
                                    days104 = 30;
                                    break;
                                case 17:
                                    itemcode106 = "D816";
                                    days104 = -1;
                                    break;
                                case 18:
                                    itemcode106 = "DH15";
                                    days104 = 15;
                                    break;
                                case 19:
                                    itemcode106 = "DH15";
                                    days104 = 30;
                                    break;
                                case 20:
                                    itemcode106 = "DH15";
                                    days104 = -1;
                                    break;
                                case 21:
                                    itemcode106 = "DC85";
                                    days104 = 15;
                                    break;
                                case 22:
                                    itemcode106 = "DC85";
                                    days104 = 30;
                                    break;
                                case 23:
                                    itemcode106 = "DC85";
                                    days104 = -1;
                                    break;
                                case 24:
                                    itemcode106 = "DT17";
                                    days104 = 15;
                                    break;
                                case 25:
                                    itemcode106 = "DT17";
                                    days104 = 30;
                                    break;
                                case 26:
                                    itemcode106 = "DT17";
                                    days104 = -1;
                                    break;
                                case 27:
                                    itemcode106 = "DA08";
                                    days104 = 15;
                                    break;
                                case 28:
                                    itemcode106 = "DA08";
                                    days104 = 30;
                                    break;
                                case 29:
                                    itemcode106 = "DA08";
                                    days104 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode106, days104);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode106, days104));
                            return;
                        case "CY57":
                            int num114 = Game_Server.Generic.random(0, 29);
                            int days105 = 1;
                            string itemcode107 = (string)null;
                            switch (num114)
                            {
                                case 0:
                                    itemcode107 = "DF93";
                                    days105 = 15;
                                    break;
                                case 1:
                                    itemcode107 = "DF93";
                                    days105 = 30;
                                    break;
                                case 2:
                                    itemcode107 = "DF93";
                                    days105 = -1;
                                    break;
                                case 3:
                                    itemcode107 = "GF17";
                                    days105 = 15;
                                    break;
                                case 4:
                                    itemcode107 = "GF17";
                                    days105 = 30;
                                    break;
                                case 5:
                                    itemcode107 = "GF17";
                                    days105 = -1;
                                    break;
                                case 6:
                                    itemcode107 = "DF53";
                                    days105 = 15;
                                    break;
                                case 7:
                                    itemcode107 = "DF53";
                                    days105 = 30;
                                    break;
                                case 8:
                                    itemcode107 = "DF53";
                                    days105 = -1;
                                    break;
                                case 9:
                                    itemcode107 = "DE80";
                                    days105 = 15;
                                    break;
                                case 10:
                                    itemcode107 = "DE80";
                                    days105 = 30;
                                    break;
                                case 11:
                                    itemcode107 = "DE80";
                                    days105 = -1;
                                    break;
                                case 12:
                                    itemcode107 = "DE46";
                                    days105 = 15;
                                    break;
                                case 13:
                                    itemcode107 = "DE46";
                                    days105 = 30;
                                    break;
                                case 14:
                                    itemcode107 = "DE46";
                                    days105 = -1;
                                    break;
                                case 15:
                                    itemcode107 = "DB27";
                                    days105 = 15;
                                    break;
                                case 16:
                                    itemcode107 = "DB27";
                                    days105 = 30;
                                    break;
                                case 17:
                                    itemcode107 = "DB27";
                                    days105 = -1;
                                    break;
                                case 18:
                                    itemcode107 = "DE62";
                                    days105 = 15;
                                    break;
                                case 19:
                                    itemcode107 = "DE62";
                                    days105 = 30;
                                    break;
                                case 20:
                                    itemcode107 = "DE62";
                                    days105 = -1;
                                    break;
                                case 21:
                                    itemcode107 = "DJ10";
                                    days105 = 15;
                                    break;
                                case 22:
                                    itemcode107 = "DJ10";
                                    days105 = 30;
                                    break;
                                case 23:
                                    itemcode107 = "DJ10";
                                    days105 = -1;
                                    break;
                                case 24:
                                    itemcode107 = "DG77";
                                    days105 = 15;
                                    break;
                                case 25:
                                    itemcode107 = "DG77";
                                    days105 = 30;
                                    break;
                                case 26:
                                    itemcode107 = "DG77";
                                    days105 = -1;
                                    break;
                                case 27:
                                    itemcode107 = "DI12";
                                    days105 = 15;
                                    break;
                                case 28:
                                    itemcode107 = "DI12";
                                    days105 = 30;
                                    break;
                                case 29:
                                    itemcode107 = "DI12";
                                    days105 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode107, days105);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode107, days105));
                            return;
                        case "CY58":
                            int num115 = Game_Server.Generic.random(0, 29);
                            int days106 = 1;
                            string itemcode108 = (string)null;
                            switch (num115)
                            {
                                case 0:
                                    itemcode108 = "DG46";
                                    days106 = 15;
                                    break;
                                case 1:
                                    itemcode108 = "DG46";
                                    days106 = 30;
                                    break;
                                case 2:
                                    itemcode108 = "DG46";
                                    days106 = -1;
                                    break;
                                case 3:
                                    itemcode108 = "DF36";
                                    days106 = 15;
                                    break;
                                case 4:
                                    itemcode108 = "DF36";
                                    days106 = 30;
                                    break;
                                case 5:
                                    itemcode108 = "DF36";
                                    days106 = -1;
                                    break;
                                case 6:
                                    itemcode108 = "DF51";
                                    days106 = 15;
                                    break;
                                case 7:
                                    itemcode108 = "DF51";
                                    days106 = 30;
                                    break;
                                case 8:
                                    itemcode108 = "DF51";
                                    days106 = -1;
                                    break;
                                case 9:
                                    itemcode108 = "DJ59";
                                    days106 = 15;
                                    break;
                                case 10:
                                    itemcode108 = "DJ59";
                                    days106 = 30;
                                    break;
                                case 11:
                                    itemcode108 = "DJ59";
                                    days106 = -1;
                                    break;
                                case 12:
                                    itemcode108 = "DH10";
                                    days106 = 15;
                                    break;
                                case 13:
                                    itemcode108 = "DH10";
                                    days106 = 30;
                                    break;
                                case 14:
                                    itemcode108 = "DH10";
                                    days106 = -1;
                                    break;
                                case 15:
                                    itemcode108 = "DE52";
                                    days106 = 15;
                                    break;
                                case 16:
                                    itemcode108 = "DE52";
                                    days106 = 30;
                                    break;
                                case 17:
                                    itemcode108 = "DE52";
                                    days106 = -1;
                                    break;
                                case 18:
                                    itemcode108 = "D832";
                                    days106 = 15;
                                    break;
                                case 19:
                                    itemcode108 = "D832";
                                    days106 = 30;
                                    break;
                                case 20:
                                    itemcode108 = "D832";
                                    days106 = -1;
                                    break;
                                case 21:
                                    itemcode108 = "DI03";
                                    days106 = 15;
                                    break;
                                case 22:
                                    itemcode108 = "DI03";
                                    days106 = 30;
                                    break;
                                case 23:
                                    itemcode108 = "DI03";
                                    days106 = -1;
                                    break;
                                case 24:
                                    itemcode108 = "DC86";
                                    days106 = 15;
                                    break;
                                case 25:
                                    itemcode108 = "DC86";
                                    days106 = 30;
                                    break;
                                case 26:
                                    itemcode108 = "DC86";
                                    days106 = -1;
                                    break;
                                case 27:
                                    itemcode108 = "DF86";
                                    days106 = 15;
                                    break;
                                case 28:
                                    itemcode108 = "DF86";
                                    days106 = 30;
                                    break;
                                case 29:
                                    itemcode108 = "DF86";
                                    days106 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode108, days106);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode108, days106));
                            return;
                        case "CY59":
                            int num116 = Game_Server.Generic.random(0, 29);
                            int days107 = 1;
                            string itemcode109 = (string)null;
                            switch (num116)
                            {
                                case 0:
                                    itemcode109 = "DG22";
                                    days107 = 15;
                                    break;
                                case 1:
                                    itemcode109 = "DG22";
                                    days107 = 30;
                                    break;
                                case 2:
                                    itemcode109 = "DG22";
                                    days107 = -1;
                                    break;
                                case 3:
                                    itemcode109 = "D602";
                                    days107 = 15;
                                    break;
                                case 4:
                                    itemcode109 = "D602";
                                    days107 = 30;
                                    break;
                                case 5:
                                    itemcode109 = "D602";
                                    days107 = -1;
                                    break;
                                case 6:
                                    itemcode109 = "DJ67";
                                    days107 = 15;
                                    break;
                                case 7:
                                    itemcode109 = "DJ67";
                                    days107 = 30;
                                    break;
                                case 8:
                                    itemcode109 = "DJ67";
                                    days107 = -1;
                                    break;
                                case 9:
                                    itemcode109 = "DK03";
                                    days107 = 15;
                                    break;
                                case 10:
                                    itemcode109 = "DK03";
                                    days107 = 30;
                                    break;
                                case 11:
                                    itemcode109 = "DK03";
                                    days107 = -1;
                                    break;
                                case 12:
                                    itemcode109 = "DG31";
                                    days107 = 15;
                                    break;
                                case 13:
                                    itemcode109 = "DG31";
                                    days107 = 30;
                                    break;
                                case 14:
                                    itemcode109 = "DG31";
                                    days107 = -1;
                                    break;
                                case 15:
                                    itemcode109 = "DF87";
                                    days107 = 15;
                                    break;
                                case 16:
                                    itemcode109 = "DF87";
                                    days107 = 30;
                                    break;
                                case 17:
                                    itemcode109 = "DF87";
                                    days107 = -1;
                                    break;
                                case 18:
                                    itemcode109 = "DC93";
                                    days107 = 15;
                                    break;
                                case 19:
                                    itemcode109 = "DC93";
                                    days107 = 30;
                                    break;
                                case 20:
                                    itemcode109 = "DC93";
                                    days107 = -1;
                                    break;
                                case 21:
                                    itemcode109 = "DF65";
                                    days107 = 15;
                                    break;
                                case 22:
                                    itemcode109 = "DF65";
                                    days107 = 30;
                                    break;
                                case 23:
                                    itemcode109 = "DF65";
                                    days107 = -1;
                                    break;
                                case 24:
                                    itemcode109 = "DB17";
                                    days107 = 15;
                                    break;
                                case 25:
                                    itemcode109 = "DB17";
                                    days107 = 30;
                                    break;
                                case 26:
                                    itemcode109 = "DB17";
                                    days107 = -1;
                                    break;
                                case 27:
                                    itemcode109 = "D902";
                                    days107 = 15;
                                    break;
                                case 28:
                                    itemcode109 = "D902";
                                    days107 = 30;
                                    break;
                                case 29:
                                    itemcode109 = "D902";
                                    days107 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode109, days107);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode109, days107));
                            return;
                        case "CY60":
                            int num117 = Game_Server.Generic.random(0, 29);
                            int days108 = 1;
                            string itemcode110 = (string)null;
                            switch (num117)
                            {
                                case 0:
                                    itemcode110 = "DE68";
                                    days108 = 15;
                                    break;
                                case 1:
                                    itemcode110 = "DE68";
                                    days108 = 30;
                                    break;
                                case 2:
                                    itemcode110 = "DE68";
                                    days108 = -1;
                                    break;
                                case 3:
                                    itemcode110 = "GF03";
                                    days108 = 15;
                                    break;
                                case 4:
                                    itemcode110 = "GF03";
                                    days108 = 30;
                                    break;
                                case 5:
                                    itemcode110 = "GF03";
                                    days108 = -1;
                                    break;
                                case 6:
                                    itemcode110 = "DG49";
                                    days108 = 15;
                                    break;
                                case 7:
                                    itemcode110 = "DG49";
                                    days108 = 30;
                                    break;
                                case 8:
                                    itemcode110 = "DG49";
                                    days108 = -1;
                                    break;
                                case 9:
                                    itemcode110 = "DC94";
                                    days108 = 15;
                                    break;
                                case 10:
                                    itemcode110 = "DC94";
                                    days108 = 30;
                                    break;
                                case 11:
                                    itemcode110 = "DC94";
                                    days108 = -1;
                                    break;
                                case 12:
                                    itemcode110 = "DC72";
                                    days108 = 15;
                                    break;
                                case 13:
                                    itemcode110 = "DC72";
                                    days108 = 30;
                                    break;
                                case 14:
                                    itemcode110 = "DC72";
                                    days108 = -1;
                                    break;
                                case 15:
                                    itemcode110 = "DF25";
                                    days108 = 15;
                                    break;
                                case 16:
                                    itemcode110 = "DF25";
                                    days108 = 30;
                                    break;
                                case 17:
                                    itemcode110 = "DF25";
                                    days108 = -1;
                                    break;
                                case 18:
                                    itemcode110 = "DC73";
                                    days108 = 15;
                                    break;
                                case 19:
                                    itemcode110 = "DC73";
                                    days108 = 30;
                                    break;
                                case 20:
                                    itemcode110 = "DC73";
                                    days108 = -1;
                                    break;
                                case 21:
                                    itemcode110 = "DG24";
                                    days108 = 15;
                                    break;
                                case 22:
                                    itemcode110 = "DG24";
                                    days108 = 30;
                                    break;
                                case 23:
                                    itemcode110 = "DG24";
                                    days108 = -1;
                                    break;
                                case 24:
                                    itemcode110 = "DI06";
                                    days108 = 15;
                                    break;
                                case 25:
                                    itemcode110 = "DI06";
                                    days108 = 30;
                                    break;
                                case 26:
                                    itemcode110 = "DI06";
                                    days108 = -1;
                                    break;
                                case 27:
                                    itemcode110 = "DH04";
                                    days108 = 15;
                                    break;
                                case 28:
                                    itemcode110 = "DH04";
                                    days108 = 30;
                                    break;
                                case 29:
                                    itemcode110 = "DH04";
                                    days108 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode110, days108);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode110, days108));
                            return;
                        case "CY61":
                            int num118 = Game_Server.Generic.random(0, 29);
                            int days109 = 1;
                            string itemcode111 = (string)null;
                            switch (num118)
                            {
                                case 0:
                                    itemcode111 = "D801";
                                    days109 = 15;
                                    break;
                                case 1:
                                    itemcode111 = "D801";
                                    days109 = 30;
                                    break;
                                case 2:
                                    itemcode111 = "D801";
                                    days109 = -1;
                                    break;
                                case 3:
                                    itemcode111 = "DE93";
                                    days109 = 15;
                                    break;
                                case 4:
                                    itemcode111 = "DE93";
                                    days109 = 30;
                                    break;
                                case 5:
                                    itemcode111 = "DE93";
                                    days109 = -1;
                                    break;
                                case 6:
                                    itemcode111 = "DF68";
                                    days109 = 15;
                                    break;
                                case 7:
                                    itemcode111 = "DF68";
                                    days109 = 30;
                                    break;
                                case 8:
                                    itemcode111 = "DF68";
                                    days109 = -1;
                                    break;
                                case 9:
                                    itemcode111 = "DF41";
                                    days109 = 15;
                                    break;
                                case 10:
                                    itemcode111 = "DF41";
                                    days109 = 30;
                                    break;
                                case 11:
                                    itemcode111 = "DF41";
                                    days109 = -1;
                                    break;
                                case 12:
                                    itemcode111 = "DD28";
                                    days109 = 15;
                                    break;
                                case 13:
                                    itemcode111 = "DD28";
                                    days109 = 30;
                                    break;
                                case 14:
                                    itemcode111 = "DD28";
                                    days109 = -1;
                                    break;
                                case 15:
                                    itemcode111 = "DG32";
                                    days109 = 15;
                                    break;
                                case 16:
                                    itemcode111 = "DG32";
                                    days109 = 30;
                                    break;
                                case 17:
                                    itemcode111 = "DG32";
                                    days109 = -1;
                                    break;
                                case 18:
                                    itemcode111 = "GG09";
                                    days109 = 15;
                                    break;
                                case 19:
                                    itemcode111 = "GG09";
                                    days109 = 30;
                                    break;
                                case 20:
                                    itemcode111 = "GG09";
                                    days109 = -1;
                                    break;
                                case 21:
                                    itemcode111 = "D901";
                                    days109 = 15;
                                    break;
                                case 22:
                                    itemcode111 = "D901";
                                    days109 = 30;
                                    break;
                                case 23:
                                    itemcode111 = "D901";
                                    days109 = -1;
                                    break;
                                case 24:
                                    itemcode111 = "DJ39";
                                    days109 = 15;
                                    break;
                                case 25:
                                    itemcode111 = "DJ39";
                                    days109 = 30;
                                    break;
                                case 26:
                                    itemcode111 = "DJ39";
                                    days109 = -1;
                                    break;
                                case 27:
                                    itemcode111 = "DB53";
                                    days109 = 15;
                                    break;
                                case 28:
                                    itemcode111 = "DB53";
                                    days109 = 30;
                                    break;
                                case 29:
                                    itemcode111 = "DB53";
                                    days109 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode111, days109);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode111, days109));
                            return;
                        case "CY62":
                            int num119 = Game_Server.Generic.random(0, 11);
                            int days110 = 1;
                            string itemcode112 = (string)null;
                            switch (num119)
                            {
                                case 0:
                                    itemcode112 = "DC87";
                                    days110 = -1;
                                    break;
                                case 1:
                                    itemcode112 = "DC88";
                                    days110 = -1;
                                    break;
                                case 2:
                                    itemcode112 = "DC89";
                                    days110 = -1;
                                    break;
                                case 3:
                                    itemcode112 = "DC90";
                                    days110 = -1;
                                    break;
                                case 4:
                                    itemcode112 = "DC96";
                                    days110 = -1;
                                    break;
                                case 5:
                                    itemcode112 = "DE17";
                                    days110 = -1;
                                    break;
                                case 6:
                                    itemcode112 = "DE18";
                                    days110 = -1;
                                    break;
                                case 7:
                                    itemcode112 = "DE19";
                                    days110 = -1;
                                    break;
                                case 8:
                                    itemcode112 = "DE20";
                                    days110 = -1;
                                    break;
                                case 9:
                                    itemcode112 = "DE21";
                                    days110 = -1;
                                    break;
                                case 10:
                                    itemcode112 = "DE51";
                                    days110 = -1;
                                    break;
                                case 11:
                                    itemcode112 = "DC97";
                                    days110 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode112, days110);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode112, days110));
                            return;
                        case "CY63":
                            int num120 = Game_Server.Generic.random(0, 7);
                            int days111 = 1;
                            string itemcode113 = (string)null;
                            switch (num120)
                            {
                                case 0:
                                    itemcode113 = "CA04";
                                    days111 = 15;
                                    break;
                                case 1:
                                    itemcode113 = "CA04";
                                    days111 = 30;
                                    break;
                                case 2:
                                    itemcode113 = "CA04";
                                    days111 = 45;
                                    break;
                                case 3:
                                    itemcode113 = "CA04";
                                    days111 = 90;
                                    break;
                                case 4:
                                    itemcode113 = "CA04";
                                    days111 = 180;
                                    break;
                                case 5:
                                    itemcode113 = "CA04";
                                    days111 = 365;
                                    break;
                                case 6:
                                    itemcode113 = "CA04";
                                    days111 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode113, days111);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode113, days111));
                            return;
                        case "CY64":
                            int num121 = Game_Server.Generic.random(0, 24);
                            int days112 = 1;
                            string itemcode114 = (string)null;
                            switch (num121)
                            {
                                case 0:
                                    itemcode114 = "DC30";
                                    days112 = 15;
                                    break;
                                case 1:
                                    itemcode114 = "DC30";
                                    days112 = 30;
                                    break;
                                case 2:
                                    itemcode114 = "DC30";
                                    days112 = 45;
                                    break;
                                case 3:
                                    itemcode114 = "DC30";
                                    days112 = 90;
                                    break;
                                case 4:
                                    itemcode114 = "DC30";
                                    days112 = -1;
                                    break;
                                case 5:
                                    itemcode114 = "D810";
                                    days112 = 15;
                                    break;
                                case 6:
                                    itemcode114 = "D810";
                                    days112 = 30;
                                    break;
                                case 7:
                                    itemcode114 = "D810";
                                    days112 = 45;
                                    break;
                                case 8:
                                    itemcode114 = "D810";
                                    days112 = 90;
                                    break;
                                case 9:
                                    itemcode114 = "D810";
                                    days112 = -1;
                                    break;
                                case 10:
                                    itemcode114 = "DD13";
                                    days112 = 15;
                                    break;
                                case 11:
                                    itemcode114 = "DD13";
                                    days112 = 30;
                                    break;
                                case 12:
                                    itemcode114 = "DD13";
                                    days112 = 45;
                                    break;
                                case 13:
                                    itemcode114 = "DD13";
                                    days112 = 90;
                                    break;
                                case 14:
                                    itemcode114 = "DD13";
                                    days112 = -1;
                                    break;
                                case 15:
                                    itemcode114 = "GG27";
                                    days112 = 15;
                                    break;
                                case 16:
                                    itemcode114 = "GG27";
                                    days112 = 30;
                                    break;
                                case 17:
                                    itemcode114 = "GG27";
                                    days112 = 45;
                                    break;
                                case 18:
                                    itemcode114 = "GG27";
                                    days112 = 90;
                                    break;
                                case 19:
                                    itemcode114 = "GG27";
                                    days112 = -1;
                                    break;
                                case 20:
                                    itemcode114 = "GF36";
                                    days112 = 15;
                                    break;
                                case 21:
                                    itemcode114 = "GF36";
                                    days112 = 30;
                                    break;
                                case 22:
                                    itemcode114 = "GF36";
                                    days112 = 45;
                                    break;
                                case 23:
                                    itemcode114 = "GF36";
                                    days112 = 90;
                                    break;
                                case 24:
                                    itemcode114 = "GF36";
                                    days112 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode114, days112);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode114, days112));
                            return;
                        case "CY65":
                            int num122 = Game_Server.Generic.random(0, 25);
                            int days113 = 1;
                            string itemcode115 = (string)null;
                            switch (num122)
                            {
                                case 0:
                                    itemcode115 = "DC56";
                                    days113 = 15;
                                    break;
                                case 1:
                                    itemcode115 = "DC56";
                                    days113 = 30;
                                    break;
                                case 2:
                                    itemcode115 = "DC56";
                                    days113 = 45;
                                    break;
                                case 3:
                                    itemcode115 = "DC56";
                                    days113 = 90;
                                    break;
                                case 4:
                                    itemcode115 = "DC56";
                                    days113 = -1;
                                    break;
                                case 5:
                                    itemcode115 = "D609";
                                    days113 = 15;
                                    break;
                                case 6:
                                    itemcode115 = "D609";
                                    days113 = 30;
                                    break;
                                case 7:
                                    itemcode115 = "D609";
                                    days113 = 45;
                                    break;
                                case 8:
                                    itemcode115 = "D609";
                                    days113 = 90;
                                    break;
                                case 9:
                                    itemcode115 = "D609";
                                    days113 = -1;
                                    break;
                                case 10:
                                    itemcode115 = "DF79";
                                    days113 = 15;
                                    break;
                                case 11:
                                    itemcode115 = "DF79";
                                    days113 = 30;
                                    break;
                                case 12:
                                    itemcode115 = "DF79";
                                    days113 = 45;
                                    break;
                                case 13:
                                    itemcode115 = "DF79";
                                    days113 = 90;
                                    break;
                                case 14:
                                    itemcode115 = "DF79";
                                    days113 = -1;
                                    break;
                                case 15:
                                    itemcode115 = "GC08";
                                    days113 = 15;
                                    break;
                                case 16:
                                    itemcode115 = "GC08";
                                    days113 = 30;
                                    break;
                                case 17:
                                    itemcode115 = "GC08";
                                    days113 = 45;
                                    break;
                                case 18:
                                    itemcode115 = "GC08";
                                    days113 = 90;
                                    break;
                                case 19:
                                    itemcode115 = "GC08";
                                    days113 = -1;
                                    break;
                                case 20:
                                    itemcode115 = "GG28";
                                    days113 = 15;
                                    break;
                                case 21:
                                    itemcode115 = "GG28";
                                    days113 = 30;
                                    break;
                                case 22:
                                    itemcode115 = "GG28";
                                    days113 = 45;
                                    break;
                                case 23:
                                    itemcode115 = "GG28";
                                    days113 = 90;
                                    break;
                                case 24:
                                    itemcode115 = "GG28";
                                    days113 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode115, days113);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode115, days113));
                            return;
                        case "CY66":
                            int num125 = Game_Server.Generic.random(0, 13);
                            int days114 = 1;
                            string itemcode116 = (string)null;
                            switch (num125)
                            {
                                case 0:
                                    itemcode116 = "DC22";
                                    days114 = 30;
                                    break;
                                case 1:
                                    itemcode116 = "DC22";
                                    days114 = -1;
                                    break;
                                case 2:
                                    itemcode116 = "D807";
                                    days114 = -1;
                                    break;
                                case 3:
                                    itemcode116 = "D807";
                                    days114 = 30;
                                    break;
                                case 4:
                                    itemcode116 = "DC87";
                                    days114 = -1;
                                    break;
                                case 5:
                                    itemcode116 = "DC87";
                                    days114 = 30;
                                    break;
                                case 6:
                                    itemcode116 = "DD09";
                                    days114 = -1;
                                    break;
                                case 7:
                                    itemcode116 = "DD09";
                                    days114 = 30;
                                    break;
                                case 8:
                                    itemcode116 = "DF75";
                                    days114 = -1;
                                    break;
                                case 9:
                                    itemcode116 = "DF75";
                                    days114 = 30;
                                    break;
                                case 10:
                                    itemcode116 = "DE49";
                                    days114 = -1;
                                    break;
                                case 11:
                                    itemcode116 = "DE49";
                                    days114 = 30;
                                    break;
                                case 12:
                                    itemcode116 = "DG46";
                                    days114 = -1;
                                    break;
                                case 13:
                                    itemcode116 = "DG46";
                                    days114 = 30;
                                    break;
                                case 14:
                                    itemcode116 = "DT13";
                                    days114 = -1;
                                    break;
                                case 15:
                                    itemcode116 = "DT13";
                                    days114 = 30;
                                    break;
                                case 16:
                                    itemcode116 = "D827";
                                    days114 = -1;
                                    break;
                                case 17:
                                    itemcode116 = "D827";
                                    days114 = 30;
                                    break;
                                case 18:
                                    itemcode116 = "DF89";
                                    days114 = -1;
                                    break;
                                case 19:
                                    itemcode116 = "DF89";
                                    days114 = 30;
                                    break;
                                case 20:
                                    itemcode116 = "GF08";
                                    days114 = -1;
                                    break;
                                case 21:
                                    itemcode116 = "GF08";
                                    days114 = 30;
                                    break;
                                case 22:
                                    itemcode116 = "DG94";
                                    days114 = -1;
                                    break;
                                case 23:
                                    itemcode116 = "DG94";
                                    days114 = 30;
                                    break;
                                case 24:
                                    itemcode116 = "DD21";
                                    days114 = -1;
                                    break;
                                case 25:
                                    itemcode116 = "DD21";
                                    days114 = 30;
                                    break;
                                case 26:
                                    itemcode116 = "DT18";
                                    days114 = -1;
                                    break;
                                case 27:
                                    itemcode116 = "DT18";
                                    days114 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode116, days114);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode116, days114));
                            return;
                        case "CY67":
                            int num126 = Game_Server.Generic.random(0, 39);
                            int days115 = 1;
                            string itemcode117 = (string)null;
                            switch (num126)
                            {
                                case 0:
                                    itemcode117 = "DC26";
                                    days115 = 30;
                                    break;
                                case 1:
                                    itemcode117 = "DC26";
                                    days115 = -1;
                                    break;
                                case 2:
                                    itemcode117 = "D812";
                                    days115 = 30;
                                    break;
                                case 3:
                                    itemcode117 = "D812";
                                    days115 = -1;
                                    break;
                                case 4:
                                    itemcode117 = "DC88";
                                    days115 = 30;
                                    break;
                                case 5:
                                    itemcode117 = "DC88";
                                    days115 = -1;
                                    break;
                                case 6:
                                    itemcode117 = "DD10";
                                    days115 = 30;
                                    break;
                                case 7:
                                    itemcode117 = "DD10";
                                    days115 = -1;
                                    break;
                                case 8:
                                    itemcode117 = "DE28";
                                    days115 = 30;
                                    break;
                                case 9:
                                    itemcode117 = "DE28";
                                    days115 = -1;
                                    break;
                                case 10:
                                    itemcode117 = "DF58";
                                    days115 = 30;
                                    break;
                                case 11:
                                    itemcode117 = "DF58";
                                    days115 = -1;
                                    break;
                                case 12:
                                    itemcode117 = "DE29";
                                    days115 = 30;
                                    break;
                                case 13:
                                    itemcode117 = "DE29";
                                    days115 = -1;
                                    break;
                                case 14:
                                    itemcode117 = "DF59";
                                    days115 = 30;
                                    break;
                                case 15:
                                    itemcode117 = "DF59";
                                    days115 = -1;
                                    break;
                                case 16:
                                    itemcode117 = "DG54";
                                    days115 = 30;
                                    break;
                                case 17:
                                    itemcode117 = "DG54";
                                    days115 = -1;
                                    break;
                                case 18:
                                    itemcode117 = "DJ25";
                                    days115 = 30;
                                    break;
                                case 19:
                                    itemcode117 = "DJ25";
                                    days115 = -1;
                                    break;
                                case 20:
                                    itemcode117 = "DF81";
                                    days115 = 30;
                                    break;
                                case 21:
                                    itemcode117 = "DF81";
                                    days115 = -1;
                                    break;
                                case 22:
                                    itemcode117 = "DF91";
                                    days115 = 30;
                                    break;
                                case 23:
                                    itemcode117 = "DF91";
                                    days115 = -1;
                                    break;
                                case 24:
                                    itemcode117 = "DG82";
                                    days115 = 30;
                                    break;
                                case 25:
                                    itemcode117 = "DG82";
                                    days115 = -1;
                                    break;
                                case 26:
                                    itemcode117 = "DE62";
                                    days115 = 30;
                                    break;
                                case 27:
                                    itemcode117 = "DE62";
                                    days115 = -1;
                                    break;
                                case 28:
                                    itemcode117 = "DT15";
                                    days115 = 30;
                                    break;
                                case 29:
                                    itemcode117 = "DT15";
                                    days115 = -1;
                                    break;
                                case 30:
                                    itemcode117 = "DB42";
                                    days115 = 30;
                                    break;
                                case 31:
                                    itemcode117 = "DB42";
                                    days115 = -1;
                                    break;
                                case 32:
                                    itemcode117 = "GF17";
                                    days115 = 30;
                                    break;
                                case 33:
                                    itemcode117 = "GF17";
                                    days115 = -1;
                                    break;
                                case 34:
                                    itemcode117 = "GG05";
                                    days115 = 30;
                                    break;
                                case 35:
                                    itemcode117 = "GG05";
                                    days115 = -1;
                                    break;
                                case 36:
                                    itemcode117 = "DH14";
                                    days115 = 30;
                                    break;
                                case 37:
                                    itemcode117 = "DH14";
                                    days115 = -1;
                                    break;
                                case 38:
                                    itemcode117 = "DJ60";
                                    days115 = 30;
                                    break;
                                case 39:
                                    itemcode117 = "DJ60";
                                    days115 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode117, days115);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode117, days115));
                            return;
                        case "CY68":
                            int num190 = Game_Server.Generic.random(0, 15);
                            int days116 = 1;
                            string itemcode118 = (string)null;
                            switch (num190)
                            {
                                case 0:
                                    itemcode118 = "DC24";
                                    days116 = 30;
                                    break;
                                case 1:
                                    itemcode118 = "DC24";
                                    days116 = -1;
                                    break;
                                case 2:
                                    itemcode118 = "D808";
                                    days116 = -1;
                                    break;
                                case 3:
                                    itemcode118 = "D808";
                                    days116 = 30;
                                    break;
                                case 4:
                                    itemcode118 = "DC89";
                                    days116 = -1;
                                    break;
                                case 5:
                                    itemcode118 = "DC89";
                                    days116 = 30;
                                    break;
                                case 6:
                                    itemcode118 = "DE23";
                                    days116 = -1;
                                    break;
                                case 7:
                                    itemcode118 = "DE23";
                                    days116 = 30;
                                    break;
                                case 8:
                                    itemcode118 = "DD11";
                                    days116 = -1;
                                    break;
                                case 9:
                                    itemcode118 = "DD11";
                                    days116 = 30;
                                    break;
                                case 10:
                                    itemcode118 = "DF80";
                                    days116 = -1;
                                    break;
                                case 11:
                                    itemcode118 = "DF80";
                                    days116 = 30;
                                    break;
                                case 12:
                                    itemcode118 = "DG71";
                                    days116 = -1;
                                    break;
                                case 13:
                                    itemcode118 = "DG71";
                                    days116 = 30;
                                    break;
                                case 14:
                                    itemcode118 = "DJ31";
                                    days116 = -1;
                                    break;
                                case 15:
                                    itemcode118 = "DJ31";
                                    days116 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode118, days116);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode118, days116));
                            return;
                        case "CY69":
                            int num189 = Game_Server.Generic.random(0, 18);
                            int days117 = 1;
                            string itemcode119 = (string)null;
                            switch (num189)
                            {
                                case 0:
                                    itemcode119 = "DC62";
                                    days117 = 30;
                                    break;
                                case 1:
                                    itemcode119 = "DC62";
                                    days117 = -1;
                                    break;
                                case 2:
                                    itemcode119 = "DC17";
                                    days117 = 30;
                                    break;
                                case 3:
                                    itemcode119 = "D809";
                                    days117 = -1;
                                    break;
                                case 4:
                                    itemcode119 = "D809";
                                    days117 = 30;
                                    break;
                                case 5:
                                    itemcode119 = "DC83";
                                    days117 = -1;
                                    break;
                                case 6:
                                    itemcode119 = "DC83";
                                    days117 = 30;
                                    break;
                                case 7:
                                    itemcode119 = "DC90";
                                    days117 = -1;
                                    break;
                                case 8:
                                    itemcode119 = "DC90";
                                    days117 = 30;
                                    break;
                                case 9:
                                    itemcode119 = "DE24";
                                    days117 = -1;
                                    break;
                                case 10:
                                    itemcode119 = "DE24";
                                    days117 = 30;
                                    break;
                                case 11:
                                    itemcode119 = "DC83";
                                    days117 = -1;
                                    break;
                                case 12:
                                    itemcode119 = "DC83";
                                    days117 = 30;
                                    break;
                                case 13:
                                    itemcode119 = "DE45";
                                    days117 = -1;
                                    break;
                                case 14:
                                    itemcode119 = "DE45";
                                    days117 = 30;
                                    break;
                                case 15:
                                    itemcode119 = "DF69";
                                    days117 = -1;
                                    break;
                                case 16:
                                    itemcode119 = "DF69";
                                    days117 = 30;
                                    break;
                                case 17:
                                    itemcode119 = "DG55";
                                    days117 = -1;
                                    break;
                                case 18:
                                    itemcode119 = "DG55";
                                    days117 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode119, days117);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode119, days117));
                            return;
                        case "CY70":
                            int num127 = Game_Server.Generic.random(0, 11);
                            int days118 = 1;
                            string itemcode120 = (string)null;
                            switch (num127)
                            {
                                case 0:
                                    itemcode120 = "DJ33";
                                    days118 = -1;
                                    break;
                                case 1:
                                    itemcode120 = "DJ63";
                                    days118 = -1;
                                    break;
                                case 2:
                                    itemcode120 = "DJ07";
                                    days118 = -1;
                                    break;
                                case 3:
                                    itemcode120 = "DJ93";
                                    days118 = -1;
                                    break;
                                case 4:
                                    itemcode120 = "DJ13";
                                    days118 = -1;
                                    break;
                                case 5:
                                    itemcode120 = "DJ22";
                                    days118 = -1;
                                    break;
                                case 6:
                                    itemcode120 = "DJ23";
                                    days118 = -1;
                                    break;
                                case 7:
                                    itemcode120 = "DJ26";
                                    days118 = -1;
                                    break;
                                case 8:
                                    itemcode120 = "DJ37";
                                    days118 = -1;
                                    break;
                                case 9:
                                    itemcode120 = "DJ44";
                                    days118 = -1;
                                    break;
                                case 10:
                                    itemcode120 = "DJ45";
                                    days118 = -1;
                                    break;
                                case 11:
                                    itemcode120 = "DJ61";
                                    days118 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode120, days118);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode120, days118));
                            return;
                        case "CY71":
                            int num128 = Game_Server.Generic.random(0, 39);
                            int days119 = 1;
                            string itemcode121 = (string)null;
                            switch (num128)
                            {
                                case 0:
                                    itemcode121 = "CD01";
                                    days119 = 15;
                                    break;
                                case 1:
                                    itemcode121 = "CD01";
                                    days119 = 30;
                                    break;
                                case 2:
                                    itemcode121 = "CD01";
                                    days119 = 45;
                                    break;
                                case 3:
                                    itemcode121 = "CD01";
                                    days119 = 60;
                                    break;
                                case 4:
                                    itemcode121 = "CD02";
                                    days119 = 15;
                                    break;
                                case 5:
                                    itemcode121 = "CD02";
                                    days119 = 30;
                                    break;
                                case 6:
                                    itemcode121 = "CD02";
                                    days119 = 45;
                                    break;
                                case 7:
                                    itemcode121 = "CD02";
                                    days119 = 60;
                                    break;
                                case 8:
                                    itemcode121 = "CD03";
                                    days119 = 15;
                                    break;
                                case 9:
                                    itemcode121 = "CD03";
                                    days119 = 30;
                                    break;
                                case 10:
                                    itemcode121 = "CD03";
                                    days119 = 45;
                                    break;
                                case 11:
                                    itemcode121 = "CD03";
                                    days119 = 60;
                                    break;
                                case 12:
                                    itemcode121 = "CD04";
                                    days119 = 15;
                                    break;
                                case 13:
                                    itemcode121 = "CD04";
                                    days119 = 30;
                                    break;
                                case 14:
                                    itemcode121 = "CD04";
                                    days119 = 45;
                                    break;
                                case 15:
                                    itemcode121 = "CD04";
                                    days119 = 60;
                                    break;
                                case 16:
                                    itemcode121 = "CD05";
                                    days119 = 15;
                                    break;
                                case 17:
                                    itemcode121 = "CD05";
                                    days119 = 30;
                                    break;
                                case 18:
                                    itemcode121 = "CD05";
                                    days119 = 45;
                                    break;
                                case 19:
                                    itemcode121 = "CD05";
                                    days119 = 60;
                                    break;
                                case 20:
                                    itemcode121 = "CD06";
                                    days119 = 15;
                                    break;
                                case 21:
                                    itemcode121 = "CD06";
                                    days119 = 30;
                                    break;
                                case 22:
                                    itemcode121 = "CD06";
                                    days119 = 45;
                                    break;
                                case 23:
                                    itemcode121 = "CD06";
                                    days119 = 60;
                                    break;
                                case 24:
                                    itemcode121 = "CD07";
                                    days119 = 15;
                                    break;
                                case 25:
                                    itemcode121 = "CD07";
                                    days119 = 30;
                                    break;
                                case 26:
                                    itemcode121 = "CD07";
                                    days119 = 45;
                                    break;
                                case 27:
                                    itemcode121 = "CD07";
                                    days119 = 60;
                                    break;
                                case 28:
                                    itemcode121 = "CC05";
                                    days119 = 15;
                                    break;
                                case 29:
                                    itemcode121 = "CC05";
                                    days119 = 30;
                                    break;
                                case 30:
                                    itemcode121 = "CC05";
                                    days119 = 45;
                                    break;
                                case 31:
                                    itemcode121 = "CC05";
                                    days119 = 60;
                                    break;
                                case 32:
                                    itemcode121 = "CC72";
                                    days119 = 15;
                                    break;
                                case 33:
                                    itemcode121 = "CC72";
                                    days119 = 30;
                                    break;
                                case 34:
                                    itemcode121 = "CC72";
                                    days119 = 45;
                                    break;
                                case 35:
                                    itemcode121 = "CC72";
                                    days119 = 60;
                                    break;
                                case 36:
                                    itemcode121 = "CC76";
                                    days119 = 15;
                                    break;
                                case 37:
                                    itemcode121 = "CC76";
                                    days119 = 30;
                                    break;
                                case 38:
                                    itemcode121 = "CC76";
                                    days119 = 45;
                                    break;
                                case 39:
                                    itemcode121 = "CC76";
                                    days119 = 60;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode121, days119);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode121, days119));
                            return;
                        case "CY73":
                            int num129 = Game_Server.Generic.random(0, 8);
                            int days120 = 1;
                            string itemcode122 = (string)null;
                            switch (num129)
                            {
                                case 0:
                                    itemcode122 = "BK14";
                                    days120 = 30;
                                    break;
                                case 1:
                                    itemcode122 = "BK15";
                                    days120 = 30;
                                    break;
                                case 2:
                                    itemcode122 = "CM07";
                                    days120 = 5000;
                                    break;
                                case 3:
                                    itemcode122 = "GA02";
                                    days120 = 30;
                                    break;
                                case 4:
                                    itemcode122 = "GA02";
                                    days120 = -1;
                                    break;
                                case 5:
                                    itemcode122 = "BD16";
                                    days120 = 30;
                                    break;
                                case 6:
                                    itemcode122 = "BD16";
                                    days120 = -1;
                                    break;
                                case 7:
                                    itemcode122 = "CE02";
                                    days120 = 30;
                                    break;
                                case 8:
                                    itemcode122 = "CE02";
                                    days120 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode122, days120);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode122, days120));
                            return;
                        case "CY74":
                            int num130 = Game_Server.Generic.random(0, 39);
                            int days121 = 1;
                            string itemcode123 = (string)null;
                            switch (num130)
                            {
                                case 0:
                                    itemcode123 = "DC87";
                                    days121 = 30;
                                    break;
                                case 1:
                                    itemcode123 = "DC88";
                                    days121 = 30;
                                    break;
                                case 2:
                                    itemcode123 = "DC89";
                                    days121 = 30;
                                    break;
                                case 3:
                                    itemcode123 = "DC90";
                                    days121 = 30;
                                    break;
                                case 4:
                                    itemcode123 = "DC96";
                                    days121 = 30;
                                    break;
                                case 5:
                                    itemcode123 = "DE17";
                                    days121 = 30;
                                    break;
                                case 6:
                                    itemcode123 = "DE18";
                                    days121 = 30;
                                    break;
                                case 7:
                                    itemcode123 = "DE19";
                                    days121 = 30;
                                    break;
                                case 8:
                                    itemcode123 = "DE20";
                                    days121 = 30;
                                    break;
                                case 9:
                                    itemcode123 = "DE21";
                                    days121 = 30;
                                    break;
                                case 10:
                                    itemcode123 = "DE51";
                                    days121 = 30;
                                    break;
                                case 11:
                                    itemcode123 = "DC97";
                                    days121 = 30;
                                    break;
                                case 12:
                                    itemcode123 = "GC21";
                                    days121 = -1;
                                    break;
                                case 13:
                                    itemcode123 = "GC15";
                                    days121 = 30;
                                    break;
                                case 14:
                                    itemcode123 = "GC11";
                                    days121 = 30;
                                    break;
                                case 15:
                                    itemcode123 = "DE80";
                                    days121 = 30;
                                    break;
                                case 16:
                                    itemcode123 = "GC04";
                                    days121 = 30;
                                    break;
                                case 17:
                                    itemcode123 = "DE74";
                                    days121 = 30;
                                    break;
                                case 18:
                                    itemcode123 = "DE73";
                                    days121 = 30;
                                    break;
                                case 19:
                                    itemcode123 = "DE52";
                                    days121 = 30;
                                    break;
                                case 20:
                                    itemcode123 = "DC87";
                                    days121 = -1;
                                    break;
                                case 21:
                                    itemcode123 = "DC88";
                                    days121 = -1;
                                    break;
                                case 22:
                                    itemcode123 = "DC89";
                                    days121 = -1;
                                    break;
                                case 23:
                                    itemcode123 = "DC90";
                                    days121 = -1;
                                    break;
                                case 24:
                                    itemcode123 = "DC96";
                                    days121 = -1;
                                    break;
                                case 25:
                                    itemcode123 = "DE17";
                                    days121 = -1;
                                    break;
                                case 26:
                                    itemcode123 = "DE18";
                                    days121 = -1;
                                    break;
                                case 27:
                                    itemcode123 = "DE19";
                                    days121 = -1;
                                    break;
                                case 28:
                                    itemcode123 = "DE20";
                                    days121 = -1;
                                    break;
                                case 29:
                                    itemcode123 = "DE21";
                                    days121 = -1;
                                    break;
                                case 30:
                                    itemcode123 = "DE51";
                                    days121 = -1;
                                    break;
                                case 31:
                                    itemcode123 = "DC97";
                                    days121 = -1;
                                    break;
                                case 32:
                                    itemcode123 = "GC21";
                                    days121 = 30;
                                    break;
                                case 33:
                                    itemcode123 = "GC15";
                                    days121 = -1;
                                    break;
                                case 34:
                                    itemcode123 = "GC11";
                                    days121 = -1;
                                    break;
                                case 35:
                                    itemcode123 = "DE80";
                                    days121 = -1;
                                    break;
                                case 36:
                                    itemcode123 = "GC04";
                                    days121 = -1;
                                    break;
                                case 37:
                                    itemcode123 = "DE74";
                                    days121 = -1;
                                    break;
                                case 38:
                                    itemcode123 = "DE73";
                                    days121 = -1;
                                    break;
                                case 39:
                                    itemcode123 = "DE52";
                                    days121 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode123, days121);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode123, days121));
                            return;
                        case "CY75":
                            int num131 = Game_Server.Generic.random(0, 49);
                            int days122 = 1;
                            string itemcode124 = (string)null;
                            switch (num131)
                            {
                                case 0:
                                    itemcode124 = "GF50";
                                    days122 = -1;
                                    break;
                                case 1:
                                    itemcode124 = "GF42";
                                    days122 = -1;
                                    break;
                                case 2:
                                    itemcode124 = "GF41";
                                    days122 = -1;
                                    break;
                                case 3:
                                    itemcode124 = "GF36";
                                    days122 = -1;
                                    break;
                                case 4:
                                    itemcode124 = "GF35";
                                    days122 = -1;
                                    break;
                                case 5:
                                    itemcode124 = "GF19";
                                    days122 = -1;
                                    break;
                                case 6:
                                    itemcode124 = "DF99";
                                    days122 = -1;
                                    break;
                                case 7:
                                    itemcode124 = "DF98";
                                    days122 = -1;
                                    break;
                                case 8:
                                    itemcode124 = "DF97";
                                    days122 = -1;
                                    break;
                                case 9:
                                    itemcode124 = "DF94";
                                    days122 = -1;
                                    break;
                                case 10:
                                    itemcode124 = "GF52";
                                    days122 = -1;
                                    break;
                                case 11:
                                    itemcode124 = "DF89";
                                    days122 = -1;
                                    break;
                                case 12:
                                    itemcode124 = "DF85";
                                    days122 = -1;
                                    break;
                                case 13:
                                    itemcode124 = "DF79";
                                    days122 = -1;
                                    break;
                                case 14:
                                    itemcode124 = "DF75";
                                    days122 = -1;
                                    break;
                                case 15:
                                    itemcode124 = "DF74";
                                    days122 = -1;
                                    break;
                                case 16:
                                    itemcode124 = "DF69";
                                    days122 = -1;
                                    break;
                                case 17:
                                    itemcode124 = "DF58";
                                    days122 = -1;
                                    break;
                                case 18:
                                    itemcode124 = "DF52";
                                    days122 = -1;
                                    break;
                                case 19:
                                    itemcode124 = "DF51";
                                    days122 = -1;
                                    break;
                                case 20:
                                    itemcode124 = "DF50";
                                    days122 = -1;
                                    break;
                                case 21:
                                    itemcode124 = "DF47";
                                    days122 = -1;
                                    break;
                                case 22:
                                    itemcode124 = "DF25";
                                    days122 = -1;
                                    break;
                                case 23:
                                    itemcode124 = "DF35";
                                    days122 = -1;
                                    break;
                                case 24:
                                    itemcode124 = "GF50";
                                    days122 = 30;
                                    break;
                                case 25:
                                    itemcode124 = "GF42";
                                    days122 = 30;
                                    break;
                                case 26:
                                    itemcode124 = "GF41";
                                    days122 = 30;
                                    break;
                                case 27:
                                    itemcode124 = "GF36";
                                    days122 = 30;
                                    break;
                                case 28:
                                    itemcode124 = "GF35";
                                    days122 = 30;
                                    break;
                                case 29:
                                    itemcode124 = "GF19";
                                    days122 = 30;
                                    break;
                                case 30:
                                    itemcode124 = "DF99";
                                    days122 = 30;
                                    break;
                                case 31:
                                    itemcode124 = "DF98";
                                    days122 = 30;
                                    break;
                                case 32:
                                    itemcode124 = "DF97";
                                    days122 = 30;
                                    break;
                                case 33:
                                    itemcode124 = "DF94";
                                    days122 = 30;
                                    break;
                                case 34:
                                    itemcode124 = "GF52";
                                    days122 = 30;
                                    break;
                                case 35:
                                    itemcode124 = "DF89";
                                    days122 = 30;
                                    break;
                                case 36:
                                    itemcode124 = "DF85";
                                    days122 = 30;
                                    break;
                                case 37:
                                    itemcode124 = "DF79";
                                    days122 = 30;
                                    break;
                                case 38:
                                    itemcode124 = "DF75";
                                    days122 = 30;
                                    break;
                                case 39:
                                    itemcode124 = "DF74";
                                    days122 = 30;
                                    break;
                                case 40:
                                    itemcode124 = "DF69";
                                    days122 = 30;
                                    break;
                                case 41:
                                    itemcode124 = "DF58";
                                    days122 = 30;
                                    break;
                                case 42:
                                    itemcode124 = "DF52";
                                    days122 = 30;
                                    break;
                                case 43:
                                    itemcode124 = "DF51";
                                    days122 = 30;
                                    break;
                                case 46:
                                    itemcode124 = "DF50";
                                    days122 = 30;
                                    break;
                                case 47:
                                    itemcode124 = "DF47";
                                    days122 = 30;
                                    break;
                                case 48:
                                    itemcode124 = "DF25";
                                    days122 = 30;
                                    break;
                                case 49:
                                    itemcode124 = "DF35";
                                    days122 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode124, days122);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode124, days122));
                            return;
                        case "CY76":
                            int num132 = Game_Server.Generic.random(0, 21);
                            int days123 = 1;
                            string itemcode125 = (string)null;
                            switch (num132)
                            {
                                case 0:
                                    itemcode125 = "GG44";
                                    days123 = -1;
                                    break;
                                case 1:
                                    itemcode125 = "GG33";
                                    days123 = -1;
                                    break;
                                case 2:
                                    itemcode125 = "GG29";
                                    days123 = -1;
                                    break;
                                case 3:
                                    itemcode125 = "GG19";
                                    days123 = -1;
                                    break;
                                case 4:
                                    itemcode125 = "GG18";
                                    days123 = -1;
                                    break;
                                case 5:
                                    itemcode125 = "GG09";
                                    days123 = -1;
                                    break;
                                case 6:
                                    itemcode125 = "DG36";
                                    days123 = -1;
                                    break;
                                case 7:
                                    itemcode125 = "DG61";
                                    days123 = -1;
                                    break;
                                case 8:
                                    itemcode125 = "DG23";
                                    days123 = -1;
                                    break;
                                case 9:
                                    itemcode125 = "DG31";
                                    days123 = -1;
                                    break;
                                case 10:
                                    itemcode125 = "GG44";
                                    days123 = 30;
                                    break;
                                case 11:
                                    itemcode125 = "GG33";
                                    days123 = 30;
                                    break;
                                case 12:
                                    itemcode125 = "GG29";
                                    days123 = 30;
                                    break;
                                case 13:
                                    itemcode125 = "GG19";
                                    days123 = 30;
                                    break;
                                case 14:
                                    itemcode125 = "GG18";
                                    days123 = 30;
                                    break;
                                case 15:
                                    itemcode125 = "GG09";
                                    days123 = 30;
                                    break;
                                case 16:
                                    itemcode125 = "DG36";
                                    days123 = 30;
                                    break;
                                case 17:
                                    itemcode125 = "DG61";
                                    days123 = 30;
                                    break;
                                case 18:
                                    itemcode125 = "DG23";
                                    days123 = 30;
                                    break;
                                case 19:
                                    itemcode125 = "DG31";
                                    days123 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode125, days123);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode125, days123));
                            return;
                        case "CY77":
                            int num133 = Game_Server.Generic.random(0, 21);
                            int days124 = 1;
                            string itemcode126 = (string)null;
                            switch (num133)
                            {
                                case 0:
                                    itemcode126 = "DJ79";
                                    days124 = -1;
                                    break;
                                case 1:
                                    itemcode126 = "DJ45";
                                    days124 = -1;
                                    break;
                                case 2:
                                    itemcode126 = "DJ44";
                                    days124 = -1;
                                    break;
                                case 3:
                                    itemcode126 = "DJ37";
                                    days124 = -1;
                                    break;
                                case 4:
                                    itemcode126 = "DJ23";
                                    days124 = -1;
                                    break;
                                case 5:
                                    itemcode126 = "DJ22";
                                    days124 = -1;
                                    break;
                                case 6:
                                    itemcode126 = "DJ21";
                                    days124 = -1;
                                    break;
                                case 7:
                                    itemcode126 = "DJ93";
                                    days124 = -1;
                                    break;
                                case 8:
                                    itemcode126 = "DJ07";
                                    days124 = -1;
                                    break;
                                case 9:
                                    itemcode126 = "DJ63";
                                    days124 = -1;
                                    break;
                                case 10:
                                    itemcode126 = "DJ33";
                                    days124 = -1;
                                    break;
                                case 11:
                                    itemcode126 = "DJ79";
                                    days124 = -1;
                                    break;
                                case 12:
                                    itemcode126 = "DJ45";
                                    days124 = -1;
                                    break;
                                case 13:
                                    itemcode126 = "DJ44";
                                    days124 = -1;
                                    break;
                                case 14:
                                    itemcode126 = "DJ37";
                                    days124 = -1;
                                    break;
                                case 15:
                                    itemcode126 = "DJ23";
                                    days124 = -1;
                                    break;
                                case 16:
                                    itemcode126 = "DJ22";
                                    days124 = -1;
                                    break;
                                case 17:
                                    itemcode126 = "DJ21";
                                    days124 = -1;
                                    break;
                                case 18:
                                    itemcode126 = "DJ93";
                                    days124 = -1;
                                    break;
                                case 19:
                                    itemcode126 = "DJ07";
                                    days124 = -1;
                                    break;
                                case 20:
                                    itemcode126 = "DJ63";
                                    days124 = -1;
                                    break;
                                case 21:
                                    itemcode126 = "DJ33";
                                    days124 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode126, days124);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode126, days124));
                            return;
                        case "CY78":
                            int num134 = Game_Server.Generic.random(0, 39);
                            int days125 = 1;
                            string itemcode127 = (string)null;
                            switch (num134)
                            {
                                case 0:
                                    itemcode127 = "DF70";
                                    days125 = -1;
                                    break;
                                case 1:
                                    itemcode127 = "DF70";
                                    days125 = 30;
                                    break;
                                case 2:
                                    itemcode127 = "DF70";
                                    days125 = 15;
                                    break;
                                case 3:
                                    itemcode127 = "DF87";
                                    days125 = -1;
                                    break;
                                case 4:
                                    itemcode127 = "DF87";
                                    days125 = 30;
                                    break;
                                case 5:
                                    itemcode127 = "DF87";
                                    days125 = 15;
                                    break;
                                case 6:
                                    itemcode127 = "D607";
                                    days125 = -1;
                                    break;
                                case 7:
                                    itemcode127 = "D607";
                                    days125 = 30;
                                    break;
                                case 8:
                                    itemcode127 = "D607";
                                    days125 = 15;
                                    break;
                                case 9:
                                    itemcode127 = "GF02";
                                    days125 = -1;
                                    break;
                                case 10:
                                    itemcode127 = "GF02";
                                    days125 = 30;
                                    break;
                                case 11:
                                    itemcode127 = "GF02";
                                    days125 = 15;
                                    break;
                                case 12:
                                    itemcode127 = "GF06";
                                    days125 = -1;
                                    break;
                                case 13:
                                    itemcode127 = "GF06";
                                    days125 = 30;
                                    break;
                                case 14:
                                    itemcode127 = "GF06";
                                    days125 = 15;
                                    break;
                                case 15:
                                    itemcode127 = "D609";
                                    days125 = -1;
                                    break;
                                case 16:
                                    itemcode127 = "D609";
                                    days125 = 30;
                                    break;
                                case 17:
                                    itemcode127 = "D609";
                                    days125 = 15;
                                    break;
                                case 18:
                                    itemcode127 = "GF09";
                                    days125 = -1;
                                    break;
                                case 19:
                                    itemcode127 = "GF09";
                                    days125 = 30;
                                    break;
                                case 20:
                                    itemcode127 = "GF09";
                                    days125 = 15;
                                    break;
                                case 21:
                                    itemcode127 = "GF12";
                                    days125 = -1;
                                    break;
                                case 22:
                                    itemcode127 = "GF12";
                                    days125 = 30;
                                    break;
                                case 23:
                                    itemcode127 = "GF12";
                                    days125 = 15;
                                    break;
                                case 24:
                                    itemcode127 = "GF17";
                                    days125 = -1;
                                    break;
                                case 25:
                                    itemcode127 = "GF17";
                                    days125 = 30;
                                    break;
                                case 26:
                                    itemcode127 = "GF17";
                                    days125 = 15;
                                    break;
                                case 27:
                                    itemcode127 = "GF26";
                                    days125 = -1;
                                    break;
                                case 28:
                                    itemcode127 = "GF26";
                                    days125 = 30;
                                    break;
                                case 29:
                                    itemcode127 = "GF26";
                                    days125 = 15;
                                    break;
                                case 30:
                                    itemcode127 = "GF31";
                                    days125 = -1;
                                    break;
                                case 31:
                                    itemcode127 = "GF44";
                                    days125 = 30;
                                    break;
                                case 32:
                                    itemcode127 = "GF44";
                                    days125 = 15;
                                    break;
                                case 33:
                                    itemcode127 = "GF44";
                                    days125 = -1;
                                    break;
                                case 34:
                                    itemcode127 = "GF51";
                                    days125 = 30;
                                    break;
                                case 35:
                                    itemcode127 = "GF51";
                                    days125 = 15;
                                    break;
                                case 36:
                                    itemcode127 = "GF51";
                                    days125 = -1;
                                    break;
                                case 37:
                                    itemcode127 = "GF56";
                                    days125 = 30;
                                    break;
                                case 38:
                                    itemcode127 = "GF56";
                                    days125 = 15;
                                    break;
                                case 39:
                                    itemcode127 = "GF56";
                                    days125 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode127, days125);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode127, days125));
                            return;
                        case "CY79":
                            int num135 = Game_Server.Generic.random(0, 50);
                            int days126 = 1;
                            string itemcode128 = (string)null;
                            switch (num135)
                            {
                                case 0:
                                    itemcode128 = "DC01";
                                    days126 = -1;
                                    break;
                                case 1:
                                    itemcode128 = "DC01";
                                    days126 = 30;
                                    break;
                                case 2:
                                    itemcode128 = "DC01";
                                    days126 = 15;
                                    break;
                                case 3:
                                    itemcode128 = "DC31";
                                    days126 = -1;
                                    break;
                                case 4:
                                    itemcode128 = "DC31";
                                    days126 = 30;
                                    break;
                                case 5:
                                    itemcode128 = "DC31";
                                    days126 = 15;
                                    break;
                                case 6:
                                    itemcode128 = "DC65";
                                    days126 = -1;
                                    break;
                                case 7:
                                    itemcode128 = "DC65";
                                    days126 = 30;
                                    break;
                                case 8:
                                    itemcode128 = "DC65";
                                    days126 = 15;
                                    break;
                                case 9:
                                    itemcode128 = "DC91";
                                    days126 = -1;
                                    break;
                                case 10:
                                    itemcode128 = "DC91";
                                    days126 = 30;
                                    break;
                                case 11:
                                    itemcode128 = "DC91";
                                    days126 = 15;
                                    break;
                                case 12:
                                    itemcode128 = "DC70";
                                    days126 = -1;
                                    break;
                                case 13:
                                    itemcode128 = "DC70";
                                    days126 = 30;
                                    break;
                                case 14:
                                    itemcode128 = "DC70";
                                    days126 = 15;
                                    break;
                                case 15:
                                    itemcode128 = "DC72";
                                    days126 = -1;
                                    break;
                                case 16:
                                    itemcode128 = "DC72";
                                    days126 = 30;
                                    break;
                                case 17:
                                    itemcode128 = "DC72";
                                    days126 = 15;
                                    break;
                                case 18:
                                    itemcode128 = "DC84";
                                    days126 = -1;
                                    break;
                                case 19:
                                    itemcode128 = "DC84";
                                    days126 = 30;
                                    break;
                                case 20:
                                    itemcode128 = "DC84";
                                    days126 = 15;
                                    break;
                                case 21:
                                    itemcode128 = "DC85";
                                    days126 = -1;
                                    break;
                                case 22:
                                    itemcode128 = "DC85";
                                    days126 = 30;
                                    break;
                                case 23:
                                    itemcode128 = "DC85";
                                    days126 = 15;
                                    break;
                                case 24:
                                    itemcode128 = "DE52";
                                    days126 = -1;
                                    break;
                                case 25:
                                    itemcode128 = "DE52";
                                    days126 = 30;
                                    break;
                                case 26:
                                    itemcode128 = "DE52";
                                    days126 = 15;
                                    break;
                                case 27:
                                    itemcode128 = "DE70";
                                    days126 = -1;
                                    break;
                                case 28:
                                    itemcode128 = "DE70";
                                    days126 = 30;
                                    break;
                                case 29:
                                    itemcode128 = "DE70";
                                    days126 = 15;
                                    break;
                                case 30:
                                    itemcode128 = "DE73";
                                    days126 = -1;
                                    break;
                                case 31:
                                    itemcode128 = "DE73";
                                    days126 = 30;
                                    break;
                                case 32:
                                    itemcode128 = "DE73";
                                    days126 = 15;
                                    break;
                                case 33:
                                    itemcode128 = "DE74";
                                    days126 = -1;
                                    break;
                                case 34:
                                    itemcode128 = "DE74";
                                    days126 = 30;
                                    break;
                                case 35:
                                    itemcode128 = "DE74";
                                    days126 = 15;
                                    break;
                                case 36:
                                    itemcode128 = "DE80";
                                    days126 = -1;
                                    break;
                                case 37:
                                    itemcode128 = "DE80";
                                    days126 = 30;
                                    break;
                                case 38:
                                    itemcode128 = "DE80";
                                    days126 = 15;
                                    break;
                                case 39:
                                    itemcode128 = "GC04";
                                    days126 = -1;
                                    break;
                                case 40:
                                    itemcode128 = "GC04";
                                    days126 = 30;
                                    break;
                                case 41:
                                    itemcode128 = "GC04";
                                    days126 = 15;
                                    break;
                                case 42:
                                    itemcode128 = "GC11";
                                    days126 = -1;
                                    break;
                                case 43:
                                    itemcode128 = "GC11";
                                    days126 = 30;
                                    break;
                                case 44:
                                    itemcode128 = "GC11";
                                    days126 = 15;
                                    break;
                                case 45:
                                    itemcode128 = "GC15";
                                    days126 = -1;
                                    break;
                                case 46:
                                    itemcode128 = "GC15";
                                    days126 = 30;
                                    break;
                                case 47:
                                    itemcode128 = "GC15";
                                    days126 = 15;
                                    break;
                                case 48:
                                    itemcode128 = "GC21";
                                    days126 = -1;
                                    break;
                                case 49:
                                    itemcode128 = "GC21";
                                    days126 = 30;
                                    break;
                                case 50:
                                    itemcode128 = "GC21";
                                    days126 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode128, days126);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode128, days126));
                            return;
                        case "CY84":
                            int num136 = Game_Server.Generic.random(0, 21);
                            int days127 = 1;
                            string itemcode129 = (string)null;
                            switch (num136)
                            {
                                case 0:
                                    itemcode129 = "DD19";
                                    days127 = 30;
                                    break;
                                case 1:
                                    itemcode129 = "DD19";
                                    days127 = -1;
                                    break;
                                case 2:
                                    itemcode129 = "DE74";
                                    days127 = 30;
                                    break;
                                case 3:
                                    itemcode129 = "DE74";
                                    days127 = -1;
                                    break;
                                case 4:
                                    itemcode129 = "DJ45";
                                    days127 = 30;
                                    break;
                                case 5:
                                    itemcode129 = "DJ45";
                                    days127 = -1;
                                    break;
                                case 6:
                                    itemcode129 = "DG92";
                                    days127 = 30;
                                    break;
                                case 7:
                                    itemcode129 = "DG92";
                                    days127 = -1;
                                    break;
                                case 8:
                                    itemcode129 = "DB46";
                                    days127 = 30;
                                    break;
                                case 9:
                                    itemcode129 = "DB46";
                                    days127 = -1;
                                    break;
                                case 10:
                                    itemcode129 = "DB43";
                                    days127 = 30;
                                    break;
                                case 11:
                                    itemcode129 = "DF97";
                                    days127 = 30;
                                    break;
                                case 12:
                                    itemcode129 = "DF97";
                                    days127 = -1;
                                    break;
                                case 13:
                                    itemcode129 = "DG86";
                                    days127 = 30;
                                    break;
                                case 14:
                                    itemcode129 = "DG86";
                                    days127 = -1;
                                    break;
                                case 15:
                                    itemcode129 = "DE66";
                                    days127 = 30;
                                    break;
                                case 16:
                                    itemcode129 = "DE66";
                                    days127 = -1;
                                    break;
                                case 17:
                                    itemcode129 = "DJ43";
                                    days127 = 30;
                                    break;
                                case 18:
                                    itemcode129 = "DJ43";
                                    days127 = -1;
                                    break;
                                case 19:
                                    itemcode129 = "CF01";
                                    days127 = 15;
                                    break;
                                case 20:
                                    itemcode129 = "CZ81";
                                    days127 = 1;
                                    break;
                                case 21:
                                    itemcode129 = "DS01";
                                    days127 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode129, days127);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode129, days127));
                            return;
                        case "CY96":
                            int num137 = Game_Server.Generic.random(0, 3);
                            int days128 = 1;
                            string itemcode130 = (string)null;
                            switch (num137)
                            {
                                case 0:
                                    itemcode130 = "DC61";
                                    days128 = 30;
                                    break;
                                case 1:
                                    itemcode130 = "DC61";
                                    days128 = -1;
                                    break;
                                case 2:
                                    itemcode130 = "DC34";
                                    days128 = 30;
                                    break;
                                case 3:
                                    itemcode130 = "DC34";
                                    days128 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode130, days128);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode130, days128));
                            return;
                        case "CY97":
                            int num138 = Game_Server.Generic.random(0, 6);
                            int days129 = 1;
                            string itemcode131 = (string)null;
                            switch (num138)
                            {
                                case 0:
                                    itemcode131 = "DG51";
                                    days129 = 30;
                                    break;
                                case 1:
                                    itemcode131 = "CZ81";
                                    days129 = 1;
                                    break;
                                case 2:
                                    itemcode131 = "DC80";
                                    days129 = 30;
                                    break;
                                case 3:
                                    itemcode131 = "DC98";
                                    days129 = 30;
                                    break;
                                case 4:
                                    itemcode131 = "DE30";
                                    days129 = 30;
                                    break;
                                case 5:
                                    itemcode131 = "DE46";
                                    days129 = 30;
                                    break;
                                case 6:
                                    itemcode131 = "CB09";
                                    days129 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode131, days129);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode131, days129));
                            return;
                        case "CY98":
                            int num139 = Game_Server.Generic.random(0, 58);
                            int days130 = 1;
                            string itemcode132 = (string)null;
                            switch (num139)
                            {
                                case 0:
                                    itemcode132 = "DB10";
                                    days130 = -1;
                                    break;
                                case 1:
                                    itemcode132 = "DB10";
                                    days130 = 30;
                                    break;
                                case 2:
                                    itemcode132 = "DB10";
                                    days130 = -1;
                                    break;
                                case 3:
                                    itemcode132 = "DC33";
                                    days130 = 30;
                                    break;
                                case 4:
                                    itemcode132 = "DC33";
                                    days130 = -1;
                                    break;
                                case 5:
                                    itemcode132 = "DF35";
                                    days130 = 30;
                                    break;
                                case 6:
                                    itemcode132 = "DF35";
                                    days130 = -1;
                                    break;
                                case 7:
                                    itemcode132 = "DG13";
                                    days130 = 30;
                                    break;
                                case 8:
                                    itemcode132 = "DG13";
                                    days130 = -1;
                                    break;
                                case 9:
                                    itemcode132 = "DJ33";
                                    days130 = -1;
                                    break;
                                case 10:
                                    itemcode132 = "DJ33";
                                    days130 = 30;
                                    break;
                                case 11:
                                    itemcode132 = "DC64";
                                    days130 = 30;
                                    break;
                                case 12:
                                    itemcode132 = "DC64";
                                    days130 = -1;
                                    break;
                                case 13:
                                    itemcode132 = "DB16";
                                    days130 = 30;
                                    break;
                                case 14:
                                    itemcode132 = "DB16";
                                    days130 = -1;
                                    break;
                                case 15:
                                    itemcode132 = "DF37";
                                    days130 = 30;
                                    break;
                                case 16:
                                    itemcode132 = "DF37";
                                    days130 = -1;
                                    break;
                                case 17:
                                    itemcode132 = "DC39";
                                    days130 = -1;
                                    break;
                                case 18:
                                    itemcode132 = "DF95";
                                    days130 = 30;
                                    break;
                                case 19:
                                    itemcode132 = "DC39";
                                    days130 = 30;
                                    break;
                                case 20:
                                    itemcode132 = "DF95";
                                    days130 = -1;
                                    break;
                                case 21:
                                    itemcode132 = "DE07";
                                    days130 = 30;
                                    break;
                                case 22:
                                    itemcode132 = "DE07";
                                    days130 = 30;
                                    break;
                                case 23:
                                    itemcode132 = "DA45";
                                    days130 = -1;
                                    break;
                                case 24:
                                    itemcode132 = "DF77";
                                    days130 = -1;
                                    break;
                                case 25:
                                    itemcode132 = "DF77";
                                    days130 = 30;
                                    break;
                                case 26:
                                    itemcode132 = "DB63";
                                    days130 = 30;
                                    break;
                                case 27:
                                    itemcode132 = "DH09";
                                    days130 = 30;
                                    break;
                                case 28:
                                    itemcode132 = "DB63";
                                    days130 = -1;
                                    break;
                                case 29:
                                    itemcode132 = "DH09";
                                    days130 = -1;
                                    break;
                                case 30:
                                    itemcode132 = "DF60";
                                    days130 = 30;
                                    break;
                                case 31:
                                    itemcode132 = "DF60";
                                    days130 = -1;
                                    break;
                                case 32:
                                    itemcode132 = "DE30";
                                    days130 = 30;
                                    break;
                                case 33:
                                    itemcode132 = "DE30";
                                    days130 = -1;
                                    break;
                                case 34:
                                    itemcode132 = "DE35";
                                    days130 = 30;
                                    break;
                                case 35:
                                    itemcode132 = "DE35";
                                    days130 = -1;
                                    break;
                                case 36:
                                    itemcode132 = "DE37";
                                    days130 = 30;
                                    break;
                                case 37:
                                    itemcode132 = "DE37";
                                    days130 = -1;
                                    break;
                                case 38:
                                    itemcode132 = "DE38";
                                    days130 = 30;
                                    break;
                                case 39:
                                    itemcode132 = "DE38";
                                    days130 = -1;
                                    break;
                                case 40:
                                    itemcode132 = "DE40";
                                    days130 = 30;
                                    break;
                                case 41:
                                    itemcode132 = "DE40";
                                    days130 = -1;
                                    break;
                                case 42:
                                    itemcode132 = "DE41";
                                    days130 = 30;
                                    break;
                                case 43:
                                    itemcode132 = "DE41";
                                    days130 = -1;
                                    break;
                                case 44:
                                    itemcode132 = "DE43";
                                    days130 = 30;
                                    break;
                                case 45:
                                    itemcode132 = "DE43";
                                    days130 = -1;
                                    break;
                                case 46:
                                    itemcode132 = "DE52";
                                    days130 = 30;
                                    break;
                                case 47:
                                    itemcode132 = "DE52";
                                    days130 = -1;
                                    break;
                                case 48:
                                    itemcode132 = "DA45";
                                    days130 = 30;
                                    break;
                                case 49:
                                    itemcode132 = "DF77";
                                    days130 = -1;
                                    break;
                                case 50:
                                    itemcode132 = "DF77";
                                    days130 = 30;
                                    break;
                                case 51:
                                    itemcode132 = "DA72";
                                    days130 = -1;
                                    break;
                                case 52:
                                    itemcode132 = "DA72";
                                    days130 = 30;
                                    break;
                                case 53:
                                    itemcode132 = "DE53";
                                    days130 = -1;
                                    break;
                                case 54:
                                    itemcode132 = "DE53";
                                    days130 = 30;
                                    break;
                                case 55:
                                    itemcode132 = "DN14";
                                    days130 = -1;
                                    break;
                                case 56:
                                    itemcode132 = "DN14";
                                    days130 = 30;
                                    break;
                                case 57:
                                    itemcode132 = "DB49";
                                    days130 = -1;
                                    break;
                                case 58:
                                    itemcode132 = "DB49";
                                    days130 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode132, days130);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode132, days130));
                            return;
                        case "CY99":
                            int num140 = Game_Server.Generic.random(0, 5);
                            int days131 = 1;
                            string itemcode133 = (string)null;
                            switch (num140)
                            {
                                case 0:
                                    itemcode133 = "DE68";
                                    days131 = 30;
                                    break;
                                case 1:
                                    itemcode133 = "DE68";
                                    days131 = -1;
                                    break;
                                case 2:
                                    itemcode133 = "DF95";
                                    days131 = 7;
                                    break;
                                case 3:
                                    itemcode133 = "CZ84";
                                    days131 = 1;
                                    break;
                                case 4:
                                    itemcode133 = "CB09";
                                    days131 = 1;
                                    break;
                                case 5:
                                    itemcode133 = "CZ81";
                                    num3 = 1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode133, days131);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode133, days131));
                            return;
                        case "CZ04":
                            int num141 = Game_Server.Generic.random(0, 4);
                            int days132 = 1;
                            string itemcode134 = (string)null;
                            switch (num141)
                            {
                                case 0:
                                    itemcode134 = "DC67";
                                    days132 = 30;
                                    break;
                                case 1:
                                    itemcode134 = "DF42";
                                    days132 = 30;
                                    break;
                                case 2:
                                    itemcode134 = "DC78";
                                    days132 = 30;
                                    break;
                                case 3:
                                    itemcode134 = "DE49";
                                    days132 = 30;
                                    break;
                                case 4:
                                    itemcode134 = "DF79";
                                    days132 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode134, days132);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode134, days132));
                            return;
                        case "CZ06":
                            int num142 = Game_Server.Generic.random(0, 4);
                            int days133 = 1;
                            string itemcode135 = (string)null;
                            switch (num142)
                            {
                                case 0:
                                    itemcode135 = "DE67";
                                    days133 = 30;
                                    break;
                                case 1:
                                    itemcode135 = "D604";
                                    days133 = -1;
                                    break;
                                case 2:
                                    itemcode135 = "DF65";
                                    days133 = 30;
                                    break;
                                case 3:
                                    itemcode135 = "DF49";
                                    days133 = -1;
                                    break;
                                case 4:
                                    itemcode135 = "DF67";
                                    days133 = 30;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode135, days133);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode135, days133));
                            return;
                        case "CZ52":
                            int num143 = Game_Server.Generic.random(0, 5);
                            int days134 = 1;
                            string itemcode136 = (string)null;
                            switch (num143)
                            {
                                case 0:
                                    itemcode136 = "D701";
                                    days134 = -1;
                                    break;
                                case 1:
                                    itemcode136 = "D701";
                                    days134 = 15;
                                    break;
                                case 2:
                                    itemcode136 = "D701";
                                    days134 = 30;
                                    break;
                                case 3:
                                    itemcode136 = "CF01";
                                    days134 = 15;
                                    break;
                                case 4:
                                    itemcode136 = "CIO1";
                                    break;
                                case 5:
                                    itemcode136 = "DS03";
                                    days134 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode136, days134);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode136, days134));
                            return;
                        case "CZ55":
                            int num144 = Game_Server.Generic.random(0, 5);
                            int days135 = 1;
                            string itemcode137 = (string)null;
                            switch (num144)
                            {
                                case 0:
                                    itemcode137 = "D602";
                                    days135 = -1;
                                    break;
                                case 1:
                                    itemcode137 = "D602";
                                    days135 = 15;
                                    break;
                                case 2:
                                    itemcode137 = "D602";
                                    days135 = 30;
                                    break;
                                case 3:
                                    itemcode137 = "DS03";
                                    days135 = 15;
                                    break;
                                case 4:
                                    itemcode137 = "CA01";
                                    days135 = 15;
                                    break;
                                case 5:
                                    itemcode137 = "DS10";
                                    days135 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode137, days135);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode137, days135));
                            return;
                        case "CZ59":
                            int num145 = Game_Server.Generic.random(0, 5);
                            int days136 = 1;
                            string itemcode138 = (string)null;
                            switch (num145)
                            {
                                case 0:
                                    itemcode138 = "DF40";
                                    days136 = -1;
                                    break;
                                case 1:
                                    itemcode138 = "DF40";
                                    days136 = 15;
                                    break;
                                case 2:
                                    itemcode138 = "DF40";
                                    days136 = 30;
                                    break;
                                case 3:
                                    itemcode138 = "CF01";
                                    days136 = 15;
                                    break;
                                case 4:
                                    itemcode138 = "CI01";
                                    days136 = 15;
                                    break;
                                case 5:
                                    itemcode138 = "CA01";
                                    days136 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode138, days136);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode138, days136));
                            return;
                        case "CZ60":
                            int num146 = Game_Server.Generic.random(0, 6);
                            int days137 = 1;
                            string itemcode139 = (string)null;
                            switch (num146)
                            {
                                case 0:
                                    itemcode139 = "DI04";
                                    days137 = 5;
                                    break;
                                case 1:
                                    itemcode139 = "DI04";
                                    days137 = 10;
                                    break;
                                case 2:
                                    itemcode139 = "DB24";
                                    days137 = 30;
                                    break;
                                case 3:
                                    itemcode139 = "CF01";
                                    days137 = 15;
                                    break;
                                case 4:
                                    itemcode139 = "CIO1";
                                    days137 = 15;
                                    break;
                                case 5:
                                    itemcode139 = "CA01";
                                    days137 = 15;
                                    break;
                                case 6:
                                    itemcode139 = "DI09";
                                    days137 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode139, days137);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode139, days137));
                            return;
                        case "CZ63":
                            int num147 = Game_Server.Generic.random(0, 5);
                            int days138 = 1;
                            string itemcode140 = (string)null;
                            switch (num147)
                            {
                                case 0:
                                    itemcode140 = "DC24";
                                    days138 = -1;
                                    break;
                                case 1:
                                    itemcode140 = "DC24";
                                    days138 = 15;
                                    break;
                                case 2:
                                    itemcode140 = "DC24";
                                    days138 = 30;
                                    break;
                                case 3:
                                    itemcode140 = "CF01";
                                    days138 = 15;
                                    break;
                                case 4:
                                    itemcode140 = "CIO1";
                                    days138 = 15;
                                    break;
                                case 5:
                                    itemcode140 = "CA01";
                                    days138 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode140, days138);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode140, days138));
                            return;
                        case "CZ64":
                            int num148 = Game_Server.Generic.random(0, 5);
                            int days139 = 1;
                            string itemcode141 = (string)null;
                            switch (num148)
                            {
                                case 0:
                                    itemcode141 = "DC62";
                                    days139 = -1;
                                    break;
                                case 1:
                                    itemcode141 = "DC62";
                                    days139 = 15;
                                    break;
                                case 2:
                                    itemcode141 = "DC62";
                                    days139 = 30;
                                    break;
                                case 3:
                                    itemcode141 = "CF01";
                                    days139 = 15;
                                    break;
                                case 4:
                                    itemcode141 = "CIO1";
                                    days139 = 15;
                                    break;
                                case 5:
                                    itemcode141 = "CA01";
                                    days139 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode141, days139);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode141, days139));
                            return;
                        case "CZ65":
                            int num149 = Game_Server.Generic.random(0, 5);
                            int days140 = 1;
                            string itemcode142 = (string)null;
                            switch (num149)
                            {
                                case 0:
                                    itemcode142 = "DC65";
                                    days140 = -1;
                                    break;
                                case 1:
                                    itemcode142 = "DC65";
                                    days140 = 15;
                                    break;
                                case 2:
                                    itemcode142 = "DC65";
                                    days140 = 30;
                                    break;
                                case 3:
                                    itemcode142 = "CF01";
                                    days140 = 15;
                                    break;
                                case 4:
                                    itemcode142 = "CIO1";
                                    days140 = 15;
                                    break;
                                case 5:
                                    itemcode142 = "CA01";
                                    days140 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode142, days140);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode142, days140));
                            return;
                        case "CZ66":
                            int num150 = Game_Server.Generic.random(0, 5);
                            int days141 = 1;
                            string itemcode143 = (string)null;
                            switch (num150)
                            {
                                case 0:
                                    itemcode143 = "DC22";
                                    days141 = -1;
                                    break;
                                case 1:
                                    itemcode143 = "DC22";
                                    days141 = 15;
                                    break;
                                case 2:
                                    itemcode143 = "DC22";
                                    days141 = 30;
                                    break;
                                case 3:
                                    itemcode143 = "CF01";
                                    days141 = 15;
                                    break;
                                case 4:
                                    itemcode143 = "CIO1";
                                    days141 = 15;
                                    break;
                                case 5:
                                    itemcode143 = "CA01";
                                    days141 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode143, days141);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode143, days141));
                            return;
                        case "CZ67":
                            int num151 = Game_Server.Generic.random(0, 5);
                            int days142 = 1;
                            string itemcode144 = (string)null;
                            switch (num151)
                            {
                                case 0:
                                    itemcode144 = "DC04";
                                    days142 = -1;
                                    break;
                                case 1:
                                    itemcode144 = "DC04";
                                    days142 = 30;
                                    break;
                                case 2:
                                    itemcode144 = "DC04";
                                    days142 = 15;
                                    break;
                                case 3:
                                    itemcode144 = "CI01";
                                    days142 = 15;
                                    break;
                                case 4:
                                    itemcode144 = "CF01";
                                    days142 = 15;
                                    break;
                                case 5:
                                    itemcode144 = "CA01";
                                    days142 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode144, days142);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode144, days142));
                            return;
                        case "CZ68":
                            int num152 = Game_Server.Generic.random(0, 5);
                            int days143 = 1;
                            string itemcode145 = (string)null;
                            switch (num152)
                            {
                                case 0:
                                    itemcode145 = "DC16";
                                    days143 = -1;
                                    break;
                                case 1:
                                    itemcode145 = "DC04";
                                    days143 = 15;
                                    break;
                                case 2:
                                    itemcode145 = "DC04";
                                    days143 = 30;
                                    break;
                                case 3:
                                    itemcode145 = "CF01";
                                    days143 = 15;
                                    break;
                                case 4:
                                    itemcode145 = "CIO1";
                                    days143 = 15;
                                    break;
                                case 5:
                                    itemcode145 = "CA01";
                                    days143 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode145, days143);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode145, days143));
                            return;
                        case "CZ69":
                            int num153 = Game_Server.Generic.random(0, 5);
                            int days144 = 1;
                            string itemcode146 = (string)null;
                            switch (num153)
                            {
                                case 0:
                                    itemcode146 = "DC67";
                                    days144 = -1;
                                    break;
                                case 1:
                                    itemcode146 = "DC04";
                                    days144 = 15;
                                    break;
                                case 2:
                                    itemcode146 = "DC04";
                                    days144 = 30;
                                    break;
                                case 3:
                                    itemcode146 = "CF01";
                                    days144 = 15;
                                    break;
                                case 4:
                                    itemcode146 = "CI01";
                                    days144 = 15;
                                    break;
                                case 5:
                                    itemcode146 = "CA01";
                                    days144 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode146, days144);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode146, days144));
                            return;
                        case "CZ70":
                            int num154 = Game_Server.Generic.random(0, 5);
                            int days145 = 1;
                            string itemcode147 = (string)null;
                            switch (num154)
                            {
                                case 0:
                                    itemcode147 = "DC64";
                                    days145 = -1;
                                    break;
                                case 1:
                                    itemcode147 = "DC04";
                                    days145 = 15;
                                    break;
                                case 2:
                                    itemcode147 = "DC04";
                                    days145 = 30;
                                    break;
                                case 3:
                                    itemcode147 = "CF01";
                                    days145 = 10;
                                    break;
                                case 4:
                                    itemcode147 = "CI01";
                                    days145 = 15;
                                    break;
                                case 5:
                                    itemcode147 = "CA01";
                                    days145 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode147, days145);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode147, days145));
                            return;
                        case "CZ73":
                            string block3 = this.getBlock(5);
                            Inventory.DecreaseEAItem(usr, block1);
                            string[] strArray1 = block3.Split(' ');
                            for (int index9 = 0; index9 < strArray1.Length; ++index9)
                                strArray1[index9] = WordFilterManager.Replace(strArray1[index9]);
                            string Message = string.Join(" ", strArray1);
                            usr.send((Packet)new SP_CashItemUse(usr, "CB03"));
                            usr.AddAdminCPLog(usr.nickname + " sent message " + Message.Replace('\x001D', ' ') + " [HAM_RADIO]");
                            UserManager.sendToServer((Packet)new SP_Chat(usr.nickname, SP_Chat.ChatType.Notice1, Message, 0U, usr.nickname));
                            return;
                        case "CZ74":
                            int num155 = Game_Server.Generic.random(0, 5);
                            int days146 = 1;
                            string itemcode148 = (string)null;
                            switch (num155)
                            {
                                case 0:
                                    itemcode148 = "DC68";
                                    days146 = -1;
                                    break;
                                case 1:
                                    itemcode148 = "DC76";
                                    days146 = 15;
                                    break;
                                case 2:
                                    itemcode148 = "DC76";
                                    days146 = 30;
                                    break;
                                case 3:
                                    itemcode148 = "CF01";
                                    days146 = 10;
                                    break;
                                case 4:
                                    itemcode148 = "CI01";
                                    days146 = 15;
                                    break;
                                case 5:
                                    itemcode148 = "CA01";
                                    days146 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode148, days146);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode148, days146));
                            return;
                        case "CZ76":
                            int num156 = Game_Server.Generic.random(0, 5);
                            int days147 = 1;
                            string itemcode149 = (string)null;
                            switch (num156)
                            {
                                case 0:
                                    itemcode149 = "DC39";
                                    days147 = -1;
                                    break;
                                case 1:
                                    itemcode149 = "DC39";
                                    days147 = 15;
                                    break;
                                case 2:
                                    itemcode149 = "DC39";
                                    days147 = 30;
                                    break;
                                case 3:
                                    itemcode149 = "CF01";
                                    days147 = 10;
                                    break;
                                case 4:
                                    itemcode149 = "CI01";
                                    days147 = 15;
                                    break;
                                case 5:
                                    itemcode149 = "CA01";
                                    days147 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode149, days147);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode149, days147));
                            return;
                        case "CZ77":
                            int num157 = Game_Server.Generic.random(0, 5);
                            int days148 = 1;
                            string itemcode150 = (string)null;
                            switch (num157)
                            {
                                case 0:
                                    itemcode150 = "DI12";
                                    days148 = -1;
                                    break;
                                case 1:
                                    itemcode150 = "DI11";
                                    days148 = 15;
                                    break;
                                case 2:
                                    itemcode150 = "DI11";
                                    days148 = 30;
                                    break;
                                case 3:
                                    itemcode150 = "CF01";
                                    days148 = 10;
                                    break;
                                case 4:
                                    itemcode150 = "CI01";
                                    days148 = 15;
                                    break;
                                case 5:
                                    itemcode150 = "CA01";
                                    days148 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode150, days148);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode150, days148));
                            return;
                        case "CZ78":
                            int num158 = Game_Server.Generic.random(0, 5);
                            int days149 = 1;
                            string itemcode151 = (string)null;
                            string str9;
                            switch (num158)
                            {
                                case 0:
                                    itemcode151 = "DF24";
                                    days149 = -1;
                                    break;
                                case 1:
                                    itemcode151 = "DF23";
                                    days149 = 15;
                                    break;
                                case 2:
                                    itemcode151 = "DF23";
                                    days149 = 30;
                                    break;
                                case 3:
                                    str9 = "CF01";
                                    days149 = 10;
                                    break;
                                case 4:
                                    str9 = "CI01";
                                    days149 = 15;
                                    break;
                                case 5:
                                    str9 = "CA01";
                                    days149 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode151, days149);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode151, days149));
                            return;
                        case "CZ81":
                            if (!usr.HasItem("CB08"))
                            {
                                usr.send((Packet)new SP_CashItemUse(SP_CashItemUse.ErrCode.NeedSupplyBox, usr, "CB08"));
                                return;
                            }
                            int num159 = Game_Server.Generic.random(0, 5);
                            string itemcode152 = (string)null;
                            switch (num159)
                            {
                                case 0:
                                    itemcode152 = "DB40";
                                    break;
                                case 1:
                                    itemcode152 = "DA74";
                                    break;
                                case 2:
                                    itemcode152 = "DJ93";
                                    break;
                                case 3:
                                    itemcode152 = "DG39";
                                    break;
                                case 4:
                                    itemcode152 = "DG63";
                                    break;
                                case 5:
                                    itemcode152 = "DC48";
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode152, 7);
                            Inventory.DecreaseEAItem(usr, "CZ81");
                            Inventory.DecreaseEAItem(usr, "CB08");
                            usr.send((Packet)new SP_WinItem(usr, itemcode152, 7));
                            return;
                        case "CZ90":
                            int num160 = Game_Server.Generic.random(0, 5);
                            int days150 = 1;
                            string itemcode153 = (string)null;
                            switch (num160)
                            {
                                case 0:
                                    itemcode153 = "DE13";
                                    days150 = -1;
                                    break;
                                case 1:
                                    itemcode153 = "DE11";
                                    days150 = 30;
                                    break;
                                case 2:
                                    itemcode153 = "DE11";
                                    days150 = 15;
                                    break;
                                case 3:
                                    itemcode153 = "CF01";
                                    days150 = 15;
                                    break;
                                case 4:
                                    itemcode153 = "CI01";
                                    days150 = 15;
                                    break;
                                case 5:
                                    itemcode153 = "CA01";
                                    days150 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode153, days150);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode153, days150));
                            return;
                        case "CZ91":
                            int num161 = Game_Server.Generic.random(0, 5);
                            int days151 = 1;
                            string itemcode154 = (string)null;
                            switch (num161)
                            {
                                case 0:
                                    itemcode154 = "DD05";
                                    days151 = -1;
                                    break;
                                case 1:
                                    itemcode154 = "DD05";
                                    days151 = 30;
                                    break;
                                case 2:
                                    itemcode154 = "DD05";
                                    days151 = 15;
                                    break;
                                case 3:
                                    itemcode154 = "CF01";
                                    days151 = 15;
                                    break;
                                case 4:
                                    itemcode154 = "CI01";
                                    days151 = 15;
                                    break;
                                case 5:
                                    itemcode154 = "CA01";
                                    days151 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode154, days151);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode154, days151));
                            return;
                        case "CZ93":
                            int num162 = Game_Server.Generic.random(0, 6);
                            int days152 = 1;
                            string itemcode155 = (string)null;
                            switch (num162)
                            {
                                case 0:
                                    itemcode155 = "DG39";
                                    days152 = -1;
                                    break;
                                case 1:
                                    itemcode155 = "DG39";
                                    days152 = 5;
                                    break;
                                case 2:
                                    itemcode155 = "DG38";
                                    days152 = 30;
                                    break;
                                case 3:
                                    itemcode155 = "DG38";
                                    days152 = 10;
                                    break;
                                case 4:
                                    itemcode155 = "CF01";
                                    days152 = 15;
                                    break;
                                case 5:
                                    itemcode155 = "CI01";
                                    days152 = 15;
                                    break;
                                case 6:
                                    itemcode155 = "CA01";
                                    days152 = 15;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode155, days152);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode155, days152));
                            return;
                        case "CZ94":
                            int num163 = Game_Server.Generic.random(0, 6);
                            int days153 = 1;
                            string itemcode156 = (string)null;
                            switch (num163)
                            {
                                case 0:
                                    itemcode156 = "DB22";
                                    days153 = 5;
                                    break;
                                case 1:
                                    itemcode156 = "DB24";
                                    days153 = 10;
                                    break;
                                case 2:
                                    itemcode156 = "DB24";
                                    days153 = 30;
                                    break;
                                case 3:
                                    itemcode156 = "CF01";
                                    days153 = 15;
                                    break;
                                case 4:
                                    itemcode156 = "CIO1";
                                    days153 = 15;
                                    break;
                                case 5:
                                    itemcode156 = "CA01";
                                    days153 = 15;
                                    break;
                                case 6:
                                    itemcode156 = "DB22";
                                    days153 = -1;
                                    break;
                            }
                            Inventory.AddItem(usr, itemcode156, days153);
                            Inventory.DecreaseEAItem(usr, block1);
                            usr.send((Packet)new SP_WinItem(usr, itemcode156, days153));
                            return;
                        case "CZ99":
                            if (Inventory.GetFreeItemSlotCount(usr) <= 1)
                                return;
                            string[] items3 = Game_Server.Configs.Server.RandomBoxEvent.items;
                            int index10 = Game_Server.Generic.random(0, items3.Length - 1);
                            string str10 = items3[index10];
                            if (ItemManager.GetItem(str10) != null)
                            {
                                int days5 = new Random().Next(Game_Server.Configs.Server.RandomBoxEvent.MinDays, Game_Server.Configs.Server.RandomBoxEvent.MaxDays);
                                if (str10.ToUpper().StartsWith("B"))
                                    Inventory.AddCostume(usr, str10, days5);
                                else
                                    Inventory.AddItem(usr, str10, days5);
                                Inventory.DecreaseEAItem(usr, block1);
                                usr.send((Packet)new SP_WinItem(usr, str10, days5));
                                return;
                            }
                            Log.WriteError(str10 + " is not a valid item @ random box event!");
                            return;
                        default:
                            if (str1 == null)
                                ;
                            return;
                    }
                case CP_CashItemBuy.SubCodes.Storage:
                    int action = int.Parse(this.getBlock(1));
                    switch (action)
                    {
                        case 2:
                            string block4 = this.getBlock(3);
                            int index11 = int.Parse(this.getBlock(2));
                            if (!(usr.inventory[index11].Split('-')[0] == block4))
                                return;
                            int index12 = Array.IndexOf<string>(usr.storageInventory, "^");
                            if (index12 >= 0 && index12 < usr.storageInventoryMax)
                            {
                                usr.storageInventory[index12] = usr.inventory[index11];
                                usr.inventory[index11] = "^";
                                string inventory = Inventory.calculateInventory(index11);
                                for (int index9 = 0; index9 < 5; ++index9)
                                {
                                    for (int index13 = 0; index13 < 8; ++index13)
                                    {
                                        if (usr.equipment[index9, index13] == "I" + inventory || usr.equipment[index9, index13] == block4)
                                            usr.equipment[index9, index13] = "^";
                                        else if (usr.equipment[index9, index13].Contains("-"))
                                        {
                                            string[] strArray2 = usr.equipment[index9, index13].Split('-');
                                            if (strArray2[0] == "I" + inventory)
                                                usr.equipment[index9, index13] = strArray2[1];
                                            else if (strArray2[1] == "I" + inventory)
                                                usr.equipment[index9, index13] = strArray2[0];
                                        }
                                    }
                                }
                                usr.SaveEquipment();
                                usr.send((Packet)new SP_StorageInventoryUpdate(usr, action, index11, block4));
                                usr.send((Packet)new SP_UpdateInventory(usr, (List<string>)null));
                                DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId.ToString() + "'");
                            }
                            else
                                usr.send((Packet)new SP_StorageInventoryUpdate(SP_StorageInventoryUpdate.ErrorCode.NoStorageFreeSpace));
                            return;
                        case 3:
                            string block5 = this.getBlock(3);
                            int index14 = int.Parse(this.getBlock(2));
                            int warRockDateTime1 = Game_Server.Generic.WarRockDateTime;
                            string[] strArray3 = usr.storageInventory[index14].Split('-');
                            if (!(strArray3[0] == block5))
                                return;
                            if (int.Parse(strArray3[3]) > warRockDateTime1)
                            {
                                if (usr.HasItem(block5))
                                {
                                    usr.storageInventory[index14] = "^";
                                    int itemIndex = usr.GetItemIndex(block5);
                                    TimeSpan timeSpan = DateTime.ParseExact(usr.inventory[itemIndex].Split('-')[3], "yyMMddHH", (IFormatProvider)null) - DateTime.Now;
                                    Inventory.AddItem(usr, block5, (int)timeSpan.TotalDays);
                                    usr.send((Packet)new SP_StorageInventoryUpdate(usr, action, index14, block5));
                                }
                                else
                                {
                                    int index9 = Array.IndexOf<string>(usr.inventory, "^");
                                    if (index9 >= 0)
                                    {
                                        usr.inventory[index9] = usr.storageInventory[index14];
                                        usr.storageInventory[index14] = "^";
                                        usr.send((Packet)new SP_StorageInventoryUpdate(usr, action, index14, block5));
                                    }
                                    else
                                        usr.send((Packet)new SP_StorageInventoryUpdate(SP_StorageInventoryUpdate.ErrorCode.NoInventoryFreeSpace));
                                }
                            }
                            else
                            {
                                usr.storageInventory[index14] = "^";
                                usr.send((Packet)new SP_StorageInventoryList(usr));
                            }
                            DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId.ToString() + "'");
                            return;
                        case 4:
                            int warRockDateTime2 = Game_Server.Generic.WarRockDateTime;
                            for (int index9 = 0; index9 < usr.storageInventory.Length; ++index9)
                            {
                                try
                                {
                                    string str11 = usr.storageInventory[index9];
                                    if (str11 != "^")
                                    {
                                        if (int.Parse(str11.Split('-')[3]) < warRockDateTime2)
                                            usr.storageInventory[index9] = "^";
                                    }
                                }
                                catch
                                {
                                }
                            }
                            usr.send((Packet)new SP_StorageInventoryList(usr));
                            DB.RunQuery("UPDATE equipment SET inventory = '" + Inventory.Itemlist(usr) + "', storage = '" + Inventory.Storage(usr) + "' WHERE ownerid = '" + usr.userId.ToString() + "'");
                            return;
                        default:
                            return;
                    }
            }
        }

        internal enum SubCodes
        {
            OnItemBuy = 1110, // 0x00000456
            OnItemUse = 1111, // 0x00000457
            OnItemShopOpen = 1113, // 0x00000459
            Storage = 1400, // 0x00000578
        }
    }
}
