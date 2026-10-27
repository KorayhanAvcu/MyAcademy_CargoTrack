using CargoTrack.Business;
using CargoTrack.Business.Mappings;
using CargoTrack.Business.Services.Abouts;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.Cities;
using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.Abouts;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.Cities;
using CargoTrack.Entity.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.CargoPrices;
using CargoTrack.Business.Services.CargoPricing;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Dashboards;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.Business.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// DATABASE
// =====================================================

builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ICargoMovementRepository, CargoMovementRepository>();
builder.Services.AddScoped<ITransferCenterRepository, TransferCenterRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryExceptionRepository, DeliveryExceptionRepository>();
builder.Services.AddScoped<ICargoPriceRepository, CargoPriceRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseLazyLoadingProxies();
});

// =====================================================
// IDENTITY
// =====================================================

builder.Services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
    config.LogoutPath = "/Login/Logout";
    config.AccessDeniedPath = "/ErrorPages/AccessDenied";   // ⚠️ bu controller henüz yok
    config.Cookie.Name = "CargoTrackCookie";
});

// =====================================================
// VALIDATION
// =====================================================

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<BusinessAssembly>();

// =====================================================
// REPOSITORIES
// =====================================================

builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
// ➕ Yeni repository'ler buraya (ICargoRepository, ICargoMovementRepository...)

// =====================================================
// SERVICES
// =====================================================

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

// =====================================================
// MAPSTER
// =====================================================

MapsterConfig.Configure();

builder.Services.AddControllersWithViews();

// =====================================================
// APP / MIDDLEWARE
// =====================================================

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // önce "kimsin?"
app.UseAuthorization();    // sonra "yetkin var mı?"

// =====================================================
// ROUTES
// =====================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();