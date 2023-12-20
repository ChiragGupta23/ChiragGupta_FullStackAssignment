using Microsoft.EntityFrameworkCore;
using ChiragGupta_FullStackAssignment.Models;

namespace ChiragGupta_FullStackAssignment.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        
        public DbSet<UserModel> Users { get; set; }

        public DbSet<Login> Login { get; set; }

    }
}
