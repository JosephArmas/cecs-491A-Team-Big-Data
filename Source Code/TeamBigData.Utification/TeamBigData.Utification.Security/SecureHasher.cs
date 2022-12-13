using System.Security.Cryptography;
using System.Text;

namespace TeamBigData.Utification.Security
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
    }
}