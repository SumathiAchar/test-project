using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class AdjudicationEngineTest
    {
        //Creating object for Logic
        private AdjudicationEngine _target;

        //Creating mock object for ContractLogic
        private Mock<IContractLogic> _mockContractLogic;
        //Creating mock object for ContractRepository
        private Mock<IContractRepository> _mockContractRepository;
        //Creating mock object for EvaluateableClaimLogic
        private Mock<IEvaluateableClaimLogic> _mockEvaluateableClaimLogic;
        //Creating mock object for EvaluateableClaimRepository
        private Mock<IEvaluateableClaimRepository> _mockEvaluateableClaimRepository;
        //Creating mock object for PaymentResultLogic
        private Mock<IPaymentResultLogic> _mockPaymentResultLogic;
        //Creating mock object for ContractLogLogic
        private Mock<IContractLogLogic> _mockContractLogLogic;
        //Creating mock object for FacilityLogic
        private Mock<IFacilityLogic> _mockFacilityLogic;

        

        /// <summary>
        /// Adjudicates the task claims test.
        /// </summary>
        [TestMethod]
        public void AdjudicateTaskClaimsThreadTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 1,
                    AdjudicatedValue = 100.58
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };

            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(contractList[0])).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());


            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaimsDataThread(1, 1, contractList, 1, 50);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Adjudicates the claims if not null.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfNotNull()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Adjudicates the claims if line and service type is null.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfLineAndServiceTypeIsNull()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField2 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180},
                new PaymentResult {ClaimId = 411930180}
            };
            const double expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].ClaimId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims if line and service type is not null.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfLineAndServiceTypeIsNotNull()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, Line = 1, ServiceTypeId = 5},
                new PaymentResult {ClaimId = 411930180, Line = 2, ServiceTypeId = 10}
            };
            const double expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());


            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].ClaimId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims if contract identifier is not null.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfContractIdIsNotNull()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };
            const double expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].ClaimId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims if contract identifier is null.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfContractIdIsNull()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = null},
                new PaymentResult {ClaimId = 411930180}
            };
            const double expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].ClaimId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims if exception.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfException()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = null},
                new PaymentResult {ClaimId = 411930180}
            };
            const double expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].ClaimId;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the task claims thread if claims not null test.
        /// </summary>
        [TestMethod]
        public void AdjudicateTaskClaimsThreadIfClaimsNotNullTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                   ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaimsDataThread(1, 1, contractList, 1, 50);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the task claims thread if claims null test.
        /// </summary>
        [TestMethod]
        public void AdjudicateTaskClaimsThreadIfClaimsNullTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                   ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaimsDataThread(1, 1, contractList, 1, 50);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims if claim is early exit test.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsIfClaimIsEarlyExitTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1",
                    BackgroundContractId = 0
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims for contract conditions.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForContractConditions()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions =  new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "121",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               },
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField1 = "Test1",
                    BackgroundContractId = 0
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims for retained claims test.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForRetainedClaimsTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 7845,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions =  new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "121",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               },
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "blue Cross",
                    CustomField1 = "Test1",
                    BackgroundContractId = 123,
                    ContractId = 7845
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockContractLogic.Setup(x => x.Contract).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adjudicates the claims for background claims test.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForBackgroundClaimsTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 7845,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions =  new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "121",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               },
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 29,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "blue Cross",
                    CustomField1 = "Test1",
                    BackgroundContractId = 7845
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };
            const double expected = 100.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockContractLogic.Setup(x => x.Contract).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());


            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            double? actual = evaluateableClaim[0].AdjudicatedValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Adds the claims for a task test.
        /// </summary>
        [TestMethod]
        public void AddClaimsForATaskTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            const long result = 1;
            const long taskId = 892;
            _mockEvaluateableClaimRepository.Setup(x => x.UpdateRunningTask(It.IsAny<long>(), 1));
            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(result);
            

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var expected = _target.AddClaimsForATask(taskId);

            // Assert
            double actual = 1;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Gets the contracts test.
        /// </summary>
        [TestMethod]
        public void GetContractsTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            const long taskId = 892;
            const int facilityId = 725;
            const long totalClaimCount = 500;
            var expected = new List<Contract>
            {
                new Contract {ContractId = 147, ContractName = "Contract1"}
            };
            Facility facility = new Facility
            {
                FacilityName = "Facility1",
                IsMedicareIpAcute = true,
                IsMedicareOpApc = true

            };
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(facility);
            _mockContractRepository.Setup(
                x => x.GetContracts(taskId, facility.IsMedicareIpAcute, facility.IsMedicareOpApc)).Returns(expected);
            

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.GetContracts(taskId, facilityId, totalClaimCount);

            // Assert
            Assert.AreEqual(expected.First().ContractId, result.First().ContractId);
        }

        /// <summary>
        /// Adjudicates the task claims thread if evaluateable claims is not null test.
        /// </summary>
        [TestMethod]
        public void AdjudicateTaskClaimsThreadIfEvaluateableClaimsIsNotNullTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 1,
                    AdjudicatedValue = 100.58
                }
            };
            var adjudicateClaims = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 158,
                    AdjudicatedValue = 101.58
                }
            };
            var earlyExitClaims = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 15288,
                    AdjudicatedValue = 102.58
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123},
                new PaymentResult {ClaimId = 123, Line = 1}
            };

            const double expected = 101.58;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<long>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockContractLogic.Setup(x => x.AdjudicateClaims).Returns(adjudicateClaims);
            _mockContractLogic.Setup(x => x.EarlyExitClaims).Returns(earlyExitClaims);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());


            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaimsDataThread(1, 1, contractList, 1, 50);

            // Assert
            Assert.AreEqual(expected, result.AdjudicateClaims[0].AdjudicatedValue);
        }

        /// <summary>
        /// Updates the payment results thread test.
        /// </summary>
        [TestMethod]
        public void UpdatePaymentResultsThreadTest()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross"
                }
            };
            var adjudicatedClaims = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 158,
                    AdjudicatedValue = 101.58
                }
            };
            var earlyExitClaims = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 15288,
                    AdjudicatedValue = 102.58
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 10},
                new PaymentResult {ClaimId = 123, Line = 1, ServiceTypeId = 5}
            };
            var paymentResultDictionary = new Dictionary<long, List<PaymentResult>>
            {
                {1, paymentResults}
            };

            _mockPaymentResultLogic.Setup(
                x => x.UpdatePaymentResults(It.IsAny<Dictionary<long, List<PaymentResult>>>(), 1, 1, It.IsAny<List<EvaluateableClaim>>(), It.IsAny<List<EvaluateableClaim>>()))
                .Returns(true);
            _mockEvaluateableClaimRepository.Setup(x => x.UpdateRunningTask(1, 1));
            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.UpdatePaymentResultsThread(1, 1, contractList, paymentResultDictionary, adjudicatedClaims, earlyExitClaims);

            // Assert
            Assert.AreEqual(true, result);
        }



        /// <summary>
        /// Adjudicates the claims for property custom field2.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForPropertyCustomField2()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 30,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField2 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };
            const long expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            _mockContractLogic.Setup(
                x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false))
                    //It.IsAny<IEvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), true, true))
                .Returns(paymentResults);

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            Assert.AreEqual(expected, result.FirstOrDefault().Key);
        }


        /// <summary>
        /// Adjudicates the claims for property custom field3.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForPropertyCustomField3()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 31,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField3 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };
            const long expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            _mockContractLogic.Setup(
                x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false))
                //It.IsAny<IEvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), true, true))
                .Returns(paymentResults);

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            Assert.AreEqual(expected, result.FirstOrDefault().Key);
        }

        /// <summary>
        /// Adjudicates the claims for property custom field4.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForPropertyCustomField4()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 32,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField4 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };
            const long expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            _mockContractLogic.Setup(
                x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false))
                //It.IsAny<IEvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), true, true))
                .Returns(paymentResults);

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            Assert.AreEqual(expected, result.FirstOrDefault().Key);
        }

        /// <summary>
        /// Adjudicates the claims for property custom field5.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForPropertyCustomField5()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 33,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField5 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101},
                new PaymentResult {ClaimId = 411930180}
            };
            const long expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            _mockContractLogic.Setup(
                x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false))
                //It.IsAny<IEvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), true, true))
                .Returns(paymentResults);

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            Assert.AreEqual(expected, result.FirstOrDefault().Key);
        }

        /// <summary>
        /// Adjudicates the claims for property custom field6.
        /// </summary>
        [TestMethod]
        public void AdjudicateClaimsForPropertyCustomField6()
        {
            _mockContractLogic = new Mock<IContractLogic>();
            _mockContractRepository = new Mock<IContractRepository>();
            _mockEvaluateableClaimLogic = new Mock<IEvaluateableClaimLogic>();
            _mockEvaluateableClaimRepository = new Mock<IEvaluateableClaimRepository>();
            _mockPaymentResultLogic = new Mock<IPaymentResultLogic>();
            _mockContractLogLogic = new Mock<IContractLogLogic>();
            _mockFacilityLogic = new Mock<IFacilityLogic>();
            // Arrange
            var contractList = new List<Contract>
            {
                new Contract
                {
                    ContractId = 123,
                    ContractName = "ABC",
                    IsProfessional = true,
                    PayersList = "blue Cross",
                    Conditions = new List<ICondition>(),
                    Payers = new List<Payer>{ new Payer{PayerName = "Aetna"}},
                    CustomField = 34,
                    PayerCode = "Test1"
                }
            };
            var evaluateableClaim = new List<EvaluateableClaim>
            {
                new EvaluateableClaim
                {
                    ClaimId = 411930180,
                    PatientData = new PatientData{Dob = Convert.ToDateTime("1/1/2000") },
                    AdjudicatedValue = 100.58,
                    DiagnosisCodes = new List<DiagnosisCode>{new DiagnosisCode{Instance = "P"}},
                    ClaimCharges = new List<ClaimCharge>{new ClaimCharge{Amount = 22, CoveredCharge = 22, HcpcsCode = "36415" }},
                    PriPayerName = "Aetna",
                    CustomField6 = "Test1"
                }
            };
            var paymentResults = new List<PaymentResult>
            {
                new PaymentResult {ClaimId = 411930180, ContractId = 101}
            };
            const long expected = 411930180;

            _mockEvaluateableClaimRepository.Setup(x => x.AddClaimsForATask(It.IsAny<long>())).Returns(1);
            _mockContractRepository.Setup(x => x.GetContracts(It.IsAny<long>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(contractList);
            _mockEvaluateableClaimRepository.Setup(x => x.GetEvaluateableClaims(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>())).Returns(evaluateableClaim);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockPaymentResultLogic.Setup(x => x.GetPaymentResults(evaluateableClaim[0])).Returns(paymentResults);
            // ReSharper disable once ImplicitlyCapturedClosure
            _mockEvaluateableClaimLogic.Setup(x => x.UpdateEvaluateableClaims(evaluateableClaim)).Returns(evaluateableClaim);
            _mockEvaluateableClaimLogic.Setup(x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false)).Returns(paymentResults);
            _mockContractLogic.Setup(x => x.UpdateContractCondition(It.IsAny<Contract>())).Returns(contractList[0]);
            _mockFacilityLogic.Setup(x => x.GetFacilityMedicareDetails(It.IsAny<int>())).Returns(new Facility());
            _mockContractLogic.Setup(
                x => x.Evaluate(evaluateableClaim[0], paymentResults, false, false))
                //It.IsAny<IEvaluateableClaim>(), It.IsAny<List<PaymentResult>>(), true, true))
                .Returns(paymentResults);

            _target = new AdjudicationEngine(_mockContractLogic.Object, _mockContractRepository.Object, _mockEvaluateableClaimLogic.Object,
                        _mockEvaluateableClaimRepository.Object, _mockPaymentResultLogic.Object, _mockContractLogLogic.Object, _mockFacilityLogic.Object);

            //Act
            var result = _target.AdjudicateClaim(evaluateableClaim, contractList, 1);

            // Assert
            Assert.AreEqual(expected, result.FirstOrDefault().Key);
        }

        
    }

}
