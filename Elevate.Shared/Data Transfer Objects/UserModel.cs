using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDisplayName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedAtText { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedAtText { get; set; }
    }
}