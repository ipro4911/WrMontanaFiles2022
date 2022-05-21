﻿// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_Place
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Room_Data
{
  internal class RoomHandler_Place : RoomDataHandler
  {
    public override void Handle(User usr, Room room)
    {
      if (!room.gameactive || usr.Plantings >= 8 || (usr.Health <= 0 || !usr.IsAlive()))
        return;
      string block = this.getBlock(27);
      if (usr.HasItem(block))
      {
        ++usr.Plantings;
        this.sendBlocks[8] = (object) room.AddPlacement(usr, block);
      }
      else
        usr.disconnect();
      this.sendPacket = true;
    }
  }
}