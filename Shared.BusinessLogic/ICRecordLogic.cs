using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface ICRecordLogic
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        string ValidationErrors { get; }

        /// <summary>
        /// Gets the c record line.
        /// </summary>
        /// <param name="cRecordDataRaw">The c record data raw.</param>
        /// <returns></returns>
        string GetCRecordLine(CRecord cRecordDataRaw);
    }
}
