using System.Text.RegularExpressions;

namespace SSI.ContractManagement.Shared.Helpers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class RegexHelper : Regex
    {
        /// <summary>
        /// Initializes a RegexHelper with the given search pattern.
        /// </summary>
        /// <param name="pattern">The RegexHelper pattern to match.</param>
        public RegexHelper(string pattern)
            : base(WildcardToRegex(pattern))
        {
        }

        /// <summary>
        /// Converts a RegexHelper to a reg ex.
        /// </summary>
        /// <param name="pattern">The RegexHelper pattern to convert.</param>
        /// <returns>A reg ex equivalent of the given RegexHelper.</returns>
        private static string WildcardToRegex(string pattern)
        {
            return "^" + Escape(pattern).
                Replace("\\*", ".*").
                Replace("\\?", ".") + "$";
        }
    }
}
