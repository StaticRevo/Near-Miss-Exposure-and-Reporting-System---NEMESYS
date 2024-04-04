using Microsoft.EntityFrameworkCore;
using Nemesis.Models; 

namespace Nemesis.Models.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
