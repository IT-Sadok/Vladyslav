using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Result;
using Healthcare.Application.DTOs.Schedule;
using MediatR;

namespace Healthcare.Application.Schedules.Queries.AvailableTimeSlots;

public class
    GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, Result<TimeSlotsDictionary>>
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IScheduleRepository _scheduleRepository;


    public GetAvailableSlotsQueryHandler(IUserManagerDecorator<ApplicationUser> userManager,
        IScheduleRepository scheduleRepository)
    {
        _userManager = userManager;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Result<TimeSlotsDictionary>> Handle(GetAvailableSlotsQuery request,
        CancellationToken cancellationToken)
    {
        var doctor = await _userManager.FindByIdAsync(request.DoctorId);
        if (doctor == null) return Result<TimeSlotsDictionary>.Failure("Doctor not found");

        var doctorSchedules = await _scheduleRepository.GetDoctorSchedule(request.DoctorId, request.PageSize);

        if (!doctorSchedules.Any())
            return Result<TimeSlotsDictionary>.Failure("Schedule not found");

        var timeSlots = await _scheduleRepository.GetTimeSlots(doctor, doctorSchedules);
        
        return Result<TimeSlotsDictionary>.Success(timeSlots);
    }
}