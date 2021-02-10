using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public interface IEmployeeBL
    {
        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeDTO">info for new employee</param>
        /// <returns>created employee info</returns>
        EmployeeDTO CreateEmployee(EmployeeDTO employeeDTO);

        /// <summary>This method gets info of an employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>employee info</returns>
        EmployeeDTO GetEmployee(int employeeId);

        /// <summary>This method updates a new employee.</summary>
        /// <param name="employeeDTO">info for employee to update</param>
        /// <returns>updated employee info</returns>
        EmployeeDTO UpdateEmployee(EmployeeDTO employeeDTO);

        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>deleted employee info</returns>
        EmployeeDTO DeleteEmployee(int employeeId);

        /// <summary>This method gets data for cards on employment benifits dashboard.</summary>
        /// <param name="companyId">the id of company data should be retrieved for.</param>
        /// <returns>the card data</returns>
        EBDashbaordStatsDTO GetEBDashboardCardsData(int companyId);
    }
}
