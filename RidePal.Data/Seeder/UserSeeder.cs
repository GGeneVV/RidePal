using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RidePal.Models;
using System;

namespace RidePal.Data.Seeder
{

    public static class UserSeeder

    {
        public static void SeedRoles(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<User>();
            var adminUser = new User();
            adminUser.Id = Guid.NewGuid();
            adminUser.CreatedOn = DateTime.UtcNow;
            adminUser.FirstName = "Gencho";
            adminUser.LastName = "Genev";
            adminUser.UserName = "gigenev@gmail.com";
            adminUser.NormalizedUserName = "GIGENEV@ADMIN.COM";
            adminUser.Email = "gigenev@gmail.com";
            adminUser.NormalizedEmail = "GIGENEV@GMAIL.COM";
            adminUser.EmailConfirmed = true;
            adminUser.Image = "~/images/Profile.jpg"; // To set image,
            adminUser.IsDeleted = false;
            adminUser.IsBanned = false;
            adminUser.IsAdmin = true;
            adminUser.LockoutEnabled = false;
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");
            adminUser.SecurityStamp = Guid.NewGuid().ToString();
            builder.Entity<User>().HasData(adminUser);
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();

            builder.Entity<Role>().HasData(
                new Role() { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new Role() { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );
            builder.Entity<IdentityUserRole<Guid>>().HasData(
                 new IdentityUserRole<Guid>()
                 {
                     RoleId = adminRoleId,
                     UserId = adminUser.Id
                 }
             );

        }


    }

}


