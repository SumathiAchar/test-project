using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for ServiceLineClaimFieldLogicTest and is intended
    ///to contain all ServiceLineClaimFieldLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ServiceLineClaimFieldLogicTest
    {
        /// <summary>
        ///A test for AddNewClaimFieldSelection
        ///</summary>
        [TestMethod]
        public void AddNewClaimFieldSelectionTestNull()
        {
            var mockAddClaimFieldSelection = new Mock<IServiceLineClaimFieldRepository>();
            mockAddClaimFieldSelection.Setup(f => f.AddNewClaimFieldSelection(It.IsAny<List<ContractServiceLineClaimFieldSelection>>())).Returns(0);
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockAddClaimFieldSelection.Object);
            long actual = target.AddNewClaimFieldSelection(null);
            Assert.AreEqual(0, actual);
        }


        ///// <summary>
        /////A test for EditClaimFieldSelection
        /////</summary>
        //[TestMethod]
        //public void EditClaimFieldSelectionTestNotNull()
        //{
        //    var mockEditClaimFieldSelection = new Mock<IServiceLineClaimFieldRepository>();
        //    mockEditClaimFieldSelection.Setup(f => f.EditClaimFieldSelection(It.IsAny<List<ContractServiceLineClaimFieldSelection>>())).Returns(2);
        //    ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockEditClaimFieldSelection.Object);
        //    List<ContractServiceLineClaimFieldSelection> claimFieldSelection =
        //       new List<ContractServiceLineClaimFieldSelection> { new ContractServiceLineClaimFieldSelection { ClaimFieldId = 2, ContractServiceTypeId = null, ContractId = 1321, Operator = 4, Values = "112", ServiceLineTypeId = 7 }, new ContractServiceLineClaimFieldSelection { ClaimFieldId = 2, ContractServiceTypeId = null, ContractId = 1321, Operator = 3, Values = "221", ServiceLineTypeId = 7 } };
        //  long actual=target.EditClaimFieldSelection(claimFieldSelection);
        //   Assert.AreEqual(2, actual);
        //}

        ///// <summary>
        /////A test for EditClaimFieldSelection
        /////</summary>
        //[TestMethod]
        //public void EditClaimFieldSelectionTestNull()
        //{
        //    var mockEditClaimFieldSelection = new Mock<IServiceLineClaimFieldRepository>();
        //    mockEditClaimFieldSelection.Setup(f => f.EditClaimFieldSelection(It.IsAny<List<ContractServiceLineClaimFieldSelection>>())).Returns(0);
        //    ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockEditClaimFieldSelection.Object);
        //    long actual = target.EditClaimFieldSelection(null);
        //    Assert.AreEqual(0, actual);
        //}

        ///// <summary>
        /////A test for GetAllClaimFieldsOperators
        /////</summary>
        //[TestMethod]
        //public void GetAllClaimFieldsOperatorsTest()
        //{

        //    List<ClaimFieldOperator> result = new List<ClaimFieldOperator> { new ClaimFieldOperator { OperatorID = 1, OperatorType = "<" }, new ClaimFieldOperator { OperatorID = 2, OperatorType = ">" }, new ClaimFieldOperator { OperatorID = 3, OperatorType = "=" } };
        //    var mockGetClaimFieldOperators = new Mock<IServiceLineClaimFieldRepository>();
        //    mockGetClaimFieldOperators.Setup(f => f.GetAllClaimFieldsOperators()).Returns(result);
        //    ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockGetClaimFieldOperators.Object);
        //    List<ClaimFieldOperator> actual = target.GetAllClaimFieldsOperators();
        //    Assert.AreEqual(result, actual);
        //}

        ///// <summary>
        /////A test for GetClaimFieldSelection
        /////</summary>
        //[TestMethod]
        //public void GetClaimFieldSelectionTestNotNull()
        //{
        //    //Mock Input
        //    ContractServiceLineClaimFieldSelection inputData = new ContractServiceLineClaimFieldSelection { ContractId = 235, ContractServiceTypeId = null, ServiceLineTypeId=7 };

        //    //Mock output
        //    List<ContractServiceLineClaimFieldSelection> result = new List<ContractServiceLineClaimFieldSelection> { new ContractServiceLineClaimFieldSelection { Operator = 2, Values = "234", ClaimFieldId = 2 }, new ContractServiceLineClaimFieldSelection { Operator = 1, Values = "9808", ClaimFieldId = 1 } };
        //    var mockGetClaimFieldSelection = new Mock<IServiceLineClaimFieldRepository>();
        //    mockGetClaimFieldSelection.Setup(f => f.GetClaimFieldSelection(inputData)).Returns(result);
        //    ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockGetClaimFieldSelection.Object);
        //    List<ContractServiceLineClaimFieldSelection> actual = target.GetClaimFieldSelection(inputData);
        //    Assert.AreEqual(result,actual);

        //}

        /// <summary>
        /// Gets the claim field selection test null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldSelectionTestNull()
        {
            List<ContractServiceLineClaimFieldSelection> result = new List<ContractServiceLineClaimFieldSelection>();
            var mockGetClaimFieldSelection = new Mock<IServiceLineClaimFieldRepository>();
            mockGetClaimFieldSelection.Setup(f => f.GetClaimFieldSelection(null)).Returns(result);
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockGetClaimFieldSelection.Object);
            List<ContractServiceLineClaimFieldSelection> actual = target.GetClaimFieldSelection(null);
            Assert.AreEqual(result, actual);

        }


    }
}
