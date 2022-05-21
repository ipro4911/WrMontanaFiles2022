// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.ZombieManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;

namespace Game_Server.Managers
{
  internal class ZombieManager
  {
    private static Dictionary<int, ZombieData> Datas = new Dictionary<int, ZombieData>();

    public static void Load()
    {
      ZombieManager.Datas.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM zombies");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        int num1 = int.Parse(row["type"].ToString());
        string Name = row["name"].ToString();
        int Health = int.Parse(row["health"].ToString());
        int Points = int.Parse(row["points"].ToString());
        int Damage = int.Parse(row["damage"].ToString());
        int num2 = int.Parse(row["skillpoint"].ToString());
        ZombieData zombieData = new ZombieData(num1, Name, Health, Points, Damage, num2 > 0);
        if (!ZombieManager.Datas.ContainsKey(num1))
          ZombieManager.Datas.Add(num1, zombieData);
        else
          Log.WriteError("Duplicate Zombie Type [" + (object) num1 + "]");
      }
    }

    public static ZombieData GetZombieDataByType(int Type)
    {
      if (ZombieManager.Datas.ContainsKey(Type))
        return ZombieManager.Datas[Type];
      return (ZombieData) null;
    }

    public static void GetZombieData(Zombie Zombie)
    {
      ZombieData zombieDataByType = ZombieManager.GetZombieDataByType(Zombie.Type);
      if (zombieDataByType == null)
        return;
      Zombie.Name = zombieDataByType.Name;
      Zombie.Health = zombieDataByType.Health;
      Zombie.Points = zombieDataByType.Points;
      Zombie.Damage = zombieDataByType.Damage;
      Zombie.SkillPoint = zombieDataByType.SkillPoint;
    }
  }
}
