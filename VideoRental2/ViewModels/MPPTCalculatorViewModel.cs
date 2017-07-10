using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoRental2.Models;

namespace VideoRental2.ViewModels
{
    public class MPPTCalculatorViewModel
    {
        public int xData { get; set; }
        public int yData { get; set; }
        public List<ChartPoints> dataSets { get; set; }
        public int lengthArray { get; set; }
        public BuckBoostParams buckBoostParams { get; set; }
    }
}