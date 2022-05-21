// Decompiled with JetBrains decompiler
// Type: LoginServer.User
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Managers;
using LoginServer.Packets;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LoginServer
{
  internal class User : IDisposable
  {
    public int rank = 1;
    public int clanid = -1;
    public string username = "#";
    public string nickname = "#";
    public string clanname = "NULL";
    private byte[] buffer = new byte[1024];
    public int userId;
    public int sessionId;
    public int clanrank;
    public long claniconid;
    public bool firstlogin;
    private Socket socket;
    private bool disconnected;

    public string ip
    {
      get
      {
        return this.socket.RemoteEndPoint.ToString().Split(':')[0];
      }
    }

    public User(int sessionId, Socket socket)
    {
      this.socket = socket;
      this.sessionId = sessionId;
      this.SendBuffer(LoginServer.Configs.Server.incomingBuffer);
      this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
    }

    private void OnReceive(IAsyncResult iAr)
    {
      try
      {
        int length = this.socket.EndReceive(iAr);
        if (length > 0)
        {
          byte[] inputByte = new byte[length];
          Array.Copy((Array) this.buffer, 0, (Array) inputByte, 0, length);
          string str = Encoding.Default.GetString(Cryption.XOR.decrypt(inputByte));
          string[] separator = new string[1]{ "\n" };
          foreach (string packetStr in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            if (packetStr.Length > 5)
            {
              try
              {
                Handler handler = Packet_Manager.parsePacket(packetStr);
                if (handler != null)
                  new Thread((ThreadStart) (() =>
                  {
                    try
                    {
                      handler.Handle(this);
                    }
                    catch
                    {
                    }
                  })).Start();
              }
              catch
              {
              }
            }
          }
          this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), (object) null);
        }
        else
          this.disconnect();
      }
      catch
      {
        this.disconnect();
      }
    }

    public void send(Packet p)
    {
      try
      {
        byte[] bytes = p.getBytes();
        if (bytes == null)
          return;
        this.socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object) null);
      }
      catch
      {
        this.disconnect();
      }
    }

    public void SendBuffer(byte[] buffer)
    {
      try
      {
        if (buffer == null)
          return;
        this.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object) null);
      }
      catch
      {
        this.disconnect();
      }
    }

    private void sendCallBack(IAsyncResult iAr)
    {
      try
      {
        this.socket.EndSend(iAr);
      }
      catch
      {
        this.disconnect();
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      int num = disposing ? 1 : 0;
    }

    public void Dispose()
    {
      this.Dispose(true);
    }

    public void disconnect()
    {
      if (this.disconnected)
        return;
      this.disconnected = true;
      try
      {
        this.socket.Close();
      }
      catch
      {
      }
      this.socket = (Socket) null;
      this.Dispose();
    }
  }
}
