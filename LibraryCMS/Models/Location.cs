using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCMS.Models
{
    public class Location
    {
        //lists library locations: names, address, postal codes
        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string PostalCode{ get; set; }

        //data for keeping track of image uploads
        //images deposited into /Content/Images/Location/default.png
        public bool LocationHasPic { get; set; }
        public string PicExtension { get; set; }

        //a location can have many books
        public ICollection<Book> Book { get; set; }

    }

    public class LocationDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public bool LocationHasPic { get; set; }
        public string PicExtension { get; set; }
    }
}