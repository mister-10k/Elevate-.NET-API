using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EmployeeDependentDTO : PersonDTO
    {    
        public int EmployeeId { get; set; }
        public int RelationshipId { get; set; }
        public string RelationshipDisplayName { get; set; }
    }
}