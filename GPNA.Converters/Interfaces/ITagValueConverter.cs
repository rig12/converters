
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
        /// Возвращает значение типа, соответствующего реальному значению в строке 
        /// </summary>
        /// <param name="value">Значение в виде строки</param>
        /// <param name="datetimeParseFormat">Шаблон парсинга значения параметра value если внутри дата-время></param>
        /// <returns>Воозвращает значение у случае успеха, иначе null</returns>
        dynamic? GetValue(string value, string datetimeParseFormat = "yyyy-MM-dd HH:mm");

        /// <summary>
        /// вовзращает значение типа Double, если переданная строка может быть успешно преобразована в double
        /// для других типов - int->int.0; bool->0.0(false)/1.0(true); datetime->UnixTimestam(кол-во секунд от 01.01.1970)
        /// в остальных случаях - null
        /// </summary>
        /// <param name="valuestring"></param>
        /// <param name="datetimeParseFormat"></param>
        /// <returns></returns>
        double? GetDoubleValue(string valuestring, string datetimeParseFormat = "yyyy-MM-dd HH:mm");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <param name="tagname"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        TagValue GetTagValue(string value, DateTime dateTime, string tagname, int quality);

        /// <summary>
        /// получить структуру TagValue со строковым значением <seealso cref="TagValueString"/>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        TagValueString GetTagValueString(string json);
    }
}
