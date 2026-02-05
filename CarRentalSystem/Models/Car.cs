using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalSystem.Models;

public class Car
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PricePerDay { get; set; }

    public bool IsAvailable { get; set; } = true;

    public string? ImageUrl { get; set; }
}
