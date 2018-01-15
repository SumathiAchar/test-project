using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for ClaimFieldValueLogicTest and is intended
    ///to contain all ClaimFieldValueLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ClaimFieldValueLogicTest
    {



        /// <summary>
        /// Claims the field value logic constructor test.
        /// </summary>
        [TestMethod]
        public void ClaimFieldValueLogicConstructorTest()
        {
            var target = new ClaimFieldValueLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ClaimFieldValueLogic));
        }

        /// <summary>
        ///A test for ClaimFieldValueLogic Constructor
        ///</summary>
        [TestMethod]
        public void ClaimFieldValueLogicConstructorTest2()
        {
            Mock<IClaimFieldValueRepository> mockImportPaymentRepository = new Mock<IClaimFieldValueRepository>();
            ClaimFieldValueLogic target = new ClaimFieldValueLogic(mockImportPaymentRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimFieldValueLogic));
        }

        /// <summary>
        ///A test for AddClaimFieldValues
        ///</summary>
        [TestMethod]
        public void AddClaimFieldValuesTest()
        {
            //Arrange
            var mockImportPaymentRepository = new Mock<IClaimFieldValueRepository>();
            const long result = 1;
            mockImportPaymentRepository.Setup(f => f.AddClaimFieldValues(It.IsAny<ClaimFieldValue>())).Returns(result);
            ClaimFieldValueLogic target = new ClaimFieldValueLogic(mockImportPaymentRepository.Object);

            //Act
            long actual = target.AddClaimFieldValues(null);

            //Assert
            Assert.AreEqual(result, actual);
        }
    }
}
