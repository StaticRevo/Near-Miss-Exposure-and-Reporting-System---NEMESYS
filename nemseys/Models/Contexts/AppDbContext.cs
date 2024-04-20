using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;

namespace Nemesys.Models.Contexts
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //This will pass any options passed in the constructor to the base class DbContext
        }

        public DbSet<Report> Reports { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setup schemd for identity framework
            base.OnModelCreating(modelBuilder);

            //Seed roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "d234f58e-7373-4ee5-98f0-c17892784b05", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
                new IdentityRole() { Id = "1db56103-a3e2-4edc-afab-abde856cebe0", Name = "User", ConcurrencyStamp = "1", NormalizedName = "USER" }
            );


            //Seed admin user
            IdentityUser user = new IdentityUser()
            {
                Id = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32", //https://www.guidgenerator.com/online-guid-generator.aspx
                UserName = "admin@mail.com", //Has to be the email address for the login logic to work
                NormalizedUserName = "ADMIN@MAIL.COM ",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = ""
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "S@fePassw0rd1"); //make sure you adhere to policies (incl confirmed etc…)
            modelBuilder.Entity<IdentityUser>().HasData(user);

            //Assign existing user to the admin role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "d234f58e-7373-4ee5-98f0-c17892784b05", UserId = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32" }
            );


            modelBuilder.Entity<Report>().HasData(
               new Report
               {
                   ReportId = 1,
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
