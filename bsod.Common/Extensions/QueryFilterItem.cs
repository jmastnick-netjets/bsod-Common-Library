using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bsod.Common.Extensions
{
    public enum QueryFilterCondition
    {
        [Display(Name = "Equals")]
        Eq,
        [Display(Name = "Not Equals")]
        Neq,
        [Display(Name = "Greater Than")]
        Gt,
        [Display(Name = "Greater Than Or Equals")]
        Ge,
        [Display(Name = "Less Than")]
        Lt,
        [Display(Name = "Less Than Or Equals")]
        Le,
        [Display(Name = "Like")]
        Like,
        [Display(Name = "Not Like")]
        NotLike
    }
    public class QueryFilterItem
    {
        public const string AllOption = "--all--";
        public string Field { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
