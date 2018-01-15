using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class AuditLogReportLogicTest
    {
        /// <summary>
        /// Audits the log report logic parameterless constructor test1.
        /// </summary>
        [TestMethod]
        public void AuditLogReportLogicParameterlessConstructorTest1()
        {
            var target = new AuditLogReportLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(AuditLogReportLogic));
        }

        /// <summary>
        /// Audits the log report logic parameterless constructor test2.
        /// </summary>
        [TestMethod]
        public void AuditLogReportLogicParameterlessConstructorTest2()
        {
            Mock<IAuditLogReportRepository> mockAuditLogReportLogic = new Mock<IAuditLogReportRepository>();
            AuditLogReportLogic target = new AuditLogReportLogic(mockAuditLogReportLogic.Object);
            Assert.IsInstanceOfType(target, typeof(AuditLogReportLogic));
        }

        /// <summary>
        /// Gets the audit log report.
        /// </summary>
        [TestMethod]
        public void GetAuditLogReport()
        {
            Mock<IAuditLogReportRepository> mockAuditLogReportLogic = new Mock<IAuditLogReportRepository>();
            AuditLogReport result = new AuditLogReport();
            mockAuditLogReportLogic.Setup(f => f.GetAuditLogReport(It.IsAny<AuditLogReport>())).Returns(result);
            AuditLogReportLogic target = new AuditLogReportLogic(mockAuditLogReportLogic.Object);
            AuditLogReport actual = target.GetAuditLogReport(new AuditLogReport());
            Assert.AreEqual(result, actual);
        }
    }
}
