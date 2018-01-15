using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class ReassignClaimLogicTest
    {
        private ReassignClaimLogic _target;
        private Mock<IReassignClaimRepository> _mockReassignClaimRepository;

        /// <summary>
        /// Reassigns the claim logic logic paremeterless constructor.
        /// </summary>
        [TestMethod]
        public void ReassignClaimLogicLogicParemeterlessConstructor()
        {
            _target = new ReassignClaimLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(ReassignClaimLogic));
        }

        /// <summary>
        /// Reassigns the claim logic constructor.
        /// </summary>
        [TestMethod]
        public void ReassignClaimLogicConstructor()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            Assert.IsInstanceOfType(_target, typeof(ReassignClaimLogic));
        }

        /// <summary>
        /// Reassigns the claim logic for null constructor.
        /// </summary>
        [TestMethod]
        public void ReassignClaimLogicForNullConstructor()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            _target = new ReassignClaimLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(_target, typeof(ReassignClaimLogic));
        }

        /// <summary>
        /// Gets the reassign grid data test.
        /// </summary>
        [TestMethod]
        public void GetReassignGridDataTest()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            ClaimSearchCriteria claimSearchCriteria = new ClaimSearchCriteria { DateType = 1, StartDate = DateTime.MinValue, EndDate = DateTime.MinValue, NodeId = 123456, UserName = "admin1" };
            var result = new ReassignClaimContainer { ClaimData = new List<EvaluateableClaim> { new EvaluateableClaim { ClaimId = 98745621, BillType = "121", ClaimTotal = 1875, ModelId = 5879 } } };
            _mockReassignClaimRepository.Setup(f => f.GetReassignGridData(It.IsAny<ClaimSearchCriteria>())).Returns(result);
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            ReassignClaimContainer actual = _target.GetReassignGridData(claimSearchCriteria);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Inserts the track task with retained claim test.
        /// </summary>
        [TestMethod]
        public void InsertTrackTaskWithRetainedClaimTest()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            ReassignedClaimJob reassignedClaimJob = new ReassignedClaimJob
            {
                RequestName = "AdjRequest_4576",
                UserName = "jay",
                DateType = 1,
                DateFrom = DateTime.MinValue,
                DateTo = DateTime.MinValue
            };
            _mockReassignClaimRepository.Setup(f => f.AddReassignedClaimJob(It.IsAny<ReassignedClaimJob>())).Returns(true);
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            bool actual = _target.AddReassignedClaimJob(reassignedClaimJob);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Gets the contracts by node identifier test.
        /// </summary>
        [TestMethod]
        public void GetContractsByNodeIdTest()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            ContractHierarchy contractHierarchy = new ContractHierarchy
            {
                NodeId = 579892
            };
            var result = new List<Contract>
            {
                new Contract{ContractId = 194588, ContractName = "Contract 1"}, 
                new Contract{ContractId = 194589, ContractName = "Contract 2"}
            };
            _mockReassignClaimRepository.Setup(f => f.GetContractsByNodeId(It.IsAny<ContractHierarchy>())).Returns(result);
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            List<Contract> actual = _target.GetContractsByNodeId(contractHierarchy);
            Assert.AreEqual(result, actual);
        }


        /// <summary>
        /// Gets the claim linked count test.
        /// </summary>
        [TestMethod]
        public void GetClaimLinkedCountTest()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            ContractHierarchy contractHierarchy = new ContractHierarchy
            {
                NodeId = 579892
            };
            const int result = 5;
            _mockReassignClaimRepository.Setup(f => f.GetClaimLinkedCount(It.IsAny<ContractHierarchy>())).Returns(result);
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            int actual = _target.GetClaimLinkedCount(contractHierarchy);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the reassign grid data test if null.
        /// </summary>
        [TestMethod]
        public void GetReassignGridDataTestIfNull()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            ClaimSearchCriteria claimSearchCriteria = null;
            _mockReassignClaimRepository.Setup(f => f.GetReassignGridData(claimSearchCriteria));
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            ReassignClaimContainer actual = _target.GetReassignGridData(null);
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Adds the reassigned claim job test if null.
        /// </summary>
        [TestMethod]
        public void AddReassignedClaimJobTestIfNull()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            _mockReassignClaimRepository.Setup(f => f.AddReassignedClaimJob(null));
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            bool actual = _target.AddReassignedClaimJob(null);
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Gets the contracts by node identifier test if null.
        /// </summary>
        [TestMethod]
        public void GetContractsByNodelIdTestIfNull()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            _mockReassignClaimRepository.Setup(f => f.GetContractsByNodeId(null));
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            List<Contract> actual = _target.GetContractsByNodeId(null);
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Gets the claim linked count if null.
        /// </summary>
        [TestMethod]
        public void GetClaimLinkedCountIfNull()
        {
            _mockReassignClaimRepository = new Mock<IReassignClaimRepository>();
            const int result = 0;
            _mockReassignClaimRepository.Setup(f => f.GetClaimLinkedCount(null));
            _target = new ReassignClaimLogic(_mockReassignClaimRepository.Object);
            int actual = _target.GetClaimLinkedCount(null);
            Assert.AreEqual(result, actual);
        }

    }
}
