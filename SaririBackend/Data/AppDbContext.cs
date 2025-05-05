using Microsoft.EntityFrameworkCore;
using SaririBackend.Classes;

namespace SaririBackend.Data // Make sure this matches your project's namespace
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet <bedManagement> bedManagement { get; set; }
        public DbSet <Patient> Patient { get; set; }
        public DbSet <MedicalHistory> MedicalHistory { get; set; }
        public DbSet <EmergencyRequest> EmergencyRequest { get; set; }
        public DbSet <TransferService> TransferServices { get; set; }
        public DbSet <AdminAction> AdminAction { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce unique constraint on Email in the User table
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            // Unique constraint for Patient National ID
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.paitentNationalID)
                .IsUnique();
        }


    }

   

}
