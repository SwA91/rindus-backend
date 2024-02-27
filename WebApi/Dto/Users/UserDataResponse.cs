namespace WebApi.Dto.Users;

public class UserDataResponse : UserDto
{
    public int Id { get; set; }
    public string? LastUpdated { get; set; }
    public string? DateCreated { get; set; }
}