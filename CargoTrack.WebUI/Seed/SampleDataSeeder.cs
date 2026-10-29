using CargoTrack.Business.Services.Cargos;
using CargoTrack.DataAccess.Context;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.WebUI.Seed
{
    public static class SampleDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            var defaultAddresses = new[]
            {
                (User: "musteri1", City: "İstanbul", District: "Ataşehir", Street: "Atatürk Mah. Ertuğrul Gazi Sok. No:12 D:5"),
                (User: "musteri2", City: "Ankara",   District: "Çankaya",  Street: "Kızılay Mah. Atatürk Bulvarı No:45 D:8"),
                (User: "musteri3", City: "İzmir",    District: "Konak",    Street: "Alsancak Mah. Kıbrıs Şehitleri Cad. No:30 D:2"),
                (User: "musteri4", City: "Bursa",    District: "Nilüfer",  Street: "Beşevler Mah. Fatih Sultan Mehmet Bulvarı No:7"),
                (User: "musteri5", City: "Antalya",  District: "Muratpaşa", Street: "Lara Mah. Güzeloba Cad. No:88 D:3"),
                (User: "musteri6", City: "İstanbul", District: "Kadıköy",  Street: "Moda Mah. Bahariye Cad. No:21 D:4")
            };
            foreach (var d in defaultAddresses)
            {
                var owner = await userManager.FindByNameAsync(d.User);
                if (owner is not null && !await context.Addresses.AnyAsync(a => a.UserId == owner.Id))
                    await AddressAsync(owner, (await CityAsync(d.City)).Id, "Ev", d.District, d.Street);
            }

            if (await context.Cargos.AnyAsync(c => c.TrackCode.StartsWith("MYC-")))
                return;

            var cargoService = services.GetRequiredService<ICargoService>();
            var admin = await userManager.FindByNameAsync("admin")
                        ?? throw new InvalidOperationException("Önce admin kullanıcısı oluşturulmalı.");

            async Task<Address> AddressAsync(AppUser user, Guid cityId, string title, string district, string fullAddress)
            {
                var address = new Address { UserId = user.Id, CityId = cityId, Title = title, District = district, FullAddress = fullAddress };
                context.Addresses.Add(address);
                await context.SaveChangesAsync();
                return address;
            }

            async Task<City> CityAsync(string name)
            {
                var city = await context.Cities.FirstOrDefaultAsync(c => c.Name == name);
                if (city is null)
                {
                    city = new City { Name = name };
                    context.Cities.Add(city);
                    await context.SaveChangesAsync();
                }
                return city;
            }

            async Task<Branch> BranchAsync(string name, string cityName)
            {
                var branch = await context.Branches.FirstOrDefaultAsync(b => b.Name == name);
                if (branch is null)
                {
                    branch = new Branch { Name = name, CityId = (await CityAsync(cityName)).Id };
                    context.Branches.Add(branch);
                    await context.SaveChangesAsync();
                }
                return branch;
            }

            var atasehir = await BranchAsync("Ataşehir Şubesi", "İstanbul");
            var kadikoy = await BranchAsync("Kadıköy Şubesi", "İstanbul");
            var cankaya = await context.Branches.FirstOrDefaultAsync(b => b.Name.Contains("Çankaya"))
                          ?? await BranchAsync("Çankaya Şubesi", "Ankara");
            var konak = await BranchAsync("Konak Şubesi", "İzmir");
            var nilufer = await BranchAsync("Nilüfer Şubesi", "Bursa");
            var muratpasa = await BranchAsync("Muratpaşa Şubesi", "Antalya");

            async Task<TransferCenter> CenterAsync(string name, string cityName)
            {
                var center = await context.TransferCenters.FirstOrDefaultAsync(t => t.Name == name);
                if (center is null)
                {
                    center = new TransferCenter { Name = name, CityId = (await CityAsync(cityName)).Id };
                    context.TransferCenters.Add(center);
                    await context.SaveChangesAsync();
                }
                return center;
            }

            var tmIstanbul = await CenterAsync("İstanbul Anadolu Transfer Merkezi", "İstanbul");
            var tmAnkara = await CenterAsync("Ankara Transfer Merkezi", "Ankara");
            var tmIzmir = await CenterAsync("İzmir Transfer Merkezi", "İzmir");

            async Task<AppUser> UserAsync(string userName, string password, string first, string last, string role)
            {
                var user = await userManager.FindByNameAsync(userName);
                if (user is null)
                {
                    user = new AppUser { UserName = userName, Email = $"{userName}@cargotrack.com", FirstName = first, LastName = last };
                    var result = await userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                        throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                if (!await userManager.IsInRoleAsync(user, role))
                    await userManager.AddToRoleAsync(user, role);
                return user;
            }

            async Task<Employee> EmployeeAsync(Branch branch, string userName, string first, string last)
            {
                var existing = await context.Employees.FirstOrDefaultAsync(e => e.BranchId == branch.Id);
                if (existing is not null)
                    return existing;

                var user = await UserAsync(userName, "Mudur123*", first, last, "Manager");
                var employee = await context.Employees.FirstOrDefaultAsync(e => e.AppUserId == user.Id);
                if (employee is null)
                {
                    employee = new Employee { AppUserId = user.Id, BranchId = branch.Id, Position = "Şube Müdürü" };
                    context.Employees.Add(employee);
                    await context.SaveChangesAsync();
                }
                return employee;
            }

            var empAtasehir = await EmployeeAsync(atasehir, "mudur.atasehir", "Burak", "Aydın");
            var empKadikoy = await EmployeeAsync(kadikoy, "mudur.kadikoy", "Zeynep", "Arslan");
            var empCankaya = await EmployeeAsync(cankaya, "mudur.cankaya", "Elif", "Şahin");
            var empKonak = await EmployeeAsync(konak, "mudur.konak", "Emre", "Koç");
            var empNilufer = await EmployeeAsync(nilufer, "mudur.nilufer", "Selin", "Öztürk");
            var empMuratpasa = await EmployeeAsync(muratpasa, "mudur.muratpasa", "Kerem", "Çelik");

            var customers = new List<AppUser>
            {
                await UserAsync("musteri1", "Musteri123*", "Ayşe", "Kaya", "User"),
                await UserAsync("musteri2", "Musteri123*", "Mehmet", "Demir", "User"),
                await UserAsync("musteri3", "Musteri123*", "Fatma", "Yıldız", "User"),
                await UserAsync("musteri4", "Musteri123*", "Ahmet", "Kurt", "User"),
                await UserAsync("musteri5", "Musteri123*", "Zehra", "Polat", "User"),
                await UserAsync("musteri6", "Musteri123*", "Oğuz", "Aksoy", "User")
            };

            async Task<Address> AddressInCityAsync(AppUser user, Guid cityId)
            {
                var existing = await context.Addresses.FirstOrDefaultAsync(a => a.UserId == user.Id && a.CityId == cityId);
                if (existing is not null)
                    return existing;
                var city = await context.Cities.FirstAsync(c => c.Id == cityId);
                return await AddressAsync(user, cityId, "İş", "Merkez", $"{city.Name} Merkez Mah. Cumhuriyet Cad. No:{Random.Shared.Next(1, 120)}");
            }

            var employeeOf = new Dictionary<Guid, Guid>
            {
                [atasehir.Id] = empAtasehir.Id, [kadikoy.Id] = empKadikoy.Id, [cankaya.Id] = empCankaya.Id,
                [konak.Id] = empKonak.Id, [nilufer.Id] = empNilufer.Id, [muratpasa.Id] = empMuratpasa.Id
            };

            var scenarios = new (Branch From, Branch To, TransferCenter? Via, int Sender, int Receiver, double Kg, double W, double L, double H,
                                 CargoType Type, DeliveryType Delivery, int DaysAgo, string[] Steps)[]
            {
                (atasehir, cankaya, tmIstanbul, 0, 1, 2.4, 30, 20, 15, CargoType.Standard, DeliveryType.Standard, 6, new[] { "origin", "tc", "dest", "out", "deliver" }),
                (kadikoy, konak, tmIzmir, 1, 2, 5.0, 40, 30, 30, CargoType.Fragile, DeliveryType.Express, 5, new[] { "origin", "tc", "dest", "out", "deliver" }),
                (konak, atasehir, tmIzmir, 2, 0, 1.2, 25, 15, 10, CargoType.Standard, DeliveryType.Standard, 4, new[] { "origin", "tc", "dest", "out", "deliver" }),
                (nilufer, muratpasa, tmIstanbul, 3, 4, 12.0, 60, 40, 40, CargoType.Heavy, DeliveryType.Standard, 3, new[] { "origin", "tc", "dest", "out", "deliver" }),
                (atasehir, kadikoy, null, 4, 5, 0.8, 20, 15, 5, CargoType.Urgent, DeliveryType.Express, 1, new[] { "origin", "dest", "out", "deliver" }),
                (cankaya, nilufer, tmAnkara, 1, 3, 3.5, 35, 25, 20, CargoType.Standard, DeliveryType.Standard, 2, new[] { "origin", "tc", "dest", "out" }),
                (muratpasa, atasehir, tmIstanbul, 5, 0, 7.0, 50, 40, 30, CargoType.Fragile, DeliveryType.Standard, 1, new[] { "origin", "tc", "dest", "out" }),
                (kadikoy, cankaya, tmAnkara, 0, 2, 2.0, 30, 20, 10, CargoType.Standard, DeliveryType.Express, 1, new[] { "origin", "tc", "dest" }),
                (konak, nilufer, tmIzmir, 2, 3, 4.2, 40, 30, 20, CargoType.Standard, DeliveryType.Standard, 0, new[] { "origin", "tc" }),
                (atasehir, muratpasa, tmIstanbul, 0, 4, 9.5, 55, 45, 35, CargoType.Heavy, DeliveryType.Standard, 0, new[] { "origin" }),
                (nilufer, kadikoy, null, 3, 1, 1.5, 25, 20, 10, CargoType.Standard, DeliveryType.Express, 0, Array.Empty<string>()),
                (cankaya, konak, tmAnkara, 2, 5, 6.0, 45, 35, 25, CargoType.Standard, DeliveryType.Standard, 9, new[] { "origin", "tc" }),
                (muratpasa, cankaya, tmAnkara, 4, 1, 3.0, 30, 30, 20, CargoType.Fragile, DeliveryType.Standard, 8, new[] { "origin", "tc", "dest" }),
                (atasehir, konak, tmIzmir, 0, 3, 2.2, 30, 20, 15, CargoType.Standard, DeliveryType.Standard, 4, new[] { "origin", "tc", "dest", "out", "fail:ReceiverNotFound" }),
                (kadikoy, nilufer, tmIstanbul, 5, 2, 1.0, 20, 20, 10, CargoType.Urgent, DeliveryType.Express, 3, new[] { "origin", "tc", "dest", "out", "fail:WrongAddress", "out" }),
                (konak, kadikoy, tmIzmir, 3, 5, 4.0, 40, 30, 25, CargoType.Standard, DeliveryType.Standard, 7, new[] { "origin", "tc", "dest", "out", "fail:ReceiverNotFound", "out", "fail:ReceiverNotFound", "out", "fail:Rejected" }),
                (nilufer, cankaya, tmAnkara, 4, 0, 2.5, 30, 25, 15, CargoType.Standard, DeliveryType.Standard, 10, new[] { "origin", "tc", "dest", "out", "fail:Damaged", "out", "fail:Rejected", "out", "fail:Rejected", "returned" }),
            };

            foreach (var s in scenarios)
            {
                var receiverAddress = await AddressInCityAsync(customers[s.Receiver], s.To.CityId);
                var trackCode = await cargoService.CreateAsync(new CreateCargoDto
                {
                    SenderId = customers[s.Sender].Id,
                    ReceiverId = customers[s.Receiver].Id,
                    ReceiverAddressId = receiverAddress.Id,
                    OriginBranchId = s.From.Id,
                    DestinationBranchId = s.To.Id,
                    Weight = s.Kg, Width = s.W, Length = s.L, Height = s.H,
                    CargoType = s.Type,
                    DeliveryType = s.Delivery
                }, admin.Id);

                var cargo = await context.Cargos.FirstAsync(c => c.TrackCode == trackCode);
                var destEmployee = employeeOf[s.To.Id];

                foreach (var step in s.Steps)
                {
                    switch (step)
                    {
                        case "origin":
                            await Move(CargoStatus.AtOriginBranch, s.From.Id, null, "Gönderi şubeye teslim alındı", employeeOf[s.From.Id]);
                            break;
                        case "tc":
                            await Move(CargoStatus.InTransferCenter, null, s.Via!.Id, "Transfer merkezine ulaştı", null);
                            break;
                        case "dest":
                            await Move(CargoStatus.AtDestinationBranch, s.To.Id, null, "Varış şubesine ulaştı", destEmployee);
                            break;
                        case "out":
                            await Move(CargoStatus.OutForDelivery, s.To.Id, null, "Kurye dağıtıma çıktı", destEmployee);
                            break;
                        case "returned":
                            await Move(CargoStatus.ReturnedToSender, s.From.Id, null, "Gönderi göndericiye iade edildi", null);
                            break;
                        case "deliver":
                            var code = (await context.Cargos.FirstAsync(c => c.Id == cargo.Id)).DeliveryCode!;
                            var receiver = customers[s.Receiver];
                            await cargoService.DeliverAsync(new DeliverCargoDto
                            {
                                CargoId = cargo.Id,
                                DeliveryCode = code,
                                ReceivedByName = $"{receiver.FirstName} {receiver.LastName}"
                            }, admin.Id, destEmployee);
                            break;
                        default:
                            var reason = Enum.Parse<DeliveryExceptionReason>(step.Split(':')[1]);
                            await cargoService.ReportFailureAsync(new ReportDeliveryFailureDto
                            {
                                CargoId = cargo.Id,
                                Reason = reason,
                                Description = "Örnek veri"
                            }, admin.Id, destEmployee);
                            break;
                    }
                }

                await ShiftDatesAsync(cargo.Id, s.DaysAgo);

                Task Move(CargoStatus status, Guid? branchId, Guid? centerId, string description, Guid? employeeId) =>
                    cargoService.ChangeStatusAsync(new ChangeCargoStatusDto
                    {
                        CargoId = cargo.Id,
                        NewStatus = status,
                        BranchId = branchId,
                        TransferCenterId = centerId,
                        Description = description
                    }, admin.Id, employeeId);
            }

            async Task ShiftDatesAsync(Guid cargoId, int daysAgo)
            {
                var cargo = await context.Cargos.FirstAsync(c => c.Id == cargoId);
                var movements = await context.CargoMovements.Where(m => m.CargoId == cargoId)
                                                            .OrderBy(m => m.MovementDate).ToListAsync();

                var start = DateTime.Today.AddDays(-daysAgo).AddHours(9).AddMinutes(Random.Shared.Next(0, 60));
                var slaDays = (cargo.EstimatedArrivalDate.Date - cargo.ShipmentDate.Date).Days;
                var totalHours = Math.Max(1, (DateTime.Now - start).TotalHours - 1);
                var step = Math.Min(10, totalHours / Math.Max(1, movements.Count));

                for (var i = 0; i < movements.Count; i++)
                    movements[i].MovementDate = start.AddHours(i * step);

                cargo.ShipmentDate = start;
                cargo.EstimatedArrivalDate = start.Date.AddDays(slaDays);
                if (cargo.DeliveredDate.HasValue)
                {
                    cargo.DeliveredDate = movements.Last().MovementDate;
                    var delivery = await context.Deliveries.FirstOrDefaultAsync(d => d.CargoId == cargoId);
                    if (delivery is not null) delivery.DeliveredDate = cargo.DeliveredDate.Value;
                }

                var failures = await context.DeliveryExceptions.Where(d => d.CargoId == cargoId)
                                                               .OrderBy(d => d.AttemptNumber).ToListAsync();
                var failMoves = movements.Where(m => m.NewStatus == CargoStatus.DeliveryFailed).ToList();
                for (var i = 0; i < failures.Count && i < failMoves.Count; i++)
                    failures[i].AttemptDate = failMoves[i].MovementDate;

                await context.SaveChangesAsync();
            }
        }
    }
}
