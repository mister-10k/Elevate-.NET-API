using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public EmployeeModel Post(EmployeeModel employee)
        {
            return this.employeeBL.CreateEmployee(employee);
        }

        [HttpGet]
        public EmployeeModel Get(int employeeId)
        {
            return employeeBL.GetEmployee(employeeId);
        }

        [Route("api/employee/{employeeId}")]
        [HttpPut]
        public EmployeeModel Put(EmployeeModel employee)
        {
            return this.employeeBL.UpdateEmployee(employee);
        }

        [Route("api/employee/{employeeId}")]
        [HttpDelete]
        public EmployeeModel Delete(int employeeId)
        {
            return employeeBL.DeleteEmployee(employeeId);
        }

        [Route("api/employee/getebdashboardcardsdata/{companyId}")]
        [HttpGet]
        public List<EBDashbaordStatsCardModel> GetEBDashboardCardsData(int companyId)
        {
            return employeeBL.GetEBDashboardCardsData(companyId);
        }

        [Route("api/employee/getEmployeesForEBDashboard")]
        [HttpPost]
        public TableModel<EmployeeModel> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            return employeeBL.GetEmployeesForEBDashboard(requestModel);
        }

        [Route("api/employee/getEmployeeFormMasterData")]
        [HttpGet]
        public EmployeeFormMasterDataModel GetEmployeeFormMasterData()
        {
            return employeeBL.GetEmployeeFormMasterData();
        }

        [Route("api/employee/getTop10HighestEmployeeDedcutions/{companyId}")]
        [HttpGet]
        public PrimeNGBarChartModel GetTop10HighestEmployeeDedcutions(int companyId)
        {
            return employeeBL.GetTop10HighestEmployeeDedcutions(companyId);
        }
    }
}
