namespace Converters.Model
{
    #region Using
    using TagValues;
    #endregion Using

    /// <summary>
    /// Расширения тега
    /// </summary>
    public static class TagValueExtensions
    {
        /// <summary>
        /// Получить строковое значение тега
        /// </summary>
        /// <param name="tagvalue">Базовый объект тега</param>
        /// <returns>Возвращает строку</returns>
        public static string GetStringValue(this TagValue tagvalue)
        {
            var type = tagvalue.GetType();
            var prop = type.GetProperty("Value");
            var value = prop?.GetValue(tagvalue, null);

            return value?.ToString();
        }
    }
}
