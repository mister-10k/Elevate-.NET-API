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
        /// <param name="employeeModel">info for new employee</param>
        /// <returns>created employee info</returns>
        EmployeeModel CreateEmployee(EmployeeModel employeeModel);

        /// <summary>This method gets info of an employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>employee info</returns>
        EmployeeModel GetEmployee(int employeeId);

        /// <summary>This method updates a new employee.</summary>
        /// <param name="employeeModel">info for employee to update</param>
        /// <returns>updated employee info</returns>
        EmployeeModel UpdateEmployee(EmployeeModel employeeModel);

        /// <summary>This method creates a new employee.</summary>
        /// <param name="employeeId">the id of the employee.</param>
        /// <returns>deleted employee info</returns>
        EmployeeModel DeleteEmployee(int employeeId);

        /// <summary>This method gets data for cards on employment benifits dashboard.</summary>
        /// <param name="companyId">the id of company data should be retrieved for.</param>
        /// <returns>the card data</returns>
        EBDashbaordStatsModel GetEBDashboardCardsData(int companyId);

        /// <summary>Gets list of employees for employee benifits dashboard</summary>
        /// <param name="requestModel">Data for request</param>
        /// <returns>Emloyees for table</returns>
        TableModel<EmployeeModel> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel);

        /// <summary>Gets master data form employee form</summary>
        /// <returns>employee form master data</returns>
        EmployeeFormMasterDataModel GetEmployeeFormMasterData();

        /// <summary>Gets Top 10 Employees with highest deductions</summary>
        /// <returns>Top 10 employees with highest deductions</returns>
        PrimeNGBarChartModel GetTop10HighestEmployeeDedcutions();
    }
}
