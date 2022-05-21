// Decompiled with JetBrains decompiler
// Type: LoginServer.Program
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Configs;
using LoginServer.Managers;
using LoginServer.Networking;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LoginServer
{
  internal class Program
  {
    public static bool running = false;
    public static LookupService ipLookup;

    private static void Main(string[] args)
    {
      System.Console.Title = LoginServer.Configs.Console.title;
      System.Console.WriteLine("  _______ ______   _______ _____ _____ _  _____      _____ ____  _____  ______ ");
      System.Console.WriteLine(" |__   __/ __ \\ \\ / |_   _|_   _/ ____( )/ ____|    / ____/ __ \\|  __ \\|  ____|");
      System.Console.WriteLine("    | | | |  | \\ V /  | |   | || |    |/| (___     | |   | |  | | |__) | |__   ");
      System.Console.WriteLine("    | | | |  | |> <   | |   | || |       \\___ \\    | |   | |  | |  _  /|  __|  ");
      System.Console.WriteLine("    | | | |__| / . \\ _| |_ _| || |____   ____) |   | |___| |__| | | \\ \\| |____ ");
      System.Console.WriteLine("    |_|  \\____/_/ \\_|_____|_____\\_____| |_____/     \\_____\\____/|_|  \\_|______|");
      System.Console.WriteLine("    |_|  \\____/_/ \\_|_____|_____\\_____| |_____/     \\_____\\____/|_|  \\_|______|");
      System.Console.WriteLine("|______________________________________________________________________________|");
      Log.WriteBlank(1);
      System.Console.WriteLine(" - Wrote by ToXiiC");
      System.Console.WriteLine(" - Thanks to CodeDragon");
      System.Console.WriteLine(" - Fucked by cAn!");
            Log.WriteBlank(2);
      if (System.Type.GetType("Mono.Runtime") != (System.Type) null)
        Log.WriteLine("This Login Server is running under Mono VM!");
      Program.running = Program.initializeStartup();
      System.Console.ReadKey(true);
      new AutoResetEvent(false).WaitOne();
    }

    private static bool initializeStartup()
    {
      string path = Application.StartupPath + "/loginserver.xml";
      if (!File.Exists(path))
      {
        Log.WriteError("Error: Cannot find loginserver.xml");
        System.Console.ReadKey();
        System.Console.ReadKey();
        return false;
      }
      string str = LoginServer.IO.workingDirectory + "/GeoIP.dat";
      if (!File.Exists(str))
      {
        Log.WriteError("Error: Cannot find GeoIP.dat");
        return false;
      }
      Program.ipLookup = new LookupService(str, LookupService.GEOIP_MEMORY_CACHE);
      LoginServer.IO.path = path;
   //   Program.initializeStartup();
      string dbHost = LoginServer.IO.ReadValue("Database", "host", true);
      int dbPort = int.Parse(LoginServer.IO.ReadValue("Database", "port", true));
      string dbUsername = LoginServer.IO.ReadValue("Database", "user", true);
      string dbPassword = LoginServer.IO.ReadValue("Database", "password", true);
      string dbName = LoginServer.IO.ReadValue("Database", "database", true);
      int dbPoolsize = int.Parse(LoginServer.IO.ReadValue("Database", "poolsize", true));
      DB.openConnection(dbHost, dbPort, dbName, dbUsername, dbPassword, dbPoolsize);
      ServersInformations.loadServers();
      Packet_Manager.setup();
      if (NetworkSocket.InitializeSocket(5330))
        return true;
      Log.WriteError("Error: Cannot Initialize a new socket on the port 5330");
      System.Console.ReadKey();
      System.Console.ReadKey();
      return false;
    }
  }
}
