// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.CP_ZombieMultiPlayer
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class CP_ZombieMultiPlayer : Handler
  {
    public override void Handle(Game_Server.User usr)
    {
      if (usr.channel != 3)
        return;
      Room room = usr.room;
      if (room == null || room.users.Count <= 1)
        return;
      room.send((Packet) new SP_Unknown((ushort) 31490, (object[]) this.getAllBlocks));
    }
  }
}
