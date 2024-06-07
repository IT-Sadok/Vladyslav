using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;

namespace Application.Abstractions;

public interface IScheduleRepository
{
    Task<List<Schedule>> GetDoctorSchedule(string doctorId, int pageSize);
    
    Task CreateDefaultWorkingScheduleAsync(string doctorId);

    Task<Dictionary<string, List<TimeSlotDTO>>> GetTimeSlots(ApplicationUser doctor, List<Schedule> schedules);
}