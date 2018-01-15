using System;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for SelectClaimsLogicTest and is intended
    ///to contain all SelectClaimsLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ClaimSelectorLogicTest
    {
        private static ClaimSelectorLogic _target;

        /// <summary>
        /// Selects the claims logic parameterless constructor test.
        /// </summary>
        [TestMethod]
        public void SelectClaimsLogicParameterlessConstructorTest()
        {
            _target = new ClaimSelectorLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(_target, typeof(ClaimSelectorLogic));
        }

        /// <summary>
        ///A test for SelectClaimsLogic Constructor
        ///</summary>
        [TestMethod]
        public void SelectClaimsLogicConstructorParmeterTest1()
        {
            var mockSelectClaimsLogic = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimsLogic.Object, mockAdjudicationEngine.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimSelectorLogic));

        }

        /// <summary>
        ///A test for SelectClaimsLogic Constructor
        ///</summary>
        [TestMethod]
        public void SelectClaimsLogicConstructorTest1()
        {
            IClaimSelectorRepository selectClaimsRepository = new ClaimSelectorRepository(Constants.ConnectionString);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(selectClaimsRepository, mockAdjudicationEngine.Object);
            Assert.IsInstanceOfType(target, typeof(ClaimSelectorLogic));
        }

        /// <summary>
        ///A test for AddEditSelectClaims
        ///</summary>
        [TestMethod]
        public void AddEditSelectClaimsTest()
        {
            IClaimSelectorRepository selectClaimsRepository = new ClaimSelectorRepository(Constants.ConnectionString);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(selectClaimsRepository, mockAdjudicationEngine.Object);
            const long expected = 0;
            long actual = target.AddEditSelectClaims(null);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetSelectedClaimList
        ///</summary>
        [TestMethod]
        public void GetSelectedClaimListTest()
        {
            IClaimSelectorRepository selectClaimsRepository = new ClaimSelectorRepository(Constants.ConnectionString);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(selectClaimsRepository, mockAdjudicationEngine.Object);
            const long expected = 0;
            long actual = target.GetSelectedClaimList(null);
            Assert.AreEqual(expected, actual);

        }

      
        
        [TestMethod]
        public void GetSsiNumberForBackgroundAjudicationReturnsIntegers()
        {
            List<int> expected = new List<int> {1, 2, 3};
            var repository = new Mock<IClaimSelectorRepository>();
            repository.Setup(m => m.GetSsiNumberForBackgroundAjudication()).Returns(() => expected);
            var resultList = repository.Object.GetSsiNumberForBackgroundAjudication();
            CollectionAssert.AreEquivalent(expected, resultList);
        }

        [TestMethod]
        public void CheckAdjudicationRequestNameExistReturnsTrue()
        {
            var repository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            repository.Setup(x => x.CheckAdjudicationRequestNameExist(It.IsAny<ClaimSelector>())).Returns(true);
            bool isSuccess = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object).CheckAdjudicationRequestNameExist(new ClaimSelector());
            Assert.IsTrue(isSuccess);
        }

        //[TestMethod]
        //public void GetAdjudicationRequestNamesTest()
        //{
        //    //Arrange
        //    var inputClaimSelector = new ClaimSelector();
        //    var mockSelectClaimsRepository = new Mock<IClaimSelectorRepository>();
        //    var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
        //    var resultClaimSelectorList = new List<ClaimSelector> { new ClaimSelector { ModelId = 11985 ,UserName = "jay"} };
        //    mockSelectClaimsRepository.Setup(m => m.GetAdjudicationRequestNames(inputClaimSelector))
        //    .Returns(resultClaimSelectorList);

        //    //Act
        //    var target = new ClaimSelectorLogic(mockSelectClaimsRepository.Object, mockAdjudicationEngine.Object);
        //    var actual = target.GetAdjudicationRequestNames(inputClaimSelector);

        //    //Assert
        //    CollectionAssert.AreEqual(resultClaimSelectorList, actual);
        //}

        [TestMethod]
        public void GetSsiNumberForBackgroundAjudicationTest()
        {
            var mockSelectClaimsRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            List<int> result = new List<int>{1, 2, 3};
            mockSelectClaimsRepository.Setup(f => f.GetSsiNumberForBackgroundAjudication()).Returns(result);
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimsRepository.Object, mockAdjudicationEngine.Object );
            var actual = target.GetSsiNumberForBackgroundAjudication();
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        public void GetBackgroundAdjudicationTaskTest()
        {
            var mockSelectClaimsRepository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            const int facilityid = 1;
            const int batchSizeForBackgroundAdjudication = 5;
            const int result = 5;
            const int timeOut = 100;
            mockSelectClaimsRepository.Setup(
                f => f.GetBackgroundAdjudicationTask(facilityid, batchSizeForBackgroundAdjudication, timeOut))
                .Returns(result);
            ClaimSelectorLogic target = new ClaimSelectorLogic(mockSelectClaimsRepository.Object,
                mockAdjudicationEngine.Object);
            long actual =
                target.GetBackgroundAdjudicationTask(facilityid, batchSizeForBackgroundAdjudication, timeOut);
            Assert.AreEqual(result, actual);
        }


        [TestMethod]
        public void ReviewClaimTest()
        {
            //Arrange
            ClaimsReviewed claimsReviewed =new ClaimsReviewed();
            List<ClaimsReviewed> claimsRevieweds = new List<ClaimsReviewed> {claimsReviewed};

            var repository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            repository.Setup(x => x.ReviewClaim(It.IsAny<IEnumerable<ClaimsReviewed>>())).Returns(true);

            //Act
            bool isSuccess = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object).ReviewClaim(claimsRevieweds.AsEnumerable());

            //Assert
            Assert.IsTrue(isSuccess);
        }

        [TestMethod]
        public void ReviewClaimIfReviewClaimIsNullTest()
        {
            var repository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            repository.Setup(x => x.ReviewClaim(null)).Returns(false);

            bool isSuccess = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object).ReviewClaim(null);
            Assert.IsFalse(isSuccess);
        }

        [TestMethod]
        public void ReviewedAllClaimsTest()
        { 
            //Arrange

            var repository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            repository.Setup(x => x.ReviewedAllClaims(It.IsAny<SelectionCriteria>())).Returns(true);
            bool isSuccess = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object).ReviewedAllClaims(new SelectionCriteria());
            Assert.IsTrue(isSuccess);
        }

        /// <summary>
        /// Revieweds all claims if criteria is null test.
        /// </summary>
        [TestMethod]
        public void ReviewedAllClaimsIfCriteriaIsNullTest()
        {  
            //Arrange

            var repository = new Mock<IClaimSelectorRepository>();
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            repository.Setup(x => x.ReviewedAllClaims(null)).Returns(false);
            bool isSuccess = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object).ReviewedAllClaims(new SelectionCriteria());
            Assert.IsFalse(isSuccess);
        }

        /// <summary>
        /// Adds the claim note test.
        /// </summary>
        [TestMethod]
        public void AddClaimNoteTest()
        {
            var repository = new Mock<IClaimSelectorRepository>();
            ClaimNote claimNoteInput = new ClaimNote
            {
                ClaimId = 421569877,
                ClaimNoteText = "Test Claim Note",
                CurrentDateTime = DateTime.Now.ToLongDateString()
            };
            repository.Setup(x => x.AddClaimNote(claimNoteInput)).Returns(claimNoteInput);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object);
            var result = target.AddClaimNote(claimNoteInput);
            Assert.AreEqual(result.ClaimId, claimNoteInput.ClaimId);
        }

        /// <summary>
        /// Adds the claim note null test.
        /// </summary>
        [TestMethod]
        public void AddClaimNoteNullTest()
        {
            var repository = new Mock<IClaimSelectorRepository>();
           repository.Setup(x => x.AddClaimNote(null));
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object);
            var result = target.AddClaimNote(null);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Deletes the claim note test.
        /// </summary>
        [TestMethod]
        public void DeleteClaimNoteTest()
        {
            var repository = new Mock<IClaimSelectorRepository>();
            ClaimNote claimNoteInput = new ClaimNote
            {
                ClaimId = 421569877,
                ClaimNoteText = "Test Claim Note",
                CurrentDateTime = DateTime.Now.ToLongDateString()
            };
            repository.Setup(x => x.DeleteClaimNote(claimNoteInput)).Returns(true);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object);
            var result = target.DeleteClaimNote(claimNoteInput);
            Assert.IsTrue(result);
        }


        /// <summary>
        /// Gets the claim notes test.
        /// </summary>
        [TestMethod]
        public void GetClaimNotesTest()
        {
            var repository = new Mock<IClaimSelectorRepository>();
            ClaimNotesContainer claimNoteContainerInput = new ClaimNotesContainer
            {
                ClaimId = 421569877,
                UserName = "Ssi@demo.com",
                FacilityName = "Baxter"
            };
            ClaimNotesContainer claimNoteContainerOutput = new ClaimNotesContainer
            {
                ClaimId = 421569877,
                UserName = "Ssi@demo.com",
                FacilityName = "Baxter",
                ClaimNotes = new List<ClaimNote>
                {
                    new ClaimNote
                    {
                       ClaimId = 421569877,
                       ClaimNoteText = "hi there",
                       ShortDateTime = DateTime.Now.ToShortDateString()
                    },
                    new ClaimNote
                    {
                       ClaimId = 421569877,
                       ClaimNoteText = "sggfdgdfgdfgdfgdf",
                       ShortDateTime = DateTime.Now.ToShortDateString()
                    }
                }
            };

            repository.Setup(x => x.GetClaimNotes(claimNoteContainerInput)).Returns(claimNoteContainerOutput);
            var mockAdjudicationEngine = new Mock<IAdjudicationEngine>();
            ClaimSelectorLogic target = new ClaimSelectorLogic(repository.Object, mockAdjudicationEngine.Object);
            var result = target.GetClaimNotes(claimNoteContainerInput);
            Assert.AreEqual(result.ClaimNotes.Count, claimNoteContainerOutput.ClaimNotes.Count);
            Assert.AreEqual(result.ClaimId, claimNoteContainerOutput.ClaimId);
        }
    }
}
