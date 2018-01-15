using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for SelectClaimsLogicTest and is intended
    ///to contain all SelectClaimsLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ClaimSelectorUnitTest
    {
        /// <summary>
        ///A test for SelectClaimsLogic Constructor
        ///</summary>
        [TestMethod]
        public void SelectClaimsLogicConstructorTest()
        {
            IClaimSelectorRepository selectClaimsRepository = new ClaimSelectorRepository(Constants.ConnectionString);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(selectClaimsRepository, mockAdjudicationEngine.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimSelectorLogic));
        }

        /// <summary>
        ///A test for SelectClaimsLogic Constructor
        ///</summary>
        [TestMethod]
        public void SelectClaimsLogicConstructorTest1()
        {

            Mock<IClaimSelectorRepository> mockSelectClaimRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimRepository.Object, mockAdjudicationEngine.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimSelectorLogic));
        }

        /// <summary>
        ///A test for AddEditSelectClaims
        ///</summary>
        [TestMethod]
        public void AddEditSelectClaimsTest()
        {
            Mock<IClaimSelectorRepository> mockSelectClaimRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimRepository.Object, mockAdjudicationEngine.Object);
            const long expected = 0;
            long actual = target.AddEditSelectClaims(null);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for AddEditSelectClaims
        ///</summary>
        [TestMethod]
        public void AddEditSelectClaimsMockTest()
        {


            //Mock input
            ClaimSelector selectClaims = new ClaimSelector
            {
                ModelId = 1,
                DateType = 1,
                RequestName = "TestCase",
                FacilityId = 101
            };
            //Mock output
            const long expected = 1458;
            //Mock setup
            Mock<IClaimSelectorRepository> mockAddEditSelectClaimsMockTest = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            mockAddEditSelectClaimsMockTest.Setup(f => f.AddEditSelectClaims(It.IsAny<ClaimSelector>())).Returns(expected);
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockAddEditSelectClaimsMockTest.Object, mockAdjudicationEngine.Object);
            long actual = target.AddEditSelectClaims(selectClaims);
            Assert.AreEqual(expected, actual);

        }
      
        [TestMethod]
        public void GetSsiNumberForBackgroundAjudication()
        {
            Mock<IClaimSelectorRepository> selectClaimRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();

            selectClaimRepository.Setup(x => x.GetSsiNumberForBackgroundAjudication())
                .Returns(new List<int>());

            new ClaimSelectorLogic(selectClaimRepository.Object, mockAdjudicationEngine.Object).GetSsiNumberForBackgroundAjudication();
            selectClaimRepository.VerifyAll();
        }

        /// <summary>
        ///A test for GetSelectedClaimList
        ///</summary>
        [TestMethod]
        public void GetSelectedClaimListTest()
        {
            Mock<IClaimSelectorRepository> mockSelectClaimRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimRepository.Object, mockAdjudicationEngine.Object);
            const long expected = 0;
            long actual = target.GetSelectedClaimList(null);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        ///A test for GetSelectedClaimList
        ///</summary>
        [TestMethod]
        public void GetSelectedClaimListMockTest()
        {
            //Mock input
            ClaimSelector selectClaims = new ClaimSelector
            {
                ModelId = 1,
                DateType = 1,
                RequestName = "TestCase",
                FacilityId = 101
            };
           //Mock setup
            Mock<IClaimSelectorRepository> mockGetSelectedClaimListMockTest = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();

            mockGetSelectedClaimListMockTest.Setup(f => f.GetClaimsCountForAdjudication(It.IsAny<ClaimSelector>())).Returns(1);
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockGetSelectedClaimListMockTest.Object,mockAdjudicationEngine.Object);
            long actual = target.GetSelectedClaimList(selectClaims);
            Assert.AreEqual(1, actual);

        }


       
    }
}
