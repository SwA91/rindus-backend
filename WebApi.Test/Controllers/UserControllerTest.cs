using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using WebApi.Controllers;
using WebApi.Data.Users;
using WebApi.Dto.Users;
using WebApi.Models;
namespace WebApi.UnitTests;

public class UserControllerTest
{
    [Theory, AutoNSubstituteData]
    public async void GetUsers_Ok(
     IEnumerable<UserModel> users,
     [Frozen] IUserRepository _repository,
     [NoAutoProperties] UserController _controller,
     [Frozen] IMapper _mapper
 )
    {
        // Arrange
        _repository.GetAllUsers().Returns(users);
        _mapper.Map<IEnumerable<UserDataResponse>>(users);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        _mapper.Received().Map<IEnumerable<UserDataResponse>>(users);
        Assert.IsType<ActionResult<IEnumerable<UserDataResponse>>>(result);
        Assert.NotNull(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async void GetUsers_Nok(
        IEnumerable<UserModel> users,
        [Frozen] IUserRepository _repository,
        [NoAutoProperties] UserController _controller,
        [Frozen] IMapper _mapper
    )
    {
        // Arrange
        _repository.GetAllUsers().Returns(Task.FromException<IEnumerable<UserModel>>(new Exception("error")));

        // Act
        var result = () => _controller.GetUsers();

        // Assert
        Exception ex = await Assert.ThrowsAnyAsync<Exception>(result);
        Assert.Equal("error", ex.Message);
        _mapper.DidNotReceive().Map<IEnumerable<UserDataResponse>>(users);
    }

    [Theory, AutoNSubstituteData]
    public async void DeleteUser_Ok(
        int id,
        Task ok,
        [Frozen] IUserRepository _repository,
        [NoAutoProperties] UserController _controller
    )
    {
        // Arrange
        _repository.DeleteUser(id).Returns(ok);
        _repository.SaveChanges().Returns(true);

        // Act
        var result = await _controller.DeleteUser(id);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ActionResult>(result);
    }

    [Theory, AutoNSubstituteData]
    public async void DeleteUser_Nok(
        int id,
        [Frozen] IUserRepository _repository,
        [NoAutoProperties] UserController _controller
        )
    {
        // Arrange
        _repository.DeleteUser(id).Returns(Task.FromException(new Exception("error")));

        // Act
        var result = () => _controller.DeleteUser(id);

        // Assert
        Exception ex = await Assert.ThrowsAnyAsync<Exception>(result);
        Assert.Equal("error", ex.Message);
        await _repository.DidNotReceive().SaveChanges();
    }

    [Theory, AutoNSubstituteData]
    public async void AddUser_Ok(
        UserAddRequest request,
        UserModel userModel,
        Task task,
        [Frozen] IUserRepository _repository,
        [NoAutoProperties] UserController _controller,
        [Frozen] IMapper _mapper
    )
    {
        // Arrange
        _mapper.Map<UserModel>(request).Returns(userModel);
        _repository.AddUser(userModel).Returns(task);
        _repository.SaveChanges().Returns(true);
        _mapper.Map<UserAddResponse>(userModel);

        // Act
        var result = await _controller.AddUser(request);

        // Assert
        _mapper.Received().Map<UserAddResponse>(userModel);
        Assert.IsType<ActionResult<UserAddResponse>>(result);
        Assert.NotNull(result.Result);
    }

    [Theory, AutoNSubstituteData]
    public async void AddUser_Nok(
        UserAddRequest request,
        UserModel userModel,
        [Frozen] IUserRepository _repository,
        [NoAutoProperties] UserController _controller,
        [Frozen] IMapper _mapper
    )
    {
        // Arrange
        _repository.AddUser(userModel).Returns(Task.FromException(new Exception("error")));
        // _repository.SaveChanges().Returns(Task.FromException(new Exception("error")));

        // Act
        var result = () => _controller.AddUser(request);

        // Assert
        // Exception ex = await Assert.ThrowsAsync<Exception>(result);
        // Assert.Equal("error", ex.Message);
        await _repository.DidNotReceive().SaveChanges();
        _mapper.DidNotReceive().Map<UserAddResponse>(userModel);
    }
}

public class AutoNSubstituteDataAttribute : AutoDataAttribute
{
    public AutoNSubstituteDataAttribute()
          : base(() => new Fixture()
          .Customize(new AutoNSubstituteCustomization()))
    {
    }
}