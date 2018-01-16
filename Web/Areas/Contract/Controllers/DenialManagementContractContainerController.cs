using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    /// <summary>
    /// Is used to view contract specific details from CMS Display contract module.
    /// This controller can be accessed from other application with out logging into CMS. 
    /// At this point of time this controller is being called from denial management application.
    /// Authentication is required for this page and it is being taken care by SSO integarted with denial management application and CMS.
    /// </summary>
    /// 
    public class DenialManagementContractContainerController : CommonController
    {
        public ActionResult Index(long? contractId)
        {
            long? id = contractId;
            BaseModel obj = new BaseModel();
            
            Shared.Models.Contract contract = GetApiResponse<Shared.Models.Contract>("Contract", "GetContractFirstLevelDetails", id);
            long? nodeId = contract.NodeId;
            int facilityId = contract.FacilityId;
            long? parentId = contract.ParentId;
            obj.UserName = contract.UserName ?? GetLoggedInUserName();

            if (parentId != null || contractId != null)
            {
                if (contractId == null)
                    contractId = 0;

                Shared.Models.Contract contractInfo = new Shared.Models.Contract { FacilityId = facilityId, ContractId = contractId.Value, ParentId = parentId };

                ContractFullInfo contractFullInfo = PostApiResponse<ContractFullInfo>("Contract", "GetContractFullInfo", contractInfo);

                ContractViewModel viewModel = new ContractViewModel
                {
                    ContractId = contractId.Value,
                    ContractBasicInfo = new ContractBasicInfoViewModel(),
                    ContractContactIds = new List<long>(),
                    ContractNotes = new List<ContractNotesViewModel>(),
                    ContractUploadFiles = new List<ContractUploadFiles>(),
                    FacilityId = facilityId,
                    NodeId = nodeId,
                    PayerCode = contractFullInfo.ContractBasicInfo.PayerCode,
                    CustomField = contractFullInfo.ContractBasicInfo.CustomField
                };
                if (contractFullInfo.ContractBasicInfo != null)
                {
                    ContractBasicInfoViewModel contractBasicInfoViewModel =
                        GetContractBasicInfo(contractFullInfo.ContractBasicInfo, parentId, facilityId, nodeId);
                    viewModel.ContractBasicInfo = contractBasicInfoViewModel;
                }

                if (contractFullInfo.ContractContactIds != null && contractFullInfo.ContractContactIds.Any())
                {
                    viewModel.ContractContactIds = contractFullInfo.ContractContactIds;
                }

                if (contractFullInfo.ContractDocs != null && contractFullInfo.ContractDocs.Count > 0)
                {
                    List<ContractUploadFiles> contractUploadFiles = GetContractUploadFilesesInfo(contractFullInfo);
                    viewModel.ContractUploadFiles = contractUploadFiles;
                }

                if (contractFullInfo.ContractNotes != null)
                {
                    List<ContractNotesViewModel> contractNotes = GetContractNotesInfo(contractFullInfo);
                    viewModel.ContractNotes = contractNotes;
                }
                return View("~/Areas/Contract/Views/DenialManagementContractContainer/Index.cshtml", viewModel);
            }
            return View("~/Areas/Contract/Views/DenialManagementContractContainer/SSIMedworthHome.cshtml");
        }

        /// <summary>
        /// Gets the contract basic information.
        /// </summary>
        /// <param name="contractBasicInfo">The contract basic information.</param>
        /// <param name="parentId">The parent unique identifier.</param>
        /// <param name="facilityId">The facility unique identifier.</param>
        /// <param name="nodeId">The node unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private ContractBasicInfoViewModel GetContractBasicInfo(Shared.Models.Contract contractBasicInfo, long? parentId, int facilityId, long? nodeId)
        {
            ContractBasicInfoViewModel contractBasicInfoViewModel = AutoMapper.Mapper.Map<Shared.Models.Contract, ContractBasicInfoViewModel>(contractBasicInfo);

            if (contractBasicInfo.Payers != null && contractBasicInfo.Payers.Count > 0)
            {
                contractBasicInfoViewModel.ParentId = parentId;
                contractBasicInfoViewModel.FacilityId = facilityId;
                contractBasicInfoViewModel.NodeId = nodeId;
                contractBasicInfoViewModel.AvailablePayerList = new List<ContractPayerViewModel>();
                contractBasicInfoViewModel.SelectedPayerList = new List<ContractPayerViewModel>();

                foreach (var payer in contractBasicInfo.Payers)
                {
                    if (payer.IsSelected)
                        contractBasicInfoViewModel.SelectedPayerList.Add(new ContractPayerViewModel
                        {
                            Name = payer.PayerName
                        });
                    else
                        contractBasicInfoViewModel.AvailablePayerList.Add(new ContractPayerViewModel
                        {
                            Name = payer.PayerName
                        });

                }
                contractBasicInfoViewModel.AvailablePayerList = contractBasicInfoViewModel.AvailablePayerList.OrderBy(q => q.Name).ToList();
                contractBasicInfoViewModel.SelectedPayerList = contractBasicInfoViewModel.SelectedPayerList.OrderBy(q => q.Name).ToList();
            }
            return contractBasicInfoViewModel;
        }

        /// <summary>
        /// Gets the contract notes information.
        /// </summary>
        /// <param name="contractFullInfo">The contract full information.</param>
        /// <returns></returns>
        private List<ContractNotesViewModel> GetContractNotesInfo(ContractFullInfo contractFullInfo)
        {
            List<ContractNotesViewModel> contractNotes = new List<ContractNotesViewModel>();
            contractFullInfo.ContractNotes.ForEach(
                note => contractNotes.Add(AutoMapper.Mapper.Map<ContractNote, ContractNotesViewModel>(note)));
            return contractNotes;
        }

        /// <summary>
        /// Gets the contract upload fileses information.
        /// </summary>
        /// <param name="contractFullInfo">The contract full information.</param>
        /// <returns></returns>
        private List<ContractUploadFiles> GetContractUploadFilesesInfo(ContractFullInfo contractFullInfo)
        {
            List<ContractUploadFiles> contractUploadFiles = new List<ContractUploadFiles>();
            contractFullInfo.ContractDocs.ForEach(
                doc => contractUploadFiles.Add(AutoMapper.Mapper.Map<ContractDoc, ContractUploadFiles>(doc)));
            return contractUploadFiles;
        }
    }
}
