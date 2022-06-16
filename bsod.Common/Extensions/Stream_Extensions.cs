using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class Stream_Extensions
    {
        /// <summary>
        /// Reads the Stream into a buffer and returns the buffer.
        /// </summary>
        /// <param name="Data">Stream to read into the buffer.</param>
        public static byte[] GetBytes(this Stream Data)
        {
            byte[] buffer = null;
            buffer = new byte[Data.Length];
            Data.Read(buffer, 0, (int)Data.Length);
            return buffer;
        }
        /// <summary>
        /// Converts Bytes Array to a Memory Stream, if key given will decrypt bytes.
        /// </summary>
        /// <param name="Bytes">Bytes to convert.</param>
        public static Stream ConvertBytesToStream(this byte[] Bytes) //, byte[] key = null)
        {
            //if(key != null)
            //    Bytes = decryptBytes(Bytes, key);
            MemoryStream s = new MemoryStream();
            s.Write(Bytes, 0, Bytes.Length);
            s.Seek(0, SeekOrigin.Begin);
            return s;
        }


        /// <summary>
        /// Reads Bytes Array into stream given
        /// </summary>
        /// <param name="bytes">Bytes Array to Read into Stream</param>
        /// <param name="stream">Stream to Read Bytes Array into</param>
        public static void ToStream(this byte[] bytes, Stream stream)
        {
            try
            {
                if (bytes == null) { throw new ArgumentNullException("bytes"); }
                if (stream == null) { throw new ArgumentNullException("stream"); }
                using (BinaryWriter w = new BinaryWriter(stream)) { w.Write(bytes); }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
