// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.UDP
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server;
using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/*namespace Game_Server.Networking
{
  internal class UDP
  {
    private byte[] buffer = new byte[4096];
    private int port;
    private UdpClient socket;
    private Thread receiveConnectionsThread;
    private EndPoint endPoint;

    public bool InitializeSocket()
    {
      try
      {
        if (this.socket != null)
          this.socket.Close();
      }
      catch
      {
      }
      this.socket = (UdpClient) null;
      this.endPoint = (EndPoint) new IPEndPoint(IPAddress.Any, this.port);
      try
      {
        this.endPoint = (EndPoint) new IPEndPoint(IPAddress.Any, this.port);
        this.socket = new UdpClient(this.port);
        this.receiveConnectionsThread = new Thread(new ThreadStart(this.ReceiveUDPConnections));
        this.receiveConnectionsThread.Priority = ThreadPriority.Highest;
        this.receiveConnectionsThread.SetApartmentState(ApartmentState.STA);
        this.receiveConnectionsThread.Start();
        return true;
      }
      catch (Exception ex)
      {
        Log.WriteError("Error re-initializing socket...");
      }
      return false;
    }

    public void RestartUDP()
    {
      try
      {
        this.socket.Close();
      }
      catch
      {
      }
      if (!this.InitializeSocket())
        return;
      Log.WriteError("UDP restarted successfully");
    }

    public bool Start(int port)
    {
      this.port = port;
      bool flag = this.InitializeSocket();
      if (flag)
        Log.WriteLine("Binded the UDP socket to port " + (object) port);
      return flag;
    }

    private void HandleUDP(byte[] buffer, IPEndPoint IPeo)
    {
      try
      {
        byte[] datagram = this.AnalyzePacket(buffer, IPeo);
        if (datagram == null || datagram.Length <= 0)
          return;
        this.socket.BeginSend(datagram, datagram.Length, IPeo, new AsyncCallback(this.sendCallBack), (object) this.socket);
      }
      catch
      {
      }
    }

    public void ReceiveUDPConnections()
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, this.port);
      while (true)
      {
        try
        {
          byte[] buffer;
          do
          {
            buffer = this.socket.Receive(ref remoteEP);
          }
          while (buffer.Length <= 0);
          this.HandleUDP(buffer, remoteEP);
        }
        catch (Exception ex)
        {
        }
      }
    }

    private void sendCallBack(IAsyncResult iAr)
    {
      try
      {
        ((Socket) iAr.AsyncState).EndSendTo(iAr);
      }
      catch
      {
      }
    }

    private byte[] AnalyzePacket(byte[] packet, IPEndPoint IPeo)
    {
      byte[] numArray = packet;
      int num1 = (int) packet.ToUShort(0);
      ushort num2 = packet.ToUShort(4);
      int num3 = (int) packet.ToUInt(10);
      User tarGetUser1 = UserManager.getTarGetUser((uint) num2);
      switch (new UDPPacket(packet).identity)
      {
        case UDPPacket.Identity.Authentication:
          if (tarGetUser1 != null)
          {
            if (tarGetUser1.userId != num3)
              return (byte[]) null;
            packet.WriteUShort((ushort) (this.port + 1), 4);
            tarGetUser1.setRemoteEndPoint(IPeo);
            break;
          }
          break;
        case UDPPacket.Identity.IP:
          if (tarGetUser1 != null)
          {
            IPEndPoint ipEndPoint = packet.ToIPEndPoint(32);
            if (tarGetUser1.remoteEndPoint == ipEndPoint)
            {
              tarGetUser1.setLocalEndPoint(ipEndPoint);
              tarGetUser1.setRemoteEndPoint(IPeo);
              numArray = packet.Extend(65);
              numArray[17] = (byte) 65;
              numArray[numArray.Length - 1] = (byte) 62;
              numArray.WriteUShort((ushort) tarGetUser1.sessionId, 4);
              if (tarGetUser1.remoteEndPoint != null)
                numArray.WriteIPEndPoint(tarGetUser1.remoteEndPoint, 32);
              if (tarGetUser1.localEndPoint != null)
              {
                numArray.WriteIPEndPoint(tarGetUser1.localEndPoint, 50);
                break;
              }
              break;
            }
            break;
          }
          break;
        case UDPPacket.Identity.Tunneling:
          if (tarGetUser1 != null)
          {
            int num4 = (int) packet.ToUShort(6);
            if (tarGetUser1.room != null && tarGetUser1.room.id == num4)
            {
              User tarGetUser2 = UserManager.getTarGetUser((uint) packet.ToUShort(22));
              if (tarGetUser2 != null && tarGetUser2.room.id == tarGetUser1.room.id)
              {
                IPEndPoint endPoint = tarGetUser1.remoteEndPoint != tarGetUser2.remoteEndPoint ? tarGetUser2.remoteEndPoint : tarGetUser2.localEndPoint;
                this.socket.BeginSend(numArray, numArray.Length, endPoint, new AsyncCallback(this.sendCallBack), (object) null);
                using (IEnumerator<User> enumerator = tarGetUser2.room.spectators.Values.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    User current = enumerator.Current;
                    this.socket.BeginSend(numArray, numArray.Length, endPoint, new AsyncCallback(this.sendCallBack), (object) null);
                  }
                  break;
                }
              }
              else
                break;
            }
            else
              break;
          }
          else
            break;
      }
      return numArray;
    }
  }
}
*/

