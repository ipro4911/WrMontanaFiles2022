using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_Server;

namespace Game_Server.Game
{
    class HANDLE_POSTBOX : Handler
    {
        public override void Handle(Game_Server.User user)
        {
            int Type = int.Parse(getBlock(0));
            int ID = int.Parse(getBlock(1));
            if (Type == 1)
            {
                string[] strArray = DB.RunQuery("SELECT title, itemcode, days, count, starttime, endtime FROM postbox WHERE ownerid='" + this.userId + "'");
                Item asd = ItemManager.GetItem(strArray[1]);
                User.AddItem(asd.Code, int.Parse(strArray[3]), int.Parse(strArray[2]), 0, true);
                DB.runQuery("DELETE FROM postbox WHERE ownerid='" + User.UserID + "'");
                User.send(new PACKET_POSTBOX_RECEIVE(User));
            }
        }
    }
}

