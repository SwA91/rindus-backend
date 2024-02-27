using System.Net;
using Microsoft.EntityFrameworkCore;
using NetKubernetes.Middleware;
using WebApi.Dto.Users;
using WebApi.Models;

namespace WebApi.Data.Users;

public class UserRepository : IUserRepository
{
    private enum UserValidate
    {
        DATA_CORRECT,
        EMAIL_EXIST,
        EMAIL_NOT_EXIST,
        ID
    };
    private readonly AppDbContext _dbContext;

    public UserRepository(
        AppDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsers()
    {
        return await _dbContext.User!.ToListAsync();
    }

    public async Task<UserModel> GetUserById(int id)
    {
        await ValidateRequest(UserValidate.ID, new UserModel { Id = id });

        return await _dbContext.User!.FirstAsync(user => user.Id == id);
    }

    public async Task AddUser(UserModel userRequest)
    {

        await ValidateRequest(UserValidate.DATA_CORRECT, userRequest);
        await ValidateRequest(UserValidate.EMAIL_EXIST, userRequest);

        userRequest.DateCreated = DateTime.Now;

        await _dbContext.User!.AddAsync(userRequest);
    }

    public async Task<UserModel> UpdateUser(UserModel userRequest)
    {
        await ValidateRequest(UserValidate.DATA_CORRECT, userRequest);
        await ValidateRequest(UserValidate.ID, userRequest);

        UserModel userDb = await _dbContext.User!.FirstAsync(user => user.Id == userRequest.Id);
        userDb.Name = userRequest.Name;
        userDb.Surname = userRequest.Surname;
        userDb.LastUpdated = DateTime.Now;

        return userDb;
    }

    public async Task DeleteUser(int id)
    {
        await ValidateRequest(UserValidate.ID, new UserModel { Id = id });
        UserModel user = await _dbContext.User!.FirstAsync(user => user.Id == id);
        _dbContext.User!.Remove(user!);
    }

    public async Task<bool> SaveChanges()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }

    private async Task<bool> ValidateRequest(UserValidate typeValidate, UserModel userRequest)
    {
        bool isValid = true;
        string message = string.Empty;

        switch (typeValidate)
        {
            case UserValidate.DATA_CORRECT:
                if (userRequest is null)
                {
                    isValid = false;
                    message = "User data is invalid";
                }
                break;

            case UserValidate.ID:
                if (!await _dbContext.User!.Where(user => user.Id == userRequest.Id).AnyAsync())
                {
                    isValid = false;
                    message = "User id does not exist in Data Base";
                }
                break;
            case UserValidate.EMAIL_NOT_EXIST:
                if (await _dbContext.User!.Where(user => user.Email != userRequest.Email).AnyAsync())
                {
                    isValid = false;
                    message = "User email does not existed in Data Base";
                }
                break;
            case UserValidate.EMAIL_EXIST:
                if (await _dbContext.User!.Where(user => user.Email == userRequest.Email).AnyAsync())
                {
                    isValid = false;
                    message = "User email already existed in Data Base";
                }
                break;
        }

        if (!isValid)
        {
            throw new MiddlewareException(HttpStatusCode.BadRequest, message);
        }

        return isValid;
    }


}