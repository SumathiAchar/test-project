using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for ClaimAdjudicationReportLogicTest and is intended
    ///to contain all ClaimAdjudicationReportLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ClaimAdjudicationReportLogicTest
    {
        /// <summary>
        ///A test for ClaimAdjudicationReportLogic Constructor
        ///</summary>
        [TestMethod]
        public void ClaimAdjudicationReportLogicConstructorTest()
        {
            Mock<IAdjudicationReportRepository> repository = new Mock<IAdjudicationReportRepository>();
            ClaimAdjudicationReportLogic target = new ClaimAdjudicationReportLogic(repository.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimAdjudicationReportLogic));
        }

        /// <summary>
        /// Claims the adjudication report logic parameter less constructor test.
        /// </summary>
        [TestMethod]
        public void ClaimAdjudicationReportLogicParameterLessConstructorTest()
        {
            ClaimAdjudicationReportLogic target = new ClaimAdjudicationReportLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ClaimAdjudicationReportLogic));
        }

        /// <summary>
        ///A test for GetClaimAdjudicationReport
        ///</summary>
        [TestMethod]
        public void GetClaimAdjudicationReportNullLogicUnitTest()
        {
            //Arrange
            var mockClaimAdjudicationReport = new Mock<IAdjudicationReportRepository>();
            ClaimAdjudicationReport reportInfo = new ClaimAdjudicationReport();
            mockClaimAdjudicationReport.Setup(f => f.GetClaimAdjudicationReport(It.IsAny<ClaimAdjudicationReport>()))
                                       .Returns(reportInfo);
            ClaimAdjudicationReportLogic target = new ClaimAdjudicationReportLogic(mockClaimAdjudicationReport.Object);

            //Act
            ClaimAdjudicationReport actual = target.GetClaimAdjudicationReport(null);

            //Assert
            Assert.AreEqual(actual.GetType(), reportInfo.GetType());
        }
        //TODO:ok
        /// <summary>
        /// Gets all claim adjudication test difference not null unit test.
        /// </summary>
        [TestMethod]
        public void GetClaimAdjudicationReportTestIfNotNullUnitTest()
        {

            var mockGetPaymentTypeMedicareOpDetails = new Mock<IAdjudicationReportRepository>();
            //Mock Input
            var objMedicareOpDetails = new ClaimAdjudicationReport { ContractId = 345 };
            //Mock output 
            ClaimAdjudicationReport result = new ClaimAdjudicationReport { NodeId = 345 };
            mockGetPaymentTypeMedicareOpDetails.Setup(f => f.GetClaimAdjudicationReport(objMedicareOpDetails)).Returns(result);
            ClaimAdjudicationReportLogic target = new ClaimAdjudicationReportLogic(mockGetPaymentTypeMedicareOpDetails.Object);
            ClaimAdjudicationReport actual = target.GetClaimAdjudicationReport(objMedicareOpDetails);
            Assert.AreEqual(result, actual);

        }

        /// <summary>
        /// Gets the claim adjudication report test if null unit test.
        /// </summary>
        [TestMethod]
        public void GetClaimAdjudicationReportTestIfNullUnitTest()
        {
            var mockGetPaymentTypeMedicareOpDetails = new Mock<IAdjudicationReportRepository>();
            //Mock Input
            ClaimAdjudicationReport objMedicareOpDetails = null;
            //Mock output 
            ClaimAdjudicationReport result = new ClaimAdjudicationReport();
            mockGetPaymentTypeMedicareOpDetails.Setup(f => f.GetClaimAdjudicationReport(objMedicareOpDetails)).Returns(result);
            ClaimAdjudicationReportLogic target =
                new ClaimAdjudicationReportLogic(mockGetPaymentTypeMedicareOpDetails.Object);
            ClaimAdjudicationReport actual = target.GetClaimAdjudicationReport(null);
            Assert.AreEqual(result.GetType(), actual.GetType());
        }


        /// <summary>
        /// Gets the claim adjudication report test on request name selection.
        /// </summary>
        [TestMethod]
        public void GetClaimAdjudicationReportTestOnRequestNameSelection()
        {
            var mockGetPaymentTypeMedicareOpDetails = new Mock<IAdjudicationReportRepository>();
            var objMedicareOpDetails = new ClaimAdjudicationReport
            {
                ClaimSearchCriteria = Constants.AdjudicationRequestCriteria
            };
            ClaimAdjudicationReport result = new ClaimAdjudicationReport();
            mockGetPaymentTypeMedicareOpDetails.Setup(f => f.GetClaimAdjudicationReport(objMedicareOpDetails)).Returns(result);
            ClaimAdjudicationReportLogic target =
                new ClaimAdjudicationReportLogic(mockGetPaymentTypeMedicareOpDetails.Object);
            ClaimAdjudicationReport actual = target.GetClaimAdjudicationReport(objMedicareOpDetails);
            Assert.AreEqual(result.GetType(), actual.GetType());
        }


        /// <summary>
        /// Gets the selected claim test.
        /// </summary>
        [TestMethod]
        public void GetSelectedClaimTest()
        {
            Mock<IAdjudicationReportRepository> mockAdjudicationReportRepository = new Mock<IAdjudicationReportRepository>();

            ClaimAdjudicationReport claimAdjudicationReport = new ClaimAdjudicationReport
            {
                ClaimSearchCriteria = Constants.AdjudicationRequestCriteria
            };
            ClaimAdjudicationReport result = new ClaimAdjudicationReport();
            ClaimAdjudicationReportLogic target =
                new ClaimAdjudicationReportLogic(mockAdjudicationReportRepository.Object);
            mockAdjudicationReportRepository.Setup(f => f.GetSelectedClaim(claimAdjudicationReport, GlobalConfigVariable.MaxRecordLimitForExcelReport)).Returns(result);
            ClaimAdjudicationReport actual = target.GetSelectedClaim(claimAdjudicationReport);

            Assert.AreEqual(result.GetType(), actual.GetType());
        }

        /// <summary>
        /// Get OpenClaim Column Names Based On UserId Test
        /// </summary>
        [TestMethod]
        public void GetOpenClaimColumnNamesBasedOnUserIdTest()
        {
            Mock<IAdjudicationReportRepository> mockAdjudicationReportRepository = new Mock<IAdjudicationReportRepository>();

            ClaimAdjudicationReport claimAdjudicationReport = new ClaimAdjudicationReport
            {
                UserId = 4178
            };
            ClaimAdjudicationReport expectedResult = new ClaimAdjudicationReport { ColumnNames = new List<string> { "ClaimTotal", "AdjudicatedValue", "ActualPayment" } };
            ClaimAdjudicationReportLogic target =
                new ClaimAdjudicationReportLogic(mockAdjudicationReportRepository.Object);
            mockAdjudicationReportRepository.Setup(f => f.GetOpenClaimColumnNamesBasedOnUserId(claimAdjudicationReport)).Returns(expectedResult);
            ClaimAdjudicationReport actual = target.GetOpenClaimColumnNamesBasedOnUserId(claimAdjudicationReport);

            Assert.AreEqual(expectedResult.GetType(), actual.GetType());
            Assert.AreEqual(expectedResult.ColumnNames, actual.ColumnNames);
            Assert.AreEqual(expectedResult.ColumnNames.Count, actual.ColumnNames.Count);
        }

        /// <summary>
        /// Get OpenClaim Column Names Based On UserId null unit test.
        /// </summary>
        [TestMethod]
        public void GetOpenClaimColumnNamesBasedOnUserIdIfNullTest()
        {
            Mock<IAdjudicationReportRepository> mockAdjudicationReportRepository = new Mock<IAdjudicationReportRepository>();

            ClaimAdjudicationReport claimAdjudicationReport = null;
            ClaimAdjudicationReport expectedResult = new ClaimAdjudicationReport();
            ClaimAdjudicationReportLogic target =
                new ClaimAdjudicationReportLogic(mockAdjudicationReportRepository.Object);
            mockAdjudicationReportRepository.Setup(f => f.GetOpenClaimColumnNamesBasedOnUserId(claimAdjudicationReport)).Returns(expectedResult);
            ClaimAdjudicationReport actual = target.GetOpenClaimColumnNamesBasedOnUserId(null);

            Assert.AreEqual(expectedResult.GetType(), actual.GetType());
        }
    }
}
