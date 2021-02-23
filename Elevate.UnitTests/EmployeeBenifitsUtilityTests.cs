using Elevate.Business;
using Elevate.IOC;
using Elevate.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;

namespace Elevate.UnitTests
{
    [TestClass]
    public class EmployeeBenifitsUtilityTests
    {

        [TestInitialize]
        public void Initialize()
        {
        }


        [TestMethod]
        public void GetEmployeeDeductionCost_NoEmployeeDependentsAndEmployeesNameDoesntStartWithA_Returns1000USD()
        {
            // Arrange
            var employeeDTO = new EmployeeDTO
            {
                FirstName = "kwabena",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 2
            };

            // Act
            var result = EmployeeBenifitsUtility.GetEmployeeDeductionCost(employeeDTO);

            // Assert
            Assert.AreEqual(1000.00, result);
        }

        [TestMethod]
        public void GetEmployeeDeductionCost_NoEmployeeDependentsAndEmployeeNameStartsWithA_Returns900USD()
        {
            var employeeDTO = new EmployeeDTO
            {
                FirstName = "Ashley",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 2
            };

            var result = EmployeeBenifitsUtility.GetEmployeeDeductionCost(employeeDTO);

            Assert.AreEqual(900.00, result);
        }

        [TestMethod]
        public void GetEmployeeDeductionCost_OneDependentAndBothEmpployeeAndDependentsNameDontStartWithA_Returns1500USD()
        {
            var employeeDTO = new EmployeeDTO
            {
                FirstName = "Kwabena",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 2,
                Dependents = new List<EmployeeDependentDTO>
                {
                    new EmployeeDependentDTO { 
                        FirstName = "Bob",
                        LastName = "Ohemeng"
                    }
                }
            };

            var result = EmployeeBenifitsUtility.GetEmployeeDeductionCost(employeeDTO);

            Assert.AreEqual(1500.00, result);
        }

        [TestMethod]
        public void GetEmployeeDeductionCost_OneDependentAndBothEmpployeeAndDependentsNameStartWithA_Returns1500USD()
        {
            var employeeDTO = new EmployeeDTO
            {
                FirstName = "Ashley",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 2,
                Dependents = new List<EmployeeDependentDTO>
                {
                    new EmployeeDependentDTO {
                        FirstName = "Able",
                        LastName = "Ohemeng"
                    }
                }
            };

            var result = EmployeeBenifitsUtility.GetEmployeeDeductionCost(employeeDTO);

            Assert.AreEqual(1350, result);
        }


    }
}
