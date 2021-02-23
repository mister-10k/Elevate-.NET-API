using Elevate.Business;
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
        public async Task CreateEmployeeAsync_WithNoEmail_ReturnsNull()
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
        public async Task GetEmployeeAsync_WithEmployeeIdOfZero_ReturnsNull()
        {
            var employeeId = 0;

            var result = await employeeBL.GetEmployeeAsync(employeeId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateEmployeeAsync_WithExistingEmployee_ReturnsUpdatedEmployee()
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
        public async Task DeleteEmployeeAsync_WithEmployeeIdOfZero_ReturnsNull()
        {
            var employeeId = 0;

            var result = await employeeBL.DeleteEmployeeAsync(employeeId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetEBDashboardCardsDataAsync_WithComapnyIdOfOne_ReturnsCardsDataForCompanyWithIdOne()
        {
            var companyId = 1;

            var result = await employeeBL.GetEBDashboardCardsDataAsync(companyId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetEmployeesForEBDashboardAsync_WithReqDTOWithCompanyIdOfOne_ReturnsCardsDataBasedOfReqDTO()
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
        public async Task GetEmployeeFormMasterDataAsync_NoParameters_ReturnsNotNullMasterData()
        {
            var result = await  employeeBL.GetEmployeeFormMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetTop10HighestEmployeeDedcutions_WithReqDTOWithCompanyIdOfOne_ReturnsTop10EmployeesForHighestDeductions()
        {
            var companyId = 1;

            var result = await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);

            Assert.IsNotNull(result);
        }

    }
}
