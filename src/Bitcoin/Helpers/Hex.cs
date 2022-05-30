
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public class Hex
    {
        public static byte[] toByteArray(string hexString)
        {
            hexString = hexString.PadLeft(2, '0');
            byte[] hexBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < hexBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                hexBytes[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber);
            }

            return hexBytes;
        }

        public static string fromByteArray(List<byte> byteArray)
        {
            var hexString = "";

            byteArray.ForEach(i => {
                hexString += i.ToString("X").PadLeft(2,'0');
             });

            return hexString;
        }

        public static string fromByteArray(byte[] byteArray)
        {
            return fromByteArray(byteArray.ToList());
        }
    }
}
