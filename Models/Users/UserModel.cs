using System.ComponentModel.DataAnnotations;

namespace RindusBackend.Models;

public class UserModel
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Surname { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public DateTime? CreationDate { get; set; }
}