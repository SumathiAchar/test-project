/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Service Line TableLookUp Details Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ServiceLineTableLookUpDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class ServiceLineTableLookUpDetailsLogicUnitTest
    {

        /// <summary>
        /// Gets the table look up details by contract identifier if not null.
        /// </summary>
        [TestMethod]
        public void GetTableLookUpDetailsByContractIdIfNotNull()
        {
            //Arrange
            var repository = new Mock<IClaimFieldDocRepository>();
            List<ClaimFieldDoc> result = new List<ClaimFieldDoc>{new ClaimFieldDoc()};
            repository.Setup(f => f.GetClaimFieldDocs(It.IsAny<ClaimFieldDoc>())).Returns(result);
            ClaimFieldDocLogic target = new ClaimFieldDocLogic(repository.Object);

            //Act
            List<ClaimFieldDoc> actual = target.GetClaimFieldDocs(null);

            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Gets the table look up details by contract identifier n ull.
        /// </summary>
        [TestMethod]
        public void GetTableLookUpDetailsByContractIdNUll()
        {
            var mockGetTableLookUpDetailsByContractId = new Mock<IClaimFieldDocRepository>();
            const int actual = 0;
            mockGetTableLookUpDetailsByContractId.Setup(f => f.GetClaimFieldDocs(null)).Returns(new List<ClaimFieldDoc>());
            ClaimFieldDocLogic target = new ClaimFieldDocLogic(mockGetTableLookUpDetailsByContractId.Object);
            List<ClaimFieldDoc> result = target.GetClaimFieldDocs(null);
            Assert.AreEqual(result.Count, actual);
        }
       
   }
}
