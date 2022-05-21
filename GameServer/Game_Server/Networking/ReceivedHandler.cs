// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.ReceivedHandler
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;

namespace Game_Server.Networking
{
  internal class ReceivedHandler
  {
    public static void HandlePacket(User usr, string packet)
    {
      try
      {
        Packet_Manager.ParsePacket(packet)?.Handle(usr);
      }
      catch
      {
      }
    }
  }
}
