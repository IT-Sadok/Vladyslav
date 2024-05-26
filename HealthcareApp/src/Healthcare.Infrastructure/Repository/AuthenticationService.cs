using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions;
using Application.DTOs.Login;
using Application.DTOs.Register;
using Healthcare.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repository
{
    public class AuthenticationService : IUserAuthenticationService

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        public async Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDto)
        {
            var user = new ApplicationUser
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            var userRole = registerUserDto.Role;
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(userRole))
                    await _roleManager.CreateAsync(new IdentityRole(userRole));

                await _userManager.AddToRoleAsync(user, registerUserDto.Role);
            }

            return result;
        }

        public async Task<string> LoginAsync(LoginUserDTO loginUserDto)
        {
            var result =
                await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, false);
            if (!result.Succeeded) return "";
            
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            var token = await GenerateJwtToken(user ?? new ApplicationUser());
            
            return token;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>
            {
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Email", user.Email ?? "")
            };

            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}