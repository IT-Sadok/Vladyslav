using Application.Abstractions;
using Application.Abstractions.Decorators;
using Application.DTOs.Login;
using Application.DTOs.Register;
using AutoMapper;
using Domain.Entities;
using Healthcare.Application.DTOs.Result;
using Healthcare.Application.Schedules.Notifications;
using MediatR;
using Microsoft.AspNetCore.Identity;
using static Domain.Constants.UserRolesConstants;

namespace Application.Implementations;

public class AuthenticationService : IUserAuthenticationService
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly ISignInManagerDecorator<ApplicationUser> _signInManager;
    private readonly ITokenGeneratorService _generatorService;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;


    public AuthenticationService(IUserManagerDecorator<ApplicationUser> userManager,
        ISignInManagerDecorator<ApplicationUser> signInManager,
        ITokenGeneratorService generatorService, IMapper mapper, IMediator mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _generatorService = generatorService;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterUserDTO registerUserDto)
    {
        if (!IsRoleAllowed(registerUserDto.Role))
        {
            string description = $"Invalid role. Allowed roles are: {string.Join(' ', AllowedRoles)}";

            return IdentityResult.Failed(new IdentityError { Description = description });
        }

        var user = _mapper.Map<ApplicationUser>(registerUserDto);
        user.Id = Guid.NewGuid().ToString();

        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        
        if (!result.Succeeded) return result;

        await _userManager.AddToRoleAsync(user, registerUserDto.Role);

        await _mediator.Publish(new UserRegisteredNotification(registerUserDto.Role, user.Id));

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