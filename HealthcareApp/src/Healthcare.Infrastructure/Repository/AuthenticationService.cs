using Application.Abstractions;
using Application.Abstractions.Decorators;
using Application.DTOs.Login;
using Application.DTOs.Register;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using static Domain.Constants.UserRolesConstants;

namespace Infrastructure.Repository
{
    public class AuthenticationService : IUserAuthenticationService

    {
        private readonly IUserManagerDecorator<ApplicationUser> _userManager;
        private readonly ISignInManagerDecorator<ApplicationUser> _signInManager;
        private readonly IRoleManagerDecorator<IdentityRole> _roleManager;
        private readonly ITokenGeneratorService _generatorService;


        public AuthenticationService(IUserManagerDecorator<ApplicationUser> userManager,
            ISignInManagerDecorator<ApplicationUser> signInManager,
            IRoleManagerDecorator<IdentityRole> roleManager, ITokenGeneratorService generatorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _generatorService = generatorService;
        }
        
        public async Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDto)
        {
            if(!IsRoleAllowed(registerUserDto.Role))
            {
                string description = $"Invalid role. Allowed roles are: ";
                foreach (var role in AllowedRoles)
                    description += $"{role} ";
                
                return IdentityResult.Failed(new IdentityError { Description = description });
            }
            var user = new ApplicationUser
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            var userRole = registerUserDto.Role;
            if (!result.Succeeded) return result;

            await _userManager.AddToRoleAsync(user, registerUserDto.Role);

            return result;
        }

        public async Task<string> LoginAsync(LoginUserDTO loginUserDto)
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            
            var result =
                await _signInManager.PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, false);
            if (!result.Succeeded) return string.Empty;
            
            var userRoles = await _userManager.GetRolesAsync(user ?? new ApplicationUser());
            
            var token = await _generatorService.GenerateJwtToken(user ?? new ApplicationUser(), userRoles.ToList());
            
            return token;
        }
    }
}
