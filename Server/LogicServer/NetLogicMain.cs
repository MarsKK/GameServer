using LogicServer.Plaza;
using NetEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicServer
{
    public class NetLogicMain : INetLogciInit
    {
        public void NetLogicInit(NetHandler handler)
        {
            new LogonLogic(handler);
        }
    }
}
