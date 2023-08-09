using Microsoft.EntityFrameworkCore;
using msa_nasa_project.Models;

namespace msa_nasa_project.Data
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options) { }

        public DbSet<Images> Images { get; set; }
    }
}
