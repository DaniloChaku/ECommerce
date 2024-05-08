using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.Core.ServiceContracts.Image;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using ECommerce.Core.Services.Category;
using ECommerce.Core.Services.Image;
using ECommerce.Core.Services.Manufacturer;
using ECommerce.Core.Services.Products;
using ECommerce.Core.Services.ShoppingCartItem;
using ECommerce.Core.Services.Users;
using ECommerce.Core.Settings;
using ECommerce.Infrastructure.Db;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.UI.StartupExtensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.Configure<ImageUploadOptions>(configuration.GetSection("ImageUpload"));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryGetterService, CategoryGetterService>();
            services.AddScoped<ICategoryAdderService, CategoryAdderService>();
            services.AddScoped<ICategoryUpdaterService, CategoryUpdaterService>();
            services.AddScoped<ICategoryDeleterService, CategoryDeleterService>();
            services.AddScoped<ICategorySorterService, CategorySorterService>();

            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IManufacturerGetterService, ManufacturerGetterService>();
            services.AddScoped<IManufacturerAdderService, ManufacturerAdderService>();
            services.AddScoped<IManufacturerUpdaterService, ManufacturerUpdaterService>();
            services.AddScoped<IManufacturerDeleterService, ManufacturerDeleterService>();
            services.AddScoped<IManufacturerSorterService, ManufacturerSorterService>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductGetterService, ProductGetterService>();
            services.AddScoped<IProductAdderService, ProductAdderService>();
            services.AddScoped<IProductUpdaterService, ProductUpdaterService>();
            services.AddScoped<IProductDeleterService, ProductDeleterService>();
            services.AddScoped<IProductSorterService, ProductSorterService>();

            services.AddScoped<IImageUploaderService, ImageUploaderService>();
            services.AddScoped<IImageDeleterService, ImageDeleterService>();

            services.AddScoped<IShoppingCartItemRepository, ShoppingCartItemRepository>();
            services.AddScoped<IShoppingCartItemAdderService, ShoppingCartItemAdderService>();
            services.AddScoped<IShoppingCartItemGetterService, ShoppingCartItemGetterService>();
            services.AddScoped<IShoppingCartItemUpdaterService, ShoppingCartItemUpdaterService>();
            services.AddScoped<IShoppingCartItemDeleterService, ShoppingCartItemDeleterService>();

            services.AddScoped<IUserContextService, UserContextService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                options.Password.RequireNonAlphanumeric = false
            )
                .AddEntityFrameworkStores<ApplicationDbContext>();


            return services;
        }
    }
}
