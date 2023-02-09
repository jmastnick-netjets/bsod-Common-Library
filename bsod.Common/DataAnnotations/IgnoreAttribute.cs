using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.DataAnnotations
{
    public class IgnoreAttribute : Attribute
    {
        public bool ShowTable { get; set; }
        public bool ShowList { get; set; }
        public bool ShowExport { get; set; }
        public IgnoreAttribute()
        {

        }
    }
}
