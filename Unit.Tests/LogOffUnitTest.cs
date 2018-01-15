using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class LogOffUnitTest
    {
        /// <summary>
        /// Logs the out logic parameterless constructor.
        /// </summary>
        [TestMethod]
        public void LogOutLogicParameterlessConstructor()
        {
            var target = new LogOffLogic("connectionString");
            //Assert
            Assert.IsInstanceOfType(target, typeof(LogOffLogic));
        }

        /// <summary>
        /// Logs the out logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void LogOutLogicConstructorUnitTest1()
        {
            var mockLogOutRepository = new Mock<ILogOffRepository>();
            LogOffLogic target = new LogOffLogic(mockLogOutRepository.Object);
            Assert.IsInstanceOfType(target, typeof(LogOffLogic));
        }
        
    }
}
