/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Contract Information Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ContractInfoLogicUnitTest
    /// </summary>
    [TestClass]
    public class ContractPayerInfoLogicUnitTest
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
        /// Contracts the information logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractInfoLogicConstructorTest()
        {
            var target = new ContractPayerInfoLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractPayerInfoLogic));
        }

        /// <summary>
        /// Contracts the information logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractInfoLogicConstructorTest1()
        {
            Mock<IContractPayerInfoRepository> mockAddEditContractPayerInfo = new Mock<IContractPayerInfoRepository>();
            ContractPayerInfoLogic target = new ContractPayerInfoLogic(mockAddEditContractPayerInfo.Object);
            Assert.IsInstanceOfType(target, typeof(ContractPayerInfoLogic));
        }

        /// <summary>
        /// Adds contract payer information test for null
        /// </summary>
        [TestMethod]
        public void AddEditContractPayerInfoIfNull()
        {
            Mock<IContractPayerInfoRepository> mockAddEditContractPayerInfo = new Mock<IContractPayerInfoRepository>();
            mockAddEditContractPayerInfo.Setup(f => f.AddEditContractPayerInfo(It.IsAny<ContractPayerInfo>())).Returns(1);
            ContractPayerInfoLogic target = new ContractPayerInfoLogic(mockAddEditContractPayerInfo.Object);
            ContractPayerInfo objAddEditContractPayerInfo = new ContractPayerInfo { ContractPayerInfoId = 1 };

            long actual = target.AddContractPayerInfo(objAddEditContractPayerInfo);
            Assert.IsNotNull(actual);
        }
        /// <summary>
        /// Adds contract payer information test for not null
        /// </summary>
        [TestMethod]
        public void AddEditContractPayerInfoifNotNull()
        {
            var mockAddEditContractPayerInfo = new Mock<IContractPayerInfoRepository>();
            mockAddEditContractPayerInfo.Setup(f => f.AddEditContractPayerInfo(It.IsAny<ContractPayerInfo>())).Returns(1);
            ContractPayerInfoLogic target = new ContractPayerInfoLogic(mockAddEditContractPayerInfo.Object);
            ContractPayerInfo objAddEditContractPayerInfo = new ContractPayerInfo { ContractPayerInfoId = 1 };

            long actual = target.AddContractPayerInfo(objAddEditContractPayerInfo);
            Assert.AreEqual(1, actual);

        }

        /// <summary>
        /// Gets the contract payer information mock test1.
        /// </summary>
        [TestMethod]
        public void GetContractPayerInfoMockTest1()
        {
            ContractPayerInfo objAddEditContractPayerInfo = new ContractPayerInfo { ContractPayerInfoId = 1 };

            var mockAddEditContractPayerInfo = new Mock<IContractPayerInfoRepository>();
            mockAddEditContractPayerInfo.Setup(f => f.GetContractPayerInfo(It.IsAny<long>())).Returns(objAddEditContractPayerInfo);
            ContractPayerInfoLogic target = new ContractPayerInfoLogic(mockAddEditContractPayerInfo.Object);

            ContractPayerInfo actual = target.GetContractPayerInfo(1);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// Deletes the contract payer information by Id test1.
        /// </summary>
        [TestMethod]
        public void DeleteContractPayerInfoByIdMockTest1()
        {
            var mockAddEditContractPayerInfo = new Mock<IContractPayerInfoRepository>();
            ContractPayerInfo objAddEditContractPayerInfo = new ContractPayerInfo { ContractPayerInfoId = 1,UserName = "Admin1"};
            mockAddEditContractPayerInfo.Setup(f => f.DeleteContractPayerInfo(objAddEditContractPayerInfo)).Returns(true);
            ContractPayerInfoLogic target = new ContractPayerInfoLogic(mockAddEditContractPayerInfo.Object);

            bool actual = target.DeleteContractPayerInfo(objAddEditContractPayerInfo);
            Assert.AreEqual(true,actual);

        }
     }
 }
