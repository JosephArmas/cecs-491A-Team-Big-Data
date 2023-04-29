using System.Security.Cryptography;
using System.Text;

namespace TeamBigData.Utification.Security
{
    public class Encryptor
    {
        private readonly byte[] _iv;
        private readonly byte[] _key;
        public Encryptor()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateIV();
                aes.GenerateKey();
                _iv = aes.IV;
                _key = aes.Key;
            }
        }

        public byte[] encryptString(String message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encrypted;
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                using (var encryptor = aes.CreateEncryptor())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(message);
                            }
                            encrypted = memoryStream.ToArray();
                        }
                    }
                }
            }
            return encrypted;
        }

        public String decryptString(byte[] message)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                using (var decryptor = aes.CreateDecryptor())
                {
                    using (var memoryStream = new MemoryStream(message))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}