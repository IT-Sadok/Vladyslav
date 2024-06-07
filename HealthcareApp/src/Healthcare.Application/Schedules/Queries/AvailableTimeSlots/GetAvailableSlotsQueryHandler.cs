using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Schedule;
using MediatR;

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
        var doctor = await _userManager.FindByIdAsync(request.DoctorId);
        if (doctor == null) throw new Exception("Doctor not found");

        var doctorSchedules = await _scheduleRepository.GetDoctorSchedule(request.DoctorId, request.PageSize);
        if (doctorSchedules == null || !doctorSchedules.Any()) throw new Exception("Schedule not found");

        return await _scheduleRepository.GetTimeSlots(doctor, doctorSchedules);
    }
}