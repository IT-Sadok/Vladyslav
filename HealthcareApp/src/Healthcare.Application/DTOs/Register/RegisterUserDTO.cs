namespace Application.DTOs.Register;

public record RegisterUserDTO(
    string FirstName, 
    string LastName, 
    string Email, 
    string Password, 
    string Role);