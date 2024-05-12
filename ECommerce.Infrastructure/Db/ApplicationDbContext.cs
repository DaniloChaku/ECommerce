using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Db
{
    /// <summary>
    /// Database context for the application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        /// <summary>
        /// DbSet for categories.
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// DbSet for manufacturers.
        /// </summary>
        public DbSet<Manufacturer> Manufacturers { get; set; }

        /// <summary>
        /// DbSet for products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DbSet for shopping cart items.
        /// </summary>
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the DbContext.</param>
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Configures the database schema and seed initial data.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance used to construct the EF model.</param>
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
                Id = new Guid("5a598257-8e8b-4d63-bc64-d1068ce37f58"),
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            };

            modelBuilder.Entity<ApplicationRole>().HasData(adminRole);
            modelBuilder.Entity<ApplicationRole>().HasData(customerRole);

            // Seed admin user
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

            // Associate admin user with admin role
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
        }
    }
}
