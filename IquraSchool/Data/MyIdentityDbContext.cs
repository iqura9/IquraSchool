using IquraSchool.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IquraSchool.Data
{
    public class MyIdentityDbContext : IdentityDbContext<User>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
