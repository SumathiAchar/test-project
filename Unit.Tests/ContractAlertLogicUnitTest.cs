using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{


    /// <summary>
    ///This is a test class for ContarctAlertsLogicTest and is intended
    ///to contain all ContarctAlertsLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContarctAlertsLogicTest
    {
        /// <summary>
        /// A test for ContarctAlertsLogic Constructor
        /// </summary>
        [TestMethod]
        
        public void ContarctAlertsLogicConstructorTest()
        {
            ContractAlertLogic target = new ContractAlertLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractAlertLogic));
        }

        /// <summary>
        ///A test for ContarctAlertsLogic Constructor
        ///</summary>
        [TestMethod]
        public void ContarctAlertsLogicConstructorTest1()
        {
            Mock<IContractAlertRepository> mockProductRepository = new Mock<IContractAlertRepository>();
            ContractAlertLogic target = new ContractAlertLogic(mockProductRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ContractAlertLogic));
        }

        /// <summary>
        ///A test for GetContractAlerts If Null 
        ///</summary>
        [TestMethod]
        public void GetContractAlertsNullTest()
        {
            var repository = new Mock<IContractAlertRepository>();
            repository.Setup(f => f.GetContractAlerts(null));
            ContractAlertLogic target = new ContractAlertLogic(repository.Object);

            List<ContractAlert> actual = target.GetContractAlerts(null);
            Assert.AreEqual(null, actual);
        }

        /// <summary>
        /// Gets the contract alerts mock test.
        /// </summary>
        [TestMethod]
        public void GetContractAlertsMockTest()
        {
            //Mock input
            ContractAlert contractAlertList = new ContractAlert
                                                      {

                                                          UserName = "Admin2"

                                                      };
            //Mock output
            List<ContractAlert> test = new List<ContractAlert>
                                                   {
                                                       new ContractAlert
                                                           {
                                                               PayerName ="BCBS AL",
                                                               ContractName = "Maha testing #2013-13",
                                                               ContractId = 1526,
                                                               DateOfExpiry = new DateTime(11/7/2013),
                                                               
                                                           },

                                                           new ContractAlert
                                                           {
                                                               PayerName ="AETNA",
                                                               ContractName = "Ragini Test123 #2013-13",
                                                               ContractId = 1533,
                                                               DateOfExpiry = new DateTime(10/9/2013),
                                                               
                                                           },
                                                            new ContractAlert
                                                           {
                                                               PayerName ="BCBS AL",
                                                               ContractName = "Copy Of Maha testing #2013-13",
                                                               ContractId = 1540,
                                                               DateOfExpiry = new DateTime(11/7/2013),
                                                               
                                                           },
                                                            new ContractAlert
                                                           {
                                                               PayerName ="AETNA",
                                                               ContractName = "Copy Of Ragini Test123 #2013-13",
                                                               ContractId = 1544,
                                                               DateOfExpiry = new DateTime(10/9/2013),
                                                               
                                                           },
               };
            List<ContractAlert> expected = test;
            //Mock setup
            Mock<IContractAlertRepository> mockProductRepository = new Mock<IContractAlertRepository>();
            mockProductRepository.Setup(f => f.GetContractAlerts(It.IsAny<ContractAlert>())).Returns(test);
            ContractAlertLogic target = new ContractAlertLogic(mockProductRepository.Object);
            List<ContractAlert> actual = target.GetContractAlerts(contractAlertList);

            Assert.AreEqual(expected, actual);

        }

        //todo: ok
        /// <summary>
        ///A test for UpdateContractAlerts
        ///</summary>
        [TestMethod]
        public void UpdateContractAlertsIfNullTest()
        {

            var repository = new Mock<IContractAlertRepository>();
            repository.Setup(f => f.UpdateContractAlerts(null));
            ContractAlertLogic target = new ContractAlertLogic(repository.Object);

            bool actual = target.UpdateContractAlerts(null);
            Assert.AreEqual(false, actual);
        }
        /// <summary>
        /// Updates the contract alerts difference not null test.
        /// </summary>
        [TestMethod]
        public void UpdateContractAlertsIfNotNullTest()
        {
            //Arrange
            var repository = new Mock<IContractAlertRepository>();
            ContractAlert value = new ContractAlert();
            repository.Setup(f => f.UpdateContractAlerts(It.IsAny<ContractAlert>())).Returns(true);
            ContractAlertLogic target = new ContractAlertLogic(repository.Object);

            //Act
            bool actual = target.UpdateContractAlerts(value);

            //Assert
            Assert.AreEqual(true, actual);
        }
        /// <summary>
        /// Updates the contract alerts mock test1.
        /// </summary>
        [TestMethod]
        public void UpdateContractAlertsMockTest1()
        {
            Mock<IContractAlertRepository> mockContractAlertsRepository = new Mock<IContractAlertRepository>();
            ContractAlert objContractAlert = new ContractAlert { ContractId = 1 };
            mockContractAlertsRepository.Setup(f => f.UpdateContractAlerts(objContractAlert)).Returns(true);
            ContractAlertLogic target = new ContractAlertLogic(mockContractAlertsRepository.Object);
            bool actual = target.UpdateContractAlerts(objContractAlert);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Updates the alert verified status.
        /// </summary>
        [TestMethod]
        public void UpdateAlertVerifiedStatus()
        {
            Mock<IContractAlertRepository> contractAlertRepository = new Mock<IContractAlertRepository>();
            contractAlertRepository.Setup(x => x.UpdateAlertVerifiedStatus(It.IsAny<ContractAlert>())).Returns(true);

            new ContractAlertLogic(contractAlertRepository.Object).UpdateAlertVerifiedStatus(new ContractAlert());
            contractAlertRepository.VerifyAll();
        }

        /// <summary>
        /// Contracts the alert count.
        /// </summary>
        [TestMethod]
        public void ContractAlertCount()
        {
            Mock<IContractAlertRepository> contractAlertRepository = new Mock<IContractAlertRepository>();
            contractAlertRepository.Setup(x => x.ContractAlertCount(It.IsAny<ContractAlert>())).Returns(1);

            new ContractAlertLogic(contractAlertRepository.Object).ContractAlertCount(new ContractAlert());
            contractAlertRepository.VerifyAll();
        }


       }
}

