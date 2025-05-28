using System.ComponentModel.DataAnnotations;

namespace CW_9_s31270.DTOs
{
    public class CreatePrescriptionDto
    {
        [Required]
        public PatientDto Patient { get; set; } = null!;
    
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public List<MedicamentRequestDto> Medicaments { get; set; } = null!;
    
        [Required]
        public DateTime Date { get; set; }
    
        [Required]
        public DateTime DueDate { get; set; }
    
        [Required]
        public int IdDoctor { get; set; }
    }

    public class PatientDto
    {
        public int? IdPatient { get; set; }
    
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
    
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
    
        public DateTime Birthdate { get; set; }
    
        [MaxLength(100)]
        public string? Email { get; set; }
    }

    public class MedicamentRequestDto
    {
        [Required]
        public int IdMedicament { get; set; }
    
        [Required]
        [Range(1, int.MaxValue)]
        public int Dose { get; set; }
    
        [MaxLength(100)]
        public string? Description { get; set; }
    }

}

