using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Appointments.Queries;

public class
    GetDoctorAppointmentsQueryHandler : IRequestHandler<GetDoctorAppointmentsQuery, List<AppointmentDTO>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;


    public GetDoctorAppointmentsQueryHandler(IAppointmentRepository appointmentRepository,
        IUserManagerDecorator<ApplicationUser> userManager)
    {
        _appointmentRepository = appointmentRepository;
        _userManager = userManager;
    }

    public async Task<List<AppointmentDTO>> Handle(GetDoctorAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var doctor = await _userManager.FindByIdAsync(request.DoctorId);
        if (doctor == null) throw new Exception("Doctor not found");


        return await _appointmentRepository.GetDoctorAppointments(request.DoctorId, request.Pagesize);
    }
}