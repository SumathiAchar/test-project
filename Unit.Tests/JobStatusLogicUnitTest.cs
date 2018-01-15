using MedWorth.ContractManagement.BusinessLogic;
using MedWorth.ContractManagement.Data.Repository;
using MedWorth.ContractManagement.Shared.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MedWorth.ContractManagement.Shared.Models;
using System.Collections.Generic;

namespace MedWorth.ContractManagement.Unit.Tests
{
    
    
    /// <summary>
    ///This is a test class for JobStatusLogicTest and is intended
    ///to contain all JobStatusLogicTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JobStatusLogicTest
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


        /// <summary>
        ///A test for JobStatusLogic Constructor
        ///</summary>
        [TestMethod()]
        public void JobStatusLogicConstructorTest()
        {
            IJobsStatusRepository jobsStatusLogicRepository = new JobsStatusRepository();
            JobStatusLogic target = new JobStatusLogic();
            Assert.IsInstanceOfType(target, typeof(JobStatusLogic));
        }

        /// <summary>
        ///A test for CreateJob for Not Null
        ///</summary>
        [TestMethod()]
        public void CreateJobTest()
        {
            JobStatusLogic target = new JobStatusLogic();
            long idJob = 0;
            int idUser = 0;
            Job expected = new Job(idJob,idUser);
            Job actual = target.CreateJob(idJob, idUser);
            Assert.AreEqual(expected.GetType(),actual.GetType());
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAllJobs for Not Null
        ///</summary>
        [TestMethod()]
        public void GetAllJobsTest()
        {
            JobStatusLogic target = new JobStatusLogic();
            List<JobsStatus> expected = new List<JobsStatus>
            {
                new JobsStatus{RequestName = "RName1",TaskId = "102",Status = "Pass1"},
                new JobsStatus{RequestName = "RName2",TaskId = "102",Status = "Pass2"},
                new JobsStatus{RequestName = "RName3",TaskId = "103",Status = "Pass3"},
                new JobsStatus{RequestName = "RName4",TaskId = "104",Status = "Pass4"},
                new JobsStatus{RequestName = "RName5",TaskId = "105",Status = "Pass5"}
            };
            List<JobsStatus> actual = target.GetAllJobs();
            Assert.AreEqual(expected.Count, actual.Count);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        
        /// <summary>
        ///A test for GetAllJobsForAdjudication for Not Null
        ///</summary>
        [TestMethod()]
        public void GetAllJobsForAdjudicationTest()
        {
            JobStatusLogic target = new JobStatusLogic();
            List<JobsStatus> expected = new List<JobsStatus>
            {
                new JobsStatus{RequestName = "RName1",TaskId = "101",Status = "Pass1"},
                new JobsStatus{RequestName = "RName2",TaskId = "102",Status = "Pass2"},
                new JobsStatus{RequestName = "RName3",TaskId = "103",Status = "Pass3"},
                new JobsStatus{RequestName = "RName4",TaskId = "104",Status = "Pass4"},
                new JobsStatus{RequestName = "RName5",TaskId = "105",Status = "Pass5"},
                new JobsStatus{RequestName = "RName6",TaskId = "106",Status = "Pass6"},
                new JobsStatus{RequestName = "RName7",TaskId = "107",Status = "Pass7"}
            };
            List<JobsStatus> actual = target.GetAllJobsForAdjudication();
            Assert.AreEqual(expected.Count, actual.Count);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetJob for Null
        ///</summary>
        [TestMethod()]
        public void GetJobNullTest()
        {
            JobStatusLogic target = new JobStatusLogic();
            long idJob = 0;
            Job expected = null;
            Job actual = target.GetJob(idJob);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
        //TODO: To Check
        /// <summary>
        ///A test for GetJob for Not Null
        ///</summary>
        //[TestMethod()]
        //public void GetJobTest()
        //{
        //    JobStatusLogic target = new JobStatusLogic();
        //    long iJob = 1;
        //    int intUser = 0;
        //    Job expected = new Job(iJob,intUser);
        //    Job actual = target.GetJob(iJob);
        //    Assert.AreEqual(expected, actual);
        //    //Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for UpdateJobStatus for Null
        ///</summary>
        [TestMethod()]
        public void UpdateJobStatusTest()
        {
            JobStatusLogic target = new JobStatusLogic();
            JobsStatus job = null;
            int expected = 0;
            int actual = target.UpdateJobStatus(job);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
