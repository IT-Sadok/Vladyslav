using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;

namespace Application.Abstractions;

public interface IScheduleRepository
{
    Task<List<Schedule>> GetDoctorSchedule(string doctorId, int pageSize);
    
    Task CreateDefaultWorkingScheduleAsync(List<Schedule> workingHours);

    Task<Dictionary<string, List<TimeSlotDTO>>> GetTimeSlots(ApplicationUser doctor, List<Schedule> schedules);
}