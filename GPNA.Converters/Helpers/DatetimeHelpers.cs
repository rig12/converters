namespace GPNA.Converters.Helpers
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        /// <summary>
        /// Разделить временной промежуток по месяцам
        /// </summary>
        /// <param name="start">Начальное время</param>
        /// <param name="end">Конечное время</param>
        /// <returns>Возвращает коллекцию временных промежутков</returns>
        public static IDictionary<DateTime, DateTime> SplitByMonth(DateTime start, DateTime end)
        {
            var periods = new Dictionary<DateTime, DateTime>();
            if (start == end)
            {
                periods.Add(start, end);
            }
            if (start >= end)
            {
                return periods;
            }
            if (end.Month != start.Month)
            {
                var border = new DateTime(end.Year, end.Month, 1);
                var collection = new Dictionary<DateTime, DateTime>()
                {
                    { start, border.AddMilliseconds(-1) }
                };
                if (border != end)
                {
                    collection.Add(border, end);
                }
                foreach (var inner in collection.Select(x => SplitByMonth(x.Key, x.Value)))
                {
                    foreach (var pair in inner)
                    {
                        if (!periods.ContainsKey(pair.Key))
                        {
                            periods.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }
            else
            {
                periods.Add(start, end);
            }
            return periods;
        }

        /// <summary>
        /// Выделить по датам и продолжительности
        /// </summary>
        /// <param name="start">Начальная дата/время</param>
        /// <param name="end">Конечная дата/время</param>
        /// <param name="durationSeconds">Продолжительность (задается в секундах)</param>
        /// <returns>Возвращает коллекцию временных промежутков</returns>
        public static IDictionary<DateTime, DateTime> GetPeriods(DateTime start, DateTime end, int durationSeconds)
        {
            var periods = new Dictionary<DateTime, DateTime>();
            if (start >= end)
            {
                return periods;
            }
            if ((end - start).TotalSeconds <= durationSeconds)
            {
                if (end.Month != start.Month)
                {
                    var border = new DateTime(end.Year, end.Month, end.Day);
                    periods.Add(start, border.AddMilliseconds(-1));
                    if (border != end)
                    {
                        periods.Add(border, end);
                    }
                }
                else
                {
                    periods.Add(start, end);
                }
                return periods;
            }
            while (end > start)
            {
                var datetime = start.AddSeconds(durationSeconds);
                if (datetime >= end)
                {
                    datetime = end;
                }
                if (datetime.Month != start.Month)
                {
                    var border = new DateTime(datetime.Year, datetime.Month, datetime.Day);
                    periods.Add(start, border.AddMilliseconds(-1));
                    if (border != datetime)
                    {
                        periods.Add(border, datetime);
                    }
                }
                else
                {
                    periods.Add(start, datetime);
                }
                start = datetime.AddMilliseconds(1);
            }
            return periods;
        }
    }
}
