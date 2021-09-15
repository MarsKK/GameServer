using NetEvent;
using NetPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    class MonitorHeartBeat
    {
        private Server server;
        private Thread thread;
        private bool isRun;
        private Pack pack;

        private long time = 8 * 1000;
        private long timeOut = 30 * 1000;


        public MonitorHeartBeat(Server server)
        {
            this.server = server;
            thread = new Thread(OnMonitor);
            pack = new Pack(NetCmd.SOCKET_MAIN_HEART, NetCmd.SOCKET_SUB_HEART, null);
        }

        public void OnMonitor()
        {
            while (isRun)
            {
                var recives = server.GetRecives();
                lock (recives)
                {
                    long curTime = Tools.GetMilliseconds();
                    for (int i = 0; i < recives.Count; i++)
                    {
                        var recive = recives[i];
                        var delayTime = curTime - recive.client.lastSendTime;
                        if (delayTime >= time) //发送心跳
                        {
                            server.handler.SendHandler(EventDefine.SOCKET_SEND, recive.client, pack);
                            recive.client.lastSendTime = curTime;
                        }
                        delayTime = curTime - recive.lastReceiveTime;
                        if (delayTime >= timeOut) //超时断开连接
                        {
                            Console.WriteLine(recive.client.clientMsg + "，接收超时，断开连接.");
                            recive.Close();
                            recives.Remove(recive);
                            i = i - 1;
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        public void Start()
        {
            isRun = true;
            thread.Start();
        }

        public void Close()
        {
            isRun = false;
            thread.Abort();
        }
    }
}
