// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.TCP
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game_Server.Networking
{
    internal class TCP
    {
        public static List<TCP_Client> userConnections = new List<TCP_Client>();
        private static object lockobj = new object();
        private static ushort lastConnectionId = 0;
        public static byte[] ClientHearthBeatPacket = new byte[7]
        {
      (byte) 204,
      (byte) 4,
      (byte) 0,
      (byte) 24,
      (byte) 86,
      (byte) 0,
      (byte) 0
        };
        private static Socket socket;
        private static ushort port;
        private static EndPoint endPoint;

        private static void HearthBeat()
        {
            while (true)
            {
                foreach (TCP_Client tcpClient in TCP.userConnections.ToList<TCP_Client>())
                {
                    if (tcpClient.usr != null)
                        tcpClient.Send(TCP.ClientHearthBeatPacket);
                }
                Thread.Sleep(60000);
            }
        }

        private static bool Initialize()
        {
            TCP.endPoint = (EndPoint)new IPEndPoint(IPAddress.Any, (int)TCP.port);
            try
            {
                TCP.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TCP.socket.Bind(TCP.endPoint);
                TCP.socket.Listen(0);
                TCP.socket.BeginAccept(new AsyncCallback(TCP.OnNewConnection), (object)TCP.socket);
                new Thread(new ThreadStart(TCP.HearthBeat)).Start();
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static bool Start(int srvPort)
        {
            TCP.port = (ushort)srvPort;
            bool flag = TCP.Initialize();
            if (flag)
                Log.WriteLine("Listening TCP connections on port " + (object)TCP.port);
            return flag;
        }

        public static void RemoveConnection(TCP_Client client)
        {
            if (!TCP.userConnections.Contains(client))
                return;
            TCP.userConnections.Remove(client);
        }

        public static ushort GetFreeConnectionID
        {
            get
            {
                ++TCP.lastConnectionId;
                if (TCP.lastConnectionId >= ushort.MaxValue)
                    TCP.lastConnectionId = (ushort)1;
                return TCP.lastConnectionId;
            }
        }

        private static void OnNewConnection(IAsyncResult iAr)
        {
            TCP.socket.BeginAccept(new AsyncCallback(TCP.OnNewConnection), (object)TCP.socket);
            try
            {
                TCP_Client tcpClient = new TCP_Client(((Socket)iAr.AsyncState).EndAccept(iAr));
                TCP.userConnections.Add(tcpClient);
            }
            catch
            {
            }
        }
    }
}
