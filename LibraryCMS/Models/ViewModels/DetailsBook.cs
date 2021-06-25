using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCMS.Models.ViewModels
{
    public class DetailsBook
    {

        public BookDto SelectedBook { get; set; }

        public IEnumerable<LocationDto> CurrentLocation { get; set; }
    }
}