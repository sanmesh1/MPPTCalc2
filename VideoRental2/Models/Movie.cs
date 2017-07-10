using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace VideoRental2.Models
{
    public class Movie
    {
        public string name { get; set; }
        public byte? id { get; set; }
        [Display(Name = "Release Date")]
        public DateTime releaseDate { get; set; }
        [Display(Name = "Date Added (Ex. 01/13/14)")]
        public DateTime dateAdded { get; set; }
        [Display(Name = "# in stock")]
        public int numberInStock { get; set; }
        public Genre Genre { get; set; }
        [Display(Name = "Genre")]
        public byte GenreId { get; set; }
    }
}