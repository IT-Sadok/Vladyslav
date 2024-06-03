using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Application.Appointments.Queries;

public class
    GetRequestedAppointmentsQueryHandler : IRequestHandler<GetRequestedAppointmentsQuery, List<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserManagerDecorator<ApplicationUser> _userManager;


    public GetRequestedAppointmentsQueryHandler(IAppointmentRepository appointmentRepository,
        IUserManagerDecorator<ApplicationUser> userManager)
    {
        _appointmentRepository = appointmentRepository;
        _userManager = userManager;
    }

    public async Task<List<Appointment>> Handle(GetRequestedAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _userManager.GetAllUsersAsync();

        var doctor = await users.FirstOrDefaultAsync(x => x.Id == request.DoctorId, cancellationToken);
        if (doctor == null) throw new Exception("Doctor not found");


        return await _appointmentRepository.GetRequestedAppointments(request.DoctorId);
    }
}