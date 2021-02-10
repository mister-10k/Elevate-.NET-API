using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EmployeeDTO : UserDTO
    {
        public List<EmployeeDependentDTO> Dependents { get; set; }
    }
}