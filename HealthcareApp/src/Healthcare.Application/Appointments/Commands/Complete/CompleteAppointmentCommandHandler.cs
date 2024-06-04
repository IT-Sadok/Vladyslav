using Application.Abstractions;
using Healthcare.Appointments.Commands.Submit;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Complete;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<bool> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            throw new Exception("Appointment not found");

        await _appointmentRepository.CompleteAppointmentAsync(request.AppointmentId);
        return true;
    }
}