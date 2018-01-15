/************************************************************************************************************/
/**  Author         : Ragini
/**  Created        : 24-Aug-2013
/**  Summary        : Handles Service Line Code Details Testing
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using Moq;
using System.Collections.Generic;


namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for ServiceLineCodeDetailsLogicUnitTest
    /// </summary>
    [TestClass]
    public class ServiceLineCodeDetailsLogicUnitTest
    {

        /// <summary>
        /// Services the line code details logic constructor unit test1.
        /// </summary>
        [TestMethod]
        public void ServiceLineCodeDetailsLogicConstructorUnitTest1()
        {
            var target = new ServiceLineCodeLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ServiceLineCodeLogic));
        }

        /// <summary>
        /// Services the line code details logic constructor unit test2.
        /// </summary>
        [TestMethod]
        public void ServiceLineCodeDetailsLogicConstructorUnitTest2()
        {
            var mockServiceLineCodeDetailsLogic = new Mock<IServiceLineCodeRepository>();
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockServiceLineCodeDetailsLogic.Object);
            Assert.IsInstanceOfType(target, typeof(ServiceLineCodeLogic));
        }

        /// <summary>
        /// Adds the service line code details difference null.
        /// </summary>
        [TestMethod]
        public void AddEditServiceLineCodeDetailsIfNull()
        {
            var mockAddServiceLineCodeDetails = new Mock<IServiceLineCodeRepository>();
            mockAddServiceLineCodeDetails.Setup(f => f.AddEditServiceLineCodeDetails(It.IsAny<ContractServiceLine>())).Returns(0);
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockAddServiceLineCodeDetails.Object);
            long actual = target.AddEditServiceLineCodeDetails(null);
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        /// Adds the service line code detailsif not null.
        /// </summary>
        [TestMethod]
        public void AddEditServiceLineCodeDetailsifNotNull()
        {
            var mockAddServiceLineCodeDetails = new Mock<IServiceLineCodeRepository>();
            mockAddServiceLineCodeDetails.Setup(f => f.AddEditServiceLineCodeDetails(It.IsAny<ContractServiceLine>())).Returns(2);
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockAddServiceLineCodeDetails.Object);
            ContractServiceLine objServiceLineCodeDetails = new ContractServiceLine { ContractServiceLineId = 1 };
            long actual = target.AddEditServiceLineCodeDetails(objServiceLineCodeDetails);
            Assert.AreEqual(2, actual);

        }

        /// <summary>
        /// Gets the service line code details unit test if not null.
        /// </summary>
        [TestMethod]
        public void GetServiceLineCodeDetailsUnitTestIfNotNull()
        {
            //Mock Input
            ContractServiceLine inputData = new ContractServiceLine { ContractId = 267, ContractServiceTypeId = null, ServiceLineId = 2 };

            //Mock output
            ContractServiceLine getServiceLineCodeDetails = new ContractServiceLine { IncludedCode = "234", ExcludedCode = "745", ContractServiceLineId = 756 };

            var mockGetServiceLineCodeDetails = new Mock<IServiceLineCodeRepository>();
            mockGetServiceLineCodeDetails.Setup(f => f.GetServiceLineCodeDetails(inputData)).Returns(getServiceLineCodeDetails);
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockGetServiceLineCodeDetails.Object);
            ContractServiceLine actual = target.GetServiceLineCodeDetails(inputData);
            Assert.AreEqual(getServiceLineCodeDetails, actual);
        }


        ///// <summary>
        ///// Gets all service line code details by contract unique identifier difference not null.
        ///// </summary>
        [TestMethod]
        public void GetAllServiceLineCodeDetailsByContractIdIfNotNull()
        {

            //Mock Input
            const long contractId = 1234;

            //Mock output
            List<ContractServiceLine> serviceLineDetails = new List<ContractServiceLine>
            {
               new ContractServiceLine{ ContractId=1234,
                IncludedCode="include1",
                Description="testDescription1",
                ServiceLineId=143
               },
                new ContractServiceLine{ ContractId=1234,
                IncludedCode="include2",
                Description="testDescription2",
                ServiceLineId=142
               }

            };

            //Mock Setup
            var mockGetAllServiceLineCodeDetailsByContractId = new Mock<IServiceLineCodeRepository>();
            mockGetAllServiceLineCodeDetailsByContractId.Setup(f => f.GetAllServiceLineCodeDetailsByContractId(It.IsAny<long>())).Returns(serviceLineDetails);
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockGetAllServiceLineCodeDetailsByContractId.Object);
            List<ContractServiceLine> actual = target.GetAllServiceLineCodeDetailsByContractId(contractId);
            Assert.AreEqual(actual, serviceLineDetails);
        }



        /// <summary>
        /// Gets all service line codes difference not null.
        /// </summary>
        [TestMethod]
        public void GetAllServiceLineCodesIfNotNull()
        {

            //Mock Input
            const long serviceLineTypeId = 1234;

            //Mock output
            List<ServiceLineCode> serviceLineCodeDetails = new List<ServiceLineCode>
            {
               new ServiceLineCode{ 
                   CodeString="Abc123",
                   Description="Drug1"
               },
                new ServiceLineCode{ 
                     CodeString="Abc123",
                   Description="Drug2"
               }

            };

            //Mock Setup
            var mockGetAllServiceLineCodes = new Mock<IServiceLineCodeRepository>();
            mockGetAllServiceLineCodes.Setup(f => f.GetAllServiceLineCodes(It.IsAny<long>(), 1, 10)).Returns(serviceLineCodeDetails);
            ServiceLineCodeLogic target = new ServiceLineCodeLogic(mockGetAllServiceLineCodes.Object);
            List<ServiceLineCode> actual = target.GetAllServiceLineCodes(serviceLineTypeId, 1, 10);
            Assert.AreEqual(actual, serviceLineCodeDetails);
        }

        /// <summary>
        /// Gets the service line code details unit test pass input as null.
        /// </summary>
        [TestMethod]
        public void GetServiceLineCodeDetailsUnitTestforNull()
        {
            // Arrange
            var mockGetServiceLineCodeDetails = new Mock<IServiceLineCodeRepository>();
            mockGetServiceLineCodeDetails.Setup(f => f.GetServiceLineCodeDetails(It.IsAny<ContractServiceLine>())).Returns((ContractServiceLine)null);
            var target = new ServiceLineCodeLogic(mockGetServiceLineCodeDetails.Object);

            // Act
            ContractServiceLine actual = target.GetServiceLineCodeDetails(null);

            // Assert
            Assert.IsNull(actual);
        }
    }
}
