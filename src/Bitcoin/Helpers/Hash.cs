
using System.Security.Cryptography;

namespace Helpers
{
    public class Hash
    {
        public static byte[] Compute(byte[] bytes)
        {
            var hasher = new SHA256Managed();
            return hasher.ComputeHash(bytes);
        }
    }
}
