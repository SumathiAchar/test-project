using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;

namespace MedWorth.ContractManagement.Unit.Tests
{


    /// <summary>
    ///This is a test class for ContractHierarchyTest and is intended
    ///to contain all ContractHierarchyTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractHierarchyTest
    {
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
        ///A test for ContractHierarchy Constructor
        ///</summary>
        [TestMethod]
        public void ContractHierarchyConstructorTest()
        {
            var target = new ContractHierarchyLogic(Constants.ConnectionString);
            //Assert
            Assert.IsInstanceOfType(target, typeof(ContractHierarchyLogic));
        }


        /// <summary>
        /// Contracts the hierarchy constructor test2.
        /// </summary>
        [TestMethod]
        public void ContractHierarchyConstructorTest2()
        {
            Mock<IContractHierarchyRepository> mockVarianceReportLogic = new Mock<IContractHierarchyRepository>();
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockVarianceReportLogic.Object);
            Assert.IsInstanceOfType(target, typeof(ContractHierarchyLogic));
        }

        /// <summary>
        ///A test for AppendString
        ///</summary>
        [TestMethod]
        public void AppendStringTest()
        {
            ContractHierarchy target = new ContractHierarchy();
            string expected = string.Empty;
            target.AppendString = expected;
            string actual = target.AppendString;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ContractId
        ///</summary>
        [TestMethod]
        public void ContractIdTest()
        {
            ContractHierarchy target = new ContractHierarchy();
            const long expected = 0;
            target.ContractId = expected;
            long actual = target.ContractId;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ContractServiceTypeId
        ///</summary>
        [TestMethod]
        public void ContractServiceTypeIdTest()
        {
            ContractHierarchy target = new ContractHierarchy();
            const long expected = 0;
            target.ContractServiceTypeId = expected;
            long actual = target.ContractServiceTypeId;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for IsContract
        ///</summary>
        [TestMethod]
        public void IsContractTest()
        {
            ContractHierarchy target = new ContractHierarchy { IsContract = null };
            bool? actual = target.IsContract;
            Assert.AreEqual(null, actual);

        }

        /// <summary>
        ///A test for IsPrimaryNode
        ///</summary>
        [TestMethod]
        public void IsPrimaryNodeTest()
        {
            ContractHierarchy target = new ContractHierarchy { IsPrimaryNode = false };
            bool actual = target.IsPrimaryNode;
            Assert.AreEqual(false, actual);

        }

        /// <summary>
        ///A test for NodeId
        ///</summary>
        [TestMethod]
        public void NodeIdTest()
        {
            ContractHierarchy target = new ContractHierarchy();
            const long expected = 0;
            target.NodeId = expected;
            long actual = target.NodeId;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for NodeText
        ///</summary>
        [TestMethod]
        public void NodeTextTest()
        {
            ContractHierarchy target = new ContractHierarchy();
            string expected = string.Empty;
            target.NodeText = expected;
            string actual = target.NodeText;
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ParentId
        ///</summary>
        [TestMethod]
        public void ParentIdTest()
        {
            ContractHierarchy target = new ContractHierarchy { ParentId = null };
            long? actual = target.ParentId;
            Assert.AreEqual(null, actual);

        }

        /// <summary>
        ///A test for nodes
        ///</summary>
        [TestMethod]
        public void NodesTest()
        {
            ContractHierarchy target = new ContractHierarchy { Nodes = null };
            List<ContractHierarchy> actual = target.Nodes;
            Assert.AreEqual(null, actual);

        }


        /// <summary>
        ///A test for CopyContractByNodeAndParentId if Null
        ///</summary>
        [TestMethod]
        public void CopyContractByNodeAndParentIdTestNull()
        {
            ContractHierarchy objContractHierarchy = null;
            var mockCopyContractByNodeAndParentId = new Mock<IContractHierarchyRepository>();
            mockCopyContractByNodeAndParentId.Setup(f => f.CopyNode(objContractHierarchy)).Returns(1);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockCopyContractByNodeAndParentId.Object);

            long actual = target.CopyNode(null);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for CopyContractByNodeAndParentId if Not Null
        ///</summary>
        [TestMethod]
        public void CopyContractByNodeAndParentIdTestNotNull()
        {
            ContractHierarchy objContractHierarchy = new ContractHierarchy { NodeText = "Test" };
            var mockCopyContractByNodeAndParentId = new Mock<IContractHierarchyRepository>();
            mockCopyContractByNodeAndParentId.Setup(f => f.CopyNode(objContractHierarchy)).Returns(1);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockCopyContractByNodeAndParentId.Object);

            long actual = target.CopyNode(objContractHierarchy);
            Assert.IsNotNull(1 / actual);
        }
       
        /// <summary>
        /// Copies the contract null test.
        /// </summary>
        [TestMethod]
        public void CopyContractNullTest()
        {
            var repository = new Mock<IContractHierarchyRepository>();
            const long result = 0;
            repository.Setup(f => f.CopyContract(null));
            ContractHierarchyLogic target = new ContractHierarchyLogic(repository.Object);

            long actual = target.CopyContract(null);
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Ifs the model name is unique.
        /// </summary>
        [TestMethod]
        public void IfModelNameIsUnique()
        {
            //Arrange
            Mock<IContractHierarchyRepository> mockContractHierarchyRepository = new Mock<IContractHierarchyRepository>();

            mockContractHierarchyRepository.Setup(f => f.IsModelNameExit(It.IsAny<ContractHierarchy>())).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchyRepository.Object);
            const bool result = true;
            //Act
            bool actual = target.IsModelNameExit(new ContractHierarchy());
            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Ifs the model name is not unique.
        /// </summary>
        [TestMethod]
        public void IfModelNameIsNotUnique()
        {
            //Arrange
            Mock<IContractHierarchyRepository> mockContractHierarchyRepository = new Mock<IContractHierarchyRepository>();

            mockContractHierarchyRepository.Setup(f => f.IsModelNameExit(It.IsAny<ContractHierarchy>())).Returns(false);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchyRepository.Object);
            const bool result = false;

            //Act
            bool actual = target.IsModelNameExit(new ContractHierarchy());
            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Ifs the model name is not unique.
        /// </summary>
        [TestMethod]
        public void CheckModelNameIsNotUniqueIfContractHierarchyIsNull()
        {
            //Arrange
            Mock<IContractHierarchyRepository> mockContractHierarchyRepository = new Mock<IContractHierarchyRepository>();

            mockContractHierarchyRepository.Setup(f => f.IsModelNameExit(null)).Returns(false);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchyRepository.Object);
            const bool result = false;

            //Act
            bool actual = target.IsModelNameExit(null);
            //Assert
            Assert.AreEqual(result, actual);
        }

        /// <summary>
        /// Delete Contract Service Type ById
        /// </summary>
        [TestMethod]
        public void DeleteContractServiceTypeByIdMockTest1()
        {
            var mockDeleteContractServiceTypeById = new Mock<IContractHierarchyRepository>();
            ContractHierarchy objContractHierarchy = new ContractHierarchy { NodeId = 1 };
            mockDeleteContractServiceTypeById.Setup(f => f.DeleteContractServiceType(objContractHierarchy))
                .Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteContractServiceTypeById.Object);
            bool actual = target.DeleteContractServiceType(objContractHierarchy);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for DeleteContractServiceTypeById if Null
        ///</summary>
        [TestMethod]
        public void DeleteContractServiceTypeByIdTestNull()
        {
            ContractHierarchy objContractHierarchy = null;
            var mockDeleteContractServiceTypeById = new Mock<IContractHierarchyRepository>();
            mockDeleteContractServiceTypeById.Setup(f => f.DeleteContractServiceType(objContractHierarchy))
                .Returns(false
                );
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteContractServiceTypeById.Object);

            bool actual = target.DeleteContractServiceType(null);
            Assert.IsFalse(actual);

        }


        /// <summary>
        ///A test for DeleteContractServiceTypeById
        ///</summary>
        [TestMethod]
        public void DeleteContractServiceTypeByIdTestNotNull()
        {
            ContractHierarchy objContractHierarchy = new ContractHierarchy { ContractId = 123 };
            var mockDeleteContractServiceTypeById = new Mock<IContractHierarchyRepository>();
            mockDeleteContractServiceTypeById.Setup(f => f.DeleteContractServiceType(objContractHierarchy))
                .Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteContractServiceTypeById.Object);

            bool actual = target.DeleteContractServiceType(objContractHierarchy);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for DeleteNodeAndContractByNodeId if Null
        ///</summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdTestNull()
        {
            ContractHierarchy objContractHierarchy = null;
            var mockDeleteNodeAndContractByNodeId = new Mock<IContractHierarchyRepository>();
            mockDeleteNodeAndContractByNodeId.Setup(f => f.DeleteNode(objContractHierarchy)).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteNodeAndContractByNodeId.Object);
            bool actual = target.DeleteNode(null);
            Assert.IsFalse(actual);

        }

        /// <summary>
        ///A test for DeleteNodeAndContractByNodeId if Not Null
        ///</summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdTestNotNull()
        {
            ContractHierarchy objContractHierarchy = new ContractHierarchy { NodeId = 123 };
            var mockDeleteNodeAndContractByNodeId = new Mock<IContractHierarchyRepository>();
            mockDeleteNodeAndContractByNodeId.Setup(f => f.DeleteNode(objContractHierarchy)).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteNodeAndContractByNodeId.Object);

            bool actual = target.DeleteNode(objContractHierarchy);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Contracts the TreeView logic constructor test.
        /// </summary>
        [TestMethod]
        public void ContractTreeViewLogicConstructorTest()
        {
            ContractHierarchyLogic target = new ContractHierarchyLogic(Constants.ConnectionString);
            Assert.IsInstanceOfType(target, typeof(ContractHierarchyLogic));
        }

        /// <summary>
        /// Contracts the TreeView logic constructor test1.
        /// </summary>
        [TestMethod]
        public void ContractTreeViewLogicConstructorTest1()
        {
            var mockContractTreeViewLogic = new Mock<IContractHierarchyRepository>();
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractTreeViewLogic.Object);
            Assert.IsInstanceOfType(target, typeof(ContractHierarchyLogic));
        }

        /// <summary>
        /// Delete Node And Contract ByNodeId
        /// </summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdTest1()
        {
            var mockDeleteContractServiceTypeById = new Mock<IContractHierarchyRepository>();
            ContractHierarchy data = new ContractHierarchy { NodeId = 23 };
            mockDeleteContractServiceTypeById.Setup(f => f.DeleteNode(data)).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteContractServiceTypeById.Object);
            bool actual = target.DeleteNode(data);
            Assert.AreEqual(true, actual);
        }

