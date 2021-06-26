using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCMS.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        public string BookTitle { get; set; }
        public string AuthorFname { get; set; }
        public string AuthorLname { get; set; }
        public string genre { get; set; }
        public string ISBN  { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }

        //data for keeping track of image uploads
        //images deposited into /Content/Images/Book/default.png
        public bool BookHasPic { get; set; }
        public string PicExtension { get; set; }

        //a book can be in multiple locations
        public ICollection<Location> Location { get; set; }
    }

    public class BookDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string AuthorFname { get; set; }
        public string AuthorLname { get; set; }
        public string genre { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }

        public bool BookHasPic { get; set; }
        public string PicExtension { get; set; }
    }


}