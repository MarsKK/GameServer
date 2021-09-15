using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicServer.Plaza.PlazaCmd
{
    public class CmdDefine
    {
        //---------------------服务端命令---------------------------
        public const int MAIN_S_LOGON = 100;
        //----------------------------------------------------------

        //---------------------客户端命令---------------------------
        public const int MAIN_C_LOGON = 1;
        //----------------------------------------------------------

        //---------------------子命令-------------------------------
        public const int SUB_LOGON_ACCOUNT = 1;
        //----------------------------------------------------------
    }
}
