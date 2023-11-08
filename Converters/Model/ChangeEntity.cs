namespace Converters.Model
{
    #region Using
    using System.Collections.Generic;
    using System;
    using Enums;
    #endregion Using

    /// <summary>
    /// Изменяемая сущность
    /// </summary>
    public class ChangeEntity
    {
        /// <summary>
        /// Наименование изменяемой таблицы
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// Статус сущности
        /// </summary>
        public EntityStatus EntityStatus { get; set; }

        /// <summary>
        /// Коллекция изменяемых значений
        /// </summary>
        public ICollection<ChangeValue> Values { get; set; } = new List<ChangeValue>();

        /// <summary>
        /// Дата/время UTC события
        /// </summary>
        public DateTime DateTimeUtc { get; set; }

        /// <summary>
        /// Код сущности, в ктором перечисляются все ключевые поля через нижнее подчеркивание
        /// </summary>
        public string Code { get; set; } = string.Empty;
    }
}
