using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LocalData
{
    internal class CryptFileHandler
    {
        public readonly string FilePath;
        private byte[] _key;
        private byte[] _iv;

        public CryptFileHandler(string filePath, byte[] key, byte[] iv)
        {
            FilePath = filePath;
            _key = key;
            _iv = iv;
        }

        public CryptFileHandler(string filePath, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            if (keyBytes.Length != 32)
            {
                throw new NotImplementedException($"Internal encryption error, 32 character password required.");
            }
            if (ivBytes.Length != 16)
            {
                throw new NotImplementedException($"Internal encryption error, 16 character password required.");
            }
            if (keyBytes == null || ivBytes == null)
            {
                throw new NotImplementedException($"Internal encryption error: 0X0000");
            }

            _key = keyBytes;
            _iv = ivBytes;
            FilePath = filePath;
        }

        public void SaveString(string data)
        {
            SaveData(Encoding.UTF8.GetBytes(data));
        }
        private void SaveData(byte[] data)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;

                using (FileStream fs = new FileStream(FilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public string LoadString()
        {
            byte[] data = LoadData();
            return Encoding.UTF8.GetString(data);
        }

        private byte[] LoadData()
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;

                using (FileStream fs = new FileStream(FilePath, FileMode.Open))
                {
                    using (CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] data = new byte[fs.Length];
                        cs.Read(data, 0, data.Length);
                        return data;
                    }
                }
            }
        }
    }
}
