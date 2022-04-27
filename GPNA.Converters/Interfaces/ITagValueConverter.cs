
using GPNA.Converters.TagValues;

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
    }
}
