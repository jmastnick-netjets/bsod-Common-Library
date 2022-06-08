using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common
{
    public static class Functions
    {
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        public static string IsNull(string IsNullStr)
        {
            return isNull(IsNullStr, "");
        }
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <param name="NullString">Alternate string</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        public static string IsNull(string IsNullStr, string NullString)
        {
            return isNull(IsNullStr, NullString);
        }
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        public static dynamic IsNull(object IsNullStr)
        {
            return isNull(IsNullStr, "");
        }
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <param name="NullString">Alternate string</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        public static dynamic IsNull(object IsNullStr, string NullString)
        {
            return isNull(IsNullStr, NullString);
        }
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <param name="NullString">Alternate string</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        private static dynamic isNull(object IsNullStr, string NullString)
        {
            if (IsNullStr == null || IsNullStr.GetType() == typeof(string))
            {
                return isNull((string)IsNullStr, NullString);
            }
            else if (IsNullStr == DBNull.Value)
            {
                return NullString;
            }
            else
            {
                return IsNullStr;
            }
        }
        /// <summary>
        /// Checks if the string is null or empty whitespace. If it is it returns the NullString, if none supplied then it returns an empty string.
        /// </summary>
        /// <param name="IsNullStr">String to check</param>
        /// <param name="NullString">Alternate string</param>
        /// <returns>if string is null or empty whitespace returns NullString else returns the string</returns>
        private static string isNull(string IsNullStr, string NullString)
        {
            if (string.IsNullOrWhiteSpace(IsNullStr))
            {
                return NullString;
            }
            else if (string.IsNullOrWhiteSpace(IsNullStr.Trim()))
            {
                return NullString;
            }
            else
            {
                return IsNullStr;
            }
        }

        /// <summary>
        /// Creates a Random Text String that can be Used as a Key for 
        /// ion
        /// </summary>
        /// <param name="maxSize">Max Size of String</param>
        /// <returns>Random Text String</returns>
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }


        /// <summary>
        /// Gets a Random Byte Array based on size given
        /// </summary>
        /// <param name="size">size of byte array to return</param>
        public static byte[] GetRandomBytes(int size)
        {
            int _saltSize = size;
            byte[] bytes = new byte[_saltSize];
            RNGCryptoServiceProvider.Create().GetBytes(bytes);
            return bytes;
        }
    }
}
