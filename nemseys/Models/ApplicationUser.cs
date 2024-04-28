using Microsoft.AspNetCore.Identity;

namespace Nemesys.Models
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public string AuthorAlias { get; set; }

        [PersonalData]
        public int ClosedReportsCount { get; set; }
    }

}

