using System;
using System.Linq;
using System.Numerics;

namespace Helpers
{
    public class Base58
    {
        private const string Base58Digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private static readonly char[] Base58Chars = Base58Digits.ToCharArray();
        private static readonly string EncodedMaxLong = Encode(long.MaxValue);

        public static string Encode(long @long)
        {
            var array = BitConverter.GetBytes(@long);
            var encoded = Encode(array);
            return encoded;
        }

        public static long DecodeLong(string encoded, bool truncate = false)
        {
            if (encoded.Length > EncodedMaxLong.Length && false == truncate)
                throw new ArgumentException($"{encoded} exceeds long.MaxValue : {EncodedMaxLong} and will truncate." +
                "Set truncate parameter to true to allow");

            var decoded = Decode(encoded);
            var @long = BitConverter.ToInt64(decoded, 0);

            return @long;
        }

        public static string Encode(byte[] data)
        {
            // Decode byte[] to BigInteger
            var intData = data.Aggregate<byte, BigInteger>(0, (current, @byte) => current * 256 + @byte);

            // Encode BigInteger to Base58 string
            var result = "";
            while (intData > 0)
            {
                var remainder = (int)(intData % 58);
                intData /= 58;
                result = Base58Digits[remainder] + result;
            }

            // Append `1` for each leading 0 byte
            for (var i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = '1' + result;
            }
            return result;
        }

        public static byte[] Decode(string encoded)
        {
            // Decode Base58 string to BigInteger 
            BigInteger intData = 0;
            for (var i = 0; i < encoded.Length; i++)
            {
                var digitValue = Array.BinarySearch(Base58Chars, encoded[i]);
                if (digitValue < 0)
                    throw new FormatException(string.Format("Invalid Base58 character `{0}` at position {1}", encoded[i], i));
                intData = intData * 58 + digitValue;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            var leadingZeroCount = encoded.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
            intData.ToByteArray()
            .Reverse() // to big endian
                      .SkipWhile(b => b == 0); //strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
            return result;
        }

        //const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        //const int Size = 25;

        //private static byte[] DecodeBase58(string input)
        //{
        //    var output = new byte[Size];
        //    foreach (var t in input)
        //    {
        //        var p = Alphabet.IndexOf(t);
        //        if (p == -1) throw new Exception("invalid character found");
        //        var j = Size;
        //        while (--j > 0)
        //        {
        //            p += 58 * output[j];
        //            output[j] = (byte)(p % 256);
        //            p /= 256;
        //        }
        //        if (p != 0) throw new Exception("address too long");
        //    }
        //    return output;
        //}

        //private static string EncodeBase58(int number)
        //{
        //    int baseCount = Alphabet.Length;
        //    string encoded = "";
        //    while (number >= baseCount)
        //    {
        //        int remainder = number % baseCount;
        //        number = (int)Math.Floor((decimal)number / baseCount);
        //        encoded = Alphabet[remainder].ToString() + encoded;
        //    }
        //    return encoded;
        //}
    }
}