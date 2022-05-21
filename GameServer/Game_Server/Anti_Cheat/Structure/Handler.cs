using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Server.Anti_Cheat.Structure
{
    class Handler
    {
        private uint timeStamp = 0;
        public int packetId = 0;
        public string[] blocks;

        public void FillData(uint timeStamp, int packetId, string[] blocks)
        {
            this.timeStamp = timeStamp;
            this.packetId = packetId;
            this.blocks = blocks;
            Console.WriteLine("Received packetid: {0}, time: {1}", packetId, timeStamp);

            foreach (string item in blocks)
            {
                Console.WriteLine(item);
            }
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
            if (blocks[i] != null)
            {
                return blocks[i];
            }
            return null;
        }

        public virtual void Handle(User usr)
        {
            /* Override */
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void WritePacket()
        {
            Log.WriteDebug(string.Join(" ", getAllBlocks));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    


public virtual void Handle(Client usr)
        {
            /* Override */
        }
    }
}

