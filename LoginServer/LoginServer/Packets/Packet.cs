// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.Packet
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using System;
using System.Text;

namespace LoginServer.Packets
{
  internal class Packet
  {
    private int packetId = -1;
    private StringBuilder packet = new StringBuilder();

    public byte[] getBytes()
    {
      return Cryption.XOR.encrypt(Encoding.GetEncoding("Windows-1250").GetBytes(this.packet.ToString().Remove(this.packet.Length - 1, 1).ToString() + (object) ' ' + (object) '\n'));
    }

    protected void newPacket(int packetId)
    {
      if (this.packetId == -1)
      {
        this.packetId = packetId;
        this.packet.Append(Environment.TickCount);
        this.packet.Append(" ");
        this.packet.Append(packetId);
        this.packet.Append(" ");
      }
      else
        Log.WriteError("Coudln't re-declare packetId!");
    }

    protected void addBlock(object block)
    {
      block = (object) block.ToString().Replace(' ', '\x001D');
      this.packet.Append(block.ToString());
      this.packet.Append(" ");
    }

    protected void Fill(object block, int length)
    {
      for (int index = 0; index < length; ++index)
        this.addBlock(block);
    }
  }
}
