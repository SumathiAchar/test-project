namespace SSI.ContractManagement.Shared.Helpers.StringExtension
{
    public static class StringExtensions
    {
        /// <summary>
        /// To the trim.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public static string ToTrim(this string inputValue)
        {
            if (!string.IsNullOrWhiteSpace(inputValue))
            {
                return inputValue.Trim();
            }
            return inputValue;
        }
    }
}