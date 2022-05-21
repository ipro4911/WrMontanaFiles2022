// Decompiled with JetBrains decompiler
// Type: Game_Server.Game.Cryption
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

namespace Game_Server.Game
{
  internal class Cryption
  {
            public const byte clientXor = 0x5B;//0x3E;
          public const byte serverXor = 0x38;//0x60;
        //public const byte clientXor = 62;
     ///   public const byte serverXor = 96;
        public static byte[] encrypt(byte[] inputByte)
    {
            for (int index = 0; index < inputByte.Length; ++index)
                inputByte[index] ^= serverXor;//96;
      return inputByte;
    }

    public static byte[] decrypt(byte[] inputByte)
    {
            for (int index = 0; index < inputByte.Length; ++index)
                inputByte[index] ^= clientXor;///62;
            return inputByte;
    }
  }
}
