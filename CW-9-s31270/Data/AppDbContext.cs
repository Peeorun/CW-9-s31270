using CW_9_s31270.Models;
using Microsoft.EntityFrameworkCore;


namespace CW_9_s31270.Data
{
    public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Patient)
            .WithMany(pt => pt.Prescriptions)
            .HasForeignKey(p => p.IdPatient)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Doctor)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.IdDoctor)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament)
            .OnDelete(DeleteBehavior.Restrict);
        
        var doctors = new List<Doctor>
        {
            new() { IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski" },
            new() { IdDoctor = 2, FirstName = "Anna", LastName = "Nowak" }
        };
        
        var medicaments = new List<Medicament>
        {
            new() { IdMedicament = 1, Name = "Aspirin", Description = "Pain reliever", Type = "Tablet" },
            new() { IdMedicament = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Type = "Tablet" }
        };
        
        modelBuilder.Entity<Doctor>().HasData(doctors);
        modelBuilder.Entity<Medicament>().HasData(medicaments);
    }
}

}

