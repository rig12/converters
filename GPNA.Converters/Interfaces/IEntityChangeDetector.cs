namespace GPNA.Converters.Interfaces
{
    #region Using
    using Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    #endregion Using

    /// <summary>
    /// Интерфейс определения изменений сущностей
    /// </summary>
    public interface IEntityChangeDetector
    {
        /// <summary>
        /// Получить коллекцию изменившихся объектов
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entities">Сущности к проверке на изменение</param>
        /// <param name="isReload">Флаг полной загрузки</param>
        /// <returns>Возвращает коллекцию изменившихся объектов</returns>
        IEnumerable<ChangeEntity> Load<TEntity>(IEnumerable<TEntity> entities, bool isReload = false) where TEntity : INotifyPropertyChanged;
    }
}
