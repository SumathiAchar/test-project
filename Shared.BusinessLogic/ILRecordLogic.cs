using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface ILRecordLogic
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        string ValidationErrors { get; }

        /// <summary>
        /// Gets the l record line.
        /// </summary>
        /// <param name="lRecordList">The l record list.</param>
        /// <returns></returns>
        string GetLRecordLine(IEnumerable<LRecord> lRecordList);
    }
}
