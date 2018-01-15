using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for BuildDRecordLogicTest and is intended
    ///to contain all BuildDRecordLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class BuildDRecordLogicTest
    {



        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for BuildDRecordString for Null
        ///</summary>
        [TestMethod]
        public void BuildDRecordStringNullTest()
        {
            string expected = string.Empty;
            string actual = DRecordLogic.BuildDRecordString(null);
            Assert.AreEqual(expected, actual);  
        }
        /// <summary>
        ///A test for BuildDRecordString for Not Null
        ///</summary>
        [TestMethod]
        public void BuildDRecordStringNotNullTest()
        {
            DRecord dRecordRaw = new DRecord
            {
                ClaimId = "1.04.2",
                AdmitDiagnosisCode = "4444",
                PrincipalDiagnosisCode = "5555",
                SecondaryDiagnosisCodes =
                    new List<string> {"9981", "0381", "2918", "9223", "30080", "5190", "5648", "7638"}
            };

            const string expected = "D1.04.2           4444  5555  9981  0381  2918  9223  30080 5190  5648  7638                                                                                                                            ";
            string actual = DRecordLogic.BuildDRecordString(dRecordRaw);
            Assert.AreEqual(expected, actual);
        }
    }
}
