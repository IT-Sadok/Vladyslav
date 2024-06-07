using Application.Abstractions;
using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;
using Healthcare.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _appDbContext;

    public ScheduleRepository(AppDbContext appDbContext) => _appDbContext = appDbContext;

    public async Task<List<Schedule>> GetDoctorSchedule(string doctorId, int pageSize)
        => await _appDbContext.Schedules.Where(s => s.DoctorId == doctorId).Take(pageSize).ToListAsync();

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
                DayOfWeek = date.DayOfWeek,
                Date = date,
                StartTime = startTime,
                EndTime = endTime
            });
        }

        _appDbContext.Schedules.AddRange(workingHours);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Dictionary<string, List<TimeSlotDTO>>> GetTimeSlots(ApplicationUser doctor, List<Schedule> schedules)
    {
        var weekSlots = new Dictionary<string, List<TimeSlotDTO>>();

        foreach (var day in Enum.GetValues<DayOfWeek>())
        {
            var schedule = schedules.FirstOrDefault(s => s.DayOfWeek == day);
            if (schedule == null) continue;

            var allAppointments = await _appDbContext.Appointments
                .Where(a => a.DoctorId == doctor.Id)
                .ToListAsync();
            
            var appointments = allAppointments
                .Where(a => a.AppointmentDate.DayOfWeek == day)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToList();

            var availableSlots = new List<TimeSlotDTO>();
            var startTime = schedule.StartTime;

            while (startTime < schedule.EndTime)
            {
                var endTime = startTime.Add(TimeSpan.FromMinutes(15));
                var isAvailable = !appointments.Any(a => (a.StartTime <= startTime && a.EndTime > startTime) ||
                                                         (a.StartTime < endTime && a.EndTime >= endTime) ||
                                                         (a.StartTime >= startTime && a.EndTime <= endTime));

                var slot = new TimeSlotDTO(startTime, endTime, isAvailable);
                availableSlots.Add(slot);
                startTime = startTime.Add(TimeSpan.FromMinutes(30));
            }

            weekSlots[day.ToString()] = availableSlots;
        }

        return weekSlots;
    }
}