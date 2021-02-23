using Elevate.Shared;
using Elevate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using System.Net;
using Elevate.Business;

namespace Elevate.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeBL employeeBL;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeBL employeeBL, IMapper mapper)
        {
            this.employeeBL = employeeBL;
            this.mapper = mapper;
        }

        [Route("api/employee")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(EmployeeModel employeeModel)
        {
            if (employeeModel != null)
            {
                var employeeDTO = mapper.Map<EmployeeDTO>(employeeModel);

                var result = await this.employeeBL.CreateEmployeeAsync(employeeDTO);
                if (result != null)
                {
                    return Ok(mapper.Map<EmployeeModel>(result));
                }
            }

            return Content(HttpStatusCode.Conflict, AppConstants.HttpErrorMessage.FailedPost);
        }

        [Route("api/employee/{employeeId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployee(int employeeId)
        {
            var result = await employeeBL.GetEmployeeAsync(employeeId);
            if (result != null)
            {
                return Ok(mapper.Map<EmployeeModel>(result));
            }

            return Content(HttpStatusCode.NotFound, AppConstants.HttpErrorMessage.ResourceNotFound);
        }

        [Route("api/employee")]
        [HttpPatch]
        public async Task<IHttpActionResult> UpdateEmployee(EmployeeModel employeeModel)
        {
            if (employeeModel != null)
            {
                var employeeDTO = mapper.Map<EmployeeDTO>(employeeModel);

                var result = await this.employeeBL.UpdateEmployeeAsync(employeeDTO);
                if (result != null)
                {
                    return Ok(mapper.Map<EmployeeModel>(result));
                }
            }

            return Content(HttpStatusCode.Conflict, AppConstants.HttpErrorMessage.FailedPost);
        }

        [Route("api/employee/{employeeId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int employeeId)
        {
            var result = await employeeBL.DeleteEmployeeAsync(employeeId);
            if (result != null)
            {
                return Ok(mapper.Map<EmployeeModel>(result));
            }

            return Content(HttpStatusCode.NotFound, AppConstants.HttpErrorMessage.ResourceNotFound);
        }

        [Route("api/employee/getebdashboardcardsdata/{companyId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEBDashboardCardsData(int companyId)
        {
            var result = await employeeBL.GetEBDashboardCardsDataAsync(companyId);
            if (result != null)
            {
                return Ok(mapper.Map<List<EBDashbaordStatsCardModel>>(result));
            }

            return BadRequest();
        }

        [Route("api/employee/getEmployeesForEBDashboard")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            if (requestModel != null)
            {
                var requestDTO = mapper.Map<EBEmployeeListRequestDTO>(requestModel);
                
                var result = await employeeBL.GetEmployeesForEBDashboardAsync(requestDTO);
                if (result != null)
                {
                    return Ok(mapper.Map<TableModel<EmployeeModel>>(result));

                }
            }

            return BadRequest();
        }

        [Route("api/employee/getEmployeeFormMasterData")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeFormMasterData()
        {
            var result = await employeeBL.GetEmployeeFormMasterDataAsync();
            if (result != null)
            {
                return Ok(mapper.Map<EmployeeFormMasterDataModel>(result));
            }

            return BadRequest();
        }

        [Route("api/employee/getTop10HighestEmployeeDedcutions/{companyId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTop10HighestEmployeeDedcutions(int companyId)
        {
            var result = await employeeBL.GetTop10HighestEmployeeDedcutions(companyId);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest();
        }
    }
}
