using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VitaPoint.Server.Models;

namespace VitaPoint.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed user roles
            // Adding moderator role for future scaling of system if necessary

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1c44257b-cb83-4cd7-be7e-b90cf8d13e66",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1c44257b-cb83-4cd7-be7e-b90cf8d13e66"
                },
                new IdentityRole
                {
                    Id = "a9544858-d9ee-4964-9d75-5a7d5645214e",
                    Name = "Moderator",
                    NormalizedName = "MODERATOR",
                    ConcurrencyStamp = "a9544858-d9ee-4964-9d75-5a7d5645214e"
                },
                new IdentityRole
                {
                    Id = "4cf48e40-bd1c-478b-be12-40ccc232fe61",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "4cf48e40-bd1c-478b-be12-40ccc232fe61"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
