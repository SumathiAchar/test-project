using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for AccountActivationLogicUnitTest and is intended
    ///to contain all AccountActivationLogic Unit Tests
    ///</summary>
    [TestClass]
    public class AccountActivationLogicUnitTest
    {
        private static AccountActivationLogic _target;

        /// <summary>
        ///A test for AccountActivationLogic Constructor
        ///</summary>
        [TestMethod]
        public void AccountActivationLogicConstructorTest()
        {
            AccountActivationLogic target = new AccountActivationLogic(null);
            //Assert
            Assert.IsInstanceOfType(target, typeof(AccountActivationLogic));
        }

        /// <summary>
        /// AccountActivationLogic the details logic constructor test.
        /// </summary>
        [TestMethod]
        public void AccountActivationLogicParametrizedConstructorTest()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Act
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Assert
            Assert.IsInstanceOfType(_target, typeof(AccountActivationLogic));
        }

        /// <summary>
        ///A test for GetSecurityQuestion
        ///</summary>
        [TestMethod]
        public void GetSecurityQuestionUnitTest()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Input
            User userInput = new User
            {
                UserId = 12
            };

            // Mock Output
            List<SecurityQuestion> securityQuestions = new List<SecurityQuestion>
            {
              new SecurityQuestion{QuestionId =1,Question ="In what city does your nearest sibling live?",Answer = "1245"},
              new SecurityQuestion{QuestionId =2,Question ="In what city or town did your mother and father meet?",Answer = "1245"},
              new SecurityQuestion{QuestionId =3,Question ="In what city or town was your first job?",Answer = "1245"}  
            };

            // Mock Setup
            mockAccountRepository.Setup(f => f.GetSecurityQuestion(It.IsAny<User>())).Returns(securityQuestions);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            List<SecurityQuestion> actual = _target.GetSecurityQuestion(userInput);

            // Assert
            Assert.AreEqual(securityQuestions, actual);
            Assert.AreEqual(securityQuestions[0].QuestionId, actual[0].QuestionId);
            Assert.AreEqual(securityQuestions[0].Question, actual[0].Question);
            Assert.AreEqual(securityQuestions[0].Answer, actual[0].Answer);
        }

        /// <summary>
        ///A nagative test for GetSecurityQuestion
        ///</summary>
        [TestMethod]
        public void GetSecurityQuestionUnitTestForNull()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            //Mock Setup
            mockAccountRepository.Setup(f => f.GetSecurityQuestion(It.IsAny<User>())).Returns((List<SecurityQuestion>)null);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            List<SecurityQuestion> actual = _target.GetSecurityQuestion(null);

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for IsUserEmailExist
        ///</summary>
        [TestMethod]
        public void IsUserEmailExistUnitTest()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Input
            User userInput = new User
            {
                UserId = 12
            };

            // Mock Output
            const int expectedResult = 2;
            //Mock Setup
            mockAccountRepository.Setup(f => f.IsUserEmailExist(It.IsAny<User>())).Returns(expectedResult);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.IsUserEmailExist(userInput);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        ///A nagative test for IsUserEmailExist
        ///</summary>
        [TestMethod]
        public void IsUserEmailExistUnitTestForNull()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Output
            const int expectedResult = 0;

            //Mock Setup
            mockAccountRepository.Setup(f => f.IsUserEmailExist(It.IsAny<User>())).Returns(null);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.IsUserEmailExist(null);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }


        /// <summary>
        ///A test for IsUserEmailLockedOrNot
        ///</summary>
        [TestMethod]
        public void IsUserEmailLockedOrNotUnitTest()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Input
            User userInput = new User
            {
                UserId = 12
            };

            // Mock Output
            const int expectedResult = 2;
            //Mock Setup
            mockAccountRepository.Setup(f => f.IsUserEmailLockedOrNot(It.IsAny<User>())).Returns(expectedResult);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.IsUserEmailLockedOrNot(userInput);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }


        /// <summary>
        ///A nagative test for IsUserEmailLockedOrNot
        ///</summary>
        [TestMethod]
        public void IsUserEmailLockedOrNotUnitTestForNull()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Output
            const int expectedResult = 0;

            //Mock Setup
            mockAccountRepository.Setup(f => f.IsUserEmailLockedOrNot(It.IsAny<User>())).Returns(null);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.IsUserEmailLockedOrNot(null);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        ///A test for AddQuestionAnswerAndPassword
        ///</summary>
        [TestMethod]
        public void AddQuestionAnswerAndPasswordUnitTest()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Input
            User userInput = new User
            {
                UserId = 12,
                PasswordHash = "3wY6S+Xby4kZISipvj7LwIsJvbMEci88F6MX6RIEbryvSEjp5vnUSbR8DTuokIH6dSUyqMJdMy5tYwsjKLIYtw==",
                PasswordSalt = "ji0JeTBJIzicgjF52UKWVg/cs/y52v+CsBdIpYUnBvk=",
                EmailType = 1,
                UserSecurityQuestion = new List<SecurityQuestion>
                {
                    new SecurityQuestion{QuestionId =1,Question ="In what city does your nearest sibling live?",Answer = "1245"},
                    new SecurityQuestion{QuestionId =2,Question ="In what city or town did your mother and father meet?",Answer = "1245"},
                    new SecurityQuestion{QuestionId =3,Question ="In what city or town was your first job?",Answer = "1245"}  
                }
            };

            // Mock Output
            const int expectedResult = 0;

            //Mock Setup
            mockAccountRepository.Setup(f => f.AddQuestionAnswerAndPassword(It.IsAny<User>())).Returns(expectedResult);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.AddQuestionAnswerAndPassword(userInput);

            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        ///A test for AddQuestionAnswerAndPassword
        ///</summary>
        [TestMethod]
        public void AddQuestionAnswerAndPasswordUnitTestForNull()
        {
            // Arrange
            Mock<IAccountActivationRepository> mockAccountRepository = new Mock<IAccountActivationRepository>();

            // Mock Output
            const int expectedResult = 0;

            //Mock Setup
            mockAccountRepository.Setup(f => f.AddQuestionAnswerAndPassword(It.IsAny<User>())).Returns(expectedResult);
            _target = new AccountActivationLogic(mockAccountRepository.Object);

            // Act
            int actual = _target.AddQuestionAnswerAndPassword(null);

            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

    }
}
