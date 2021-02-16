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

        public struct AppColors
        {
            public const string main = "#F03A17";
        }
    }
}
