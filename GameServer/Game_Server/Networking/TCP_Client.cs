// Decompiled with JetBrains decompiler
// Type: Game_Server.Networking.TCP_Client
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Game_Server.Networking
{
    internal class TCP_Client : IDisposable
    {
        private byte[] buffer = new byte[1024];
        public User usr;
        private Socket socket;
        public ushort connectionId;
        public string remoteIp;
        public bool disconnected;

        public TCP_Client(Socket socket)
        {
            this.socket = socket;
            this.remoteIp = (socket.RemoteEndPoint as IPEndPoint).Address.ToString();
            new Thread(new ThreadStart(this.OnReceive))
            {
                Priority = ThreadPriority.Highest
            }.Start();
        }

        public void Send(byte[] buffer)
        {
            try
            {
                if (buffer == null || buffer.Length <= 0)
                    return;
                this.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallBack), (object)null);
            }
            catch
            {
            }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            if (this.socket == null)
                return;
            try
            {
                this.socket.EndSend(iAr);
            }
            catch
            {
            }
        }

        private List<TCP_Client> GetUsersInMyRoom
        {
            get
            {
                if (this.usr.room != null)
                    return TCP.userConnections.Where<TCP_Client>((Func<TCP_Client, bool>)(r =>
                    {
                        if (r.usr != null && this.usr != null && (r.usr.userId != this.usr.userId && r.usr.room != null))
                            return r.usr.room.id == this.usr.room.id;
                        return false;
                    })).ToList<TCP_Client>();
                return (List<TCP_Client>)null;
            }
        }

        private void SendToRoom(byte[] data)
        {
            foreach (TCP_Client tcpClient in this.GetUsersInMyRoom)
                tcpClient.Send(data);
        }

        private TCP_Client.TcpPacket AnalyzePacket(byte[] data)
        {
            int num1 = (int)data[0];
            ushort num2 = Game_Server.Generic.ByteToUShort(data, 1);
            ushort num3 = Game_Server.Generic.ByteToUShort(data, 3);
            TCP_Client.TcpPacket tcpPacket = (TCP_Client.TcpPacket)num3;
            if (data.Length == (int)num2 + 3)
            {
                switch (tcpPacket)
                {
                    case TCP_Client.TcpPacket.Authentication:
                        if (this.usr == null)
                        {
                            this.connectionId = Game_Server.Generic.ByteToUShort(data, 7);
                            this.usr = UserManager.GetUser(this.connectionId);
                            if (this.usr == null)
                            {
                                this.disconnect("No valid p2s user");
                                return tcpPacket;
                            }
                            this.usr.tcpClient = this;
                            break;
                        }
                        break;
                    case TCP_Client.TcpPacket.ClientHearthBeat:
                        this.usr.heartBeatTime = Game_Server.Generic.timestamp + 60;
                        break;
                    case TCP_Client.TcpPacket.HackShield:
                        break;
                    default:
                        if (tcpPacket == TCP_Client.TcpPacket.UpdatePlayerStatus || tcpPacket == TCP_Client.TcpPacket.ThrowGranadeRocket || (tcpPacket == TCP_Client.TcpPacket.WeaponExplosion || tcpPacket == TCP_Client.TcpPacket.HackInfo) || (tcpPacket == TCP_Client.TcpPacket.ObjectMove || tcpPacket == TCP_Client.TcpPacket.PlayerEmotion || (tcpPacket == TCP_Client.TcpPacket.PlayerRoll || tcpPacket == TCP_Client.TcpPacket.UpdateVehicleStatus)) || (tcpPacket == TCP_Client.TcpPacket.SwitchWeapon || tcpPacket == TCP_Client.TcpPacket.ThrowGranadeRocket || (tcpPacket == TCP_Client.TcpPacket.WeaponExplosion || tcpPacket == TCP_Client.TcpPacket.WeaponZoom) || tcpPacket == TCP_Client.TcpPacket.TextChat))
                        {
                            if (this.usr != null && this.usr.room != null && this.usr.room.gameactive && ((this.usr.room.users.Count > 1 || this.usr.room.spectators.Count > 0) && ((int)data[9] == this.usr.roomslot && this.usr.IsAlive())))
                            {
                                if (tcpPacket == TCP_Client.TcpPacket.ThrowGranadeRocket)
                                {
                                    Item itemById = ItemManager.GetItemByID(this.usr.weapon);
                                    if (itemById != null)
                                    {
                                        bool flag = this.usr.room.new_mode == 6 && this.usr.room.new_mode_sub == 2;
                                        int newMode = this.usr.room.new_mode;
                                        if (!flag)
                                        {
                                            if (itemById.UseableBranch(4) && (itemById.UseableSlot(2) || itemById.UseableSlot(5) || itemById.UseableSlot(7)))
                                                ++this.usr.throwRockets;
                                            else if (itemById.UseableSlot(3) || itemById.UseableBranch(4))
                                                ++this.usr.throwNades;
                                        }
                                    }
                                }
                                this.SendToRoom(data);
                                break;
                            }
                            break;
                        }
                     //   Log.WriteError("Unhandled TCP Packet (" + (object)num3 + ") " + this.usr.nickname + " " + (object)this.usr.room.id);
                        break;
                }
            }
            return tcpPacket;
        }

        private void OnReceive()
        {
            try
            {
                while (!this.disconnected && this.socket.Connected && (this.usr != null || this.connectionId <= (ushort)0))
                {
                    int length = this.socket.Receive(this.buffer);
                    if (length > 0)
                    {
                        byte[] data = new byte[length];
                        Array.Copy((Array)this.buffer, 0, (Array)data, 0, length);
                        int num = (int)this.AnalyzePacket(data);
                    }
                    else
                    {
                        this.disconnect((string)null);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.disconnect((string)null);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
            GC.Collect();
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private void disconnect(string reason = null)
        {
            if (this.disconnected)
                return;
            try
            {
                this.socket.Close();
            }
            catch
            {
            }
            this.disconnected = true;
            if (this.usr != null)
                this.usr.disconnect();
            if (reason != null)
                Log.WriteDebug(this.usr.nickname + " has been disconnected [Reason: " + reason + "]");
            TCP.RemoveConnection(this);
            this.Dispose();
        }

        private enum TcpPacket : ushort
        {
            UpdatePlayerStatus = 12544, // 0x3100
            PlayerRoll = 12545, // 0x3101
            WeaponZoom = 12546, // 0x3102
            PlayerEmotion = 12547, // 0x3103
            ObjectMove = 12800, // 0x3200
            UpdateVehicleStatus = 12801, // 0x3201
            Bullet = 13312, // 0x3400
            ThrowGranadeRocket = 13312, // 0x3400
            SwitchWeapon = 13313, // 0x3401
            HackInfo = 13314, // 0x3402
            Explosion = 13315, // 0x3403
            WeaponExplosion = 13315, // 0x3403
            TextChat = 13824, // 0x3600
            Authentication = 22039, // 0x5617
            ClientHearthBeat = 22040, // 0x5618
            HackShield = 31264, // 0x7A20
        }
    }
}
