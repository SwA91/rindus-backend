using System.ComponentModel.DataAnnotations;
using WebApi.Validators;

namespace WebApi.Dto.Users;

public class UserDto
{
    [ValueCheck]
    [StringLength(250, MinimumLength = 4)]
    public string? Name { get; set; }

    [ValueCheck]
    [StringLength(250, MinimumLength = 4)]
    public string? Surname { get; set; }

    [ValueCheck]
    [EmailAddress]
    public string? Email { get; set; }

}