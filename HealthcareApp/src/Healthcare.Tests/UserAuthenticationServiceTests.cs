using Application.Abstractions;
using Application.Abstractions.Decorators;
using Application.DTOs.Login;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Application.DTOs.Register;
using Application.Implementations;
using AutoMapper;
using Domain.Entities;
using MediatR;


namespace Healthcare.Tests;

public class UserAuthenticationServiceTests
{
    private readonly Mock<IUserManagerDecorator<ApplicationUser>> _userManagerMock;
    private readonly Mock<ISignInManagerDecorator<ApplicationUser>> _signInManagerMock;
    private readonly Mock<IRoleManagerDecorator<IdentityRole>> _roleManagerMock;
    private readonly Mock<ITokenGeneratorService> _generatorService;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IMediator> _mediator;
    private readonly AuthenticationService _authService;


    public UserAuthenticationServiceTests()
    {
        _userManagerMock = new Mock<IUserManagerDecorator<ApplicationUser>>();
        _signInManagerMock = new Mock<ISignInManagerDecorator<ApplicationUser>>();
        _roleManagerMock = new Mock<IRoleManagerDecorator<IdentityRole>>();
        _generatorService = new Mock<ITokenGeneratorService>();
        _mapper = new Mock<IMapper>();
        _mediator = new Mock<IMediator>();
        

        _authService = new AuthenticationService(
            _userManagerMock.Object,
            _signInManagerMock.Object, 
            _generatorService.Object,
            _mapper.Object,
            _mediator.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnSucceededResult_WhenUserIsCreatedSuccessfully()
    {
        // Arrange

        var registerUserDto = new RegisterUserDTO("John", "Doe", "email@gmail.com", "123Pass", "Patient");

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
        var registerUserDto = new RegisterUserDTO("John", "Doe", "email@gmail.com", "123Pass", "Patient");


        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerUserDto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Registration failed!" }));

        // Act
        var result = await _authService.RegisterAsync(registerUserDto);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenLoginIsSuccessful()
    {
        // Arrange
        var loginDto = new LoginUserDTO("john.doe@example.com", "Password123");
        var userRoles = new List<string> { "Patient" }; 
        var user = new ApplicationUser { Email = loginDto.Email };
        var expectedToken = "fake-jwt-token";

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(userRoles);
        
        _generatorService.Setup(x => x.GenerateJwtToken(user, userRoles))
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
        var loginDto = new LoginUserDTO("john.doe@example.com", "Password123");

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().BeEmpty();
    }
}