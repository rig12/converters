namespace Converters.Helpers
{
    #region Using
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System;
    using Model;
    using System.ComponentModel.DataAnnotations;
    #endregion Using
       
    /// <summary>
    /// Расширение для получения информации по объекту
    /// </summary>
    public static class ObjectDescriptionHelper
    {
        /// <summary>
        /// Получить описание свойств класса
        /// </summary>
        /// <returns>Возвращает коллекцию объектов описаний для свойств с атрибутом <see cref="ColumnAttribute"/></returns>
        public static IEnumerable<ObjectDescription> GetDescriptionOfProperties(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                var keyAttribute = property.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(KeyAttribute)) as KeyAttribute;
                var columnAttribute = property.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ColumnAttribute)) as ColumnAttribute;
                yield return new ObjectDescription()
                {
                    ColumnName = columnAttribute != default && !string.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : property.Name,
                    PropertyName = property.Name,
                    TypeName = property.PropertyType.Name,
                    IsKey = keyAttribute != default
                };
            }
        }

        /// <summary>
        /// Получить описание свойства класса
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <returns>Возвращает объект описания в случае наличия свойства и атрибута <see cref="ColumnAttribute"/> на нем, иначе null</returns>
        public static ObjectDescription? GetPropertyDescription(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == default)
            {
                return default;
            }
            var keyAttribute = propertyInfo.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(KeyAttribute)) as KeyAttribute;
            var columnAttribute = propertyInfo.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(ColumnAttribute)) as ColumnAttribute;
            return new ObjectDescription()
            {
                ColumnName = columnAttribute != default && !string.IsNullOrEmpty(columnAttribute.Name) ? columnAttribute.Name : propertyInfo.Name,
                PropertyName = propertyInfo.Name,
                TypeName = propertyInfo.PropertyType.Name,
                IsKey = keyAttribute != default
            };
        }

        /// <summary>
        /// Проверка содержит ли тип свойство с аттрибутом <see cref="KeyAttribute"/>
        /// </summary>
        /// <param name="type">Проверяемый тип</param>
        /// <returns>Возвращает true в случае успеха, иначе false</returns>
        public static bool IsContainsKey(Type type)
        {
            foreach (var propertyInfo in type.GetProperties())
            {
                var keyAttribute = propertyInfo.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(KeyAttribute)) as KeyAttribute;
                if (keyAttribute != default)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить значение свойства по имени
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="source">Источник</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <returns>Возвращает значение свойства в случае успеха, иначе Null</returns>
        public static object? GetPropertyValue<TEntity>(TEntity source, string propertyName)
        {
            if (source is null)
            {
                return default;
            }
            var propertyInfo = source.GetType().GetProperty(propertyName);
            return propertyInfo != default ? propertyInfo.GetValue(source) : default;
        }

        /// <summary>
        /// Установить значение по имени свойства
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="source">Источник</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <param name="value">Устанавливаемое значение</param>
        public static void SetPropertyValue<TEntity>(TEntity source, string propertyName, object? value)
        {
            if (source is null)
            {
                return;
            }
            var propertyInfo = source.GetType().GetProperty(propertyName);
            if (propertyInfo == default)
            {
                return;
            }
            propertyInfo.SetValue(source, value);
        }

        /// <summary>
        /// Получить описание таблицы класса
        /// </summary>
        /// <returns>Возвращает имя таблицы класса если существует, иначе имя класса</returns>
        public static string GetTableName(Type type)
        {
            var tableAttribute = type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            return tableAttribute != default ? tableAttribute.Name : type.Name;
        }
    }
}
