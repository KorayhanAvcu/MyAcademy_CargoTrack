
using CargoTrack.Business;
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

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// SERVICES
// =====================================================

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<BusinessAssembly>();

builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();

builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));

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

builder.Services.AddControllersWithViews();


// =====================================================
// APP
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

app.UseAuthentication();
app.UseAuthorization();


// =====================================================
// ROUTES
// =====================================================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// =====================================================
// SEED DATA
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // -------------------------------------------------
    // DATABASE
    // -------------------------------------------------

    context.Database.EnsureCreated();


    // -------------------------------------------------
    // CITIES
    // -------------------------------------------------

    if (!context.Cities.Any())
    {
        var cities = new List<City>
        {
            new City
            {
                Id = Guid.NewGuid(),
                Name = "Ýstanbul"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Ankara"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Ýzmir"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Bursa"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Antalya"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Adana"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Konya"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Þanlýurfa"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Gaziantep"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Kocaeli"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Mersin"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Diyarbakýr"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Hatay"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Kayseri"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Samsun"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Balýkesir"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Kahramanmaraþ"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Van"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Aydýn"
            },

            new City
            {
                Id = Guid.NewGuid(),
                Name = "Tekirdað"
            }
        };

        context.Cities.AddRange(cities);
        context.SaveChanges();
    }


    // -------------------------------------------------
    // ROLES
    // -------------------------------------------------

    if (!context.Roles.Any())
    {
        var roles = new List<AppRole>
        {
            new AppRole
            {
                Name = "Admin"
            },

            new AppRole
            {
                Name = "Manager"
            },

            new AppRole
            {
                Name = "User"
            }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();
    }


    // -------------------------------------------------
    // CARGO
    // -------------------------------------------------

    if (!context.Cargos.Any())
    {
        // =================================================
        // BRANCHLERI BUL
        // =================================================

        var branches = context.Branches
            .OrderBy(x => x.Id)
            .Take(2)
            .ToList();

        if (branches.Count < 2)
        {
            Console.WriteLine(
                "Cargo seed iþlemi yapýlamadý. En az 2 adet Branch kaydý bulunmalýdýr.");
        }
        else
        {
            var originBranch = branches[0];
            var destinationBranch = branches[1];


            // =================================================
            // KULLANICILARI BUL
            // =================================================

            var users = context.Users
                .OrderBy(x => x.Id)
                .Take(2)
                .ToList();

            if (users.Count < 2)
            {
                Console.WriteLine(
                    "Cargo seed iþlemi yapýlamadý. En az 2 adet kullanýcý bulunmalýdýr.");
            }
            else
            {
                var sender = users[0];
                var receiver = users[1];


                // =================================================
                // CARGO OLUÞTUR
                // =================================================

                var cargo = new Cargo
                {
                    Id = Guid.NewGuid(),

                    SenderId = sender.Id,

                    ReceiverId = receiver.Id,

                    OriginBranchId = originBranch.Id,

                    DestinationBranchId = destinationBranch.Id,

                    TrackCode = "CT202609081234",

                    ShipmentDate = DateTime.Now,

                    EstimatedArrivalDate = DateTime.Now.AddDays(2),

                    Weight = 2.5,

                    CargoType =
                        CargoTrack.Entity.Entities.Enums.CargoType.Standard,

                    CargoStatus =
                        CargoTrack.Entity.Entities.Enums.CargoStatus
                            .DispatchedFromTransferCenter
                };


                context.Cargos.Add(cargo);

                context.SaveChanges();

                Console.WriteLine(
                    "Cargo seed iþlemi baþarýyla oluþturuldu.");
            }
        }
    }
}


// =====================================================
// RUN
// =====================================================

app.Run();

