using System.Runtime.CompilerServices;
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

    public async Task<bool> RoleExistsAsync(string roleName) => await _roleManager.RoleExistsAsync(roleName);
    public async Task<IdentityResult> CreateAsync(TRole role) => await _roleManager.CreateAsync(role);
    public async Task<List<TRole>> GetExistingRolesAsync() => await _roleManager.Roles.ToListAsync();
}