using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for AppealLetterLogicUnitTest
    /// </summary>
    [TestClass]
    public class AppealLetterLogicUnitTest
    {
        /// <summary>
        /// Appeals the letter logic parameterless constructor test1.
        /// </summary>
        [TestMethod]
        public void AppealLetterLogicParameterlessConstructorTest1()
        {
            var target = new AppealLetterLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof (AppealLetterLogic));
        }

        /// <summary>
        /// Appeals the letter logic parameterless constructor test2.
        /// </summary>
        [TestMethod]
        public void AppealLetterLogicParameterlessConstructorTest2()
        {
            Mock<IAppealLetterRepository> mockAppealLetterLogic = new Mock<IAppealLetterRepository>();
            AppealLetterLogic target = new AppealLetterLogic(mockAppealLetterLogic.Object);
            Assert.IsInstanceOfType(target, typeof (AppealLetterLogic));
        }

        /// <summary>
        /// Gets the appeal letter.
        /// </summary>
        [TestMethod]
        public void GetAppealLetter()
        {
            Mock<IAppealLetterRepository> mockAppealLetterLogic = new Mock<IAppealLetterRepository>();
            AppealLetter result = new AppealLetter();
            mockAppealLetterLogic.Setup(f => f.GetAppealLetter(It.IsAny<AppealLetter>())).Returns(result);
            AppealLetterLogic target = new AppealLetterLogic(mockAppealLetterLogic.Object);

            AppealLetter actual = target.GetAppealLetter(new AppealLetter());

            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        [TestMethod]
        public void GetAppealTemplates()
        {
            Mock<IAppealLetterRepository> mockAppealLetterLogic = new Mock<IAppealLetterRepository>();
            List<LetterTemplate> result = new List<LetterTemplate>();
            //int facilityId = 3;
            LetterTemplate templateinfo = new LetterTemplate {FacilityId = 3};
            mockAppealLetterLogic.Setup(f => f.GetAppealTemplates(templateinfo)).Returns(result);
            AppealLetterLogic target = new AppealLetterLogic(mockAppealLetterLogic.Object);

            List<LetterTemplate> actual = target.GetAppealTemplates(templateinfo);

            Assert.AreEqual(result, actual);
        }
    }
}
