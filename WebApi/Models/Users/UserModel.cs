using System.ComponentModel.DataAnnotations;

namespace WebApi.Models;

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

    public DateTime DateCreated { get; set; }

    public DateTime LastUpdated { get; set; }

}