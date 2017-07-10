using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoRental2.Models
{
    public class ChartPoints
    {
        public int id { get; set; }
        public string dataName { get; set; }
        public string type { get; set; }
        public List<Double> mag { get; set; }
        public List<Double> phase { get; set; }
        public ChartPoints(int idIn, string dataNameIn, string typeIn)
        {
            id = idIn;
            dataName = dataNameIn;
            type = typeIn;
            mag = new List<Double>();
            phase = new List<Double>();
        }
        public ChartPoints(int idIn, string dataNameIn, string typeIn, List<Double> magIn)
        {
            id = idIn;
            dataName = dataNameIn;
            type = typeIn;
            mag = magIn;
            phase = new List<Double>();
        }
        public ChartPoints(int idIn, string dataNameIn, string typeIn, List<Double> magIn, List<Double> phaseIn)
        {
            id = idIn;
            dataName = dataNameIn;
            type = typeIn;
            mag = magIn;
            phase = phaseIn;
        }
        public ChartPoints()
        {
            id = 0;
            dataName = "Undefined";
            type = "Undefined";
            mag = new List<Double>();
            phase = new List<Double>();
        }
    }
}