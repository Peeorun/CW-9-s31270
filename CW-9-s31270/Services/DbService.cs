using CW_9_s31270.Data;
using CW_9_s31270.DTOs;
using CW_9_s31270.Exceptions;
using CW_9_s31270.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_9_s31270.Services
{
    public interface IDbService
{
    public Task<int> CreatePrescriptionAsync(CreatePrescriptionDto prescriptionData);
    public Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId);
}

public class DbService(AppDbContext data) : IDbService
{
    public async Task<int> CreatePrescriptionAsync(CreatePrescriptionDto prescriptionData)
    {
        // Walidacja dat
        if (prescriptionData.DueDate < prescriptionData.Date)
        {
            throw new ArgumentException("DueDate must be greater than or equal to Date");
        }
        
        // Walidacja liczby leków
        if (prescriptionData.Medicaments.Count > 10)
        {
            throw new ArgumentException("Prescription cannot contain more than 10 medicaments");
        }
        
        // Sprawdzenie czy wszystkie leki istnieją
        var medicamentIds = prescriptionData.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await data.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .ToListAsync();
        
        if (existingMedicaments.Count != medicamentIds.Count)
        {
            throw new NotFoundException("One or more medicaments do not exist");
        }
        
        // Sprawdzenie czy lekarz istnieje
        var doctorExists = await data.Doctors
            .AnyAsync(d => d.IdDoctor == prescriptionData.IdDoctor);
        
        if (!doctorExists)
        {
            throw new NotFoundException("Doctor does not exist");
        }
        
        var transaction = await data.Database.BeginTransactionAsync();
        
        try
        {
            // Obsługa pacjenta
            Patient patient;
            if (prescriptionData.Patient.IdPatient.HasValue)
            {
                patient = await data.Patients
                    .FirstOrDefaultAsync(p => p.IdPatient == prescriptionData.Patient.IdPatient.Value);
                
                if (patient == null)
                {
                    // Tworzenie nowego pacjenta jeśli nie istnieje
                    patient = new Patient
                    {
                        FirstName = prescriptionData.Patient.FirstName,
                        LastName = prescriptionData.Patient.LastName,
                        Birthdate = prescriptionData.Patient.Birthdate,
                        Email = prescriptionData.Patient.Email
                    };
                    await data.Patients.AddAsync(patient);
                    await data.SaveChangesAsync();
                }
            }
            else
            {
                // Tworzenie nowego pacjenta
                patient = new Patient
                {
                    FirstName = prescriptionData.Patient.FirstName,
                    LastName = prescriptionData.Patient.LastName,
                    Birthdate = prescriptionData.Patient.Birthdate,
                    Email = prescriptionData.Patient.Email
                };
                await data.Patients.AddAsync(patient);
                await data.SaveChangesAsync();
            }
            
            // Tworzenie recepty
            var prescription = new Prescription
            {
                Date = prescriptionData.Date,
                DueDate = prescriptionData.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = prescriptionData.IdDoctor
            };
            
            await data.Prescriptions.AddAsync(prescription);
            await data.SaveChangesAsync();
            
            // Dodawanie leków do recepty
            foreach (var medicamentRequest in prescriptionData.Medicaments)
            {
                var prescriptionMedicament = new PrescriptionMedicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = medicamentRequest.IdMedicament,
                    Dose = medicamentRequest.Dose,
                    Details = medicamentRequest.Description
                };
                
                await data.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
            }
            
            await data.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return prescription.IdPrescription;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<PatientDetailsDto> GetPatientDetailsAsync(int patientId)
    {
        var patient = await data.Patients
            .Include(p => p.Prescriptions.OrderBy(pr => pr.DueDate))
                .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
                .ThenInclude(pr => pr.PrescriptionMedicaments)
                    .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);
        
        if (patient == null)
        {
            throw new NotFoundException($"Patient with id: {patientId} not found");
        }
        
        return new PatientDetailsDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Email = patient.Email,
            Prescriptions = patient.Prescriptions.Select(p => new PrescriptionDetailsDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName,
                    LastName = p.Doctor.LastName
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new MedicamentDetailsDto
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList()
            }).ToList()
        };
    }
}

}

