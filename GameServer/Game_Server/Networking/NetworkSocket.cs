// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.NetworkSocket
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Net;
using System.Net.Sockets;

namespace Game_Server.Networking
{
  internal class NetworkSocket
  {
    private static uint acceptedConnections = 0;
    private static Socket socket;

    public static bool InitializeSocket(int port)
    {
      try
      {
        NetworkSocket.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        NetworkSocket.socket.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
        NetworkSocket.socket.Listen(0);
        NetworkSocket.socket.BeginAccept(new AsyncCallback(NetworkSocket.OnReceive), (object) NetworkSocket.socket);
        Log.WriteLine("Listening connections on port " + (object) port);
        return true;
      }
      catch
      {
      }
      return false;
    }

    private static void OnReceive(IAsyncResult iAr)
    {
      if (!Program.running)
        return;
      NetworkSocket.socket.BeginAccept(new AsyncCallback(NetworkSocket.OnReceive), (object) NetworkSocket.socket);
      Socket socket = ((Socket) iAr.AsyncState).EndAccept(iAr);
      Log.WriteLine("Accepted connection from " + socket.RemoteEndPoint.ToString().Split(':')[0]);
      ++NetworkSocket.acceptedConnections;
      if ((long) NetworkSocket.acceptedConnections >= (long) Game_Server.Configs.Server.MaxSessions)
        NetworkSocket.acceptedConnections = 1U;
      User user = new User(NetworkSocket.acceptedConnections, socket);
    }
  }
}
