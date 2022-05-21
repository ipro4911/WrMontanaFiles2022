// Decompiled with JetBrains decompiler
// Type: LoginServer.Generic
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LoginServer
{
  internal class Generic
  {
    public static long timestamp
    {
      get
      {
        return (long) (uint) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
      }
    }

    public static string currentDate
    {
      get
      {
        return DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
      }
    }

    public static string runningSince
    {
      get
      {
        TimeSpan timeSpan = DateTime.Now - Process.GetCurrentProcess().StartTime;
        return "Running since " + (object) timeSpan.Days + " days, " + (object) timeSpan.Hours + " hours," + (object) timeSpan.Minutes + " minutes!";
      }
    }

    public static string convertToMD5(string Input)
    {
      try
      {
        byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(Input));
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < hash.Length; ++index)
          stringBuilder.Append(hash[index].ToString("x2"));
        return stringBuilder.ToString();
      }
      catch
      {
        return (string) null;
      }
    }

    public static bool isMacAddress(string mac)
    {
      try
      {
        return !new Regex("^([:xdigit:]){12}$").IsMatch(mac);
      }
      catch
      {
        return false;
      }
    }

    public static int getOnlinePlayers(int srvid)
    {
      return DB.runRead("SELECT * FROM users WHERE online='1' AND serverid='" + (object) srvid + "'").Rows.Count;
    }

    public static bool isAlphaNumeric(string input)
    {
      try
      {
        return !new Regex("[^a-zA-Z0-9]").IsMatch(input);
      }
      catch
      {
        return false;
      }
    }

    public static int ServerSlots(int slots)
    {
      int result = 0;
      int.TryParse(Math.Truncate((double) (2500 / slots)).ToString(), out result);
      return result;
    }
  }
}
