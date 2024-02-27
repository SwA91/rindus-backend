using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RindusBackend.Data.Users;
using RindusBackend.Dto.Users;
using RindusBackend.Models;

namespace RindusBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    private IMapper _mapper;

    public UserController(IUserRepository repository, IMapper mapper)
    {
        _userRepository = repository;
        _mapper = mapper;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);
        await _userRepository.SaveChanges();
        return Ok();

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDataResponse>>> GetUsers()
    {
        IEnumerable<UserModel> users = await _userRepository.GetAllUsers();
        return Ok(_mapper.Map<IEnumerable<UserDataResponse>>(users));
    }

    [AllowAnonymous]
    [HttpPost("add")]
    public async Task<ActionResult<UserAddResponse>> AddUser(
        [FromBody] UserAddRequest request
    )
    {
        UserModel userModel = _mapper.Map<UserModel>(request);
        await _userRepository.AddUser(userModel);
        await _userRepository.SaveChanges();

        UserAddResponse userResponse = _mapper.Map<UserAddResponse>(userModel);

        return CreatedAtRoute(
            nameof(GetUserById),
            new { userResponse.Id },
            userResponse
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDataResponse>> GetUserById(int id)
    {
        UserModel userModel = await _userRepository.GetUserById(id);

        return Ok(_mapper.Map<UserDataResponse>(userModel));
    }
}