namespace Converters.Model
{
    #region Using
    #endregion Using

    /// <summary>
    /// Конфигурация свойства
    /// </summary>
    public class PropertyConfiguration
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Индекс
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Флаг ключевого поля
        /// </summary>
        public bool IsKey { get; set; }
    }
}