        /// <summary>
        ///A test for GetContractHierarchy
        ///</summary>
        [TestMethod]
        public void GetContractHierarchyTestNull()
        {
            //Mock Input
            ContractHierarchy contractHierarchy1 = null;
            //Mock Output
            List<ContractHierarchy> contractHierarchy = new List<ContractHierarchy>
            {
                new ContractHierarchy
                {
                    NodeId = 12345,
                    ParentId = 23455,
                    NodeText = "test1",
                    AppendString = "testvalue1",
                    IsContract = true,
                    ContractId = 111,
                    ContractServiceTypeId = 3333,
                    IsPrimaryNode = true,
                },
                new ContractHierarchy
                {
                    NodeId = 123,
                    ParentId = 234,
                    NodeText = "test2",
                    AppendString = "testvalue2",
                    IsContract = false,
                    ContractId = 2222,
                    ContractServiceTypeId = 4444,
                    IsPrimaryNode = false
                }
            };

            var mockContractHierarchy = new Mock<IContractHierarchyRepository>();
            mockContractHierarchy.Setup(f => f.GetContractHierarchy(contractHierarchy1)).Returns(contractHierarchy);

            //setup
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchy.Object);
            List<ContractHierarchy> actual = target.GetContractHierarchy(null);
            Assert.AreEqual(actual, null);
        }

        /// <summary>
        ///A test for GetContractHierarchy
        ///</summary>
        [TestMethod]
        public void GetContractHierarchyTestNotNull()
        {
            //Mock Input
            ContractHierarchy contractHierarchy1 = new ContractHierarchy { NodeId = 12345 };


            //Mock Output
            List<ContractHierarchy> contractHierarchy = new List<ContractHierarchy>
            {
                new ContractHierarchy
                {
                    NodeId = 12345,
                    ParentId = 23455,
                    NodeText = "test1",
                    AppendString = "testvalue1",
                    IsContract = true,
                    ContractId = 111,
                    ContractServiceTypeId = 3333,
                    IsPrimaryNode = true,
                },
                new ContractHierarchy
                {
                    NodeId = 123,
                    ParentId = 234,
                    NodeText = "test2",
                    AppendString = "testvalue2",
                    IsContract = false,
                    ContractId = 2222,
                    ContractServiceTypeId = 4444,
                    IsPrimaryNode = false
                }
            };

            var mockContractHierarchy = new Mock<IContractHierarchyRepository>();
            mockContractHierarchy.Setup(f => f.GetContractHierarchy(contractHierarchy1)).Returns(contractHierarchy);

            //setup
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchy.Object);
            List<ContractHierarchy> actual = target.GetContractHierarchy(contractHierarchy1);
            Assert.AreEqual(actual, contractHierarchy);

        }


        /// <summary>
        ///A test for CopyContractByNodeAndParentId
        ///</summary>
        [TestMethod]
        public void CopyContractByNodeAndParentIdTest()
        {
            ContractHierarchyLogic target = new ContractHierarchyLogic(Constants.ConnectionString);
            const long expected = 0;
            long actual = target.CopyNode(null);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for DeleteContractServiceTypeById
        ///</summary>
        [TestMethod]
        public void DeleteContractServiceTypeByIdIfNullTest()
        {
            var repository = new Mock<IContractHierarchyRepository>();
            repository.Setup(f => f.DeleteContractServiceType(null));
            ContractHierarchyLogic target = new ContractHierarchyLogic(repository.Object);

            bool actual = target.DeleteContractServiceType(null);
            Assert.AreEqual(false, actual);
        }

        /// <summary>
        ///A test for DeleteNodeAndContractByNodeId
        ///</summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdIfNullTest()
        {
            var repository = new Mock<IContractHierarchyRepository>();
            repository.Setup(f => f.DeleteNode(null));
            ContractHierarchyLogic target = new ContractHierarchyLogic(repository.Object);

            bool actual = target.DeleteNode(null);
            Assert.AreEqual(false, actual);
        }


        /// <summary>
        ///A test for GetContractHierarchy
        ///</summary>
        [TestMethod]
        public void GetContractHierarchyifNullTest()
        {
            var repository = new Mock<IContractHierarchyRepository>();
            repository.Setup(f => f.GetContractHierarchy(null));
            ContractHierarchyLogic target = new ContractHierarchyLogic(repository.Object);

            List<ContractHierarchy> actual = target.GetContractHierarchy(null);
            Assert.AreEqual(null, actual);

            //ContractHierarchyLogic target = new ContractHierarchyLogic();
            //List<ContractHierarchy> actual = target.GetContractHierarchy(null);
            //Assert.AreEqual(null, actual);

        }

        /// <summary>
        /// Gets the contract hierarchy test.
        /// </summary>
        [TestMethod]
        public void GetContractHierarchyTest()
        {
            //Mock input
            ContractHierarchy contractHierarchy1 = new ContractHierarchy
            {
                FacilityId = 2,
                NodeId = 2222,
                UserName = null,
                ParentId = 0,
            };

            //Mock output
            List<ContractHierarchy> expected = new List<ContractHierarchy>
            {
                new ContractHierarchy
                {
                    NodeText = "test",
                    ContractId = 12345,
                    IsPrimaryNode = false,
                    IsContract = true,
                   NodeId=321874
                },
                new ContractHierarchy
                {
                    NodeText = "test2",
                    ContractId = 1234521,
                    IsPrimaryNode = false,
                    IsContract = true,
                     NodeId=36584
                }
            };
            //setup
            var mockContractHierarchy = new Mock<IContractHierarchyRepository>();
            mockContractHierarchy.Setup(f => f.GetContractHierarchy(It.IsAny<ContractHierarchy>())).Returns(expected);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchy.Object);
            List<ContractHierarchy> actual = target.GetContractHierarchy(contractHierarchy1);
            Assert.AreEqual(expected, actual);

        }


        /// <summary>
        /// Gets the contract hierarchy test.
        /// </summary>
        [TestMethod]
        public void GetContractHierarchytNotNullGetChildsTest()
        {
            //Mock input
            ContractHierarchy contractHierarchy1 = new ContractHierarchy
            {
                FacilityId = 2,
                NodeId = 2222,
                UserName = null,
                ParentId = 0,
            };

            //Mock output
            List<ContractHierarchy> expected = new List<ContractHierarchy>
            {
                new ContractHierarchy
                {
                    NodeText = "test",
                    ContractId = 12345,
                    IsPrimaryNode = false,
                    IsContract = true,
                   NodeId=321874
                },
                new ContractHierarchy
                {
                    NodeText = "test2",
                    ContractId = 1234521,
                    IsPrimaryNode = false,
                    IsContract = null,
                     NodeId=321874
                }
            };
            //setup
            var mockContractHierarchy = new Mock<IContractHierarchyRepository>();
            mockContractHierarchy.Setup(f => f.GetContractHierarchy(It.IsAny<ContractHierarchy>())).Returns(expected);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchy.Object);
            List<ContractHierarchy> actual = target.GetContractHierarchy(contractHierarchy1);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetContractHierarchytContractNullTest()
        {
            //Mock input
            ContractHierarchy contractHierarchy1 = new ContractHierarchy
            {
                FacilityId = 2,
                NodeId = 2222,
                UserName = null,
                ParentId = 0,
            };

            //Mock output
            List<ContractHierarchy> expected = new List<ContractHierarchy>
            {
                new ContractHierarchy
                {
                    NodeText = "test",
                    ContractId = 12345,
                    IsPrimaryNode = false,
                    IsContract = true,
                   NodeId=321874
                },
                new ContractHierarchy
                {
                    NodeText = "test2",
                    ContractId = 1234521,
                    IsPrimaryNode = false,
                    IsContract = null,
                     NodeId=321874
                }
            };
            //setup
            var mockContractHierarchy = new Mock<IContractHierarchyRepository>();
            mockContractHierarchy.Setup(f => f.GetContractHierarchy(It.IsAny<ContractHierarchy>())).Returns(expected);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockContractHierarchy.Object);
            List<ContractHierarchy> actual = target.GetContractHierarchy(contractHierarchy1);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// Copies the contract by node and parent unique identifier difference null test.
        /// </summary>
        [TestMethod]
        public void CopyContractByNodeAndParentIdIfNullTest()
        {
            ContractHierarchyLogic target = new ContractHierarchyLogic(Constants.ConnectionString);
            const long expected = 0;
            long actual = target.CopyNode(null);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for DeleteContractServiceTypeById if Null
        /// </summary>
        [TestMethod]
        public void DeleteContractServiceTypeByIdIfNullTest1()
        {
            //Arrange
            var repository = new Mock<IContractHierarchyRepository>();
            repository.Setup(f => f.DeleteContractServiceType(It.IsAny<ContractHierarchy>())).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(repository.Object);

            //Act
            bool actual = target.DeleteContractServiceType(null);

            //Assert
            Assert.AreEqual(false, actual);
        }

        //todo: ok
        /// <summary>
        /// Deletes the node and contract by node unique identifier if null test1.
        /// </summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdIfNullTest1()
        {
            ContractHierarchyLogic target = new ContractHierarchyLogic(Constants.ConnectionString);
            bool actual = target.DeleteNode(null);
            Assert.AreEqual(false, actual);

        }

        //todo:ok
        // test cases fail for procedure problem
        /// <summary>
        ///A test for DeleteNodeAndContractByNodeId
        ///</summary>
        [TestMethod]
        public void DeleteNodeAndContractByNodeIdIfNotNullest()
        {
            var mockDeleteNodeAndContractByNodeId = new Mock<IContractHierarchyRepository>();
            ContractHierarchy data = new ContractHierarchy
            {
                NodeId = 2275,
            };
            mockDeleteNodeAndContractByNodeId.Setup(f => f.DeleteNode(data)).Returns(true);
            ContractHierarchyLogic target = new ContractHierarchyLogic(mockDeleteNodeAndContractByNodeId.Object);
            bool result = target.DeleteNode(data);
            Assert.AreEqual(true, result);

        }

    }
}
