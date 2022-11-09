namespace GPNA.Converters.Extensions
{
    #region Using
    #endregion Using

    /// <summary>
    /// Расширения для работы со скважиной
    /// </summary>
    public static class WellExtensions
    {
        private const int BRANCHID_SYMBOLS_COUNT = 3;
        private const int MIN_SYMBOLS_COUNT = 8;

        public static int ParseFieldId(this long wellId)
        {
            string strResult = string.Empty;
            var fieldIdStr = wellId.ToString();

            if (fieldIdStr.Length < MIN_SYMBOLS_COUNT)
            {
                return -1;
            }

            if (fieldIdStr.Length == 8)
            {
                fieldIdStr = fieldIdStr.Insert(0, "00");
            }
            else if (fieldIdStr.Length == 9)
            {
                fieldIdStr = fieldIdStr.Insert(0, "0");
            }

            var symbols = fieldIdStr.ToCharArray();
            for (int i = 0; i < BRANCHID_SYMBOLS_COUNT; i++)
            {
                strResult = strResult + symbols[i];
            }

            if (int.TryParse(strResult, out var result))
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
