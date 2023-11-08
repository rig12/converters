namespace Converters.Helpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    #endregion Using

    /// <summary>
    /// Помощник работы с перечислениями
    /// </summary>
    public static class EnumHelpers
    {
        /// <summary>
        /// Получить описание значения перечисления
        /// </summary>
        /// <param name="value">Значение перечиаления</param>
        /// <returns>Возвращает описание перечисления если оно присутсвует, иначе само перечисление в строковом виде</returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[]? attributes = fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }
            return value.ToString();
        }

        /// <summary>
        /// Получить значения перечисления
        /// </summary>
        /// <typeparam name="TEnumType">Тип перечисления</typeparam>
        /// <returns>Возвращает коллекцию значений</returns>
        public static IEnumerable<TEnumType> GetValues<TEnumType>()
        {
            return Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>();
        }
    }
}
