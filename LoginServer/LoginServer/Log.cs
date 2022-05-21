// Decompiled with JetBrains decompiler
// Type: LoginServer.Log
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;

namespace LoginServer
{
  internal class Log
  {
    private static object writeObj = new object();

    public static void WriteLine(string str)
    {
      Log.writeline(str, ConsoleColor.DarkGreen);
    }

    public static void WriteError(string str)
    {
      Log.writeline(str, ConsoleColor.DarkRed);
    }

    public static void WriteDebug(string str)
    {
      Log.writeline(str, ConsoleColor.DarkMagenta);
    }

    public static void WriteBlank(int count = 1)
    {
      for (int index = 0; index < count; ++index)
        Console.WriteLine("");
    }

    private static void writeline(string str, ConsoleColor c)
    {
      lock (Log.writeObj)
      {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("[" + DateTime.Now.ToString("hh:mm:ss.fff - dd/MM/yyyy") + "] > ");
        Console.ForegroundColor = c;
        Console.Write(str);
        Console.WriteLine("");
      }
    }
  }
}
