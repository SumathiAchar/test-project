using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for LetterTemplateLogicUnitTest
    /// </summary>
    [TestClass]
    public class LetterTemplateLogicUnitTest
    {
        /// <summary>
        /// Letters the template logic parameterless constructor test1.
        /// </summary>
        [TestMethod]
        public void LetterTemplateLogicParameterlessConstructorTest1()
        {
            var target = new LetterTemplateLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(LetterTemplateLogic));
        }

        /// <summary>
        /// Payments the table logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void LetterTemplateLogicConstructorTest2()
        {
            Mock<ILetterTemplateRepository> mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);
            Assert.IsInstanceOfType(target, typeof(LetterTemplateLogic));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        [TestMethod]
        public void Save()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            const long result = 12;
            mockLetterTemplateLogic.Setup(f => f.Save(It.IsAny<LetterTemplate>())).Returns(result);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);

            long actual = target.Save(new LetterTemplate());

            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the letter template by identifier.
        /// </summary>
        [TestMethod]
        public void GetLetterTemplateById()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            LetterTemplate result = new LetterTemplate();
            mockLetterTemplateLogic.Setup(f => f.GetLetterTemplateById(It.IsAny<LetterTemplate>())).Returns(result);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);

            LetterTemplate actual = target.GetLetterTemplateById(new LetterTemplate());

            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Determines whether [is letter name exists].
        /// </summary>
        [TestMethod]
        public void IsLetterNameExists()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            mockLetterTemplateLogic.Setup(f => f.IsLetterNameExists(It.IsAny<LetterTemplate>())).Returns(true);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);
            bool actual = target.IsLetterNameExists(new LetterTemplate());
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Gets the letter templates.
        /// </summary>
        [TestMethod]
        public void GetLetterTemplates()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            LetterTemplateContainer result = new LetterTemplateContainer();
            mockLetterTemplateLogic.Setup(f => f.GetLetterTemplates(It.IsAny<LetterTemplateContainer>()))
                .Returns(result);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);
            LetterTemplateContainer actual = target.GetLetterTemplates(new LetterTemplateContainer());
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            mockLetterTemplateLogic.Setup(f => f.Delete(It.IsAny<LetterTemplate>())).Returns(true);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);
            bool actual = target.Delete(new LetterTemplate());
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        [TestMethod]
        public void InsertAuditLog()
        {
            var mockLetterTemplateLogic = new Mock<ILetterTemplateRepository>();
            mockLetterTemplateLogic.Setup(f => f.InsertAuditLog(It.IsAny<LetterTemplate>())).Returns(true);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateLogic.Object);
            bool actual = target.InsertAuditLog(new LetterTemplate());
            Assert.AreEqual(true, actual);
        }
    }
}
