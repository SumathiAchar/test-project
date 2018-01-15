/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Service Line Table Selection Details Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;
using System.Collections.Generic;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ServiceLineTableSelectionDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class ServiceLineTableSelectionDetailsLogicUnitTest
    {
        /// <summary>
        /// Services the line table selection constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ServiceLineTableSelectionConstructorUnitTest1()
        {
            IServiceLineTableSelectionRepository serviceLineTableSelectionRepository = new ServiceLineTableSelectionRepository(Constants.ConnectionString);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(serviceLineTableSelectionRepository);
            Assert.IsInstanceOfType(target, typeof(ServiceLineTableSelectionLogic));
        }
        /// <summary>
        /// Services the line table selection constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ServiceLineTableSelectionConstructorUnitTest2()
        {
            var mockserviceLineTableSelectionRepository = new Mock<IServiceLineTableSelectionRepository>();
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockserviceLineTableSelectionRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ServiceLineTableSelectionLogic));
        }

        /// <summary>
        /// Gets the service line table selection if null.
        /// </summary>
        [TestMethod]
        public void GetServiceLineTableSelectionifNull()
        {

            //Mock input 
            ContractServiceLineTableSelection contractServiceLineTableSelectionDetails = null;
            //Mock Output
            //Mock setup
            var mockGetServiceLineTableSelection = new Mock<IServiceLineTableSelectionRepository>();
            mockGetServiceLineTableSelection.Setup(f => f.GetServiceLineTableSelection(contractServiceLineTableSelectionDetails)).Returns((List<ContractServiceLineTableSelection>)null);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockGetServiceLineTableSelection.Object);

            List<ContractServiceLineTableSelection> actual = target.GetServiceLineTableSelection(null);
            Assert.IsNull(actual);
        }


        /// <summary>
        /// Gets the service line table selection if not null.
        /// </summary>
        [TestMethod]
        public void GetServiceLineTableSelectionifNotNull()
        {

            //Mock input 
            ContractServiceLineTableSelection contractServiceLineTableSelectionDetails = new ContractServiceLineTableSelection
            {
                ContractId = 124589,
                ServiceLineTypeId = 1
            };

            //Mock Output
            List<ContractServiceLineTableSelection> contractServiceLineTableSelection = new List<ContractServiceLineTableSelection>
            {
                new ContractServiceLineTableSelection{ContractId=124589,ContractServiceLineTableSelectionId=13698},
                new ContractServiceLineTableSelection{ContractId=124589,ContractServiceLineTableSelectionId=1369636},
            };

            //Mock setup
            var mockGetServiceLineTableSelection = new Mock<IServiceLineTableSelectionRepository>();
            mockGetServiceLineTableSelection.Setup(f => f.GetServiceLineTableSelection(contractServiceLineTableSelectionDetails)).Returns(contractServiceLineTableSelection);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockGetServiceLineTableSelection.Object);

            List<ContractServiceLineTableSelection> actual = target.GetServiceLineTableSelection(contractServiceLineTableSelectionDetails);
            Assert.AreEqual(2, actual.Count);
        }


        /// <summary>
        /// Adds the service line claimand tables if null.
        /// </summary>
        [TestMethod]
        public void AddEditServiceLineClaimandTablesIfNull()
        {

            //Mock input 
            List<ContractServiceLineTableSelection> contractServiceLineTableSelections = null;

            //Mock setup
            var mockAddServiceLineClaimandTables = new Mock<IServiceLineTableSelectionRepository>();
            mockAddServiceLineClaimandTables.Setup(f => f.AddEditServiceLineClaimAndTables(contractServiceLineTableSelections)).Returns(0);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockAddServiceLineClaimandTables.Object);

            long actual = target.AddEditServiceLineClaimAndTables(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Adds the service line claimand tables if not null.
        /// </summary>
        [TestMethod]
        public void AddServiceLineClaimandTablesIfNotNull()
        {

            //Mock input
            List<ContractServiceLineTableSelection> contractServiceLineTableSelections = new List<ContractServiceLineTableSelection>
            {
                new ContractServiceLineTableSelection{ContractId=124589,ContractServiceLineTableSelectionId=13698},
                new ContractServiceLineTableSelection{ContractId=124589,ContractServiceLineTableSelectionId=1369636},
            };

            //Mock setup
            var mockAddServiceLineClaimandTables = new Mock<IServiceLineTableSelectionRepository>();
            mockAddServiceLineClaimandTables.Setup(f => f.AddEditServiceLineClaimAndTables(contractServiceLineTableSelections)).Returns(1);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockAddServiceLineClaimandTables.Object);

            long actual = target.AddEditServiceLineClaimAndTables(contractServiceLineTableSelections);
            Assert.AreEqual(1, actual);
        }



        /// <summary>
        /// Gets the claim fieldand tables if null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldandTablesIfNull()
        {

            //Mock input
            ContractServiceLineTableSelection contractServiceLineTableSelections = new ContractServiceLineTableSelection();

            //Mock Output
            List<ClaimField> claimFields = new List<ClaimField>();

            //Mock setup
            var mockGetClaimFieldandTables = new Mock<IServiceLineTableSelectionRepository>();
            mockGetClaimFieldandTables.Setup(f => f.GetClaimFieldAndTables(contractServiceLineTableSelections)).Returns(claimFields);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockGetClaimFieldandTables.Object);

            List<ClaimField> actual = target.GetClaimFieldAndTables(null);
            Assert.IsNull(actual);
        }

        /// <summary>
        /// Gets the claim fieldand tables if not null.
        /// </summary>
        [TestMethod]
        public void GetClaimFieldandTablesIfNotNull()
        {

            //Mock input
            ContractServiceLineTableSelection contractServiceLineTableSelections = new ContractServiceLineTableSelection
            {
                ContractId = 12569,
                ContractServiceTypeId = 1,
                ContractServiceLineTableSelectionId = 1045

            };

            //Mock Output
            List<ClaimField> claimFields = new List<ClaimField>
                  {
                    new ClaimField{  ClaimFieldId=125,Text="test1", ClaimFieldDocId=14589,TableName="testing"},
                    new ClaimField{  ClaimFieldId=1251, Text="test2",  ClaimFieldDocId=145892, TableName="testing2"}

                  };

            //Mock setup
            var mockGetClaimFieldandTables = new Mock<IServiceLineTableSelectionRepository>();
            mockGetClaimFieldandTables.Setup(f => f.GetClaimFieldAndTables(contractServiceLineTableSelections)).Returns(claimFields);
            ServiceLineTableSelectionLogic target = new ServiceLineTableSelectionLogic(mockGetClaimFieldandTables.Object);

            List<ClaimField> actual = target.GetClaimFieldAndTables(contractServiceLineTableSelections);
            Assert.AreEqual(2, actual.Count);
        }

        /// <summary>
        /// Gets the table selection claim field operators test.
        /// </summary>
        [TestMethod]
        public void GetTableSelectionClaimFieldOperatorsTest()
        {
            Mock<IServiceLineTableSelectionRepository> mockServiceLineTableSelectionRepository = new Mock<IServiceLineTableSelectionRepository>();

            ServiceLineTableSelectionLogic target=new ServiceLineTableSelectionLogic(mockServiceLineTableSelectionRepository.Object);
            mockServiceLineTableSelectionRepository.Setup(x => x.GetTableSelectionClaimFieldOperators())
                .Returns(new List<ClaimFieldOperator>());

            var actual = target.GetTableSelectionClaimFieldOperators();

            Assert.IsInstanceOfType(actual, typeof(List<ClaimFieldOperator>));
        }

    }
}
