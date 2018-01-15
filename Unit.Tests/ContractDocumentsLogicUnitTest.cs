/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Contract Documents Logic Testing
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
    /// Summary description for ContractDocumentsLogicUnitTest
    /// </summary>
    [TestClass]
    public class ContractDocumentsLogicUnitTest
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
        /// Contracts the documents logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractDocumentsLogicConstructorTest()
        {
            var target = new ContractDocumentLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractDocumentLogic));

            //IContractDocumentRepository productRepository = new ContractDocumentRepository(); 
            //ContractDocumentLogic target = new ContractDocumentLogic(productRepository);
            //Assert.IsInstanceOfType(target, typeof(ContractDocumentLogic));
        }

        /// <summary>
        /// Contracts the documents logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractDocumentsLogicConstructorTest1()
        {
            Mock<IContractDocumentRepository> mockProductRepository = new Mock<IContractDocumentRepository>();
            ContractDocumentLogic target = new ContractDocumentLogic(mockProductRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractDocumentLogic));
        }

        /// <summary>
        /// AddEditContractDocs Unit Test for Null.
        /// </summary>
        [TestMethod]
        public void AddEditContractDocsifNull()
        {
            Mock<IContractDocumentRepository> mockProductRepository = new Mock<IContractDocumentRepository>();
            var result = new ContractDoc{ContractId = 123, ContractDocId = 24656};
            mockProductRepository.Setup(f => f.AddEditContractDocs(It.IsAny<ContractDoc>())).Returns(result);
            ContractDocumentLogic target = new ContractDocumentLogic(mockProductRepository.Object);

            ContractDoc actual = target.AddEditContractDocs(null);
            Assert.AreEqual(result.ContractDocId, actual.ContractDocId);
        }

        /// <summary>
        /// Adds the edit contract if not null.
        /// </summary>
        [TestMethod]
        public void AddEditContractDocsifNotNull()
        {
            Mock<IContractDocumentRepository> mockAddNewPaymentTypeFeeSchedule = new Mock<IContractDocumentRepository>();
            var result = new ContractDoc { ContractId = 123, ContractDocId = 24656 };
            mockAddNewPaymentTypeFeeSchedule.Setup(f => f.AddEditContractDocs(It.IsAny<ContractDoc>())).Returns(result);
            var target = new ContractDocumentLogic(mockAddNewPaymentTypeFeeSchedule.Object);
            ContractDoc actual = target.AddEditContractDocs(result);
            Assert.AreEqual(result.ContractDocId, actual.ContractDocId);
        }

        /// <summary>
        /// Deletes the contract document by Id mock test1.
        /// </summary>
        [TestMethod]
        public void DeleteContractDocByIdMockTest1()
        {
            Mock<IContractDocumentRepository> mockProductRepository = new Mock<IContractDocumentRepository>();
            ContractDoc objcontractDocs = new ContractDoc { ContractDocId = 1 };
            mockProductRepository.Setup(f => f.DeleteContractDoc(objcontractDocs)).Returns(true);
            ContractDocumentLogic target = new ContractDocumentLogic(mockProductRepository.Object);
            bool actual = target.DeleteContractDoc(objcontractDocs);
            Assert.AreEqual(true,actual);
        }
        


        /// <summary>
        /// Gets the contract document by Id mock test1.
        /// </summary>
       [TestMethod]
       public void GetContractDocByIdMockTest2()
        {
            ContractDoc objPaymentTypeStopLoss = new ContractDoc {ContractDocId = 1, FileName = "TestFileName" };

            Mock<IContractDocumentRepository> mockProductRepository = new Mock<IContractDocumentRepository>();
            mockProductRepository.Setup(f => f.GetContractDocById(It.IsAny<long>())).Returns(objPaymentTypeStopLoss);
            ContractDocumentLogic target = new ContractDocumentLogic(mockProductRepository.Object);

            ContractDoc actual = target.GetContractDocById(1);
            //Assert.IsNotNull(actual);
            Assert.AreEqual(objPaymentTypeStopLoss.FileName, actual.FileName);
        }
       /// <summary>
       /// Gets the contract document by unique identifier difference null test.
       /// </summary>
       [TestMethod]
       public void GetContractDocByIdIfNullTest()
       {
           var repository = new Mock<IContractDocumentRepository>();
           const long value = 0;
           repository.Setup(f => f.GetContractDocById(value));
           ContractDocumentLogic target = new ContractDocumentLogic(repository.Object);

           ContractDoc actual = target.GetContractDocById(value);
           Assert.AreEqual(null, actual);
       }

    }
}
