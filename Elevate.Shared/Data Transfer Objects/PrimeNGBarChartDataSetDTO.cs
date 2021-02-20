using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class PrimeNGBarChartDataSetDTO
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public List<double> data { get; set; }
    }
}