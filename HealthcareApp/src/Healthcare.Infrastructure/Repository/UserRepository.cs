using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Healthcare.Application.Contracts;
using Healthcare.Application.DTOs;
using Healthcare.Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Healthcare.Infrastructure.Repository
{
    public class UserRepository : IUser
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration;
        }

        public async Task<LoginResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginUserDTO.Email);

            if (user == null) return new LoginResponse(false, "User Not found");

            if (user.Password == loginUserDTO.Password)
                return new LoginResponse(true, "Success", GenerateToken(user));
            else
                return new LoginResponse(false, "Invalid data");
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == registerUserDTO.Email);
            if (user != null)
                return new RegisterResponse(false, "User already exist");

            var userToAdd = new Domain.Entities.User()
            {
                FirstName = registerUserDTO.FirstName,
                LastName = registerUserDTO.LastName,
                Email = registerUserDTO.Email,
                Password = registerUserDTO.Password,
                Role = registerUserDTO.Role
            };

            _context.Users.Add(userToAdd);
            await _context.SaveChangesAsync();

            return new RegisterResponse(true, "User has registered successfully", GenerateToken(userToAdd));
        }

    }
}