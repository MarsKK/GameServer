using GameServer;
using NetEvent;
using NetPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicServer
{
    public interface INetLogicHandler
    {
        void Disperse(EventNet ev);
        void SendPack(ClientItem client, Pack pack);
    }
}
