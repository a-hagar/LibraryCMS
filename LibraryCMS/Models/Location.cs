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

        //a location can have many books
        public ICollection<Book> Books { get; set; }
    }

    public class LocationDto
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}