using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class LogOnTest
    {
        private static LogOnLogic _target;

        /// <summary>
        /// Logs the on logic parameterless constructor.
        /// </summary>
        [TestMethod]
        public void LogOnLogicParameterlessConstructor()
        {
            _target = new LogOnLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(LogOnLogic));
        }

        /// <summary>
        /// Logs the on logic parametrized constructor test.
        /// </summary>
        [TestMethod]
        public void LogOnLogicParametrizedConstructorTest()
        {
            var mockLogOnRepository = new Mock<ILogOnRepository>();
            _target = new LogOnLogic(mockLogOnRepository.Object);
            Assert.IsInstanceOfType(_target, typeof(LogOnLogic));
        }
    }
}
