/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Service Line Claim Field Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ServiceLineClaimFieldLogicUnitTest
    /// </summary>
    [TestClass]
    public class ServiceLineClaimFieldLogicUnitTest
    {
        /// <summary>
        /// Services the line claim field constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ServiceLineClaimFieldConstructorUnitTest1()
        {
            var target = new ServiceLineClaimFieldLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ServiceLineClaimFieldLogic));
        }

        /// <summary>
        /// Services the line claim field constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ServiceLineClaimFieldConstructorUnitTest2()
        {
            var mockAddNewClaimFieldSelectionCase = new Mock<IServiceLineClaimFieldRepository>();
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockAddNewClaimFieldSelectionCase.Object);
            Assert.IsInstanceOfType(target, typeof (ServiceLineClaimFieldLogic));
        }


        /// <summary>
        /// Adds the new claim field selection difference null logic unit test.
        /// </summary>
        [TestMethod]
        public void AddNewClaimFieldSelectionIfNullLogicUnitTest()
        {
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(Constants.ConnectionString);
            const long expected = 0;
            long actual = target.AddNewClaimFieldSelection(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Edits the claim field selection difference null logic unit test.
        /// </summary>
        [TestMethod]
        public void EditClaimFieldSelectionIfNullLogicUnitTest()
        {
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(Constants.ConnectionString);
            const long expected = 0;
            long actual = target.EditClaimFieldSelection(null);
            Assert.AreEqual(expected, actual);
        }

        
        /// <summary>
        /// Adds the new claim field selection difference if not null.
        /// </summary>
        [TestMethod]
        public void AddNewClaimFieldSelectionLogicUnitTestIfNotNull()
        {

            //Mock input
            List<ContractServiceLineClaimFieldSelection> contractServiceLineTableSelections = new List
                <ContractServiceLineClaimFieldSelection>
                                                                                                  {
                                                                                                      new ContractServiceLineClaimFieldSelection
                                                                                                          {
                                                                                                              ContractId
                                                                                                                  =
                                                                                                                  124589
                                                                                                          },
                                                                                                      new ContractServiceLineClaimFieldSelection
                                                                                                          {
                                                                                                              ContractId
                                                                                                                  =
                                                                                                                  124589
                                                                                                          },
                                                                                                  };

            //Mock setup
            var mockAddServiceLineClaimandTables = new Mock<IServiceLineClaimFieldRepository>();
            mockAddServiceLineClaimandTables.Setup(f => f.AddNewClaimFieldSelection(contractServiceLineTableSelections))
                .Returns(1);
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockAddServiceLineClaimandTables.Object);

            long actual = target.AddNewClaimFieldSelection(contractServiceLineTableSelections);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void EditClaimFieldSelectionTest1()
        {

            //Mock input 
            List<ContractServiceLineClaimFieldSelection> contractServiceLineTableSelection = new List
                <ContractServiceLineClaimFieldSelection>
                                                                                                 {
                                                                                                     new ContractServiceLineClaimFieldSelection
                                                                                                         {
                                                                                                             ContractId
                                                                                                                 = 1247
                                                                                                         },
                                                                                                     new ContractServiceLineClaimFieldSelection
                                                                                                         {
                                                                                                             ContractId
                                                                                                                 = 1247
                                                                                                         },
                                                                                                 };


            //Mock setup
            var mockEditServiceLineTableSelection = new Mock<IServiceLineClaimFieldRepository>();
            mockEditServiceLineTableSelection.Setup(f => f.EditClaimFieldSelection(contractServiceLineTableSelection)).
                Returns(1);
            ServiceLineClaimFieldLogic target = new ServiceLineClaimFieldLogic(mockEditServiceLineTableSelection.Object);

            long actual = target.EditClaimFieldSelection(contractServiceLineTableSelection);
            Assert.AreEqual(1, actual);
        }



    }




}

