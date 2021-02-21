using Elevate.IOC;
using Elevate.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Unity;

namespace Elevate.IntegrationTests
{
    [TestClass]
    public class EmployeeBLTests
    {
        private readonly IEmployeeBL employeeBL;

        public EmployeeBLTests()
        {
            var container = new UnityContainer();
            IOCRegistration.Register(container);

            this.employeeBL = container.Resolve<IEmployeeBL>();
        }

        [TestMethod]
        public async Task CreateEmployeeAsyncTest()
        {
            // Arrange
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
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEmployeeAsyncTest()
        {
            var employeeId = 0;

            var result = await employeeBL.GetEmployeeAsync(employeeId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsyncTest()
        {
            // Arrange
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
            var employeeId = 0;

            var result = await employeeBL.DeleteEmployeeAsync(employeeId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEBDashboardCardsDataAsyncTest()
        {
            var companyId = 1;

            var result = await employeeBL.GetEBDashboardCardsDataAsync(companyId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeesForEBDashboardAsyncTest()
        {
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
            var result = await  employeeBL.GetEmployeeFormMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTop10HighestEmployeeDedcutionsTest()
        {
            var companyId = 1;

            var result = await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);

            Assert.IsNotNull(result);
        }

    }
}
