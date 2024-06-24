using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Constants;
using Domain.Entities;
using Domain.Extensions;
using Healthcare.Infrastructure.Persistance;


namespace Infrastructure.Repository;

public class MigrationsRepository : IMigrationsRepository
{
    private readonly AppDbContext _context;
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IScheduleRepository _scheduleRepository;

    public MigrationsRepository(AppDbContext context, IUserManagerDecorator<ApplicationUser> userManager,
        IScheduleRepository scheduleRepository)
    {
        _context = context;
        _userManager = userManager;
        _scheduleRepository = scheduleRepository;
    }


    public async Task MigrateRangeAsync(List<ApplicationUser> patients, List<ApplicationUser> doctors,
        List<Appointment>? appointments = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Users.AddRangeAsync(patients);
            await _context.Users.AddRangeAsync(doctors);

            
            if (appointments != null)
            {
                const int batchSize = 100; // Adjust batch size as needed
                foreach (var batch in appointments.Batch(batchSize))
                {
                    await _context.Appointments.AddRangeAsync(batch);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();

            foreach (var patient in patients)
            {
                patient.UserName = patient.Email;
                await _userManager.AddToRoleAsync(patient, UserRolesConstants.Patient);
            }

            foreach (var doctor in doctors)
            {
                doctor.UserName = doctor.Email;
                await _userManager.AddToRoleAsync(doctor, UserRolesConstants.Doctor);

                var workingHours = DefaultWorkingHours.CreateWorkingHours(doctor.Id).ToList();
                await _scheduleRepository.CreateDefaultWorkingScheduleAsync(workingHours);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}