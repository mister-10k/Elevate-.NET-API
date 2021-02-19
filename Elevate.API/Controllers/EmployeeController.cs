using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Elevate.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeBL employeeBL;
        public EmployeeController(IEmployeeBL employeeBL)
        {
            this.employeeBL = employeeBL;
        }

        [HttpPost]
        public async Task<EmployeeModel> Post(EmployeeModel employee)
        {
            return await this.employeeBL.CreateEmployeeAsync(employee);
        }

        [Route("api/employee/{employeeId}")]
        [HttpGet]
        public async Task<EmployeeModel> GetEmployee(int employeeId)
        {
            return await employeeBL.GetEmployeeAsync(employeeId);

        }

        [Route("api/employee/{employeeId}")]
        [HttpPut]
        public async Task<EmployeeModel> UpdateEmployee(EmployeeModel employee)
        {
            return await this.employeeBL.UpdateEmployeeAsync(employee);
        }

        [Route("api/employee/{employeeId}")]
        [HttpDelete]
        public async Task<EmployeeModel> Delete(int employeeId)
        {
            return await employeeBL.DeleteEmployeeAsync(employeeId);
        }

        [Route("api/employee/getebdashboardcardsdata/{companyId}")]
        [HttpGet]
        public async Task<List<EBDashbaordStatsCardModel>> GetEBDashboardCardsData(int companyId)
        {
            return await employeeBL.GetEBDashboardCardsDataAsync(companyId);
        }

        [Route("api/employee/getEmployeesForEBDashboard")]
        [HttpPost]
        public async Task<TableModel<EmployeeModel>> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            return await employeeBL.GetEmployeesForEBDashboardAsync(requestModel);
        }

        [Route("api/employee/getEmployeeFormMasterData")]
        [HttpGet]
        public async Task<EmployeeFormMasterDataModel> GetEmployeeFormMasterData()
        {
            return await employeeBL.GetEmployeeFormMasterDataAsync();
        }

        [Route("api/employee/getTop10HighestEmployeeDedcutions/{companyId}")]
        [HttpGet]
        public async Task<PrimeNGBarChartModel> GetTop10HighestEmployeeDedcutions(int companyId)
        {
            return await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);
        }
    }
}
