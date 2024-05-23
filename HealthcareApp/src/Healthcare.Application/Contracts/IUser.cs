using Healthcare.Application.DTOs;

namespace Healthcare.Application.Contracts;

public interface IUser
{
    Task<RegisterResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO);

    Task<LoginResponse> LoginUserAsync(LoginUserDTO loginUserDTO);

}
