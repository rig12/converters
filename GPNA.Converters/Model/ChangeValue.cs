namespace GPNA.Converters.Model
{
    #region Using
    #endregion Using

    /// <summary>
    /// Базовое изменяемое значение 
    /// </summary>
    public class ChangeValue
    {
        /// <summary>
        /// Наименование изменяемого поля
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Значение
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// Наименование типа
        /// </summary>
        public string TypeName { get; set; } = string.Empty;

        /// <summary>
        /// Флаг модификации
        /// </summary>
        public bool IsModified { get; set; }

        /// <summary>
        /// Флаг ключевого поля
        /// </summary>
        public bool IsKey { get; set; }
    }
}
