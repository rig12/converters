namespace GPNA.Converters.Interfaces
{
    #region Using
    using System.Collections.Generic;
    using Model;
    #endregion Using

    /// <summary>
    /// Интерфейс сервиса работы с файлами конфигурации
    /// </summary>
    public interface IFileConfigurationService
    {
        /// <summary>
        /// Получить объекты из файла
        /// </summary>
        /// <typeparam name="TEntity">Тип</typeparam>
        /// <typeparam name="TIdType">Тип идентификатора</typeparam>
        /// <param name="initFilePath">Полный путь к файлу</param>
        /// <param name="configurations">Конфигурация для парсинга файла</param>
        /// <returns>Возвращает коллекцию объектов</returns>
        IEnumerable<TEntity> Get<TEntity, TIdType>(string initFilePath, IEnumerable<PropertyConfiguration> configurations)
            where TEntity : CacheEntityBase<TIdType>
            where TIdType : struct;
    }
}
