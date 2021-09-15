using System.Net.Sockets;
using System.Text;
using NetPack;

namespace GameServer
{
    public class ClientItem
    {
        public TcpClient tcpClient;
        public NetworkStream stream;
        //客户端信息
        public StringBuilder clientMsg = new StringBuilder("");
        //上次发包时间
        public long lastSendTime = 0;

        public ClientItem()
        {
            lastSendTime = Tools.GetMilliseconds();
        }

        public void Close()
        {
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
    }
}
