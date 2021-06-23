using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityByExamples.Models
{
    public static class UserSeed
    {
        public static void Seed(ModelBuilder builder)
        {
            //seed user
            PasswordHasher<User> ph = new PasswordHasher<User>();
            builder.Entity<User>().HasData(new User {
                Id = 1,
                Email = "admin@codaxy.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin@codaxy.com",
                NormalizedUserName = "ADMIN@CODAXY.COM",
                NormalizedEmail = "ADMIN@CODAXY.COM",
                PasswordHash = ph.HashPassword(null, "SuperSecret"),
                SecurityStamp = "QCLNYISX2LRR43w4J5UU23ZLFLPZBKPE",

            });

            builder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Name = "Visitor",
                   NormalizedName = "VISITOR"
               },
               new IdentityRole
               {
                   Name = "Administrator",
                   NormalizedName = "ADMINISTRATOR"
               }
            );
        }  
        public static void Seed(ApplicationContext context)
        {
            //seed user
            PasswordHasher<User> ph = new PasswordHasher<User>();
            context.Users.AddRange(new[]
            {
                new User
                {
                Id = 1,
                Email = "admin@codaxy.com",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin@codaxy.com",
                NormalizedUserName = "ADMIN@CODAXY.COM",
                NormalizedEmail = "ADMIN@CODAXY.COM",
                PasswordHash = ph.HashPassword(null, "SuperSecret"),
                SecurityStamp = "QCLNYISX2LRR43w4J5UU23ZLFLPZBKPE",
                }
            });

            context.SaveChanges();
           
        }

    }
}
