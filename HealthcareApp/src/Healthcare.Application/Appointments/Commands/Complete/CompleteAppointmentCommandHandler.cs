using Application.Abstractions;
using Healthcare.Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AppointmentStatuses;

namespace Healthcare.Application.Appointments.Commands.Complete;

public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand, Result>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public CompleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            return Result.Failure("Appointment not found");

        await _appointmentRepository.ChangeStatusAsync(request.AppointmentId, Completed);
        return Result.Success();
    }
}