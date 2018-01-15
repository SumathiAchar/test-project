using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for CRecordLogicTest and is intended
    ///to contain all CRecordLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class CRecordLogicTest
    {
        //Creating object for Logic
        private CRecordLogic _target;
        /// <summary>
        ///A test for BuildCRecordString
        ///</summary>
        [TestMethod]
        public void BuildCRecordStringTest()
        {
            //Arrange
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = null,
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }

            };

            const string expected = "C1.04.2           201812019091220191020              331                   011                    SAIRAM              ANUPAMA        A00050352                                                           ";

            //Act
            string actual = CRecordLogic.BuildCRecordString(cRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        // <summary>
        //A test for BuildCRecordString
        //</summary>
        [TestMethod]
        public void BuildCRecordStringNullUnitTest()
        {
            //Arrange
            string expected = string.Empty;

            //Act
            string actual = CRecordLogic.BuildCRecordString(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the c record string returns exception.
        /// </summary>
        [TestMethod]
        public void BuildCRecordStringReturnsException()
        {
            //Arrange
            CRecord cRecordDataRaw = new CRecord { Sex = 1, OppsFlag = 1, BloodPint = 1, Dob = DateTime.Now.AddYears(-1).AddDays(1), ThruDate = DateTime.Now };

            //Act
            string actual = CRecordLogic.BuildCRecordString(cRecordDataRaw);

            //Assert
            Assert.IsTrue(actual.Length > 0);
        }

        /// <summary>
        /// Builds the c record string returns error message.
        /// </summary>
        [TestMethod]
        public void BuildCRecordStringReturnsErrorMessage()
        {
            //Arrange
            CRecord cRecordDataRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = null,
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }

            };

            //Act
            string actual = CRecordLogic.BuildCRecordString(cRecordDataRaw);

            //Assert
            Assert.IsTrue(actual.Contains("A00050352"));
        }

        /// <summary>
        /// Builds the c record for build occurrence codes test.
        /// </summary>
        [TestMethod]
        public void BuildCRecordForBuildOccurrenceCodesTest()
        {
            //Arrange
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = null,
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = new List<string> { "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" },
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"
                   
                }
            };

            const string expected =
                "C1.04.2           201812019091220191020              331                   01113141516171819202122SAIRAM              ANUPAMA        A00050352                                                           ";

            //Act
            string actual = CRecordLogic.BuildCRecordString(cRecordRaw);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the c record for condition codes test.
        /// </summary>
        [TestMethod]
        public void BuildCRecordForConditionCodesTest()
        {
            //Arrange
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> {"20", "21", "22", "23", "24", "25", "26"},
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }
            };

            const string expected =
                "C1.04.2           20181201909122019102020212223242526331                   011                    SAIRAM              ANUPAMA        A00050352                                                           ";
            //Act
            string actual = CRecordLogic.BuildCRecordString(cRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the c record if patient data is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecord()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),
                }
            };
            const string expected =
                "C1.04.2            191201909122019102020212223242526331                   011                    SAIRAM              ANUPAMA        A00050352                                                           ";

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);
            
            //Assert
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Validates the c record if patient data is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfPatientDataIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);
            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if opps flag is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfOppsFlagIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 3,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if blood pint is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfBloodPintIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 4,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);
            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if claim identifier length is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfClaimIdLengthIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.20000000000000",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if bill type is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfBillTypeIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.20",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "3310",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if npi is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfNpiIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.20",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = "123456789000000000",
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if occurrence codes is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfOccurrenceCodesIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.20",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = new List<string>{"Test Occurance Code"},
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if beneficiary amount is in valid.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfBeneficiaryAmountIsInValid()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.20",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.00000000000035,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1",
                    Dob = DateTime.Parse("10/20/2000"),

                }
            };

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the c record if null.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfNull()
        {
            //Arrange 
            _target = new CRecordLogic();
            const string expected = "";

            //Act
            string actual = _target.GetCRecordLine(null);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Validates the c record if patient data is null.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfPatientDataIsNull()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = null
            };
            const string expected = "";

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        /// <summary>
        /// Validates the c record if bill type is null.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfBillTypeIsNull()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> { "20", "21", "22", "23", "24", "25", "26" },
                BillType = null,
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }
            };
            const string expected = "";

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Validates the c record if condition codes is null.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfConditionCodesIsNull()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "C1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = null,
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }

            };

            const string expected = "";

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }


        /// <summary>
        /// Validates the c record if claim identifier is null.
        /// </summary>
        [TestMethod]
        public void ValidateCRecordIfClaimIdIsNull()
        {
            //Arrange
            _target = new CRecordLogic();
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = null,
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = null,
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = null,
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2,
                PatientData = new PatientData
                {
                    FirstName = "ANUPAMA",
                    LastName = "SAIRAM",
                    MiddleName = "A",
                    Medicare = "",
                    Gender = 1,
                    Status = "1"

                }

            };

            const string expected = "";

            //Act
            string actual = _target.GetCRecordLine(cRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

    }
}
