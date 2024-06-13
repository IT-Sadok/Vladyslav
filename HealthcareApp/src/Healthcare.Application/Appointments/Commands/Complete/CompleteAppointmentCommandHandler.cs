using Application.Abstractions;
using Healthcare.Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AppointmentStatuses;

namespace Healthcare.Application.Appointments.Commands.Complete;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, Result<string>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result<string>> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            return Result<string>.Failure("Appointment not found");

        await _appointmentRepository.ChangeStatusAsync(request.AppointmentId, Completed);
        return Result<string>.Success();
    }
}