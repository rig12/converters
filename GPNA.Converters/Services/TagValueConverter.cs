namespace GPNA.Converters.Services
{
    #region Using

    using GPNA.Converters.TagValues;
    using GPNA.Extensions.Types;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;

    #endregion Using

    /// <summary>
    /// Класс преобразования значений тегов
    /// </summary>
    public class TagValueConverter : ITagValueConverter
    {
        #region Constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="logger"></param>
        public TagValueConverter(ILogger<TagValueConverter> logger)
        {
            _logger = logger;
            foreach (var item in _pairs)
            {
                var lambda = BuildLambda(item.Key);
                if (lambda != default)
                {
                    _lambdas.Add(item.Key, lambda);
                }
            }

            _settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
        #endregion Constructors


        #region Fields
        private readonly ILogger<TagValueConverter> _logger;
        private readonly Dictionary<Type, Func<TagValue>> _lambdas = new();
        private readonly Dictionary<Type, Type> _pairs = new()
        {
            { typeof(double), typeof(TagValueDouble) },
            { typeof(DateTime), typeof(TagValueDateTime) },
            { typeof(bool), typeof(TagValueBool) },
            { typeof(int), typeof(TagValueInt32) },
            { typeof(long), typeof(TagValueInt64) },
            { typeof(ulong), typeof(TagValueInt64) },
            { typeof(string), typeof(TagValueString) }
        };
        private JsonSerializerSettings _settings;

        #endregion Fields


        #region Methods 
        /// <summary>
        /// Получить лябмда-выражение, возвращающее экземпляр указанного в аргументе типа
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Возвращает объект лябмды</returns>
        private Func<TagValue> BuildLambda(Type type)
        {
            var newExpr = Expression.New(_pairs[type]);
            var newlambda = Expression.Lambda(newExpr);
            var func = newlambda.Compile();
            if (func is Func<TagValue> nnfunc)
            {
                return nnfunc;
            }
            return default;
        }

        /// <summary>
        /// Преобразовать объект к типу
        /// </summary>
        /// <param name="obj">Объект для преобразования</param>
        /// <param name="castTo">Тип</param>
        /// <returns>Возвращает преобразованный объект</returns>
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

        /// <summary>
        /// Преобразовать JSON объект к типу тега
        /// </summary>
        /// <param name="json">JSON объекта</param>
        /// <returns>Возвращает тег</returns>
        public TagValue GetTagValue(string json)
        {
            TagValue result = null;
            try
            {
                var tagValueDynamic = JsonConvert.DeserializeObject<TagValueDynamic>(json);
                if (tagValueDynamic == default)
                {
                    return default;
                }
                if (tagValueDynamic.Value != null)
                {
                    var type = tagValueDynamic.Value.GetType();
                    var func = _lambdas.ContainsKey(type) ? _lambdas[type] : null;
                    if (func != null)
                    {
                        result = func();
                        if (result != default)
                        {
                            var property = result.GetType().GetProperty(nameof(TagValueDynamic.Value));
                            if (property != default)
                            {
                                property.SetValue(result, Cast(tagValueDynamic.Value, type));
                            }
                        }
                    }
                }
                if (result == default)
                {
                    result = new TagValueNull();
                }
                result.DateTime = tagValueDynamic.DateTime;
                result.DateTimeUtc = tagValueDynamic.DateTimeUtc;
                result.TimeStampUtc = tagValueDynamic.TimeStampUtc;
                result.OpcQuality = tagValueDynamic.OpcQuality;
                result.TagId = tagValueDynamic.TagId;
                if (!string.IsNullOrEmpty(tagValueDynamic.Tagname))
                {
                    result.Tagname = tagValueDynamic.Tagname.Trim();
                    result.Tagname = result.Tagname.Contains('!') ? result.Tagname.Substring(result.Tagname.LastIndexOf('!') + 1)
                        : result.Tagname;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
            }
            return result;
        }


        /// <summary>
        /// Возвращает значение типа, соответствующего реальному значению в строке 
        /// </summary>
        /// <param name="value">Значение в виде строки</param>
        /// <param name="datetimeParseFormat">Шаблон парсинга значения параметра value если внутри дата-время></param>
        /// <returns>Воозвращает значение у случае успеха, иначе null</returns>
        public dynamic? GetValue(string value, string datetimeParseFormat = "yyyy-MM-dd HH:mm")
        {
            if (string.IsNullOrEmpty(value))
                return default;

            if (DateTime.TryParseExact(value, datetimeParseFormat, null, DateTimeStyles.None, out var datetimeval))
                return datetimeval.ToUniversalTime();

            if (int.TryParse(value, out var intval))
                return intval;

            if (bool.TryParse(value, out var boolval))
                return boolval;

            if (double.TryParse(value, out var doubleval))
                return doubleval;

            return value;

        }

        /// <summary>
        /// Возвращает соответствующую реальному типу значения Value одну из структур TagValueXxx
        /// <seealso cref="TagValueBool"/> <seealso cref="TagValueDateTime"/> <seealso cref="TagValueDouble"/> <seealso cref="TagValueInt32"/> <seealso cref="TagValueInt64"/> <seealso cref="TagValueNull"/><seealso cref="TagValueString"/>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <param name="tagname"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        public TagValue GetTagValue(string value, DateTime dateTime, string tagname, int quality)
        {
            var dynamicvalue = GetValue(value);
            TagValue result = null;
            if (value != null)
            {
                if (dynamicvalue is object objvalue
                    && _lambdas.TryGetValue(objvalue.GetType(), out var func))
                {
                    result = func();

                    if (objvalue?.GetType() is Type type && (type.IsSimple() || type == typeof(DateTime)))
                    {
                        var valueproperty = result.GetType().GetProperty("Value");
                        if (valueproperty != null)
                        {
                            valueproperty.SetValue(result, objvalue);
                        }
                    }
                }
            }
            else
            {
                result = new TagValueNull();
            }

            result.Tagname = tagname;
            result.DateTime = dateTime.Kind == DateTimeKind.Utc ? dateTime.ToLocalTime() : dateTime;
            result.DateTimeUtc = result.DateTime?.ToUniversalTime();
            result.OpcQuality = quality;
            return result;
        }

        /// <summary>
        /// Получить преобразованное из строки значение с плавающей точкой
        /// </summary>
        /// <param name="valuestring">Строка для преобразования</param>
        /// <param name="datetimeParseFormat">Формат даты/времени</param>
        /// <returns>Возвращает значение в случае успеха, иначе Null</returns>
        public double? GetDoubleValue(string valuestring, string datetimeParseFormat = "yyyy-MM-dd HH:mm")
        {
            if (double.TryParse(valuestring, out var doublevalue))
            {
                return doublevalue;
            }
            else
                if (bool.TryParse(valuestring, out var boolvalue))
            {
                return boolvalue ? 1 : 0;
            }
            else if (DateTime.TryParseExact(valuestring, datetimeParseFormat, null, DateTimeStyles.None, out var datetimevalue))
            {
                return datetimevalue.ToUniversalTime().ConvertToUnixTimestamp();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// возвращает значение <seealso cref="TagValueString.Value"/> в виде строки
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public TagValueString GetTagValueString(string json)
        {
            return JsonConvert.DeserializeObject<TagValueString>(json, _settings);
        }
        #endregion Methods
    }
}
