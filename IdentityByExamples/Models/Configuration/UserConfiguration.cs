using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityByExamples.Models.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            PasswordHasher<User> ph = new PasswordHasher<User>();
            builder.HasData(new User
            {
                Id = 1,
                Email = "admin@codaxy.com",
                EmailConfirmed = false,
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin@codaxy.com",
                NormalizedUserName = "ADMIN@CODAXY.COM",
                NormalizedEmail = "ADMIN@CODAXY.COM",
                PasswordHash = ph.HashPassword(null, "SuperSecret"),
                SecurityStamp = "QCLNYISX2LRR43w4J5UU23ZLFLPZBKPE",
            });
        }
    }
}
