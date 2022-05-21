// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.LevelCalculator
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Managers
{
  internal class LevelCalculator
  {
    private static uint[] EXPTable = new uint[101]
    {
      0U,
      2250U,
      6750U,
      11250U,
      16650U,
      24750U,
      32850U,
      41625U,
      50400U,
      59175U,
      67950U,
      76725U,
      94725U,
      112725U,
      130725U,
      148725U,
      166725U,
      189225U,
      211725U,
      234225U,
      256725U,
      279225U,
      306225U,
      333225U,
      360225U,
      387225U,
      414225U,
      441225U,
      497475U,
      553725U,
      609975U,
      666225U,
      722475U,
      778725U,
      857475U,
      936225U,
      1014975U,
      1093725U,
      1172475U,
      1251225U,
      1363725U,
      1476225U,
      1588725U,
      1701225U,
      1813725U,
      1926225U,
      2038725U,
      2207475U,
      2376225U,
      2544975U,
      2713725U,
      2882475U,
      3051225U,
      3219975U,
      3444975U,
      3669975U,
      3894975U,
      4119975U,
      4344975U,
      4569975U,
      4794975U,
      5132475U,
      5469975U,
      5807475U,
      6144975U,
      6482475U,
      6819975U,
      7157475U,
      7494975U,
      7944975U,
      8394975U,
      8844975U,
      9294975U,
      9744975U,
      10194975U,
      10644975U,
      11094975U,
      11657475U,
      12219975U,
      12782475U,
      13344975U,
      13907475U,
      14469975U,
      15032475U,
      15932475U,
      17282475U,
      18632475U,
      19982475U,
      21332475U,
      22682475U,
      24032475U,
      25382475U,
      26732475U,
      28307475U,
      29882475U,
      31457475U,
      33032475U,
      34607475U,
      36182475U,
      37757475U,
      (uint) int.MaxValue
    };

    public static byte getLevelforExp(int Exp)
    {
      byte num = 0;
      while ((long) Exp >= (long) LevelCalculator.EXPTable[(int) num])
      {
        ++num;
        if (num >= (byte) 101)
          break;
      }
      return num;
    }

    public static uint getExpForLevel(int Level)
    {
      if (Level > 0)
        return LevelCalculator.EXPTable[Level - 1];
      return 0;
    }
  }
}
