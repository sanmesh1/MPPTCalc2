using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoRental2.Models;

namespace VideoRental2.ViewModels
{
    public class RandomMovieViewModel
    {
        public Movie randomMovie { get; set; }
        public List<Customer> customers { get; set; }
    }
}