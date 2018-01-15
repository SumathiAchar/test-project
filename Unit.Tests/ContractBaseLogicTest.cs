using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace MedWorth.ContractManagement.Unit.Tests
{
       [TestClass]
    public class ContractBaseLogicTest
    {
        //Creating object for Logic
           private ContractBaseLogic _target;

           //Creating mock object for ContractLogic
           private Mock<IConditionLogic> _mockConditionLogic;

           [TestMethod]
           public void IsMatchValidDataTest()
           {
               // Arrange
              
               _mockConditionLogic = new Mock<IConditionLogic>();
               _target = new ContractBaseLogic(_mockConditionLogic.Object);
               List<string> leftOperand = new List<string> {"121"};
               _mockConditionLogic.Setup(x => x.GetPropertyValues(null, Constants.PropertyBillType)).Returns(leftOperand);
               _mockConditionLogic.Setup(x => x.IsMatch()).Returns(true);
               _mockConditionLogic.SetupAllProperties();

               EvaluateableClaim  evaluateableClaim= new EvaluateableClaim {ClaimId = 123, BillType = "121"};

               List<ICondition> conditions = new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "121",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               };

               //Act
               bool actual = _target.IsConditionsValid(conditions, evaluateableClaim);

               // Assert
               Assert.AreEqual(true, actual);

           }
           [TestMethod]
           public void IsMatchInValidDataTest()
           {
               // Arrange
               _target = new ContractBaseLogic();
               _mockConditionLogic = new Mock<IConditionLogic>();
               List<string> leftOperand = new List<string> { "121" };
               _mockConditionLogic.Setup(x => x.GetPropertyValues(null, Constants.PropertyBillType)).Returns(leftOperand);
               _mockConditionLogic.Setup(x => x.IsMatch()).Returns(false);
               _mockConditionLogic.SetupAllProperties();

               EvaluateableClaim evaluateableClaim = new EvaluateableClaim { ClaimId = 123, BillType = "121" };

               List<ICondition> conditions = new List<ICondition>
               {
                   new Condition
                   {
                       ConditionOperator = (byte) Enums.ConditionOperation.EqualTo,
                       RightOperand = "131",
                       OperandIdentifier = (byte) Enums.OperandIdentifier.BillType,
                       OperandType = (byte) Enums.OperandType.Numeric,
                       PropertyColumnName = Constants.PropertyBillType
                   }
               };

               //Act
               bool actual = _target.IsConditionsValid(conditions, evaluateableClaim);

               // Assert
               Assert.AreEqual(false, actual);

           }

    }
}
