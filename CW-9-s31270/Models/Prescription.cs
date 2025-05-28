using System.ComponentModel.DataAnnotations;

namespace CW_9_s31270.Models;

public class Prescription
{
    [Key]  // Upewnij się, że to jest
    public int IdPrescription { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    public int IdPatient { get; set; }
    public virtual Patient Patient { get; set; } = null!;
    
    public int IdDoctor { get; set; }
    public virtual Doctor Doctor { get; set; } = null!;
    
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}