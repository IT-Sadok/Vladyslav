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
        List<object[]> userParameters = new List<object[]>();

        foreach (var user in allUsers)
        {
            var userParams = new object[]
            {
                user.Id,
                user.Email ?? "",
                (user.Email ?? "").ToUpper(),
                user.Email ?? "",
                (user.Email ?? "").ToUpper(),
                user.EmailConfirmed,
                user.FirstName,
                user.LastName,
                user.PasswordHash ?? "",
                user.SecurityStamp ?? "",
                user.ConcurrencyStamp ?? "",
                user.PhoneNumber ?? "",
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEnd ?? new DateTimeOffset(),
                user.LockoutEnabled,
                user.AccessFailedCount
            };

            userParameters.Add(userParams);
        }

        foreach (var userParams in userParameters)
        {
            string insertStatement = $@"
        INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, FirstName, LastName, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
        VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)";

            await _context.Database.ExecuteSqlRawAsync(insertStatement, userParams);
        }
    }

    public Task<TUser?> FindByEmailAsync(string email) => _userManager.FindByEmailAsync(email);
    public Task<IList<string>> GetRolesAsync(TUser user) => _userManager.GetRolesAsync(user);
    public Task<IdentityResult> AddToRoleAsync(TUser user, string role) => _userManager.AddToRoleAsync(user, role);
    public async Task<TUser?> FindByIdAsync(string id) => await _userManager.FindByIdAsync(id);
}