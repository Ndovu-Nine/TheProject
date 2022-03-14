

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NdovuTisa
{
    public static class Text
    {
        /// <summary>
        /// Removes special characters
        /// </summary>
        /// <param name="str">dirty string</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            var sb = new StringBuilder();
            foreach (var c in str.Where(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
            {
                sb.Append(c);
            }
            return sb.ToString();
        }
        /// <summary>
        /// If you want a to z only
        /// </summary>
        /// <param name="str">dirty</param>
        /// <returns></returns>
        public static string ToLettersOnly(this string str)
        {
            if (str.Length != 0)
            {
                var sb = new StringBuilder();
                foreach (var c in str.Where(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                {
                    sb.Append(c);
                }
                return sb.ToString().Trim().Length != 0 ? sb.ToString() : null;
            }
            else
            {
                return null;
            }
        }
        public static string ToExactLength(this string source, int maxLength, char a = ' ')
        {
            if (source.Length == maxLength)
            {
                return source;
            }
            if (source.Length < maxLength)
            {
                for (var t = source.Length; t < maxLength; t++)
                {
                    source += a;
                }
                return source;
            }

            return source.Substring(0, maxLength);
        }
        /// <summary>
        /// if you want 0 to 9 only
        /// </summary>
        /// <param name="str">dirty</param>
        /// <returns></returns>
        public static string ToNumbersOnly(this string str)
        {
            if (str.Length != 0)
            {
                var sb = new StringBuilder();
                foreach (var c in str.Where(c => (c >= '0' && c <= '9')))
                {
                    sb.Append(c);
                }
                return sb.ToString().Trim().Length != 0 ? sb.ToString() : null;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Turns the first letter after space to capital
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string ToTitleCase(this string str)
        {
            if (str == null) return "";
            return new string(CharsToTitleCase(str.ToLower().Trim(),' ').ToArray());
        }
        /// <summary>
        /// Turns the first letter after fullstop to capital
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string ToSentenceCase(this string str)
        {
            if (str == null) return "";
            return new string(CharsToTitleCase(str.ToLower(),'.').ToArray());
        }
        /// <summary>
        /// if you want a hex string
        /// </summary>
        /// <param name="original">normal string</param>
        /// <returns></returns>
       public static string ToHex(this string original)
       {
           string levelOneEncryption = null;
            var values = original.ToCharArray();
            foreach (var value in values.Select(Convert.ToInt32))
            {
                if (levelOneEncryption == null)
                {
                    levelOneEncryption += $"{value:X}";
                }
                else
                {
                    levelOneEncryption += "-" + $"{value:X}";
                }
            }
            return levelOneEncryption;
        }
        public static bool IsEmailValid(this string email)
        {
            var ck = email.Split('@');
            if (ck.Length == 2)
            {
                var dk = ck[1].Split('.');
                return dk.Length >= 2;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if a string is empty or null
        /// </summary>
        /// <param name="str">string you want to check</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            if (str == null)
            {
                return true;
            }

            if (str.Length <= 0)
            {
                return true;
            }
            return false;
        }
        private static IEnumerable<char> CharsToTitleCase(string s, char nw)
        {
            var newWord = true;
            foreach (var c in s)
            {
                if (newWord)
                {
                    yield return char.ToUpper(c);
                    newWord = false;
                }
                else
                {
                    yield return char.ToLower(c);
                }
                if (c == nw)
                {
                    newWord = true;
                }
            }
        }
    }
}
