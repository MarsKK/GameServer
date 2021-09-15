using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace NetPack
{
    public class Pack
    {
        public Int16 main = 0;
        public Int16 sub = 0;
        public byte[] data = new byte[0];

        public Pack(Int16 main, Int16 sub, byte[] data = null)
        {
            this.main = main;
            this.sub = sub;
            if (data != null)
            {
                this.data = data;
            }
        }

        public Pack(byte[] bytes)
        {
            Read(bytes);
        }

        public byte[] GetBytes()
        {
            var mData = DataManager.GetInstance().Get();
            mData.WriteInt16((Int16)main);
            mData.WriteInt16((Int16)sub);
            mData.WriteBytes(data);
            var bytes = mData.GetBytes();
            DataManager.GetInstance().Release(mData);
            return bytes;
        }

        public void Read(byte[] bytes)
        {
            var mData = DataManager.GetInstance().Get();
            mData.Read(bytes);
            main = mData.ReadInt16();
            sub = mData.ReadInt16();
            if (bytes.Length > 4)
            {
                data = bytes.Skip(4).Take(bytes.Length-4).ToArray();
            }
            DataManager.GetInstance().Release(mData);
        }

        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        //将Byte转换为结构体类型
        public static object ByteToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            //分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
    }
}
