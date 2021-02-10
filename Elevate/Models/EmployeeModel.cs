using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class EmployeeModel : UserModel
    {
        public List<EmployeeDependentModel> Dependents { get; set; }
    }
}