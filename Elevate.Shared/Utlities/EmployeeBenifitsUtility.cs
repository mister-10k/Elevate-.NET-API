using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Shared
{
    public static class EmployeeBenifitsUtility
    {
        public static double GetEmployeeDeductionCost(EmployeeDTO employee)
        {
            double total = 0.0;
            double employeeCost = AppConstants.EmployeeBenifits.CostOfEmployeeBenifits;
            if (employee.FirstName.ToUpper().StartsWith("A") )
            {
                employeeCost = employeeCost - (employeeCost * AppConstants.EmployeeBenifits.NameStartsWithADiscount);
            }
            total += employeeCost;

            foreach(var dependent in employee.Dependents)
            {
                double dependentCost = AppConstants.EmployeeBenifits.CostOfDependent;
                if (dependent.FirstName.ToUpper().StartsWith("A"))
                {
                    dependentCost = dependentCost - (dependentCost * AppConstants.EmployeeBenifits.NameStartsWithADiscount);
                }
                total += dependentCost;
            }

            return Math.Round(total,2);
        }
    }
}
