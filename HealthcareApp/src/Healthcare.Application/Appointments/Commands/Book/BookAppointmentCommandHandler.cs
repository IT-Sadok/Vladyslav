using Application.Abstractions;
using Application.Abstractions.Decorators;
using AutoMapper;
using Domain.Entities;
using Healthcare.Application.Appointments.Commands.Book;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, Result>
{
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public BookAppointmentCommandHandler(IUserManagerDecorator<ApplicationUser> userManager,
        IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _userManager = userManager;
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _userManager.FindByIdAsync(request.DoctorId);
        if (doctor == null) return Result.Failure("Doctor not found");

        var patient = await _userManager.FindByIdAsync(request.PatientId);
        if (patient == null) return Result.Failure("Patient not found");

        var isAvailable =
            await _appointmentRepository.IsAvailableAsync(doctor.Id, request.AppointmentDate, request.StartTime);
        if (!isAvailable)
            return Result.Failure("Appointment slot already booked");

        var appointment = _mapper.Map<Appointment>(request);
        
        await _appointmentRepository.RequestAppointmentAsync(appointment);

        return Result.Success();
    }
}