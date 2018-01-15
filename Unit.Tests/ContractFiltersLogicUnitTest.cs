/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Contract Filters Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ContractFiltersLogicUnitTest
    /// </summary>
    [TestClass]
    public class ContractFiltersLogicUnitTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        //public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Contracts the filters logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractFiltersLogicConstructorTest()
        {
            var target = new ContractFilterLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractFilterLogic));
        }
        /// <summary>
        /// Contracts the filters logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractFiltersLogicConstructorTest1()
        {
            Mock<IContractFilterRepository> mockProductRepository = new Mock<IContractFilterRepository>();
            ContractFilterLogic target = new ContractFilterLogic(mockProductRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractFilterLogic));
        }


        //todo: ok & 3 blocks need to test
        /// <summary>
        /// Gets the contract filters data test.
        /// </summary>
        [TestMethod]
        public void GetContractFiltersDataTest()
        {
            var mockGetContractFiltersDataBasedOnContractId  = new Mock<IContractFilterRepository>();
            //Mock Input
            ContractServiceType inputData = new ContractServiceType { ContractId = 234, ContractServiceTypeId = 0 };
            //Mock output 
            List<ContractFilter> result = new List<ContractFilter> { new ContractFilter {FilterName="Bill Type",FilterValues="23,45,67" } };
            mockGetContractFiltersDataBasedOnContractId.Setup(f => f.GetContractFilters(inputData)).Returns(result);
            ContractFilterLogic target = new ContractFilterLogic(mockGetContractFiltersDataBasedOnContractId.Object);
            List<ContractFilter> actual = target.GetContractFiltersData(inputData);
            Assert.AreEqual(result, actual);
            
        }
    }
}
