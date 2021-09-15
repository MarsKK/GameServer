using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer;
using LogicServer.Plaza.PlazaCmd;
using LogicServer.PlazaCmd;
using NetEvent;
using NetPack;

namespace LogicServer.Plaza
{
    public class LogonLogic : NetLogic
    {
        public LogonLogic(NetHandler handler):base(handler)
        {

        }

        public override void DisperseHandler(EventNet ev)
        {
            int main = ev.pack.main;
            if (main == CmdDefine.MAIN_C_LOGON)
            {
                int sub = ev.pack.sub;
                switch (sub)
                {
                    case CmdDefine.SUB_LOGON_ACCOUNT:
                        OnLogonAccount(ev);
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnLogonAccount(EventNet ev)
        {
            CMD_C_Logon cmd = (CMD_C_Logon)Pack.ByteToStruct(ev.pack.data, typeof(CMD_C_Logon));
            Console.WriteLine("OnLogonAccount:" + cmd.account + "," + cmd.password);

            //生成消息体
            CMD_S_Logon sendCmd;
            sendCmd.userID = 100001;
            sendCmd.userName = "UserName";
            sendCmd.account = "a123456"; 
            sendCmd.password = "a123456";
            
            SendPack(ev.client, CmdDefine.MAIN_S_LOGON, CmdDefine.SUB_LOGON_ACCOUNT, sendCmd);
        }
    }
}
