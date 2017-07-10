using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoRental2.Models;

namespace VideoRental2.ViewModels
{
    public class NewMovieViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Genre> Genre { get; set; }

    }
}