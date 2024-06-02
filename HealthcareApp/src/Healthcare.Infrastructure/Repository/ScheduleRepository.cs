using Application.Abstractions;
using Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _appDbContext;

    public ScheduleRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;
    
    public async Task<List<Schedule>> GetAllSchedules() => await _appDbContext.Schedules.ToListAsync();
    
    public async Task CreateDefaultWorkingScheduleAsync(string doctorId)
    {
        var workingHours = new List<Schedule>();
        var startTime = new TimeSpan(8, 0, 0); 
        var endTime = new TimeSpan(16, 0, 0);

        for (int i = 0; i < 5; i++) 
        {
            var date = DateTime.Today.AddDays(i - (int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            workingHours.Add(new Schedule
            {
                DoctorId = doctorId,
                DayOfWeek = date.DayOfWeek.ToString(),
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            });
        }

        _appDbContext.Schedules.AddRange(workingHours);
        await _appDbContext.SaveChangesAsync(); 
    }
}