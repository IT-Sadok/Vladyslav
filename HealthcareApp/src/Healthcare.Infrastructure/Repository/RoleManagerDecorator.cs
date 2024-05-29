using Application.Abstractions.Decorators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class RoleManagerDecorator<TRole> : IRoleManagerDecorator<TRole> where TRole : class
{
    private readonly RoleManager<TRole> _roleManager;

    public RoleManagerDecorator(RoleManager<TRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task<bool> RoleExistsAsync(string roleName) => _roleManager.RoleExistsAsync(roleName);
    public Task<IdentityResult> CreateAsync(TRole role) => _roleManager.CreateAsync(role);
    public async Task<List<TRole>> GetExistingRolesAsync() => await _roleManager.Roles.ToListAsync();
}