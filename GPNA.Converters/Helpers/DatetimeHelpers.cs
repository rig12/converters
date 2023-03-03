namespace GPNA.Converters.Helpers
{
    #region Using
    using System;
    #endregion Using

    /// <summary>
    /// Класс помощник для типа даты/времени
    /// </summary>
    public static class DatetimeHelpers
    {
        /// <summary>
        /// Привести к числовому виду ГГГГММ
        /// </summary>
        /// <param name="dateTime">Дата/время</param>
        /// <returns>Возвращает числовое значение</returns>
        public static int FormatToYearMonth(DateTime dateTime)
        {
            return (dateTime.Year * 100 + dateTime.Month) * 10000;
        }

        /// <summary>
        /// Привести к числовому виду ГГГГММДД
        /// </summary>
        /// <param name="dateTime">Дата/время</param>
        /// <returns>Возвращает числовое значение</returns>
        public static int FormatToYearMonthDay(DateTime dateTime)
        {
            return FormatToYearMonth(dateTime) + (dateTime.Day * 100);
        }

        /// <summary>
        /// Привести к числовому виду ГГГГММДДЧЧ
        /// </summary>
        /// <param name="dateTime">Дата/время</param>
        /// <returns>Возвращает числовое значение</returns>
        public static int FormatToYearMonthDayHour(DateTime dateTime)
        {
            return FormatToYearMonthDay(dateTime) + dateTime.Hour;
        }
    }
}
