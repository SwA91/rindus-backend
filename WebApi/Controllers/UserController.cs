using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Users;
using WebApi.Dto.Users;
using WebApi.Models;

namespace WebApi.Controllers;

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

    [AllowAnonymous]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUser(id);
        await _userRepository.SaveChanges();
        return Ok();

    }

    [AllowAnonymous]
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

        return Ok(_mapper.Map<UserAddResponse>(userModel));
    }

    [AllowAnonymous]
    [HttpPut("update")]
    public async Task<ActionResult<UserUpdateResponse>> UpdateUser(
        [FromBody] UserUpdateRequest request
    )
    {
        UserModel userModel = _mapper.Map<UserModel>(request);
        UserModel userDb = await _userRepository.UpdateUser(userModel);
        await _userRepository.SaveChanges();

        return Ok(_mapper.Map<UserUpdateResponse>(userDb));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ActionName(nameof(GetUserById))]
    public async Task<ActionResult<UserDataResponse>> GetUserById(int id)
    {
        UserModel userModel = await _userRepository.GetUserById(id);

        return Ok(_mapper.Map<UserDataResponse>(userModel));
    }
}