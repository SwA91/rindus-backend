using RindusBackend.Dto.Users;
using RindusBackend.Models;

namespace RindusBackend.Data.Users;

public interface IUserRepository
{
    Task<bool> SaveChanges();

    Task<UserModel> GetUserById(int id);

    Task AddUser(UserModel userRequest);

    Task<IEnumerable<UserModel>> GetAllUsers();

    Task UpdateUser(UserUpdateRequest userRequest);

    Task DeleteUser(int id);
}