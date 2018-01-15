using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
    [TestClass]
    public class ConditionLogicTest
    {
        //Creating object for Logic
        private ConditionLogic _target;

        [TestMethod]
        public void IsMatchTest()
        {
            // Arrange
            _target = new ConditionLogic
            {
                Condition = new Condition
                {
                    ConditionOperator = (byte)Enums.ConditionOperation.EqualTo,
                    RightOperand = "250",
                    OperandIdentifier = (byte)Enums.OperandIdentifier.RevCode,
                    OperandType = (byte)Enums.OperandType.Numeric,
                    PropertyColumnName = Constants.PropertyRevCode,
                    LeftOperands = new List<string> {"250", "350"}
                }
            };

            //Act
            bool actual = _target.IsMatch();

            // Assert
            Assert.AreEqual(true, actual);

        }

        [TestMethod]
        public void GetPropertyValuesTest()
        {
            // Arrange
            _target = new ConditionLogic();
            EvaluateableClaim evaluateableClaim = new EvaluateableClaim { ClaimId = 123, BillType = "123" };

            //Act
            List<string> actual = _target.GetPropertyValues(evaluateableClaim, Constants.PropertyBillType);

            // Assert
            Assert.AreEqual("123", actual[0]);
        }


        [TestMethod]
        public void GetChildPropertyValuesTest()
        {
            // Arrange
            _target = new ConditionLogic();
            EvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                ClaimId = 123,
                BillType = "123",
                ClaimCharges =
                    new List<ClaimCharge>
                    {
                        new ClaimCharge {Line = 1, RevCode = "250"},
                        new ClaimCharge {Line = 2, RevCode = "251"}
                    }
            };

            //Act
            List<string> actual = _target.GetPropertyValues(evaluateableClaim, Constants.PropertyRevCode);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("250", actual[0]);
            Assert.AreEqual("251", actual[1]);
        }


        [TestMethod]
        public void GetPropertyValueForPhysicianTest()
        {
            // Arrange
            _target = new ConditionLogic();
            EvaluateableClaim evaluateableClaim = new EvaluateableClaim
            {
                ClaimId = 123,
                BillType = "123",
                Physicians =
                    new List<Physician>
                    {
                        new Physician
                        {
                            FirstName = "Raul",
                            MiddleName = "M",
                            LastName = "Bush",
                            PhysicianType = "Referring"
                        }
                    }
            };
            //Act
            List<string> actual = _target.GetPropertyValues(evaluateableClaim, Constants.PropertyReferringPhysician);

            // Assert
            Assert.AreEqual("RaulMBush", actual[0]);
        }
       
        [TestMethod]
        public void GetPropertyValueForStatementCoverPeriodTest()
        {
            // Arrange
            _target = new ConditionLogic();
            EvaluateableClaim evaluateableClaim = new EvaluateableClaim { ClaimId = 123, BillType = "123", StatementFrom = DateTime.Now, StatementThru = DateTime.Now.AddDays(1)};
           
            //Act
            List<string> actual = _target.GetPropertyValues(evaluateableClaim, Constants.PropertyStatementCoversPeriod);

            // Assert
            Assert.AreEqual(2, actual.Count);
        }


    }
}
