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
    public class ModelingReportLogicTest
    {
        /// <summary>
        ///A test for ModelingReportLogic Constructor
        ///</summary>
        [TestMethod]
        public void ModelingReportLogicConstructorTest()
        {
            var target = new ModelingReportLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ModelingReportLogic));
        }

        /// <summary>
        ///A test for ModelingReportLogic Constructor
        ///</summary>
        [TestMethod]
        public void ModelingReportLogicConstructorTest1()
        {
            Mock<IModelingReportRepository> mockModelingReport = new Mock<IModelingReportRepository>();
            ModelingReportLogic target = new ModelingReportLogic(mockModelingReport.Object);
            Assert.IsInstanceOfType(target, typeof(ModelingReportLogic));
        }

        /// <summary>
        /// Gets all modeling details ifnot null unit test.
        /// </summary>
        [TestMethod]
        public void GetAllModelingDetailsIfnotNullUnitTest()
        {
            

            ModelingReport result = new ModelingReport
            {
                ContractId = 1,
                ContractName = "Contract 1",
                PayerName = "payer 1",
                ServiceType = "service type 1",
                ClaimTools = "claimtool",
                PaymentTool = "paymenttool"
            };
            
            Mock<IModelingReportRepository> mockGetAllVarianceReport = new Mock<IModelingReportRepository>();
            mockGetAllVarianceReport.Setup(f => f.GetAllModelingDetails(It.IsAny<ModelingReport>())).Returns(result);

            new ModelingReportLogic(mockGetAllVarianceReport.Object).GetAllModelingDetails(new ModelingReport());
            mockGetAllVarianceReport.VerifyAll();
        }   

       
    }
}
