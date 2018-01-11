using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IDRecordLogic
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        string ValidationErrors { get; }

        /// <summary>
        /// Gets the d record line.
        /// </summary>
        /// <param name="dRecordData">The d record data.</param>
        /// <returns></returns>
        string GetDRecordLine(DRecord dRecordData);
    }
}
