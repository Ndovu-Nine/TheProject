using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NdovuTisa.Cryptography
{
    public static class Password
    {
        private static readonly char[] Letters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        };
        private static readonly char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly char[] Special = { '!', '@', '#', '$', '%', '^', '&', '*', '?', '_', '~', '-', '£', '(', ',', ')', '"', ' ' };
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        /// <summary>
        /// Nataka letters
        /// </summary>
        /// <returns>unapewa L Lutuce</returns>
        public static IEnumerable<char> GetLetters()
        {
            return Letters;
        }
        /// <summary>
        /// Nataka numbers
        /// </summary>
        /// <returns>unapewa 9 (Nine, Tisa)</returns>
        public static IEnumerable<char> GetNumbers()
        {
            return Numbers;
        }
        /// <summary>
        /// I want a millennial
        /// </summary>
        /// <returns>cool thick bitch</returns>
        public static IEnumerable<char> GetSpecialCharacters()
        {
            return Special;
        }
        public static string Hash(this string str)
        {
            var x1 = CreateMd5(str);
            return Sha512(x1);
        }
        /// <summary>
        /// Ranks the stringth of a password
        /// </summary>
        /// <param name="password">string</param>
        /// <returns>points fro strength</returns>
        public static PasswordScore Score(this string password)
        {
            var score = 0;
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 6)
                score++;
            if (password.Length >= 8)
                score++;
            score += PasswordContains(GetNumbers(), password);
            score += PasswordContains(GetLetters(), password);
            score += PasswordContains(GetSpecialCharacters(), password);
            return (PasswordScore)score;
        }
        public static string CreateMd5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (byte t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string Sha512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        private static int PasswordContains(IEnumerable<char> items, string password)
        {
            return items.Any(password.Contains) ? 1 : 0;
        }
    }
}
