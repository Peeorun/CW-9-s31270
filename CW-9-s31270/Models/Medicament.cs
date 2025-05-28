using System.ComponentModel.DataAnnotations;

namespace CW_9_s31270.Models;

public class Medicament
{
    [Key]  // Upewnij się, że to jest
    public int IdMedicament { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(100)]
    public string? Description { get; set; }
    
    [MaxLength(100)]
    public string? Type { get; set; }
    
    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}