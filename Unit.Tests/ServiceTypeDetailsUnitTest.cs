/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 05-Sept-2013
/**  Summary        : Handles Service Type Details
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Helpers;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ServiceTypeDetailsUnitTest
    /// </summary>
    [TestClass]
    public class ServiceTypeDetailsUnitTest
    {
       /// <summary>
        /// Services the line table selection details logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ServiceLineTableSelectionDetailsLogicConstructorUnitTest1()
        {
            var target = new ContractServiceTypeLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractServiceTypeLogic));
        }

        /// <summary>
        /// Services the line table selection details logic constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ServiceLineTableSelectionDetailsLogicConstructorUnitTest2()
        {
            ContractServiceTypeLogic target = new ContractServiceTypeLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractServiceTypeLogic));
        }

        

    }
}
