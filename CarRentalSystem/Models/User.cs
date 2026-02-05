using System.ComponentModel.DataAnnotations;

namespace CarRentalSystem.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty; // In a real app, hash this!
    
    [Required]
    public string Role { get; set; } = "User"; // "Admin" or "User"
}
