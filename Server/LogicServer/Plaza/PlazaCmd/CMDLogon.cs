using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LogicServer.PlazaCmd
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CMD_C_Logon
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string account;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string password;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CMD_S_Logon
    {
        public int userID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string userName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string account;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string password;
    }
}
