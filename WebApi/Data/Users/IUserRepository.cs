using WebApi.Models;

namespace WebApi.Data.Users;

public interface IUserRepository
{
    Task<bool> SaveChanges();

    Task<UserModel> GetUserById(int id);

    Task AddUser(UserModel userRequest);

    Task<IEnumerable<UserModel>> GetAllUsers();

    Task<UserModel> UpdateUser(UserModel userRequest);

    Task DeleteUser(int id);
}