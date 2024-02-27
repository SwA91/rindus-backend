using System.ComponentModel.DataAnnotations;

namespace RindusBackend.Models;

public class UserModel
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    public string? Name { get; set; }

    [Required]
    [MinLength(4)]
    public string? Surname { get; set; }

    [Key]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime LastUpdated { get; set; }
}