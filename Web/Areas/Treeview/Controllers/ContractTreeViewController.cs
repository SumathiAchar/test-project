using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Treeview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.Treeview.Controllers
{
    public class ContractTreeViewController : SessionStoreController
    {

        /// <summary>
        /// Contracts the tree view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractTreeView()
        {
            return View();
        }

        /// <summary>
        /// Gets the tree view data.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTreeViewData(ContractHierarchy values)
        {
            SetCurrentFacilityIdToSession(values);

            //FIXED-FEB16 - We can move below code(line 35-81) into separate method called UpdateContractHierarchyData

            ContractHierarchy data = UpdateContractHierarchyData(values);

            List<TreeViewModel> treenodes = new List<TreeViewModel>();
            UserInfo userInfo = GetUserInfo();

            if (data.FacilityId != 0)
            {
                GetTreeviewNodesByFacilityDetails(values, data, treenodes, userInfo);
            }
            else
            {
                AddFacilityDetails(treenodes, userInfo);
            }

            //FIXED-FEB16 - Remove unnecessary commented code. 

            //Sort Contract and Service type according to name in tree nodes
            treenodes = SortTreeView(treenodes);
            return Json(treenodes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the contract hierarchy data.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private ContractHierarchy UpdateContractHierarchyData(ContractHierarchy values)
        {
            ContractHierarchy data = new ContractHierarchy
            {
                NodeId = values.NodeId,
                ParentId = values.ParentId,
                FacilityId = values.FacilityId,
            };

            // if the values.NodeId <> 0 that means that user has clicked on the node which was not expanded before. 
            // To expand the clicked node, we need get the data from database, and need to bind it on UI
            if (values.NodeId != 0)
            {
                data = new ContractHierarchy
                {
                    NodeId = values.NodeId,
                    ParentId = values.ParentId,
                    FacilityId = values.FacilityId
                };
                // Setting the LastRequestedNode property which internally sets the Session["LastRequestedNode"]. This session is required to set in order to get the last selected node again.
                LastRequestedNode = values;
                LastExpandedNodeId = values.NodeId;
            }
            else
            {
                // If the Session["LastRequestedNode"] is present then we have to get the tree view data based on the session values.
                if (LastRequestedNode != null && LastRequestedNode.NodeId != 0)
                {
                    data.NodeId = LastRequestedNode.NodeId;
                    data.ParentId = LastRequestedNode.ParentId;
                    data.FacilityId = LastRequestedNode.FacilityId;
                }
            }
            
            data.CommandTimeoutForContractHierarchy = Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForContractHierarchy);
            
            if (data.FacilityId == 0)//TODO Janaki
                data.FacilityId = GetCurrentFacilityId();
            return data;
        }

        /// <summary>
        /// Gets the treeview nodes by facility details.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="data">The data.</param>
        /// <param name="treenodes">The tree nodes.</param>
        /// <param name="userInfo">The user information.</param>
        private void GetTreeviewNodesByFacilityDetails(ContractHierarchy values, ContractHierarchy data, List<TreeViewModel> treenodes, UserInfo userInfo)
        {
            if (LastDeletedNodeId != null)
            {
                data.NodeId = LastDeletedNodeId.Value;
                LastDeletedNodeId = null;
            }

            List<ContractHierarchy> contractList = PostApiResponse<List<ContractHierarchy>>(Constants.ContractTreeView, Constants.GetContractHierarchy, data);
            foreach (FacilityViewModel facility in userInfo.AssignedFacilities)
            {
                TreeViewModel parentNode = new TreeViewModel
                {
                    //FIXED-FEB16 - read string value from constants. Do the same for other places also.  
                    spriteCssClass = Constants.CssFacilityNodeClass,
                    FacilityId = facility.FacilityId,
                    NodeText = facility.FacilityDisplayName
                };

                if (data.FacilityId == facility.FacilityId && contractList != null && contractList.Any())
                {
                    foreach (ContractHierarchy parentHierarchy in contractList.Where(contract => contract.ParentId.HasValue && contract.ParentId.Value == 0))
                    {
                        //To expand parent node by default
                        parentNode.expanded = true;
                        parentNode.spriteCssClass = string.Format("{0}  {1}", Constants.CssFacilityNodeClass,
                            Constants.CssHighlight);

                        // Finding if facility is having any node under it.
                        if (contractList.Any(model => model.ParentId.HasValue && model.ParentId == parentHierarchy.NodeId))
                        {
                            // Getting a list of primary and forecast node under the facility.
                            List<ContractHierarchy> primaryNodeList =
                                contractList.Where(primaryNodeItem => primaryNodeItem.ParentId.HasValue && primaryNodeItem.ParentId == parentHierarchy.NodeId).
                                    ToList();

                            LoadChildNodes(values, data, contractList, parentNode, primaryNodeList);
                        }
                        else
                        {
                            // We are adding dummy child for all the nodes which are having some data but we didn't fetch them due to performance. The data will be fetched on click on them
                            TreeViewModel dummyChild = new TreeViewModel { NodeText = Constants.Processing };
                            parentNode.IsExpanded = true;
                            parentNode.expanded = false;
                            parentNode.items = new List<TreeViewModel> { dummyChild };
                        }
                    }
                }
                else
                {
                    TreeViewModel dummyChild = new TreeViewModel { NodeText = Constants.Processing };
                    parentNode.IsExpanded = true;
                    parentNode.items = new List<TreeViewModel> { dummyChild };
                }

                treenodes.Add(parentNode);
            }
        }

        /// <summary>
        /// Loads the child nodes.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="data">The data.</param>
        /// <param name="contractList">The contract list.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="primaryNodeList">The primary node list.</param>
        private void LoadChildNodes(ContractHierarchy values, ContractHierarchy data, List<ContractHierarchy> contractList, TreeViewModel parentNode, IEnumerable<ContractHierarchy> primaryNodeList)
        {
            // Looping through the primary and foretasted nodes
            foreach (ContractHierarchy primaryNodeHierarchy in primaryNodeList)
            {
                TreeViewModel primaryNode =
                    AutoMapper.Mapper.Map<ContractHierarchy, TreeViewModel>(primaryNodeHierarchy);
                bool isLastExpanded = LastExpandedNodeId.HasValue &&
                                      contractList.Any(a => a.NodeId == LastExpandedNodeId);
                //If the last expanded NodeId is has some value then tree view need to be expanded till that node.
                if (isLastExpanded)
                {
                    primaryNode.expanded = parentNode.expanded = true;
                }
                else
                {
                    //This one is required when we are clicking on node whose data is not present and we are going to get it from database. 
                    // by default the clicked node should be expanded. Below code will do that.
                    primaryNode.expanded = values.NodeId == primaryNode.NodeId;
                    parentNode.expanded = values.NodeId == primaryNode.NodeId || parentNode.expanded;
                }

                primaryNode.NodeText += " " + primaryNode.AppendString;

                //To expand primary node by default
                primaryNode.expanded = true;

                //spriteCssClass property is set to give menu from JS file. This menu will be different for different type of nodes.
                primaryNode.spriteCssClass = primaryNode.IsPrimaryNode
                                                ? Constants.CssPrimaryModel
                                                : Constants.CssSecondaryModel;

                if (LastExpandedNodeId == primaryNode.NodeId)
                {
                    primaryNode.spriteCssClass = primaryNode.IsPrimaryNode
                                               ? Constants.CssHighlightedPrimaryModel
                                               : Constants.CssHighlightedSecondaryModel;
                }

                // Below method is going to add child nodes
                AddChildNode(primaryNode, contractList, primaryNode.IsPrimaryNode);

                if (parentNode.items == null)
                {
                    parentNode.items = new List<TreeViewModel>();
                }
                parentNode.items.Add(primaryNode);
            }
            if (data.NodeId > 0 && data.ParentId == 0)
            {
                parentNode.spriteCssClass = string.Format("{0}  {1}", Constants.CssFacilityNodeClass,
                            Constants.CssHighlight);
            }
        }

        /// <summary>
        /// Adds the facility details.
        /// </summary>
        /// <param name="treenodes">The tree nodes.</param>
        /// <param name="userInfo">The user information.</param>
        private static void AddFacilityDetails(List<TreeViewModel> treenodes, UserInfo userInfo)
        {
            treenodes.AddRange(userInfo.AssignedFacilities.Select(facility => new TreeViewModel
            {
                //FIXED-FEB16 - No need to assign false value to expanded & NodeId as zero as by default value will be false for bool property & zero for long property
                spriteCssClass = Constants.CssFacilityNodeClass,
                FacilityId = facility.FacilityId,
                NodeText = facility.FacilityDisplayName,
                IsExpanded = true,
                NodeDisplayText = facility.FacilityDisplayName,
                items = new List<TreeViewModel> { new TreeViewModel { NodeText = Constants.Processing } }
            }));
        }

        /// <summary>
        /// Sets the current facility identifier to session.
        /// </summary>
        /// <param name="values">The values.</param>
        private void SetCurrentFacilityIdToSession(ContractHierarchy values)
        {
            if (values.FacilityId > 0)
            {
                //FIXED-FEB16 - both if condition we can merge and add into single if. 
                if ((GetCurrentFacilityId() != values.FacilityId) && LastRequestedNode != null)
                {
                    LastRequestedNode.NodeId = 0;
                    LastRequestedNode.ParentId = 0;
                }
                SetCurrentFacilityId(values.FacilityId);
            }
        }

        /// <summary>
        /// Sort Contract and Service type according to name in tree nodes
        /// </summary>
        /// <param name="treenodes">List of TreeViewModel</param>
        /// <returns>Sorted TreeViewModel list</returns>
        private List<TreeViewModel> SortTreeView(List<TreeViewModel> treenodes)
        {
            if (treenodes != null && treenodes.Count > 0)
            {
                IEnumerable<TreeViewModel> nonCurrentNodes = from facilityNode in treenodes
                                                             where facilityNode.items != null && facilityNode.items.Count > 0
                                                             from topNode in facilityNode.items
                                                             where topNode != null && topNode.items != null && topNode.items.Count > 0
                                                             from activeInactiveNode in topNode.items
                                                             where
                                                                 activeInactiveNode != null && activeInactiveNode.items != null &&
                                                                 activeInactiveNode.items.Count > 0
                                                             from currentNonCurrentNode in activeInactiveNode.items
                                                             where
                                                                 currentNonCurrentNode != null && currentNonCurrentNode.items != null &&
                                                                 currentNonCurrentNode.items.Count > 0
                                                             select currentNonCurrentNode;

                foreach (TreeViewModel nonCurrentNode in nonCurrentNodes)
                {
                    foreach (TreeViewModel contractNode in nonCurrentNode.items.Where(contract => contract != null && contract.items != null))
                    {
                        contractNode.items = contractNode.items.OrderBy(a => a.NodeText).ToList();
                        if (contractNode.items.Any(a => a.NodeText == Constants.CarveOutNodeText))
                        {
                            TreeViewModel carveOut = contractNode.items.First(a => a.NodeText == Constants.CarveOutNodeText);
                            contractNode.items.RemoveAll(a => a.NodeText == Constants.CarveOutNodeText);
                            carveOut.items = carveOut.items.OrderBy(a => a.NodeText).ToList();
                            contractNode.items.Add(carveOut);
                        }
                    }
                    nonCurrentNode.items = nonCurrentNode.items.OrderBy(a => a.NodeText).ToList();
                }
            }
            return treenodes;
        }

        /// <summary>
        /// Adds the child node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="contractList">The contract list.</param>
        /// <param name="isPrimaryModelContract">if set to <c>true</c> [is primary model contract].</param>
        /// <returns></returns>
        private void AddChildNode(TreeViewModel parentNode, List<ContractHierarchy> contractList, bool isPrimaryModelContract)
        {
            bool isLastExpanded = false;
            // if there are any child available for the parent we are finding them and adding them to tree view
            if (contractList.Any(a => a.ParentId == parentNode.NodeId))
            {
                List<ContractHierarchy> allChild = contractList.Where(a => a.ParentId == parentNode.NodeId).ToList();
                foreach (ContractHierarchy childHierarchy in allChild)
                {
                    TreeViewModel child = AutoMapper.Mapper.Map<ContractHierarchy, TreeViewModel>(childHierarchy);
                    child.NodeText += " " + child.AppendString;

                    // if the current child is having any child available then we are first adding sub child to current child
                    if (contractList.Any(a => a.ParentId == childHierarchy.NodeId))
                    {
                        // Recursion call to add child into current child
                        AddChildNode(child, contractList, isPrimaryModelContract);
                    }
                    else
                    {
                        // Finding if the current child is a contract or not
                        if (childHierarchy.IsContract.HasValue && childHierarchy.IsContract.Value)
                        {
                            child.spriteCssClass = Constants.CssContracts;
                            child.IsPrimaryModelContract = isPrimaryModelContract;

                            // Contract will have service type as a child. Below code will service type into tree view.
                            if (childHierarchy.Nodes != null)
                            {
                                //child.items =AutoMapper.Mapper.Map<List<ContractHierarchy>, List<TreeViewModel>>(childHierarchy.nodes);
                                child.items = AutoMapper.Mapper.Map<List<ContractHierarchy>, List<TreeViewModel>>(childHierarchy.Nodes.Where(x => x.IsCarveOut == false).ToList());

                                foreach (var node in child.items)
                                {
                                    node.spriteCssClass = Constants.CssServicetype;

                                    if (LastExpandedNodeId != null && child.NodeId == LastExpandedNodeId.Value)
                                    {
                                        child.expanded = true;
                                        if (child.items.Any(a => a.ContractServiceTypeId == LastHighlightedNodeId))
                                        {
                                            child.items.Where(a => a.ContractServiceTypeId == LastHighlightedNodeId)
                                                .ToList()
                                                .ForEach(
                                                    a =>
                                                        a.spriteCssClass =
                                                            string.Format("{0}  {1}", Constants.CssServicetype,
                                                                Constants.CssHighlight));
                                        }
                                        else if (LastHighlightedNodeId == child.NodeId)
                                            child.spriteCssClass = string.Format("{0}  {1}", Constants.CssContracts,
                                                                Constants.CssHighlight);
                                    }
                                    else
                                        child.expanded = false;
                                }

                                if (childHierarchy.Nodes.Any(x => x.IsCarveOut))
                                {
                                    child.items.Add(new TreeViewModel
                                    {
                                        NodeText = Constants.CarveOutNodeText,
                                    });

                                    foreach (ContractHierarchy contractHierarchy in
                                            childHierarchy.Nodes.Where(x => x.IsCarveOut))
                                        AddCarveOutItems(child.items.Last(), contractHierarchy, child);

                                    if (LastExpandedNodeId != null && child.NodeId == LastExpandedNodeId.Value)
                                        child.expanded = true;
                                }
                            }
                            else
                            {
                                if (LastHighlightedNodeId == child.NodeId)
                                {
                                    child.spriteCssClass = string.Format("{0}  {1}", Constants.CssContracts,
                                                                Constants.CssHighlight);
                                }
                            }

                            // If the last expanded node ID is present in session and that ID is same as Contract's Node ID then we need to expand our tree view till this contract.
                            if (LastExpandedNodeId.HasValue && LastExpandedNodeId.Value == childHierarchy.NodeId)
                            {
                                isLastExpanded = true;
                            }
                        }
                    }

                    // For the first time when parent node will come here, it will not have any items into this. So first checking null if it null then we are initializing the items.
                    if (parentNode.items == null)
                    {
                        parentNode.items = new List<TreeViewModel>();

                    }

                    // We have three condition based on that we have to expand the node.
                    // If node was expanded last time, then we have to expand it again based on the LastExpandedNodeId check. 
                    // If the child is expanded then we have to expand the parent as well.
                    if (isLastExpanded || child.expanded ||
                        (LastExpandedNodeId.HasValue && LastExpandedNodeId.Value == parentNode.NodeId))
                    {
                        parentNode.expanded = true;
                    }


                    parentNode.items.Add(child);
                }
            }
            else
            {
                // We are adding dummy child for all the nodes which are having some data but we didn't fetch them due to performance. The data will be fetched on click on them
                TreeViewModel dummyChild = new TreeViewModel { NodeText = Constants.Processing };
                parentNode.IsExpanded = true;
                parentNode.expanded = false;
                parentNode.items = new List<TreeViewModel> { dummyChild };
            }
        }

        /// <summary>
        /// Adds the carve out items.
        /// </summary>
        /// <param name="treeViewModel">The tree view model.</param>
        /// <param name="contractHierarchy">The contract hierarchy.</param>
        /// <param name="child">The child.</param>
        private void AddCarveOutItems(TreeViewModel treeViewModel, ContractHierarchy contractHierarchy, TreeViewModel child)
        {
            if (treeViewModel.items == null)
                treeViewModel.items = new List<TreeViewModel>();

            treeViewModel.items.Add(new TreeViewModel
            {
                NodeText = contractHierarchy.NodeText,

                spriteCssClass = Constants.CssServicetype,
                NodeId = contractHierarchy.NodeId,
                ContractId = contractHierarchy.ContractId,
                ParentId = contractHierarchy.ParentId,

                ContractServiceTypeId = contractHierarchy.ContractServiceTypeId

            });

            if (LastExpandedNodeId != null && contractHierarchy.NodeId == LastExpandedNodeId.Value)
            {

                if (treeViewModel.items.Any(a => a.ContractServiceTypeId == LastHighlightedNodeId))
                {
                    treeViewModel.expanded = true;

                    treeViewModel.items.Where(a => a.ContractServiceTypeId == LastHighlightedNodeId)
                        .ToList()
                        .ForEach(a => a.spriteCssClass = string.Format("{0}  {1}", Constants.CssServicetype,
                                                                Constants.CssHighlight));
                }
                else if (LastHighlightedNodeId == child.NodeId)
                {
                    child.spriteCssClass = string.Format("{0}  {1}", Constants.CssContracts,
                                                                Constants.CssHighlight);
                    child.expanded = true;
                }
                treeViewModel.expanded = true;
            }
        }


        /// <summary>
        /// Renames the contract.
        /// </summary>
        /// <param name="renamerequest">The rename request.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RenameContract(TreeViewModel renamerequest)
        {
            //Get the UserName logged in
            renamerequest.UserName = GetLoggedInUserName();
            ContractHierarchy contracts = new ContractHierarchy
            {
                NodeId = renamerequest.NodeId,
                NodeText = renamerequest.NodeText,
                UserName = renamerequest.UserName,
                ParentId = renamerequest.ParentId,
                FacilityId = renamerequest.FacilityId
            };
            // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
            LastExpandedNodeId = renamerequest.NodeId;
            LastHighlightedNodeId = renamerequest.NodeId;

            if (renamerequest.ContractId == 0)
                LastRequestedNode = contracts;

            contracts = PostApiResponse<ContractHierarchy>(Constants.Contract, Constants.RenameContract, contracts);
            return Json(contracts);
        }

        /// <summary>
        /// Copies the contract by node and parent id.
        /// </summary>
        /// <param name="copyrequest">The copy request.</param>
        /// <returns></returns>
        /// WID-1922
        [HttpPost]
        public JsonResult CopyContractByNodeAndParentId(TreeViewModel copyrequest)
        {
            ContractHierarchy moduleToCopy = new ContractHierarchy { NodeId = copyrequest.NodeId, ParentId = copyrequest.ParentId, NodeText = copyrequest.RenameText };
            //Get the UserName logged in
            string currentUserName = GetLoggedInUserName();
            moduleToCopy.UserName = currentUserName;
            moduleToCopy.CommandTimeoutForContractHierarchy = Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForContractHierarchy);
            
            moduleToCopy.LoggedInUserName = currentUserName;
            long insertedModelId = PostApiResponse<long>(Constants.ContractTreeView, Constants.CopyContractByNodeAndParentId, moduleToCopy);
            if (insertedModelId > 0 && copyrequest.ParentId.HasValue && copyrequest.FacilityId > 0)//TODO Janaki
            {
                // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
                LastExpandedNodeId = insertedModelId;
                LastHighlightedNodeId = insertedModelId;
                if (LastRequestedNode == null)
                    LastRequestedNode = new ContractHierarchy();
                LastRequestedNode.NodeId = insertedModelId;
                LastRequestedNode.ParentId = copyrequest.ParentId;
                LastRequestedNode.FacilityId = copyrequest.FacilityId;
            }
            return Json(moduleToCopy);
        }

        /// <summary>
        /// Copies the contract by node and parent id.
        /// </summary>
        /// <param name="copyrequest">The copy request.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopyContractById(TreeViewModel copyrequest)
        {
            ContractHierarchy contractHierarchy = new ContractHierarchy
            {
                NodeId = copyrequest.NodeId,
                ContractId = copyrequest.ContractId,
                ContractName = copyrequest.RenameText,
                UserName = GetLoggedInUserName(),
                CommandTimeoutForContractHierarchy =
                    Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForContractHierarchy)
            };

            //Get the UserName logged in
            long contarctid = PostApiResponse<long>(Constants.ContractTreeView, Constants.CopyContractByContractId, contractHierarchy);
            bool isSuccess = contarctid != 0;
            // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
            LastExpandedNodeId = contarctid;
            LastHighlightedNodeId = contarctid;
            return Json(new { sucess = isSuccess, addedId = contarctid });
        }

        /// <summary>
        /// Deletes the node and contract by node id.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="parentId">The parent unique identifier.</param>
        /// <param name="isModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteNodeAndContractByNodeId(long? nodeId, long? parentId, bool isModel)
        {
            bool isSuccess = false;
            if (nodeId.HasValue && parentId.HasValue)
            {
                //Get the UserName logged in
                ContractHierarchy contractHierarchy = new ContractHierarchy { UserName = GetLoggedInUserName(), NodeId = (long)nodeId };
                // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
                LastExpandedNodeId = parentId;
                LastHighlightedNodeId = parentId;

                // If deleted node is model the LastRequestedNode.NodeId we are setting to current node id
                if (isModel)
                {
                    LastDeletedNodeId = nodeId;
                    if (LastRequestedNode != null)
                        LastRequestedNode.NodeId = Convert.ToInt64(nodeId);
                }

                isSuccess = PostApiResponse<bool>(Constants.ContractTreeView, Constants.DeleteNodeAndContractByNodeId,
                                                         contractHierarchy);
            }
            return Json(new { sucess = isSuccess });
        }

        /// <summary>
        /// Deletes the contract service type by Id
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <param name="parentId">The parent unique identifier.</param>
        /// <returns>
        /// JsonResult.
        /// </returns>
        public JsonResult DeleteContractServiceTypeById(long? id, long? parentId)
        {
            bool isSuccess = false;
            if (id.HasValue)
            {
                //Get the UserName logged in
                ContractHierarchy contractHierarchy = new ContractHierarchy
                {
                    UserName = GetLoggedInUserName(),
                    ContractServiceTypeId = (long)id
                };
                // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
                LastExpandedNodeId = parentId;
                LastHighlightedNodeId = parentId;

                isSuccess = PostApiResponse<bool>(Constants.ContractTreeView, Constants.DeleteContractServiceTypeById,
                                                         contractHierarchy);

            }
            return Json(new { sucess = isSuccess });
        }

        //To Copy Contract ServiceType
        public ActionResult CopyContractServieType()
        {
            return View();
        }

        //To Copy Contract ServiceType
        public ActionResult CopyContractType()
        {
            return View();
        }

        /// <summary>
        /// Copies the service type by node and service type by id.
        /// </summary>
        /// <param name="copyrequest">The copy request.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CopyContractServiceTypeById(TreeViewModel copyrequest)
        {
            //Get the UserName logged in

            ContractServiceType contratServiceType = new ContractServiceType
            {
                ContractServiceTypeName = copyrequest.RenameText,
                ContractServiceTypeId = copyrequest.ContractServiceTypeId,
                UserName = GetLoggedInUserName(),
                CommandTimeoutForContractHierarchyCopyContractServiceTypeById =
                    Convert.ToInt32(GlobalConfigVariable.CommandTimeoutForContractHierarchy)
            };

            long contarctid = PostApiResponse<long>(Constants.ContractServiceType, Constants.CopyContractServiceTypeById, contratServiceType);
            bool isSuccess = contarctid != 0;
            // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
            LastExpandedNodeId = copyrequest.NodeId;//copyrequest.NodeId;
            LastHighlightedNodeId = contarctid;
            return Json(new { sucess = isSuccess, addedId = contarctid });
        }

        /// <summary>
        /// Renames the contract service type.
        /// </summary>
        /// <param name="renamerequest">The rename request.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RenameContractServiceType(TreeViewModel renamerequest)
        {
            ContractServiceType contratServiceType = new ContractServiceType
            {
                ContractServiceTypeName = renamerequest.NodeText,
                ContractServiceTypeId = renamerequest.ContractServiceTypeId,
                UserName = GetLoggedInUserName()
            };
            // LastExpandedNodeId is set to store the nodeID into session and on the next page load, it expands the tree view to node location.
            LastExpandedNodeId = renamerequest.NodeId;
            LastHighlightedNodeId = renamerequest.ContractServiceTypeId;
            long serviceType = PostApiResponse<long>(Constants.ContractServiceType, Constants.RenameContractServiceType, contratServiceType);
            return Json(serviceType);
        }

        /// <summary>
        /// Checks the model name is unique.
        /// </summary>
        /// <param name="copyRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsModelNameExit(TreeViewModel copyRequest)
        {
            ContractHierarchy contractHierarchy = new ContractHierarchy { NodeId = copyRequest.NodeId, ParentId = copyRequest.ParentId, NodeText = copyRequest.RenameText, FacilityId = copyRequest.FacilityId };
            bool data = PostApiResponse<bool>(Constants.ContractTreeView, Constants.IsModelNameExit, contractHierarchy);
            return Json(data);
        }
    }
}
