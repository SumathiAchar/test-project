using System;
using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class UserLogicTest
    {
        private static UserLogic _target;

        /// <summary>
        /// UserLogic without Parameter Constructor unit test.
        /// </summary>
        [TestMethod]
        public void UserLogicConstructorTest()
        {
            var target = new UserLogic(null);
            //Assert
            Assert.IsInstanceOfType(target, typeof(UserLogic));
        }

        /// <summary>
        /// UserLogic Parameterized Constructor unit test.
        /// </summary>
        [TestMethod]
        public void UserLogicParameterizedConstructorTest()
        {
            var mockfacilityDetailsRepository = new Mock<IUserRepository>();
            UserLogic target = new UserLogic(mockfacilityDetailsRepository.Object);
            //Assert
            Assert.IsInstanceOfType(target, typeof(UserLogic));
        }

        /// <summary>
        /// Gets the All User List unit test.
        /// </summary>
        [TestMethod]
        public void GetAllUser()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                FacilityId = 1
            };
            List<User> expectedResult = new List<User>
            {
                new User
                {
                    UserId = 17,
                    UserName = "123@emids.com",
                    FirstName = "jani",
                    LastName = "m",
                    IsLocked = true
                }
            };
            mockUserRepository.Setup(f => f.GetAllUserModels(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);

            //Act
            List<User> actual = _target.GetAllUserModels(user);

            // Assert
            Assert.AreEqual(expectedResult.Count, actual.Count);
            Assert.AreEqual(expectedResult[0].UserId, actual[0].UserId);
            Assert.AreEqual(expectedResult[0].UserName, actual[0].UserName);
            Assert.AreEqual(expectedResult[0].FirstName, actual[0].FirstName);
            Assert.AreEqual(expectedResult[0].LastName, actual[0].LastName);
        }

        /// <summary>
        /// Gets the All User List with Negative FacilityId unit test.
        /// </summary>
        [TestMethod]
        public void GetAllUserWithNegativeFacilityId()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                FacilityId = -1
            };
            List<User> expectedResult = new List<User>
            {
                new User
                {
                    UserId = 0,
                    UserName = " ",
                    FirstName = " ",
                    LastName = " ",
                    IsLocked = false
                }
            };
            mockUserRepository.Setup(f => f.GetAllUserModels(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            List<User> actual = _target.GetAllUserModels(user);
            // Assert
            Assert.AreEqual(expectedResult[0].UserId, actual[0].UserId);
            Assert.AreEqual(expectedResult[0].UserName, actual[0].UserName);
            Assert.AreEqual(expectedResult[0].FirstName, actual[0].FirstName);
            Assert.AreEqual(expectedResult[0].LastName, actual[0].LastName);
        }



        /// <summary>
        /// Gets the All User List with Null FacilityId unit test.
        /// </summary>
        [TestMethod]
        public void GetUserById()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                LoggedInUserId = 0,
                UserId = 0
            };
            List<Facility> facilty = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User expectedResult = new User
            {
                AvailableFacilityList = facilty
            };
            mockUserRepository.Setup(f => f.GetUserById(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.GetUserById(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
        }


        /// <summary>
        /// Gets the All User List with LoggedIn UserId 0 and UserId=1 unit test.
        /// </summary>
        [TestMethod]
        public void GetUserByIdWithUserId()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                LoggedInUserId = 0,
                UserId = 1
            };
            List<Facility> availableFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User expectedResult = new User
            {
                UserId = 1,
                UserName = "ssiadmin@ssi.com",
                FirstName = "ssi",
                LastName = "admin",
                MiddleName = "",
                IsLocked = false,
                AvailableFacilityList = availableFacilityList,
                SelectedFacilityList = selectedFacilityList,


            };
            mockUserRepository.Setup(f => f.GetUserById(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.GetUserById(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
            Assert.AreEqual(expectedResult.AvailableFacilityList, actual.AvailableFacilityList);
            Assert.AreEqual(expectedResult.SelectedFacilityList, actual.SelectedFacilityList);
        }

        /// <summary>
        /// Gets the All User List with LoggedIn UserId -1 and UserId=-1 unit test.
        /// </summary>
        [TestMethod]
        public void GetUserByIdWithNegativeLoggedInUserId()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                LoggedInUserId = -1,
                UserId = -1
            };
            List<Facility> availableFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 0,
                    FacilityName = " "
                }
            };
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 0,
                    FacilityName = " "
                }
            };
            User expectedResult = new User
            {
                UserId = 0,
                UserName = "",
                FirstName = "",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                AvailableFacilityList = availableFacilityList,
                SelectedFacilityList = selectedFacilityList,


            };
            mockUserRepository.Setup(f => f.GetUserById(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.GetUserById(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
            Assert.AreEqual(expectedResult.AvailableFacilityList, actual.AvailableFacilityList);
            Assert.AreEqual(expectedResult.SelectedFacilityList, actual.SelectedFacilityList);
        }

        /// <summary>
        /// Add User unit test.
        /// </summary>
        [TestMethod]
        public void AddUser()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 62,
                    FacilityName = "test facility"
                }
            };
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid(),
                UserName = "123@test.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = 1
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Add User SelectedFacilityList IsNull unit test.
        /// </summary>
        [TestMethod]
        public void AddUserSelectedFacilityListIsNull()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 0,
                    FacilityName = ""
                }
            };
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid(),
                UserName = "123@test.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacility = "0",
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = 0
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Add User DomainName Validation unit test.
        /// </summary>
        [TestMethod]
        public void AddUserDomainNameValidation()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 62,
                    FacilityName = "test facility"
                }
            };
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid(),
                UserName = "test@gmail.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = -2
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Add User UserName ExistOrNot Validation
        /// </summary>
        [TestMethod]
        public void AddUserUserNameExistOrNotValidation()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 62,
                    FacilityName = "test facility"
                }
            };
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid(),
                UserName = "test@test.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = -1
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Edit User unit test.
        /// </summary>
        [TestMethod]
        public void EditUser()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User user = new User
            {
                UserId = 16,
                UserGuid = new Guid(),
                UserName = "test@emids.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = 1
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Edit User CmUserIdIsZero unit test.
        /// </summary>
        [TestMethod]
        public void EditUserCmUserIdIsZero()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid(),
                UserName = "test@emids.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacility = "0",
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = -1
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Edit User SelectedFacilityList IsNull unit test.
        /// </summary>
        [TestMethod]
        public void EditUserSelectedFacilityListIsNull()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 0,
                    FacilityName = ""
                }
            };
            User user = new User
            {
                UserId = 16,
                UserGuid = new Guid(),
                UserName = "test@emids.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacility = "0",
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = 0
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Edit User DomainName Validation unit test.
        /// </summary>
        [TestMethod]
        public void EditUserDomainNameValidation()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User user = new User
            {
                UserId = 16,
                UserGuid = new Guid(),
                UserName = "test@emids.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = -2
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Edit User UserName ExistOrNot Validation unit test.
        /// </summary>
        [TestMethod]
        public void EditUserUserNameExistOrNotValidation()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            List<Facility> selectedFacilityList = new List<Facility>
            {
                new Facility
                {
                    FacilityId = 1,
                    FacilityName = "Emids"
                }
            };
            User user = new User
            {
                UserId = 16,
                UserGuid = new Guid(),
                UserName = "test@emids.com",
                FirstName = "T",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 2,
                SelectedFacilityList = selectedFacilityList
            };
            User expectedResult = new User
            {
                UserId = -1
            };
            mockUserRepository.Setup(f => f.AddEditUser(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.AddEditUser(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
        }

        /// <summary>
        /// Delete User unit test.
        /// </summary>
        [TestMethod]
        public void DeleteUser()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 16,

            };
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.DeleteUser(user);
            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Delete User UserId Is Zero unit test.
        /// </summary>
        [TestMethod]
        public void DeleteUserUserIdIsZero()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 0,

            };
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.DeleteUser(user);
            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Delete User Negative UserId unit test.
        /// </summary>
        [TestMethod]
        public void DeleteUserNegativeUserId()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = -1,

            };
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.DeleteUser(user);
            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Get User Details unit test.
        /// </summary>
        [TestMethod]
        public void GetUserDetails()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                Email = "test@emids.com"
            };

            User expectedResult = new User
            {
                UserId = 16,
                UserName = "test@emids.com",
                FirstName = "test",
                LastName = "",
                MiddleName = "",
                IsLocked = true,
                UserTypeId = 2,

            };
            mockUserRepository.Setup(f => f.GetUserDetails(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.GetUserDetails(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
        }

        /// <summary>
        /// Get User Details Email Is Null unit test.
        /// </summary>
        [TestMethod]
        public void GetUserDetailsEmailIsNull()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                Email = " "
            };

            User expectedResult = new User
            {
                UserId = 0,
                UserName = "",
                FirstName = "",
                LastName = "",
                MiddleName = "",
                IsLocked = false,
                UserTypeId = 0,

            };
            mockUserRepository.Setup(f => f.GetUserDetails(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.GetUserDetails(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
        }

        /// <summary>
        /// Save User AccountSetting unit test.
        /// </summary>
        [TestMethod]
        public void SaveUserAccountSetting()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 1,
                UserGuid = new Guid("26C053B3-CE05-44D5-B26C-BAE5BA318B21"),
                RequestedUserName = "ssiadmin@ssi.com"
            };
            User expectedResult = new User
            {
                FirstName = "ssi",
                LastName = "admin",
                UserName = "ssiadmin@ssi.com"

            };
            mockUserRepository.Setup(f => f.SaveUserAccountSetting(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.SaveUserAccountSetting(user);
            // Assert
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
        }

        /// <summary>
        /// Save User AccountSetting UserId Is Zero unit test.
        /// </summary>
        [TestMethod]
        public void SaveUserAccountSettingUserIdIsZero()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid("26C053B3-CE05-44D5-B26C-BAE5BA318B21"),
                RequestedUserName = "ssiadmin@ssi.com"
            };
            User expectedResult = new User
            {
                FirstName = "",
                LastName = "",
                UserName = ""

            };
            mockUserRepository.Setup(f => f.SaveUserAccountSetting(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.SaveUserAccountSetting(user);
            // Assert
            Assert.AreEqual(expectedResult.FirstName, actual.FirstName);
            Assert.AreEqual(expectedResult.LastName, actual.LastName);
            Assert.AreEqual(expectedResult.UserName, actual.UserName);
        }

        /// <summary>
        /// Save EmailLog unit test.
        /// </summary>
        [TestMethod]
        public void SaveEmailLog()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 1,
                UserGuid = new Guid("26C053B3-CE05-44D5-B26C-BAE5BA318B21"),
                EmailType = 1,
                RequestedUserName = "ssiadmin@ssi.com"
            };
            mockUserRepository.Setup(f => f.SaveEmailLog(user)).Returns(true);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.SaveEmailLog(user);
            // Assert
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Save EmailLog UserId Is Zero unit test.
        /// </summary>
        [TestMethod]
        public void SaveEmailLogUserIdIsZero()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 0,
                UserGuid = new Guid("26C053B3-CE05-44D5-B26C-BAE5BA318B21"),
                EmailType = 1,
                RequestedUserName = "ssiadmin@ssi.com"
            };
            mockUserRepository.Setup(f => f.SaveEmailLog(user)).Returns(false);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.SaveEmailLog(user);
            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Validate Token unit test.
        /// </summary>
        [TestMethod]
        public void ValidateToken()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserGuid = new Guid("26C053B3-CE05-44D5-B26C-BAE5BA318B21")
            };
            User expectedResult = new User
            {
                UserId = 1,
                EmailType = 1,

            };
            mockUserRepository.Setup(f => f.ValidateToken(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.ValidateToken(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.EmailType, actual.EmailType);
        }

        /// <summary>
        /// ValidateToken Default Guid unit test.
        /// </summary>
        [TestMethod]
        public void ValidateTokenDefaultGuid()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserGuid = Guid.Empty
            };
            User expectedResult = new User
            {
                UserId = -1,
                EmailType = 0,

            };
            mockUserRepository.Setup(f => f.ValidateToken(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            User actual = _target.ValidateToken(user);
            // Assert
            Assert.AreEqual(expectedResult.UserId, actual.UserId);
            Assert.AreEqual(expectedResult.EmailType, actual.EmailType);
        }

        /// <summary>
        /// Update UserLogin unit test.
        /// </summary>
        [TestMethod]
        public void UpdateUserLogin()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 1,
                ValidLogin = true,
                RequestedUserName = "ssiadmin@ssi.com"
            };
            mockUserRepository.Setup(f => f.UpdateUserLogin(user)).Returns(true);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.UpdateUserLogin(user);
            // Assert
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        /// Update UserLogin UserId Is Zero unit test.
        /// </summary>
        [TestMethod]
        public void UpdateUserLoginUserIdIsZero()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 0,
                ValidLogin = true,
                RequestedUserName = "ssiadmin@ssi.com"
            };
            mockUserRepository.Setup(f => f.UpdateUserLogin(user)).Returns(false);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.UpdateUserLogin(user);
            // Assert
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        /// Save Audit Log unit test.
        /// </summary>
        [TestMethod]
        public void SaveAuditLog()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            AuditLogReport auditlog = new AuditLogReport
            {
                RequestedUserName = "ssiadmin@ssi.com",
                Description = "User logged in"
            };

            const int expectedResult = 0;
            mockUserRepository.Setup(f => f.SaveAuditLog(auditlog)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            int actual = _target.SaveAuditLog(auditlog);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save AuditLog RequestName Is Null.
        /// </summary>
        [TestMethod]
        public void SaveAuditLogRequestNameIsNull()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            AuditLogReport auditlog = new AuditLogReport
            {
                RequestedUserName = " ",
                Description = "User logged in"
            };

            const int expectedResult = 0;
            mockUserRepository.Setup(f => f.SaveAuditLog(auditlog)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            int actual = _target.SaveAuditLog(auditlog);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save AuditLog Description Is Null unit test.
        /// </summary>
        [TestMethod]
        public void SaveAuditLogDescriptionIsNull()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            AuditLogReport auditlog = new AuditLogReport
            {
                RequestedUserName = "ssiadmin@ssi.com",
                Description = " "
            };

            const int expectedResult = 0;
            mockUserRepository.Setup(f => f.SaveAuditLog(auditlog)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            int actual = _target.SaveAuditLog(auditlog);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save User Setting unit test.
        /// </summary>
        [TestMethod]
        public void GetUserLandingPageTest()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            int userId = 1;
            const byte expectedResult = 2;
            mockUserRepository.Setup(f => f.GetUserLandingPage(userId)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            byte actual = _target.GetUserLandingPage(userId);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save User Setting with UserId Zero unit test.
        /// </summary>
        [TestMethod]
        public void GetUserSettingWithUserIdIsZeroTest()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            int userId = 0;
            const byte expectedResult = 0;
            mockUserRepository.Setup(f => f.GetUserLandingPage(userId)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            byte actual = _target.GetUserLandingPage(userId);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save User Setting unit test.
        /// </summary>
        [TestMethod]
        public void SaveUserLandingPageTest()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            User user = new User
            {
                UserId = 1,
                LandingPageId = 2
            };
            const bool expectedResult = true;
            mockUserRepository.Setup(f => f.SaveUserLandingPage(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.SaveUserLandingPage(user);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save User Setting with UserId Zero unit test.
        /// </summary>
        [TestMethod]
        public void SaveUserSettingWithUserIdIsZeroTest()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            // Arrange
            User user = new User
            {
                UserId = 0,
                LandingPageId = 1
            };
            const bool expectedResult = false;
            mockUserRepository.Setup(f => f.SaveUserLandingPage(user)).Returns(expectedResult);
            _target = new UserLogic(mockUserRepository.Object);
            //Act
            bool actual = _target.SaveUserLandingPage(user);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

    }
}
