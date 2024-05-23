using Microsoft.EntityFrameworkCore;
using Healthcare.Domain.Entities;

namespace Healthcare.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    internal DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}
