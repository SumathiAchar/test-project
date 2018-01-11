using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IERecordLogic
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        string ValidationErrors { get; }

        /// <summary>
        /// Gets the e record line.
        /// </summary>
        /// <param name="eRecordData">The e record data.</param>
        /// <returns></returns>
        string GetERecordLine(ERecord eRecordData);
    }
}
