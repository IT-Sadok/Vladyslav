using Application.Abstractions;
using Healthcare.Appointments.Commands.Submit;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Reject;

public class RejectAppointmentCommandHandler : IRequestHandler<RejectAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public RejectAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<bool> Handle(RejectAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            throw new Exception("Appointment not found");

        await _appointmentRepository.RejectAppointmentAsync(request.AppointmentId);
        return true;
    }
}