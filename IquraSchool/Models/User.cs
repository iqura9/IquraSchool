using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IquraSchool.Models
{
    public class User : IdentityUser
    {
        [Range(1945, 2017)]
        public int Year { get; set; }
        public int? StudentId { get; set; }
        public int? TeacherId { get; set; }
        public virtual Student Student { get; set; } = null;
        public virtual Teacher Teacher { get; set; } = null;
        
    }
}
