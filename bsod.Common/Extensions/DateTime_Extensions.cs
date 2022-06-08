using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public static class DateTime_Extensions
    {

        /// <summary>
        /// Gets the Average of a time span
        /// </summary>
        /// <param name="tme">TimeSpan to get the average from</param>
        /// <param name="totalDiv">Total to divide the TimeSpan from</param>
        /// <returns>the average of the TimeSpan</returns>
        public static TimeSpan Average(this TimeSpan tme, int totalDiv)
        {
            return new TimeSpan(tme.Ticks / totalDiv);
        }
    }
}
