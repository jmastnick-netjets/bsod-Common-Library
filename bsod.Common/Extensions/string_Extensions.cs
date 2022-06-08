using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
