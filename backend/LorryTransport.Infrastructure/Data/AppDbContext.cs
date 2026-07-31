using LorryTransport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LorryTransport.Infrastructure.Data
{
    // AppDbContext is the "bridge" between our C# classes and SQL Server tables.
    // Each DbSet<T> below becomes one table in the database.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<LoadEntry> LoadEntries => Set<LoadEntry>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<DriverPayment> DriverPayments => Set<DriverPayment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision so SQL Server doesn't truncate money values.
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // Prevent accidental cascade-delete chains across LoadEntry relations.
            modelBuilder.Entity<LoadEntry>()
                .HasOne(l => l.Customer)
                .WithMany(c => c.LoadEntries)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoadEntry>()
                .HasOne(l => l.Vehicle)
                .WithMany(v => v.LoadEntries)
                .HasForeignKey(l => l.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LoadEntry>()
                .HasOne(l => l.Driver)
                .WithMany(d => d.LoadEntries)
                .HasForeignKey(l => l.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
