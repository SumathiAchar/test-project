using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for PaymentTableLogicUnitTest
    /// </summary>
    [TestClass]
    public class PaymentTableLogicUnitTest
    {
        /// <summary>
        /// Payments the table logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTableLogicParameterlessConstructorTest1()
        {
            var target = new PaymentTableLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof (PaymentTableLogic));
        }

        /// <summary>
        /// Payments the table logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void PaymentTableLogicParameterlessConstructorTest2()
        {
            Mock<IPaymentTableRepository> mockPaymentTableLogic = new Mock<IPaymentTableRepository>();
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableLogic.Object);
            Assert.IsInstanceOfType(target, typeof (PaymentTableLogic));
        }

        /// <summary>
        /// Determines whether [is table name exists].
        /// </summary>
        [TestMethod]
        public void IsTableNameExists()
        {
            var mockPaymentTableLogic = new Mock<IPaymentTableRepository>();
            const bool result = true;
            mockPaymentTableLogic.Setup(f => f.IsTableNameExists(It.IsAny<ClaimFieldDoc>())).Returns(result);
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableLogic.Object);

            bool actual = target.IsTableNameExists(new ClaimFieldDoc());

            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the payment table.
        /// </summary>
        [TestMethod]
        public void GetPaymentTable()
        {
            var mockPaymentTableLogic = new Mock<IPaymentTableRepository>();
            PaymentTableContainer result = new PaymentTableContainer();
            mockPaymentTableLogic.Setup(f => f.GetPaymentTable(It.IsAny<ClaimFieldDoc>())).Returns(result);
            PaymentTableLogic target = new PaymentTableLogic(mockPaymentTableLogic.Object);

            PaymentTableContainer actual = target.GetPaymentTable(new ClaimFieldDoc());

            Assert.AreEqual(result, actual);
        }

         [TestMethod]
        public void GetCustomPaymentTable()
         {
             var repository = new Mock<IPaymentTableRepository>();
             PaymentTableContainer result = new PaymentTableContainer();
             var value = new ClaimFieldDoc {ClaimFieldDocId = 10101, ClaimFieldId = 35, PageSetting = new PageSetting { Skip = 0, SortDirection = "", SortField = "", Take = 5}};
             repository.Setup(
                 f => f.GetCustomPaymentTable(value)).Returns(result);
             PaymentTableLogic target = new PaymentTableLogic(repository.Object);

             PaymentTableContainer actual =
                 target.GetPaymentTable(value);

             Assert.AreEqual(result, actual);
         }

    }
}
