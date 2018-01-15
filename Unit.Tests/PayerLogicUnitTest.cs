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
    public class PayerLogicUnitTest
    {
        /// <summary>
        /// Payers the logic parameterless constructor.
        /// </summary>
        [TestMethod]
        public void PayerLogicParameterlessConstructor()
        {
            var target = new PayerLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(PayerLogic));
        }

        /// <summary>
        /// Payers the logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void PayerLogicConstructorUnitTest1()
        {
            var mockPayerRepository = new Mock<IPayerRepository>();
            PayerLogic target = new PayerLogic(mockPayerRepository.Object);
            Assert.IsInstanceOfType(target, typeof(PayerLogic));
        }

        /// <summary>
        /// Gets the payers test by facility identifier.
        /// </summary>
        [TestMethod]
        public void GetPayersTestByFacilityId()
        {
            var mockPayerRepository = new Mock<IPayerRepository>();
            var value = new ContractServiceLineClaimFieldSelection {FacilityId = 3};
            var result = new List<Payer> { new Payer {PayerName = "AETNA"} };
            mockPayerRepository.Setup(f => f.GetPayers(It.IsAny<ContractServiceLineClaimFieldSelection>())).Returns(result);
            PayerLogic target = new PayerLogic(mockPayerRepository.Object);
            List<Payer> actual = target.GetPayers(value);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the payers test by contract identifier.
        /// </summary>
        [TestMethod]
        public void GetPayersTestByContractId()
        {
            var mockPayerRepository = new Mock<IPayerRepository>();
            var value = new ContractServiceLineClaimFieldSelection { FacilityId = 0, ContractId = 101};
            var result = new List<Payer> { new Payer { PayerName = "AETNA" } };
            mockPayerRepository.Setup(f => f.GetPayers(It.IsAny<ContractServiceLineClaimFieldSelection>())).Returns(result);
            PayerLogic target = new PayerLogic(mockPayerRepository.Object);
            List<Payer> actual = target.GetPayers(value);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the payers test by service type identifier.
        /// </summary>
        [TestMethod]
        public void GetPayersTestByServiceTypeId()
        {
            var mockPayerRepository = new Mock<IPayerRepository>();
            var value = new ContractServiceLineClaimFieldSelection {FacilityId = 0, ContractId = 0, ContractServiceTypeId = 275};
            var result = new List<Payer> { new Payer { PayerName = "AETNA" } };
            mockPayerRepository.Setup(f => f.GetPayers(It.IsAny<ContractServiceLineClaimFieldSelection>())).Returns(result);
            PayerLogic target = new PayerLogic(mockPayerRepository.Object);
            List<Payer> actual = target.GetPayers(value);
            Assert.AreEqual(result, actual);
        }
    }
}
