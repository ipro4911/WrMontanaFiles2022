// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.UDPPacket
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

/*namespace Game_Server.Networking
{
  public class UDPPacket
  {
    public byte[] data;
    public UDPPacket.Identity identity;

    public UDPPacket(byte[] data)
    {
      this.data = data;
      if (data.Length == 14 && data[0] == (byte) 16 && (data[1] == (byte) 1 && data[2] == (byte) 1))
        this.identity = UDPPacket.Identity.Authentication;
      else if (data.Length == 46 && data[0] == (byte) 16 && (data[1] == (byte) 16 && data[2] == (byte) 0) && data[14] == (byte) 33)
        this.identity = UDPPacket.Identity.IP;
      else if (data.Length > 20 && data[0] == (byte) 16 && (data[1] == (byte) 16 && data[2] == (byte) 0) && (data[14] == (byte) 46 || data[14] == (byte) 49 || (data[14] == (byte) 52 || data[14] == (byte) 48)))
        this.identity = UDPPacket.Identity.Tunneling;
      else
        this.identity = UDPPacket.Identity.Unknown;
    }

    public enum Identity
    {
      Unknown,
      Authentication,
      IP,
      Tunneling,
    }
  }
}
*/