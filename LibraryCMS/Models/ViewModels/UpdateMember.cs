using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCMS.Models.ViewModels
{
    public class UpdateMember
    {
        //stores info for the Member/Updates/{id}
        
        //existing member data
        public MemberDto SelectedMember { get; set; }

        //all locations when updating member data
        public IEnumerable<LocationDto> LocationsOptions { get; set; }
    }
}