using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elevate.Shared
{
    public class EBDashbaordStatsCardModel
    {
        public string Title { get; set; }
        public string Color { get; set; }
        public double Number { get; set; }
        public bool IsCurrency { get; set; }

    }
}