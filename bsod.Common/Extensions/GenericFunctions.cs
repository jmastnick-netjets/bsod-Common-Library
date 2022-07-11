using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using bsod.Common.DataAnnotations;

namespace bsod.Common.Extensions
{
    public static class GenericFunctions
    {
        /// <summary>
        /// Returns the public and instance properties in the given generic class, excluding by default the properties marked hidden.
        /// </summary>
        /// <typeparam name="I"></typeparam>
        /// <param name="showHiden">if true returns the properties with the attribute hidden otherwise ignores those.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties<I>(bool showHiden = false)
        {
            var t = typeof(I);
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in props)
            {
                if (p.IsHidden())
                    continue;
                yield return p;
            }
        }
    }
}
