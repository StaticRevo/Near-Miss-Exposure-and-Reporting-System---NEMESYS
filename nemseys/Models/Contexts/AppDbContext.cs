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
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Uncategorised"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Ex1"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Ex2"
                }
            );
            base.OnModelCreating(modelBuilder);
           
        }

    }
}
