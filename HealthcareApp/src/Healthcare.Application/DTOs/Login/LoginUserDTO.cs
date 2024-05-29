using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Login;

public record LoginUserDTO(string Email, string Password);
