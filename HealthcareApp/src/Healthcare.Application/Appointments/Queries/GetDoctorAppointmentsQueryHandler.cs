using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using Healthcare.Application.DTOs.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Appointments.Queries;

public class
    GetDoctorAppointmentsQueryHandler : IRequestHandler<GetDoctorAppointmentsQuery, Result<List<AppointmentDTO>>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;


    public GetDoctorAppointmentsQueryHandler(IAppointmentRepository appointmentRepository,
        IUserManagerDecorator<ApplicationUser> userManager)
    {
        _appointmentRepository = appointmentRepository;
        _userManager = userManager;
    }

    public async Task<Result<List<AppointmentDTO>>> Handle(GetDoctorAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var doctor = await _userManager.FindByIdAsync(request.DoctorId);
        if (doctor == null) return Result<List<AppointmentDTO>>.Failure("Doctor not found");


        var appointments =  await _appointmentRepository.GetDoctorAppointments(request.DoctorId, request.Pagesize);

        return Result<List<AppointmentDTO>>.Success(appointments);
    }
}