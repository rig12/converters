namespace GPNA.Converters.Services
{
    #region Using
    
    using GPNA.Converters.TagValues;    
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    #endregion Using

    /// <summary>
    /// Класс преобразования значений тегов
    /// </summary>
    public class TagValueConverter : ITagValueConverter
    {
        #region Constructors
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
        }
        #endregion Constructors


        #region Fields
        private readonly ILogger<TagValueConverter> _logger;
        private readonly Dictionary<Type, Func<TagValue>> _lambdas = new Dictionary<Type, Func<TagValue>>();
        private readonly Dictionary<Type, Type> _pairs = new Dictionary<Type, Type>()
        {
            { typeof(double), typeof(TagValueDouble) },
            { typeof(DateTime), typeof(TagValueDateTime) },
            { typeof(bool), typeof(TagValueBool) },
            { typeof(int), typeof(TagValueInt32) },
            { typeof(long), typeof(TagValueInt64) },
            { typeof(string), typeof(TagValueString) }
        };
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

        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }

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
        #endregion Methods       
    }
}
