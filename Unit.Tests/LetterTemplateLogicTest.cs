using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
   
    [TestClass]
    public class LetterTemplateLogicTest
    {
        /// <summary>
        /// Adds the edit letter template.
        /// </summary>
        [TestMethod]
        public void AddEditLetterTemplate()
        {
            // Arrange
            var mockLetterTemplateRepository = new Mock<ILetterTemplateRepository>();
            const long result = 2;
            mockLetterTemplateRepository.Setup(f => f.Save(It.IsAny<LetterTemplate>())).Returns(result);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateRepository.Object);

            // Act
            long actual = target.Save(null);

            // Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the letter template by identifier test.
        /// </summary>
        [TestMethod]
        public void GetLetterTemplateByIdTest()
        {
            // Arrange
            var mockLetterTemplateRepository = new Mock<ILetterTemplateRepository>();
            LetterTemplate letterTemplateResult = new LetterTemplate();
            mockLetterTemplateRepository.Setup(f => f.GetLetterTemplateById(It.IsAny<LetterTemplate>())).Returns(letterTemplateResult);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateRepository.Object);

            //Act
            LetterTemplate actual = target.GetLetterTemplateById(null);

            //Assert 
            Assert.AreEqual(letterTemplateResult, actual);
        }


        /// <summary>
        /// Determines whether [is letter name exists test].
        /// </summary>
        [TestMethod]
        public void IsLetterNameExistsTest()
        {
            // Arrange
            var mockLetterTemplateRepository = new Mock<ILetterTemplateRepository>();
            mockLetterTemplateRepository.Setup(f => f.IsLetterNameExists(It.IsAny<LetterTemplate>())).Returns(true);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateRepository.Object);

            //Act
            bool actual = target.IsLetterNameExists(null);

            //Assert
            Assert.AreEqual(true, actual);
        }
    }
}
