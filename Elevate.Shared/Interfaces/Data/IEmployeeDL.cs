using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public interface IEmployeeDL
    {
        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeModel">info for new employee</param>
        /// <returns>created employee info as a task</returns>
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employee);

        /// <summary>This method gets info of an employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>employee info as a task</returns>
        Task<EmployeeDTO> GetEmployeeAsync(int employeeId);

        /// <summary>This methof gets all employees for a company.</summary>
        /// <param name="comapnyId">The companies Id</param>
        /// <returns>all employees as a task</returns>
        Task<List<EmployeeDTO>> GetAllEmployeesForComapnyAsync(int comapnyId);

        /// <summary>This method updates a new employee.</summary>
        /// <param name="employeeModel">info for employee to update</param>
        /// <returns>updated employee info as a task</returns>
        Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employee);

        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>deleted employee info as a task</returns>
        Task<EmployeeDTO> DeleteEmployeeAsync(int employeeId);

        /// <summary>Gets list of employees for employee benifits dashboard</summary>
        /// <param name="requestModel">Data for request</param>
        /// <returns>Emloyees for table as a task</returns>
        Task<TableDTO<EmployeeDTO>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestDTO requestModel);

        /// <summary>Gets master data form employee form</summary>
        /// <returns>employee form master data as a task</returns>
        Task<EmployeeFormMasterDataDTO> GetEmployeeFormMasterDataAsync();
    }
}
