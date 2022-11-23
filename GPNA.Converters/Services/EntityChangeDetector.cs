namespace GPNA.Converters.Services
{
    #region Using
    using System;
    using System.Linq;
    using Interfaces;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Helpers;
    using Model;
    using Enums;
    using System.Text;
    using System.ComponentModel.DataAnnotations;
    #endregion Using

    /// <summary>
    /// Класс определения изменений сущностей
    /// </summary>
    public class EntityChangeDetector : IEntityChangeDetector
    {
        #region Fields
        private readonly IDictionary<Type, IDictionary<string, NotifyEntity>> _storage;
        private readonly IObjectDescriptionCache _objectDescriptionCache;
        #endregion Fields


        #region Constructors
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="objectDescriptionCache"></param>
        public EntityChangeDetector(IObjectDescriptionCache objectDescriptionCache)
        {
            _storage = new Dictionary<Type, IDictionary<string, NotifyEntity>>();
            _objectDescriptionCache = objectDescriptionCache;
        }
        #endregion Constructors


        #region Constants
        private const string NULL_TEXT = "NULL";
        private const string DELIMITER = "_";
        #endregion Constants


        #region Properties
        private Func<DateTimeOffset> GetUtcOffset = () => DateTimeOffset.UtcNow;
        #endregion Properties


        #region Classes
        /// <summary>
        /// Сущность к обработке
        /// </summary>
        private class NotifyEntity
        {
            /// <summary>
            /// Обрабатываемая сущность
            /// </summary>
            public INotifyPropertyChanged Entity { get; set; } = null!;

            /// <summary>
            /// Флаг наличия
            /// </summary>
            public bool IsDeleted { get; set; }

            /// <summary>
            /// Обновляемые свойства
            /// </summary>
            public IDictionary<string, ObjectDescription> ModifiedProperties { get; set; } = new Dictionary<string, ObjectDescription>();
        }
        #endregion Classes


        #region Methods
        private string GetCode<TEntity>(TEntity entity) where TEntity : INotifyPropertyChanged
        {
            StringBuilder stringBuilder = new();
            string delimiter = String.Empty;
            foreach (var description in ObjectDescriptionHelper.GetDescriptionOfProperties(typeof(TEntity)))
            {
                if (description.IsKey)
                {
                    stringBuilder.Append(delimiter);
                    var value = ObjectDescriptionHelper.GetPropertyValue(entity, description.PropertyName);
                    stringBuilder.Append(value ?? NULL_TEXT);
                    if (!string.Equals(delimiter, DELIMITER))
                    {
                        delimiter = DELIMITER;
                    }
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Получить коллекцию изменившихся объектов
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entities">Сущности к проверке на изменение</param>
        /// <param name="isReload">Флаг полной загрузки</param>
        /// <returns>Возвращает коллекцию изменившихся объектов</returns>
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом<see cref="KeyAttribute"/></exception>
        public IEnumerable<ChangeEntity> Load<TEntity>(IEnumerable<TEntity> entities, bool isReload = false)
            where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            if (!ObjectDescriptionHelper.IsContainsKey(type))
            {
                throw new ArgumentException($"The type '{type.FullName}' should contains at least one property with '{typeof(KeyAttribute).FullName}'");
            }
            var tableName = ObjectDescriptionHelper.GetTableName(type);
            var offset = GetUtcOffset();
            if (!_storage.ContainsKey(type) || isReload)
            {
                foreach (var entity in Read(entities, tableName, offset.DateTime))
                {
                    yield return entity;
                }
            }
            else
            {
                ResetEntities(_storage[type].Values);
                foreach (var entity in entities)
                {
                    var code = GetCode(entity);
                    var notifyEntity = _storage[type].ContainsKey(code) ? _storage[type][code] : default;
                    ChangeEntity? activeEntity = default;
                    if (notifyEntity != default)
                    {
                        activeEntity = Update(entity, tableName, offset.DateTime, notifyEntity, code);
                    }
                    else
                    {
                        activeEntity = Add(entity, tableName, offset.DateTime, code);
                    }
                    if (activeEntity == default)
                    {
                        continue;
                    }
                    yield return activeEntity;
                }
                foreach (var deletedEntity in Delete<TEntity>(tableName, offset.DateTime))
                {
                    yield return deletedEntity;
                }
            }
        }

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип изменяемого объекта</typeparam>
        /// <param name="entity">Сущность к удалению</param>
        /// <returns>Возвращает объект если он ранее был добавлен, в обратном случае Null</returns>
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом<see cref="KeyAttribute"/></exception>
        public ChangeEntity? Delete<TEntity>(TEntity entity) where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            if (!ObjectDescriptionHelper.IsContainsKey(type))
            {
                throw new ArgumentException($"The type '{type.FullName}' should contains at least one property with '{typeof(KeyAttribute).FullName}'");
            }
            var code = GetCode(entity);
            if (!_storage.ContainsKey(type))
            {
                return default;
            }           
            if (!_storage[type].ContainsKey(code))
            {
                return default;
            }
            var deletedEntity = BuildEntity<TEntity>(EntityStatus.Delete, 
                ObjectDescriptionHelper.GetTableName(type), GetUtcOffset().DateTime, code);
            foreach (var propertyDescription in _objectDescriptionCache.Get(type))
            {
                deletedEntity.Values.Add(new ChangeValue()
                {
                    Name = propertyDescription.ColumnName,
                    Value = ObjectDescriptionHelper.GetPropertyValue(entity, propertyDescription.PropertyName),
                    TypeName = propertyDescription.TypeName,
                    IsKey = propertyDescription.IsKey
                });
            }
            return deletedEntity;
        }

        /// <summary>
        /// Сохранить сущность
        /// </summary>
        /// <typeparam name="TEntity">Тип изменяемого объекта</typeparam>
        /// <param name="entity">Сущность к сохранению</param>
        /// <returns>Возвращает объект если он ранее не был добавил или обновлен, иначе Null</returns>
        /// <exception cref="ArgumentException">Возвращает в случае отсутсвия свойств с ключом<see cref="KeyAttribute"/></exception>
        public ChangeEntity? Save<TEntity>(TEntity entity) where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            if (!ObjectDescriptionHelper.IsContainsKey(type))
            {
                throw new ArgumentException($"The type '{type.FullName}' should contains at least one property with '{typeof(KeyAttribute).FullName}'");
            }
            var code = GetCode(entity);
            var tableName = ObjectDescriptionHelper.GetTableName(type);
            var offset = GetUtcOffset();
            var notifyEntity = _storage[type].ContainsKey(code) ? _storage[type][code] : default;
            ChangeEntity? activeEntity = default;
            if (notifyEntity != default)
            {
                activeEntity = Update(entity, tableName, offset.DateTime, notifyEntity, code);
            }
            else
            {
                activeEntity = Add(entity, tableName, offset.DateTime, code);
            }
            return activeEntity;
        }

        private IEnumerable<ChangeEntity> Delete<TEntity>(string tableName, DateTime dateTime) where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            foreach (var pair in _storage[type].Where(x => x.Value.IsDeleted))
            {
                var entity = pair.Value.Entity;
                var code = GetCode(entity);
                var deletedEntity = BuildEntity<TEntity>(EntityStatus.Delete, tableName, dateTime, code);
                foreach (var propertyDescription in _objectDescriptionCache.Get(type))
                {
                    deletedEntity.Values.Add(new ChangeValue()
                    {
                        Name = propertyDescription.ColumnName,
                        Value = ObjectDescriptionHelper.GetPropertyValue(pair.Value.Entity, propertyDescription.PropertyName),
                        TypeName = propertyDescription.TypeName,
                        IsKey = propertyDescription.IsKey
                    });
                }
                yield return deletedEntity;
                _storage[type].Remove(code);
            }
        }

        private ChangeEntity Add<TEntity>(TEntity entity, string tableName, DateTime dateTime, string code)
            where TEntity : INotifyPropertyChanged
        {
            var additionEntity = BuildEntity<TEntity>(EntityStatus.Create, tableName, dateTime, code);
            foreach (var propertyDescription in _objectDescriptionCache.Get(typeof(TEntity)))
            {
                additionEntity.Values.Add(new ChangeValue()
                {
                    Name = propertyDescription.ColumnName,
                    Value = ObjectDescriptionHelper.GetPropertyValue(entity, propertyDescription.PropertyName),
                    TypeName = propertyDescription.TypeName,
                    IsKey = propertyDescription.IsKey
                });
            }
            AddToStorage(entity, code);
            return additionEntity;
        }

        private ChangeEntity? Update<TEntity>(TEntity entity, string tableName, DateTime dateTime, NotifyEntity notifyEntity, string code)
            where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            notifyEntity.IsDeleted = false;
            foreach (var propertyDescription in _objectDescriptionCache.Get(type))
            {
                ObjectDescriptionHelper.SetPropertyValue(notifyEntity.Entity, propertyDescription.ColumnName,
                    ObjectDescriptionHelper.GetPropertyValue(entity, propertyDescription.ColumnName));
            }
            if (!notifyEntity.ModifiedProperties.Any())
            {
                return default;
            }
            var updatedEntity = BuildEntity<TEntity>(EntityStatus.Update, tableName, dateTime, code);
            foreach (var propertyDescription in _objectDescriptionCache.Get(type))
            {
                updatedEntity.Values.Add(new ChangeValue()
                {
                    Name = propertyDescription.ColumnName,
                    Value = ObjectDescriptionHelper.GetPropertyValue(notifyEntity.Entity, propertyDescription.PropertyName),
                    TypeName = propertyDescription.TypeName,
                    IsKey = propertyDescription.IsKey,
                    IsModified = notifyEntity.ModifiedProperties.ContainsKey(propertyDescription.PropertyName)
                });
            }
            return updatedEntity;
        }

        private IEnumerable<ChangeEntity> Read<TEntity>(IEnumerable<TEntity> entities, string tableName, DateTime dateTime)
            where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            _storage.TryAdd(type, new Dictionary<string, NotifyEntity>(entities.Count()));
            _storage[type].Clear();
            foreach (var entity in entities)
            {
                var code = GetCode(entity);
                var readEntity = BuildEntity<TEntity>(EntityStatus.Read, tableName, dateTime, code);
                foreach (var propertyDescription in _objectDescriptionCache.Get(type))
                {
                    readEntity.Values.Add(new ChangeValue()
                    {
                        Name = propertyDescription.ColumnName,
                        Value = ObjectDescriptionHelper.GetPropertyValue(entity, propertyDescription.PropertyName),
                        TypeName = propertyDescription.TypeName,
                        IsKey = propertyDescription.IsKey
                    });
                }
                AddToStorage(entity, code);
                yield return readEntity;
            }
        }

        private void ResetEntities(IEnumerable<NotifyEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.ModifiedProperties.Clear();
            }
        }

        private ChangeEntity BuildEntity<TEntity>(EntityStatus entityStatus, string tableName, DateTime dateTime, string code)
            where TEntity : INotifyPropertyChanged
        {
            return new ChangeEntity()
            {
                EntityStatus = entityStatus,
                TableName = tableName,
                DateTimeUtc = dateTime,
                Code = code
            };
        }

        private NotifyEntity? AddToStorage<TEntity>(TEntity entity, string code)
             where TEntity : INotifyPropertyChanged
        {
            var type = typeof(TEntity);
            if (!_storage.ContainsKey(type))
            {
                _storage.Add(type, new Dictionary<string, NotifyEntity>());
            }
            if (_storage[type].ContainsKey(code))
            {
                return _storage[type][code];
            }
            var notifyEntity = new NotifyEntity()
            {
                Entity = entity
            };
            entity.PropertyChanged += (object? sender, PropertyChangedEventArgs e) =>
            {
                if (sender != default && !string.IsNullOrEmpty(e.PropertyName) && !notifyEntity.ModifiedProperties.ContainsKey(e.PropertyName))
                {
                    var objectDescription = _objectDescriptionCache.Get(sender.GetType(), e.PropertyName);
                    if (objectDescription != default)
                    {
                        notifyEntity.ModifiedProperties.Add(e.PropertyName, objectDescription);
                    }
                }
            };
            _storage[type].Add(code, notifyEntity);
            return notifyEntity;
        }
        #endregion Methods
    }
}
