using CloudinaryDotNet;
using ENN_Cargo.Core;
using ENN_Cargo.DataAccess;
using ENN_Cargo.DataAccess.Repository;
using ENN_Cargo.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;using ENN_Cargo.Utility;var builder = WebApplication.CreateBuilder(args);builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ENN_CargoApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ENN Cargo.DataAccess")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ENN_CargoApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICompanyStockService, CompanyStockService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ITruckCompanyService, TruckCompanyService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddRazorPages();var cloudinarySettings = builder.Configuration
                         .GetSection("Cloudinary")
                         .Get<CloudinarySettings>();
var account = new Account(cloudinarySettings.CloudName,
cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
var cloudinary = new Cloudinary(account);builder.Services.AddSingleton(cloudinary);var app = builder.Build();if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                await context.Response.WriteAsync($"Error: {exceptionFeature.Error.Message}\nStackTrace: {exceptionFeature.Error.StackTrace}");
            }
            else
            {
                await context.Response.WriteAsync($"Unknown 400 error. Request Path: {context.Request.Path}\nHeaders: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}\nQuery: {context.Request.QueryString}");
            }
        });
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path} {context.Request.QueryString} - Authenticated: {context.User?.Identity?.IsAuthenticated} - Roles: {string.Join(", ", context.User?.Claims.Where(c => c.Type == "role").Select(c => c.Value) ?? new List<string>())}");
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Middleware Exception: {ex.Message}\n{ex.StackTrace}"    );
        throw;
    }
});app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await CreateRoles(services);
        await CreateAdmin(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding roles/admin: {ex.Message}");
        throw;
    }
}app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");app.Run();static async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Driver", "TruckCompany", "ShipmentCompany" };    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Failed to create role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }
}static async Task CreateAdmin(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var adminEmail = "ENNCargo@admin.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);    if (adminUser == null)
    {
        var user = new IdentityUser { UserName = adminEmail, Email = adminEmail };
        var result = await userManager.CreateAsync(user, "ENN_Cargo06");        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
        else
        {
            throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}