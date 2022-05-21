// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.PackageManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;

namespace Game_Server.Managers
{
    internal class PackageManager
    {
        public static bool AddItem(User usr, string itemCode)
        {
            if (itemCode == "CC36" || itemCode == "CC37" || (itemCode == "CC56" || itemCode == "CC57"))
                return false;
            string[] array = new string[1] { "CZ99" };
            Item obj = ItemManager.GetItem(itemCode);
            if (obj != null)
            {
                switch (itemCode)
                {
                    case "CC41": Inventory.AddItem(usr, "CR13", 5000); usr.AddPremium(3, 30); break;
                    case "CC44": usr.AddPremium(2, 30); break;
                    case "CU83": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CZ23": Inventory.AddItem(usr, "CR99", 5000); Inventory.AddItem(usr, "CR99", 5000); Inventory.AddItem(usr, "CR99", 5000); usr.AddPremium(3, 30); break;
                    case "CZ13": Inventory.AddItem(usr, "CZ66", 5000); Inventory.AddItem(usr, "CZ66", 5000); break;
                    case "CU81": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CU82": Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); Inventory.AddItem(usr, "CY46", 5000); break;
                    case "CU87": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU84": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU90": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CV06": Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); Inventory.AddItem(usr, "CY77", 5000); break;
                    case "CV05": Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); Inventory.AddItem(usr, "CY76", 5000); break;
                    case "CV04": Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); Inventory.AddItem(usr, "CY75", 5000); break;
                    case "CV03": Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); Inventory.AddItem(usr, "CY74", 5000); break;
                    case "CU88": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU85": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU91": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CU89": Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); Inventory.AddItem(usr, "CY48", 5000); break;
                    case "CU86": Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); Inventory.AddItem(usr, "CY47", 5000); break;
                    case "CU92": Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); Inventory.AddItem(usr, "CY49", 5000); break;
                    case "CC42": Inventory.AddItem(usr, "CR13", 5000); usr.AddPremium(3, 90); break;
                    case "CC43": Inventory.AddItem(usr, "CR13", 5000); usr.AddPremium(3, 180); Inventory.DecreaseEAItem(usr, "CC43", 1); break;
                    case "CC74": Inventory.AddItem(usr, "CR13", 5000); usr.AddPremium(3, 7); break;
                    case "CC75": Inventory.AddItem(usr, "CR13", 5000); usr.AddPremium(3, 15); break;
                    case "CC73": usr.AddPremium(3, 3); break;
                    case "CU08": usr.AddPremium(3, 30); break;
                    case "CS14": usr.AddPremium(3, 30); break;
                    case "CS11": usr.AddPremium(3, 30); break;
                    case "CS09": usr.AddPremium(3, 30); break;
                    case "CS10": usr.AddPremium(3, 30); break;
                    case "CS08": usr.AddPremium(3, 30); break;
                    case "CS93": usr.AddPremium(3, 30); break;
                    case "CC45": usr.AddPremium(2, 90); break;
                    case "CC46": usr.AddPremium(2, 180); break;
                    case "CC60": usr.AddPremium(4, 30); break;
                    case "CC61": usr.AddPremium(4, 90); break;
                    case "CC62": usr.AddPremium(4, 180); break;
                    case "CQ36": usr.AddPremium(3, 30); break;
                    case "CP25": usr.AddPremium(3, 30); break;
                    case "CP36": usr.AddPremium(3, 30); break;
                    case "CP37": usr.AddPremium(3, 30); break;
                    case "CU44": usr.AddPremium(3, 30); break;
                    case "CU43": usr.AddPremium(3, 30); break;
                    case "CU42": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CZ81", 5000); break;
                    case "CU41": usr.AddPremium(3, 30); break;
                    case "CS43": usr.AddPremium(3, 30); break;
                    case "CS42": usr.AddPremium(3, 30); break;
                    case "CS40": usr.AddPremium(3, 30); break;
                    case "CQ46": usr.AddPremium(3, 30); break;
                    case "CQ45": usr.AddPremium(3, 30); break;
                    case "CQ56": usr.AddPremium(3, 30); break;
                    case "CU56": usr.AddPremium(3, 30); break;
                    case "CU55": usr.AddPremium(3, 30); break;
                    case "CU52": usr.AddPremium(3, 30); break;
                    case "CU51": usr.AddPremium(3, 30); break;
                    case "CS59": usr.AddPremium(3, 30); break;
                    case "CS58": usr.AddPremium(3, 30); break;
                    case "CS57": usr.AddPremium(3, 30); break;
                    case "CS56": usr.AddPremium(3, 30); break;
                    case "CS55": usr.AddPremium(3, 30); break;
                    case "CS54": usr.AddPremium(3, 30); break;
                    case "CQ55": usr.AddPremium(3, 30); break;
                    case "CQ53": usr.AddPremium(3, 30); break;
                    case "CQ52": usr.AddPremium(3, 30); break;
                    case "CQ51": usr.AddPremium(3, 30); break;
                    case "CQ50": usr.AddPremium(3, 30); break;
                    case "CU69": usr.AddPremium(3, 30); break;
                    case "CU67": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); break;
                    case "CU66": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ85", 5000); Inventory.AddItem(usr, "CZ84", 5000); Inventory.AddItem(usr, "CZ83", 5000); Inventory.AddItem(usr, "CZ75", 5000); break;
                    case "CU64": usr.AddPremium(3, 30); break;
                    case "CU63": usr.AddPremium(3, 30); break;
                    case "CU61": usr.AddPremium(3, 30); break;
                    case "CU60": usr.AddPremium(3, 30); break;
                    case "CP60": usr.AddPremium(3, 30); break;
                    case "CP61": usr.AddPremium(3, 30); break;
                    case "CQ64": usr.AddPremium(3, 30); break;
                    case "CP62": usr.AddPremium(3, 30); break;
                    case "CP63": usr.AddPremium(3, 30); break;
                    case "CU72": usr.AddPremium(3, 30); break;
                    case "CU71": usr.AddPremium(3, 30); break;
                    case "CQ79": usr.AddPremium(3, 30); break;
                    case "CQ78": usr.AddPremium(3, 30); break;
                    case "CQ71": usr.AddPremium(3, 30); break;
                    case "CQ70": usr.AddPremium(3, 30); break;
                    case "CS89": usr.AddPremium(3, 30); break;
                    case "CQ89": usr.AddPremium(3, 30); break;
                    case "CQ88": usr.AddPremium(3, 30); break;
                    case "CQ85": usr.AddPremium(3, 30); break;
                    case "CQ84": usr.AddPremium(3, 30); break;
                    case "CP81": usr.AddPremium(3, 30); break;
                    case "CP82": usr.AddPremium(3, 30); break;
                    case "CP83": usr.AddPremium(3, 30); break;
                    case "CP85": usr.AddPremium(3, 30); break;
                    case "CP88": usr.AddPremium(3, 30); break;
                    case "CP89": usr.AddPremium(3, 30); break;
                    case "CU99": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CR89", 5000); break;
                    case "CU98": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CR89", 5000); break;
                    case "CU97": usr.AddPremium(3, 30); break;
                    case "CU95": usr.AddPremium(3, 30); break;
                    case "CU94": usr.AddPremium(3, 30); break;
                    case "CU93": usr.AddPremium(3, 30); break;
                    case "CS90": usr.AddPremium(3, 30); break;
                    case "CS91": usr.AddPremium(3, 30); break;
                    case "CQ99": usr.AddPremium(3, 30); break;
                    case "CQ98": usr.AddPremium(3, 30); break;
                    case "CQ97": usr.AddPremium(3, 30); break;
                    case "CQ96": usr.AddPremium(3, 30); break;
                    case "CQ95": usr.AddPremium(3, 30); break;
                    case "CP98": usr.AddPremium(3, 30); break;
                    case "CP99": usr.AddPremium(3, 30); break;
                    case "CP90": usr.AddPremium(3, 30); break;
                    case "CP97": usr.AddPremium(3, 30); break;
                    case "CP32": usr.AddPremium(3, 30); break;
                    case "CP33": usr.AddPremium(3, 30); break;
                    case "CC27": usr.AddPremium(3, 30); break;
                    case "CC28": usr.AddPremium(3, 30); break;
                    case "CC29": usr.AddPremium(3, 30); break;
                    case "CC30": usr.AddPremium(3, 30); break;
                    case "CC52": usr.AddPremium(3, 30); break;
                    case "CC53": usr.AddPremium(3, 30); break;
                    case "CC54": usr.AddPremium(3, 30); break;
                    case "CC55": usr.AddPremium(3, 30); break;
                    case "CP86": usr.AddPremium(3, 30); break;
                    case "CV09": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CY28", 5000); break;
                    case "CV08": usr.AddPremium(3, 30); break;
                    case "CV07": usr.AddPremium(3, 30); break;
                    case "CV02": usr.AddPremium(3, 30); break;
                    case "CV01": usr.AddPremium(3, 30); break;
                    case "CS04": usr.AddPremium(3, 30); break;
                    case "CS03": usr.AddPremium(3, 30); break;
                    case "CV10": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CY28", 5000); break;
                    case "CQ18": usr.AddPremium(3, 30); break;
                    case "CQ17": usr.AddPremium(3, 30); break;
                    case "CQ16": usr.AddPremium(3, 30); break;
                    case "CQ14": usr.AddPremium(3, 30); break;
                    case "CQ13": usr.AddPremium(3, 30); break;
                    case "CQ12": usr.AddPremium(3, 30); break;
                    case "CP13": usr.AddPremium(3, 30); break;
                    case "CP14": usr.AddPremium(3, 30); break;
                    case "CP15": usr.AddPremium(3, 30); break;
                    case "CZ22": usr.AddPremium(3, 30); break;
                    case "CS26": usr.AddPremium(3, 30); break;
                    case "CS25": usr.AddPremium(3, 30); break;
                    case "CS23": usr.AddPremium(3, 30); break;
                    case "CS22": usr.AddPremium(3, 30); break;
                    case "CS20": usr.AddPremium(3, 30); break;
                    case "CP23": usr.AddPremium(3, 30); break;
                    case "CP20": usr.AddPremium(3, 30); break;
                    case "CP26": usr.AddPremium(3, 30); break;
                    case "CS36": usr.AddPremium(3, 30); break;
                    case "CS35": usr.AddPremium(3, 30); break;
                    case "CS34": usr.AddPremium(3, 30); break;
                    case "CS30": usr.AddPremium(3, 30); break;
                    case "CQ39": usr.AddPremium(3, 30); break;
                    case "CQ38": usr.AddPremium(3, 30); break;
                    case "CQ35": usr.AddPremium(3, 30); break;
                    case "CQ34": usr.AddPremium(3, 30); break;
                    case "CP30": usr.AddPremium(3, 30); break;
                    case "CZ48": usr.AddPremium(3, 30); break;
                    case "CZ49": usr.AddPremium(3, 30); break;
                    case "CS39": usr.AddPremium(3, 30); break;
                    case "CU40": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CZ81", 50000); break;
                    case "CZ50": usr.AddPremium(3, 30); Inventory.AddItem(usr, "CZ67", 5000); break;
                    case "CP58": usr.AddPremium(3, 30); break;
                    case "CP59": usr.AddPremium(3, 30); break;
                    case "CQ02": usr.AddPremium(3, 30); break;
                    case "CQ03": usr.AddPremium(3, 30); break;
                    case "CQ91": usr.AddPremium(3, 30); break;
                    case "CU32": Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CK02", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CZ81", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); Inventory.AddItem(usr, "CB09", 5000); break;
                        //  case "CP27": Inventory.AddCostume(usr, "")
                }
                DB.RunQuery("UPDATE users SET premium =  '" + (object)usr.premium + "' WHERE id='" + (object)usr.userId + "'");

                if (obj.dinarReward > 0U)
                {
                    usr.dinar += (int)obj.dinarReward;
                    DB.RunQuery("UPDATE users SET dinar = '" + (object)usr.dinar + "' WHERE id='" + (object)usr.userId + "'");
                }
                if (obj.packageItems.Count > 0 && Array.IndexOf<string>(array, obj.Code) == -1 && (!itemCode.StartsWith("B") || Inventory.GetFreeCostumeSlotCount(usr) >= obj.packageItems.Count) && (itemCode.StartsWith("B") || Inventory.GetFreeItemSlotCount(usr) >= obj.packageItems.Count))
                {
                    foreach (PackageItem packageItem in obj.packageItems)
                    {
                        if (Inventory.GetFreeItemSlotCount(usr) > 0)
                        {
                            if (packageItem.item.StartsWith("B"))
                                Inventory.AddCostume(usr, packageItem.item, (int)packageItem.days);
                            else
                                Inventory.AddItem(usr, packageItem.item, (int)packageItem.days);
                        }
                        else
                            DB.RunQuery("INSERT INTO inbox (ownerid, itemcode, days) VALUES ('" + (object)usr.userId + "', '" + packageItem.item + "', '" + (object)packageItem.days + "')");
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
