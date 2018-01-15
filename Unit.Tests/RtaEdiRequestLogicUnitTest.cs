using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for RtaEdiRequestLogicUnitTest
    /// </summary>
    [TestClass]
    public class RtaEdiRequestLogicUnitTest
    {
        /// <summary>
        /// Rtas the edi request logic parameterless constructor test1.
        /// </summary>
        [TestMethod]
        public void RtaEdiRequestLogicParameterlessConstructorTest1()
        {
            var target = new RtaEdiRequestLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(RtaEdiRequestLogic));
        }

        /// <summary>
        /// Rtas the edi request logic constructor test2.
        /// </summary>
        [TestMethod]
        public void RtaEdiRequestLogicConstructorTest2()
        {
            Mock<IRtaEdiRequestRepository> mockRtaEdiRequestRepository = new Mock<IRtaEdiRequestRepository>();
            RtaEdiRequestLogic target = new RtaEdiRequestLogic(mockRtaEdiRequestRepository.Object);
            Assert.IsInstanceOfType(target, typeof(RtaEdiRequestLogic));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        [TestMethod]
        public void Save()
        {
            //Arrange
            var mockRtaEdiRequestRepository = new Mock<IRtaEdiRequestRepository>();
            const long result = 12;
            mockRtaEdiRequestRepository.Setup(f => f.Save(It.IsAny<RtaEdiRequest>())).Returns(result);
            RtaEdiRequestLogic target = new RtaEdiRequestLogic(mockRtaEdiRequestRepository.Object);

            //Act
            long actual = target.Save(new RtaEdiRequest());

            //Assert
            Assert.AreEqual(result, actual);
        }

    }
}
