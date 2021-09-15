using System.Threading;
using NetPack;

namespace GameServer
{
    public class ReciveItem
    {
        public delegate void Recive(ReciveItem item);
        public event Recive recive;
        private Thread thread;
        private bool isRun;
        public ClientItem client;
        //上次收包时间
        public long lastReceiveTime = 0;
        public int maxLen = 1024*8;
        public byte[] data;

        public ReciveItem(Recive recive, ClientItem client)
        {
            this.client = client;
            this.recive = recive;
            data = new byte[maxLen];
            thread = new Thread(OnRecive);
            lastReceiveTime = Tools.GetMilliseconds();
        }

        public void OnRecive()
        {
            while (isRun)
            {
                recive.Invoke(this);
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
            client.Close();
        }
    }
}
