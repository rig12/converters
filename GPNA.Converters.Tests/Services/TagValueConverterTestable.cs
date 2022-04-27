namespace GPNA.Common.Tests.Services
{
    #region Using
    using Common.Services;
    using Microsoft.Extensions.Logging;
    #endregion Using

    public class TagValueConverterTestable : TagValueConverter
    {
        public TagValueConverterTestable(ILogger<TagValueConverterTestable> logger) : base(logger)
        { }
    }
}
