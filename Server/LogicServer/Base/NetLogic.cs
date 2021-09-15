using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer;
using NetEvent;
using NetPack;

namespace LogicServer
{
    public abstract class NetLogic : INetLogicHandler, IDisposable
    {
        NetHandler handler;

        public NetLogic(NetHandler handler)
        {
            this.handler = handler;
            handler.RegisterDisperse(Disperse);
        }

        public void Disperse(EventNet ev)
        {
            DisperseHandler(ev);
        }

        public abstract void DisperseHandler(EventNet ev);

        public void SendPack(ClientItem client, Pack pack)
        {
            handler.SendHandler(EventDefine.SOCKET_SEND, client, pack);
        }

        public void SendPack(ClientItem client, Int16 main, Int16 sub, Object structObj)
        {
            Pack pack = new Pack(main, sub, Pack.StructToBytes(structObj));
            handler.SendHandler(EventDefine.SOCKET_SEND, client, pack);
        }

        public void Dispose()
        {
            handler.CancellationDisperse(Disperse);
        }
    }
}
