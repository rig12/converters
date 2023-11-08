

namespace Converters.TagValues
{
    #region Using
    using System;
    using Newtonsoft.Json;
    #endregion Using

    /// <summary>
    /// Класс значение тега
    /// </summary>
    public abstract class TagValue
    {
        /// <summary>
        /// Идентификатор типа тега
        /// </summary>
        public long TagId { get; set; }

        /// <summary>
        /// Метка времени тега в источнике
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Метка времени UTC тега в источнике
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? DateTimeUtc { get; set; }

        /// <summary>
        /// Метка времени фиксации тега в коллекторе
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DateTime? TimeStampUtc { get; set; }

                
        /// <summary>
        /// Уровень качества OPC
        /// </summary>
        public int OpcQuality { get; set; }


        /// <summary>
        /// Полное наименование тэга
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Tagname { get; set; } = string.Empty;
    }
}
