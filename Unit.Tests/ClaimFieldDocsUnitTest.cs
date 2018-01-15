using System.Collections.Generic;
using System.Linq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ClaimFieldDocsUnitTest
    /// </summary>
    [TestClass]
    public class ClaimFieldDocsUnitTest
    {
        //Creating mock object for ContractLogic
        private Mock<IClaimFieldDocRepository> _mockClaimFieldDocRepository;

        //Creating object for Logic
        private ClaimFieldDocLogic _target;

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
        /// Claims the field document logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ClaimFieldDocLogicConstructorUnitTest1()
        {
            _target = new ClaimFieldDocLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(_target, typeof(ClaimFieldDocLogic));
        }

        /// <summary>
        /// Claims the field document logic constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ClaimFieldDocLogicConstructorUnitTest2()
        {
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);
            Assert.IsInstanceOfType(_target, typeof(ClaimFieldDocLogic));
        }

        /// <summary>
        /// Adds the claim field docs difference null.
        /// </summary>
        [TestMethod]
        public void AddClaimFieldDocsIfNull()
        {
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            _mockClaimFieldDocRepository.Setup(f => f.AddClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(0);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);
            long actual = _target.AddClaimFieldDocs(null);
            Assert.AreEqual(0, actual);
        }


        /// <summary>
        /// Adds the claim field docs if not null.
        /// </summary>
        [TestMethod]
        public void AddClaimFieldDocsIfNotNull()
        {
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            _mockClaimFieldDocRepository.Setup(f => f.AddClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(1);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);
            var objAddClaimFieldDocs = new ClaimFieldDoc { ContractId = 1 };
            long actual = _target.AddClaimFieldDocs(objAddClaimFieldDocs);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Gets the table look up details by contract identifier if null.
        /// </summary>
        [TestMethod]
        public void GetTableLookUpDetailsByContractIdIfNull()
        {

            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            _mockClaimFieldDocRepository.Setup(f => f.GetClaimFieldDocs(null));
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(null);
            Assert.AreEqual(null, actual);
        }


        /// <summary>
        /// Gets all claim fields difference not null logic unit test.
        /// </summary>
        [TestMethod]
        public void GetAllClaimFieldsIfNotNullLogicUnitTest()
        {
            //Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var result = new List<ClaimField> { new ClaimField() };
            _mockClaimFieldDocRepository.Setup(f => f.GetAllClaimFields()).Returns(result);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimField> actual = _target.GetAllClaimFields();

            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        ///A test for GetClaimFieldDocs
        ///</summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfNull()
        {
            //Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var result = new List<ClaimFieldDoc>();
            _mockClaimFieldDocRepository.Setup(f => f.GetClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(result);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(null);

            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the claim field docs test if not null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfNotNull()
        {
            //Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc { ContractId = 8, ClaimFieldId = 5, ClaimFieldDocId = 10 };
            var result = new List<ClaimFieldDoc>();
            _mockClaimFieldDocRepository.Setup(f => f.GetClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(result);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets all claim fields test.
        /// </summary>
        [TestMethod]
        public void GetAllClaimFieldsTest()
        {
            //Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var result = new List<ClaimField>
            {
                new ClaimField {ClaimFieldId = 1, Text = "Test name 1", ClaimFieldDocId = 1, TableName = "Table 1"},
                new ClaimField {ClaimFieldId = 2, Text = "Test name 2", ClaimFieldDocId = 2, TableName = "Table 2"}
            };
            _mockClaimFieldDocRepository.Setup(f => f.GetAllClaimFields()).Returns(result);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimField> actual = _target.GetAllClaimFields();

            //Assert
            Assert.AreEqual(result, actual);
        }


        /// <summary>
        /// Adds the claim field docs test.
        /// </summary>
        [TestMethod]
        public void AddClaimFieldDocsTest()
        {
            //Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            const long result = 1;
            _mockClaimFieldDocRepository.Setup(f => f.AddClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(result);
            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            long actual = _target.AddClaimFieldDocs(null);

            //Assert
            Assert.AreEqual(result, actual);
        }


        /// <summary>
        /// Gets the claim field docs test if claim field document list is null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfClaimFieldDocListIsNull()
        {
            // Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc
            {
                ContractId = 237043,
                ClaimFieldId = 4,
                ClaimFieldDocId = 271326,
                NodeId = 0
            };
            _mockClaimFieldDocRepository.Setup(x => x.GetClaimFieldDocs(dataVal)).Returns((List<ClaimFieldDoc>)null);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            // Assert
            Assert.AreEqual(null, actual);
        }

        /// <summary>
        /// Gets the claim field docs test if claim field document list is not null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfClaimFieldDocListIsNotNull()
        {
            // Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc
            {
                ContractId = 237043,
                ClaimFieldId = 4,
                ClaimFieldDocId = 271326,
                NodeId = 0
            };
            var claimFieldDocs = new List<ClaimFieldDoc>
            {
                new ClaimFieldDoc
                {
                    ClaimFieldDocId = 1,
                    TableName = "Test 1",
                    ClaimFieldValues =
                        new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue {Identifier = "Test Identifier", Value = "Test Value"}
                        }
                }
            };
            _mockClaimFieldDocRepository.Setup(x => x.GetClaimFieldDocs(dataVal)).Returns(claimFieldDocs);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimFieldDocId != 0);
            if (firstOrDefault != null)
                Assert.AreEqual(claimFieldDocs[0].ClaimFieldDocId, firstOrDefault.ClaimFieldDocId);
        }

        /// <summary>
        /// Gets the claim field docs test if claim field values is null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfClaimFieldValuesIsNull()
        {
            // Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc
            {
                ContractId = 237043,
                ClaimFieldId = 4,
                ClaimFieldDocId = 271326,
                NodeId = 0
            };
            var claimFieldDocs = new List<ClaimFieldDoc>
            {
                new ClaimFieldDoc {ClaimFieldDocId = 1, TableName = "Test 1", ClaimFieldValues = null}
            };
            _mockClaimFieldDocRepository.Setup(x => x.GetClaimFieldDocs(dataVal)).Returns(claimFieldDocs);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimFieldDocId != 0);
            if (firstOrDefault != null)
                Assert.AreEqual(claimFieldDocs[0].TableName, firstOrDefault.TableName);
            Assert.AreEqual(actual.Count, claimFieldDocs.Count);
        }


        /// <summary>
        /// Gets the claim field docs test if claim field values is not null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfClaimFieldValuesIsNotNull()
        {
            // Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc
            {
                ContractId = 237043,
                ClaimFieldId = 4,
                ClaimFieldDocId = 271326,
                NodeId = 0
            };
            var claimFieldDocs = new List<ClaimFieldDoc>
            {
                new ClaimFieldDoc
                {
                    TableName = "Test 1",
                    ClaimFieldValues =
                        new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                ClaimFieldDocId = 1,
                                Identifier = "Test Identifier",
                                Value = "Test Value"
                            }
                        }
                },
                new ClaimFieldDoc
                {
                    TableName = "Test 2",
                    ClaimFieldValues =
                        new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue
                            {
                                ClaimFieldDocId = 12,
                                Identifier = "Test Identifier2",
                                Value = "Test Value2"
                            }
                        }
                }
            };
            _mockClaimFieldDocRepository.Setup(x => x.GetClaimFieldDocs(dataVal)).Returns(claimFieldDocs);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimFieldDocId != 0);
            if (firstOrDefault != null)
                Assert.AreEqual(claimFieldDocs[0].TableName, firstOrDefault.TableName);
            Assert.AreEqual(actual.Count, claimFieldDocs.Count);
        }

        /// <summary>
        /// Gets the claim field docs test if claim field document identifier is not null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldDocsTestIfClaimFieldDocIdIsNotNull()
        {
            // Arrange
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc
            {
                ContractId = 237043,
                ClaimFieldId = 4,
                ClaimFieldDocId = 271326,
                NodeId = 0
            };
            var claimFieldDocs = new List<ClaimFieldDoc>
            {
                new ClaimFieldDoc
                {
                    ClaimFieldDocId = 1,
                    TableName = "Test 1",
                    ClaimFieldValues =
                        new List<ClaimFieldValue>
                        {
                            new ClaimFieldValue {Identifier = "Test Identifier", Value = "Test Value"}
                        }
                }
            };
            _mockClaimFieldDocRepository.Setup(x => x.GetClaimFieldDocs(dataVal)).Returns(claimFieldDocs);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ClaimFieldDoc> actual = _target.GetClaimFieldDocs(dataVal);

            // Assert
            var firstOrDefault = actual.FirstOrDefault(q => q.ClaimFieldDocId != 0);
            if (firstOrDefault != null)
                Assert.AreEqual(claimFieldDocs[0].ClaimFieldDocId, firstOrDefault.ClaimFieldDocId);
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var result = new ClaimFieldDoc { ClaimFieldDocId = 271326 };
            _mockClaimFieldDocRepository.Setup(x => x.Delete(result)).Returns(true);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            bool actual = _target.Delete(result);

            // Assert
            Assert.AreEqual(actual, true);
        }

        /// <summary>
        /// Checks the claim field document test.
        /// </summary>
        [TestMethod]
        public void IsDocumentInUseTest()
        {
            _mockClaimFieldDocRepository = new Mock<IClaimFieldDocRepository>();
            var dataVal = new ClaimFieldDoc { ClaimFieldDocId = 271326 };
            var result = new List<ContractLog>
            {
                new ContractLog {ModelName = "Model 1", ContractName = "Contract 1", ServiceTypeName = "Service 1"}
            };
            _mockClaimFieldDocRepository.Setup(x => x.IsDocumentInUse(dataVal)).Returns(result);

            _target = new ClaimFieldDocLogic(_mockClaimFieldDocRepository.Object);

            //Act
            List<ContractLog> actual = _target.IsDocumentInUse(dataVal);

            // Assert
            Assert.AreEqual(actual, result);
        }


        /// <summary>
        /// Rename Payment Table
        /// </summary>
        [TestMethod]
        public void RenamePaymentTable()
        {
            var claimFieldDocRepository = new Mock<IClaimFieldDocRepository>();

            // Arrange
            ClaimFieldDoc claimColumnsInfo = new ClaimFieldDoc
            {
                ClaimFieldDocId = 129988,
                FacilityId = 1085,
                TableName = "test",
                UserName = "sumathiachar@emids.com"
            };
            ClaimFieldDoc expectedResult = new ClaimFieldDoc
            {
                ClaimFieldDocId = 129988
            };
            claimFieldDocRepository.Setup(f => f.RenamePaymentTable(claimColumnsInfo)).Returns(expectedResult);
            _target = new ClaimFieldDocLogic(claimFieldDocRepository.Object);

            //Act
            ClaimFieldDoc actual = _target.RenamePaymentTable(claimColumnsInfo);

            //Assert
            Assert.AreEqual(expectedResult.ClaimFieldDocId, actual.ClaimFieldDocId);
        }

        /// <summary>
        /// Rename Paymen tTable By ClaimFieldDocId Is Zero.
        /// </summary>
        [TestMethod]
        public void RenamePaymentTableByClaimFieldDocIdIsZero()
        {
            var claimFieldDocRepository = new Mock<IClaimFieldDocRepository>();

            // Arrange
            ClaimFieldDoc claimColumnsInfo = new ClaimFieldDoc
            {
                ClaimFieldDocId = 0,
                FacilityId = 1085,
                TableName = "test",
                UserName = "sumathiachar@emids.com"
            };
            ClaimFieldDoc expectedResult = new ClaimFieldDoc
            {
                ClaimFieldDocId = 0
            };

            claimFieldDocRepository.Setup(f => f.RenamePaymentTable(claimColumnsInfo)).Returns(expectedResult);
            _target = new ClaimFieldDocLogic(claimFieldDocRepository.Object);

            //Act
            ClaimFieldDoc actual = _target.RenamePaymentTable(claimColumnsInfo);

            //Assert
            Assert.AreEqual(expectedResult.ClaimFieldDocId, actual.ClaimFieldDocId);
        }


        /// <summary>
        /// Rename Payment Table By TableName Is Null
        /// </summary>
        [TestMethod]
        public void RenamePaymentTableByTableNameIsNull()
        {
            var claimFieldDocRepository = new Mock<IClaimFieldDocRepository>();

            // Arrange
            ClaimFieldDoc claimColumnsInfo = new ClaimFieldDoc
            {
                ClaimFieldDocId = 129988,
                FacilityId = 1085,
                TableName = " ",
                UserName = "sumathiachar@emids.com"
            };
            ClaimFieldDoc expectedResult = new ClaimFieldDoc
            {
                ClaimFieldDocId = 0
            };

            claimFieldDocRepository.Setup(f => f.RenamePaymentTable(claimColumnsInfo)).Returns(expectedResult);
            _target = new ClaimFieldDocLogic(claimFieldDocRepository.Object);

            //Act
            ClaimFieldDoc actual = _target.RenamePaymentTable(claimColumnsInfo);

            //Assert
            Assert.AreEqual(expectedResult.ClaimFieldDocId, actual.ClaimFieldDocId);
        }

    }

}
