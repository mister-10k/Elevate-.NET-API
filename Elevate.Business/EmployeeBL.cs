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
        public async Task<EmployeeModel> CreateEmployeeAsync(EmployeeModel employee)
        {
            return await employeeDL.CreateEmployeeAsync(employee);
        }
        public async Task<EmployeeModel> GetEmployeeAsync(int id)
        {
            return await employeeDL.GetEmployeeAsync(id);
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee)
        {
            return await employeeDL.UpdateEmployeeAsync(employee);
        }

        public async Task<EmployeeModel> DeleteEmployeeAsync(int id)
        {
            return await employeeDL.DeleteEmployeeAsync(id);
        }

        public async Task<List<EBDashbaordStatsCardModel>> GetEBDashboardCardsDataAsync(int companyId)
        {
            List<EBDashbaordStatsCardModel> ret = null;
            try
            {
                ret = new List<EBDashbaordStatsCardModel>();
                var employees = await employeeDL.GetAllEmployeesForComapnyAsync(companyId);
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
                Console.WriteLine("Business Layer: GetEBDashboardCardsDataAsync Exception Msg", ex.Message);
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

        public async Task<TableModel<EmployeeModel>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestModel requestModel)
        {
            return await employeeDL.GetEmployeesForEBDashboardAsync(requestModel);
        }

        public async Task<EmployeeFormMasterDataModel> GetEmployeeFormMasterDataAsync()
        {
            return await employeeDL.GetEmployeeFormMasterDataAsync();
        }

        public async Task<PrimeNGBarChartModel> GetTop10HighestEmployeeDedcutions(int companyId)
        {
            var ret = new PrimeNGBarChartModel();

            var employees = await employeeDL.GetAllEmployeesForComapnyAsync(companyId);

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
