using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class EBEmployeeListRequestModel
    {
        public int CompanyId { get; set; }
        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public string SortColumn { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}