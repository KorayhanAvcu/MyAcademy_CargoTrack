using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentCargos)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedCargos)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.OriginBranch)
                .WithMany(u => u.OriginCargos)
                .HasForeignKey(c => c.OriginBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cargo>()
                .HasOne(c => c.DestinationBranch)
                .WithMany(u => u.DestinationCargos)
                .HasForeignKey(c => c.DestinationBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // BİRE-BİR İLİŞKİLER
            // =====================================================

            // Cargo 1 - 0..1 Delivery (bir kargonun en fazla 1 teslimatı olur)
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Cargo)
                .WithOne(c => c.Delivery)
                .HasForeignKey<Delivery>(d => d.CargoId);

            // AppUser 1 - 0..1 Employee (her kullanıcı personel olmak zorunda değil)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.AppUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.AppUserId);

            // =====================================================

            // Para alanları: toplam 18 basamak, virgülden sonra 2
            modelBuilder.Entity<Cargo>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CargoPrice>()
                .Property(x => x.Price)
                .HasPrecision(18, 2);

            // Takip numarası benzersiz olmalı
            modelBuilder.Entity<Cargo>()
                .HasIndex(x => x.TrackCode)
                .IsUnique();

            // =====================================================
            // Silinmiş kayıtlar sorgularda otomatik olarak gelmez
            // =====================================================
            modelBuilder.Entity<Cargo>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Branch>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<TransferCenter>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CargoPrice>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Address>().HasQueryFilter(x => !x.IsDeleted);

            // =====================================================
            // Bizim tablolarımızda bir kayıt silinince bağlı kayıtlar silinmesin
            // (Identity tablolarına dokunmuyoruz)
            // =====================================================
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                         .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType))
                         .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // =====================================================
        // OTOMATİK TARİH + SOFT DELETE
        // =====================================================
        private void ApplyAuditInfo()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now;
                        // Güncellemede oluşturulma tarihi ezilmesin
                        entry.Property(x => x.CreatedDate).IsModified = false;
                        break;

                    case EntityState.Deleted:
                        // Gerçekten silme, sadece işaretle
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedDate = DateTime.Now;
                        break;
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        // =====================================================
        // DBSET'LER
        // =====================================================
        public DbSet<About> Abouts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }

        public DbSet<CargoMovement> CargoMovements { get; set; }
        public DbSet<TransferCenter> TransferCenters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryException> DeliveryExceptions { get; set; }
        public DbSet<CargoPrice> CargoPrices { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}