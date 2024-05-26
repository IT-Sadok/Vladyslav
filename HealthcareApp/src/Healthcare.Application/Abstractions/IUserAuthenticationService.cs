using Application.DTOs;
using Application.DTOs.Login;
using Application.DTOs.Register;
using Microsoft.AspNetCore.Identity;


namespace Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDto);

    Task<string> LoginAsync(LoginUserDTO loginUserDto);
}
