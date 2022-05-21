// Decompiled with JetBrains decompiler
// Type: LoginServer.Managers.Packet_Manager
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Packets;
using System;
using System.Collections.Generic;

namespace LoginServer.Managers
{
  internal class Packet_Manager
  {
    private static Dictionary<int, Handler> packets = new Dictionary<int, Handler>();

    public static void setup()
    {
      Packet_Manager.addPacket(4112, (Handler) new CP_PatchInformationHandler());
      Packet_Manager.addPacket(4352, (Handler) new CP_LoginHandler());
      Packet_Manager.addPacket(4353, (Handler) new CP_NewUserHandler());
      Packet_Manager.addPacket(4609, (Handler) new CP_ServerRefresh());
      Packet_Manager.addPacket(4252, (Handler) new CP_PatchInformationUpdateHandler());
      Log.WriteLine("Loaded " + (object) Packet_Manager.packets.Count + " packet handlers");
    }

    public static Handler parsePacket(string packetStr)
    {
      try
      {
        string[] strArray = packetStr.Split(' ');
        long result = 0;
        long.TryParse(strArray[0], out result);
        int index = int.Parse(strArray[1]);
        if (Packet_Manager.packets.ContainsKey(index))
        {
          string[] blocks = new string[strArray.Length - 2];
          Array.Copy((Array) strArray, 2, (Array) blocks, 0, strArray.Length - 2);
          Handler packet = Packet_Manager.packets[index];
          packet.FillData(result, index, blocks);
          return packet;
        }
      }
      catch
      {
      }
      return (Handler) null;
    }

    private static void addPacket(int id, Handler handler)
    {
      if (!Packet_Manager.packets.ContainsKey(id))
        Packet_Manager.packets.Add(id, handler);
      else
        Log.WriteError("Packet Manager already contains packetID: " + (object) id);
    }
  }
}
