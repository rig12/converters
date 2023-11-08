namespace Converters.Interfaces
{
    #region Using
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
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
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом <see cref="KeyAttribute"/></exception>
        IEnumerable<ChangeEntity> Load<TEntity>(IEnumerable<TEntity> entities, bool isReload = false) where TEntity : INotifyPropertyChanged;

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип объекта</typeparam>
        /// <param name="entity"></param>
        /// <returns>Возвращает объект если он ранее был добавлен, в обратном случае Null</returns>
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом <see cref="KeyAttribute"/></exception>
        ChangeEntity? Delete<TEntity>(TEntity entity) where TEntity : INotifyPropertyChanged;

        /// <summary>
        /// Сохранить сущность
        /// </summary>
        /// <typeparam name="TEntity">Тип изменяемого объекта</typeparam>
        /// <param name="entity">Сущность к сохранению</param>
        /// <returns>Возвращает объект если он ранее не был добавил или обновлен, иначе Null</returns>
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом <see cref="KeyAttribute"/></exception>
        ChangeEntity? Save<TEntity>(TEntity entity) where TEntity : INotifyPropertyChanged;
    }
}
