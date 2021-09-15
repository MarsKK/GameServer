using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NetPack
{
    public class Data {
        private MemoryStream ms;
        private int offset = 0;
        public int recycleMaxLen = 1024;

        public Data()
        {
            ms = new MemoryStream();
        }

        ~Data()
        {
            ms.Close();
        }

        public MemoryStream GetMemoryStream()
        {
            return ms;
        }

        public void Clear()
        {
            var bytes = ms.GetBuffer();
            var bufferLen = bytes.Length;
            if (bufferLen >= recycleMaxLen) //内存缓冲区达到最大临界值，回收内存。
            {
                ms.Close();
                ms = new MemoryStream();
                Console.WriteLine("释放内存:"+bufferLen+"字节.");
            }
            else
            {
                for (int i = 0; i < bufferLen; i++)
                {
                    bytes[i] = 0;
                }
            }
            ms.Seek(0, SeekOrigin.Begin);
            offset = 0;
        }

        public byte[] GetBytes()
        {
            var bytes = ms.GetBuffer();
            return bytes.Skip(0).Take(offset).ToArray();
        }

        public int GetBytesLen(byte[] bytes)
        {
            int len = bytes.Length;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                {
                    len = i;
                    break;
                }
            }
            return len;
        }

        #region   读数据包
        public void Read(byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                return;
            }
            ms.Write(bytes);
            offset = 0;
        }

        private byte[] Read(int len)
        {
            var bytes = ms.GetBuffer();
            var buffer = new byte[len];
            Array.Copy(bytes, offset, buffer, 0, len);
            offset += len; 
            return buffer;
        }

        public byte ReadByte()
        {
            var buffer = Read(1);
            return buffer[0];
        }

        public int ReadInt()
        {
            var buffer = Read(4);
            return BitConverter.ToInt32(buffer);
        }

        public Int16 ReadInt16()
        {
            var buffer = Read(2);
            return BitConverter.ToInt16(buffer); 
        }

        public float ReadFloat()
        {
            var buffer = Read(4);
            return BitConverter.ToSingle(buffer);
        }

        public double ReadDouble()
        {
            var buffer = Read(8);
            return BitConverter.ToDouble(buffer);
        }

        public string ReadString(int len)
        {
            var buffer = Read(len);
            len = GetBytesLen(buffer);
            return Encoding.UTF8.GetString(buffer, 0, len);
        }
        #endregion

        #region   写入数据包
        public void WriteBytes(byte[] bytes)
        {
            if (bytes.Length == 0)
            {
                return;
            }
            ms.Write(bytes);
            offset += bytes.Length;
        }

        public void WriteByte(byte b)  
        {
            ms.WriteByte(b);
            offset += 1;
        }

        public void WriteInt(int i)
        {
            var bytes = BitConverter.GetBytes(i);
            ms.Write(bytes);
            offset += bytes.Length;
        }

        public void WriteInt16(Int16 i)
        {
            var bytes = BitConverter.GetBytes(i);
            ms.Write(bytes);
            offset += bytes.Length;
        }
        
        public void WriteFloat(float f)
        {
            var bytes = BitConverter.GetBytes(f);
            ms.Write(bytes);
            offset += bytes.Length;
        }

        public void WriteDouble(double d)
        {
            var bytes = BitConverter.GetBytes(d);
            ms.Write(bytes);
            offset += bytes.Length;
        }

        public void WriteString(string s, int len)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var strBytes = new byte[len];
            Array.Copy(bytes, 0, strBytes, 0, bytes.Length < len ? bytes.Length : len);
            ms.Write(strBytes);
            offset += strBytes.Length;
        }
        #endregion
    }
}