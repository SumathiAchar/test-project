using FileHelpers;

namespace SSI.ContractManagement.Shared.Models
{
    [DelimitedRecord(",")]
    public class FileImportFormat
    {
        /// <summary>
        /// The code
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        // ReSharper disable once UnassignedField.Global
        public string Code;

        /// <summary>
        /// The amount
        /// </summary>
        public string Amount { get; set; }
    }
}
