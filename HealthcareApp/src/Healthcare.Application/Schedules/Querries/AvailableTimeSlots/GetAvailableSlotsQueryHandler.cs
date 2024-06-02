using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Schedules.Querries.AvailableTimeSlots;

public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<TimeSlotDTO>>
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IScheduleRepository _scheduleRepository;



    public GetAvailableSlotsQueryHandler(IUserManagerDecorator<ApplicationUser> userManager,
        IScheduleRepository scheduleRepository)
    {
        _userManager = userManager;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<List<TimeSlotDTO>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        var doctors = await _userManager.GetAllUsersAsync();
        var doctor = await doctors.Include(u => u.DoctorAppointments)
            .FirstOrDefaultAsync(u => u.Id == request.DoctorId, cancellationToken);

        if (doctor == null) throw new Exception("Doctor not found");

        var schedules = await _scheduleRepository.GetAllSchedules();
        var schedule = schedules.FirstOrDefault<Schedule>(
            s => s.DoctorId == request.DoctorId && s.DayOfWeek == request.Date.DayOfWeek.ToString());

        if (schedule == null) throw new Exception("Schedule not found");

        var appointments = doctor.DoctorAppointments.Where(a => a.AppointmentDate.Date == request.Date.Date)
            .Select(a => a.StartTime)
            .ToList();

        var availableSlots = new List<TimeSlotDTO>();
        var startTime = schedule.StartTime;
        while (startTime < schedule.EndTime)
        {
            var slot = new TimeSlotDTO(startTime, startTime.Add(TimeSpan.FromMinutes(15)),
                !appointments.Contains(startTime));
            availableSlots.Add(slot);
            startTime = startTime.Add(TimeSpan.FromMinutes(30));
        }

        return availableSlots;
    }
}