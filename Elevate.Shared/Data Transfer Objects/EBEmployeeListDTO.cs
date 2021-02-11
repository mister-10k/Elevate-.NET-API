using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EBEmployeeListDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public int Dependents { get; set; }
        public string CreatedAt { get; set; }
        public int TotalCount { get; set; }
    }
}