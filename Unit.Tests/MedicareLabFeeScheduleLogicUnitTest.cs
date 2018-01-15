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
    public class MedicareLabFeeScheduleLogicUnitTest
    {
        /// <summary>
        /// Medicares the lab fee schedule logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void MedicareLabFeeScheduleLogicParameterlessConstructorTest()
        {
            var target = new MedicareLabFeeScheduleLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(MedicareLabFeeScheduleLogic));
        }

        /// <summary>
        /// Medicares the lab fee schedulef repository constructor unit test1.
        /// </summary>
        [TestMethod]
        public void MedicareLabFeeSchedulefRepositoryConstructorUnitTest1()
        {
            var mockMedicareLabFeeSchedule = new Mock<IMedicareLabFeeScheduleRepository>();
            MedicareLabFeeScheduleLogic target = new MedicareLabFeeScheduleLogic(mockMedicareLabFeeSchedule.Object);
            Assert.IsInstanceOfType(target, typeof(MedicareLabFeeScheduleLogic));
        }

        /// <summary>
        /// Gets the medicare lab fee schedule table names.
        /// </summary>
        [TestMethod]
        public void GetMedicareLabFeeScheduleTableNames()
        {
            var repository = new Mock<IMedicareLabFeeScheduleRepository>();
            List<MedicareLabFeeSchedule> result = new List<MedicareLabFeeSchedule>{new MedicareLabFeeSchedule()};
            repository.Setup(f => f.GetMedicareLabFeeScheduleTableNames(It.IsAny<MedicareLabFeeSchedule>())).Returns(result);
            MedicareLabFeeScheduleLogic target = new MedicareLabFeeScheduleLogic(repository.Object);

            List<MedicareLabFeeSchedule> actual = target.GetMedicareLabFeeScheduleTableNames(It.IsAny<MedicareLabFeeSchedule>());

            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the medicare lab fee schedule.
        /// </summary>
        [TestMethod]
        public void GetMedicareLabFeeSchedule()
        {
            var repository = new Mock<IMedicareLabFeeScheduleRepository>();
            MedicareLabFeeScheduleResult result = new MedicareLabFeeScheduleResult();
            repository.Setup(f => f.GetMedicareLabFeeSchedule(It.IsAny<MedicareLabFeeSchedule>())).Returns(result);
            MedicareLabFeeScheduleLogic target = new MedicareLabFeeScheduleLogic(repository.Object);
            MedicareLabFeeScheduleResult actual = target.GetMedicareLabFeeSchedule(new MedicareLabFeeSchedule());
            Assert.AreEqual(result, actual);
        }
    }
}
