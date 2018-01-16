using System.Globalization;
using AutoMapper;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractContainerController : SessionStoreController
    {

        public ActionResult HomeIndex()
        {

            return GetUserTypeId() == (int)Enums.UserRoles.SsiAdmin ?
                RedirectToAction("FacilityAndUserHomePage", "HomePage", new { area = "UserManagement" }) :
                RedirectToAction("Index", "ContractContainer", new { area = "Contract" });
        }



        /// <summary>
        /// Indexes the specified contract identifier.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <param name="nodeId">The node identifier.</param>
        /// <param name="isParentNode">The is parent node.</param>
        /// <param name="modelParentId">The model parent identifier.</param>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        public ActionResult Index(long? contractId, long? parentId, int? facilityId, long? nodeId, bool? isParentNode, long? modelParentId, string currentDateTime)
        {
            // Sample code to get the facility list and the permissions
            //TODO: From UI nodeID is coming in ContractID parameter which is wrong, Need to do changes if required later.
            if (nodeId.HasValue)
            {
                LastExpandedNodeId = nodeId;
                LastHighlightedNodeId = nodeId;
                if (parentId.HasValue && facilityId.HasValue)//TODO Janaki
                {
                    if (LastRequestedNode == null)
                        LastRequestedNode = new ContractHierarchy();
                    LastRequestedNode.NodeId = nodeId.Value;
                    LastRequestedNode.ParentId = parentId.Value;
                    LastRequestedNode.FacilityId = facilityId.Value;
                }
            }

            if (facilityId != null && (parentId != null || contractId != null))
            {
                if (contractId == null)
                    contractId = 0;

                Shared.Models.Contract contractInfo = new Shared.Models.Contract
                {
                    FacilityId = facilityId.Value,
                    ContractId = contractId.Value,
                    UserName = GetLoggedInUserName(),
                    CurrentDateTime = currentDateTime
                };
                ContractFullInfo contractFullInfo = PostApiResponse<ContractFullInfo>(Constants.Contract, Constants.GetContractFullInfo,
                                                                                      contractInfo);
                ContractViewModel viewModel = new ContractViewModel
                {
                    ContractId = contractId.Value,
                    ContractBasicInfo = new ContractBasicInfoViewModel(),
                    ContractContactIds = new List<long>(),
                    ContractNotes = new List<ContractNotesViewModel>(),
                    ContractUploadFiles = new List<ContractUploadFiles>(),
                    FacilityId = facilityId.Value,
                    NodeId = nodeId,
                    PayerCode = contractFullInfo.ContractBasicInfo != null ? contractFullInfo.ContractBasicInfo.PayerCode : string.Empty,
                    CustomField = contractFullInfo.ContractBasicInfo != null ? contractFullInfo.ContractBasicInfo.CustomField : null,
                    IsParentNode = isParentNode,
                    ModelParentId = modelParentId
                };
                ContractBasicInfoViewModel contractBasicInfoViewModel =
                    GetContractBasicInfo(contractFullInfo.ContractBasicInfo, parentId, facilityId.Value, contractFullInfo.ContractBasicInfo != null ? contractFullInfo.ContractBasicInfo.NodeId : null);
                viewModel.ContractBasicInfo = contractBasicInfoViewModel;

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
                ViewBag.CurrentFacilityId = GetCurrentFacilityId();
                return View("Index", viewModel);
            }
            ViewBag.CurrentFacilityId = GetCurrentFacilityId();
            ViewBag.CurrentFacilityName = GetCurrentFacilityName();
            ViewBag.UserName = GetLoggedInUserName();

            UserInfo userinfo = GetUserInfo();
            if (userinfo.SingleFacility != null && !userinfo.IsFromSecurityPage)
            {
                userinfo.SingleFacility = 0;
            }
            else if (userinfo.AssignedFacilities.Count == 1)
            {
                userinfo.SingleFacility = 1;
                // ReSharper disable once SimplifyConditionalTernaryExpression
                userinfo.IsFromSecurityPage = (userinfo.IsFromSecurityPage) ? false : userinfo.IsFromSecurityPage;
            }
            ViewBag.SingleFacility = userinfo.SingleFacility;
            ViewBag.UserId = userinfo.UserId;
            ViewBag.UserTypeId = userinfo.UserTypeId;
            ViewBag.LastLoginDate = userinfo.LastLoginDate;
            ViewBag.PasswordExpirationDays = userinfo.PasswordExpirationDays;
            ViewBag.LandingPageId = userinfo.LandingPageId;
            ViewBag.IsAutoRefresh = GetAutoRefresh();

            return View("~/Areas/Contract/Views/ContractContainer/SSIMedworthHome.cshtml");
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
        private ContractBasicInfoViewModel GetContractBasicInfo(Shared.Models.Contract contractBasicInfo, long? parentId, int facilityId, long? nodeId)//TODO Janaki
        {
            ContractBasicInfoViewModel contractBasicInfoViewModel = Mapper.Map<Shared.Models.Contract, ContractBasicInfoViewModel>(contractBasicInfo);

            if (contractBasicInfo != null)
            {
                if (contractBasicInfo.Payers != null && contractBasicInfo.Payers.Count > 0)
                {
                    contractBasicInfoViewModel.ParentId = parentId;
                    contractBasicInfoViewModel.FacilityId = facilityId;
                    contractBasicInfoViewModel.NodeId = nodeId;
                    contractBasicInfoViewModel.AvailablePayerList = new List<ContractPayerViewModel>();
                    contractBasicInfoViewModel.SelectedPayerList = new List<ContractPayerViewModel>();
                    if (nodeId == null)
                    {
                        contractBasicInfoViewModel.ThresholdDaysToExpireAlters =
                            GlobalConfigVariables.DefaultThresholdDaysToExpireAlters;
                    }
                    foreach (var payer in contractBasicInfo.Payers.Where(payer => payer != null))
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
            }
            else //if Contract view model is null.
            {
                contractBasicInfoViewModel = new ContractBasicInfoViewModel
                {
                    ThresholdDaysToExpireAlters = GlobalConfigVariables.DefaultThresholdDaysToExpireAlters
                };
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
                note => contractNotes.Add(Mapper.Map<ContractNote, ContractNotesViewModel>(note)));
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
                doc => contractUploadFiles.Add(Mapper.Map<ContractDoc, ContractUploadFiles>(doc)));
            return contractUploadFiles;
        }


        //TODO: Delete below method it is not used till yet (Need to confirm with other developer)
        /// <summary>
        /// Copy Contract
        /// </summary>
        /// <param name="contractId">The contract ID.</param>
        /// <param name="nodeId">The node unique identifier.</param>
        /// <returns></returns>
        public ActionResult CopyContract(long? contractId, long? nodeId)
        {
            Shared.Models.Contract contracts = new Shared.Models.Contract();
            if (contractId != null) contracts.ContractId = (long)contractId;
            LastExpandedNodeId = nodeId;
            LastHighlightedNodeId = nodeId;
            //Get the UserName logged in
            contracts.UserName = GetLoggedInUserName();
            long id = PostApiResponse<long>("Contract", "CopyContractById", contracts);
            return RedirectToAction("Index", "ContractContainer", id);
        }

        /// <summary>
        /// Gets the adjudicated contract names.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="userText"></param>
        /// <returns></returns>
        public JsonResult GetAdjudicatedContracts(long modelId, string userText)
        {
            ContractViewModel contractViewModel = new ContractViewModel
            {
                ModelId = modelId,
                Values = userText
            };
            Shared.Models.Contract contract = Mapper.Map<ContractViewModel, Shared.Models.Contract>(contractViewModel);
            List<Shared.Models.Contract> contractDetails = PostApiResponse<List<Shared.Models.Contract>>(Constants.Contract, Constants.GetAdjudicatedContracts, contract);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.AddRange(
                contractDetails.Select(
                    requesteritem =>
                        new SelectListItem
                        {
                            Value = requesteritem.ContractId.ToString(CultureInfo.InvariantCulture),
                            Text = requesteritem.ContractName
                        }));
            return Json(selectListItems, JsonRequestBehavior.AllowGet);
        }



    }
}
