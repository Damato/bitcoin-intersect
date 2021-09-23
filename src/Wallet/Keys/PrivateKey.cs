
using System;
using System.Linq;
using Org.BouncyCastle.Security;

using Helpers;
using System.Threading.Tasks;
using System.Text;

namespace Wallet
{
    public class PrivateKey
    {
        public enum PrivateKeyType
        {
            Lowest = 0,
            Random = 1,
            Hightest = 2
        }

        public static async Task<string> GenerateBrainWalletKey(string brainWalletKey)
        {
            return await Task.Run(() =>
            {
                // Get the brain wallet.
                var contenderBytes = Encoding.ASCII.GetBytes(brainWalletKey.Trim());

                // Generate the hash.
                var address = Hash.Compute(contenderBytes);

                // Get in hex string format.
                var encode = Hex.fromByteArray(address.ToList());

                return encode;
            });
        }

        public static async Task<string> GeneratePrivateKey(PrivateKeyType option = PrivateKeyType.Random)
        {
            return await Task.Run(() =>
            { 
                // Create new randomizer.
                var r = new Random();

                // Create our new address struct.
                var address = new byte[32];

                // Based on requirements.
                if (option == PrivateKeyType.Random)
                {
                    // Our curve.
                    SecureRandom secureRandom = new SecureRandom();
                    secureRandom.NextBytes(address);
                }
                else if (option == PrivateKeyType.Lowest)
                {
                    // Set lowest possible.
                    address[31] = 1;
                }
                else if (option == PrivateKeyType.Hightest)
                {
                    // Set highest possible.
                    for (var i = 0; i <32; i++) address[i] = 255;

                    // Start should be half (max).
                    address[0] = 127;
                }

                // Check for max allowed starting byte of 127.
                if (address[0] > 0x7F) address[0] = (byte)r.Next(0, 127);

                // Get in hex string format.
                var encode = Hex.fromByteArray(address.ToList());

                return encode;
             });
        }

        public static async Task<string> GeneratePrivateKey(string wifAddress)
        {
            return await Task.Run(() =>
            {
                if (wifAddress.Substring(0, 1) == "5")
                {
                    if (wifAddress.Length != 51) throw new Exception("wrong length");
                }
                else if (wifAddress.Substring(0, 1) == "L" || wifAddress.Substring(0, 1) == "K")
                {
                    if (wifAddress.Length != 52) throw new Exception("wrong length");
                }
                else
                {
                    throw new Exception("incorrect format");
                }

                var byteAddress = Base58.Decode(wifAddress);

                var hexString = Hex.fromByteArray(byteAddress.ToList());

                if (hexString.Substring(0, 2) != "80") throw new Exception("incorrect identifier");

                var nochecksum = hexString.Substring(0, 66);
                var noidentifier = nochecksum.Substring(2, 64);

                return noidentifier.ToString();
            });
        }

        public static async Task<string> GenerateWIF(string hexAddress, bool compressed)
        {
            return await Task.Run(() =>
            {
                if (hexAddress.Length != 64) throw new Exception($"wrong length { hexAddress.Length }");

                var hexString = "80" + hexAddress;
                if (compressed) hexString += "01";

                byte[] hexAsBytes = Hex.toByteArray(hexString);

                var d1 = Hash.Compute(hexAsBytes);
                var d2 = Hash.Compute(d1);

                var hexChecksum = Hex.fromByteArray(d2.SubArray(0, 4).ToList());

                hexString += hexChecksum;
                byte[] byteAddress = Hex.toByteArray(hexString);

                return Base58.Encode(byteAddress);
            });
        }
    }
}
