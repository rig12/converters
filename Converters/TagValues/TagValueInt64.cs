using System;
using System.Collections.Generic;
using System.Text;

namespace Converters.TagValues
{
    /// <summary>
    /// Значение тега целочисленного типа
    /// </summary>
    public class TagValueInt64 : TagValue
    {
        /// <summary>
        /// Значение тега
        /// </summary>
        public long? Value { get; set; }
    }
}
