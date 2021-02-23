using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Business
{
    public interface IEmployeeBL
    {
        /// <summary>This method creates a new employee.</summary>
        /// <param name="employee">info for new employee</param>
        /// <returns>created employee info as a task</returns>
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employee);

        /// <summary>This method gets info of an employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>employee info as a task</returns>
        Task<EmployeeDTO> GetEmployeeAsync(int employeeId);

        /// <summary>This method updates a new employee.</summary>
        /// <param name="employeeModel">info for employee to update</param>
        /// <returns>updated employee info as a task</returns>
        Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employee);

        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>deleted employee info as a task</returns>
        Task<EmployeeDTO> DeleteEmployeeAsync(int employeeId);

        /// <summary>This method gets data for cards on employment benifits dashboard.</summary>
        /// <param name="companyId">the id of company data should be retrieved for.</param>
        /// <returns>the card data as a task</returns>
        Task<List<EBDashbaordStatsCardDTO>> GetEBDashboardCardsDataAsync(int companyId);

        /// <summary>Gets list of employees for employee benifits dashboard</summary>
        /// <param name="requestModel">Data for request</param>
        /// <returns>Emloyees for table as a task</returns>
        Task<TableDTO<EmployeeDTO>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestDTO requestDTO);

        /// <summary>Gets master data form employee form</summary>
        /// <returns>employee form master data as a task</returns>
        Task<EmployeeFormMasterDataDTO> GetEmployeeFormMasterDataAsync();

        /// <summary>Gets Top 10 Employees with highest deductions</summary>
        /// <returns>Top 10 employees with highest deductions as a task</returns>
        Task<PrimeNGBarChartDTO> GetTop10HighestEmployeeDedcutions(int companyId);
    }
}
