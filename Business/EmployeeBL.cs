using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Business
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDL employeeDL;
        public EmployeeBL(IEmployeeDL employeeDL)
        {
            this.employeeDL = employeeDL;
        }
        public EmployeeModel CreateEmployee(EmployeeModel employee)
        {
            return employeeDL.CreateEmployee(employee);
        }
        public EmployeeModel GetEmployee(int id)
        {
            return employeeDL.GetEmployee(id);
        }

        public EmployeeModel UpdateEmployee(EmployeeModel employee)
        {
            return employeeDL.UpdateEmployee(employee);
        }

        public EmployeeModel DeleteEmployee(int id)
        {
            return employeeDL.DeleteEmployee(id);
        }

        public EBDashbaordStatsModel GetEBDashboardCardsData(int companyId)
        {
            EBDashbaordStatsModel ret = null;
            try
            {
                ret = employeeDL.GetEBDashboardCardsData(companyId);
                ret.NumberOfEmployees =  ret.Employees.Count;
                ret.NumberOfEmployeeDependents = GetNumberOfDependentsForEmployee(ret.Employees);
                ret.DeductionTotalPerYearTotal = GetTotalForYearDeductionsForEmployees(ret.Employees);
                ret.DeductionPerMonthTotal = ret.DeductionTotalPerYearTotal / 12;
                ret.DeductionBiWeeklyTotal = ret.DeductionTotalPerYearTotal / 26;
                ret.Employees = null; // avoid large json load
            }
            catch (Exception ex)
            {
                Console.WriteLine("Business Layer: GetEBDashboardCardsData Exception Msg", ex.Message);
            }
            return ret;
        }

        private int GetNumberOfDependentsForEmployee(List<EmployeeModel> employees)
        {
            var count = 0;
            foreach (var employee in employees)
            {
                count += employee.Dependents.Count;
            }
            return count;
        }

        private double GetTotalForYearDeductionsForEmployees(List<EmployeeModel> employees)
        {
            double total = 0.0;
            foreach(var employee in employees)
            {
                total += EmployeeBenifitsUtility.GetEmployeeDeductionCost(employee);
            }
            return total;
        }

        public TableModel<EmployeeModel> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            return employeeDL.GetEmployeesForEBDashboard(requestModel);
        }

        public EmployeeFormMasterDataModel GetEmployeeFormMasterData()
        {
            return employeeDL.GetEmployeeFormMasterData();
        }
    }
}
