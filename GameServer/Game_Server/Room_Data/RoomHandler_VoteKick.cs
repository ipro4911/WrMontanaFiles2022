// Decompiled with JetBrains decompiler
// Type: Game_Server.Room_Data.RoomHandler_VoteKick
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using Game_Server.Game;
using System;
using System.Linq;

namespace Game_Server.Room_Data
{
  internal class RoomHandler_VoteKick : RoomDataHandler
  {
    public override void Handle(Game_Server.User usr, Room room)
    {
      byte num = byte.Parse(this.getBlock(7));
      bool flag = int.Parse(this.getBlock(6)) == 0;
      if ((int) num == usr.roomslot || room.mode == 1 || room.channel == 3)
        return;
      Game_Server.User user = room.GetUser((int) num);
      int side1 = room.GetSide(usr);
      room.GetSideCount(side1);
      int.Parse(this.getBlock(6));
      if (user == null)
        usr.send((Packet) new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.InvalidCandidate));
      else if (room.GetSideCount(side1) < 4)
        usr.send((Packet) new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.Need4Players));
      else if (room.GetSide(user) != side1 && room.voteKick.running && flag)
        usr.send((Packet) new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.VoteKickInProgress));
      else if (user == null || (int) num == usr.roomslot || user.rank > 2)
        usr.send((Packet) new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.InvalidCandidate));
      else if (room.voteKick.lastKickTimestamp > Generic.timestamp)
      {
        usr.send((Packet) new SP_Chat("GM", SP_Chat.ChatType.Room_ToAll, "GM >> You have to wait 60 seconds!", 999U, "GM"));
      }
      else
      {
        int side2 = room.GetSide(user);
        if (side2 == side1)
        {
          if (!room.voteKick.running)
          {
            this.sendPacket = true;
            this.sendBlocks[6] = (object) "1";
            room.voteKick.StartVote(user.roomslot, side2);
          }
          else
          {
            if (room.voteKick.votes.Where<Room.VoteKick.VoteKickVote>((Func<Room.VoteKick.VoteKickVote, bool>) (r => r.usr.userId == usr.userId)).Count<Room.VoteKick.VoteKickVote>() != 0)
              return;
            room.voteKick.AddUserVotekick(usr, true);
          }
        }
        else
          usr.send((Packet) new SP_RoomData_VoteKick(SP_RoomData_VoteKick.ErrCodes.CannotKickOpposingTeam));
      }
    }
  }
}
