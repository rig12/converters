
using GPNA.Converters.TagValues;
using System;

namespace GPNA.Converters.Interfaces
{
    /// <summary>
    /// Интерфейс преобразования значений тегов
    /// </summary>
    public interface ITagValueConverter
    {
        /// <summary>
        /// Преобразование json к значению тега
        /// </summary>
        /// <param name="json">Входной объект в формате json</param>
        /// <returns>Возвращает базовый объект значения тега</returns>
        TagValue GetTagValue(string json);

        /// <summary>
        /// возвращает значение типа, соответствующего реальному значению в строке 
        /// </summary>
        /// <param name="valueString">значение в виде строки</param>
        /// <param name="datetimeParseFormat">шаблон парсинга значения <seealso cref="valueString"/> если внутри дата-время> </param>
        /// <returns></returns>
        dynamic? GetValue(string valueString, string datetimeParseFormat = "yyyy-MM-dd HH:mm");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <param name="tagname"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        TagValue GetTagValue(string value, DateTime dateTime, string tagname, int quality);
    }
}
