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
    public class EmployeeBLTests
    {
        private IEmployeeBL employeeBL;

        #region mock DL's
        private IEmployeeDL MockEmployeeDL;
        #endregion
        public EmployeeBLTests()
        {
            MockEmployeeDL = MockRepository.GenerateMock<IEmployeeDL>();
        }

        [TestInitialize]
        public void Initialize()
        {
            employeeBL = new EmployeeBL(MockEmployeeDL);
        }


        [TestMethod]
        public async Task CreateEmployeeAsyncTest()
        {
            // Arrange
            MockEmployeeDL
              .Stub(x => x.CreateEmployeeAsync(Arg<EmployeeDTO>.Is.Anything))
              .IgnoreArguments()
              .Return(Task.FromResult(new EmployeeDTO()));

            var employeeDTO = new EmployeeDTO
            {
                FirstName = "kwabena",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 2,
            };

            // Act
            var result = await employeeBL.CreateEmployeeAsync(employeeDTO);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeAsyncTest()
        {
            MockEmployeeDL
             .Stub(x => x.GetEmployeeAsync(Arg<int>.Is.Anything))
             .IgnoreArguments()
             .Return(Task.FromResult(new EmployeeDTO()));

            var employeeId = 0;

            var result = await employeeBL.GetEmployeeAsync(employeeId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsyncTest()
        {
            MockEmployeeDL
            .Stub(x => x.UpdateEmployeeAsync(Arg<EmployeeDTO>.Is.Anything))
            .IgnoreArguments()
            .Return(Task.FromResult(new EmployeeDTO()));

            var employeeDTO = new EmployeeDTO
            {
                Id = 1,
                FirstName = "kwabenaa",
                LastName = "Ohemeng",
                Email = "kwabeohemeng@gmail.com",
            };

            var result = await employeeBL.UpdateEmployeeAsync(employeeDTO);


            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsyncTest()
        {
            MockEmployeeDL
            .Stub(x => x.DeleteEmployeeAsync(Arg<int>.Is.Anything))
            .IgnoreArguments()
            .Return(Task.FromResult(new EmployeeDTO()));

            var employeeId = 0;

            var result = await employeeBL.DeleteEmployeeAsync(employeeId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEBDashboardCardsDataAsyncTest()
        {
            MockEmployeeDL
            .Stub(x => x.GetAllEmployeesForComapnyAsync(Arg<int>.Is.Anything))
            .IgnoreArguments()
            .Return(Task.FromResult(new List<EmployeeDTO>()));

            var companyId = 1;

            var result = await employeeBL.GetEBDashboardCardsDataAsync(companyId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeesForEBDashboardAsyncTest()
        {
            MockEmployeeDL
           .Stub(x => x.GetEmployeesForEBDashboardAsync(Arg<EBEmployeeListRequestDTO>.Is.Anything))
           .IgnoreArguments()
           .Return(Task.FromResult(new TableDTO<EmployeeDTO>()));

            var requestDTO = new EBEmployeeListRequestDTO
            {
                CompanyId = 1,
                SearchText = "",
                SortBy = "ASC",
                SortColumn = "Id",
                PageSize = 5,
                PageNumber = 0
            };

            var result = await employeeBL.GetEmployeesForEBDashboardAsync(requestDTO);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeFormMasterDataAsyncTest()
        {
            MockEmployeeDL
           .Stub(x => x.GetEmployeeFormMasterDataAsync())
           .IgnoreArguments()
           .Return(Task.FromResult(new EmployeeFormMasterDataDTO()));


            var result = await employeeBL.GetEmployeeFormMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTop10HighestEmployeeDedcutionsTest()
        {
            MockEmployeeDL
            .Stub(x => x.GetAllEmployeesForComapnyAsync(Arg<int>.Is.Anything))
            .IgnoreArguments()
            .Return(Task.FromResult(new List<EmployeeDTO>()));

            var companyId = 1;

            var result = await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);

            Assert.IsNotNull(result);
        }

    }
}
