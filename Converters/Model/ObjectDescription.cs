namespace Converters.Model
{
    #region Using
    #endregion Using

    /// <summary>
    /// Класс описания объекта
    /// </summary>
    public class ObjectDescription
    {
        /// <summary>
        /// Наименование свойства
        /// </summary>
        public string PropertyName { get; set; } = string.Empty;

        /// <summary>
        /// Наименование столбца
        /// </summary>
        public string ColumnName { get; set; } = string.Empty;

        /// <summary>
        /// Наименование типа
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Флаг ключевого поля
        /// </summary>
        public bool IsKey { get; set; }
    }
}
