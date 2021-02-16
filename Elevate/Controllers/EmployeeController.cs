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

        // POST api/employee
        [HttpPost]
        public EmployeeModel Post(EmployeeModel employee)
        {
            return this.employeeBL.CreateEmployee(employee);
        }

        // POST api/employee/id
        [HttpGet]
        public EmployeeModel Get(int id)
        {
            EmployeeModel employeeModel = null;

            //var result = employeeBL.GetEmployee(id);
            //if (result != null)
            //{
            //    employeeModel = new EmployeeModel { };
            //    FillEmployeeModel(employeeModel, result);
            //}

            return employeeModel;
        }

        // PUT api/employee
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
        public EBDashbaordStatsModel GetEBDashboardCardsData(int companyId)
        {
            return employeeBL.GetEBDashboardCardsData(companyId);
        }

        //private void FillEmployeeModel(EmployeeModel employeeModel, EmployeeModel employee)
        //{

        //    employee.FirstName = employee.FirstName;
        //    employee.LastName = employee.LastName;
        //    employee.CompanyId = employee.CompanyId;
        //    employee.CompanyName = employee.CompanyName;
        //    employee.CompanyDisplayName = employee.CompanyDisplayName;
        //    employee.CreatedAt = employee.CreatedAtText != null && employee.CreatedAt.HasValue ? employee.CreatedAt.Value.ToString("MM/dd/yyyy") : string.Empty;
        //    employee.ModifiedAt = employee.ModifiedAt != null && employee.ModifiedAt.HasValue ? employee.ModifiedAt.Value.ToString("MM/dd/yyyy") : string.Empty;
        //    employee.Dependents = new List<EmployeeDependentModel>();
            

        //    foreach(var dependent in employeeModel.Dependents)
        //    {
        //        var d = new EmployeeDependentModel
        //        {
        //            Id = dependent.Id,
        //            EmployeeId = dependent.EmployeeId,
        //            FirstName = dependent.FirstName,
        //            LastName = dependent.LastName,
        //            RelationshipId = dependent.RelationshipId,
        //            CreatedAt = dependent.CreatedAt.HasValue ? dependent.CreatedAt.Value.ToString("MM/dd/yyyy") : string.Empty,
        //            ModifiedAt = dependent.ModifiedAt.HasValue ? dependent.ModifiedAt.Value.ToString("MM/dd/yyyy") : string.Empty,
        //        };
        //        employeeModel.Dependents.Add(d);
        //    }
        //}

        [Route("api/employee/GetEmployeesForEBDashboard")]
        [HttpPost]
        public TableModel<EmployeeModel> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            return employeeBL.GetEmployeesForEBDashboard(requestModel);
        }

        [Route("api/employee/GetEmployeeFormMasterData")]
        [HttpGet]
        public EmployeeFormMasterDataModel GetEmployeeFormMasterData()
        {
            return employeeBL.GetEmployeeFormMasterData();
        }

        [Route("api/employee/GetTop10HighestEmployeeDedcutions")]
        [HttpGet]
        public PrimeNGBarChartModel GetTop10HighestEmployeeDedcutions()
        {
            return employeeBL.GetTop10HighestEmployeeDedcutions();
        }
    }
}
