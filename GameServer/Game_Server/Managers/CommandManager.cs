// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.CommandManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System.Threading;

namespace Game_Server.Managers
{
  internal class CommandManager
  {
    private static Thread consoleThread;

    public static void Load()
    {
      CommandManager.consoleThread = new Thread(new ThreadStart(CommandManager.ConsoleCommand));
      CommandManager.consoleThread.Priority = ThreadPriority.Lowest;
      CommandManager.consoleThread.Start();
    }

    public static void ConsoleCommand()
    {
      while (true)
      {
        try
        {
          string str = System.Console.ReadLine();
          switch (str.Split(' ')[0])
          {
            case "notice":
              UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, str.Substring(7), 999U, "NULL"));
              Log.WriteLine("Successfully notice: " + str.Substring(7));
              break;
            case "stop":
              Log.WriteLine("Server is going to be shutdown!");
              UserManager.sendToServer((Packet) new SP_Chat("NOTICE", SP_Chat.ChatType.Notice1, "Server is going to be restarted, sorry!!!", 999U, "NULL"));
              UserManager.sendToServer((Packet) new SP_Chat(Game_Server.Configs.Server.SystemName, SP_Chat.ChatType.Room_ToAll, Game_Server.Configs.Server.SystemName + " >> Server is going to be restarted, sorry!!!", 999U, "Server"));
              Thread.Sleep(500);
              Program.shutDown();
              break;
          }
        }
        catch
        {
        }
        Thread.Sleep(2000);
      }
    }
  }
}
