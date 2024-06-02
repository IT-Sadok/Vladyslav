using Domain.Entities;

namespace Application.Abstractions;

public interface IScheduleRepository
{
    Task<List<Schedule>> GetAllSchedules();
    
    Task CreateDefaultWorkingScheduleAsync(string doctorId);
}