using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.BusinessLogic;
using System;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for LRecordDataTest and is intended
    ///to contain all LRecordDataTest Unit Tests
    ///</summary>
    [TestClass]
    public class LRecordDataTest
    {
        private LRecordLogic _target;
        // <summary>
        //A test for BuildCRecordString
        //</summary>
        [TestMethod]
        public void BuildLRecordStringNullUnitTest()
        {
            //Arrange
            string expected = string.Empty;

            //Act
            string actual = LRecordLogic.BuildLRecordString(null);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Builds the l record string.
        /// </summary>
        [TestMethod]
        public void BuildLRecordString()
        {
            //Arrange
            LRecord lRecord = new LRecord
            {
                ClaimId = "11111",
                LineItemId = 1,
                HcpcsProcedureCode="1",
                HcpcsModifiers= "HO78",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode="1",
                UnitsofService=1,
                LineItemCharge=100,
                LineItemFlag = 1
            };

            //Act
            string actual = LRecordLogic.BuildLRecordString(lRecord);

            //Assert
            Assert.IsTrue(actual.Contains("L" + lRecord.ClaimId));
        }

        /// <summary>
        /// Builds the length of the l record string test for.
        /// </summary>
        [TestMethod]
        public void BuildLRecordStringTestForLength()
        {
            //Arrange
            LRecord lRecord = new LRecord
            {
                ClaimId = "1111125566644155656664",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers= "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 1
            };

            //Act
            string actual = LRecordLogic.BuildLRecordString(lRecord);

            //Assert
            Assert.IsTrue(actual.Contains("L" + lRecord.ClaimId));
        }

        /// <summary>
        /// Validates the l record.
        /// </summary>
        [TestMethod]
        public void ValidateLRecord()
        {
            //Arrange
            _target=new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "1111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 1
            };
            
            List<LRecord> listLRecords=new List<LRecord>{lRecord};

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.IsTrue(actual.Contains("L" + lRecord.ClaimId));
        }

        /// <summary>
        /// Validatels the record if claim identifier is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfClaimIdIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "1111125566644155656664",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 1
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual,"");
        }

        /// <summary>
        /// Validatels the record if HCPCS modifiers is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfHcpcsModifiersIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86222",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 1
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validatels the record if line item flag is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfLineItemFlagIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86222",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validatels the record if line item identifier is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfLineItemIdIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 0,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> {lRecord};

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");

        }

        /// <summary>
        /// Validatels the record if HCPCS code is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfHcpcsCodeIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "123456",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validatels the record if rev code is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfRevCodeIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "123456",
                UnitsofService = 1,
                LineItemCharge = 100,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Validatels the record if unit of service is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfUnitOfServiceIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = null,
                LineItemCharge = 100,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> {lRecord};

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");


        }

        /// <summary>
        /// Validatels the record if line charge is invalid.
        /// </summary>
        [TestMethod]
        public void ValidatelRecordIfLineChargeIsInvalid()
        {
            //Arrange
            _target = new LRecordLogic();
            LRecord lRecord = new LRecord
            {
                ClaimId = "111",
                LineItemId = 1,
                HcpcsProcedureCode = "1",
                HcpcsModifiers = "GO86",
                ServiceDate = DateTime.Parse("10/20/2019"),
                RevenueCode = "1",
                UnitsofService = 1,
                LineItemCharge = 1000000000,
                LineItemFlag = 0
            };

            List<LRecord> listLRecords = new List<LRecord> { lRecord };

            //Act
            string actual = _target.GetLRecordLine(listLRecords);

            //Assert
            Assert.AreEqual(actual, "");
        }

        /// <summary>
        /// Builds the l record string.
        /// </summary>
        [TestMethod]
        public void ValidateLRecordIfLRecordIsNull()
        {
            //Act
            string actual = LRecordLogic.BuildLRecordString(null);

            //Assert
            Assert.AreSame(actual, "");
        }
    }
}



