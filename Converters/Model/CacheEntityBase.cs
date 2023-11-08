namespace GPNA.Converters.Model
{
    #region Using
    #endregion Using

    /// <summary>
    /// Базовый класс сущности кэша
    /// </summary>
    public abstract class CacheEntityBase<TIdType> where TIdType : struct
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public TIdType Id { get; set; }
    }
}
