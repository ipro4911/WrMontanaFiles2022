// Decompiled with JetBrains decompiler
// Type: LoginServer.Server
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

namespace LoginServer
{
  internal class Server
  {
    public int id;
    public string name;
    public string ip;
    public int flag;
    public int minrank;
    public int slot;

    public Server(int serverid, string name, string ip, int flag, int rank, int slot)
    {
      this.id = serverid;
      this.name = name;
      this.ip = ip;
      this.flag = flag;
      this.minrank = rank;
      this.slot = slot;
    }
  }
}
