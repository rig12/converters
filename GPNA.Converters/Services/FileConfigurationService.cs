namespace GPNA.Converters.Services
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Interfaces;
    using Model;
    #endregion Using

    /// <summary>
    /// Класс сервиса работы с конфигурациями
    /// </summary>
    public class FileConfigurationService : IFileConfigurationService
    {
        #region Methods
        private static IEnumerable<string[]> GetFromFile(string initFilePath, bool isDelete = false, char separator = ';', char commentMark = '#')
        {
            var result = new List<string[]>();
            if (!File.Exists(initFilePath))
            {
                return result;
            }
            var lines = File.ReadAllLines(initFilePath)
               .Where(c => !string.IsNullOrEmpty(c) && !c.StartsWith(commentMark));
            result.AddRange(lines.Select(x => x.Split(separator)));
            if (isDelete)
            {
                File.Delete(initFilePath);
            }
            return result;
        }

        public IEnumerable<TEntity> Get<TEntity, TIdType>(string fileName, IEnumerable<PropertyConfiguration> configurations)
            where TEntity : CacheEntityBase<TIdType>
            where TIdType : struct
        {
            var count = configurations.Count();
            if (count == default)
            {
                yield break;
            }
            var codes = new HashSet<string>();
            var lineNumb = 0;

            foreach (var row in GetFromFile(fileName, true))
            {
                lineNumb++;
                StringBuilder codeBuilder = new();
                var type = typeof(TEntity);
                var entity = Activator.CreateInstance(type);
                try
                {
                    foreach (var configuration in configurations)
                    {
                        var propertyInfo = type.GetProperty(configuration.Name);
                        if (propertyInfo == default)
                        {
                            continue;
                        }
                        var value = row[configuration.Index];
                        propertyInfo.SetValue(entity, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        if (configuration.IsKey)
                        {
                            codeBuilder.Append(value);
                        }
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception($"Error {exception} in {lineNumb} line");
                }
                var code = codeBuilder.ToString();
                if (!codes.Contains(code))
                {
                    codes.Add(code);
                    yield return (TEntity)entity!;
                }
            }
        }
        #endregion Methods
    }
}
