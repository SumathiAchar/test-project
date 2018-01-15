using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for ContractServiceLinePaymentTypeLogicTest and is intended
    ///to contain all ContractServiceLinePaymentTypeLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractServiceLinePaymentTypeLogicTest
    {

        /// <summary>
        ///A test for DeleteContractServiceLinesAndPaymentTypes
        ///</summary>
        [TestMethod]
        public void DeleteContractServiceLinesAndPaymentTypesTest()
        {
            var repository = new Mock<IContractServiceLinePaymentTypeRepository>();
            repository.Setup(f => f.DeleteContractServiceLinesAndPaymentTypes(null)).Returns(false);
            ContractServiceLinePaymentTypeLogic target = new ContractServiceLinePaymentTypeLogic(repository.Object);

            bool actual = target.DeleteContractServiceLinesAndPaymentTypes(null);
            Assert.AreEqual(false, actual);
           
            //ContractServiceLinePaymentTypeLogic target = new ContractServiceLinePaymentTypeLogic();
            //bool actual = target.DeleteContractServiceLinesAndPaymentTypes(null);
            //Assert.AreEqual(false, actual);
           
        }
        /// <summary>
        /// 
        /// Deletes the contract service and  payment types by ID unit test.
        /// </summary>
        [TestMethod]
        public void DeleteContractServiceLInesandPaymentTypesByIdUnitTest()
        {
            var mockProductRepository = new Mock<IContractServiceLinePaymentTypeRepository>();
            ContractServiceLinePaymentType objcontractDocs = new ContractServiceLinePaymentType { ContractId = 1 };
            mockProductRepository.Setup(f => f.DeleteContractServiceLinesAndPaymentTypes(objcontractDocs)).Returns(true);
            ContractServiceLinePaymentTypeLogic target = new ContractServiceLinePaymentTypeLogic(mockProductRepository.Object);
            bool actual = target.DeleteContractServiceLinesAndPaymentTypes(objcontractDocs);
            Assert.AreEqual(true, actual);
        }


        /// <summary>
        ///A test for ContractServiceLinePaymentTypeLogic Constructor
        ///</summary>
        [TestMethod]
        public void ContractServiceLinePaymentTypeLogicConstructorTest()
        {

            var mockcontractServiceLinePaymentTypesRepository = new Mock<IContractServiceLinePaymentTypeRepository>();
            var target = new ContractServiceLinePaymentTypeLogic(mockcontractServiceLinePaymentTypesRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractServiceLinePaymentTypeLogic));
        }

        /// <summary>
        ///A test for ContractServiceLinePaymentTypeLogic Constructor
        ///</summary>
        [TestMethod]
        public void ContractServiceLinePaymentTypeLogicConstructorTest1()
        {
            ContractServiceLinePaymentTypeLogic target = new ContractServiceLinePaymentTypeLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractServiceLinePaymentTypeLogic));
        }
    }
}
