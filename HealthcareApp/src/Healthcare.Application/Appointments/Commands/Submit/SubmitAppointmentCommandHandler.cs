using Application.Abstractions;
using Healthcare.Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AppointmentStatuses;

namespace Healthcare.Appointments.Commands.Submit;

public class SubmitAppointmentCommandHandler : IRequestHandler<SubmitAppointmentCommand, Result<string>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public SubmitAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result<string>> Handle(SubmitAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            return Result<string>.Failure("Appointment not found");

        await _appointmentRepository.ChangeStatusAsync(request.AppointmentId, Submitted);
        return Result<string>.Success();
    }
}