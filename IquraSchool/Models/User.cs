using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models
{
    public class User : IdentityUser
    {
        [Range(1945, 2017)]
        public int Year { get; set; }
    }
}
