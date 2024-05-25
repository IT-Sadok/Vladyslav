using Application.DTOs;
using Microsoft.AspNetCore.Identity;


namespace Application.Abstractions;

public interface IUserAuthenticationService
{
    Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDto);

    Task<string> LoginAsync(LoginUserDTO loginUserDto);
}
