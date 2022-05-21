// Decompiled with JetBrains decompiler
// Type: LoginServer.Managers.CountryManager
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System.Collections.Generic;

namespace LoginServer.Managers
{
  internal class CountryManager
  {
    private static List<string> countries = new List<string>();

    public static void Load()
    {
      CountryManager.countries.Clear();
      string str1 = IO.ReadValue("Settings", "LockedCountries", true);
      char[] chArray = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.Length > 0)
        {
          if (str2.Length == 2)
            CountryManager.countries.Add(str2.Trim().ToUpper());
          else
            Log.WriteError(str2 + " is not a valid country");
        }
      }
    }

    public static bool IsLockedCountry(string countryCode)
    {
      return CountryManager.countries.Contains(countryCode.ToUpper());
    }
  }
}
