using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Decorators;

public interface IRoleManagerDecorator<TRole> where TRole : class
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateAsync(TRole role);
    Task<List<TRole>> GetExistingRolesAsync();
}