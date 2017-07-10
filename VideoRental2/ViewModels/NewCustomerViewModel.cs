using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoRental2.Models;

namespace VideoRental2.ViewModels
{
    public class NewCustomerViewModel
    {
        public IEnumerable<MembershipType> MembershipType { get; set; }
        public Customer Customer { get; set; }
    }
}