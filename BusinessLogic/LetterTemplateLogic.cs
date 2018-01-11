using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class LetterTemplateLogic
    {
        /// <summary>
        /// The _letter template repository
        /// </summary>
        private readonly ILetterTemplateRepository _letterTemplateRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public LetterTemplateLogic(string connectionString)
        {
            _letterTemplateRepository = Factory.CreateInstance<ILetterTemplateRepository>(connectionString, true);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateLogic"/> class.
        /// </summary>
        /// <param name="letterTemplateRepository">The letter template repository.</param>
        public LetterTemplateLogic(ILetterTemplateRepository letterTemplateRepository)
        {
            if (letterTemplateRepository != null)
            {
                _letterTemplateRepository = letterTemplateRepository;
            }
        }

        /// <summary>
        /// Adds the edit letter template.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public long Save(LetterTemplate letterTemplate)
        {
            return _letterTemplateRepository.Save(letterTemplate);
        }

        /// <summary>
        /// Gets the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public LetterTemplate GetLetterTemplateById(LetterTemplate letterTemplate)
        {
            return _letterTemplateRepository.GetLetterTemplateById(letterTemplate);
        }

        /// <summary>
        /// Determines whether [is letter name exists] [the specified letter template].
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public bool IsLetterNameExists(LetterTemplate letterTemplate)
        {
            return _letterTemplateRepository.IsLetterNameExists(letterTemplate);
        }

        /// <summary>
        /// Gets all letter templates.
        /// </summary>
        /// <param name="letterTemplateInfo">The letter template information.</param>
        /// <returns></returns>
        public LetterTemplateContainer GetLetterTemplates(LetterTemplateContainer letterTemplateInfo)
        {
            return _letterTemplateRepository.GetLetterTemplates(letterTemplateInfo);
        }

        /// <summary>
        /// Deletes the letter template.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        public bool Delete(LetterTemplate letterTemplateToDelete)
        {
            return _letterTemplateRepository.Delete(letterTemplateToDelete);
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        public bool InsertAuditLog(LetterTemplate letterTemplateToDelete)
        {
            return _letterTemplateRepository.InsertAuditLog(letterTemplateToDelete);
        }
    }
}
