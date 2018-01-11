using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Report
{
    // ReSharper disable once UnusedMember.Global
    public class LetterTemplateController : BaseController
    {
        /// <summary>
        /// The _letter template logic
        /// </summary>
        private readonly LetterTemplateLogic _letterTemplateLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateController"/> class.
        /// </summary>
        public LetterTemplateController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _letterTemplateLogic = new LetterTemplateLogic(bubbleDataSource);
        }

        /// <summary>
        /// Adds the edit letter template.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public long Save(LetterTemplate letterTemplate)
        {
            return _letterTemplateLogic.Save(letterTemplate);
        }

        /// <summary>
        /// Gets the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        [HttpPost]
        public LetterTemplate Get(LetterTemplate letterTemplate)
        {
            return _letterTemplateLogic.GetLetterTemplateById(letterTemplate);
        }

        /// <summary>
        /// Determines whether [is letter name exists] [the specified letter template].
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public bool IsLetterNameExists(LetterTemplate letterTemplate)
        {
            return _letterTemplateLogic.IsLetterNameExists(letterTemplate);
        }

        /// <summary>
        /// Gets all letter templates.
        /// </summary>
        /// <param name="letterTemplateInfo">The letter template information.</param>
        /// <returns></returns>
        [HttpPost]
        public LetterTemplateContainer GetAll(LetterTemplateContainer letterTemplateInfo)
        {
            return _letterTemplateLogic.GetLetterTemplates(letterTemplateInfo);
        }

        /// <summary>
        /// Deletes the letter template.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        [HttpPost]
        public bool Delete(LetterTemplate letterTemplateToDelete)
        {
            return _letterTemplateLogic.Delete(letterTemplateToDelete);
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        [HttpPost]
        public bool InsertAuditLog(LetterTemplate letterTemplate)
        {
            return _letterTemplateLogic.InsertAuditLog(letterTemplate);
        }
    }
}
