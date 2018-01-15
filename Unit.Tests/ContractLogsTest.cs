using System.Collections.Generic;
using System.Collections.ObjectModel;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{


    /// <summary>
    ///This is a test class for ContractLogsLogicTest and is intended
    ///to contain all ContractLogsLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractLogsLogicTest
    {
        //Creating object for Logic
        private ContractLogLogic _target;

        //Creating mock object 
        private Mock<IContractLogRepository> _mockContractLogRepository;

        /// <summary>
        ///A test for ContractLogsLogic Constructor
        ///</summary>
        [TestMethod]
        public void ContractLogsLogicConstructorTest()
        {
            ContractLogLogic target = new ContractLogLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractLogLogic));
        }

        /// <summary>
        /// Contracts the logs logic parameterized constructor.
        /// </summary>
        [TestMethod]
        public void ContractLogsLogicParameterizedConstructor()
        {
            _mockContractLogRepository = new Mock<IContractLogRepository>();
            _target = new ContractLogLogic(_mockContractLogRepository.Object);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(ContractLogLogic));
        }

        /// <summary>
        /// Inserts the contract logs if payment results is null test.
        /// </summary>
        [TestMethod]
        public void InsertContractLogsIfPaymentResultsIsNullTest()
        {
            ContractLogLogic target = new ContractLogLogic(Constants.ConnectionString);
            bool actual = target.AddContractLog(null, null);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Inserts the contract logs if payment results is not null test.
        /// </summary>
        [TestMethod]
        public void InsertContractLogsIfPaymentResultsIsNotNullTest()
        {
            ContractLogLogic target = new ContractLogLogic(Constants.ConnectionString);
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };
            var paymentResultDictionary = new Dictionary<long, List<PaymentResult>>
            {
                {1, paymentResults}
            };
            List<Contract> contracts = new List<Contract>
            {
                new Contract{ ContractId = 123, ContractName = "Contract1"},
                new Contract{ContractId = 456, ContractName = "Contract2"}
            };
            bool actual = target.AddContractLog(paymentResultDictionary, contracts);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Inserts the contract logs if service type identifier is null test.
        /// </summary>
        [TestMethod]
        public void InsertContractLogsIfServiceTypeIdIsNullTest()
        {
            ContractLogLogic target = new ContractLogLogic(Constants.ConnectionString);
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = null, ServiceTypeId = 10, AdjudicatedValue = 1458.78, PaymentTypeId = 1252},
                new PaymentResult {ClaimId = 123, Line = null, ServiceTypeId = null, ContractId = 7845}
            };
            var paymentResultDictionary = new Dictionary<long, List<PaymentResult>>
            {
                {1, paymentResults}
            };
            List<Contract> contracts = new List<Contract>
            {
                new Contract{ ContractId = 123, ContractName = "Contract1"},
                new Contract{ContractId = 7845, ContractName = "Contract2", ContractServiceTypes = new Collection<ContractServiceType>{new ContractServiceType{ContractServiceTypeId = 10}} }
            };
            bool actual = target.AddContractLog(paymentResultDictionary, contracts);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Inserts the contract logs for get line level contract log list test.
        /// </summary>
        [TestMethod]
        public void InsertContractLogsForGetLineLevelContractLogListTest()
        {
            ContractLogLogic target = new ContractLogLogic(Constants.ConnectionString);
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 10, AdjudicatedValue = 1458.78, PaymentTypeId = 1252},
                new PaymentResult {ClaimId = 123, Line = null, ServiceTypeId = null, ContractId = 7845}
            };
            var paymentResultDictionary = new Dictionary<long, List<PaymentResult>>
            {
                {1, paymentResults}
            };
            List<Contract> contracts = new List<Contract>
            {
                new Contract{ ContractId = 123, ContractName = "Contract1"},
                new Contract{ContractId = 7845, ContractName = "Contract2", ContractServiceTypes = new Collection<ContractServiceType>{new ContractServiceType{ContractServiceTypeId = 10}} }
            };
            bool actual = target.AddContractLog(paymentResultDictionary, contracts);
            Assert.AreEqual(true, actual);
        }

        }
}
