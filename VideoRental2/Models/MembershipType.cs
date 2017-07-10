using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoRental2.Models
{
    public class MembershipType
    {
        public byte id { get; set; }
        public string name { get; set; }
        public short signUpFee { get; set; }
        public byte durationInMonths { get; set; }
        public byte discountRate { get; set; }
    }
}