using Elevate.Models;
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
        public void Post([FromBody] EmployeeModel employeeModel)
        {
        }

        // POST api/employee/id
        [HttpGet]
        public EmployeeModel Get(int id)
        {
            EmployeeModel employeeModel = null;

            var result = employeeBL.GetEmployee(id);
            if (result != null)
            {
                employeeModel = new EmployeeModel { };
                FillEmployeeModel(employeeModel, result);
            }

            return employeeModel;
        }

        // PUT api/employee
        [HttpPut]
        public void Put([FromBody] EmployeeModel employeeModel)
        {
        }

        // DELETE api/employee
        [HttpDelete]
        public void Delete(int id)
        {
        }

        [Route("api/employee/getebdashboardcardsdata/{companyId}")]
        [HttpGet]
        public EBDashbaordStatsDTO GetEBDashboardCardsData(int companyId)
        {
            return employeeBL.GetEBDashboardCardsData(companyId);
        }

        private void FillEmployeeModel(EmployeeModel employeeModel, EmployeeDTO employeeDTO)
        {

            employeeModel.FirstName = employeeDTO.FirstName;
            employeeModel.LastName = employeeDTO.LastName;
            employeeModel.CompanyId = employeeDTO.CompanyId;
            employeeModel.CompanyName = employeeDTO.CompanyName;
            employeeModel.CompanyDisplayName = employeeDTO.CompanyDisplayName;
            employeeModel.CreatedAt = employeeDTO.CreatedAt != null && employeeDTO.CreatedAt.HasValue ? employeeDTO.CreatedAt.Value.ToString("MM/dd/yyyy") : string.Empty;
            employeeModel.ModifiedAt = employeeDTO.ModifiedAt != null && employeeDTO.ModifiedAt.HasValue ? employeeDTO.ModifiedAt.Value.ToString("MM/dd/yyyy") : string.Empty;
            employeeModel.Dependents = new List<EmployeeDependentModel>();
            

            foreach(var dependent in employeeDTO.Dependents)
            {
                var d = new EmployeeDependentModel
                {
                    Id = dependent.Id,
                    EmployeeId = dependent.EmployeeId,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    RelationshipId = dependent.RelationshipId,
                    CreatedAt = dependent.CreatedAt.HasValue ? dependent.CreatedAt.Value.ToString("MM/dd/yyyy") : string.Empty,
                    ModifiedAt = dependent.ModifiedAt.HasValue ? dependent.ModifiedAt.Value.ToString("MM/dd/yyyy") : string.Empty,
                };
                employeeModel.Dependents.Add(d);
            }
        }
    }
}
