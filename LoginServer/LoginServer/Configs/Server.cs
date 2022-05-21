// Decompiled with JetBrains decompiler
// Type: LoginServer.Configs.Server
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Packets;

namespace LoginServer.Configs
{
  internal class Server
  {
    public static int MaxSessions = 1000;
    public static int MaxInventorySlot = 50;
    public static int MaxCostumeSlot = 50;
    public static byte[] incomingBuffer = new SP_ReceiveConnection().getBytes();
  }
}
