using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class EmployeeModel : PersonModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDisplayName { get; set; }
        public List<EmployeeDependentModel> Dependents { get; set; }
    }
}