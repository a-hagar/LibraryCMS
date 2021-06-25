using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCMS.Models.ViewModels
{
	public class DetailsLocation
	{
		public LocationDto SelectedLocation { get; set; }
		public IEnumerable<MemberDto> RelatedMembers { get; set; }
		public IEnumerable<BookDto> BookSelection { get; set; }
	}
}