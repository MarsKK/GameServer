using System;
using System.Text;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace CipherTool
{
    public class DesCipher {

        private static DesCipher instance;
        private byte[] key;
        private byte[] iv;
        private DESCryptoServiceProvider provider;
        private MemoryStream mEStream, mDStream;
        private CryptoStream eStream, dStream;

        public static DesCipher GetInstance()
        {
            if (instance == null)
            {
                instance = new DesCipher();
            }
            return instance;
        }

        public DesCipher()
        {
            key = Encoding.UTF8.GetBytes(CipherDefine.key);
            iv = Encoding.UTF8.GetBytes(CipherDefine.iv);
            provider = new DESCryptoServiceProvider();
        }

        public byte[] DesEncrypt(byte[] srcBytes)
        {
            mEStream = new MemoryStream();
            eStream = new CryptoStream(mEStream, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            eStream.Write(srcBytes, 0, srcBytes.Length);
            eStream.FlushFinalBlock();
            var bytes = mEStream.ToArray();
            eStream.Close();
            mEStream.Close();
            return bytes;
        }

        public byte[] DesDecrypt(byte[] deBytes)
        {
            mDStream = new MemoryStream();
            dStream = new CryptoStream(mDStream, provider.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            dStream.Write(deBytes, 0, deBytes.Length);
            dStream.FlushFinalBlock();
            var bytes = mDStream.ToArray();
            dStream.Close();
            mDStream.Close();
            return bytes;
        }

    }
}