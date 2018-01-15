/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Contract Notes Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ContractNotesLogicUnitTest
    /// </summary>
    [TestClass]
    public class ContractNotesLogicUnitTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        //public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Contracts the notes logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractNotesLogicConstructorTest()
        {
            var target = new ContractNoteLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractNoteLogic));
        }

        /// <summary>
        /// Contracts the notes logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractNotesLogicConstructorTest1()
        {
            var mockAddEditContractNote = new Mock<IContractNoteRepository>();
            ContractNoteLogic target = new ContractNoteLogic(mockAddEditContractNote.Object);
            Assert.IsInstanceOfType(target, typeof(ContractNoteLogic));
        }
        /// <summary>
        /// Adds contract note test for null
        /// </summary>
        [TestMethod]
        public void AddEditContractNoteIfNull()
        {
            var mockAddEditContractNote = new Mock<IContractNoteRepository>();
            mockAddEditContractNote.Setup(f => f.AddEditContractNote(It.IsAny<ContractNote>())).Returns(new ContractNote {ContractNoteId = 1});
            ContractNoteLogic target = new ContractNoteLogic(mockAddEditContractNote.Object);

            ContractNote actual = target.AddEditContractNote(null);
            Assert.IsNotNull(actual);
        }
        /// <summary>
        /// Adds  contract note  test. for not null
        /// </summary>
        [TestMethod]
        public void AddEditContractNoteIfNotNull()
        {
            var mockAddEditContractNote = new Mock<IContractNoteRepository>();
            mockAddEditContractNote.Setup(f => f.AddEditContractNote(It.IsAny<ContractNote>())).Returns(new ContractNote { ContractNoteId = 1});
            ContractNoteLogic target = new ContractNoteLogic(mockAddEditContractNote.Object);
            ContractNote objAddEditContractNote = new ContractNote { ContractNoteId = 1 };

            ContractNote actual = target.AddEditContractNote(objAddEditContractNote);
            Assert.AreEqual(1, actual.ContractNoteId);
        }
        /// <summary>
        /// Deletes the contract note by unique identifier mock test1.
        /// </summary>
        [TestMethod]
        public void DeleteContractNoteByIdMockTest1()
        {
            var mockDeleteContractNoteById = new Mock<IContractNoteRepository>();
            ContractNote objDeleteContractNotes = new ContractNote { ContractNoteId = 1,UserName = "Admin2"};
            mockDeleteContractNoteById.Setup(f => f.DeleteContractNote(objDeleteContractNotes)).Returns(true);
            ContractNoteLogic target = new ContractNoteLogic(mockDeleteContractNoteById.Object);

            bool actual = target.DeleteContractNote(objDeleteContractNotes);
            Assert.AreEqual(true, actual);
        }
    }
}
