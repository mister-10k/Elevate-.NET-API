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
        public async Task CreateEmployeeAsync_WithValidNotTakenEmail_ReturnsNewlyCreatedEmployeeDTO()
        {
            // Arrange
            var employeeDTO = new EmployeeDTO
            {
                FirstName = "kwabena",
                LastName = "Ohemeng",
                Email = "kwabeohemeng2@gmail.com",
                Password = "Welcome4",
                UserTypeId = 2,
            };

            MockEmployeeDL
              .Stub(x => x.CreateEmployeeAsync(Arg<EmployeeDTO>.Is.NotNull))
              .Return(Task.FromResult(employeeDTO));

            // Act
            var result = await employeeBL.CreateEmployeeAsync(employeeDTO);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeAsync_WithEmployeeIdGreaterThanZero_ReturnsEmployeeDTO()
        {
            MockEmployeeDL
             .Stub(x => x.GetEmployeeAsync(Arg<int>.Is.Anything))
             .Return(Task.FromResult(new EmployeeDTO()));

            var employeeId = 1;

            var result = await employeeBL.GetEmployeeAsync(employeeId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_WithNotNullValidEmplyeeDTO_returnsEmployeeDTOOfUpdatedEmployee()
        {
            var employeeDTO = new EmployeeDTO
            {
                Id = 1,
                FirstName = "kwabenaa",
                LastName = "Ohemeng",
                Email = "kwabeohemen2g@gmail.com",
            };

            MockEmployeeDL
            .Stub(x => x.UpdateEmployeeAsync(Arg<EmployeeDTO>.Is.NotNull))
            .Return(Task.FromResult(employeeDTO));

            var result = await employeeBL.UpdateEmployeeAsync(employeeDTO);


            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_WithEmployeeIdGreaterThanZero_ReturnsEmployeeDTO()
        {
            MockEmployeeDL
            .Stub(x => x.DeleteEmployeeAsync(Arg<int>.Is.Anything))
            .Return(Task.FromResult(new EmployeeDTO()));

            var employeeId = 1;

            var result = await employeeBL.DeleteEmployeeAsync(employeeId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEBDashboardCardsDataAsync_WithComapnyIdGreaterThanZero_ReturnsNotNullCardsData()
        {
            MockEmployeeDL
            .Stub(x => x.GetAllEmployeesForComapnyAsync(Arg<int>.Is.Anything))
            .Return(Task.FromResult(new List<EmployeeDTO>()));

            var companyId = 1;

            var result = await employeeBL.GetEBDashboardCardsDataAsync(companyId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeesForEBDashboardAsync_WithValidRequestDTO_ReturnsNotNullTableDTO()
        {
            MockEmployeeDL
           .Stub(x => x.GetEmployeesForEBDashboardAsync(Arg<EBEmployeeListRequestDTO>.Is.Anything))
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
        public async Task GetEmployeeFormMasterDataAsync_NoParameters_ReturnsNotNullMasterData()
        {
            MockEmployeeDL
           .Stub(x => x.GetEmployeeFormMasterDataAsync())
           .Return(Task.FromResult(new EmployeeFormMasterDataDTO()));


            var result = await employeeBL.GetEmployeeFormMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTop10HighestEmployeeDedcutions_WithComapnyIdGreaterThanZero_ReturnsNotPrimeNGBarChartDTO()
        {
            MockEmployeeDL
            .Stub(x => x.GetAllEmployeesForComapnyAsync(Arg<int>.Is.Anything))
            .Return(Task.FromResult(new List<EmployeeDTO>()));

            var companyId = 1;

            var result = await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);

            Assert.IsNotNull(result);
        }

    }
}
