using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for FacilityLogicTest and is intended
    ///to contain all FacilityLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class FacilityLogicUnitTest
    {
        private static FacilityLogic _target;
        /// <summary>
        ///A test for FacilityLogic Constructor
        ///</summary>
        [TestMethod]
        public void FacilityLogicConstructorTest()
        {
            _target = new FacilityLogic(null);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(FacilityLogic));
        }

        /// <summary>
        /// Facilities the details logic constructor test.
        /// </summary>
        [TestMethod]
        public void FacilityLogicParametrizedConstructorTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockfacilityRepository = new Mock<IFacilityRepository>();

            // Act
            _target = new FacilityLogic(mockfacilityRepository.Object);

            // Assert
            Assert.IsInstanceOfType(_target, typeof(FacilityLogic));
        }

        /// <summary>
        ///A test for AddEditFacilityUnitTest
        ///</summary>
        [TestMethod]
        public void AddFacilityUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();
            Facility facility = new Facility
            {
                FacilityId = 0,
                DisplayName = "test Facility",
                FacilityName = "test Facility",
                Address = "USA",
                City = "Alabama",
                StateId = "AL",
                Zip = "635205",
                Domains = "@ssigroup.com,@emids.com",
                Phone = "321-321-321",
                Fax = "321-654-987",
                IsActive = false,
                EarlyStatementDate = DateTime.Now,
                PasswordExpirationDays = 12,
                InvalidPasswordAttempts = 4,
                SsiNo = "321654,321654,9 87654",
                FacilityBubbleId = 1,
                FacilityFeatureControl = new List<FeatureControl>
                {
                    new FeatureControl { FeatureControlId = 1,IsSelected=true }, 
                    new FeatureControl { FeatureControlId = 2,IsSelected=true }
                }
            };
            const string expectedResult = "0";
            //Mock Setup
            mockFacilityRepository.Setup(f => f.SaveFacility(It.IsAny<Facility>())).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            string actual = _target.SaveFacility(facility);

            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        ///A test for AddFacilityRemoveSpaceFromSSiNumberUnitTest
        ///</summary>
        [TestMethod]
        public void AddFacilityPassDomainNameAsEmptyUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();
            Facility value = new Facility
            {
                FacilityId = 0,
                DisplayName = "test Facility",
                FacilityName = "test Facility",
                Address = "USA",
                City = "Alabama",
                StateId = "AL",
                Zip = "635205",
                Domains = string.Empty,
                Phone = "321-321-321",
                Fax = "321-654-987",
                IsActive = false,
                EarlyStatementDate = DateTime.Now,
                PasswordExpirationDays = 12,
                InvalidPasswordAttempts = 4,
                SsiNo = string.Empty,
                FacilityBubbleId = 1,
                FacilityFeatureControl = new List<FeatureControl>
                {
                    new FeatureControl { FeatureControlId = 1,IsSelected=true }, 
                    new FeatureControl { FeatureControlId = 2,IsSelected=true }
                }
            };
            const string expectedResult = "0";
            //Mock Setup
            mockFacilityRepository.Setup(f => f.SaveFacility(It.IsAny<Facility>())).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            string actual = _target.SaveFacility(value);

            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        ///A test for AddEditFacilityIfNullUnitTest
        ///</summary>
        [TestMethod]
        public void AddEditFacilityIfNullUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> repository = new Mock<IFacilityRepository>();

            //Mock Setup
            repository.Setup(f => f.SaveFacility(null));
            _target = new FacilityLogic(repository.Object);

            // Act
            string actual = _target.SaveFacility(null);

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for EditFacilityUnitTest
        ///</summary>
        [TestMethod]
        public void EditFacilityUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();
            Facility value = new Facility
            {
                FacilityId = 12,
                DisplayName = "Facility",
                FacilityName = "Facility",
                Address = "USA",
                City = "Alabama",
                StateId = "AL",
                Zip = "635205",
                Domains = "@ssigroup.com,@emids.com",
                Phone = "321-321-321",
                Fax = "321-654-987",
                IsActive = false,
                EarlyStatementDate = DateTime.Now,
                PasswordExpirationDays = 12,
                InvalidPasswordAttempts = 4,
                SsiNo = "321654,321654,9 87654",
                FacilityBubbleId = 1,
                FacilityFeatureControl = new List<FeatureControl>
                {
                    new FeatureControl { FeatureControlId = 1,IsSelected=true }, 
                    new FeatureControl { FeatureControlId = 2,IsSelected=true }
                }
            };
            const string expectedResult = "0";
            //Mock Setup
            mockFacilityRepository.Setup(f => f.SaveFacility(It.IsAny<Facility>())).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            string actual = _target.SaveFacility(value);

            // Assert
            Assert.AreEqual(actual, expectedResult);
        }

        /// <summary>
        ///A test for GetFacilityById
        ///</summary>
        [TestMethod]
        public void GetFacilityByIdUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            //Mock Input
            Facility facilityDetailsInput = new Facility
            {
                FacilityId = 12
            };

            //Mock Output
            Facility expectedResult = new Facility
            {
                FacilityId = 12,
                DisplayName = "Facility",
                FacilityName = "Facility",
                Address = "USA",
                City = "Alabama",
                StateId = "AL",
                Zip = "635205",
                Domains = "@ssigroup.com,@emids.com",
                Phone = "321-321-321",
                Fax = "321-654-987",
                IsActive = false,
                EarlyStatementDate = DateTime.Now,
                PasswordExpirationDays = 12,
                InvalidPasswordAttempts = 4,
                SsiNo = "321654,321654,9 87654",
                FacilityBubbleId = 1,
                FacilityFeatureControl = new List<FeatureControl>
                {
                    new FeatureControl { FeatureControlId = 1,IsSelected=true }, 
                    new FeatureControl { FeatureControlId = 2,IsSelected=true }
                }
            };

            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetFacilityById(It.IsAny<Facility>())).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            Facility actual = _target.GetFacilityById(facilityDetailsInput);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult.FacilityId, actual.FacilityId);
            Assert.AreEqual(expectedResult.FacilityFeatureControl[0].FeatureControlId, actual.FacilityFeatureControl[0].FeatureControlId);
            Assert.AreEqual(expectedResult.FacilityFeatureControl[0].IsSelected, actual.FacilityFeatureControl[0].IsSelected);
            Assert.AreEqual(expectedResult.DisplayName, actual.DisplayName);
            Assert.AreEqual(expectedResult.FacilityName, actual.FacilityName);
            Assert.AreEqual(expectedResult.Address, actual.Address);
        }

        /// <summary>
        /// A test for GetFacilityByIdUnitTestForNull
        /// </summary>
        [TestMethod]
        public void GetFacilityByIdUnitTestForNull()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityDetails = new Mock<IFacilityRepository>();
            mockFacilityDetails.Setup(f => f.GetFacilityById(It.IsAny<Facility>())).Returns((Facility)null);
            _target = new FacilityLogic(mockFacilityDetails.Object);

            // Act
            Facility actual = _target.GetFacilityById(null);

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for GetAllFacilitiesUnitTest
        ///</summary>
        [TestMethod]
        public void GetAllFacilitiesUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            //Mock Input
            FacilityContainer facilityContainerInput = new FacilityContainer
            {
                PageSetting = new PageSetting
                {
                    Skip = 0,
                    Take = 50
                },
                IsActive = true
            };

            //Mock Output
            FacilityContainer expectedResult = new FacilityContainer
            {
                Facilities = new List<Facility>
                {
                    new Facility
                    {
                        FacilityId = 12,
                        DisplayName = "Facility",
                        FacilityName = "Facility",
                        Address = "USA",
                        City = "Alabama",
                        StateId = "AL",
                        Zip = "635205",
                        Domains = "@ssigroup.com,@emids.com",
                        Phone = "321-321-321",
                        Fax = "321-654-987",
                        IsActive = false,
                        EarlyStatementDate = DateTime.Now,
                        PasswordExpirationDays = 12,
                        InvalidPasswordAttempts = 4,
                        SsiNo = "321654,321654,9 87654",
                        FacilityBubbleId = 1,
                        FacilityFeatureControl = new List<FeatureControl>
                        {
                            new FeatureControl {FeatureControlId = 1, IsSelected = true},
                            new FeatureControl {FeatureControlId = 2, IsSelected = true}
                        }
                    }
                }
            };

            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetAllFacilities(It.IsAny<FacilityContainer>())).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            FacilityContainer actual = _target.GetAllFacilities(facilityContainerInput);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult.Facilities[0].DisplayName, actual.Facilities[0].DisplayName);
            Assert.AreEqual(expectedResult.Facilities[0].FacilityName, actual.Facilities[0].FacilityName);
        }

        /// <summary>
        /// A test for GetAllFacilitiesUnitTestForNull
        /// </summary>
        [TestMethod]
        public void GetAllFacilitiesUnitTestForNull()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityDetails = new Mock<IFacilityRepository>();
            mockFacilityDetails.Setup(f => f.GetAllFacilities(It.IsAny<FacilityContainer>())).Returns((FacilityContainer)null);
            _target = new FacilityLogic(mockFacilityDetails.Object);

            // Act
            FacilityContainer actual = _target.GetAllFacilities(null);

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for DeleteFacilityUnitTest
        ///</summary>
        [TestMethod]
        public void DeleteFacilityUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();
            Facility value = new Facility
            {
                FacilityId = 12,
            };
            const bool result = true;
            //Mock Setup
            mockFacilityRepository.Setup(f => f.DeleteFacility(It.IsAny<Facility>())).Returns(result);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            bool actual = _target.DeleteFacility(value);

            // Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        ///A test for DeleteFacilityUnitTestForNull
        ///</summary>
        [TestMethod]
        public void DeleteFacilityUnitTestForNull()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();
            const bool result = false;
            //Mock Setup
            mockFacilityRepository.Setup(f => f.DeleteFacility(It.IsAny<Facility>())).Returns(result);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            bool actual = _target.DeleteFacility(null);

            // Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        ///A test for GetActiveStatesUnitTest
        ///</summary>
        [TestMethod]
        public void GetActiveStatesUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            List<string> expectedResult = new List<string>
            {
               "AK","AL","AR","AS"
            };
            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetActiveStates()).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<string> actual = _target.GetActiveStates();

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult[0], actual[0]);
        }

        /// <summary>
        ///A test for GetActiveStatesUnitTestForNull
        ///</summary>
        [TestMethod]
        public void GetActiveStatesUnitTestForNull()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetActiveStates()).Returns((List<string>)null);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<string> actual = _target.GetActiveStates();

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for GetActiveFacilityBubbleUnitTest
        ///</summary>
        [TestMethod]
        public void GetActiveFacilityBubbleUnitTest()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            List<FacilityBubble> expectedResult = new List<FacilityBubble>
            {
              new FacilityBubble{FacilityBubbleId = 1,Description = "Bubble1"},
              new FacilityBubble{FacilityBubbleId = 2,Description = "Bubble2"}
            };
            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetBubbles()).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<FacilityBubble> actual = _target.GetBubbles();

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult[0].Description, actual[0].Description);
            Assert.AreEqual(expectedResult[0].FacilityBubbleId, actual[0].FacilityBubbleId);
        }

        /// <summary>
        ///A test for GetActiveFacilityBubbleUnitTestForNull
        ///</summary>
        [TestMethod]
        public void GetActiveFacilityBubbleUnitTestForNull()
        {
            // Arrange
            Mock<IFacilityRepository> mockFacilityRepository = new Mock<IFacilityRepository>();

            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetBubbles()).Returns((List<FacilityBubble>)null);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<FacilityBubble> actual = _target.GetBubbles();

            // Assert
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Gets the All Facility List unit test.
        /// </summary>
        [TestMethod]
        public void GetAllFacility()
        {
            // Arrange
            var mockFacilityRepository = new Mock<IFacilityRepository>();
            User user = new User
            {
                UserId = 1
            };
            List<Facility> expectedResult = new List<Facility>
           {
              new Facility
              {
                  FacilityId = 1,
                  FacilityName = "Emids"
              }
           };
            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetAllFacilityList(user)).Returns(expectedResult);
            _target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<Facility> actual = _target.GetAllFacilityList(user);

            // Assert
            Assert.AreEqual(expectedResult[0].FacilityId, actual[0].FacilityId);
            Assert.AreEqual(expectedResult[0].FacilityName, actual[0].FacilityName);
        }

        /// <summary>
        /// Gets the All Facility List With Negative FacilityId unit test.
        /// </summary>.
        [TestMethod]
        public void GetAllFacilityWithNegativeCmUserId()
        {
            // Arrange
            var mockFacilityRepository = new Mock<IFacilityRepository>();
            User user = new User
            {
                UserId = -1
            };
            List<Facility> expectedResult = new List<Facility>
           {
              new Facility
              {
                  FacilityId = 0,
                  FacilityName = " "
              }
           };
            //Mock Setup
            mockFacilityRepository.Setup(f => f.GetAllFacilityList(user)).Returns(expectedResult);
            FacilityLogic target = new FacilityLogic(mockFacilityRepository.Object);

            // Act
            List<Facility> actual = target.GetAllFacilityList(user);

            // Assert
            Assert.AreEqual(expectedResult[0].FacilityId, actual[0].FacilityId);
            Assert.AreEqual(expectedResult[0].FacilityName, actual[0].FacilityName);
        }

        /// <summary>
        /// Gets the facility data source test.
        /// </summary>
        [TestMethod]
        public void GetFacilityDataSourceTest()
        {
            // Arrange
            var mockFacilityRepository = new Mock<IFacilityRepository>();
            var result = new List<Facility> { new Facility { FacilityId = 1, ConnectionString = "Bubble1ConnectionString" }, new Facility { FacilityId = 2, ConnectionString = "Bubble2ConnectionString" } };
            mockFacilityRepository.Setup(f => f.GetFacilitiesDataSource("Data Source=69.85.245.160;Initial Catalog=Bubble2;Persist Security Info=True;User ID=sa;Password=Cmsqldevu$3r"))
                .Returns(result);
            FacilityLogic target = new FacilityLogic(mockFacilityRepository.Object);
            // Act
            List<Facility> actual = target.GetFacilitiesDataSource("Data Source=69.85.245.160;Initial Catalog=Bubble2;Persist Security Info=True;User ID=sa;Password=Cmsqldevu$3r");
            // Assert
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        /// Gets the bubble connection string test.
        /// </summary>
        [TestMethod]
        public void GetBubbleConnectionStringTest()
        {
            // Arrange
            var mockFacilityRepository = new Mock<IFacilityRepository>();
            string expectedResult = "Data Source=69.85.245.160;Initial Catalog=Bubble1;Persist Security Info=True;User ID=sa;Password=Cmsqldevu$3r";
            mockFacilityRepository.Setup(f => f.GetBubbleConnectionString("Bubble 1"))
                .Returns(expectedResult);
            FacilityLogic target = new FacilityLogic(mockFacilityRepository.Object);
            // Act
            string actual = target.GetBubbleConnectionString("Bubble 1");
            // Assert
            Assert.AreEqual(actual, expectedResult);
        }
    }
}
