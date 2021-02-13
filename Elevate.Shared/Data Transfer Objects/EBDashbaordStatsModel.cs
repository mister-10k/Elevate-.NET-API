using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EBDashbaordStatsModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public int NumberOfEmployees { get; set; }
        public int NumberOfEmployeeDependents { get; set; }
        public double DeductionBiWeeklyTotal { get; set; }
        public double DeductionPerMonthTotal { get; set; }
        public double DeductionTotalPerYearTotal { get; set; }
    }
}