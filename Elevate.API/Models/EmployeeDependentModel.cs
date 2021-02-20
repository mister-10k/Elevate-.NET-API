using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class EmployeeDependentModel
    {    
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedAt { get; set; }
        public int RelationshipId { get; set; }
        public string Relationship { get; set; }
    }
}