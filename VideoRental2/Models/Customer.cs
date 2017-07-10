using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; //to use [Required] and other constraints/data annotations

namespace VideoRental2.Models
{
    public class Customer
    {
        public byte? id { get; set; }
        [Required(ErrorMessage = "Please enter customer's name")]
        [StringLength(255)]
        public string name { get; set; }
        public bool isSubscribedToNewsLetter { get; set; }
        public MembershipType MembershipType { get; set; }
        //[Range(1,10)]
        //[Compare("OtherProperty")]
        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; } //foreign key to access specific membership type
        [Display(Name = "Date of Birth")]
        [Min18YearsIfAMember]
        public DateTime? birthday { get; set; }
    }
}