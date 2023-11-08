namespace Converters.TagValues
{
    #region Using
    using System;
    #endregion Using

    /// <summary>
    /// Тег даты/времени
    /// </summary>
    public class TagValueDateTime : TagValue
    {
        /// <summary>
        /// Значение даты/времени
        /// </summary>
        public DateTime Value { get; set; }
    }
}
