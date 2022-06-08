using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class other_Extensions
    {


        /// <summary>
        /// Disposes Object if not null
        /// </summary>
        /// <param name="objToDispose">Object to Dispose of</param>
        public static void DisposeIfNotNull(this IEnumerable<IDisposable> objToDispose)
        {
            for (int i = 0; i < objToDispose.Count(); i++)
            {
                objToDispose.ElementAt(i).DisposeIfNotNull();
            }
        }

        /// <summary>
        /// Disposes Object if not null
        /// </summary>
        /// <param name="objToDispose">Object to Dispose of</param>
        public static void DisposeIfNotNull(this IDisposable objToDispose)
        {
            if (objToDispose != null)
                objToDispose.Dispose();
            objToDispose = null;
        }
    }
}
