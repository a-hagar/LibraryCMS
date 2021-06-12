using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryCMS.Models
{
    public class Member
    {
        //create table for members: names, contact info, location preference referencing location table
        [Key]
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public string eMail { get; set; }
        public DateTime RegistrationDate { get; set; }

        //a user belongs to a single location
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        public virtual Location Locations { get; set; }
    }

    public class MemberDto
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public string eMail { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}