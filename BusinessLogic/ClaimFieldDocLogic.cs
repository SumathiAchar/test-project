using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
     public class ClaimFieldDocLogic
    {
        /// <summary>
        /// Initializes Repository
        /// </summary>
        private readonly IClaimFieldDocRepository _claimFieldDocsRepository;


        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldDocLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldDocLogic(string connectionString)
        {
            _claimFieldDocsRepository = Factory.CreateInstance<IClaimFieldDocRepository>(connectionString, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldDocLogic"/> class.
        /// </summary>
        /// <param name="claimFieldDocsRepository">The claim field docs repository.</param>
        public ClaimFieldDocLogic(IClaimFieldDocRepository claimFieldDocsRepository)
        {
            if (claimFieldDocsRepository != null)
            {
                _claimFieldDocsRepository = claimFieldDocsRepository;
            }
        }

        /// <summary>
        /// Adds the claim field docs.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public long AddClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsRepository.AddClaimFieldDocs(claimFieldDoc);
        }

        /// <summary>
        /// Gets the claim field docs.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimFieldDoc> GetClaimFieldDocs(ClaimFieldDoc claimFieldDoc)
        {
            List<ClaimFieldDoc> claimFieldDocList = _claimFieldDocsRepository.GetClaimFieldDocs(claimFieldDoc);
            if (claimFieldDocList != null && claimFieldDocList.Any() && claimFieldDocList.First().ClaimFieldValues != null)
            {
                List<ClaimFieldValue> claimFieldValueList = claimFieldDocList.First().ClaimFieldValues.ToList();
                if (claimFieldValueList.Any())
                {
                    claimFieldDocList.ForEach(
                       a => a.ClaimFieldValues = claimFieldValueList.Where(b => b.ClaimFieldDocId == a.ClaimFieldDocId).ToList());
                }
            }
            return claimFieldDocList;
        }
        
        /// <summary>
        /// Gets all claim fields.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ClaimField> GetAllClaimFields()
        {
            return _claimFieldDocsRepository.GetAllClaimFields();
        }

        /// <summary>
        /// Deletes the specified claim field document.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public bool Delete(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsRepository.Delete(claimFieldDoc);
        }
         
        /// <summary>
        /// Determines whether [is document in use] [the specified claim field document].
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<ContractLog> IsDocumentInUse(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsRepository.IsDocumentInUse(claimFieldDoc);
        }

        /// <summary>
        /// Rename Payment Table.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        public ClaimFieldDoc RenamePaymentTable(ClaimFieldDoc claimFieldDoc)
        {
            return _claimFieldDocsRepository.RenamePaymentTable(claimFieldDoc);
        }
    }
}

