using GPNA.Converters.TagValues;

namespace GPNA.Converters.Model
{
    public static class TagValueExtensions
    {
        public static string GetValue(this TagValue tagvalue)
        {
            var type = tagvalue.GetType();
            var prop = type.GetProperty("Value");
            var value = prop?.GetValue(tagvalue, null);

            return value?.ToString();
        }
    }
}
