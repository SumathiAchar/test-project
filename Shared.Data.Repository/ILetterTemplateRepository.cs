using System;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    /// <summary>
    /// Interface for Letter Template Repository
    /// </summary>
    public interface ILetterTemplateRepository : IDisposable
    {
        /// <summary>
        /// Adds the edit letter template.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        long Save(LetterTemplate letterTemplate);
        
        /// <summary>
        /// Gets the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        LetterTemplate GetLetterTemplateById(LetterTemplate letterTemplate);
        
        /// <summary>
        /// Determines whether [is letter name exists] [the specified letter template].
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        bool IsLetterNameExists(LetterTemplate letterTemplate);

        /// <summary>
        /// Gets all letter templates.
        /// </summary>
        /// <param name="letterTemplateInfo">The letter template information.</param>
        /// <returns></returns>
        LetterTemplateContainer GetLetterTemplates(LetterTemplateContainer letterTemplateInfo);

        /// <summary>
        /// Deletes the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        bool Delete(LetterTemplate letterTemplateToDelete);

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        bool InsertAuditLog(LetterTemplate letterTemplate);
    }
}
