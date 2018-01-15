using System.Collections.Generic;
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
    public class RtaLogicTest
    {
        private static RtaLogic _target;
        private Mock<IRtaRepository> _mockRtaRepository;
        private Mock<IAdjudicationEngine> _mockAdjudicationEngine;

        /// <summary>
        /// Rtas the logic parameterless constructor.
        /// </summary>
        [TestMethod]
        public void RtaLogicParameterlessConstructor()
        {
            _target = new RtaLogic();
            //Assert
            Assert.IsInstanceOfType(_target, typeof(RtaLogic));
        }
        /// <summary>
        /// Rtas the logic parameterless constructor.
        /// </summary>
        [TestMethod]
        public void RtaLogicParameterlessConstructor1()
        {
            var target = new RtaLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(RtaLogic));
        }

        /// <summary>
        /// Rtas the logic parametrized constructor test.
        /// </summary>
        [TestMethod]
        public void RtaLogicParametrizedConstructorTest()
        {
            var mockRtaRepository = new Mock<IRtaRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            _target = new RtaLogic(mockRtaRepository.Object, mockAdjudicationEngine.Object);
            Assert.IsInstanceOfType(_target, typeof(RtaLogic));
        }

        /// <summary>
        /// Gets the rta data by claim is not null.
        /// </summary>
        [TestMethod]
        public void GetRtaDataByClaimIsNotNull()
        {

            //Mock Input
            EvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                ClaimId = 123,
                ClaimTotal = 100,
                SmartBox = new SmartBox
                {
                    CAA = 5,
                    LOS = 10,
                    TCC = 15
                },
                ClaimCharges = new List<ClaimCharge>
                {
                    new ClaimCharge
                    {
                        Line = 1,
                        Amount = 20,
                        RevCode = "300"
                    }
                }
            };
            RtaData rtaData = new RtaData
            {
                Contracts = new List<Contract>
                {
                    new Contract
                    {
                        ContractId = 111,
                        ContractName = "Contract1"
                    }
                },
                EvaluateableClaim = new EvaluateableClaim
                {
                    ClaimId = 123,
                    ClaimTotal = 100,
                    SmartBox = new SmartBox
                    {
                        CAA = 5,
                        LOS = 10,
                        TCC = 15
                    },
                    ClaimCharges = new List<ClaimCharge>
                    {
                        new ClaimCharge
                        {
                            Line = 1,
                            Amount = 20,
                            RevCode = "300"
                        }
                    }
                }
            };
            _mockRtaRepository = new Mock<IRtaRepository>();
            _mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            _mockRtaRepository.Setup(f => f.GetRtaDataByClaim(It.IsAny<EvaluateableClaim>())).Returns(rtaData);
            _target = new RtaLogic(_mockRtaRepository.Object, _mockAdjudicationEngine.Object);
            RtaData actual = _target.GetRtaDataByClaim(evaluateableClaim);
            Assert.AreEqual(rtaData, actual);
        }

        /// <summary>
        /// Saves the time log test.
        /// </summary>
        [TestMethod]
        public void SaveTimeLogTest()
        {

            //Mock Input
            RtaEdiTimeLog rtaEdiTimeLog = new RtaEdiTimeLog
            {
                LogId = 123,
                RequestType = "Request",
                TimeTaken = 14,
                EdiResponseId = 789
            };

            const long result = 896;
            _mockRtaRepository = new Mock<IRtaRepository>();
            _mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            _mockRtaRepository.Setup(f => f.SaveTimeLog(It.IsAny<RtaEdiTimeLog>())).Returns(result);
            _target = new RtaLogic(_mockRtaRepository.Object, _mockAdjudicationEngine.Object);
            long actual = _target.SaveTimeLog(rtaEdiTimeLog);
            Assert.AreEqual(result, actual);
        }

    }
}
