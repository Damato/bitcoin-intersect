
using Helpers;
using System.Linq;
using System.Security.Cryptography;

using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System.Threading.Tasks;
using System;

namespace Wallet
{
    public class PublicKey
    {
        private static X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
        private static ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        private static SecureRandom secureRandom = new SecureRandom();
        
        public static async Task<string> GeneratePublicKey(string privateKey, bool compressed)
        {
            return await Task.Run(() =>
            {
                BigInteger d = new BigInteger(Hex.toByteArray(privateKey));
                Org.BouncyCastle.Math.EC.ECPoint q = domain.G.Multiply(d);

                var publicParams = new ECPublicKeyParameters(q, domain);
                var encoded = publicParams.Q.GetEncoded();

                var publicKey = Hex.fromByteArray(encoded.ToList());

                if (compressed)
                {
                    var prefix = Hex.toByteArray(publicKey).Last() > 0 ? "02" : "03";
                    publicKey = $"{ prefix }{ publicKey.Substring(2, 64) }";
                }

                return publicKey;
            });
        }

        public static async Task<Tuple<BigInteger,BigInteger>> GenerateCurvePoints(string privateKey, bool compressed)
        {
            return await Task.Run(() =>
            {
                BigInteger d = new BigInteger(Hex.toByteArray(privateKey));
                Org.BouncyCastle.Math.EC.ECPoint q = domain.G.Multiply(d);

                var publicParams = new ECPublicKeyParameters(q, domain);
                var encoded = publicParams.Q.GetEncoded();

                BigInteger x = publicParams.Q.XCoord.ToBigInteger();
                BigInteger y = publicParams.Q.YCoord.ToBigInteger();

                return new Tuple<BigInteger, BigInteger>(x, y);
            });
        }

        public static async Task<string> GenerateBTCAddress(string publicKey)
        {
            return await Task.Run(() =>
            {
                var SHA256 = new SHA256Managed().ComputeHash(Hex.toByteArray(publicKey));
                var SHA256str = Hex.fromByteArray(SHA256.ToList());

                var RIPEMD160 = new RIPEMD160Managed().ComputeHash(SHA256);
                var RIPEMD160str = $"00{ Hex.fromByteArray(RIPEMD160.ToList()) }";

                var version = Hex.toByteArray(RIPEMD160str);

                var SHA256k1 = new SHA256Managed().ComputeHash(version);
                var SHA256k1str = Hex.fromByteArray(SHA256k1.ToList());

                var SHA256k2 = new SHA256Managed().ComputeHash(SHA256k1);
                var SHA256k2str = Hex.fromByteArray(SHA256k2.ToList());

                var checksum = SHA256k2str.Substring(0, 8);

                var address = $"{ RIPEMD160str }{ checksum }";

                var bitcoinAddress = Base58.Encode(Hex.toByteArray(address));

                //ValidateBitcoinAddress(bitcoinAddress);

                return bitcoinAddress;
            });
        }

        public static bool ValidateBitcoinAddress(string address)
        {
            if (address.Length < 26 || address.Length > 35) throw new System.Exception("wrong length");
            var decoded = Base58.Decode(address);
            var d1 = new SHA256Managed().ComputeHash(decoded.SubArray(0, 21));
            var d2 = new SHA256Managed().ComputeHash(d1);
            if (!decoded.SubArray(21, 4).SequenceEqual(d2.SubArray(0, 4))) throw new System.Exception("bad digest");
            return true;
        }
    }
}
