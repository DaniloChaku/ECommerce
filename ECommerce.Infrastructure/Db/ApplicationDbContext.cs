using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Db
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRole = new ApplicationRole
            {
                Id = new Guid("D1EF53AC-AAB9-4FA0-BA8C-C3F69505A62E"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var customerRole = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            };

            modelBuilder.Entity<ApplicationRole>().HasData(adminRole);
            modelBuilder.Entity<ApplicationRole>().HasData(customerRole);

            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Manager2024No1");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
        }
    }
}
