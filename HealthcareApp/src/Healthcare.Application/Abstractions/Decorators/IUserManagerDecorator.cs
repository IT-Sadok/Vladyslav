using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Decorators;

public interface IUserManagerDecorator<TUser> where TUser : class
{
    Task<IdentityResult> CreateAsync(TUser user, string password);
    Task<TUser?> FindByEmailAsync(string email);
    Task MigrateRangeAsync(List<ApplicationUser> patients, List<ApplicationUser> doctors);
    Task<IList<string>> GetRolesAsync(TUser user);
    Task<IdentityResult> AddToRoleAsync(TUser user, string role);

    Task<TUser?> FindByIdAsync(string id);
}