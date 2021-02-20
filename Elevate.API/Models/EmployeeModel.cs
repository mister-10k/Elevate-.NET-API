using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
        public string CreatedAt { get; set; }
        public List<EmployeeDependentModel> Dependents { get; set; }
    }
}