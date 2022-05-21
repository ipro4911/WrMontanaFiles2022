// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.ZombieData
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Managers
{
  internal class ZombieData
  {
    public int Points = 1;
    public int Damage = 150;
    public int Type;
    public string Name;
    public int Health;
    public bool SkillPoint;

    public ZombieData(
      int Type,
      string Name,
      int Health,
      int Points,
      int Damage,
      bool SkillPoint)
    {
      this.Health = Health;
      this.Name = Name;
      this.Points = Points;
      this.Damage = Damage;
      this.SkillPoint = SkillPoint;
      this.Type = Type;
    }
  }
}
