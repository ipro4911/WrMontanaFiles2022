// Decompiled with JetBrains decompiler
// Type: LoginServer.Networking.NetworkSocket
// Assembly: LoginServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4AB4670-453A-40FB-BD3B-766B5B590597
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\LS\LoginServer.exe

using LoginServer.Managers;
using System;
using System.Net;
using System.Net.Sockets;

namespace LoginServer.Networking
{
  internal class NetworkSocket
  {
    private static int acceptedConnections = 0;
    private static Socket socket;

    public static bool InitializeSocket(int port)
    {
      try
      {
        NetworkSocket.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        NetworkSocket.socket.Bind((EndPoint) new IPEndPoint(IPAddress.Any, port));
        NetworkSocket.socket.Listen(0);
        NetworkSocket.socket.BeginAccept(new AsyncCallback(NetworkSocket.OnReceive), (object) NetworkSocket.socket);
        Log.WriteLine("Listening connections from " + (object) port);
        return true;
      }
      catch
      {
      }
      return false;
    }

    private static void OnReceive(IAsyncResult iAr)
    {
      NetworkSocket.socket.BeginAccept(new AsyncCallback(NetworkSocket.OnReceive), (object) NetworkSocket.socket);
      try
      {
        Socket socket = ((Socket) iAr.AsyncState).EndAccept(iAr);
        string ipAddress = socket.RemoteEndPoint.ToString().Split(':')[0];
        Log.WriteLine("Accepted connection from " + ipAddress);
        ++NetworkSocket.acceptedConnections;
        if (NetworkSocket.acceptedConnections >= LoginServer.Configs.Server.MaxSessions)
          NetworkSocket.acceptedConnections = 1;
        string code = Program.ipLookup.getCountry(ipAddress).getCode();
        if (!CountryManager.IsLockedCountry(code))
        {
          User user = new User(NetworkSocket.acceptedConnections, socket);
        }
        else
          Log.WriteError(ipAddress + " has been refused [" + code + "]");
      }
      catch
      {
      }
    }
  }
}