namespace GameServer.Networking
{
    class UDP
    {
        ~UDP()
        {
            GC.Collect();
        }
        private int port;
        private Socket socket = null;
        private EndPoint endPoint;
        private Thread receiveThread;
        private byte[] buffer = new byte[1024];
        private object _server;

        public bool InitializeSocket()
        {
            try
            {
                if (socket != null)
                {
                    socket.Close();
                }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(OnReceive), null);
                return true;
            }
            catch
            {
                InitializeSocket();
                Log.WriteError("Error re-initializing socket...");
            }
            return false;
        }

        public bool Start(int port)
        {
            this.port = port;
            this.endPoint = new IPEndPoint(IPAddress.Any, this.port);
            bool status = InitializeSocket();
            if (status)
            {
                Log.WriteLine("Binded the UDP socket to port " + port);
            }
            return status;
        }

        public void OnReceive(IAsyncResult iAr)
        {
            try
            {
                EndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
                int length = socket.EndReceiveFrom(iAr, ref receiveEndPoint);
                if (length > 0)
                {
                    byte[] pBuffer = new byte[length];
                    Array.Copy(buffer, 0, pBuffer, 0, length);
                    byte[] response = AnalyzePacket(pBuffer, receiveEndPoint as IPEndPoint);
                    socket.SendTo(response, receiveEndPoint as IPEndPoint);
                  //  socket.Send(response, response.Length, receiveEndPoint);
                    //socket.BeginSend(response, response.Length, endPoint, SendProc, socket);
                }

                endPoint = new IPEndPoint(IPAddress.Any, 0);
                socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint, new AsyncCallback(OnReceive), null);
            }
            catch (Exception ex)
            {
                InitializeSocket();
            }
        }

        void SendProc(IAsyncResult t)
        {
            try
            {
                UdpClient a = (UdpClient)t.AsyncState;
                a.EndSend(t);
            }
            catch (Exception ex)
            {
                Log.WriteError("Error while 'SendProc':\n" + ex.Message);
            }
        }

        private byte[] AnalyzePacket(byte[] packet, IPEndPoint IPeo)
        {
            try
            {
                byte[] response = packet;
                uint type = packet.ToUShort(0);
                ushort sessionID = packet.ToUShort(4);
                User user = Game_Server.Managers.UserManager.getTarGetUser(sessionID);
                if (user == null)
                    return new Byte[1] { 0x00 };

                switch (type)
                {
                    case 0x1001: //Initial packet
                        packet.WriteUShort((ushort)(this.port + 1), 4);
                        user.setRemoteEndPoint(IPeo);
                        Log.WriteDebug(user.nickname + " -> UDP Connected & Set Remote End Point " + IPeo.Address.ToString());
                        break;
                    case 0x1010: //UDP Ping packet
                        if (packet[14] == 0x21)
                        {
                            user.setLocalEndPoint(packet.ToIPEndPoint(32));

                            Log.WriteDebug(user.nickname + " -> UDP Connected & Set Local End Point " + user.localEndPoint.Address.ToString());

                            response = packet.Extend(65);

                            #region old response bytes
                            /*
                            byte[] response = new byte[65]
                                {
                                    0x10, 0x10, //0
                                    0x0, 0x0, //2
                                    0x0, 0x0, //4
                                    0xFF, 0xFF, 0xFF, 0xFF, //6
                                    0x0, 0x0, 0x0, 0x0, //10
                                    0x21, 0x0, 0x0, 0x41, //14
                                    0x0, 0x0, 0x0, 0x0, //18
                                    0x0, 0x0, 0x0, 0x0, //22
                                    0x0, 0x0, //26
                                    0x1, 0x11, 0x13, 0x11, //28
                                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, //32 remoteip
                                    0x11, 0x11, 0x11, 0x11, //38
                                    0x11, 0x11, 0x11, 0x11, //42
                                    0x01, 0x11, 0x13, 0x11, //48
                                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, //50 localip
                                    0x19, 0x19, 0x19, 0x19, //56
                                    0x19, 0x19, 0x19, 0x19, //60
                                    0x11 //64
                                };
                             */
                            #endregion

                            response[17] = 0x41;
                            response[response.Length - 1] = 0x11;
                            response.WriteUShort((ushort)user.sessionId, 4); //Not really necessary
                            response.WriteIPEndPoint(user.remoteEndPoint, 32);
                            response.WriteIPEndPoint(user.localEndPoint, 50);
                        }
                        else if (packet[14] == 0x31)
                        {
                            if (user.room != null)
                            {
                                ushort targetID = packet.ToUShort(22);
                                User player = Game_Server.Managers.UserManager.getTarGetUser(targetID);
                                if (player != null && player.room.id == user.room.id)
                                {
                                    socket.SendTo(packet, player.remoteEndPoint);

                                    foreach (User u in player.room.spectators.Values)
                                    {
                                        socket.SendTo(packet, socket.RemoteEndPoint);
                                    }
                                }
                            }

                            #region Daan Tunneling
                            /*if (user.room == null) return;
                            uint room = packet.ToUShort(6);
                            if (User.CurrentRoom == room) {
                                ushort targetID = packet.ToUShort(22);
                                User target = UserManager.GetUser(targetID);
                                if (target != null) {
                                    Socket.SendTo(packet, user.socket.BeginConnect.targetID);
                                } else {
                                    Console.WriteLine("UDP TUNNEL PACKET FAULT - TARGET DOES NOT EXIST");
                                    Console.WriteLine("Press enter to continue...");
                                    Console.ReadLine();
                                }
                            } else {
                                Console.WriteLine("UDP TUNNEL PACKET FAULTY - ROOM DID NOT MATCH SENDER");
                                Console.WriteLine("Press enter to continue...");
                                Console.ReadLine();
                            }*/
                            #endregion
                        }
                        break;
                }
                return response;
            }
            catch
            {
                return new Byte[1] { 0x00 };
            }
        }
    }

    public static class UdpReader
    {
        private const byte xOrSendKey = Game_Server.Game.Cryption.clientXor;
        private const byte xOrReceiveKey = Game_Server.Game.Cryption.serverXor;

        public static ushort ToUShort(this byte[] packet, int offset)
        {
            try
            {
                byte[] value = new byte[2];
                Array.Copy(packet, offset, value, 0, 2);
                Array.Reverse(value);
                return BitConverter.ToUInt16(value, 0);
            }
            catch { return 0; }
        }

        public static uint ToUInt(this byte[] packet, int offset)
        {
            try
            {
                byte[] value = new byte[4];
                Array.Copy(packet, offset, value, 0, 4);
                Array.Reverse(value);
                return BitConverter.ToUInt32(value, 0);
            }
            catch { return 0; }
        }

        public static IPEndPoint ToIPEndPoint(this byte[] packet, int offset)
        {
            try
            {
                for (int i = offset; i < offset + 6; i++)
                    packet[i] ^= xOrSendKey;
                ushort port = BitConverter.ToUInt16(packet, offset);
                uint ip = BitConverter.ToUInt32(packet, offset + 2);
                return new IPEndPoint(ip, port);
            }
            catch { return null; }
        }

        public static void WriteUShort(this byte[] packet, ushort value, int offset)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                Array.Copy(bytes, 0, packet, offset, 2);
            }
            catch { }
        }

        public static void WriteUInt(this byte[] packet, uint value, int offset)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Array.Reverse(bytes);
                Array.Copy(bytes, 0, packet, offset, 4);
            }
            catch { }
        }

        public static void WriteIPEndPoint(this byte[] packet, IPEndPoint endpoint, int offset)
        {
            try
            {
                byte[] value = new byte[6];
                Array.Copy(BitConverter.GetBytes(endpoint.Port), 0, value, 0, 2);
                Array.Copy(endpoint.Address.GetAddressBytes(), 0, value, 2, 4);
                Array.Reverse(value);
                for (int i = offset; i < offset + 6; i++)
                    packet[i] = (byte)(value[i - offset] ^ xOrReceiveKey);
            }
            catch { }
        }

        public static byte[] Extend(this byte[] packet, int length)
        {
            try
            {
                byte[] newPacket = new byte[length];
                Array.Copy(packet, newPacket, packet.Length);
                return newPacket;
            }
            catch { return packet; }
        }
    }
}