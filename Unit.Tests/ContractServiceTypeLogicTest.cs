using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class ContractServiceTypeLogicTest
    {

        private Mock<IContractServiceTypeRepository> _mockContractServiceTypeRepository;

        /// <summary>
        /// Adds contract service type difference null.
        /// </summary>
        [TestMethod]
        public void AddEditContractServiceTypeIfNull()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(f => f.AddEditContractServiceType(It.IsAny<ContractServiceType>())).Returns(0);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            long actual = target.AddEditContractServiceType(null);
            Assert.AreEqual(0, actual);
        }


        /// <summary>
        /// Adds  contract service typeif not null.
        /// </summary>
        [TestMethod]
        public void AddEditContractServiceTypeifNotNull()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(f => f.AddEditContractServiceType(It.IsAny<ContractServiceType>())).Returns(2);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            ContractServiceType objAddEditContractServiceType = new ContractServiceType { ContractServiceTypeId = 1 };
            long actual = target.AddEditContractServiceType(objAddEditContractServiceType);
            Assert.AreEqual(2, actual);
        }


        /// <summary>
        /// Get contract service type details.
        /// </summary>
        [TestMethod]
        public void GetAllContractServiceTypeUnitTestIfNull()
        {
            const long contractId = 0;
            List<ContractServiceType> result = new List<ContractServiceType>();
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(f => f.GetAllContractServiceType(contractId)).Returns(result);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            List<ContractServiceType> actual = target.GetAllContractServiceType(contractId);
            Assert.AreEqual(0, actual.Count);
        }

        /// <summary>
        /// Get contract service type details.
        /// </summary>
        [TestMethod]
        public void GetAllContractServiceTypeUnitTestIfNotNull()
        {
            //Mock Input
            const long contractId = 354;

            //Mock output
            List<ContractServiceType> result = new List<ContractServiceType> { new ContractServiceType { ContractServiceTypeName = "IP-Surgery", ContractServiceTypeId = 976 }, new ContractServiceType { ContractServiceTypeName = "OP-Surgery", ContractServiceTypeId = 542 } };
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(f => f.GetAllContractServiceType(contractId)).Returns(result);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            List<ContractServiceType> actual = target.GetAllContractServiceType(contractId);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        ///A test for ServiceTypeDetailsLogic Constructor
        ///</summary>
        [TestMethod]
        public void ServiceTypeDetailsLogicConstructorTest()
        {
            IContractServiceTypeRepository serviceTypeDetailsRepository = new ContractServiceTypeRepository(Constants.ConnectionString);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(serviceTypeDetailsRepository);
            Assert.IsInstanceOfType(target, typeof(ContractServiceTypeLogic));

        }

        /// <summary>
        ///A test for ServiceTypeDetailsLogic Constructor
        ///</summary>
        [TestMethod]
        public void ServiceTypeDetailsLogicConstructorTest1()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractServiceTypeLogic));
        }

        /// <summary>
        ///A test for AddEditContractServiceType
        ///</summary>
        [TestMethod]
        public void AddEditContractServiceTypeTest()
        {
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(Constants.ConnectionString);
            const long expected = 0;
            long actual = target.AddEditContractServiceType(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetAllContractServiceType
        ///</summary>
        [TestMethod]
        public void GetAllContractServiceTypeTest()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.GetAllContractServiceType(It.IsAny<long>()))
                .Returns(new List<ContractServiceType>());

            new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object).GetAllContractServiceType(1);
            _mockContractServiceTypeRepository.VerifyAll();
        }

        /// <summary>
        /// Copies the type of the contract service.
        /// </summary>
        [TestMethod]
        public void CopyContractServiceType()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.CopyContractServiceType(It.IsAny<ContractServiceType>()))
                .Returns(1);
            new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object).CopyContractServiceType(
                new ContractServiceType());
            _mockContractServiceTypeRepository.VerifyAll();
        }

        /// <summary>
        /// Renames the type of the contract service.
        /// </summary>
        [TestMethod]
        public void RenameContractServiceType()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.RenameContractServiceType(It.IsAny<ContractServiceType>()))
                .Returns(1);
            new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object).RenameContractServiceType(
                new ContractServiceType());
            _mockContractServiceTypeRepository.VerifyAll();
        }

        /// <summary>
        /// Gets the contract service type details.
        /// </summary>
        [TestMethod]
        public void GetContractServiceTypeDetails()
        {
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.GetContractServiceTypeDetails(It.IsAny<ContractServiceType>()))
                .Returns(new ContractServiceType());
            new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object).GetContractServiceTypeDetails(
                new ContractServiceType());
            _mockContractServiceTypeRepository.VerifyAll();
        }

        /// <summary>
        /// Ifs the contract service type name is unique.
        /// </summary>
        [TestMethod]
        public void IfContractServiceTypeNameIsUnique()
        {
            //Arrange
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.IsContractServiceTypeNameExit(It.IsAny<ContractServiceType>()))
                .Returns(true);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            const bool expected = true;

            //Act
            bool actual = target.IsContractServiceTypeNameExit(new ContractServiceType());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Ifs the contract service type name is not unique.
        /// </summary>
        [TestMethod]
        public void IfContractServiceTypeNameIsNotUnique()
        {
            //Arrange
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.IsContractServiceTypeNameExit(It.IsAny<ContractServiceType>()))
                .Returns(false);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            const bool expected = false;
            //Act
            bool actual = target.IsContractServiceTypeNameExit(new ContractServiceType());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EvaluateIfConditionNotMatch()
        {

            //Arrange
            EvaluateableClaim evaluateableClaim =
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData { Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode> { new DiagnosisCode { Instance = "P" } },
                    ClaimCharges = new List<ClaimCharge> { new ClaimCharge { Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" } },
                    PriPayerName = "Medi",
                    CustomField1 = "Test1"
                };
           
            List<PaymentResult> paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };


            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();

            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object)
            {
                ContractServiceType = new ContractServiceType
                {
                    Conditions = new List<ICondition>
                    {
                        new Condition
                        {
                            OperandType = (int) Enums.OperandType.AlphaNumeric,
                            ConditionOperator = (int) Enums.ConditionOperation.EqualTo,
                            OperandIdentifier = (int) Enums.OperandIdentifier.PayerName,
                            LeftOperands = new List<string> {"Medicare", "Medi"},
                            PropertyColumnName = Constants.PropertyPriPayerName,
                            RightOperand = "Aetna"
                        }
                    }
                }
            };

            //Act
            var result = target.Evaluate(evaluateableClaim, paymentResults, false, false);

            //Assert
            Assert.AreEqual(result.Count, 2);
            var firstOrDefault = result.FirstOrDefault();
            if (firstOrDefault != null)
                Assert.AreEqual((object)firstOrDefault.AdjudicatedValue, null);
        }

        /// <summary>
        /// Checks the name if contract service type is null.
        /// </summary>
        [TestMethod]
        public void CheckNameIfContractServiceTypeIsNull()
        {
            //Arrange
            _mockContractServiceTypeRepository = new Mock<IContractServiceTypeRepository>();
            _mockContractServiceTypeRepository.Setup(x => x.IsContractServiceTypeNameExit(null))
                .Returns(false);
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(_mockContractServiceTypeRepository.Object);
            const bool expected = false;
            //Act
            bool actual = target.IsContractServiceTypeNameExit(null);

            // Assert
            Assert.AreEqual(expected, actual);
        }

    }
}
