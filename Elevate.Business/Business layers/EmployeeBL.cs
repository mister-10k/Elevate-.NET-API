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

        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employee)
        {
            return await employeeDL.CreateEmployeeAsync(employee);
        }
        public async Task<EmployeeDTO> GetEmployeeAsync(int id)
        {
            return await employeeDL.GetEmployeeAsync(id);
        }

        public async Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employee)
        {
            return await employeeDL.UpdateEmployeeAsync(employee);
        }

        public async Task<EmployeeDTO> DeleteEmployeeAsync(int id)
        {
            return await employeeDL.DeleteEmployeeAsync(id);
        }

        public async Task<List<EBDashbaordStatsCardDTO>> GetEBDashboardCardsDataAsync(int companyId)
        {
            List<EBDashbaordStatsCardDTO> ret = null;
            try
            {
                ret = new List<EBDashbaordStatsCardDTO>();
                var employees = await employeeDL.GetAllEmployeesForComapnyAsync(companyId);
                ret.Add(new EBDashbaordStatsCardDTO
                {
                    Title = AppConstants.StatCardTitle.MyEmployees,
                    Number = employees.Count,
                    Color = AppConstants.AppColor.Primary
                });
                ret.Add(new EBDashbaordStatsCardDTO
                {
                    Title = AppConstants.StatCardTitle.EmployeeDependents,
                    Number = GetNumberOfDependentsForEmployees(employees),
                    Color = AppConstants.AppColor.Secondary
                });
                var totalYearDeductions = GetTotalForYearDeductionsForEmployees(employees);
                ret.Add(new EBDashbaordStatsCardDTO
                {
                    Title = AppConstants.StatCardTitle.DeductionBiWeeklyTotal,
                    Number = totalYearDeductions/26,
                    Color = AppConstants.AppColor.Tertiary,
                    IsCurrency = true
                });
                ret.Add(new EBDashbaordStatsCardDTO
                {
                    Title = AppConstants.StatCardTitle.DeductionPerMonthTotal,
                    Number = totalYearDeductions / 12,
                    Color = AppConstants.AppColor.Tertiary,
                    IsCurrency = true
                });
                ret.Add(new EBDashbaordStatsCardDTO
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

        private int GetNumberOfDependentsForEmployees(List<EmployeeDTO> employees)
        {
            var count = 0;
            foreach (var employee in employees)
            {
                count += employee.Dependents.Count;
            }
            return count;
        }

        private double GetTotalForYearDeductionsForEmployees(List<EmployeeDTO> employees)
        {
            double total = 0.0;
            foreach(var employee in employees)
            {
                total += EmployeeBenifitsUtility.GetEmployeeDeductionCost(employee);
            }
            return total;
        }

        public async Task<TableDTO<EmployeeDTO>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestDTO requestDTO)
        {
            return await employeeDL.GetEmployeesForEBDashboardAsync(requestDTO);
        }

        public async Task<EmployeeFormMasterDataDTO> GetEmployeeFormMasterDataAsync()
        {
            return await employeeDL.GetEmployeeFormMasterDataAsync();
        }

        public async Task<PrimeNGBarChartDTO> GetTop10HighestEmployeeDedcutions(int companyId)
        {
            var ret = new PrimeNGBarChartDTO();

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

                var dataset = new PrimeNGBarChartDataSetDTO
                {
                    label = "Highest Employee Deductions",
                    backgroundColor = AppConstants.AppColor.Primary,
                    borderColor = "#1E88E5",
                    data = employees.Select(x => x.TotalDeduction).ToList()
                };
                ret.datasets = new List<PrimeNGBarChartDataSetDTO> { dataset };
            }

            return ret;
            
        }
    }
}
