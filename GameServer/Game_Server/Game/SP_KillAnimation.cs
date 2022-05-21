﻿// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.SP_KillAnimation
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class SP_KillAnimation : Packet
  {
    public SP_KillAnimation(SP_KillAnimation.Type opcode)
    {
      this.newPacket((ushort) 31510);
      this.addBlock((object) (ushort) opcode);
      this.addBlock((object) 0);
    }

    public enum Type : ushort
    {
      FirstKill,
      SecondKill,
      RevengeKill,
      Assasin,
      GrenadeKill,
      HeadShot,
      DoubleKill,
      TripleKill,
      QuadraKill,
      PentaKill,
      HexaKill,
      UltraKill,
      Unknown,
    }
  }
}
