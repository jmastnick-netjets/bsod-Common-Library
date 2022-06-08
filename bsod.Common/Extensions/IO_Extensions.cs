using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class IO_Extensions
    {
        /// <summary>
        /// Checkes if File is locked, by attempting to open file. If File open failes with an IOException and the HR for the Exception returns an errorCode 32 or 33 will return true
        /// otherwise it returns false.
        /// </summary>
        /// <param name="fle">File to check if locked.</param>
        public static bool IsFileLocked(this FileInfo fle)
        {
            try
            {
                if (!fle.Exists)
                    throw new Exception($"File does not exist.\r\n\tFile Location {fle.FullName}");
                using (fle.Open(FileMode.Open)) { }
            }
            catch (IOException ex)
            {
                int errorCode = Marshal.GetHRForException(ex) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
    }
}
