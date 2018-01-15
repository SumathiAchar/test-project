using System.Threading.Tasks;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using System.Collections.Generic;

namespace MedWorth.ContractManagement.Unit.Tests
{
    /// <summary>
    ///This is a test class for JobStatusLogicTest and is intended
    ///to contain all JobStatusLogicTest Unit Tests
    ///</summary>
    [TestClass]
    public class JobStatusLogicTest
    {
        private static JobStatusLogic _target;
        /// <summary>
        ///A test for CreateJob
        ///</summary>
        [TestMethod]
        public void CreateJobTest()
        {
            JobStatusLogic target = new JobStatusLogic(Constants.ConnectionString);
            const long idJob = 0;
            Job actual = target.CreateJob(idJob);
            Assert.AreEqual(actual.TaskId, idJob);
        }

        /// <summary>
        ///A test for GetAllJobs
        ///</summary>
        [TestMethod]
        public void GetAllJobsTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.GetAllJobs(It.IsAny<TrackTask>())).Returns(new List<TrackTask>());

            new JobStatusLogic(jobStatusRepository.Object).GetAllJobs(new TrackTask());
            jobStatusRepository.VerifyAll();
        }


        /// <summary>
        ///A test for UpdateJobStatus
        ///</summary>
        [TestMethod]
        public void UpdateJobStatusTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.UpdateJobStatus(It.IsAny<TrackTask>())).Returns(1);

            new JobStatusLogic(jobStatusRepository.Object).UpdateJobStatus(new TrackTask());
            jobStatusRepository.VerifyAll();

        }

        /// <summary>
        ///A test for Stop
        ///</summary>
        [TestMethod]
        public void StopTest()
        {
            const long idJob = 0;
            Job target = new Job(idJob, new Task(() => { }));
            target.Stop();
        }

        /// <summary>
        ///A test for Resume
        ///</summary>
        [TestMethod]
        public void ResumeTest()
        {
            const long idJob = 0;
            Job target = new Job(idJob);
            target.Resume();
        }

        /// <summary>
        ///A test for Pause
        ///</summary>
        [TestMethod]
        public void PauseTest()
        {
            const long idJob = 0;
            Job target = new Job(idJob);
            target.Pause();
        }

        /// <summary>
        ///A test for RunJob
        ///</summary>
        [TestMethod]
        public void RunJobTest()
        {
            const long idJob = 0;
            Job target = new Job(idJob);
            const long taskId = 0;
            target.RunJob(taskId, 1, Constants.ConnectionString);
        }

        [TestMethod]
        public void JobCountAlertTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.JobCountAlert(It.IsAny<TrackTask>())).Returns(1);

            new JobStatusLogic(jobStatusRepository.Object).JobCountAlert(new TrackTask());
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateJobVerifiedStatusTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.UpdateJobVerifiedStatus(It.IsAny<TrackTask>())).Returns(true);

            new JobStatusLogic(jobStatusRepository.Object).UpdateJobVerifiedStatus(new TrackTask());
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void IsManualAdjudicationRunningTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.IsManualAdjudicationRunning()).Returns(true);

            new JobStatusLogic(jobStatusRepository.Object).IsManualAdjudicationRunning();
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void CleanupCancelledTasksTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.CleanupCancelledTasks()).Returns(true);

            new JobStatusLogic(jobStatusRepository.Object).CleanupCancelledTasks();
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void IsModelExistTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.IsModelExist(It.IsAny<TrackTask>())).Returns(true);

            new JobStatusLogic(jobStatusRepository.Object).IsModelExist(new TrackTask());
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void ReAdjudicateTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.ReAdjudicate(It.IsAny<TrackTask>())).Returns(1);

            new JobStatusLogic(jobStatusRepository.Object).ReAdjudicate(new TrackTask());
            jobStatusRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllJobsForAdjudicationTest()
        {
            Mock<IJobStatusRepository> jobStatusRepository = new Mock<IJobStatusRepository>();
            jobStatusRepository.Setup(x => x.GetAllJobsForAdjudication(It.IsAny<string>())).Returns(new List<TrackTask>());
            new JobStatusLogic(jobStatusRepository.Object).GetAllJobsForAdjudication();
            jobStatusRepository.VerifyAll();
        }

        /// <summary>
        /// Gets the All Available and Selected Claim Column List with LoggedIn UserId and unit test.
        /// </summary>
        [TestMethod]
        public void GetOpenClaimColumnOptionByUserId()
        {
            var jobStatusRepository = new Mock<IJobStatusRepository>();

            // Arrange
            ClaimColumnOptions claimColumnOptions = new ClaimColumnOptions
            {
                UserId = 4178
            };
            List<ClaimColumnOptions> availableColumnList = new List<ClaimColumnOptions>
            {
                new ClaimColumnOptions
                {
                    ClaimColumnOptionId = 1,
                    ColumnName = "DRG"
                }
            };
            List<ClaimColumnOptions> selectedColumnList = new List<ClaimColumnOptions>
            {
                new ClaimColumnOptions
                {
                    ClaimColumnOptionId = 5,
                    ColumnName = "BillDate"
                }
            };
            ClaimColumnOptions expectedResult = new ClaimColumnOptions
            {
                AvailableColumnList = availableColumnList,
                SelectedColumnList = selectedColumnList
            };
            jobStatusRepository.Setup(f => f.GetOpenClaimColumnOptionByUserId(claimColumnOptions)).Returns(expectedResult);
            _target = new JobStatusLogic(jobStatusRepository.Object);

            //Act
            ClaimColumnOptions actual = _target.GetOpenClaimColumnOptionByUserId(claimColumnOptions);

            // Assert
            Assert.AreEqual(expectedResult.AvailableColumnList, actual.AvailableColumnList);
            Assert.AreEqual(expectedResult.SelectedColumnList, actual.SelectedColumnList);
        }

        /// <summary>
        /// Gets the All Available and Selected Claim Column List with Negative LoggedIn UserId and unit test.
        /// </summary>
        [TestMethod]
        public void GetAdjudicationColumnsByNegativeLoggedInUserId()
        {
            var jobStatusRepository = new Mock<IJobStatusRepository>();

            // Arrange
            ClaimColumnOptions claimColumnOptions = new ClaimColumnOptions
            {
                UserId = 0
            };
            List<ClaimColumnOptions> availableColumnList = new List<ClaimColumnOptions>
            {
                new ClaimColumnOptions
                {
                    ClaimColumnOptionId = 0,
                    ColumnName = ""
                }
            };
            List<ClaimColumnOptions> selectedColumnList = new List<ClaimColumnOptions>
            {
                new ClaimColumnOptions
                {
                    ClaimColumnOptionId = 0,
                    ColumnName = ""
                }
            };
            ClaimColumnOptions expectedResult = new ClaimColumnOptions
            {
                AvailableColumnList = availableColumnList,
                SelectedColumnList = selectedColumnList
            };
            jobStatusRepository.Setup(f => f.GetOpenClaimColumnOptionByUserId(claimColumnOptions)).Returns(expectedResult);
            _target = new JobStatusLogic(jobStatusRepository.Object);

            //Act
            ClaimColumnOptions actual = _target.GetOpenClaimColumnOptionByUserId(claimColumnOptions);

            // Assert
            Assert.AreEqual(expectedResult.AvailableColumnList, actual.AvailableColumnList);
            Assert.AreEqual(expectedResult.SelectedColumnList, actual.SelectedColumnList);
        }

        //Fixed-2016-R3-S2 : Expected object is not correct. Method always returns false.
        /// <summary>
        /// Save ClaimColumnOptions.
        /// </summary>
        [TestMethod]
        public void SaveClaimColumnOptions()
        {
            var jobStatusRepository = new Mock<IJobStatusRepository>();

            // Arrange
            ClaimColumnOptions claimColumnsInfo = new ClaimColumnOptions
            {
                ClaimColumnOptionIds = "1,2,3",
                UserId = 4178
            };

            const bool expectedResult = true;

            jobStatusRepository.Setup(f => f.SaveClaimColumnOptions(claimColumnsInfo)).Returns(expectedResult);
            _target = new JobStatusLogic(jobStatusRepository.Object);

            //Act
            bool actual = _target.SaveClaimColumnOptions(claimColumnsInfo);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save ClaimColumnOption By UserId Is Zero.
        /// </summary>
        [TestMethod]
        public void SaveClaimColumnOptionByUserIdIsZero()
        {
            var jobStatusRepository = new Mock<IJobStatusRepository>();

            // Arrange
            ClaimColumnOptions claimColumnsInfo = new ClaimColumnOptions
            {
                ClaimColumnOptionIds = "1,2,3",
                UserId = 0
            };

            const bool expectedResult = false;

            jobStatusRepository.Setup(f => f.SaveClaimColumnOptions(claimColumnsInfo)).Returns(expectedResult);
            _target = new JobStatusLogic(jobStatusRepository.Object);

            //Act
            bool actual = _target.SaveClaimColumnOptions(claimColumnsInfo);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Save ClaimColumnOptionId Is NUll.
        /// </summary>
        [TestMethod]
        public void SaveClaimColumnOptionClaimColumnOptionIdIsNull()
        {
            var jobStatusRepository = new Mock<IJobStatusRepository>();

            // Arrange
            ClaimColumnOptions claimColumnsInfo = new ClaimColumnOptions
            {
                ClaimColumnOptionIds = " ",
                UserId = 4178
            };

            const bool expectedResult = false;

            jobStatusRepository.Setup(f => f.SaveClaimColumnOptions(claimColumnsInfo)).Returns(expectedResult);
            _target = new JobStatusLogic(jobStatusRepository.Object);

            //Act
            bool actual = _target.SaveClaimColumnOptions(claimColumnsInfo);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }
    }
}
