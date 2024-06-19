using Application.Abstractions.Decorators;
using Domain.Constants;
using Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserManagerDecorator<TUser> : IUserManagerDecorator<TUser> where TUser : class
{
    private readonly UserManager<TUser> _userManager;
    private readonly AppDbContext _context;


    public UserManagerDecorator(UserManager<TUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public Task<IdentityResult> CreateAsync(TUser user, string password) => _userManager.CreateAsync(user, password);

    public async Task MigrateRangeAsync(List<ApplicationUser> patients, List<ApplicationUser> doctors)
    {
        var allUsers = patients.Concat(doctors);
        string sqlQuery = $@"
        INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, FirstName, LastName, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
        VALUES ";

        List<string> valuePlaceholders = new List<string>();
        List<object> parameters = new List<object>();
        int index = 0;

        foreach (var user in allUsers)
        {
            valuePlaceholders.Add(
                $"(@p{index}, @p{index + 1}, @p{index + 2}, @p{index + 3}, @p{index + 4}, @p{index + 5}, @p{index + 6}, @p{index + 7}, @p{index + 8}, @p{index + 9}, @p{index + 10}, @p{index + 11}, @p{index + 12}, @p{index + 13}, @p{index + 14}, @p{index + 15}, @p{index + 16})");

            parameters.Add(user.Id);
            parameters.Add(user.Email ?? "");
            parameters.Add(user.Email?.ToUpper() ?? "");
            parameters.Add(user.Email ?? "");
            parameters.Add(user.Email?.ToUpper() ?? "");
            parameters.Add(user.EmailConfirmed);
            parameters.Add(user.FirstName);
            parameters.Add(user.LastName);
            parameters.Add(user.PasswordHash ?? "");
            parameters.Add(user.SecurityStamp ?? "");
            parameters.Add(user.ConcurrencyStamp ?? "");
            parameters.Add(user.PhoneNumber ?? "");
            parameters.Add(user.PhoneNumberConfirmed);
            parameters.Add(user.TwoFactorEnabled);
            parameters.Add(user.LockoutEnd ?? new DateTimeOffset());
            parameters.Add(user.LockoutEnabled);
            parameters.Add(user.AccessFailedCount);

            index += 17;
        }

        sqlQuery += string.Join(", ", valuePlaceholders);
        
        await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters.ToArray());
    }

    public Task<TUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);
    public Task<IList<string>> GetRolesAsync(TUser user) => _userManager.GetRolesAsync(user);
    public Task<IdentityResult> AddToRoleAsync(TUser user, string role) => _userManager.AddToRoleAsync(user, role);
    public async Task<TUser?> FindByIdAsync(string id) => await _userManager.FindByIdAsync(id);
}