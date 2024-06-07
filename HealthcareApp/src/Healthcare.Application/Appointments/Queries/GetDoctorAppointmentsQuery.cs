using Domain.Entities;
using Healthcare.Application.DTOs.Appointment;
using Healthcare.Application.DTOs.Result;
using MediatR;

namespace Healthcare.Application.Appointments.Queries;

public record GetDoctorAppointmentsQuery(string DoctorId, int Pagesize) : IRequest<Result<List<AppointmentDTO>>>;