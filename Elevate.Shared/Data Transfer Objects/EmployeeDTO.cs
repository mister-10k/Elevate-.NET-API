using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EmployeeDTO : PersonDTO
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDisplayName { get; set; }
        public List<EmployeeDependentDTO> Dependents { get; set; }
    }
}