using Domain.Entities;
using MediatR;

namespace Healthcare.Application.Appointments.Queries;

public record GetRequestedAppointmentsQuery(string DoctorId) : IRequest<List<Appointment>>;