using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using MediatR;

namespace Healthcare.Application.Appointments.Queries;

public record GetDoctorAppointmentsQuery(string DoctorId, int Pagesize) : IRequest<List<AppointmentDTO>>;