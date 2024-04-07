using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.Core.Services.Category;
using ECommerce.Core.Services.Manufacturer;
using ECommerce.Infrastructure.Db;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryGetterService, CategoryGetterService>();
builder.Services.AddScoped<ICategoryAdderService, CategoryAdderService>();
builder.Services.AddScoped<ICategoryUpdaterService, CategoryUpdaterService>();
builder.Services.AddScoped<ICategoryDeleterService, CategoryDeleterService>();

builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IManufacturerGetterService, ManufacturerGetterService>();
builder.Services.AddScoped<IManufacturerAdderService, ManufacturerAdderService>();
builder.Services.AddScoped<IManufacturerUpdaterService, ManufacturerUpdaterService>();
builder.Services.AddScoped<IManufacturerDeleterService, ManufacturerDeleterService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
