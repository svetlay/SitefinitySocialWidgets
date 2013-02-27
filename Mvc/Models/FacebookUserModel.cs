using System;
using System.Collections.Generic;
using System.Linq;

namespace SitefinityWebApp.Mvc.Models
{
    public class FacebookUserModel
    {
        public string UserId { get; set; }


        public string Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Name{ get; set; }

        public string Birthday { get; set; }

        public string Location { get; set; }

        public List<String> CustomFields { get; set; }

        public string ProfileImageUrl { get; set; }

        public string CoverImaegUrl { get; set; }

    }
}