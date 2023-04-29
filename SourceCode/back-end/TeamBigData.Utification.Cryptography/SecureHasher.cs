using System.Security.Cryptography;
using System.Text;

namespace TeamBigData.Utification.Cryptography
{
    public class SecureHasher
    {
        public static String HashString(String key, String message)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = HMACSHA512.HashData(keyBytes, messageBytes);
            String hashedMessage = BitConverter.ToString(hash);
            return hashedMessage;
        }

        public static String HashString(long key, String message)
        {
            byte[] keyBytes = BitConverter.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = HMACSHA512.HashData(keyBytes, messageBytes);
            String hashedMessage = BitConverter.ToString(hash);
            return hashedMessage;
        }
        public static String Base64Hash(String message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hash = SHA256.HashData(messageBytes);
            String hashedMessage = Convert.ToBase64String(hash);
            //remove the == at the end of base64 so just alpha numeric
            hashedMessage = hashedMessage.Substring(0, hashedMessage.Length - 2);
            return hashedMessage.Replace('/', 'a');
        }
    }
}
