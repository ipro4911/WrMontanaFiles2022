// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.UDPReader
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Net;

/*namespace Game_Server.Networking
{
  public static class UDPReader
  {
    private const byte xOrSendKey = 96;
    private const byte xOrReceiveKey = 62;

    public static ushort ToUShort(this byte[] packet, int offset)
    {
      byte[] numArray = new byte[2];
      Array.Copy((Array) packet, offset, (Array) numArray, 0, 2);
      Array.Reverse((Array) numArray);
      return BitConverter.ToUInt16(numArray, 0);
    }

    public static uint ToUInt(this byte[] packet, int offset)
    {
      byte[] numArray = new byte[4];
      Array.Copy((Array) packet, offset, (Array) numArray, 0, 4);
      Array.Reverse((Array) numArray);
      return BitConverter.ToUInt32(numArray, 0);
    }

    public static IPEndPoint ToIPEndPoint(this byte[] packet, int offset)
    {
      for (int index = offset; index < offset + 6; ++index)
        packet[index] ^= (byte) 96;
      ushort uint16 = BitConverter.ToUInt16(packet, offset);
      return new IPEndPoint((long) BitConverter.ToUInt32(packet, offset + 2), (int) uint16);
    }

    public static void WriteUShort(this byte[] packet, ushort value, int offset)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse((Array) bytes);
      Array.Copy((Array) bytes, 0, (Array) packet, offset, 2);
    }

    public static void WriteUInt(this byte[] packet, uint value, int offset)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Reverse((Array) bytes);
      Array.Copy((Array) bytes, 0, (Array) packet, offset, 4);
    }

    public static void WriteIPEndPoint(this byte[] packet, IPEndPoint endpoint, int offset)
    {
      byte[] numArray = new byte[6];
      Array.Copy((Array) BitConverter.GetBytes(endpoint.Port), 0, (Array) numArray, 0, 2);
      Array.Copy((Array) endpoint.Address.GetAddressBytes(), 0, (Array) numArray, 2, 4);
      Array.Reverse((Array) numArray);
      for (int index = offset; index < offset + 6; ++index)
        packet[index] = (byte) ((uint) numArray[index - offset] ^ 62U);
    }

    public static byte[] Extend(this byte[] packet, int length)
    {
      byte[] numArray = new byte[length];
      Array.Copy((Array) packet, (Array) numArray, packet.Length);
      return numArray;
    }
  }
}
*/