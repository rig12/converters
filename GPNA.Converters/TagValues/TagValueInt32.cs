using System;
using System.Collections.Generic;
using System.Text;

namespace GPNA.Converters.TagValues
{
    public class TagValueInt32 : TagValue
    {
        /// <summary>
        /// Значение тега
        /// </summary>
        public int? Value { get; set; }
    }
}