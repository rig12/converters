namespace Converters.Interfaces
{
    #region Using
    using Model;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    #endregion Using

    /// <summary>
    /// Кэш объектов описаний
    /// </summary>
    public interface IObjectDescriptionCache
    {
        /// <summary>
        /// Получить описание свойства
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <returns>Возвращает объект описания в случае наличия свойства и атрибута <see cref="ColumnAttribute"/> на нем, иначе null</returns>
        ObjectDescription? Get(Type type, string propertyName);

        /// <summary>
        /// Получить описание объекта
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <returns>Возвращает коллекцию описаний свойств где есть атрибут <see cref="ColumnAttribute"/></returns>
        IEnumerable<ObjectDescription> Get(Type type);
    }
}
