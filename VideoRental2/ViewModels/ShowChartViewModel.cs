using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoRental2.Models;

namespace VideoRental2.ViewModels
{
    public class ShowChartViewModel
    {
        public List<ChartPoints> dataSets { get; set; }
        public int xVal { get; set; }
        public int yVal { get; set; }
        public string sessionId { get; set; }
        public ShowChartViewModel(List<ChartPoints> dataSetsIn, int xValIn, int yValIn, string sessionIdIn)
        {
            dataSets = dataSetsIn;
            xVal = xValIn;
            yVal = yValIn;
            sessionId = sessionIdIn;
        }
        public ShowChartViewModel()
        {
            dataSets = new List<ChartPoints>();
        }
    }

}
