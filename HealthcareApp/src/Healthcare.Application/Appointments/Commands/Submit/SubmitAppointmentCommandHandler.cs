using Application.Abstractions;
using Healthcare.Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AppointmentStatuses;

namespace Healthcare.Appointments.Commands.Submit;

public class SubmitAppointmentCommandHandler : IRequestHandler<SubmitAppointmentCommand, Result>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public SubmitAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(SubmitAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            return Result.Failure("Appointment not found");

        await _appointmentRepository.ChangeStatusAsync(request.AppointmentId, Submitted);
        return Result.Success();
    }
}