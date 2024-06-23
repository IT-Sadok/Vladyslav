using Application.Abstractions.Decorators;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repository;

public class UserManagerDecorator<TUser> : IUserManagerDecorator<TUser> where TUser : class
{
    private readonly UserManager<TUser> _userManager;
    


    public UserManagerDecorator(UserManager<TUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<IdentityResult> CreateAsync(TUser user, string password) => _userManager.CreateAsync(user, password);
    public Task<TUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);
    public Task<IList<string>> GetRolesAsync(TUser user) => _userManager.GetRolesAsync(user);
    public Task<IdentityResult> AddToRoleAsync(TUser user, string role) => _userManager.AddToRoleAsync(user, role);
    public async Task<TUser?> FindByIdAsync(string id) => await _userManager.FindByIdAsync(id);
}