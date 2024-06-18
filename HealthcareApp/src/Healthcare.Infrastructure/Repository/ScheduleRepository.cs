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

    public async Task CreateDefaultWorkingScheduleAsync(List<Schedule> workingHours)
    {
        _appDbContext.Schedules.AddRange(workingHours);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<TimeSlotsDictionary> GetTimeSlots(ApplicationUser doctor, List<Schedule> schedules)
    {
        var weekSlots = new TimeSlotsDictionary();

        foreach (var day in Enum.GetValues<DayOfWeek>())
        {
            var schedule = schedules.FirstOrDefault(s => s.DayOfWeek == day);
            if (schedule == null) continue;

            var appointments = await _appDbContext.Appointments.FromSqlRaw(
                    @"SELECT * 
                FROM Appointments
                WHERE DoctorId = {0} AND DATEPART(dw, AppointmentDate) = {1}", doctor.Id, (int)day + 1)
                .ToListAsync();


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