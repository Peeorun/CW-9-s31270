using System.ComponentModel.DataAnnotations;



namespace CW_9_s31270.Models;

public class Doctor
{
    [Key]  // Dodaj ten atrybut
    public int IdDoctor { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}

