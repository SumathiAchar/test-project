using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for RtaEdiResponseLogicUnitTest
    /// </summary>
    [TestClass]
    public class RtaEdiResponseLogicUnitTest
    {
        /// <summary>
        /// Rtas the edi response logic parameterless constructor test1.
        /// </summary>
        [TestMethod]
        public void RtaEdiResponseLogicParameterlessConstructorTest1()
        {
            var target = new RtaEdiResponseLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(RtaEdiResponseLogic));
        }

        /// <summary>
        /// Rtas the edi response logic constructor test2.
        /// </summary>
        [TestMethod]
        public void RtaEdiResponseLogicConstructorTest2()
        {
            Mock<IRtaEdiResponseRepository> mockRtaEdiResponseRepository = new Mock<IRtaEdiResponseRepository>();
            RtaEdiResponseLogic target = new RtaEdiResponseLogic(mockRtaEdiResponseRepository.Object);
            Assert.IsInstanceOfType(target, typeof(RtaEdiResponseLogic));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        [TestMethod]
        public void Save()
        {
            //Arrange
            var mockRtaEdiResponseRepository = new Mock<IRtaEdiResponseRepository>();
            const long result = 12;
            mockRtaEdiResponseRepository.Setup(f => f.Save(It.IsAny<RtaEdiResponse>())).Returns(result);
            RtaEdiResponseLogic target = new RtaEdiResponseLogic(mockRtaEdiResponseRepository.Object);

            //Act
            long actual = target.Save(new RtaEdiResponse());

            //Assert
            Assert.AreEqual(result, actual);
        }

    }
}
