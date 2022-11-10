using System.Security.Cryptography;
using System.Text;

namespace TeamBigData.Utification.Security
{
    public class SecureHasher
    {
        public static String HashString(String message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            HashAlgorithm sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(messageBytes);
            String hashedMessage = BitConverter.ToString(hash);
            return hashedMessage;
        }
    }
}