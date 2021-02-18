using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public class AppConstants
    {
        public struct UserType
        {
            public const string BenifitsManager = "BenifitsManager";
            public const string Employee = "Employee";
        }

        public struct EmployeeBenifits 
        {
            public const int EmployeePaycheckAmount = 2000;
            public const int PaycheckInAYear = 26;
            public const int CostOfEmployeeBenifits = 1000;
            public const int CostOfDependent = 500;
            public const double NameStartsWithADiscount = 0.1;
        }

        public struct AppColor
        {
            public const string Primary = "#F03A17";
            public const string Secondary = "#007bff";
            public const string Tertiary = "grey";
        }

        public struct StatCardTitle
        {
            public const string MyEmployees = "My Employees";
            public const string EmployeeDependents = "Employee Dependents";      
            public const string DeductionBiWeeklyTotal = "Deduction Total bi-weekly";
            public const string DeductionPerMonthTotal = "Deduction Total per month";
            public const string DeductionTotalPerYearTotal = "Deduction Total per year";
        }
    }
}
