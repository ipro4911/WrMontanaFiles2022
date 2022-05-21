// Decompiled with JetBrains decompiler
// Type: LoginServer.Configs.Patch
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer.Configs
{
  internal class Patch
  {
    public static string Format;
    public static string Launcher;
    public static string Updater;
    public static string Client;
    public static string Sub;
    public static string Option;
    public static string UpdateUrl;

    public static void Load()
    {
      Patch.Format = IO.ReadValue("UpdaterInformation", "Format", true);
      Patch.Launcher = IO.ReadValue("UpdaterInformation", "Launcher", true);
      Patch.Updater = IO.ReadValue("UpdaterInformation", "Updater", true);
      Patch.Client = IO.ReadValue("UpdaterInformation", "Client", true);
      Patch.Sub = IO.ReadValue("UpdaterInformation", "Sub", true);
      Patch.Option = IO.ReadValue("UpdaterInformation", "Option", true);
      Patch.UpdateUrl = IO.ReadValue("UpdaterInformation", "UpdaterURL", true);
    }
  }
}
