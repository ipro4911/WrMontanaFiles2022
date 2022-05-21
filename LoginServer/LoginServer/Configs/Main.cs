// Decompiled with JetBrains decompiler
// Type: LoginServer.Configs.Main
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Managers;
using System;

namespace LoginServer.Configs
{
  internal class Main
  {
    public static void setup()
    {
      try
      {
        Patch.Load();
        CountryManager.Load();
        Log.WriteLine("Configs loaded successfully");
      }
      catch (Exception ex)
      {
        Log.WriteError("Couldn't setup configs (" + ex.Message + ") @ " + ex.StackTrace);
      }
    }
  }
}
