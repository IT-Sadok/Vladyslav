using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Schedules.Queries.AvailableTimeSlots;

public class
    GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, Dictionary<string, List<TimeSlotDTO>>>
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IScheduleRepository _scheduleRepository;

    public GetAvailableSlotsQueryHandler(IUserManagerDecorator<ApplicationUser> userManager,
        IScheduleRepository scheduleRepository)
    {
        _userManager = userManager;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Dictionary<string, List<TimeSlotDTO>>> Handle(GetAvailableSlotsQuery request,
        CancellationToken cancellationToken)
    {
        var doctors = await _userManager.GetAllUsersAsync();
        var doctor = await doctors.Include(u => u.DoctorAppointments)
            .FirstOrDefaultAsync(u => u.Id == request.DoctorId, cancellationToken);

        if (doctor == null) throw new Exception("Doctor not found");

        var schedules = await _scheduleRepository.GetAllSchedules();
        var doctorSchedules = schedules.Where(s => s.DoctorId == request.DoctorId).ToList();

        if (doctorSchedules == null || !doctorSchedules.Any()) throw new Exception("Schedule not found");

        var weekSlots = new Dictionary<string, List<TimeSlotDTO>>();

        foreach (var day in Enum.GetValues<DayOfWeek>())
        {
            var schedule = doctorSchedules.FirstOrDefault(s => s.DayOfWeek == day.ToString());
            if (schedule == null) continue;

            var appointments = doctor.DoctorAppointments
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