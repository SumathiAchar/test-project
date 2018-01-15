using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for ReportSelectionLogicTest and is intended
    ///to contain all ReportSelectionLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class ReportSelectionLogicTest
    {
        /// <summary>
        ///A test for ReportSelectionLogic Constructor
        ///</summary>
        [TestMethod]
        public void ReportSelectionLogicConstructorTest()
        {
            ReportSelectionLogic target = new ReportSelectionLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ReportSelectionLogic));
        }

        /// <summary>
        ///A test for ReportSelectionLogic Constructor
        ///</summary>
        [TestMethod]
        public void ReportSelectionLogicConstructorTest1()
        {
            var reportSelectionRepository = new Mock<IReportSelectionRepository>();
            ReportSelectionLogic target = new ReportSelectionLogic(reportSelectionRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ReportSelectionLogic));
        }


        /// <summary>
        /// Gets all claim fields difference not null logic unit test.
        /// </summary>
        [TestMethod]
        public void GetAllClaimFieldsIfNotNullLogicUnitTest()
        {
            List<ClaimField> claimFieldlisList = new List<ClaimField>
            {
                                                                  new ClaimField {ClaimFieldId =  -99, Text = "Adjudication Request Name"},
                                                                  new ClaimField {ClaimFieldId = 14,Text = "Patient  Account Number"},
                                                                  new ClaimField {ClaimFieldId = 2,Text = "Type of Bill (I)"},
                                                                  new ClaimField {ClaimFieldId = 3,Text = "Revenue Code(I)"},
                                                                  new ClaimField {ClaimFieldId = 4,Text = "HCPCS/RATE/HIPPS"},
                                                                  new ClaimField {ClaimFieldId = 6,Text = "Payer Name"},
                                                                  new ClaimField {ClaimFieldId = 7,Text = "Member ID"},
                                                                  new ClaimField {ClaimFieldId = 8,Text = "DRG(I)"},
                                                                  new ClaimField {ClaimFieldId = 9,Text = "Place of Service(P)"},
                                                                  new ClaimField {ClaimFieldId = 10,Text = "Referring Physician(P)"},
                                                                  new ClaimField {ClaimFieldId = 11,Text = "Rendering Physician(P)"},
                                                                  new ClaimField {ClaimFieldId = 12,Text = "ICD-9 Diagnosis"},
                                                                  new ClaimField {ClaimFieldId = 13,Text = "ICD-9 Procedure(I)"},
                                                                  new ClaimField {ClaimFieldId = 14,Text = "Attending Physician(I)"},
                                                                  new ClaimField {ClaimFieldId = 15,Text = "Total Charges"},
                                                                  new ClaimField {ClaimFieldId = 16,Text = "Statement covers period(I)- Dates of service(P)"},
                                                                  new ClaimField {ClaimFieldId = 17,Text = "Value Codes(I)"},
                                                                  new ClaimField {ClaimFieldId = 18,Text = "Occurrence Code(I)"},
                                                                  new ClaimField {ClaimFieldId = 19,Text = "Condition Codes(I)"},
                                                                  new ClaimField {ClaimFieldId = 20,Text = "Insured’s group"},
                                                                  new ClaimField {ClaimFieldId = 24,Text = "ClaimID"},
                                                                  new ClaimField {ClaimFieldId = 25,Text = "Payment Variance"},
                                                                  new ClaimField {ClaimFieldId = 26,Text = "Contractual Variance"},
                                                                  new ClaimField {ClaimFieldId = 27,Text = "Actual Adj"},
                                                                  new ClaimField {ClaimFieldId = 28,Text = "Calculated Adj"},
                                                                  new ClaimField {ClaimFieldId = 29,Text = "Custom Field 1"},
                                                                  new ClaimField {ClaimFieldId = 30,Text = "Custom Field 2"},
                                                                  new ClaimField {ClaimFieldId = 31,Text = "Custom Field 3"},
                                                                  new ClaimField {ClaimFieldId = 32,Text = "Custom Field 4"},
                                                                  new ClaimField {ClaimFieldId = 33,Text = "Custom Field 5"},
                                                                  new ClaimField {ClaimFieldId = 34,Text = "Custom Field 6"},
                                                                  new ClaimField {ClaimFieldId = 36,Text = "NPI"},
                                                                  new ClaimField {ClaimFieldId = 37,Text = "Claim State"},
                                                                  new ClaimField {ClaimFieldId = 38,Text = "Discharge Status"},
                                                                  new ClaimField {ClaimFieldId = 50,Text = "ICN"},
                                                                  new ClaimField {ClaimFieldId = 51,Text = "MRN"},
                                                                  new ClaimField {ClaimFieldId = 52,Text = "Reviewed"},
                                                                  new ClaimField {ClaimFieldId = 53,Text = "LOS"}
                                                                };
            ClaimSelector claimInfo = new ClaimSelector
            {
                ModuleId = 5
            };
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            reportSelectionRepository.Setup(x => x.GetAllClaimFields(It.IsAny<ClaimSelector>())).Returns(claimFieldlisList);
            var actual = new ReportSelectionLogic(reportSelectionRepository.Object).GetAllClaimFields(claimInfo);
            const int expected = 38;
            Assert.AreEqual(expected, actual.Count);
        }

        /// <summary>
        ///A test for GetAllClaimFieldsOperators
        ///</summary>
        [TestMethod]
        public void GetAllClaimFieldsOperatorsTest()
        {
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            ReportSelectionLogic target = new ReportSelectionLogic(reportSelectionRepository.Object);
            List<ClaimFieldOperator> claimFieldOperators = new List<ClaimFieldOperator>
            {
                new ClaimFieldOperator {FacilityId = 1}
            };
            reportSelectionRepository.Setup(f => f.GetAllClaimFieldsOperators()).Returns(claimFieldOperators);
            List<ClaimFieldOperator> actual = target.GetAllClaimFieldsOperators();
            Assert.IsNotNull(actual);

        }

        /// <summary>
        /// Gets all claim fields operators difference not null logic unit test.
        /// </summary>
        [TestMethod]
        public void GetAllClaimFieldsOperatorsIfNotNullLogicUnitTest()
        {
            List<ClaimFieldOperator> claimFieldOperator = new List<ClaimFieldOperator>
            {
                                                        new ClaimFieldOperator{OperatorId = 1,OperatorType = "<>"},
                                                        new ClaimFieldOperator{OperatorId =2 ,OperatorType = ">"},
                                                        new ClaimFieldOperator{OperatorId =3 ,OperatorType = "="},
                                                        new ClaimFieldOperator{OperatorId = 4,OperatorType = "<"}
                                                    };
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            reportSelectionRepository.Setup(x => x.GetAllClaimFieldsOperators()).Returns(claimFieldOperator);
            var actual = new ReportSelectionLogic(reportSelectionRepository.Object).GetAllClaimFieldsOperators();
            const int expected = 4;
            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod]
        public void GetAllClaimFields()
        {
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            reportSelectionRepository.Setup(x => x.GetAllClaimFields(It.IsAny<ClaimSelector>())).Returns(new List<ClaimField>());
            ClaimSelector claimInfo = new ClaimSelector
            {
                ModuleId = 5
            };
            new ReportSelectionLogic(reportSelectionRepository.Object).GetAllClaimFields(claimInfo);
            reportSelectionRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllClaimFieldsOperators()
        {
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            reportSelectionRepository.Setup(x => x.GetAllClaimFieldsOperators()).Returns(new List<ClaimFieldOperator>());

            new ReportSelectionLogic(reportSelectionRepository.Object).GetAllClaimFieldsOperators();
            reportSelectionRepository.VerifyAll();
        }

        /// <summary>
        /// Gets the claim reviewed option test.
        /// </summary>
        [TestMethod]
        public void GetClaimReviewedOptionTest()
        {
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();
            reportSelectionRepository.Setup(x => x.GetClaimReviewedOption()).Returns(new List<ReviewedOptionType>());

            var target = new ReportSelectionLogic(reportSelectionRepository.Object);

            var actual = target.GetClaimReviewedOption();

            Assert.IsInstanceOfType(actual, typeof(List<ReviewedOptionType>));

        }

        [TestMethod]
        public void GetAdjudicationRequestNames()
        {
            Mock<IReportSelectionRepository> reportSelectionRepository = new Mock<IReportSelectionRepository>();

            reportSelectionRepository.Setup(x => x.GetAdjudicationRequestNames(It.IsAny<ClaimSelector>()))
                .Returns(new List<ClaimSelector>());
            var target = new ReportSelectionLogic(reportSelectionRepository.Object);
            var actual = target.GetAdjudicationRequestNames(It.IsAny<ClaimSelector>());

            Assert.IsInstanceOfType(actual, typeof(List<ClaimSelector>));
        }

        /// <summary>
        /// Adds query name if not null.
        /// </summary>
        [TestMethod]
        public void AddQueryNameUnitTest()
        {
            // Arrange
            const int expectedResult = 1;
            ClaimSelector claimSelector = new ClaimSelector
            {
                UserId = 3421,
                FacilityId = 1,
                UserName = "xyz@emids.com",
                FacilityName = "SSI Demo Hospital",
                QueryId = 0,
                QueryName = "Q101",
                SelectCriteria = "54|3|21"
            };
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.AddEditQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            //Act
            int actual = target.AddEditQueryName(claimSelector);
            //Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Adds query name if null.
        /// </summary>
        [TestMethod]
        public void AddQueryNameIfNullUnitTest()
        {
            // Arrange
            const int expectedResult = -1;
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.AddEditQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            //Act
            int actual = target.AddEditQueryName(null);
            //Assert
            Assert.AreEqual(expectedResult, actual);
        }


        /// <summary>
        /// Edit query name if not null.If query name is new then queryId is 0.If query name is existing then queryId is passed.
        /// </summary>
        [TestMethod]
        public void EditQueryNameWithQueryIdUnitTest()
        {
            // Arrange
            const int expectedResult = 2;
            ClaimSelector claimSelector = new ClaimSelector
            {
                UserId = 3421,
                FacilityId = 1,
                UserName = "xyz@emids.com",
                FacilityName = "SSI Demo Hospital",
                QueryId = 23,
                QueryName = "Q101",
                SelectCriteria = "54|3|21~2|3|131"
            };
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.AddEditQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            //Act
            int actual = target.AddEditQueryName(claimSelector);
            //Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Edit query name if not null.If query name is new then queryId is 0.If query name is existing then queryId is passed.
        /// </summary>
        [TestMethod]
        public void EditQueryNameWithoutQueryIdUnitTest()
        {
            // Arrange
            const int expectedResult = 1;
            ClaimSelector claimSelector = new ClaimSelector
            {
                UserId = 3421,
                FacilityId = 1,
                UserName = "xyz@emids.com",
                FacilityName = "SSI Demo Hospital",
                QueryId = 0,
                QueryName = "Q1011",
                SelectCriteria = "54|3|21~2|3|131"
            };
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.AddEditQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            //Act
            int actual = target.AddEditQueryName(claimSelector);
            //Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Delete query name by Id if not null.
        /// </summary>
        [TestMethod]
        public void DeleteQueryNameUnitTest()
        {
            // Arrange
            const bool expectedResult = true;
            ClaimSelector claimSelector = new ClaimSelector
            {
                QueryId = 23,
                UserName = "xyz@emids.com",
                FacilityName = "SSI Demo Hospital",
                QueryName = "Q1011",
                SelectCriteria = "54|3|21~2|3|131"
            };
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.DeleteQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            // Act
            bool actual = target.DeleteQueryName(claimSelector);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Delete query name by Id if null.
        /// </summary>
        [TestMethod]
        public void DeleteQueryNameIsNullUnitTest()
        {
            // Arrange
            const bool expectedResult = false;
            //Mock setup
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(x => x.DeleteQueryName(It.IsAny<ClaimSelector>())).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            // Act
            bool actual = target.DeleteQueryName(null);
            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Get QuriesById Test  
        /// </summary>
        [TestMethod]
        public void GetQueriesByIdTest()
        {
            // Arrange
            //Mock Input
            User userInfo = new User { FacilityId = 11672, UserId = 12 };
            //Mock Output
            List<ClaimSelector> expectedResult = new List<ClaimSelector> { new ClaimSelector { QueryName = "TestQuery", QueryId = 12 } };

            // Act
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(f => f.GetQueriesById(userInfo)).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            List<ClaimSelector> actual = target.GetQueriesById(userInfo);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult.Count, actual.Count);
            Assert.AreEqual(expectedResult[0].QueryName, actual[0].QueryName);
            Assert.AreEqual(expectedResult[0].QueryId, actual[0].QueryId);
        }

        /// <summary>
        /// Get QuriesById Null Test  
        /// </summary>
        [TestMethod]
        public void GetQueriesByIdNullTest()
        {
            // Arrange
            //Mock Output
            List<ClaimSelector> expectedResult = new List<ClaimSelector>();

            // Act
            Mock<IReportSelectionRepository> mockReportSelectionRepository = new Mock<IReportSelectionRepository>();
            mockReportSelectionRepository.Setup(f => f.GetQueriesById(null)).Returns(expectedResult);
            ReportSelectionLogic target = new ReportSelectionLogic(mockReportSelectionRepository.Object);
            List<ClaimSelector> actual = target.GetQueriesById(null);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.AreEqual(expectedResult.Count, actual.Count);
        }
    }
}
