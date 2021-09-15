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
            Server server = new Server("172.16.12.44", 13000);
            server.Start();
        }
    }
}
