using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Models
{
    public class TableModel <T>
    {
        public List<T> Rows { get; set; }
        public int TotalCount { get; set; }
    }
}