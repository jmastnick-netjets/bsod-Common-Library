using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class byte_Extensions
    {
        /// <summary>
        /// Compare the values of two byte arrays. If the two arrays are exact returns true else it returns false.
        /// If both are null returns true, otherwise if one is null returns false.
        /// </summary>
        /// <param name="byteArray">Byte array to check.</param>
        /// <param name="compareByteArray">Byte array to compare to.</param>
        public static bool CheckEquality(this byte[] byteArray, byte[] compareByteArray)
        {
            if (byteArray == null && compareByteArray == null) return true;
            if (byteArray == null) return false;
            if (compareByteArray == null) return false;
            if (byteArray.Length != compareByteArray.Length) return false;
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (byteArray[i] != compareByteArray[i]) return false;
            }
            return true;
        }


        /// <summary>
        /// Converts Byte Array to secure string
        /// </summary>
        /// <param name="bytes">Byte Array to convert</param>
        /// <returns>SecureString that represents the bytes array</returns>
        public static SecureString BytesToSecureString(this byte[] bytes)
        {
            SecureString secStr = new SecureString();
            char[] chars = System.Text.Encoding.UTF8.GetChars(bytes);
            for (int idx = 0; idx < chars.Length; ++idx)
            {
                secStr.AppendChar(chars[idx]);
                // Clear out the chars as you go.
                chars[idx] = '\0';
            }

            // Clear the decrypted bytes from memory, too.
            Array.Clear(bytes, 0, bytes.Length);
            bytes = null;
            Array.Clear(chars, 0, chars.Length);
            chars = null;

            return secStr;
        }

        /// <summary>
        /// Converts Byte Array Into original string form.
        /// </summary>
        /// <param name="bytes">bytes array to convert</param>
        /// <returns>String value of Bytes Array</returns>
        public static string BytesToString(this byte[] bytes)
        {
            try
            {
                return ASCIIEncoding.ASCII.GetString(bytes);
            }
            catch (Exception e)
            {
                throw new Exception("Data was not converted. An Error Occurred. ", e);
            }

        }


        /*Converts a Byte Array to a string value of a byte array of the string, assumes delimiter is comma if none given*/
        /// <summary>
        /// Converts a Byte Array to a string value of a byte array of the string, assumes delimiter is comma if none given
        /// </summary>
        /// <param name="ByteArr">Bytes to convert</param>
        /// <returns>string value of a byte array of the string given</returns>
        public static string BytesToByteString(this byte[] ByteArr)
        {
            return BytesToByteString(ByteArr, ',');
        }
        /// <summary>
        /// Converts a Byte Array to a string value of a byte array of the string, assumes delimiter is comma if none given
        /// </summary>
        /// <param name="ByteArr">Bytes to convert</param>
        /// <param name="Delimiter">delimiter of the byte array</param>
        /// <returns>string value of a byte array given</returns>
        public static string BytesToByteString(this byte[] ByteArr, char Delimiter)
        {
            return BytesToByteString(ByteArr, Delimiter.ToString());
        }


        /// <summary>
        /// Converts a Byte Array to a string value of a byte array of the string, assumes delimiter is comma if none given
        /// </summary>
        /// <param name="ByteArr">Bytes to convert</param>
        /// <param name="Delimiter">delimiter of the byte array</param>
        /// <returns>string value of a byte array given</returns>
        public static string BytesToByteString(this byte[] ByteArr, string Delimiter)
        {
            try
            {
                if (string.IsNullOrEmpty(Delimiter.ToString())) { Delimiter = ","; }
                string outPut = ""; string d = "";
                foreach (byte byteVal in ByteArr)
                {
                    outPut += d + byteVal.ToString();
                    d = Delimiter.ToString();
                }

                return outPut;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /*Converts Byte array to a Hex Array String, assumes delimiter is nothing if none given*/
        /// <summary>
        /// Converts Byte array to a Hex Array String, assumes delimiter is nothing if none given
        /// </summary>
        /// <param name="ByteArr">Byte Array to convert to Hex Array</param>
        /// <returns>String value of the Byte array in Hex</returns>
        public static string BytesToHexString(this byte[] ByteArr)
        {
            return ByteArr.BytesToHexString(null);
        }

        /// <summary>
        /// Converts Byte array to a Hex Array String, assumes delimiter is nothing if none given
        /// </summary>
        /// <param name="ByteArr">Byte Array to convert to Hex Array</param>
        /// <param name="Delimiter">Delimiter of the Hex array</param>
        /// <returns>String value of the Byte array in Hex</returns>
        public static string BytesToHexString(this byte[] ByteArr, string Delimiter)
        {
            try
            {
                string hexVal = ""; string d = "";
                if (ByteArr.Length > 0)
                {
                    foreach (byte b in ByteArr)
                    {
                        hexVal += d + b.ToString("X2");
                        d = Delimiter;
                    }
                    return hexVal;
                }
                else
                {
                    throw new Exception("No Bytes in Byte Array to convert.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
