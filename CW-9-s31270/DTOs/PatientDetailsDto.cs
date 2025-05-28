namespace CW_9_s31270.DTOs
{
    public class PatientDetailsDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Birthdate { get; set; }
        public string? Email { get; set; }
        public List<PrescriptionDetailsDto> Prescriptions { get; set; } = new List<PrescriptionDetailsDto>();
    }

    public class PrescriptionDetailsDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; } = null!;
        public List<MedicamentDetailsDto> Medicaments { get; set; } = new List<MedicamentDetailsDto>();
    }

    public class DoctorDto
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }

    public class MedicamentDetailsDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Type { get; set; }
        public int Dose { get; set; }
        public string? Details { get; set; }
    }
}

