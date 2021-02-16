using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class PrimeNGBarChartModel
    {
        public List<string> labels { get; set; }
        public List<PrimeNGBarChartDataSetModel> datasets { get; set; }
    }
}