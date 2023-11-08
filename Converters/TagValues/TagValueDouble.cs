namespace Converters.TagValues
{
    #region Using
    #endregion Using

    /// <summary>
    /// Значение тэга типа число с плавающей запятой
    /// </summary>
    public class TagValueDouble : TagValue
    {
        /// <summary>
        /// Значение с плавающей точкой
        /// </summary>
        public double? Value { get; set; }
    }
}
