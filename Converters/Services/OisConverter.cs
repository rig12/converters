namespace GPNA.Converters.Services
{
    #region Using
    using Interfaces;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    #endregion Using

    /// <summary>
    /// Класс преобразования ois кода
    /// </summary>
    public class OisConverter : IOisConverter
    {

        #region Constants
        private const string ZERO_TEXT = "0";
        private const int OIS_LENGTH = 10;
        #endregion Constants


        #region Methods
        /// <summary>
        /// Переводит геономер в номер ois
        /// </summary>
        /// <param name="geonumber">Геономер скважины</param>
        /// <param name="fieldId">Идентификатор месторождения</param>
        /// <param name="codes">Справочник кодировок</param>
        /// <returns>Возвращает ois номер скважины в случае успеха, иначе 0</returns>
        public long GetOisCode(string geonumber, int fieldId, IDictionary<string, string> codes)
        {
            try
            {
                var tailChar = geonumber.ToCharArray().FirstOrDefault(x => !char.IsNumber(x));
                string tail = $"{ZERO_TEXT}{ZERO_TEXT}";
                string body = geonumber;
                if (tailChar != default)
                {
                    var tailCode = geonumber.Substring(geonumber.IndexOf(tailChar));
                    if (!string.IsNullOrEmpty(tailCode))
                    {
                        if (codes.ContainsKey(tailCode))
                        {
                            tail = codes[tailCode];
                        }
                        else
                        {
                            return default;
                        }
                    }
                    body = geonumber[..geonumber.IndexOf(tailChar)];
                }
                string result = string.Empty;
                var secondPart = $"{body}{tail}";
                var header = fieldId.ToString("D3");
                if (header.Length + secondPart.Length >= OIS_LENGTH)
                {
                    result = $"{fieldId}{secondPart}";
                }
                else
                {
                    StringBuilder builder = new();
                    builder.Append(header);
                    while (builder.Length + secondPart.Length < OIS_LENGTH)
                    {
                        builder.Append(ZERO_TEXT);
                    }
                    builder.Append(secondPart);
                    result = builder.ToString();
                }
                if (long.TryParse(result, out long ois))
                {
                    return ois;
                }
                else
                {
                    return default;
                }
            }
            catch
            {
                return default;
            }
        }
        #endregion Methods
    }
}
