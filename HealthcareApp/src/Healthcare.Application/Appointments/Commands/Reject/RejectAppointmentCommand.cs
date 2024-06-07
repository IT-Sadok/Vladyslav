using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Commands.Reject;

public record RejectAppointmentCommand(int AppointmentId) : IRequest<Result>;