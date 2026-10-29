using CargoTrack.Business;
using CargoTrack.Business.Mappings;
using CargoTrack.Business.Services.Abouts;
using CargoTrack.Business.Services.Addresses;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.CargoPrices;
using CargoTrack.Business.Services.CargoPricing;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.Dashboards;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.Business.Services.Users;
using CargoTrack.Business.Services.Reports;
using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.Abouts;
using CargoTrack.DataAccess.Repositories.Addresses;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Cities;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});

builder.Services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
    config.LogoutPath = "/Login/Logout";
    config.AccessDeniedPath = "/ErrorPages/AccessDenied";
    config.Cookie.Name = "CargoTrackCookie";
});

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<BusinessAssembly>();

builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ICargoMovementRepository, CargoMovementRepository>();
builder.Services.AddScoped<ITransferCenterRepository, TransferCenterRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryExceptionRepository, DeliveryExceptionRepository>();
builder.Services.AddScoped<ICargoPriceRepository, CargoPriceRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<ICargoPricingService, CargoPricingService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<ITransferCenterService, TransferCenterService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICargoPriceService, CargoPriceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IReportService, ReportService>();
MapsterConfig.Configure();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();

    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());

    options.ModelBinderProviders.Insert(0, new CargoTrack.WebUI.ModelBinders.FlexibleNumberModelBinderProvider());

    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    foreach (var roleName in new[] { "Admin", "Manager", "User" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new AppRole { Name = roleName });
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    if (await userManager.FindByNameAsync("admin") is null)
    {
        var admin = new AppUser
        {
            UserName = "admin",
            Email = "admin@cargotrack.com",
            FirstName = "Sistem",
            LastName = "Yöneticisi"
        };
        await userManager.CreateAsync(admin, "Admin123*");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    async Task<AppUser> EnsureUserAsync(string userName, string password, string firstName, string lastName, string role)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
        {
            user = new AppUser { UserName = userName, Email = $"{userName}@cargotrack.com", FirstName = firstName, LastName = lastName };
            await userManager.CreateAsync(user, password);
        }
        if (!await userManager.IsInRoleAsync(user, role))
            await userManager.AddToRoleAsync(user, role);
        return user;
    }

    await EnsureUserAsync("musteri1", "Musteri123*", "Ayşe", "Kaya", "User");
    await EnsureUserAsync("musteri2", "Musteri123*", "Mehmet", "Demir", "User");

    var branches = await context.Branches.OrderBy(b => b.Name).ToListAsync();
    var managers = new[]
    {
        (UserName: "mudur1", FirstName: "Can", LastName: "Yılmaz"),
        (UserName: "mudur2", FirstName: "Elif", LastName: "Şahin")
    };
    for (var i = 0; i < managers.Length && i < branches.Count; i++)
    {
        var m = managers[i];
        var managerUser = await EnsureUserAsync(m.UserName, "Mudur123*", m.FirstName, m.LastName, "Manager");
        if (!await context.Employees.AnyAsync(e => e.AppUserId == managerUser.Id))
        {
            context.Employees.Add(new Employee { AppUserId = managerUser.Id, BranchId = branches[i].Id, Position = "Şube Müdürü" });
            await context.SaveChangesAsync();
        }
    }

    if (!await context.CargoPrices.AnyAsync())
    {
        var ranges = new[] { (0d, 5d, 90m), (5d, 15d, 140m), (15d, 30d, 220m), (30d, 100d, 380m) };
        foreach (var delivery in new[] { DeliveryType.Standard, DeliveryType.Express })
            foreach (var distance in new[] { DistanceType.SameCity, DistanceType.Intercity })
                foreach (var (min, max, price) in ranges)
                {
                    var factor = (delivery == DeliveryType.Express ? 1.5m : 1m) * (distance == DistanceType.Intercity ? 1.3m : 1m);
                    context.CargoPrices.Add(new CargoPrice
                    {
                        DeliveryType = delivery,
                        DistanceType = distance,
                        MinDesi = min,
                        MaxDesi = max,
                        Price = Math.Round(price * factor, 2)
                    });
                }
        await context.SaveChangesAsync();
    }

    var cargosWithoutCode = await context.Cargos.Where(c => c.DeliveryCode == null || c.DeliveryCode == "").ToListAsync();
    foreach (var cargo in cargosWithoutCode)
        cargo.DeliveryCode = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    if (cargosWithoutCode.Count > 0)
        await context.SaveChangesAsync();

    if (app.Environment.IsDevelopment())
        await CargoTrack.WebUI.Seed.SampleDataSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseExceptionHandler("/ErrorPages/Error");
app.UseStatusCodePagesWithReExecute("/ErrorPages/Status", "?code={0}");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization("tr-TR");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();
