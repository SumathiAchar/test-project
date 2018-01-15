using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
     [TestClass]
    public class PaymentTableLogicTest
    {
        /// <summary>
        /// Payments the table logic constructor test.
        /// </summary>
        [TestMethod]
         public void PaymentTableLogicConstructorTest()
        {
            var target = new PaymentTableLogic();
            //Assert
            Assert.IsInstanceOfType(target, typeof(PaymentTableLogic));
        }

        /// <summary>
        /// Payments the table logic constructor test1.
        /// </summary>
        [TestMethod]
        public void PaymentTableLogicConstructorTest1()
        {
            var mockPaymentTableRepository = new Mock<IPaymentTableRepository>();
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableRepository.Object);
            Assert.IsInstanceOfType(target, typeof(PaymentTableLogic));
        }

        /// <summary>
        /// Gets all claim fields test.
        /// </summary>
        [TestMethod]
        public void GetAllClaimFieldsTest()
        {
            // Arrange
            var mockPaymentTableRepository = new Mock<IPaymentTableRepository>();
            List<ClaimField> claimFieldList = new List<ClaimField>
                {
                    new ClaimField {ClaimFieldDocId = 1, TableName = "ASC Table"}
                };
            mockPaymentTableRepository.Setup(f => f.GetAllClaimFields()).Returns(claimFieldList);
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableRepository.Object);

            // Act
            List<ClaimField> actual = target.GetAllClaimFields();

            // Assert
            Assert.AreEqual(claimFieldList, actual);
        }

        /// <summary>
        /// Gets the m care lab fee schedule table name test.
        /// </summary>
        [TestMethod]
        public void GetMedicareLabFeeScheduleTableNameTest()
        {
            // Arrange
            var mockMedicareLabFeeScheduleRepository = new Mock<IMedicareLabFeeScheduleRepository>();
            List<MedicareLabFeeSchedule> mCareLabFeeSchedules = new List<MedicareLabFeeSchedule>
                {
                    new MedicareLabFeeSchedule {Date = 20140401, Name = "M Care lab fee schedule 04-01-2014"},
                    new MedicareLabFeeSchedule {Date = 20140101, Name = "M Care lab fee schedule 01-01-2014"}
                };
            mockMedicareLabFeeScheduleRepository.Setup(f => f.GetMedicareLabFeeScheduleTableNames()).Returns(mCareLabFeeSchedules);
            MedicareLabFeeScheduleLogic target = new MedicareLabFeeScheduleLogic(mockMedicareLabFeeScheduleRepository.Object);

            // Act
            List<MedicareLabFeeSchedule> actual = target.GetMedicareLabFeeScheduleTableNames();

            // Assert
            Assert.AreEqual(mCareLabFeeSchedules, actual);
        }

        /// <summary>
        /// Gets the m care lab fee schedule table data test.
        /// </summary>
        [TestMethod]
        public void GetMedicareLabFeeScheduleTest()
        {
            // Arrange
            var mockMedicareLabFeeScheduleRepository = new Mock<IMedicareLabFeeScheduleRepository>();
            MedicareLabFeeScheduleResult medicareLabFeeScheduleResult = new MedicareLabFeeScheduleResult
                {
                    Total = 22578,
                    MedicareLabFeeScheduleList =
                        new List<MedicareLabFeeSchedule>
                            {
                                new MedicareLabFeeSchedule {Amount = 20, Carrier = "12456", HCPCS = "85420"},
                                new MedicareLabFeeSchedule {Amount = 12, Carrier = "12456", HCPCS = "8542"}
                            }
                };
            mockMedicareLabFeeScheduleRepository.Setup(f => f.GetMedicareLabFeeSchedule(It.IsAny<MedicareLabFeeSchedule>())).Returns(medicareLabFeeScheduleResult);
            MedicareLabFeeScheduleLogic target = new MedicareLabFeeScheduleLogic(mockMedicareLabFeeScheduleRepository.Object);

            // Act
            MedicareLabFeeScheduleResult actual = target.GetMedicareLabFeeSchedule(null);

            // Assert
            Assert.AreEqual(medicareLabFeeScheduleResult, actual);
        }

        /// <summary>
        /// Gets the payment table claim fields test.
        /// </summary>
        [TestMethod]
        public void GetPaymentTableClaimFieldsTest()
        {
            // Arrange
            var mockPaymentTableRepository = new Mock<IPaymentTableRepository>();
            List<ClaimField> claimFieldList = new List<ClaimField>
                {
                    new ClaimField {ClaimFieldId = 1, Text = "Test name 1", ClaimFieldDocId = 1, TableName = "Table 1"},
                    new ClaimField {ClaimFieldId = 2, Text = "Test name 2", ClaimFieldDocId = 2, TableName = "Table 2"}
                };
            mockPaymentTableRepository.Setup(f => f.GetPaymentTableClaimFields()).Returns(claimFieldList);
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableRepository.Object);

            //Act
            List<ClaimField> actual = target.GetPaymentTableClaimFields();

            //Assert
            Assert.AreEqual(claimFieldList, actual);
        }

        /// <summary>
        /// Determines whether [is table name exists test].
        /// </summary>
        [TestMethod]
        public void IsTableNameExistsTest()
        {
            // Arrange
            var mockPaymentTableRepository = new Mock<IPaymentTableRepository>();
            mockPaymentTableRepository.Setup(f => f.IsTableNameExists(It.IsAny<ClaimFieldDoc>())).Returns(true);
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableRepository.Object);

            //Act
            bool actual = target.IsTableNameExists(null);

            //Assert
            Assert.AreEqual(true, actual);
        }
    }
}
