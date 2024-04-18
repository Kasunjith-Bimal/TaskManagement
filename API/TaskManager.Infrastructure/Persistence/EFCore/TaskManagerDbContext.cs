
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.EFCore
{
    public class TaskManagerDbContext : IdentityDbContext<UserDetail>
    {
        public DbSet<TaskDetail> TaskDetails { get; set; }
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options)
        {
            // The base constructor handles initializing the DbContext with the provided options.
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = Guid.NewGuid().ToString();
            // Seed roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" }
            );

            var adminUserId = Guid.NewGuid().ToString();

            var adminUser = new UserDetail
            {
                Id = adminUserId,
                UserName = "kasunysoft@gmail.com",
                NormalizedUserName = "KASUNYSOFT@GMAIL.COM",
                Email = "kasunysoft@gmail.com",
                NormalizedEmail = "KASUNYSOFT@GMAIL.COM",
                EmailConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                JoinDate = new DateTime(),
                FullName ="Mirahampe Patisthana Gedara Kasunjith Bimal Lakshitha",
                PhoneNumber = "0716063159",
                IsFirstLogin = true,
                IsActive = true,
            };

            var hasher = new PasswordHasher<UserDetail>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "KasunJith123@");

            builder.Entity<UserDetail>().HasData(adminUser);

            // Assign the Admin user to the Admin role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
        }
    }
}
