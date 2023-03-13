using Microsoft.AspNetCore.Identity;

namespace IquraSchool.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
