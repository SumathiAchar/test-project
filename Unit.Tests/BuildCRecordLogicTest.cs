using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for BuildCRecordLogicTest and is intended
    ///to contain all BuildCRecordLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class BuildCRecordLogicTest
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
        ///A test for BuildCRecordString for Null
        ///</summary>
        [TestMethod]
        public void BuildCRecordStringTestNull()
        {
            string expected = string.Empty;
            string actual = CRecordLogic.BuildCRecordString(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BuildCRecordString for Not Null
        ///</summary>
        [TestMethod]
        public void BuildCRecordStringNotNullTest()
        {
            CRecord cRecordRaw = new CRecord
            {
                ClaimId = "1.04.2",
                Sex = 1,
                FromDate = DateTime.Parse("09/12/2019"),
                ThruDate = DateTime.Parse("10/20/2019"),
                ConditionCodes = new List<string> {"9", "10"},
                BillType = "331",
                Npi = string.Empty,
                Oscar = string.Empty,
                PatientStatus = "1",
                OppsFlag = 1,
                OccurrenceCodes = new List<string> {"AB", "CD"},
                PatientFirstName = "ANUPAMA",
                PatientLastName = "SAIRAM",
                PatientMiddleInitial = "A",
                BeneAmount = 50.35,
                BloodPint = 2
            };

            //  cRecordRaw.ConditionCodes = new List<string> { "AB", "CD", "EF", "GH", "IJ", "KL", "MN", "AB", "CD", "EF" };
            //  cRecordRaw.OccurrenceCodes = new List<string> { "AB", "CD", "EF", "GH", "IJ", "KL", "MN", "AB", "CD", "EF" };

           // const string expected = "C1.04.2           01412019091220191020910           331                   011ABCD                SAIRAM              ANUPAMA        A00050352                                                           ";
            string actual = CRecordLogic.BuildCRecordString(cRecordRaw);

            Assert.IsNotNull(actual);    
        }
    }
}
