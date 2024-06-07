using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Complete;

public record CompleteAppointmentCommand(int AppointmentId) : IRequest<Result>;