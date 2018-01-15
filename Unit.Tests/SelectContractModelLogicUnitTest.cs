using System.Collections.Generic;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    /// Summary description for SelectContractModelLogicUnitTest
    /// </summary>
    [TestClass]
    public class SelectContractModelLogicUnitTest
    {
       

        /// <summary>
        /// Selects the contract model logic constructor test.
        /// </summary>
        [TestMethod]
        public void SelectContractModelLogicConstructorTest()
        {
            var target = new SelectContractModelLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(SelectContractModelLogic));
        }
    
        /// <summary>
        /// Facilities the details logic constructor test1.
        /// </summary>
        [TestMethod]
        public void SelectContractModelLogicConstructorTest1()
        {
            var mockSelectContractModelRepository = new Mock<ISelectContractModelRepository>();
            SelectContractModelLogic target = new SelectContractModelLogic(mockSelectContractModelRepository.Object);
            Assert.IsInstanceOfType(target, typeof(SelectContractModelLogic));
        }


        /// <summary>
        /// Gets all contract facilities test.
        /// </summary>
     [TestMethod]
        public void GetAllContractFacilitiesTest()
        {
            var mockSelectContractModelRepository = new Mock<ISelectContractModelRepository>();
         List<FacilityDetail> result = new List<FacilityDetail>
             {
                 new FacilityDetail {FacilityName = "Facility1", NodeId = 1}
             };
            mockSelectContractModelRepository.Setup(f => f.GetAllContractFacilities(It.IsAny<ContractHierarchy>()))
                                          .Returns(result);
         SelectContractModelLogic target = new SelectContractModelLogic(mockSelectContractModelRepository.Object);

         //Act
         List<FacilityDetail> actual = target.GetAllContractFacilities(null);
            
         //Assert
         Assert.AreEqual(actual, result);
        }

        /// <summary>
        ///A test for GetAllContractModels
        ///</summary>
     [TestMethod]
        public void GetAllContractModelsTest()
        {
            Mock<ISelectContractModelRepository> mockSelectContractModelRepository = new Mock<ISelectContractModelRepository>();
            SelectContractModelLogic target = new SelectContractModelLogic(mockSelectContractModelRepository.Object);
            List<ContractModel> expected = new List<ContractModel> { new ContractModel { FacilityId = 0 } };
            mockSelectContractModelRepository.Setup(f => f.GetAllContractModels(It.IsAny<ContractModel>()))
                                         .Returns(expected);
            const int facilityId = 0;
            List<ContractModel> actual = target.GetAllContractModels(new ContractModel {FacilityId = facilityId});
            Assert.AreEqual(expected, actual);
           
            //Assert
            //Assert.AreEqual(result, actual);
        }

     /// <summary>
     /// Gets all contract models unit test difference not null.
     /// </summary>
     [TestMethod]
     public void GetAllContractModelsUnitTestIfNotNull()
     {
         //Mock Input
         const int contractId = 354;

         //Mock output
         List<ContractModel> result = new List<ContractModel> { new ContractModel { NodeId = 976 },
             new ContractModel { NodeId = 976} };
         var mockGetAllContractModels = new Mock<ISelectContractModelRepository>();
         mockGetAllContractModels.Setup(f => f.GetAllContractModels(It.IsAny<ContractModel>())).Returns(result);
         SelectContractModelLogic target = new SelectContractModelLogic(mockGetAllContractModels.Object);
         List<ContractModel> actual = target.GetAllContractModels(new ContractModel { FacilityId = contractId });
         Assert.AreEqual(result, actual);
     }
    }
}
