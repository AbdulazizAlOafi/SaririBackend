using Microsoft.EntityFrameworkCore;
using SaririBackend.Classes;

namespace SaririBackend.Data // Make sure this matches your project's namespace
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet <BedManagement> BedManagement { get; set; }
        public DbSet <Patient> Patient { get; set; }
        public DbSet <MedicalHistory> MedicalHistory { get; set; }
        public DbSet <EmergencyRequest> EmergencyRequests { get; set; }
        public DbSet <TransferService> TransferServices { get; set; }
        public DbSet <AdminAction> AdminAction { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
