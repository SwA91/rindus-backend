namespace RindusBackend.Dto.Users;

public class UserUpdateResponse : UserDto
{
    public int Id { get; set; }

    public string? LastUpdated { get; set; }
}