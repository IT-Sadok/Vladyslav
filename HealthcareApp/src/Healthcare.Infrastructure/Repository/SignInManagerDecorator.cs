using Application.Abstractions.Decorators;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class SignInManagerDecorator<TUser> : ISignInManagerDecorator<TUser> where TUser : class
{
    private readonly SignInManager<TUser> _signInManager;

    public SignInManagerDecorator(SignInManager<TUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure) 
        => _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
}