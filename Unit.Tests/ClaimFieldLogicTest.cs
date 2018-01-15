using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class ClaimFieldLogicTest
    {
        /// <summary>
        /// Claims the field logic constructor test.
        /// </summary>
        [TestMethod]
        public void ClaimFieldLogicConstructorTest()
        {
            var target = new ClaimFieldLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ClaimFieldLogic));
        }

        [TestMethod]
        public void ClaimFieldLogicConstructorTest2()
        {
            Mock<IClaimFieldRepository> mockClaimFieldRepository = new Mock<IClaimFieldRepository>();
            ClaimFieldLogic target = new ClaimFieldLogic(mockClaimFieldRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimFieldLogic));
        }

        [TestMethod]
        public void GetClaimFieldsByModuleTest()
        {
            var mockClaimFieldLogic = new Mock<IClaimFieldRepository>();
            const int value = 2;
            mockClaimFieldLogic.Setup(f => f.GetClaimFieldsByModule(value)).Returns(new List<ClaimField>());
            ClaimFieldLogic target = new ClaimFieldLogic(mockClaimFieldLogic.Object);
            List<ClaimField> actual = target.GetClaimFieldsByModule(value);
            const int result = 0;
            Assert.AreEqual(result, actual.Count);
        }


    }
}
