using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EmployeeModel : UserModel
    {
        public int NumberOfDependents { get; set; }
        public List<EmployeeDependentModel> Dependents { get; set; }
        public double TotalDeduction { get; set; }
    }
}