namespace GPNA.Converters.TagValues
{
    #region Using
    #endregion Using

    /// <summary>
    /// Целочисленный тег
    /// </summary>
    public class TagValueInt32 : TagValue
    {
        /// <summary>
        /// Целочисленное значение тега
        /// </summary>
        public int? Value { get; set; }
    }
}