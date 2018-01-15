using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for ModelingReportLogicTest and is intended
    ///to contain all ModelingReportLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractPayerMapReportLogicUnitTest
    {
        /// <summary>
        /// Contracts the payer map report logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ContractPayerMapReportLogicConstructorUnitTest1()
        {
            ContractPayerMapReportLogic target = new ContractPayerMapReportLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractPayerMapReportLogic));
        }

        /// <summary>
        /// Contracts the payer map report logic constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ContractPayerMapReportLogicConstructorUnitTest2()
        {
            var mockContractPayerMapReportRepository = new Mock<IContractPayerMapReportRepository>();
            ContractPayerMapReportLogic target = new ContractPayerMapReportLogic(mockContractPayerMapReportRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractPayerMapReportLogic));
        }

        /// <summary>
        /// Gets if not null.
        /// </summary>
        [TestMethod]
        public void GetIfNotNull()
        {
            Mock<IContractPayerMapReportRepository> mockContractPayerMapReportRepository = new Mock<IContractPayerMapReportRepository>();
            var result = new ContractPayerMapReport();
            mockContractPayerMapReportRepository.Setup(f => f.Get(It.IsAny<ContractPayerMapReport>())).Returns(result);
            ContractPayerMapReportLogic target = new ContractPayerMapReportLogic(mockContractPayerMapReportRepository.Object);
            ContractPayerMapReport actual = target.Get(result);
            Assert.AreEqual(result, actual);
        } 

       
    }
}
