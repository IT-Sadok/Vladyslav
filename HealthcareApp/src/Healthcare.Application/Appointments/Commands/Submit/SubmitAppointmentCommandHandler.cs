using Application.Abstractions;
using MediatR;

namespace Healthcare.Appointments.Commands.Submit;

public class SubmitAppointmentCommandHandler : IRequestHandler<SubmitAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public SubmitAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<bool> Handle(SubmitAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            throw new Exception("Appointment not found");

        await _appointmentRepository.SubmitAppointmentAsync(request.AppointmentId);
        return true;
    }
}