// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.Cryption
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer.Packets
{
  internal class Cryption
  {
    internal class AdvancedXOR
    {
      public static byte[] encrypt(byte[] inputByte)
      {
        string str = "NULLABCD`abcdefghijklmnopqrstuvwxyz{|}~x/*##{}EFGHIJKLM0932N7686O1P243QRSTUVWXYZ[\\]^_";
        int length = str.Length;
        int num = 3;
        for (int index = 0; index < inputByte.Length; ++index)
        {
          if (num >= length)
            num = 3;
          inputByte[index] = (byte) ((int) inputByte[index] ^ (int) str[num++]);
        }
        return inputByte;
      }

      public static byte[] decrypt(byte[] inputByte)
      {
        string str = "NULLABCDEFGHIJK3QRSTUVWXYZ[\\]^_`abcdefghijklmnopLM0932N7686O1P24qrstuvwxyz{|}~x/*##{}";
        int length = str.Length;
        int num = 3;
        for (int index = 0; index < inputByte.Length; ++index)
        {
          if (num >= length)
            num = 3;
          inputByte[index] = (byte) ((int) inputByte[index] ^ (int) str[num++]);
        }
        return inputByte;
      }
    }

    internal class XOR
    {
      public static readonly byte clientXor = 195;
      public static readonly byte serverXor = 150;

      public static byte[] encrypt(byte[] inputByte)
      {
        for (int index = 0; index < inputByte.Length; ++index)
          inputByte[index] = (byte) ((uint) inputByte[index] ^ (uint) Cryption.XOR.serverXor);
        return inputByte;
      }

      public static byte[] decrypt(byte[] inputByte)
      {
        for (int index = 0; index < inputByte.Length; ++index)
          inputByte[index] = (byte) ((uint) inputByte[index] ^ (uint) Cryption.XOR.clientXor);
        return inputByte;
      }
    }
  }
}
