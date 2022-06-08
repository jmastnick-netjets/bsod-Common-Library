using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
