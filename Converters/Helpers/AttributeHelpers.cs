namespace GPNA.Converters.Helpers
{
    #region Using
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;
    #endregion Using

    /// <summary>
    /// Помощник работы с атрибутами
    /// </summary>
    public static class AttributeHelpers
    {
        /// <summary>
        /// Получить все описания атбирутов столбцов
        /// </summary>
        /// <typeparam name="TEntity">Тип объекта</typeparam>
        /// <returns>Возвраащет коллекцию описаний</returns>
        public static IEnumerable<PropertyDefinition> GetColumnDefinitions<TEntity>()
        {
            foreach (var propertyInfo in typeof(TEntity).GetProperties())
            {
                var attribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();
                if (attribute == default || string.IsNullOrEmpty(attribute.Name))
                {
                    continue;
                }
                yield return new PropertyDefinition()
                {
                    PropertyName = propertyInfo.Name,
                    AttributeName = attribute.Name
                };
            }
        }

        /// <summary>
        /// Получить наименование таблицы по типу
        /// </summary>
        /// <typeparam name="TEntity">Тип объекта</typeparam>
        /// <returns>Возвраащет наименование если оно существует, иначе null</returns>
        public static string? GetTableName<TEntity>()
        {
            return GetTableName(typeof(TEntity));
        }

        /// <summary>
        /// Получить наименование таблицы по типу
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <returns>Возвраащет наименование если оно существует, иначе null</returns>
        public static string? GetTableName(Type type)
        {
            var attribute = type.GetCustomAttribute<TableAttribute>(false);
            return attribute != default ? attribute.Name : default;
        }

        /// <summary>
        /// Получить значение свойства по имени
        /// </summary>
        /// <param name="entity">Объект источник</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <returns>Возвращает значение если оно сущетсвует, иначе null</returns>
        public static object? GetPropertyValue<TEntity>(TEntity entity, string propertyName)
        {
            var propertyInfo = typeof(TEntity).GetProperty(propertyName);
            if (propertyInfo == default)
            {
                return default;
            }
            var value = propertyInfo.GetValue(entity, null);
            return propertyInfo.PropertyType.IsEnum && value != default ? (int)value
                : propertyInfo.GetValue(entity, null);
        }
    }
}
