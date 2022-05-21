// Decompiled with JetBrains decompiler
// Type: LoginServer.Packets.Handler
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer.Packets
{
  internal class Handler
  {
    private long timeStamp;
    private int packetId;
    private string[] blocks;

    public void FillData(long timeStamp, int packetId, string[] blocks)
    {
      this.timeStamp = timeStamp;
      this.packetId = packetId;
      this.blocks = blocks;
    }

    public string[] getAllBlocks
    {
      get
      {
        return this.blocks;
      }
    }

    public string getBlock(int i)
    {
      if (this.blocks[i] != null)
        return this.blocks[i];
      return (string) null;
    }

    public virtual void Handle(User usr)
    {
    }
  }
}
