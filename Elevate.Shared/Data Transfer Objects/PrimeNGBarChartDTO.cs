using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class PrimeNGBarChartDTO
    {
        public List<string> labels { get; set; }
        public List<PrimeNGBarChartDataSetDTO> datasets { get; set; }
    }
}