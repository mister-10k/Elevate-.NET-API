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

        public List<EBDashbaordStatsCardModel> GetEBDashboardCardsData(int companyId)
        {
            List<EBDashbaordStatsCardModel> ret = null;
            try
            {
                ret = new List<EBDashbaordStatsCardModel>();
                var employees = employeeDL.GetAllEmployeesForComapny(companyId);
                ret.Add(new EBDashbaordStatsCardModel
                {
                    Title = AppConstants.StatCardTitle.MyEmployees,
                    Number = employees.Count,
                    Color = AppConstants.AppColor.Primary
                });
                ret.Add(new EBDashbaordStatsCardModel
                {
                    Title = AppConstants.StatCardTitle.EmployeeDependents,
                    Number = GetNumberOfDependentsForEmployees(employees),
                    Color = AppConstants.AppColor.Secondary
                });
                var totalYearDeductions = GetTotalForYearDeductionsForEmployees(employees);
                ret.Add(new EBDashbaordStatsCardModel
                {
                    Title = AppConstants.StatCardTitle.DeductionBiWeeklyTotal,
                    Number = totalYearDeductions/26,
                    Color = AppConstants.AppColor.Tertiary,
                    IsCurrency = true
                });
                ret.Add(new EBDashbaordStatsCardModel
                {
                    Title = AppConstants.StatCardTitle.DeductionPerMonthTotal,
                    Number = totalYearDeductions / 12,
                    Color = AppConstants.AppColor.Tertiary,
                    IsCurrency = true
                });
                ret.Add(new EBDashbaordStatsCardModel
                {
                    Title = AppConstants.StatCardTitle.DeductionPerMonthTotal,
                    Number = totalYearDeductions,
                    Color = AppConstants.AppColor.Tertiary,
                    IsCurrency = true
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine("Business Layer: GetEBDashboardCardsData Exception Msg", ex.Message);
            }
            return ret;
        }

        private int GetNumberOfDependentsForEmployees(List<EmployeeModel> employees)
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

        public PrimeNGBarChartModel GetTop10HighestEmployeeDedcutions(int companyId)
        {
            var ret = new PrimeNGBarChartModel();

            var employees = employeeDL.GetAllEmployeesForComapny(companyId);

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    employee.TotalDeduction = EmployeeBenifitsUtility.GetEmployeeDeductionCost(employee);
                }

                employees = employees.OrderByDescending(x => x.TotalDeduction).ToList();

                employees = employees.Take(10).ToList();


                ret.labels = employees.Select(x => x.FirstName + " " + x.LastName).ToList();

                var dataset = new PrimeNGBarChartDataSetModel
                {
                    label = "Highest Employee Deductions",
                    backgroundColor = AppConstants.AppColor.Primary,
                    borderColor = "#1E88E5",
                    data = employees.Select(x => x.TotalDeduction).ToList()
                };
                ret.datasets = new List<PrimeNGBarChartDataSetModel> { dataset };
            }

            return ret;
            
        }
    }
}
