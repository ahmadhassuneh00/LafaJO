using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinalProjAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public DataContextEF(DbContextOptions<DataContextEF> options, IConfiguration config)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyType> CompanyTypes { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<RentCar> RentCars { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<BookTrip> BookTrips { get; set; }
        public virtual DbSet<BuyItem> BuyItems { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define table mappings and relationships
            modelBuilder.Entity<Users>()
                .ToTable("Users", "FinalProjUser")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Company>()
                .ToTable("Registration", "FinalProjCompanies")
                .HasKey(c => c.RegistrationID);

            modelBuilder.Entity<CompanyType>()
                .ToTable("CompanyTypes", "FinalProjCompanies")
                .HasKey(ct => ct.TypeID); // Set TypeID as the key

            modelBuilder.Entity<CompanyType>()
                .Property(ct => ct.TypeID)
                .ValueGeneratedNever();

            modelBuilder.Entity<Trip>()
                .ToTable("Trips", "FinalProjPost")
                .HasKey(t => t.TripId);

            modelBuilder.Entity<Car>()
                .ToTable("Cars", "FinalProjPost")
                .HasKey(c => c.CarID);

            modelBuilder.Entity<RentCar>()
                .ToTable("Rentals", "FinalProjPost")
                .HasKey(r => r.RentalID);

            // Configure Reviews mapping
            modelBuilder.Entity<Review>()
                .ToTable("Reviews", "FinalProjPost")
                .HasKey(r => r.ReviewId);

            modelBuilder.Entity<Item>()
                .ToTable("Items", "FinalProjPost")
                .HasKey(ct => ct.ItemId);

            modelBuilder.Entity<BookTrip>()
            .ToTable("bookTrip", "FinalProjPost")
            .HasKey(bt => bt.BookId);

            modelBuilder.Entity<BuyItem>()
            .ToTable("buyItem", "FinalProjPost")
            .HasKey(bi => bi.BuyId);

            modelBuilder.Entity<Payments>()
            .ToTable("Payment", "FinalProjPost")
            .HasKey(p => p.paymentID);
        }
    }
}
