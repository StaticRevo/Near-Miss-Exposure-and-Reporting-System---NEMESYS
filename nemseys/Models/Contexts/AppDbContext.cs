using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Nemesys.Models;

namespace Nemesys.Models.Contexts
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Investigation> Investigations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setup schemd for identity framework
            base.OnModelCreating(modelBuilder);

            //Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "d234f58e-7373-4ee5-98f0-c17892784b05", Name = "Reporter", ConcurrencyStamp = "1", NormalizedName = "REPORTER" },
                new IdentityRole() { Id = "1db56103-a3e2-4edc-afab-abde856cebe0", Name = "Investigator", ConcurrencyStamp = "1", NormalizedName = "INVESTIGATOR" }
            );


            //Seed admin user
            ApplicationUser user = new ApplicationUser()
            {
                Id = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32", //https://www.guidgenerator.com/online-guid-generator.aspx
                UserName = "admin@mail.com", //Has to be the email address for the login logic to work
                NormalizedUserName = "ADMIN@MAIL.COM ",
                Email = "admin@mail.com",
                AuthorAlias = "Admin",
                NormalizedEmail = "ADMIN@MAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = ""
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "S@fePassw0rd1"); //make sure you adhere to policies (incl confirmed etc…)
            modelBuilder.Entity<ApplicationUser>().HasData(user);

            //Assign existing user to the admin role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "d234f58e-7373-4ee5-98f0-c17892784b05", UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32" }
            );


            modelBuilder.Entity<Report>().HasData(
               new Report
               {
                   ReportId = 1,
                   UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                   DateOfReport = DateTime.UtcNow,
                   HazardLocation = "Conference Room B",
                   DateAndTimeSpotted = DateTime.UtcNow,
                   TypeOfHazard = "Fire",
                   TitleOfReport = "AGA Today",
                   Description = "Today's AGA is characterized by a series of discussions and debates around ...",
                   Status = "New",
                   ImageUrl = "/images/seed1.jpg",
                   Upvotes = 0
               },
               new Report
               {
                   ReportId = 2,
                   UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                   DateOfReport = DateTime.UtcNow.AddDays(-1),
                   HazardLocation = "Main Street",
                   DateAndTimeSpotted = DateTime.UtcNow.AddDays(-1),
                   TypeOfHazard = "Traffic",
                   TitleOfReport = "Traffic is incredible",
                   Description = "Today's traffic can't be described using words. Only an image can do that ...",
                   Status = "Ongoing",
                   ImageUrl = "/images/seed2.jpg",
                   Upvotes = 10
               },
               new Report
               {
                   ReportId = 3,
                   UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32",
                   DateOfReport = DateTime.UtcNow.AddDays(-2),
                   HazardLocation = "Local Park",
                   DateAndTimeSpotted = DateTime.UtcNow.AddDays(-2),
                   TypeOfHazard = "Weather",
                   TitleOfReport = "When is Spring really starting?",
                   Description = "Clouds clouds all around us. I thought spring started already, but ...",
                   Status = "Closed",
                   ImageUrl = "/images/seed3.jpg",
                   Upvotes = 5
               }
            );
        }

    }
}
