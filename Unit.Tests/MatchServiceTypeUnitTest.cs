using MedWorth.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MedWorth.ContractManagement.Shared.Models;
using MedWorth.ContractManagement.Shared.Helpers;
using System.Collections.Generic;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for MatchServiceTypeTest and is intended
    ///to contain all MatchServiceTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchServiceTypeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

        //TODO: TO CHECK
        /// <summary>
        ///A test for MatchServiceType Constructor
        ///</summary>
        [TestMethod()]
        public void MatchServiceTypeConstructorTest()
        {
            MatchServiceType target = new MatchServiceType();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetAvailablePaymentType for Null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypeNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = null;
            Nullable<Enums.PaymentTypeCodes> expected = new Nullable<Enums.PaymentTypeCodes>();
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for Not Null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypeNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes();
            Nullable<Enums.PaymentTypeCodes> expected = new Nullable<Enums.PaymentTypeCodes>();
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for checking all ContractServiceType properties not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypeContractServiceTypeValuesNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes();
            Nullable<Enums.PaymentTypeCodes> expected = new Nullable<Enums.PaymentTypeCodes>();
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeASCFeeSchedule not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypeASCFeeScheduleNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeASCFeeSchedule = new PaymentTypeASCFeeSchedule { PaymentTypeDetailId = 1111, ContractId = 12121 }};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.ASCFeeSchedule;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeDRGPayment not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypeDRGPaymentNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeASCFeeSchedule = new PaymentTypeASCFeeSchedule { PaymentTypeDetailId = 1111, ContractId = 12121 }, PaymentTypeDRGPayment = new PaymentTypeDRGPayment { PaymentTypeDetailId = 1111, PaymentTypeId = 121211, ContractId = 12121 }, PaymentTypeFeeSchedules = new PaymentTypeFeeSchedules { ContractId = 12121, ClaimFieldDocID = 121 }, PaymentTypeMedicareIPPayment = new PaymentTypeMedicareIPPayment { ContractId = 12121, PaymentTypeDetailId = 1111111 }, PaymentTypeMedicareOPPayment = new PaymentTypeMedicareOPPayment { ContractId = 12121, FacilityId = 131 }, PaymentTypePerCase = new PaymentTypePerCase { ContractId = 12121, FacilityId = 131 }, PaymentTypePerDiem = new List<PaymentTypePerDiem> { new PaymentTypePerDiem { PaymentTypeDetailID = 11111, ContractID = 12121 } } };
            Nullable<Enums.PaymentTypeCodes> expected = new Nullable<Enums.PaymentTypeCodes>();
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeDRGPayment not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypeTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeDRGPayment = new PaymentTypeDRGPayment { PaymentTypeDetailId = 1111, PaymentTypeId = 121211, ContractId = 12121 }};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.DRGPayment;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeFeeSchedules not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypeFeeSchedulesNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeFeeSchedules = new PaymentTypeFeeSchedules{ContractId = 12121,ClaimFieldDocID = 131}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.FeeSchedule;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeMedicareIPPayment not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypeMedicareIpPaymentNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeMedicareIPPayment = new PaymentTypeMedicareIPPayment{ContractId = 12121,FacilityId = 121}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.MedicareIP;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypeMedicareOPPayment not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypeMedicareOpPaymentNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypeMedicareOPPayment = new PaymentTypeMedicareOPPayment{ContractId = 12121,FacilityId = 121}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.MedicareOP;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypePerCase not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypePerCaseNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypePerCase = new PaymentTypePerCase{ContractId = 12121,FacilityId = 121}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.PerCase;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypePerDiem not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypePerDiemNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypePerDiem = new List<PaymentTypePerDiem>{new PaymentTypePerDiem{ContractID = 12121,FacilityId = 121}}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.PerDiem;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypePerVisit not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypePerVisitNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypePerVisit = new PaymentTypePerVisit{ContractId = 12121,FacilityId = 121}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.PerVisit;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetAvailablePaymentType for PaymentTypePercentageDiscount not null
        ///</summary>
        [TestMethod()]
        public void GetAvailablePaymentTypePaymentTypePercentageDiscountNotNull()
        {
            MatchServiceType target = new MatchServiceType();
            ContractServiceTypes contractServiceType = new ContractServiceTypes { PaymentTypePercentageDiscount = new PaymentTypePercentageDiscount{ContractId = 12121,FacilityId = 121}};
            Nullable<Enums.PaymentTypeCodes> expected = Enums.PaymentTypeCodes.PercentageDiscountPayment;
            Nullable<Enums.PaymentTypeCodes> actual = target.GetAvailablePaymentType(contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetUpdatedClaimChargeDataBasedOnMatchedContract for Null
        ///</summary>
        [TestMethod()]
        public void GetUpdatedClaimChargeDataBasedOnMatchedContractNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            List<ClaimData> claimDataList = null;
            List<Contracts> contracts = null;
            List<ClaimData> expected = null;
            List<ClaimData> actual = target.GetUpdatedClaimChargeDataBasedOnMatchedContract(claimDataList, contracts);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetUpdatedClaimChargeDataBasedOnMatchedContract Not Null
        ///</summary>
        [TestMethod()]
        public void GetUpdatedClaimChargeDataBasedOnMatchedContractNotNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            List<ClaimData> claimDataList = new List<ClaimData>();
            List<Contracts> contracts = new List<Contracts>();
            List<ClaimData> expected = new List<ClaimData>();
            List<ClaimData> actual = target.GetUpdatedClaimChargeDataBasedOnMatchedContract(claimDataList, contracts);
            Assert.AreEqual(expected.Count, actual.Count);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for GetUpdatedClaimChargeDataBasedOnMatchedContract check IsMatched=true & PaymentTypeStopLoss Null
        ///</summary>
        [TestMethod()]
        public void GetUpdatedClaimChargeDataBasedOnMatchedContractPaymentTypeStopLossNullTest()
        {
            MatchServiceType target = new MatchServiceType();
            List<ClaimData> claimDataList = new List<ClaimData>{new ClaimData{MatchedContractId = 12121,IsMatched = true}};
            List<Contracts> contracts = new List<Contracts>{new Contracts{ContractId = 12121,PaymentTypeStopLoss = null}};
            List<ClaimData> expected = claimDataList;
            List<ClaimData> actual = target.GetUpdatedClaimChargeDataBasedOnMatchedContract(claimDataList, contracts);
            Assert.AreEqual(expected.Count, actual.Count);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsClaimFieldAvailable for Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsClaimFieldAvailableNullTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceLineClaimFieldSelection contractServiceLineClaimFieldSelection = null;
            ContractServiceTypes contractServiceTypes = null;
            List<ClaimFieldDocs> docs = null;
            bool expected = true;
            bool actual = target.IsClaimFieldAvailable(contractServiceLineClaimFieldSelection, contractServiceTypes, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for IsClaimFieldAvailable for Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsClaimFieldAvailableNotNullTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceLineClaimFieldSelection contractServiceLineClaimFieldSelection = new ContractServiceLineClaimFieldSelection();
            ContractServiceTypes contractServiceTypes = new ContractServiceTypes();
            List<ClaimFieldDocs> docs = new List<ClaimFieldDocs>();
            bool expected = true;
            bool actual = target.IsClaimFieldAvailable(contractServiceLineClaimFieldSelection, contractServiceTypes, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        /// <summary>
        ///A test for IsClaimFieldAvailable for Mock Test
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsClaimFieldAvailableMockTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceLineClaimFieldSelection contractServiceLineClaimFieldSelection = new ContractServiceLineClaimFieldSelection{ClaimFieldId = 131};
            ContractServiceTypes contractServiceTypes = new ContractServiceTypes{ContractServiceLineClaimFieldSelectionList = new List<ContractServiceLineClaimFieldSelection>{new ContractServiceLineClaimFieldSelection{ClaimFieldId = 131}}};
            List<ClaimFieldDocs> docs = new List<ClaimFieldDocs>{ new ClaimFieldDocs{ClaimFieldValues = new List<ClaimFieldValues>{new ClaimFieldValues{ContractID = 12121,Identifier = "Test"}},ClaimFieldID = 131}};
            bool expected = false;
            bool actual = target.IsClaimFieldAvailable(contractServiceLineClaimFieldSelection, contractServiceTypes, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsContractServiceTypeFiltersAreValidForLineItem for Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsContractServiceTypeFiltersAreValidForLineItemTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = null;
            ClaimCode code = new ClaimCode();
            List<ClaimFieldDocs> docs = null;
            ClaimData claimData = null;
            bool expected = false;
            bool actual = target.IsContractServiceTypeFiltersAreValidForLineItem(contractServiceTypes, code, docs, claimData);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsContractServiceTypeFiltersAreValidORNot nULL
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsContractServiceTypeFiltersAreValidORNotTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = null;
            ClaimCode code = null;
            List<ClaimFieldDocs> docs = null;
            ClaimData claimData = null;
            bool expected = false;
            bool actual = target.IsContractServiceTypeFiltersAreValidORNot(contractServiceTypes, code, docs, claimData);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsFilterForServiceLineCode for Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsFilterForServiceLineCodeTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = null;
            List<string> revenueCodes = null;
            Enums.ServiceLineCodes serviceLineType = new Enums.ServiceLineCodes();
            Enums.ClaimFieldTypes claimFieldType = new Enums.ClaimFieldTypes();
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsFilterForServiceLineCode(contractServiceTypes, revenueCodes, serviceLineType, claimFieldType, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsServiceLineClaimFieldData for Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsServiceLineClaimFieldDataNullTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = null;
            Enums.ClaimFieldTypes claimFieldType = new Enums.ClaimFieldTypes();
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsServiceLineClaimFieldData(contractServiceTypes, claimFieldType, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsServiceLineClaimFieldData for Not Null
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsServiceLineClaimFieldDataNotNullTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = new ContractServiceTypes{ContractServiceLineClaimFieldSelectionList = new List<ContractServiceLineClaimFieldSelection>{new ContractServiceLineClaimFieldSelection{ClaimFieldId = 131}}};
            Enums.ClaimFieldTypes claimFieldType = Enums.ClaimFieldTypes.InsuredID;
            List<ClaimFieldDocs> docs = new List<ClaimFieldDocs>();
            bool expected = false;
            bool actual = target.IsServiceLineClaimFieldData(contractServiceTypes, claimFieldType, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsServiceLineClaimFieldSelection
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsServiceLineClaimFieldSelectionTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            List<ContractServiceLineClaimFieldSelection> contractServiceLineClaimFieldSelectionList = null;
            ContractServiceTypes contractServiceTypes = null;
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsServiceLineClaimFieldSelection(contractServiceLineClaimFieldSelectionList, contractServiceTypes, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsServiceLineServiceTableSelection
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsServiceLineServiceTableSelectionTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceTypes contractServiceTypes = null;
            List<string> revenueCodes = null;
            Enums.ClaimFieldTypes claimFieldType = new Enums.ClaimFieldTypes();
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsServiceLineServiceTableSelection(contractServiceTypes, revenueCodes, claimFieldType, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsServiceLineTableSelection
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsServiceLineTableSelectionTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            List<ContractServiceLineTableSelection> contractServiceLineTableSelectionList = null;
            ContractServiceTypes contractServiceTypes = null;
            ClaimData claimData = null;
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsServiceLineTableSelection(contractServiceLineTableSelectionList, contractServiceTypes, claimData, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsTableSelectionAvailable
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsTableSelectionAvailableTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ContractServiceLineTableSelection contractServiceLineTableSelection = null;
            ContractServiceTypes contractServiceTypes = null;
            ClaimData claimData = null;
            List<ClaimFieldDocs> docs = null;
            bool expected = false;
            bool actual = target.IsTableSelectionAvailable(contractServiceLineTableSelection, contractServiceTypes, claimData, docs);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for IsValidCurveOutData
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void IsValidCurveOutDataTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            List<ClaimChargeData> claimChargeDataList = null;
            ContractServiceTypes contractServiceType = null;
            bool expected = false;
            bool actual;
            actual = target.IsValidCurveOutData(claimChargeDataList, contractServiceType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for UpdateClaimChargeData
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void UpdateClaimChargeDataTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            Contracts contract = null;
            ClaimData claimData = null;
            ClaimData expected = null;
            ClaimData actual;
            actual = target.UpdateClaimChargeData(contract, claimData);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: TO CHECK
        /// <summary>
        ///A test for UpdateClaimChargeDataBasedOnServiceType
        ///</summary>
        [TestMethod()]
        //[DeploymentItem("MedWorth.ContractManagement.BusinessLogic.dll")]
        public void UpdateClaimChargeDataBasedOnServiceTypeTest()
        {
            MatchServiceType_Accessor target = new MatchServiceType_Accessor();
            ClaimChargeData claimChargeData = null;
            ContractServiceTypes contractServiceType = null;
            int serviceTypeIndex = 0;
            ClaimChargeData expected = null;
            ClaimChargeData actual = target.UpdateClaimChargeDataBasedOnServiceType(claimChargeData, contractServiceType, serviceTypeIndex);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
