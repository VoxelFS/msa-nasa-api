using Microsoft.EntityFrameworkCore;
using msa_nasa_project.Models;

namespace msa_nasa_project.Data
{
    public class UsersDbContext : DbContext 
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Users> Users { get; set; }
    }
}
