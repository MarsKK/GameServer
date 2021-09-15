using GameServer;
using LogicServer;
using NetPack;
using System;

namespace NetEvent
{
    public class NetHandler : EventHandler
    {
        //分发消息委托
        public delegate void Disperse(EventNet ev);
        event Disperse disperse;
        //消息包
        EventNet ev;

        //发包委托
        public delegate void Send(ClientItem client, Pack pack);
        event Send send;

        public NetHandler(Send send)
        {
            ev = new EventNet();
            AddListener(EventDefine.SOCKET_SUCCESS, Handler);
            AddListener(EventDefine.SOCKET_ERROR, Handler);
            AddListener(EventDefine.SOCKET_RECIVE, Handler);
            AddListener(EventDefine.SOCKET_SEND, Handler);
            this.send = send;
        }

        public void SendHandler(int eventID, ClientItem client, Pack pack)
        {
            ev.eventType = eventID;
            ev.client = client;
            ev.pack = pack;
            Invoke(ev);
        }

        public void RegisterDisperse(Disperse disperse)
        {
            this.disperse += disperse;
        }

        public void CancellationDisperse(Disperse disperse)
        {
            this.disperse -= disperse;
        }

        public void Handler(Event ev)
        {
            int evntType = ev.eventType;
            switch (evntType)
            {
                case EventDefine.SOCKET_SUCCESS:
                    break;
                case EventDefine.SOCKET_ERROR:
                    break;
                case EventDefine.SOCKET_RECIVE://分发给各个服务
                    disperse.Invoke(ev as EventNet);
                    break;
                case EventDefine.SOCKET_SEND://发送数据
                    var evNet = ev as EventNet;
                    send.Invoke(evNet.client, evNet.pack);
                    break;
                default:
                    break;
            }
        }

        //初始化各个服务
        public void SetLogicServer(INetLogciInit logciInit)
        {
            logciInit.NetLogicInit(this);
        }
    }
}
