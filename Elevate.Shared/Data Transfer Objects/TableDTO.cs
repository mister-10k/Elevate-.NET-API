using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class TableDTO <T>
    {
        public List<T> Rows { get; set; }
        public int TotalCount { get; set; }
    }
}