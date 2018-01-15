using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class ERecordLogicTest
    {
        private ERecordLogic _target;

        /// <summary>
        /// Builds the e record string null unit test.
        /// </summary>
        [TestMethod]
        public void BuildERecordStringNullUnitTest()
        {
            //Arrange
            string expected = string.Empty;

            //Act
            string actual = ERecordLogic.BuildERecordString(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the e record.
        /// </summary>
        [TestMethod]
        public void ValidateERecord()
        { 
            //Arrange
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639"}
            };
            
            const string expected =
                "E422379168        1256    3568    4444    5555    9981    0381    2918    92      30080   5190    5648    7638    30081   5191    5649    7639                                                          ";

            //Act
            string actual = _target.GetERecordLine(eRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the e record string test.
        /// </summary>
        [TestMethod]
        public void BuildERecordStringTest()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
            };
            const string expected =
                "E422379168        1256    3568    4444    5555    9981    0381    2918    9223    30080   5190    5648    7638                                                                                          ";

            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the e record string test for none.
        /// </summary>
        [TestMethod]
        public void BuildERecordStringTestForNone()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "NONE",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638" }
            };
            const string expected =
                "E422379168        1256            4444    5555    9981    0381    2918    9223    30080   5190    5648    7638                                                                                          ";

            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the e record string test for secondary diagnosis code.
        /// </summary>
        [TestMethod]
        public void BuildERecordStringTestForSecondaryDiagnosisCode()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444789", "5555", "9981", "0381", "2918", "9223256354", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639"}
            };
            const string expected =
                "E422379168        1256    3568    4444789 5555    9981    0381    2918    9223256330080   5190    5648    7638    30081   5191    5649    7639                                                          ";
            
            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);
            //Assert
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Builds the length of the e record string for geater than200.
        /// </summary>
        [TestMethod]
        public void GetERecordStringForGeaterThan200Length()
        {
            //Arrange
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.223213265525888536",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const int expected = 200;

            //Act
            int actual = _target.GetERecordLine(eRecordRaw).Length;

            //Assert
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        /// Validates the e record if claim identifier is invalid.
        /// </summary>
        [TestMethod]
        public void ValidateERecordIfClaimIdIsInvalid()
        {
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.2260000000000000",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            string actual = _target.GetERecordLine(eRecordRaw);
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validates the e record if secondary diagnosis length is invalid.
        /// </summary>
        [TestMethod]
        public void ValidateERecordIfSecondaryDiagnosisLengthIsInvalid()
        {
            //Arrange
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444000", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639", "256363" }
            };
            const string expected =
                "E422379168        1256    3568    4444000 5555    9981    0381    2918    92      30080   5190    5648    7638    30081   5191    5649    7639                                                          ";
            //Act
            string actual = _target.GetERecordLine(eRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        /// <summary>
        /// Builds the e record string for geater than.
        /// </summary>
        [TestMethod]
        public void BuildERecordStringForGeaterThan()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.223213265525888536",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const string expected =
                "E1.04.2232132655258885361042    1042    4444    5555    9981    0381    2918    92      30080   5190    5648    7638    30081   5191    5649    7639                                                          ";

            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void BuildERecordStringTestForSecondaryDiagnosisCodeIfCountIsGreaterThenFourteen()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "422379168",
                AdmitDiagnosisCode = "1256",
                PrincipalDiagnosisCode = "3568",
                SecondaryDiagnosisCodes =
                    new List<string> { "4444789", "5555", "9981", "0381", "2918", "9223256354", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639", "4444789", "5555", "9981", "0381", "2918", "9223256354", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };
            const string expected =
                "E422379168        1256    3568    4444789 5555    9981    0381    2918    9223256330080   5190    5648    7638    30081   5191    5649    7639                                                          ";

            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Validates the d record if null test.
        /// </summary>
        [TestMethod]
        public void ValidateDRecordIfErecordNullTest()
        {
            //Arrange
            _target = new ERecordLogic();
            string expected = string.Empty;

            //Act
            string actual = _target.GetERecordLine(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ValidateERecordIfSecondaryDiagnosisCodesIsNull()
        {
            //Arrange
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = null
            };

            //Act
            string actual = _target.GetERecordLine(eRecordRaw);

            //Assert
            Assert.AreEqual(actual, "");
        }

        [TestMethod]
        public void ValidateERecordIfClaimIdIsNull()
        {
            //Arrange
            _target = new ERecordLogic();
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = null,
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            const string expected =
                "E                 1042    1042    4444    5555    9981    0381    2918    92      30080   5190    5648    7638    30081   5191    5649    7639                                                          ";
            //Act
            string actual = _target.GetERecordLine(eRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void BuildERecordIfPrincipalDiagnosisCodeIsNull()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = null,
                SecondaryDiagnosisCodes = new List<string> { "4444", "5555", "9981", "0381", "2918", "92", "30080", "5190", "5648", "7638", "30081", "5191", "5649", "7639" }
            };

            const string expected =
                "E1.04.2           1042            4444    5555    9981    0381    2918    92      30080   5190    5648    7638    30081   5191    5649    7639                                                          ";
            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void BuildERecordIfSecondaryDiagnosisCodeIsNull()
        {
            //Arrange
            ERecord eRecordRaw = new ERecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "1.04.2",
                PrincipalDiagnosisCode = "1.04.2",
                SecondaryDiagnosisCodes = null
            };

            const string expected =
                "E1.04.2           1042    1042                                                                                                                                                                          ";
            //Act
            string actual = ERecordLogic.BuildERecordString(eRecordRaw);

            //Assert
            Assert.AreEqual(actual, expected);
        }
    }
}
