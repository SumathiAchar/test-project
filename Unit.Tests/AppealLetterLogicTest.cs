using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class AppealLetterLogicTest
    {
        /// <summary>
        /// Gets all letter templates without pagination test.
        /// </summary>
        [TestMethod]
        public void GetAllLetterTemplatesWithoutPaginationTest()
        {
            // Arrange
            var mockAppealLetterRepository = new Mock<IAppealLetterRepository>();
            List<LetterTemplate> letterTemplates = new List<LetterTemplate>
                {
                    new LetterTemplate {LetterTemplateId = 1, Name = "Test Letter 1", TemplateText = "Template Text 1"},
                    new LetterTemplate {LetterTemplateId = 2, Name = "Test Letter 2", TemplateText = "Template Text 2"}
                };
            mockAppealLetterRepository.Setup(f => f.GetAppealTemplates())
                                        .Returns(letterTemplates);
            AppealLetterLogic target = new AppealLetterLogic(mockAppealLetterRepository.Object);

            //Act
            List<LetterTemplate> actual = target.GetAppealTemplates();

            //Assert
            Assert.AreEqual(letterTemplates, actual);
        }

        /// <summary>
        /// Gets all letter templates test.
        /// </summary>
        [TestMethod]
        public void GetAllLetterTemplatesTest()
        {
            //Arrange
            var mockLetterTemplateRepository = new Mock<ILetterTemplateRepository>();
            LetterTemplateContainer result = new LetterTemplateContainer
                {
                    LetterTemplates =
                        new List<LetterTemplate>
                            {
                                new LetterTemplate {LetterTemplateId = 1, Name = "Test1", TemplateText = "Letter Text 1"},
                                new LetterTemplate {LetterTemplateId = 2, Name = "Test2", TemplateText = "Letter Text 2"}
                            },
                    TotalRecords = 200,
                    PageSetting = new PageSetting()
                };
            mockLetterTemplateRepository.Setup(f => f.GetLetterTemplates(It.IsAny<LetterTemplateContainer>()))
                .Returns(result);
            LetterTemplateLogic target = new LetterTemplateLogic(mockLetterTemplateRepository.Object);

            //Act
            LetterTemplateContainer actual = target.GetLetterTemplates(null);

            //Assert
            Assert.AreEqual(result, actual);
        }

    }
}
