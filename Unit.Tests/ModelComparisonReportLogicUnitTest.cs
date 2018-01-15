using System;
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
    /// 
    /// </summary>
    [TestClass]
    public class ModelComparisonReportLogicUnitTest
    {
        /// <summary>
        /// Models the comparison report logic paremeterless constructor.
        /// </summary>
        [TestMethod]
        public void ModelComparisonReportLogicParemeterlessConstructor()
        {
            var target = new ModelComparisonReportLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ModelComparisonReportLogic));
        }

        /// <summary>
        /// Models the comparison report logic constructor.
        /// </summary>
        [TestMethod]
        public void ModelComparisonReportLogicConstructor()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            ModelComparisonReportLogic target = new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            Assert.IsInstanceOfType(target, typeof(ModelComparisonReportLogic));
        }


        /// <summary>
        /// Gets the available models test.
        /// </summary>
        [TestMethod]
        public void GetModelsTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            var value = new ModelComparisonReport { FacilityId = 1016 };
            var result = new List<ModelComparisonReport> { new ModelComparisonReport() };
            mockModelComparisonReportRepository.Setup(f => f.GetModels(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target = new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            List<ModelComparisonReport> actual = target.GetModels(value);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Empties the model list when facility identifier not present.
        /// </summary>
        [TestMethod]
        public void EmptyModelListWhenFacilityIdNotPresent()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            var value = new ModelComparisonReport();
            var result = new List<ModelComparisonReport> { new ModelComparisonReport() };
            mockModelComparisonReportRepository.Setup(f => f.GetModels(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target = new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            List<ModelComparisonReport> actual = target.GetModels(value);
            Assert.AreEqual(result, actual);
        }

        
        /// <summary>
        /// Generates the model comparison report test.
        /// </summary>
        [TestMethod]
        public void GenerateModelComparisonReportTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            ModelComparisonReport value = new ModelComparisonReport
            {
                FacilityId = 1,
                SelectedModelList = "287103,11672",
                DetailSelectValue = 1,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                ClaimSearchCriteria = "-99|2|Test"
            };
            EvaluateableClaim claimData = new EvaluateableClaim
            {
                NodeText = "Testing",
                ModelId = 123
            };

            List<ModelComparisonReportDetails> modelComparisonReportDetailsList = new List
                <ModelComparisonReportDetails>
            {
                new ModelComparisonReportDetails
                {
                    ClaimData = claimData
                }
            };
            ModelComparisonReport result = new ModelComparisonReport
            {
                ModelComparisonData = modelComparisonReportDetailsList
            };

            mockModelComparisonReportRepository.Setup(
                f => f.Generate(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target =
                new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            ModelComparisonReport actual = target.Generate(value);
            Assert.AreEqual(actual, result);
        }

        /// <summary>
        /// Generates the model comparison report test.
        /// </summary>
        [TestMethod]
        public void GenerateModelComparisonReportNoDataTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            var value = new ModelComparisonReport();
            var result = new ModelComparisonReport();
            mockModelComparisonReportRepository.Setup(f => f.Generate(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target = new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            ModelComparisonReport actual = target.Generate(value);
            Assert.AreEqual(actual, result);
        }

        /// <summary>
        /// Generates the model comparison report no dates test.
        /// </summary>
        [TestMethod]
        public void GenerateModelComparisonReportNoDatesTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            ModelComparisonReport value = new ModelComparisonReport
            {
                FacilityId = 1,
                SelectedModelList = "287103,11672",
                DetailSelectValue = 1
            };
            EvaluateableClaim claimData = new EvaluateableClaim
            {
                NodeText = "Testing",
                ModelId = 123
            };

            List<ModelComparisonReportDetails> modelComparisonReportDetailsList = new List
                <ModelComparisonReportDetails>
            {
                new ModelComparisonReportDetails
                {
                    ClaimData = claimData
                }
            };
            ModelComparisonReport result = new ModelComparisonReport
            {
                ModelComparisonData = modelComparisonReportDetailsList
            };

            mockModelComparisonReportRepository.Setup(
                f => f.Generate(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target =
                new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            ModelComparisonReport actual = target.Generate(value);
            Assert.AreEqual(actual, result);
        }

        /// <summary>
        /// Generates the model comparison report no search criteria test.
        /// </summary>
        [TestMethod]
        public void GenerateModelComparisonReportNoSearchCriteriaTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            ModelComparisonReport value = new ModelComparisonReport
            {
                FacilityId = 1,
                SelectedModelList = "287103,11672",
                DetailSelectValue = 1,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
            };
            EvaluateableClaim claimData = new EvaluateableClaim
            {
                NodeText = "Testing",
                ModelId = 123
            };

            List<ModelComparisonReportDetails> modelComparisonReportDetailsList = new List
                <ModelComparisonReportDetails>
            {
                new ModelComparisonReportDetails
                {
                    ClaimData = claimData
                }
            };
            ModelComparisonReport result = new ModelComparisonReport
            {
                ModelComparisonData = modelComparisonReportDetailsList
            };

            mockModelComparisonReportRepository.Setup(
                f => f.Generate(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target =
                new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            ModelComparisonReport actual = target.Generate(value);
            Assert.AreEqual(actual, result);
        }

        /// <summary>
        /// Generates the model comparison report search criteria test.
        /// </summary>
        [TestMethod]
        public void GenerateModelComparisonReportSearchCriteriaTest()
        {
            var mockModelComparisonReportRepository = new Mock<IModelComparisonReportRepository>();
            ModelComparisonReport value = new ModelComparisonReport
            {
                FacilityId = 1,
                SelectedModelList = "287103,11672",
                DetailSelectValue = 1,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue,
                ClaimSearchCriteria = "-1|2|Test"
            };
            EvaluateableClaim claimData = new EvaluateableClaim
            {
                NodeText = "Testing",
                ModelId = 123
            };

            List<ModelComparisonReportDetails> modelComparisonReportDetailsList = new List
                <ModelComparisonReportDetails>
            {
                new ModelComparisonReportDetails
                {
                    ClaimData = claimData
                }
            };
            ModelComparisonReport result = new ModelComparisonReport
            {
                ModelComparisonData = modelComparisonReportDetailsList
            };

            mockModelComparisonReportRepository.Setup(
                f => f.Generate(It.IsAny<ModelComparisonReport>())).Returns(result);
            ModelComparisonReportLogic target =
                new ModelComparisonReportLogic(mockModelComparisonReportRepository.Object);
            ModelComparisonReport actual = target.Generate(value);
            Assert.AreEqual(actual, result);
        }
    }
}
