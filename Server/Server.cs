using CipherTool;
using LogicServer;
using NetEvent;
using NetPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    struct SendPack
    {
        public ClientItem client;
        public Pack pack;

        public SendPack(ClientItem client, Pack pack)
        {
            this.client = client;
            this.pack = pack;
        }
    }

    public class Server
    {
        public string ip;
        public int point;

        private TcpListener tcpListener;
        private Thread threadListener, threadSend; private bool isLoop = false;
        //监听客户端心跳
        private MonitorHeartBeat monitor;

        //客户端接收列表
        private List<ReciveItem> recives;
        //发送队列
        private Queue<SendPack> sends;

        //消息处理
        public NetHandler handler;
        public NetLogicMain logicMain;

        public Server(string ip, int point)
        {
            this.ip = ip;
            this.point = point;
            recives = new List<ReciveItem>();
            sends = new Queue<SendPack>();

            handler = new NetHandler(AddSend);
            logicMain = new NetLogicMain();
            handler.SetLogicServer(logicMain);
        }

        public List<ReciveItem> GetRecives()
        {
            return recives;
        }

        /// <summary>
        /// 开启服务器
        /// </summary>
        public void Start()
        {
            //清空接收列表
            for (int i = 0; i < recives.Count; i++)
            {
                recives[i].Close();
            }
            recives.Clear();

            //关闭客户端接入监听线程
            if (threadListener != null)
            {
                isLoop = false;
                threadListener.Abort();
                threadListener = null;
            }
            //关闭发送线程
            if (threadSend != null)
            {
                threadSend.Abort();
                threadSend = null;
            }
            //停止监听
            if (tcpListener != null)
            {
                tcpListener.Stop();
                tcpListener = null;
            }
            //停止心跳
            if (monitor != null)
            {
                monitor.Close();
                monitor = null;
            }
            //启动监听线程
            threadListener = new Thread(ListenerClient);
            isLoop = true;
            threadListener.Start();
            //启动发送线程
            threadSend = new Thread(SendPack);
            threadSend.Start();
            //启动监听心跳线程
            monitor = new MonitorHeartBeat(this);
            monitor.Start();
        }

        /// <summary>
        /// 监听客户端接入
        /// </summary>
        private void ListenerClient()
        {
            try
            {
                var serverIPEndPoint = new IPEndPoint(IPAddress.Parse(ip), point);
                tcpListener = new TcpListener(serverIPEndPoint);
                tcpListener.Start();
                Console.WriteLine("服务器已开启");
                while (isLoop)
                {
                    var tcpClient = tcpListener.AcceptTcpClient();
                    if (tcpClient != null)
                    {
                        var client = new ClientItem();
                        client.tcpClient = tcpClient;
                        client.stream = tcpClient.GetStream();
                        client.clientMsg.Append(tcpClient.Client.RemoteEndPoint.ToString());
                        var recive = new ReciveItem(ReceivePack, client);
                        recive.Start();
                        lock (recive)
                        {
                            recives.Add(recive);
                        }      
                        Console.WriteLine(client.clientMsg + "，已连接.");               
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("监听客户端接入:" + ex.Message);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 发送队列
        /// </summary>
        private void AddSend(ClientItem client, Pack pack)
        {
            SendPack sp = new SendPack(client, pack);
            lock (sends)
            {
                sends.Enqueue(sp);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        private void SendPack()
        {
            try
            {
                while (isLoop)
                {
                    lock (sends)
                    {
                        if (sends.Count > 0)
                        {
                            var sp = sends.Dequeue();
                            if (sp.client.tcpClient.Connected)
                            {
                                var bytes = sp.pack.GetBytes();
                                if (bytes.Length < 4)
                                {
                                    throw new ArgumentException("包大小不对");
                                }
                                //加密包
                                var data = DesCipher.GetInstance().DesEncrypt(bytes);
                                sp.client.stream.Write(data, 0, data.Length);
                                sp.client.lastSendTime = Tools.GetMilliseconds();
                                Console.WriteLine("发送 " + sp.client.tcpClient.Client.RemoteEndPoint + ":[main]" + sp.pack.main + "[sub]" + sp.pack.sub + ",[len]" + (bytes.Length-4));
                            }
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送消息:" + ex.Message);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        private void ReceivePack(ReciveItem item)
        {
            lock (item)
            {
                var client = item.client;
                if (client.tcpClient.Client.Poll(-1, SelectMode.SelectRead))
                {
                    try
                    {
                        int length = client.tcpClient.Client.Receive(item.data);
                        if (length == 0) //连接已断开
                        {
                            throw new ArgumentException("客户端断开连接");
                        }
                        //解密包
                        var data = item.data.Skip(0).Take(length).ToArray();
                        data = DesCipher.GetInstance().DesDecrypt(data);
                        length = data.Length;
                        if (length < 4)
                        {
                            throw new ArgumentException("包大小不对");
                        }
                        item.lastReceiveTime = Tools.GetMilliseconds(); 
                        Pack pack = new Pack(data);
                        Console.WriteLine("接收 " + client.clientMsg + ":[main]" + pack.main + "[sub]" + pack.sub + ",[len]" + pack.data.Length);
                        handler.SendHandler(EventDefine.SOCKET_RECIVE, client, pack);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("接收消息:" + client.clientMsg + " 断开连接!" + ex.ToString());
                        item.Close();
                        lock (recives)
                        {
                            recives.Remove(item);
                        }
                        return;
                    }
                }
            }
        }
    }
}
