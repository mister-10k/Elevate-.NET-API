using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class ListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }

    }
}