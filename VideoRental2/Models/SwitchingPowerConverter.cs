using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoRental2.Models
{
    public class SwitchingPowerConverter
    {

        public int id { get; set; }

        //inputs
        public double vinMin { get; set; }
        public double vinMax { get; set; }
        public double voutMin { get; set; }
        public double voutMax { get; set; }
        public double iLoadAvgMax { get; set; }
        public double iRipplePerc { get; set; } //need to convert to decimal
        protected double iRipple { get; set; }
        public double vRipplePerc { get; set; } //need to convert to decimal
        protected double vRipple { get; set; }
        public double freq { get; set; } //chosen from database
        protected double period { get; set; }

        //Controller - Outputs
        public double dMin { get; set; }
        public double dMax { get; set; }
        public double iLoadCrit { get; set; }

        //Inductor - Outputs
        public double inductance { get; set; }
        public double iLMax { get; set; }
        public double vLMax { get; set; }
        public double pL { get; set; }
        protected double iLRipple { get; set; }

        //Capacitor - Outputs
        public double capacitance { get; set; }
        public double iCMax { get; set; }
        public double vCMax { get; set; }

        //test params
        //inputs
        public double vinTest { get; set; } //for test
        public double voutTest { get; set; } //for test
        //freq
        //iLoadAvgMax
        //iRipplePerc
        //vRipplePerc


        //outputs
        public double dTest { get; set; } //for test

    }
}