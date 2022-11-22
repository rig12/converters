namespace GPNA.Converters.TagValues
{
    #region Using
    #endregion Using

    /// <summary>
    /// Динамический тег
    /// </summary>
    public class TagValueDynamic : TagValue
    {
        /// <summary>
        /// Динамическое значение тега
        /// </summary>
        public dynamic Value { get; set; }
    }
}