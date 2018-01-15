using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for DRecordLogicTest and is intended
    ///to contain all DRecordLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class DRecordLogicTest
    {
        private DRecordLogic _target;
        /// <summary>
        ///A test for BuildDRecordString
        ///</summary>
        [TestMethod]
        public void BuildDRecordStringNullUnitTest()
        {
            //Arrange
            string expected = string.Empty;

            //Act
            string actual = DRecordLogic.BuildDRecordString(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }
        

        /// <summary>
        ///A test for BuildDRecordString
        ///</summary>
        [TestMethod]
        public void BuildDRecordStringTest()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> {"4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638"}
            };
            const string expected =
                "D1.04.2           1042  1042  4444  5555  9981  0381  2918  9223  30080 5190  5648  7638                                                                                                                ";
           
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Builds the d record string test for none.
        /// </summary>
        [TestMethod]
        public void BuildDRecordStringTestForNone()
        {
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "NONE",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
            };
            const string expected =
                "D1.04.2           1042        4444  5555  9981  0381  2918  9223  30080 5190  5648  7638                                                                                                                ";
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Builds the d record string test for secondary diagnosis code.
        /// </summary>
        [TestMethod]
        public void BuildDRecordStringTestForSecondaryDiagnosisCode()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const string expected =
                "D1.04.2           1042  1042  4444  5555  9981  0381  2918  9223  30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the d record string test for secondary diagnosis code.
        /// </summary>
        [TestMethod]
        public void BuildDRecordStringForGeaterThan()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.223213265525888536",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const string expected =
                "D1.04.2232132655258885361042  1042  4444  5555  9981  0381  2918  92    30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the d record.
        /// </summary>
        [TestMethod]
        public void ValidateDRecord()
        {
            //Arrange
            _target =new DRecordLogic();
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.226",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            
            const string expected =
                "D1.04.226         1042  1042  4444  5555  9981  0381  2918  92    30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";

            //Act
            string actual = _target.GetDRecordLine(dRecordRaw);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the d record.
        /// </summary>
        [TestMethod]
        public void ValidateDRecordIfClaimIdIsInvalid()
        {
            //Arrange
            _target = new DRecordLogic();
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2260000000000000",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            //Act
            string actual = _target.GetDRecordLine(dRecordRaw);

            //Assert
            Assert.AreEqual(actual,"");
        }
       
        /// <summary>
        /// Validates the d record if secondary diagnosis is invalid.
        /// </summary>
        [TestMethod]
        public void ValidateDRecordIfSecondaryDiagnosisIsInvalid()
        {
            //Arrange
            _target = new DRecordLogic();
            const string result = "D1.04.226         1042  1042  4444005555  9981  0381  2918  92    30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.226",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444000", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            //Act
            string actual = _target.GetDRecordLine(dRecordRaw);

            //Assert
            Assert.AreEqual(actual, result);
        }

        [TestMethod]
        public void BuildDRecordStringTestForSecondaryDiagnosisCodeIfCountIsGreaterThenFourteen()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648",
                        "7638", "30081", "5191", "5649", "7639", "4444", "5555", "9981", "0381", "2918", 
                        "9223", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const string expected =
                "D1.04.2           1042  1042  4444  5555  9981  0381  2918  9223  30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the d record if null test.
        /// </summary>
        [TestMethod]
        public void ValidateDRecordIfDrecordNullTest()
        {
            //Arrange
            _target = new DRecordLogic();
            string expected = string.Empty;

            //Act
            string actual = _target.GetDRecordLine(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidateDRecordIfSecondaryDiagnosisCodesIsNull()
        {
            //Arrange
            _target = new DRecordLogic();
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = null
            };

            //Act
            string actual = _target.GetDRecordLine(dRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        [TestMethod]
        public void ValidateDRecordIfClaimIdIsNull()
        {
            //Arrange
            _target = new DRecordLogic();
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = null,
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            const string expected = "D                 1042  1042  4444  5555  9981  0381  2918  92    30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            //Act
            string actual = _target.GetDRecordLine(dRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void BuildDRecordIfPrincipalDiagnosisCodeIsNull()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = null,
                SecondaryDiagnosisCodes = new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            const string expected = "D1.04.2           1042        4444  5555  9981  0381  2918  92    30080 5190  5648  7638  30081 5191  5649  7639                                                                                        ";
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void BuildDRecordIfSecondaryDiagnosisCodeIsNull()
        {
            //Arrange
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = null
            };

            const string expected =
                "D1.04.2           1042  1042                                                                                                                                                                            ";
            //Act
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

    }
}

