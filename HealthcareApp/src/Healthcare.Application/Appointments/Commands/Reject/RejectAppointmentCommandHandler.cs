using Application.Abstractions;
using Healthcare.Application.DTOs.Result;
using MediatR;
using static Domain.Constants.AppointmentStatuses;

namespace Healthcare.Application.Appointments.Commands.Reject;

public class RejectAppointmentCommandHandler : IRequestHandler<RejectAppointmentCommand, Result>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public RejectAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> Handle(RejectAppointmentCommand request, CancellationToken cancellationToken)
    {
        if (await _appointmentRepository.GetByIdAsync(request.AppointmentId) == null)
            return Result.Failure("Appointment not found");

        await _appointmentRepository.ChangeStatusAsync(request.AppointmentId, Rejected);
        return Result.Success();
    }
}