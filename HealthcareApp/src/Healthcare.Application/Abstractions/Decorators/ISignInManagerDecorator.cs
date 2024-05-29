using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Decorators;

public interface ISignInManagerDecorator<TUser> where TUser : class
{
    Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure);
}