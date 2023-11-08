namespace Converters.Services
{
    #region Using
    using Helpers;
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Model;
    using System.ComponentModel.DataAnnotations.Schema;
    #endregion Using

    /// <summary>
    /// Класс управления
    /// </summary>
    public class ObjectDescriptionCache : IObjectDescriptionCache
    {
        #region Fields
        private readonly IDictionary<Type, TypeActivity> _storage;
        #endregion Fields


        #region Constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ObjectDescriptionCache()
        {
            _storage = new Dictionary<Type, TypeActivity>();
        }
        #endregion Constructors


        #region Classes
        /// <summary>
        /// Активность типа
        /// </summary>
        private class TypeActivity
        {
            /// <summary>
            /// Флаг полной загрузки
            /// </summary>
            public bool IsLoaded { get; set; }

            /// <summary>
            /// Описания
            /// </summary>
            public IDictionary<string, ObjectDescription?> Descriptions { get; set; } = new Dictionary<string, ObjectDescription?>();
        }
        #endregion Classes


        #region Methods
        /// <summary>
        /// Получить описание свойства
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <returns>Возвращает объект описания в случае наличия свойства и атрибута <see cref="ColumnAttribute"/> на нем, иначе null</returns>
        public ObjectDescription? Get(Type type, string propertyName)
        {
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new TypeActivity());
            }
            if (!_storage[type].Descriptions.ContainsKey(propertyName))
            {
                _storage[type].Descriptions.Add(propertyName, ObjectDescriptionHelper.GetPropertyDescription(type, propertyName));
            }
            return _storage[type].Descriptions[propertyName];
        }

        /// <summary>
        /// Получить описание объекта
        /// </summary>
        /// <param name="type">Тип объекта</param>
        /// <returns>Возвращает коллекцию описаний свойств где есть атрибут <see cref="ColumnAttribute"/></returns>
        public IEnumerable<ObjectDescription> Get(Type type)
        {
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new TypeActivity());
            }
            var entity = _storage[type];
            entity.IsLoaded = true;
            foreach (var objectDescription in ObjectDescriptionHelper.GetDescriptionOfProperties(type))
            {
                if (!_storage[type].Descriptions.ContainsKey(objectDescription.PropertyName))
                {
                    _storage[type].Descriptions.Add(objectDescription.PropertyName, objectDescription);
                }
                yield return objectDescription;
            }
        }
        #endregion Methods
    }
}
