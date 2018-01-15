using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for BuildLRecordLogicTest and is intended
    ///to contain all BuildLRecordLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class BuildLRecordLogicTest
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
        ///A test for BuildLRecordString for Null
        ///</summary>
        [TestMethod]
        public void BuildLRecordStringNullTest()
        {
            string expected = string.Empty;
            string actual = LRecordLogic.BuildLRecordString(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BuildLRecordString
        ///</summary>
        [TestMethod]
        public void BuildLRecordStringTest()
        {
            LRecord lRecordRaw = new LRecord
            {
                ClaimId = "1.04.2",
                LineItemId = 1,
                HcpcsProcedureCode = "00164",
                HcpcsModifiers= "4455667788",
                ServiceDate = DateTime.Parse("09/30/2019"),
                RevenueCode = "0929",
                UnitsofService = 1,
                LineItemCharge = 0000000.00,
                LineItemFlag = 4
            };

            const string expected = "L1.04.2           001001644455667788201909300929000000100000000004                                                                                                                                      ";
            string actual = LRecordLogic.BuildLRecordString(lRecordRaw);
            Assert.AreEqual(expected, actual); 
        }
    }
}
