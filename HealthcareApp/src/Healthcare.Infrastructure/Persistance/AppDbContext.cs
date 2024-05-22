using Microsoft.EntityFrameworkCore;
using Healthcare.Domain.Entities;

namespace Healthcare.Infrastructure.Persistance;

public class AppDbContext : DbContext
{
    internal DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=localhost;Database=healthcare;uid=SA;pwd=VeryStr0ngP@ssw0rd;TrustServerCertificate=true");
    }
}
