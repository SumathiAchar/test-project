using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class MedicareIpAcuteOptionLogicTests
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void MedicareIpAcuteOptionConstructorUnitTest1()
        {
            var target = new MedicareIpAcuteOptionLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(MedicareIpAcuteOptionLogic));
        }


        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void PaymentTypeMedicareIpDetailsConstructorUnitTest2()
        {
            var mockMedicareIpAcuteOption = new Mock<IMedicareIpAcuteOptionRepository>();
            MedicareIpAcuteOptionLogic target = new MedicareIpAcuteOptionLogic(mockMedicareIpAcuteOption.Object);
            Assert.IsInstanceOfType(target, typeof(MedicareIpAcuteOptionLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetMedicareIpAcuteOptionsIsNotNullTest()
        {
            var mockGetMedicareIpAcuteOption = new Mock<IMedicareIpAcuteOptionRepository>();
            List<MedicareIpAcuteOption> results = new List<MedicareIpAcuteOption>();
            mockGetMedicareIpAcuteOption.Setup(f => f.GetMedicareIpAcuteOptions()).Returns(results);
            MedicareIpAcuteOptionLogic target = new MedicareIpAcuteOptionLogic(mockGetMedicareIpAcuteOption.Object);
            List<MedicareIpAcuteOption> actual = target.GetMedicareIpAcuteOptions();
            Assert.IsNotNull(actual);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetMedicareIpAcuteOptionsIsInstanceOfTypeTest()
        {
            var mockGetMedicareIpAcuteOption = new Mock<IMedicareIpAcuteOptionRepository>();
            List<MedicareIpAcuteOption> results = new List<MedicareIpAcuteOption>();
            mockGetMedicareIpAcuteOption.Setup(f => f.GetMedicareIpAcuteOptions()).Returns(results);
            MedicareIpAcuteOptionLogic target = new MedicareIpAcuteOptionLogic(mockGetMedicareIpAcuteOption.Object);
            List<MedicareIpAcuteOption> actual = target.GetMedicareIpAcuteOptions();
            Assert.IsInstanceOfType(actual, typeof(List<MedicareIpAcuteOption>));
        }
    }
}
