using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EmployeeDependentDTO : UserDTO
    {    
        public int EmployeeId { get; set; }
        public int RelationshipId { get; set; }
        public string RelationshipDisplayName { get; set; }
    }
}