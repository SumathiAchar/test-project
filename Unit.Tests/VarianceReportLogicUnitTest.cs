using System;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for VarianceReportLogicTest and is intended
    ///to contain all VarianceReportLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class VarianceReportLogicTest
    {

        [TestMethod]
        public void VarianceReportLogiConstructorTest1()
        {
            var target = new VarianceReportLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(VarianceReportLogic));
        }
        /// <summary>
        ///A test for VarianceReportLogic Constructor
        ///</summary>
         [TestMethod]
        public void VarianceReportLogicConstructorTest()
        {
            Mock<IVarianceReportRepository> mockVarianceReportLogic = new Mock<IVarianceReportRepository>();
           VarianceReportLogic target = new VarianceReportLogic(mockVarianceReportLogic.Object);
           Assert.IsInstanceOfType(target, typeof(VarianceReportLogic));
        }

        /// <summary>
        ///A test for GetAllVarianceReport
        ///</summary>
         [TestMethod]
        public void GetAllVarianceReportTest()
        {
            VarianceReportLogic target = new VarianceReportLogic(Constants.ConnectionString);
            var actual = target.GetVarianceReport(null);
            Assert.AreEqual(typeof(VarianceReport), actual.GetType());

        }

         [TestMethod]
         public void GetAllVarianceReportTestWithSpecifiedDate()
         {
             VarianceReport varianceReport = new VarianceReport
             {
                 StartDate = DateTime.Now.AddYears(-1),
                 EndDate = DateTime.Now
             };

             Mock<IVarianceReportRepository> varianceReportRepository = new Mock<IVarianceReportRepository>();
             varianceReportRepository.Setup(x => x.GetAllVarianceReport(varianceReport)).Returns(varianceReport);

             VarianceReport actualReport = new VarianceReportLogic(varianceReportRepository.Object).GetVarianceReport(varianceReport);

             Assert.AreEqual(varianceReport, actualReport);
             varianceReportRepository.VerifyAll();
         }

        [TestMethod]
        public void GetAllVarianceReportTestWithRequestAdjudicationName()
        {
            VarianceReport varianceReport = new VarianceReport
            {
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                ClaimSearchCriteria = Constants.AdjudicationRequestCriteria
            };

            Mock<IVarianceReportRepository> varianceReportRepository = new Mock<IVarianceReportRepository>();
            varianceReportRepository.Setup(x => x.GetAllVarianceReport(varianceReport)).Returns(varianceReport);

            VarianceReport actualReport = new VarianceReportLogic(varianceReportRepository.Object).GetVarianceReport(varianceReport);

            Assert.AreEqual(varianceReport, actualReport);
            varianceReportRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllVarianceReportTestWithOutRequestAdjudicationName()
        {
            VarianceReport varianceReport = new VarianceReport
            {
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            };

            Mock<IVarianceReportRepository> varianceReportRepository = new Mock<IVarianceReportRepository>();
            varianceReportRepository.Setup(x => x.GetAllVarianceReport(varianceReport)).Returns(varianceReport);

            VarianceReport actualReport = new VarianceReportLogic(varianceReportRepository.Object).GetVarianceReport(varianceReport);

            Assert.AreEqual(varianceReport, actualReport);
            varianceReportRepository.VerifyAll();
        }
    }
}
