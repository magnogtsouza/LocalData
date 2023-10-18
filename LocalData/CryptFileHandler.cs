using System;
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
            key = key.PadLeft(32, '0');
            key = key.Substring(0, 32);
            iv = iv.PadLeft(16, '0');
            iv = iv.Substring(0, 16);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            if (keyBytes.Length > 32)
            {
                Console.WriteLine("Excess bytes found: They are being removed from the encryption key...");
                Array.Resize(ref keyBytes, 32);
            }
            if (ivBytes.Length > 16)
            {
                Console.WriteLine("Excess bytes found: They are being removed from the encryption key...");
                Array.Resize(ref ivBytes, 16);
            }
            if (keyBytes == null || ivBytes == null || keyBytes.Length + ivBytes.Length != 48)
            {
                throw new NotImplementedException($"Internal encryption error:The amount of total bytes does not satisfy the requirements...\n required:{48}\n contains:{keyBytes.Length + ivBytes.Length}");
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
