using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public int UserTypeId { get; set; }
        public string Company { get; set; }
        public string CreatedAt { get; set; }
    }
}