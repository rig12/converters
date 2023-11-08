namespace GPNA.Converters.Tests.Services
{
    using GPNA.Converters.Services;
    #region Using
    using Microsoft.Extensions.Logging;
    #endregion Using

    public class TagValueConverterTestable : TagValueConverter
    {
        public TagValueConverterTestable(ILogger<TagValueConverterTestable> logger) : base(logger)
        { }
    }
}
