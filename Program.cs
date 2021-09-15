using System;
using System.Text;
using System.IO;
using NetPack;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 13000);
            server.Start();
        }
    }
}
