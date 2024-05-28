using Application.Abstractions;
using Application.DTOs.Login;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.Register;
using Healthcare.Infrastructure.Persistance;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Http;


namespace Healthcare.Tests;

public class UserAuthenticationServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<ITokenGeneratorService> _generatorService;
    private readonly AuthenticationService _authService;

    public UserAuthenticationServiceTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);

        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

        _generatorService = new Mock<ITokenGeneratorService>();
        
        _authService = new AuthenticationService(_userManagerMock.Object, _signInManagerMock.Object,
             _roleManagerMock.Object, _generatorService.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSucceededResult_WhenUserIsCreatedSuccessfully()
    {
        // Arrange

        var registerUserDto = new RegisterUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "email@gmail.com",
            Password = "123Pass",
            Role = "Patient"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerUserDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock.Setup(x => x.RoleExistsAsync(registerUserDto.Role))
            .ReturnsAsync(false);

        _roleManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityRole>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), registerUserDto.Role))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RegisterAsync(registerUserDto);

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnFailedResult_WhenUserCreationFails()
    {
        // Arrange
        var registerDto = new RegisterUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "Password123",
            Role = "User"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerDto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Registration failed!" }));
        
        // Act
        var result = await _authService.RegisterAsync(registerDto);

        // Assert
        result.Succeeded.Should().BeFalse();
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginDto = new LoginUserDTO { Email = "john.doe@example.com", Password = "Password123" };
        var user = new ApplicationUser { Email = loginDto.Email };
        var expectedToken = "fake-jwt-token";

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _generatorService.Setup(x => x.GenerateJwtToken(It.IsAny<LoginUserDTO>()))
            .ReturnsAsync(expectedToken);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().Be(expectedToken);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnEmptyString_WhenLoginFails()
    {
        // Arrange
        var loginDto = new LoginUserDTO { Email = "john.doe@example.com", Password = "wrong-password" };

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().BeEmpty();
    }
}