using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class string_Extensions
    {

        /// <summary>
        /// Checks if Array Contains the Exact String, not case sensitive
        /// </summary>
        /// <param name="str">String Array to Check</param>
        /// <param name="find">String to find</param>
        /// <returns>True if Array Contains String</returns>
        public static bool ContainsExact(this IEnumerable<string> str, string find)
        {
            return ContainsExact(str, find, false);
        }
        /// <summary>
        /// Checks if Array Contains the Exact String, Case Sensitive if CaseSensitive is true
        /// </summary>
        /// <param name="str">String Array to Check</param>
        /// <param name="find">String to find</param>
        /// <param name="CaseSensitive">Makes Search Case Sensitive</param>
        /// <returns>True if Array Contains String</returns>
        public static bool ContainsExact(this IEnumerable<string> str, string find, bool CaseSensitive)
        {
            try
            {
                if (str != null && str.Count() > 0)
                {
                    int cnt = str.Count();
                    for (int i = 0; i < cnt; i++)
                    {
                        string s = str.ElementAt(i);
                        if (CaseSensitive && s == find)
                        {
                            return true;
                        }
                        else if (s.ToLower() == find.ToLower())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, bool StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, int StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, char StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, object StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, float StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, double StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, decimal StringToRemove)
        {
            return RemoveString(str, StringToRemove.ToString());
        }
        /// <summary>
        /// Removes StringToRemove from string
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="StringToRemove">String To Remove</param>
        /// <returns>String with StringToRemove Removed</returns>
        public static string RemoveString(this string str, string StringToRemove)
        {
            try
            {
                StringBuilder retStr = new StringBuilder(str);
                if (!String.IsNullOrWhiteSpace(str) && !String.IsNullOrWhiteSpace(StringToRemove))
                {
                    retStr.Clear();
                    char[] oStr = str.ToCharArray();
                    char[] rStr = StringToRemove.ToCharArray();
                    int length = oStr.Length;
                    int rLength = rStr.Length;
                    for (int i = 0; i < length; i++)
                    {
                        char cStr = str[i];
                        int pLength = i;
                        int nLength = i;
                        bool found = false;
                        int ti = i;
                        for (int r = 0; r < rLength; r++)
                        {
                            char cRStr = rStr[r];
                            if (cRStr == cStr)
                            {
                                ti++;
                                if (ti >= length)
                                {
                                    nLength = ti;

                                    break;
                                }
                                cStr = str[ti];
                                if (r + 1 >= rLength)
                                {
                                    nLength = ti - 1;
                                    found = true;
                                    break;
                                }
                            }
                            else
                            {
                                cStr = str[i];
                                break;
                            }
                        }
                        if (nLength <= pLength && !found)
                        {
                            retStr.Append(cStr);
                        }
                        i = nLength;
                    }

                }
                return retStr.ToString(); ;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Will Convert String to an integer value representing the summed up value of the ASCII Values.
        /// </summary>
        /// <param name="str">String to Convert to integer.</param>
        /// <returns>Integer value representing the summed up value of the given string.</returns>
        public static int ConvertToASCIISum(this string str)
        {
            byte[] strVals = Encoding.ASCII.GetBytes(str);
            int res = BitConverter.ToInt32(strVals, 0);
            return res;
        }

        /// <summary>
        /// Returns a converted string from UTF8 to Unicode.
        /// </summary>
        /// <param name="input">String to change.</param>
        private static string Utf8ToUnicode(string input)
        {
            return Encoding.UTF8.GetString(input.Select(item => (byte)item).ToArray());
        }


        /// <summary>
        /// Converts secure string to unsecure byte array.
        /// </summary>
        /// <param name="SecurePassword">String to Convert</param>
        /// <returns>unsecure version of string</returns>
        public static byte[] ConvertToUnsecureBytes(this SecureString SecurePassword)
        {
            return ConvertToUnsecureString(SecurePassword).StringToBytes();
        }

        /// <summary>
        /// Converts secure string to unsecure char array
        /// </summary>
        /// <param name="SecurePassword">String to Convert</param>
        /// <returns>unsecure version of string</returns>
        public static char[] ConvertToUnsecureChars(this SecureString SecurePassword)
        {
            return ConvertToUnsecureString(SecurePassword).ToCharArray();
        }
        /// <summary>
        /// Converts secure string to unsecure string
        /// </summary>
        /// <param name="SecurePassword">String to Convert</param>
        /// <returns>unsecure version of string</returns>
        public static string ConvertToUnsecureString(this SecureString SecurePassword)
        {
            if (SecurePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(SecurePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /*Converts string to Byte Array*/
        /// <summary>
        /// Converts string to Byte Array
        /// </summary>
        /// <param name="StringToConvert">String to Convert</param>
        /// <returns>Byte Array of string</returns>
        public static byte[] StringToBytes(this string StringToConvert)
        {
            return ASCIIEncoding.ASCII.GetBytes(StringToConvert);
        }

        /*Converts a string to a byte array that represents the string then retuns a string 
         * value that represents the byte array, assumes delimiter is a comma if none given*/
        #region StringToByteString Overrides
        /// <summary>
        /// Converts a string to a byte array that represents the string then retuns a string value that represents the byte array, assumes delimiter is a comma if none given
        /// </summary>
        /// <param name="Str">string to convert</param>
        /// <returns>string value of a byte array of the string given</returns>
        public static string StringToByteString(this string Str)
        {
            return StringToByteString(Str, ',');
        }
        #endregion StringToByteString Overrides
        /// <summary>
        /// Converts a string to a byte array that represents the string then retuns a string value that represents the byte array, assumes delimiter is a comma if none given
        /// </summary>
        /// <param name="Str">string to convert</param>
        /// <param name="Delimiter">delimiter of the byte array</param>
        /// <returns>string value of a byte array of the string given</returns>
        public static string StringToByteString(this string Str, char Delimiter)
        {

            //To convert new password
            try
            {
                return Str.StringToBytes().BytesToByteString(Delimiter);

            }
            catch (Exception)
            {
                return null;
            }
        }

        /*Converts a String with Hex Array to a String Byte Array, assumes delimter is nothing if not set*/
        #region HexStringToByteString Overrides
        /// <summary>
        /// Converts a String with Hex Array to a String Byte Array
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <returns>Byte Array in string</returns>
        public static string HexStringToByteString(this string StringToParse)
        {
            try
            {
                return StringToParse.HexStringToByteString('\0');
            }
            catch (Exception) { return null; }
        }
        #endregion HexStringToByteString Overrides
        /// <summary>
        /// Converts a String with Hex Array to a String Byte Array
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <param name="Delimiter">Delimiter of byte array in string</param>
        /// <returns>Byte Array in string</returns>
        public static string HexStringToByteString(this string StringToParse, char Delimiter)
        {
            try
            {
                return StringToParse.HexStringToBytes(Delimiter).BytesToByteString();
            }
            catch (Exception) { return null; }
        }

        /*Converts String with Hex Array to a Byte Array, assumes delimter is nothing if not set*/
        #region HexStringToBytes Overrides
        /// <summary>
        /// Converts String with Hex Array to a Byte Array, assumes delimter is nothing if not set
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <returns>Byte Array in string</returns>
        public static byte[] HexStringToBytes(this string StringToParse)
        {
            try
            {
                return StringToParse.HexStringToBytes('\0');
            }
            catch (Exception) { return null; }
        }
        #endregion HexStringToBytes Overrides
        /// <summary>
        /// Converts String with Hex Array to a Byte Array, assumes delimter is nothing if not set
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <param name="Delimiter">Delimiter of byte array in string</param>
        /// <returns>Byte Array in string</returns>
        public static byte[] HexStringToBytes(this string StringToParse, char Delimiter)
        {
            try
            {
                StringToParse.Replace("#", "").Replace(Delimiter, '\0');
                int n = StringToParse.Length;
                byte[] bytes = new byte[n / 2];
                bytes[0] = Convert.ToByte(StringToParse.Substring(0, 2), 16);
                for (int i = 2; i < n; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(StringToParse.Substring(i, 2), 16);
                }
                return bytes;
            }
            catch (Exception) { return null; }
        }


        /*Converts String with Byte Array to a Byte Array, assumes delimter is a comma if not set*/
        /// <summary>
        /// Converts String with Byte Array to a Byte Array, assumes delimter is a comma if not set
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <returns>Byte Array in string</returns>
        public static byte[] ByteStringToBytes(this string StringToParse)
        {
            return StringToParse.ByteStringToBytes(',');
        }
        /// <summary>
        /// Converts String with Byte Array to a Byte Array, assumes delimter is a comma if not set
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <param name="Delimiter">Delimiter of byte array in string</param>
        /// <returns>Byte Array in string</returns>
        public static byte[] ByteStringToBytes(this string StringToParse, char Delimiter)
        {
            return StringToParse.ByteStringToBytes(Delimiter.ToString());
        }

        /// <summary>
        /// Converts String with Byte Array to a Byte Array, assumes delimter is a comma if not set
        /// </summary>
        /// <param name="StringToParse">String to Parse</param>
        /// <param name="Delimiter">Delimiter of byte array in string</param>
        /// <returns>Byte Array in string</returns>
        public static byte[] ByteStringToBytes(this string StringToParse, string Delimiter)
        {
            if (string.IsNullOrEmpty(Delimiter.ToString())) { Delimiter = ","; }
            string[] stringArray = StringToParse.Split(new string[] { Delimiter }, StringSplitOptions.None);
            byte[] byteArr = new byte[stringArray.Count()];
            int i = 0;
            foreach (string lne in stringArray)
            {
                byteArr[i++] = byte.Parse(lne);
            }
            return byteArr;
        }


        /// <summary>
        /// Returns the amount of bytes that the string takes up
        /// </summary>
        /// <param name="val">string to convert</param>
        /// <returns>Length of Bytes</returns>
        public static int GetBytesLength(this string val)
        {
            if (val == null) return 0;
            return val.Length * 8;
        }
    }
}
