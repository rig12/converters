namespace Converters.Model
{
    #region Using
    #endregion Using

    /// <summary>
    /// Класс описания свойства
    /// </summary>
    public class PropertyDefinition
    {
        /// <summary>
        /// Наименование свойства
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Наименование атрибута
        /// </summary>
        public string AttributeName { get; set; } = string.Empty;
    }
}
