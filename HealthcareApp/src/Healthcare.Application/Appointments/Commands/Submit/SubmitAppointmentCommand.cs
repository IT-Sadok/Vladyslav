using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Appointments.Commands.Submit;

public record SubmitAppointmentCommand(int AppointmentId) : IRequest<Result>;