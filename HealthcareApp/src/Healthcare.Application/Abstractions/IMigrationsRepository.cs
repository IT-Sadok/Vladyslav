using Domain.Entities;

namespace Application.Abstractions;

public interface IMigrationsRepository
{
    Task MigrateRangeAsync(List<ApplicationUser> patients, List<ApplicationUser> doctors,
        List<Appointment>? appointments = default);
}