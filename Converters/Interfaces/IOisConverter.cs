using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converters.Interfaces
{
    /// <summary>
    /// Интерфейс преобразования ois кода
    /// </summary>
    public interface IOisConverter
    {
        /// <summary>
        /// Переводит геономер в номер ois
        /// </summary>
        /// <param name="geonumber">Геономер скважины</param>
        /// <param name="fieldId">Идентификатор месторождения</param>
        /// <param name="codes">Справочник кодировок</param>
        /// <returns>Возвращает ois номер скважины в случае успеха, иначе 0</returns>
        long GetOisCode(string geonumber, int fieldId, IDictionary<string, string> codes);
    }
}
