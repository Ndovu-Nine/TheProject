using System;
using System.Linq;
using System.Text;

namespace NdovuTisa.Cryptography.Tea
{
    public class Tea
    {
        //Crypto key myst be 16 characters
        private readonly string _cryptoKey;
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();
        //private const short CryptoKeyLength = 16;
        private const short MinEncryptionLength = 6;


        public Tea(string cryptoKey)
        {
            //ensure that cryptoLength is 16 chars
            var cryptoLength = cryptoKey.Length;
            if (cryptoLength > 16)
            {
                cryptoKey = cryptoKey.Substring(0, 16);
            }
            else if (cryptoLength < 16)
            {
                cryptoKey = cryptoKey.PadRight(16, ' ');
            }

            _cryptoKey = cryptoKey;
        }

       


        /// <summary>
        /// Encryption using corrected Block TEA (xxtea) algorithm
        /// </summary>
        /// <param name="text">String to be encrypted (multi-byte safe)</param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            var textBytes = GetByteForEncryption(text);
            // Convert the text into UTF-8 encoding (byte size)
            var v = ToLongs(textBytes);

            // Simply convert first 16 chars of password as key
            var k = ToLongs(Encoding.GetBytes(_cryptoKey.Substring(0, 16)));

            // Use UInt32 as the original is based on 'unsigned long' in C, which is equiv to UInt32 in .Net (and not ulong)
            uint n = (uint)v.Length,
                   z = v[n - 1],
                   y = v[0],
                   delta = 0x9e3779b9,
                   e,
                   q = (uint)(6 + (52 / n)),
                   sum = 0,
                   p = 0;

            while (q-- > 0)
            {
                sum += delta;
                e = sum >> 2 & 3;

                for (p = 0; p < (n - 1); p++)
                {
                    y = v[(p + 1)];
                    z = v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                }

                y = v[0];
                z = v[n - 1] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
            }

            // Convert to Base64 so that Control characters doesnt break it
            return Convert.ToBase64String(ToBytes(v));
        }

        private static byte[] GetByteForEncryption(string text)
        {
            var bytes = Encoding.GetBytes(text);
            if (bytes.Length < MinEncryptionLength)
            {
                var incBytes = Enumerable.Repeat((byte)0, MinEncryptionLength).ToArray();
                Buffer.BlockCopy(bytes, 0, incBytes, 0, bytes.Length);
                bytes = incBytes;
            }
            return bytes;
        }

        /// <summary>
        /// Decryption using Corrected Block TEA (xxtea) algorithm
        /// </summary>
        /// <param name="encrypted">String to be decrypted</param>
        /// <returns> 
        /// - Empty string if the parameter is empty string.
        /// - Null if the Encrypted string is not Base64
        /// - Decrypted text if the parameter is valid
        /// </returns>
        public string Decrypt(string encrypted)
        {

            if (encrypted.Length == 0)
            { return ""; }
            try
            {
                var v = ToLongs(Convert.FromBase64String(encrypted));
                var k = ToLongs(Encoding.GetBytes(_cryptoKey.Substring(0, 16)));

                if (v.Length == 0)
                    return null;

                uint n = (uint)v.Length,
                       z = v[n - 1],
                       y = v[0],
                       delta = 0x9e3779b9,
                       e,
                       q = (uint)(6 + (52 / n)),
                       sum = q * delta,
                       p = 0;

                while (sum != 0)
                {
                    e = sum >> 2 & 3;

                    for (p = (n - 1); p > 0; p--)
                    {
                        z = v[p - 1];
                        y = v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
                    }

                    z = v[n - 1];
                    y = v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);

                    sum -= delta;
                }

                var plaintext = Encoding.GetString(ToBytes(v)).TrimEnd('\0');
                return plaintext;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// convert utf-8 byte to array of longs, each containing 4 chars to be manipulated
        /// </summary>
        /// <param name="s"></param>
        private static uint[] ToLongs(byte[] s)
        {

            // note chars must be within ISO-8859-1 (with Unicode code-point < 256) to fit 4/long
            var l = new uint[(int)Math.Ceiling(((decimal)s.Length / 4))];

            // Create an array of long, each long holding the data of 4 characters, if the last block is less than 4
            // characters in length, fill with ascii null values
            for (var i = 0; i < l.Length; i++)
            {
                // Note: little-endian encoding - endianness is irrelevant as long as it is the same in ToBytes()
                l[i] = ((s[i * 4])) +
                       ((i * 4 + 1) >= s.Length ? (UInt32)0 << 8 : ((UInt32)s[i * 4 + 1] << 8)) +
                       ((i * 4 + 2) >= s.Length ? (UInt32)0 << 16 : ((UInt32)s[i * 4 + 2] << 16)) +
                       ((i * 4 + 3) >= s.Length ? (UInt32)0 << 24 : ((UInt32)s[i * 4 + 3] << 24));
            }

            return l;
        }

        /// <summary>
        /// Convert array of longs back to utf-8 byte array
        /// </summary>
        /// <returns></returns>
        private static byte[] ToBytes(UInt32[] l)
        {
            var b = new byte[l.Length * 4];

            // Split each long value into 4 separate characters (bytes) using the same format as ToLongs()
            for (var i = 0; i < l.Length; i++)
            {
                b[(i * 4)] = (byte)(l[i] & 0xFF);
                b[(i * 4) + 1] = (byte)(l[i] >> (8 & 0xFF));
                b[(i * 4) + 2] = (byte)(l[i] >> (16 & 0xFF));
                b[(i * 4) + 3] = (byte)(l[i] >> (24 & 0xFF));
            }
            return b;
        }

    }
}
