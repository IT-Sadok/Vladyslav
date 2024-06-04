using MediatR;

namespace Healthcare.Application.Appointments.Commands.Reject;

public record RejectAppointmentCommand(int AppointmentId) : IRequest<bool>;